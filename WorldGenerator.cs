using Szczury.Blocks;
using Microsoft.Xna.Framework;
using DotnetNoise;
using System;
using System.Diagnostics;
using SharpDX.Direct3D9;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace Szczury
{
    public abstract class WorldGenerator
    {
        protected TileWorld.Tile[,] world;
        protected ushort width, height;

        protected struct GenTile
        {
            public GenTile(TileWorld.Tile tile, ushort x, ushort y)
            {
                this.tile = tile; this.x = x; this.y = y;
            }
            public TileWorld.Tile tile;
            public ushort x, y;
        }

        public virtual TileWorld.Tile[,] Generate(ushort width, ushort height)
        {            
            return null;
        }

        protected virtual GenTile[] Neighboring4(ushort x, ushort y)
        {
            return new GenTile[4] { //the most disguisting code known to man
                new GenTile(GetTile(new Point(x-1,y)), (ushort)(x - 1), y),
                new GenTile(GetTile(new Point(x+1,y)), (ushort)(x + 1), y),
                new GenTile(GetTile(new Point(x,y-1)), x, (ushort)(y - 1)),
                new GenTile(GetTile(new Point(x,y+1)), x, (ushort)(y + 1))
            };
        }

        protected virtual GenTile[] GetSolid(GenTile[] genTiles)
        {
            Block border = BlocksRegistry.GetBlock("Border");
            Block air = BlocksRegistry.GetBlock("Air");
            List<GenTile> output = new List<GenTile>();
            for(int i = 0; i < genTiles.Length; i++)
            {
                GenTile genTile = genTiles[i];
                if (genTile.tile.blockType != air && genTile.tile.blockType != border)
                    output.Add(genTile);
            }
            return output.ToArray();
        }

        protected TileWorld.Tile GetTile(Point location)
        {
            if (isInWorldBoundaries(location) == false)
            {
                return new TileWorld.Tile(BlocksRegistry.GetBlock("Border"));
            }
            return world[location.X, location.Y];
        }

        protected bool isInWorldBoundaries(Point location)
        {
            if (location.X >= width || location.X < 0 || location.Y >= height || location.Y < 0)
                return false;

            return true;
        }
    }
}
