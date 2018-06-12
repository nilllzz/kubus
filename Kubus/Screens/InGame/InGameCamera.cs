using GameDevCommon.Rendering;
using Microsoft.Xna.Framework;

namespace Kubus.Screens.InGame
{
    class InGameCamera : ICamera
    {
        public Matrix View { get; set; }
        public Matrix Projection { get; set; }

        public InGameCamera()
        {
            View = Matrix.CreateLookAt(new Vector3(5), Vector3.Zero, Vector3.Up);
            Projection = Matrix.CreateOrthographic(GameController.RENDER_WIDTH * 0.03f, GameController.RENDER_HEIGHT * 0.03f, 0.1f, 1000f);
        }
    }
}
