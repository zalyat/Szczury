using System;
using Szczury.Blocks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace Szczury
{
    public class TileWorld
    {
        public const ushort width = 80;
        public const ushort height = 45;
        public struct Tile
        {
            public Block blockType;
            public float damage; //if damage is higher than hardness, the tile will break
        }

        private Tile[,] world = new Tile[width, height];

        public void DrawTile(ushort x, ushort y, SpriteBatch _spriteBatch)
        {
            Tile tile = world[x, y];
            if(tile.blockType.GetType() == typeof(AirBlock))
            {
                return;
            }
            _spriteBatch.Draw(tile.blockType.mainTexture,
                new Rectangle(new Point(x * Util.tileSize, y * Util.tileSize),
                new Point(Util.tileSize, Util.tileSize)),
                Color.White);            
        }

        public void Initialize()
        {
            WorldGenerator worldGenerator = new WorldGenerator();
            world = worldGenerator.BasicGenerate(width, height);
        }

        public Point WorldPositionToTilePosition(Vector2 position)
        {
            int tileSize = Util.tileSize;
            return new Point((int)MathF.Ceiling(position.X/tileSize)-1, (int)MathF.Ceiling(position.Y/tileSize));
        }

        public void ChangeTile(Point location, Block block)
        {
            if (block == null) return;
            if (isInWorldBoundaries(location) == false) return;

            world[location.X, location.Y].blockType = block;
        }

        public Tile? GetTile(Point location)
        {
            //Debug.WriteLine($"Getting tile: {location.X}, {location.Y} | isInWorldBoundaries?: {isInWorldBoundaries(location)}");
            if (isInWorldBoundaries(location) == false)
            {
                Debug.WriteLine(">>>>>>>>>>Tile not in boundaries! " + location.X + " " + location.Y);
                return null;
            }
            return world[location.X, location.Y];
        }

        public bool isAir(Point location)
        {
            if (isInWorldBoundaries(location) == false) return true;
            if (world[location.X, location.Y].blockType == BlocksRegistry.GetBlock("Air")) return true;
            return false;
        }

        public bool isInWorldBoundaries(Point location)
        {
            if (location.X >= width || location.X < 0 || location.Y >= height || location.Y < 0)
                return false;

            return true;
        }

        public Rectangle GetTileRectangle(Point location)
        {
            if (isInWorldBoundaries(location) == false) return new Rectangle();
            Point topLeft = new Point(location.X * Util.tileSize, location.Y * Util.tileSize);
            Point bottomRight = new Point(location.X * Util.tileSize + Util.tileSize, location.Y * Util.tileSize + Util.tileSize);
            return new Rectangle(topLeft, bottomRight);
        }
    }
}
