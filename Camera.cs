using System;
using Microsoft.Xna.Framework;

namespace Szczury
{
    public static class Camera
    {
        public static uint width = 1280;
        public static uint height = 720;
        public static Vector2 cameraPosition;
        public static void CenterCameraOn(Vector2 position)
        {
            cameraPosition.X = position.X - width / 2;
            cameraPosition.Y = position.Y - height / 2;
        }
    }
}
