using Szczury.Blocks;
using Microsoft.Xna.Framework;
using DotnetNoise;
using System;
using System.Diagnostics;

namespace Szczury
{
    public abstract class WorldGenerator
    {
        public virtual TileWorld.Tile[,] Generate(ushort width, ushort height)
        {
            return null;
        }
    }
}
