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
    }
}
