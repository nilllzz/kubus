using GameDevCommon.Rendering.Composers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using static Core;

namespace Kubus.Game
{
    class MainCube : GameObject
    {
        public MainCubeFace UpFace { get; private set; } = MainCubeFace.Up;
        public Matrix Orientation { get; private set; } = Matrix.Identity;
        public Matrix TargetOrientation { get; private set; } = Matrix.Identity;

        public MainCube()
        {
            Position = new Vector3(0, 0, 0);
        }

        protected override void CreateWorld()
        {
            World = Orientation *
                Matrix.CreateTranslation(Position + CameraOffset);
        }

        public override void LoadContent()
        {
            Texture = new Texture2D(Controller.GraphicsDevice, 1, 1);
            Texture.SetData(new[] { new Color(120, 120, 120) });

            base.LoadContent();
        }

        public override void Update()
        {
            base.Update();

            if (TargetOrientation != Orientation)
            {
                Orientation = Matrix.Lerp(Orientation, TargetOrientation, 0.3f);
            }

            CreateWorld();
        }

        protected override void CreateGeometry()
        {
            Geometry.AddVertices(CuboidComposer.Create(4f));
        }

        private void DoTransform(Vector3 axis, float angle)
        {
            var ori = Quaternion.CreateFromRotationMatrix(TargetOrientation);
            var rot = Quaternion.CreateFromAxisAngle(axis, angle);

            var final = rot * ori;
            final.Normalize();

            TargetOrientation = Matrix.CreateFromQuaternion(final);

            SetFace();
        }

        private void SetFace()
        {
            var up = Vector3.Transform(Vector3.Up, TargetOrientation);
            var right = Vector3.Transform(Vector3.Right, TargetOrientation);
            var forward = Vector3.Transform(Vector3.Forward, TargetOrientation);

            if (Math.Round(up.Y) == 1)
            {
                UpFace = MainCubeFace.Up;
            }
            else if (Math.Round(up.Y) == -1)
            {
                UpFace = MainCubeFace.Down;
            }
            else if (Math.Round(forward.Y) == 1)
            {
                UpFace = MainCubeFace.Back;
            }
            else if (Math.Round(forward.Y) == -1)
            {
                UpFace = MainCubeFace.Front;
            }
            else if (Math.Round(right.Y) == 1)
            {
                UpFace = MainCubeFace.Right;
            }
            else if (Math.Round(right.Y) == -1)
            {
                UpFace = MainCubeFace.Left;
            }
        }

        public void TurnRight()
        {
            DoTransform(Vector3.Up, MathHelper.PiOver2);
        }

        public void TurnLeft()
        {
            DoTransform(Vector3.Up, -MathHelper.PiOver2);
        }

        public void FlipUp()
        {
            DoTransform(Vector3.Right, -MathHelper.PiOver2);
        }

        public void FlipDown()
        {
            DoTransform(Vector3.Right, MathHelper.PiOver2);
        }
    }
}
