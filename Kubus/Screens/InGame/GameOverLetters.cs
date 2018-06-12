using Microsoft.Xna.Framework;

namespace Kubus.Screens.InGame
{
    static class GameOverLetters
    {
        public static Point[] GetLetters()
        {
            return new[] {
                // g
                new Point(0, 0),
                new Point(1, 0),
                new Point(2, 0),
                new Point(3, 0),

                new Point(0, 1),
                new Point(0, 2),
                new Point(0, 3),
                new Point(0, 4),

                new Point(1, 4),
                new Point(2, 4),
                new Point(3, 4),

                new Point(3, 3),
                new Point(3, 2),
                new Point(2, 2),

                // a
                new Point(5, 0),
                new Point(6, 0),
                new Point(7, 0),
                new Point(8, 0),

                new Point(5, 1),
                new Point(5, 2),
                new Point(5, 3),
                new Point(5, 4),
                new Point(8, 1),
                new Point(8, 2),
                new Point(8, 3),
                new Point(8, 4),

                new Point(6, 2),
                new Point(7, 2),
                
                // m
                new Point(10, 0),
                new Point(12, 0),
                new Point(14, 0),

                new Point(10, 1),
                new Point(10, 2),
                new Point(10, 3),
                new Point(10, 4),

                new Point(12, 1),
                new Point(12, 2),
                new Point(12, 3),
                new Point(12, 4),

                new Point(14, 1),
                new Point(14, 2),
                new Point(14, 3),
                new Point(14, 4),

                // e
                new Point(16, 0),
                new Point(17, 0),
                new Point(18, 0),
                new Point(19, 0),
                new Point(16, 1),

                new Point(16, 2),
                new Point(17, 2),
                new Point(18, 2),
                new Point(19, 2),
                new Point(16, 3),

                new Point(16, 4),
                new Point(17, 4),
                new Point(18, 4),
                new Point(19, 4),

                // o
                new Point(0, 6),
                new Point(1, 6),
                new Point(2, 6),
                new Point(3, 6),

                new Point(0, 7),
                new Point(0, 8),
                new Point(0, 9),
                new Point(0, 10),
                new Point(3, 7),
                new Point(3, 8),
                new Point(3, 9),
                new Point(3, 10),

                new Point(1, 10),
                new Point(2, 10),

                // v
                new Point(5, 6),
                new Point(8, 6),

                new Point(5, 7),
                new Point(5, 8),
                new Point(5, 9),
                new Point(8, 7),
                new Point(8, 8),
                new Point(8, 9),

                new Point(6, 10),
                new Point(7, 10),

                // e
                new Point(10, 6),
                new Point(11, 6),
                new Point(12, 6),
                new Point(13, 6),
                new Point(14, 6),

                new Point(10, 8),
                new Point(11, 8),
                new Point(12, 8),
                new Point(13, 8),
                new Point(14, 8),

                new Point(10, 10),
                new Point(11, 10),
                new Point(12, 10),
                new Point(13, 10),
                new Point(14, 10),

                // r
                new Point(16, 6),
                new Point(17, 6),
                new Point(18, 6),
                new Point(19, 6),

                new Point(16, 7),
                new Point(19, 7),
                new Point(16, 9),
                new Point(16, 10),

                new Point(16, 8),
                new Point(17, 8),
                new Point(18, 8),
                new Point(19, 8),

                new Point(18, 9),
                new Point(19, 10),
            };
        }
    }
}
