using GameDevCommon.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using static Core;

namespace Kubus.Game
{
    class Level
    {
        private static Random _random = new Random();

        public MainCube MainCube { get; private set; }
        public List<Cube> Cubes { get; } = new List<Cube>();

        public List<CubeProjection> Projections { get; } = new List<CubeProjection>();
        public int Points { get; set; } = 0;
        public int Speed { get; set; } = 0;
        public bool IsGameOver = false;

        public void LoadContent()
        {
            MainCube = new MainCube();
            MainCube.LoadContent();
        }

        public Cube[] GetFormation()
        {
            return Cubes.Where(c => c.IsInFormation && !c.Landed).ToArray();
        }

        public Cube[] GetFaceCubes(MainCubeFace face)
        {
            return Cubes.Where(c => c.Landed && !c.IsInFormation && c.Face == face).ToArray();
        }

        public void AddCube(Point position, bool isFormationLeader)
        {
            var cube = new Cube(this, position, isFormationLeader);
            cube.LoadContent();
            Cubes.Add(cube);

            var projection = new CubeProjection(this, cube);
            projection.LoadContent();
            Projections.Add(projection);
        }

        public void Update()
        {
            if (GetComponent<KeyboardHandler>().KeyPressed(Keys.Right))
            {
                MainCube.TurnRight();
            }
            else if (GetComponent<KeyboardHandler>().KeyPressed(Keys.Left))
            {
                MainCube.TurnLeft();
            }
            else if (GetComponent<KeyboardHandler>().KeyPressed(Keys.Up))
            {
                MainCube.FlipUp();
            }
            else if (GetComponent<KeyboardHandler>().KeyPressed(Keys.Down))
            {
                MainCube.FlipDown();
            }

            if (GetComponent<KeyboardHandler>().KeyPressed(Keys.W))
            {
                MoveFormation(new Vector3(0, 0, -1));
            }
            if (GetComponent<KeyboardHandler>().KeyPressed(Keys.A))
            {
                MoveFormation(new Vector3(-1, 0, 0));
            }
            if (GetComponent<KeyboardHandler>().KeyPressed(Keys.S))
            {
                MoveFormation(new Vector3(0, 0, 1));
            }
            if (GetComponent<KeyboardHandler>().KeyPressed(Keys.D))
            {
                MoveFormation(new Vector3(1, 0, 0));
            }

            MainCube.Update();

            for (int i = 0; i < Cubes.Count; i++)
            {
                var cube = Cubes[i];
                cube.Update();
                if (cube.CanBeRemoved)
                {
                    Cubes.Remove(cube);
                    i--;
                }
            }
            for (int i = 0; i < Projections.Count; i++)
            {
                var proj = Projections[i];
                proj.Update();
                if (proj.CanBeRemoved)
                {
                    Projections.Remove(proj);
                    i--;
                }
            }
        }

        public void SpawnNewFormation()
        {
            Points++;
            Speed++;

            Point[] formation = null;
            void rotateFormation(int direction)
            {
                var rot = Matrix.CreateRotationY(MathHelper.PiOver2 * direction);
                formation = formation
                    .Select(f => Vector3.Transform(new Vector3(f.X - 1.5f, 0, f.Y - 1.5f), rot))
                    .Select(v => new Point((int)Math.Round(v.X + 1.5f), (int)Math.Round(v.Z + 1.5f)))
                    .ToArray();
            }

            void moveFormation(Point offset)
            {
                formation = formation.Select(p => p + offset).ToArray();
            }

            switch (_random.Next(0, 5))
            {
                case 0: // long piece

                    formation = new Point[]
                    {
                        new Point(0, 0),
                        new Point(1, 0),
                        new Point(2, 0),
                    };
                    if (_random.Next(0, 1) == 0)
                    {
                        rotateFormation(-1);
                        moveFormation(new Point(-_random.Next(0, 4), _random.Next(0, 2)));
                    }
                    else
                    {
                        moveFormation(new Point(_random.Next(0, 2), _random.Next(0, 4)));
                    }

                    break;
                case 1: // square 
                    formation = new Point[]
                    {
                        new Point(1, 1),
                        new Point(2, 1),
                        new Point(1, 2),
                        new Point(2, 2),
                    };
                    moveFormation(new Point(_random.Next(-1, 2), _random.Next(-1, 2)));
                    break;
                case 2: // corner
                    formation = new Point[]
                    {
                        new Point(1, 1),
                        new Point(2, 1),
                        new Point(1, 2),
                    };
                    rotateFormation(_random.Next(0, 3));
                    moveFormation(new Point(_random.Next(-1, 2), _random.Next(-1, 2)));
                    break;
                case 3: // dot
                    formation = new Point[]
                    {
                        new Point(1, 1)
                    };
                    moveFormation(new Point(_random.Next(-1, 3), _random.Next(-1, 3)));
                    break;
                case 4: // short piece
                    formation = new Point[]
                    {
                        new Point(0, 0),
                        new Point(1, 0),
                    };
                    if (_random.Next(0, 1) == 0)
                    {
                        rotateFormation(-1);
                        moveFormation(new Point(-_random.Next(0, 4), _random.Next(0, 3)));
                    }
                    else
                    {
                        moveFormation(new Point(_random.Next(0, 3), _random.Next(0, 4)));
                    }

                    break;
            }

            for (int i = 0; i < formation.Length; i++)
            {
                AddCube(formation[i], i == 0);
            }
        }

        private bool MoveFormation(Vector3 movement)
        {
            var formation = GetFormation();
            var faceCubes = GetFaceCubes(MainCube.UpFace);
            foreach (var cube in formation)
            {
                var orgPos = cube.Position;

                // first, check if any of the cubes would be outside the play area
                cube.Position += movement;
                if (cube.Position.X < -1.5f ||
                    cube.Position.X > 1.5f ||
                    cube.Position.Z < -1.5f ||
                    cube.Position.Z > 1.5f)
                {
                    cube.Position = orgPos;
                    return false;
                }

                // then, check against collision with another cube
                foreach (var faceCube in faceCubes)
                {
                    if (Math.Abs(faceCube.Position.Y - cube.Position.Y) < 1f &&
                        faceCube.CubePosition == cube.CubePosition)
                    {
                        cube.Position = orgPos;
                        return false;
                    }
                }

                cube.Position = orgPos;
            }

            foreach (var cube in formation)
            {
                cube.Position += movement;
            }

            return true;
        }

        public void CheckCompletedSlice(int cubeHeight)
        {
            var faceCubes = GetFaceCubes(MainCube.UpFace);
            var cubes = faceCubes.Where(c => c.CubeHeight == cubeHeight).ToArray();
            if (cubes.Length == 16)
            {
                // remove all cubes on that slice
                foreach (var cube in cubes)
                {
                    cube.CanBeRemoved = true;
                }
                // move all cubes above this slice on block down
                var moveDown = faceCubes.Where(c => c.CubeHeight > cubeHeight);
                foreach (var cube in moveDown)
                {
                    cube.CubeHeight--;
                    cube.Position.Y--;
                }
                // slow down 10
                Speed -= 10;
                if (Speed < 0)
                {
                    Speed = 0;
                }
            }
        }
    }
}
