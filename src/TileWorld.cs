using System;
using Szczury.Blocks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;


namespace Szczury
{
    public class TileWorld
    {
        public const ushort width = Util.chunkSize * 64;
        public const ushort height = Util.chunkSize * 32;

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
            public Tile(Block b)
            {
                blockType = b;
                damage = -1f;
            }
            public Block blockType;
            public float damage; //if damage is higher than hardness, the tile will break
        }

        private Tile[,] world = new Tile[width, height];
        private SpriteBatch _spriteBatch;
        
        public void SetSpriteBatch(SpriteBatch spriteBatch)
        {
            _spriteBatch = spriteBatch;
        }

        public void Initialize()
        {            
            WorldGenerator worldGenerator = new ClassicWorldGenerator();
            world = worldGenerator.Generate(width, height);
        }

        public static Point WorldPositionToTilePosition(Vector2 position)
        {
            int tileSize = Util.tileSize;
            return new Point((int)MathF.Ceiling(position.X/tileSize)-1, (int)MathF.Ceiling(position.Y/tileSize)-1);
        }

        public void ChangeTile(Point location, Block block)
        {
            if (block == null) return;
            if (isInWorldBoundaries(location) == false) return;

            world[location.X, location.Y].blockType = block;
        }

        public Tile GetTile(Point location)
        {
            //Debug.WriteLine($"Getting tile: {location.X}, {location.Y} | isInWorldBoundaries?: {isInWorldBoundaries(location)}");
            if (isInWorldBoundaries(location) == false)
            {
                Debug.WriteLine(">>>>>>>>>>Tile not in boundaries! " + location.X + " " + location.Y);
                return new Tile(BlocksRegistry.GetBlock("Border"));
            }
            return world[location.X, location.Y];
        }

        public void SetTile(Point location, Block block)
        {
            if (isInWorldBoundaries(location) == false)
            {
                Debug.WriteLine("!!!!!!!!!!Tile not in boundaries! " + location.X + " " + location.Y);
                return;
            }
            world[location.X, location.Y].blockType = block;
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
            //PerFrameCounter.renderingActionsInFrame += 1;
            Tile tile = world[x, y];
            if (tile.blockType.GetType() == typeof(AirBlock))
            {
                return;
            }            
            _spriteBatch.Draw(tile.blockType.mainTexture,
                new Rectangle(Camera.OnScreen(new Point(x * Util.tileSize, y * Util.tileSize)),
                new Point(Util.tileSize, Util.tileSize)),
                Color.White);
        }

        ///
        /// Chunks and stuff
        /// Use i, j to name chunk coordinates
        ///

        public Chunk GetChunkAtTilePosition(Point location)
        {
            Chunk chunk = new Chunk();
            //Debug.WriteLine($"{location.X / Util.chunkSize} {location.Y / Util.chunkSize}");
            if (isInWorldBoundaries(location) == false)
            {
                chunk.isValid = false;
                return chunk;
            }
            else chunk.isValid = true;

            chunk.i = (short)MathF.Ceiling(location.X/Util.chunkSize);
            chunk.j = (short)MathF.Ceiling(location.Y/Util.chunkSize);
            
            return chunk;
        }

        public Rectangle GetChunkBoundaries(int i, int j) =>
            new Rectangle(i * Util.chunkSize, j * Util.chunkSize, i * Util.chunkSize + Util.chunkSize, j * Util.chunkSize + Util.chunkSize);

        public void DrawChunk(Chunk chunk)
        {
            if (chunk.isValid == false) return;

            Rectangle rect = GetChunkBoundaries(chunk.i, chunk.j);
            //Debug.WriteLine($"Rendering chunk {chunk.i} {chunk.j} | {rect.X} {rect.Y} {rect.Width} {rect.Height} | {test}");
            for (short x = (short)rect.X; x < rect.Width; x++)
                for (short y = (short)rect.Y; y < rect.Height; y++)
                {
                    DrawTile((ushort)x, (ushort)y);
                }
        }

        public static bool isDifferentChunk(Chunk a, Chunk b)
        {
            if (a.i != b.i || a.j != b.j) return true;
            return false;
        }
        

        ///
        ///Dumb stuff
        ///
        ///

        public void SaveAs(string path, string name, GraphicsDevice graphicsDevice)
        {
            Texture2D map = new Texture2D(graphicsDevice, width, height);

            Color[] colBuff = new Color[width * height];
            for(int y = 0; y < height; y++)
            {
                for(int x = 0; x < width; x++)
                {
                    Block block = world[x, y].blockType;
                    colBuff[y * width + x] = block.ColorRepresentation;
                }
            }

            
            map.SetData(colBuff);

            Stream str = File.OpenWrite(path + name);
            map.SaveAsPng(str, width, height);
            str.Dispose();
            map.Dispose();
        }
    }
}
