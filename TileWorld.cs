using System;
using Szczury.Blocks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using Microsoft.Win32;

namespace Szczury
{
    public class TileWorld
    {
        public const ushort width = 512;
        public const ushort height = 64;

        public struct Chunk
        {
            public bool isValid;

            /// <summary> x axis </summary>
            public short i;
            /// <summary> y axis </summary>
            public short j;
        }

        public struct Tile
        {
            public Block blockType;
            public float damage; //if damage is higher than hardness, the tile will break
            public bool debugHighlight;
        }

        private Tile[,] world = new Tile[width, height];
        private SpriteBatch _spriteBatch;
        
        public void SetSpriteBatch(SpriteBatch spriteBatch)
        {
            _spriteBatch = spriteBatch;
        }

        public void Initialize()
        {            
            WorldGenerator worldGenerator = new WorldGenerator();
            world = worldGenerator.TestGenerate(width, height);
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

        private void DrawTile(ushort x, ushort y)
        {
            Tile tile = world[x, y];
            if (tile.blockType.GetType() == typeof(AirBlock))
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

        ///
        /// Chunks and stuff
        /// Use i, j to name chunk coordinates
        ///

        public Chunk GetChunkAtTilePosition(Point location)
        {
            Chunk chunk = new Chunk();
            if (isInWorldBoundaries(location) == false)
            {
                chunk.isValid = false;
                return chunk;
            }
            else chunk.isValid = true;

            chunk.i = (short)MathF.Ceiling(location.X/Util.chunkSize);
            chunk.j = (short)MathF.Ceiling(location.Y/Util.chunkSize);
            //Debug.WriteLine($"{location} {location} {chunk.i} {chunk.j} {chunk.isValid}");
            return chunk;
        }

        public Rectangle GetChunkBoundaries(int i, int j) =>
            new Rectangle(i * Util.chunkSize, j * Util.chunkSize, i * Util.chunkSize + Util.chunkSize, j * Util.chunkSize + Util.chunkSize);

        public void DrawChunk(Chunk chunk)
        {
            if (chunk.isValid == false) return;
            Rectangle rect = GetChunkBoundaries(chunk.i, chunk.j);

            for (short x = chunk.i; x < rect.Width; x++)
                for (short y = chunk.j; y < rect.Height; y++)
                    DrawTile((ushort)x, (ushort)y);
        }
        
    }
}
