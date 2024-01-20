using Szczury.Blocks;
using Microsoft.Xna.Framework;
using DotnetNoise;
using System;
using System.Diagnostics;

namespace Szczury
{
    public class WorldGenerator
    {
        public TileWorld.Tile[,] BasicGenerate(ushort width, ushort height)
        {
            TileWorld.Tile[,] chunk = new TileWorld.Tile[width, height];

            for(ushort y = 0; y < height; y++)
            {
                for(ushort x = 0; x < width; x++)
                {
                    if(y > height/4)//ground
                    {
                        if ((y > height * 0.35f && y < height / 2) && (x > width * 0.4f && x < width * 0.6f)) { chunk[x, y].blockType = BlocksRegistry.GetBlock("Air"); continue; }
                        chunk[x, y].blockType = BlocksRegistry.GetBlock("Dirt");
                        continue;
                    }
                    //air
                    chunk[x, y].blockType = BlocksRegistry.GetBlock("Air");
                }
            }
            return chunk;
        }

        public TileWorld.Tile[,] TestGenerate(ushort width, ushort height)
        {
            int seed = new Random().Next(0, 9999);
            TileWorld.Tile[,] chunk = new TileWorld.Tile[width, height];
            FastNoise perlin1 = new FastNoise(seed);
            FastNoise perlin2 = new FastNoise(seed/2);
            float scale = 8f;
            for (ushort y = 0; y < height; y++)
            {                
                for (ushort x = 0; x < width; x++)
                {
                    float p = perlin1.GetPerlin(x*scale, 0);
                    float r = -MathF.Abs(perlin2.GetPerlin(x*scale*0.1f, 0));

                    float value = p * (r*10f);

                    //Debug.WriteLine($"{p} {r} {value}");

                    if (y > MathF.Floor(value*10)+30)
                    {
                        chunk[x, y].blockType = BlocksRegistry.GetBlock("Dirt");
                        continue;
                    }
                    //air
                    chunk[x, y].blockType = BlocksRegistry.GetBlock("Air");
                }
            }
            return chunk;
        }
    }
}
