using GameDevCommon.Rendering.Composers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static Core;

namespace Kubus.Game
{
    class CubeProjection : GameObject
    {
        private readonly Level _level;
        private readonly Cube _parent;

        public CubeProjection(Level level, Cube parent)
        {
            _level = level;
            _parent = parent;

            Position = new Vector3(parent.Position.X, 2.001f, parent.Position.Z);
        }

        protected override void CreateWorld()
        {
            World = Matrix.CreateTranslation(Position + CameraOffset);
        }

        public override void LoadContent()
        {
            Texture = new Texture2D(Controller.GraphicsDevice, 1, 1);
            Texture.SetData(new[] { new Color(50, 50, 50) });

            base.LoadContent();
        }

        protected override void CreateGeometry()
        {
            Geometry.AddVertices(RectangleComposer.Create(1f, 1f));
        }

        public override void Update()
        {
            // default to base
            Position = new Vector3(_parent.Position.X, 2.001f, _parent.Position.Z);
            var faceCubes = _level.GetFaceCubes(_level.MainCube.UpFace);
            foreach (var faceCube in faceCubes)
            {
                if (faceCube.CubePosition == _parent.CubePosition)
                {
                    Position.Y = faceCube.Position.Y + 0.501f;
                }
            }
            CreateWorld();
            if (_parent.Landed)
            {
                CanBeRemoved = true;
            }
        }
    }
}
