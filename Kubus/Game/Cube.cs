using GameDevCommon.Rendering.Composers;
using GameDevCommon.Rendering.Texture;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Core;

namespace Kubus.Game
{
    class Cube : GameObject
    {
        private const int DROP_HEIGHT = 12;

        private static Random _random = new Random();
        private static Dictionary<int, Texture2D> _colors = new Dictionary<int, Texture2D>();

        private Level _level;

        public bool Landed { get; set; }
        public int CubeHeight { get; set; } = -1;
        public bool IsFormationLeader { get; private set; }
        public bool IsInFormation { get; set; }
        public MainCubeFace Face { get; set; }
        public Matrix Orientation { get; set; } = Matrix.Identity;

        public Cube(Level level, Point position, bool isFormationLeader = false)
        {
            _level = level;

            IsInFormation = true;
            IsFormationLeader = isFormationLeader;

            Position = new Vector3(-1.5f + position.X, DROP_HEIGHT, -1.5f + position.Y);

        }

        public override void LoadContent()
        {
            SetColor();

            base.LoadContent();
        }

        protected override void CreateGeometry()
        {
            //var texture = new TextureCuboidWrapper();
            //var bounds = new Rectangle(0, 0, 2, 1);
            //texture.AddSide(CuboidSide.Top, new TextureRectangle(new Rectangle(0, 0, 1, 1), bounds));
            //texture.AddSide(new[] { CuboidSide.Bottom, CuboidSide.Left, CuboidSide.Right, CuboidSide.Front, CuboidSide.Back },
            //    new TextureRectangle(new Rectangle(0, 0, 1, 1), bounds));
            Geometry.AddVertices(CuboidComposer.Create(1f));
        }

        protected override void CreateWorld()
        {
            if (Landed)
            {
                World = Matrix.CreateTranslation(Position) *
                    Orientation *
                    _level.MainCube.Orientation *
                    Matrix.CreateTranslation(CameraOffset);
            }
            else
            {
                World = Matrix.CreateTranslation(Position + CameraOffset);
            }
        }

        public Point CubePosition
        {
            get
            {
                Vector3 pos3;
                if (Landed)
                {
                    pos3 = Vector3.Transform(Position, Orientation * _level.MainCube.Orientation);
                }
                else
                {
                    pos3 = Position;
                }
                var p = new Point((int)Math.Round(pos3.X * 10), (int)Math.Round(pos3.Z * 10));
                return p;
            }
        }

        public override void Update()
        {
            base.Update();
            if (!Landed && IsFormationLeader)
            {
                var cubes = _level.GetFormation();
                foreach (var cube in cubes)
                {
                    cube.Position.Y -= 0.04f + _level.Speed * 0.001f + _level.Points * 0.0001f;
                    cube.CreateWorld();
                    cube.SetColor();
                }

                // check for landing on main cube
                if (Position.Y <= 2.5f)
                {
                    foreach (var cube in cubes)
                    {
                        cube.Position.Y = 2.5f;
                        cube.Landed = true;
                        cube.IsInFormation = false;
                        cube.IsFormationLeader = false;
                        cube.Orientation = Matrix.Invert(_level.MainCube.TargetOrientation);
                        cube.CubeHeight = 0;
                        cube.Face = _level.MainCube.UpFace;
                        CreateWorld();
                    }

                    _level.CheckCompletedSlice(0);
                    _level.SpawnNewFormation();
                }
                else
                {
                    var faceCubes = _level.GetFaceCubes(_level.MainCube.UpFace);
                    if (faceCubes.Length > 0)
                    {
                        var landed = false;
                        Cube supportCube = null;
                        foreach (var cube in cubes)
                        {
                            foreach (var faceCube in faceCubes)
                            {
                                if (cube.CubePosition == faceCube.CubePosition)
                                {
                                    if (faceCube.Position.Y + 1f >= cube.Position.Y)
                                    {
                                        // landed!
                                        landed = true;
                                        supportCube = faceCube;
                                        break;
                                    }
                                }
                            }
                            if (landed)
                            {
                                break;
                            }
                        }
                        if (landed)
                        {
                            foreach (var cube in cubes)
                            {
                                cube.Position.Y = supportCube.Position.Y + 1f;
                                cube.Landed = true;
                                cube.IsInFormation = false;
                                cube.IsFormationLeader = false;
                                cube.Orientation = Matrix.Invert(_level.MainCube.TargetOrientation);
                                cube.CubeHeight = supportCube.CubeHeight + 1;
                                cube.Face = _level.MainCube.UpFace;
                                CreateWorld();
                            }

                            if (supportCube.CubeHeight == 7)
                            {
                                foreach (var cube in cubes)
                                {
                                    cube.SetGameOverColor();
                                }
                                _level.IsGameOver = true;
                            }
                            else
                            {
                                _level.CheckCompletedSlice(supportCube.CubeHeight + 1);
                                _level.SpawnNewFormation();
                            }
                        }
                    }
                }
            }
            else
            {
                CreateWorld();
            }
        }

        private void SetColor()
        {
            var h = (int)((Position.Y - 2.5f) / (DROP_HEIGHT - 2.5f) * 100);
            if (!_colors.TryGetValue(h, out var texture))
            {
                texture = new Texture2D(Controller.GraphicsDevice, 1, 1);
                var mainColor = new Color(
                    (int)(255 * h * 0.01f),
                    189 - (int)(100 * h * 0.01f),
                    200 - (int)(100 * h * 0.01f));
                texture.SetData(new[] { mainColor });
                _colors.Add(h, texture);
            }
            Texture = texture;
        }

        private void SetGameOverColor()
        {
            Texture = new Texture2D(Controller.GraphicsDevice, 1, 1);
            Texture.SetData(new[] { Color.Red });
        }
    }
}
