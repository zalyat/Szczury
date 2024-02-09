using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Szczury
{
    public static class Util
    {
        public const int screenWidth = 1280;
        public const int screenHeight = 720;

        public static GameTime gameTime;
        public static float deltaTime
        {
            get => (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public const int tileSize = 16; //do not change (yet)
        public const ushort chunkSize = 32;

        public static Vector2 TilePosToWorldPos(Point location)
        {
            return new Vector2(location.X * tileSize, location.Y * tileSize);
        }

        /// <summary>
        /// Turn Vector2 into a Point
        /// </summary>
        /// <returns>Ceiling x and y of a vector</returns>
        public static Point ToPoint(this Vector2 vector)
        {
            return new Point((int)MathF.Ceiling(vector.X), (int)MathF.Ceiling(vector.Y));
        }

        /// <summary>
        /// Turn Vector2 into a Point with offset
        /// </summary>
        /// <returns>Ceiling x and y of a vector</returns>
        public static Point ToPoint(this Vector2 vector, int x, int y)
        {
            return new Point((int)MathF.Ceiling(vector.X) + x, (int)MathF.Ceiling(vector.Y) + y);
        }

        /// <summary>
        /// Turn Vector2 with offset into a Point
        /// </summary>
        /// <returns></returns>
        public static Point ToPoint(this Vector2 vector, Vector2 offset)
        {
            return new Point((int)MathF.Ceiling(vector.X + offset.X), (int)MathF.Ceiling(vector.Y + offset.Y));
        }
    }
}
