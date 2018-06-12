using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Kubus.Game
{
    static class CubeColorHelper
    {
        private const int COLORS = 16;

        public static int ColorIndex = 0;

        private static byte[] CUBE_COLOR_RED;
        private static byte[] CUBE_COLOR_GREEN;
        private static byte[] CUBE_COLOR_BLUE;

        private static void InitializeColors()
        {

            // initialze color ranges
            // represent a rainbow like this:
            // red, yellow, green, blue, purple, red
            // each color contains 256 * 6 bytes representing their color value at a time

            var up = Enumerable.Range(0, 256).Select(i => (byte)i).ToArray();
            var down = up.Reverse().ToArray();
            var upper = Enumerable.Repeat(255, 256).Select(i => (byte)i).ToArray();
            var lower = Enumerable.Repeat(0, 256).Select(i => (byte)i).ToArray();

            var red = new List<byte>();
            red.AddRange(upper);
            red.AddRange(down);
            red.AddRange(lower);
            red.AddRange(lower);
            red.AddRange(up);
            red.AddRange(upper);

            var green = new List<byte>();
            green.AddRange(up);
            green.AddRange(upper);
            green.AddRange(upper);
            green.AddRange(down);
            green.AddRange(lower);
            green.AddRange(lower);

            var blue = new List<byte>();
            blue.AddRange(down);
            blue.AddRange(down);
            blue.AddRange(up);
            blue.AddRange(upper);
            blue.AddRange(upper);
            blue.AddRange(down);

            CUBE_COLOR_RED = red.ToArray();
            CUBE_COLOR_GREEN = green.ToArray();
            CUBE_COLOR_BLUE = blue.ToArray();
        }

        public static Color GetColor()
        {
            if (CUBE_COLOR_RED == null)
            {
                InitializeColors();
            }

            while (ColorIndex >= COLORS)
            {
                ColorIndex -= COLORS;
            }

            var index = (int)Math.Floor((ColorIndex / (double)(COLORS - 1)) * (CUBE_COLOR_RED.Length - 1));
            return new Color(CUBE_COLOR_RED[index], CUBE_COLOR_GREEN[index], CUBE_COLOR_BLUE[index]);
        }
    }
}
