using DotnetNoise;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Szczury.Blocks;

namespace Szczury
{
    public class ClassicWorldGenerator : WorldGenerator
    {
        private TileWorld.Tile[,] world;
        private int seed;
        private ushort width, height;
        public override TileWorld.Tile[,] Generate(ushort width, ushort height)
        {
            this.width = width;
            this.height = height;
            seed = new Random().Next(0, 9999);
            world = new TileWorld.Tile[width, height];

            GenerateTerrain();
            GenerateCaves();

            return world;
        }

        protected virtual void GenerateTerrain()
        {
            FastNoise perlin1 = new FastNoise(seed);
            FastNoise perlin2 = new FastNoise(seed / 2);
            float scale = 8f;
            for (ushort y = 0; y < height; y++)
            {
                for (ushort x = 0; x < width; x++)
                {
                    float p = perlin1.GetPerlin(x * scale, 0);
                    float r = -MathF.Abs(perlin2.GetPerlin(x * scale * 0.1f, 0));

                    float value = p * r * 10;

                    if (y > MathF.Floor(value * 10) + 30)
                    {
                        world[x, y].blockType = BlocksRegistry.GetBlock("Dirt");
                        continue;
                    }
                    //air
                    world[x, y].blockType = BlocksRegistry.GetBlock("Air");
                }
            }
        }

        protected virtual void GenerateCaves()
        {
            FastNoise noise = new FastNoise(seed * 345);
            noise.Frequency = 0.016f;
            noise.Octaves = 2;
            noise.FractalTypeMethod = FastNoise.FractalType.RigidMulti;
            //noise.CellularJitter = 1f;
            noise.UsedCellularDistanceFunction = FastNoise.CellularDistanceFunction.Manhattan;
            noise.UsedCellularReturnType = FastNoise.CellularReturnType.Distance2Mul;
            float caveScale = 2f;
            float airThreshold = 0.8f;
            for (ushort y = 0; y < height; y++)
            {
                for (ushort x = 0; x < width; x++)
                {
                    float p = noise.GetCellular(x * caveScale, y * caveScale);

                    if(p > airThreshold)
                        world[x, y].blockType = BlocksRegistry.GetBlock("Air");
                }
            }
        }
    }
}
