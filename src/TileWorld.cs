﻿using System;
using Szczury.Blocks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using System.IO;


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
                damagePhase = 0;
            }
            public Block blockType;
            public float damage; //if damage is higher than hardness, the tile will break
            public byte damagePhase; //used only for damage texture (when damage is higher than 0)
        }

        private Tile[,] world = new Tile[width, height];
        private SpriteBatch _spriteBatch;
        private Texture2D _damageTexture;

        public void SetSpriteBatch(SpriteBatch spriteBatch)
        {
            _spriteBatch = spriteBatch;
        }

        public void Initialize()
        {
            _damageTexture = TextureSet.GetTexture("tile_damage");
            WorldGenerator worldGenerator = new ClassicWorldGenerator();
            world = worldGenerator.Generate(width, height);
        }

        public static Point WorldPositionToTilePosition(Vector2 position)
        {
            int tileSize = Util.tileSize;
            return new Point((int)MathF.Ceiling(position.X / tileSize) - 1, (int)MathF.Ceiling(position.Y / tileSize) - 1);
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

        /// <summary>
        /// Add damage to a tile & break it if it becomes too high
        /// </summary>
        /// <returns>Tile damage after damaging it</returns>
        public float DamageTile(Point location, float value)
        {
            if (isInWorldBoundaries(location) == false) return -3f;

            world[location.X, location.Y].damage += value;
            float damage = world[location.X, location.Y].damage;
            Block blockType = world[location.X, location.Y].blockType;

            if (damage >= blockType.MaxDamage) BreakTile(location);

            if (damage > 0) world[location.X, location.Y].damagePhase = GetDamagePhase(damage, blockType.MaxDamage);

            return damage;
        }

        /// <summary>
        /// Break a block with already specified block type (faster)
        /// </summary>
        private void BreakTile(Point location, Block blockType)
        {
            blockType.OnBreak(location, this);
            world[location.X, location.Y].damage = 0;
            world[location.X, location.Y].blockType = BlocksRegistry.GetBlock("Air");
        }

        /// <summary>
        /// Break a block and get it's block type to call a it's OnBreak() method
        /// </summary>
        public void BreakTile(Point location)
        {
            BreakTile(location, world[location.X, location.Y].blockType);
        }

        public bool isAir(Point location)
        {
            if (isInWorldBoundaries(location) == false) return true;
            Block blockType = world[location.X, location.Y].blockType;
            if (blockType == BlocksRegistry.GetBlock("Air") || blockType == BlocksRegistry.GetBlock("Border")) return true;
            return false;
        }

        /// <summary>
        /// Check if neighbouring tiles are something different than empty block
        /// </summary>
        public bool hasTileNeighbours(Point location)
        {
            Point[] neighbours = getTileNeigbours(location);
            for(int i = 0; i < neighbours.Length; i++)
            {
                if (isAir(neighbours[i]) == false) return true;
            }
            return false;
        }

        public Point[] getTileNeigbours(Point location)
        {
            int x = location.X;
            int y = location.Y;
            return new Point[4] { new Point(x - 1, y), new Point(x, y - 1), new Point(x + 1, y), new Point(x, y + 1) }; 
        }

        public bool isInWorldBoundaries(Point location)
        {
            if (location.X >= width || location.X < 0 || location.Y >= height || location.Y < 0)
                return false;

            return true;
        }

        /// <summary>
        /// Recursive stuff that returns first non-air tile with a air tile above. If it cannot find one, it will return a tile out of boundaries.
        /// So remember always check if the returned tile is in boundaries when using this
        /// </summary>
        /// <param name="start">first check</param>
        public Point getClosestGround(Point start)
        {
            return getClosestGround(start, 0);
        }

        private Point getClosestGround(Point start, int n)
        {
            if (start.Y >= height) return start;

            if (start.Y >= 0) n++;
            if (n == 0) return getClosestGround(start + new Point(0, 1), n);

            if (isGroundFree(start) == true)
                return start;
            else
                return getClosestGround(start + new Point(0, 1), n);
        }

        /// <summary>
        /// Warning: has no boundary check. Potentially unsafe
        /// </summary>
        public bool isGroundFree(Point location)
        {
            if(isAir(location) == false && isAir(location + new Point(0, -1)) == true)
            {
                return true;
            }
            return false;
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
            Point position = Camera.OnScreen(new Point(x * Util.tileSize, y * Util.tileSize));
            Point size = new Point(Util.tileSize, Util.tileSize);
            _spriteBatch.Draw(tile.blockType.mainTexture,
                new Rectangle(position, size),
                Color.White);
            if (tile.damage > 0)
            {
                byte damagePhase = tile.damagePhase;
                _spriteBatch.Draw(_damageTexture, position.ToVector2(), new Rectangle(Util.tileSize * damagePhase, 0, size.X, size.Y), Color.White);
            }
        }

        private byte GetDamagePhase(float damage, float maxDamage)
        {
            if (damage < 0.25f * maxDamage && damage > 0) return 0;
            if (damage < 0.5f * maxDamage) return 1;
            if (damage < 0.75f * maxDamage) return 2;
            if (damage >= 0.75f * maxDamage) return 3;

            return 0;
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
