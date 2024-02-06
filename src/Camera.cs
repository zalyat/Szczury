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
    }
}
