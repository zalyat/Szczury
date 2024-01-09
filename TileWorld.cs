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
            public bool debugHighlight;
        }

        private Tile[,] world = new Tile[width, height];

        public void DrawTile(ushort x, ushort y, SpriteBatch _spriteBatch)
        {
            Tile tile = world[x, y];
            if(tile.blockType.GetType() == typeof(AirBlock))
            {
                return;
            }
            Color color = Color.White;
            if (tile.debugHighlight == true) color = Color.Red;
            _spriteBatch.Draw(tile.blockType.mainTexture,
                new Rectangle(new Point(x * Util.tileSize - (int)MathF.Ceiling(Camera.cameraPosition.X), 
                y * Util.tileSize - (int)MathF.Ceiling(Camera.cameraPosition.Y)),
                new Point(Util.tileSize, Util.tileSize)),
                color);
            tile.debugHighlight = false;
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

        public void SetDebugHighlight(Point location)
        {
            if (isInWorldBoundaries(location) == true)
                world[location.X, location.Y].debugHighlight = !world[location.X, location.Y].debugHighlight;
        }

        ///<summary>Used for colliders etc.</summary> <returns>Rectangle that is boundaries for a tile</returns>
        public Rectangle GetTileRectangle(Point location)
        {
            if (isInWorldBoundaries(location) == false) return new Rectangle();
            Point topLeft = new Point(location.X * Util.tileSize, location.Y * Util.tileSize);
            return new Rectangle(topLeft, new Point(Util.tileSize, Util.tileSize));
        }

        /// <summary>
        /// Warning! Use this only after making sure you are not giving location that is out of boundaries!
        /// </summary>
        /// <returns>World position of a tile (top-left corner is the origin)</returns>
        public Vector2 TilePositionToWorldPosition(Point location)
        {
            if (isInWorldBoundaries(location) == false) return new Vector2(0, 0);
            return new Vector2(location.X * Util.tileSize, location.Y * Util.tileSize);
        }
    }
}
