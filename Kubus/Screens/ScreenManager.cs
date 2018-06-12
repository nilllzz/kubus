using Microsoft.Xna.Framework;

namespace Kubus.Screens
{
    class ScreenManager : IGameComponent
    {
        internal Screen ActiveScreen { get; private set; }

        public void Initialize()
        {
            var screen = new InGame.InGameScreen();
            SetScreen(screen);
        }

        public void SetScreen(Screen screen)
        {
            ActiveScreen?.UnloadContent();
            ActiveScreen = screen;
        }
    }
}
