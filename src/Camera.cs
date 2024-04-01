using System;
using Microsoft.Xna.Framework;

namespace Szczury
{
    public static class Camera
    {
        public static Vector2 cameraPosition;
        public static void CenterCameraOn(Vector2 position)
        {
            cameraPosition.X = position.X - Util.screenWidth / 2;
            cameraPosition.Y = position.Y - Util.screenHeight / 2;
        }

        /// <summary>
        /// Get point relatively to the screen
        /// </summary>
        public static Point OnScreen(Point point)
        {
            return new Point(point.X - (int)MathF.Ceiling(cameraPosition.X), point.Y - (int)MathF.Ceiling(cameraPosition.Y));
        }

        /// <summary>
        /// Get vector relatively to the screen
        /// </summary>
        public static Vector2 OnScreen(Vector2 vec)
        {
            return new Vector2(vec.X - (int)MathF.Ceiling(cameraPosition.X), vec.Y - (int)MathF.Ceiling(cameraPosition.Y));
        }
    }
}
