﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        public static Point[] Bresenham(Point start, Point end) //by Frank Lioty: https://stackoverflow.com/questions/11678693/all-cases-covered-bresenhams-line-algorithm
        {
            List<Point> points = new List<Point>();

            int w = end.X - start.X;
            int h = end.Y - start.Y;
            int dx1 = 0, dy1 = 0, dx2 = 0, dy2 = 0;
            if (w < 0) dx1 = -1; else if (w > 0) dx1 = 1;
            if (h < 0) dy1 = -1; else if (h > 0) dy1 = 1;
            if (w < 0) dx2 = -1; else if (w > 0) dx2 = 1;
            int longest = Math.Abs(w);
            int shortest = Math.Abs(h);
            if (!(longest > shortest))
            {
                longest = Math.Abs(h);
                shortest = Math.Abs(w);
                if (h < 0) dy2 = -1; else if (h > 0) dy2 = 1;
                dx2 = 0;
            }
            int numerator = longest >> 1;
            int x = start.X;
            int y = start.Y;
            for (int i = 0; i <= longest; i++)
            {
                points.Add(new Point(x, y));
                numerator += shortest;
                if (!(numerator < longest))
                {
                    numerator -= longest;
                    x += dx1;
                    y += dy1;
                }
                else
                {
                    x += dx2;
                    y += dy2;
                }
            }

            return points.ToArray();
        }

        public static Vector2 Vector(this Direction direction)
        {
            switch (direction)
            {
                case Direction.Left:
                    return new Vector2(-1, 0);
                case Direction.Right:
                    return new Vector2(1, 0);
                case Direction.Up:
                    return new Vector2(0, -1);
                case Direction.Down:
                    return new Vector2(0, 1);
            }
            return new Vector2(1, 0);
        }

        public static Direction Reverse(this Direction direction)
        {
            if (direction == Direction.Up) return Direction.Down;
            if (direction == Direction.Down) return Direction.Up;
            if (direction == Direction.Left) return Direction.Right;
            if (direction == Direction.Right) return Direction.Left;

            return Direction.Right;
        }

        public static bool isClockwise(Direction dir1, Direction dir2)
        {
            switch(dir1)
            {
                case Direction.Left:
                    if (dir2 == Direction.Up) return true;
                    return false;
                case Direction.Up:
                    if (dir2 == Direction.Right) return true;
                    return false;
                case Direction.Right:
                    if (dir2 == Direction.Down) return true;
                    return false;
                case Direction.Down:
                    if (dir2 == Direction.Left) return true;
                    return false;
            }

            return false;
        }
    }
}
