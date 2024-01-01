using System;
using Szczury.Blocks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace Szczury
{
    public class TileWorld
    {
        public const ushort width = 40;
        public const ushort height = 30;
        public struct Tile
        {
            public Block blockType;
            public float damage; //if damage is higher than hardness, the tile will break
        }

        public Tile[,] world = new Tile[width, height];

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
    }
}
