using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameDevCommon.Drawing;
using GameDevCommon.Input;
using GameDevCommon.Rendering;
using Kubus.Game;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using static Core;

namespace Kubus.Screens.InGame
{
    class InGameScreen : Screen
    {
        private ICamera _camera;
        private BasicShader _shader;

        private SpriteBatch _batch;

        private Level _level;

        internal override void LoadContent()
        {
            _camera = new InGameCamera();
            _shader = new BasicShader();
            _batch = new SpriteBatch(Controller.GraphicsDevice);

            _level = new Level();
            _level.LoadContent();

            _level.SpawnNewFormation();
        }

        internal override void UnloadContent()
        {

        }

        internal override void Draw(GameTime gameTime)
        {
            Controller.GraphicsDevice.ClearFull(Color.Black);

            _batch.Begin();
            _batch.DrawGradient(Controller.ClientRectangle, new Color(222, 132, 169), new Color(80, 55, 111), false);
            _batch.End();

            Controller.GraphicsDevice.ResetFull();

            Controller.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            _shader.Prepare(_camera);
            _shader.Render(_level.MainCube);
            foreach (var cube in _level.Cubes)
            {
                _shader.Render(cube);
            }
            foreach (var projection in _level.Projections)
            {
                _shader.Render(projection);
            }

            _batch.Begin(blendState: BlendState.NonPremultiplied);
            var y = 0;
            var x = 0;
            for (int i = 0; i < _level.Points; i++)
            {
                if (x > Controller.ClientRectangle.Width)
                {
                    x = 0;
                    y += 6;
                }
                _batch.DrawRectangle(new Rectangle(x + 1, y + 1, 4, 4), Color.White);
                x += 6;
            }

            if (_level.IsGameOver)
            {
                DrawGameOver();
            }

            _batch.End();
        }

        internal override void Update(GameTime gameTime)
        {
            _level.Update();
        }

        private void DrawGameOver()
        {
            var points = GameOverLetters.GetLetters();

            var startP = new Point(Controller.ClientRectangle.Width / 2 - 80, Controller.ClientRectangle.Height / 2 - 40);

            _batch.DrawRectangle(new Rectangle(startP.X - 10, startP.Y - 10, 180, 110), new Color(255, 255, 255, 180));

            foreach (var p in points)
            {
                var h = (10 - p.Y) / 10f;
                var mainColor = new Color(
                    (int)(255 * h),
                    189 - (int)(100 * h),
                    200 - (int)(100 * h));

                _batch.DrawRectangle(new Rectangle(startP.X + p.X * 8, startP.Y + p.Y * 8, 8, 8), mainColor);
            }
        }
    }
}
