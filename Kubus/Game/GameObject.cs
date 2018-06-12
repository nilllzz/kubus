using GameDevCommon.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kubus.Game
{
    abstract class GameObject : Base3DObject<VertexPositionNormalTexture>
    {
        public Vector3 Position;
        public Vector3 Rotation;
        public bool CanBeRemoved { get; set; }

        protected static Vector3 CameraOffset => new Vector3(0, -4, 0);

        protected static Matrix CreateRotationMatrix(Vector3 rotation)
        {
            return Matrix.CreateRotationX(rotation.X) *
                Matrix.CreateRotationY(rotation.Y) *
                Matrix.CreateRotationZ(rotation.Z);
        }

        protected Vector3 GetFaceRotation(MainCubeFace face)
        {
            switch (face)
            {
                case MainCubeFace.Up:
                    return Vector3.Zero;
                case MainCubeFace.Down:
                    return new Vector3(MathHelper.Pi, 0, 0);
                case MainCubeFace.Left:
                    return new Vector3(0, 0, MathHelper.PiOver2);
                case MainCubeFace.Right:
                    return new Vector3(0, 0, -MathHelper.PiOver2);
                case MainCubeFace.Front:
                    return new Vector3(-MathHelper.PiOver2, 0, 0);
                case MainCubeFace.Back:
                    return new Vector3(MathHelper.PiOver2, 0, 0);
            }
            return Vector3.Zero;
        }
    }
}
