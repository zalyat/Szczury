using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Szczury
{
    public static class Util
    {
        public static GameTime gameTime;
        public static float deltaTime
        {
            get => (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public static int tileSize = 16;
    }
}
