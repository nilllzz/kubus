using Microsoft.Xna.Framework;

namespace Kubus.Screens
{
    abstract class Screen
    {
        internal abstract void LoadContent();
        internal abstract void UnloadContent();
        internal abstract void Update(GameTime gameTime);
        internal abstract void Draw(GameTime gameTime);
    }
}
