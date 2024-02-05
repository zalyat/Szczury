using DotnetNoise;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Configuration.Internal;
using System.Diagnostics;
using Szczury.Blocks;

namespace Szczury
{
    public class ClassicWorldGenerator : WorldGenerator
    {
        private int seed;

        protected int terrainHeight = 100;
        protected float terrainScale = 1f;
        protected float terrainHillHeight = 12f;
        protected float terrainTorn = 6f;

        protected int basaltCaveSteps = 8;
        protected int basaltCaveRoll = 4;
        protected int basaltCaveChanceToSpawn = 13;
        protected int basaltCaveTopLimit = 120;
        protected int basaltTopLimit = 270;
        protected int basaltBorder = 90;

        public override TileWorld.Tile[,] Generate(ushort width, ushort height)
        {
            this.width = width;
            this.height = height;
            seed = new Random().Next(0, 9999);
            world = new TileWorld.Tile[width, height];

            GenerateTerrain();
            GenerateCaves();
            GenerateBasaltUnderground();

            return world;
        }

        protected virtual void GenerateTerrain()
        {
            FastNoise perlin1 = new FastNoise(seed);
            FastNoise perlin2 = new FastNoise(seed / 2);
            for (ushort y = 0; y < height; y++)
            {
                for (ushort x = 0; x < width; x++)
                {
                    float p = perlin1.GetPerlin(x * terrainScale, 0) * terrainHillHeight;
                    float r = perlin2.GetPerlin(x * terrainScale * terrainTorn, 0);

                    float value = -MathF.Abs((p + r)/2);

                    if (y > MathF.Floor(value * 10) + terrainHeight)
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

            Random random = new Random(seed);
            for (ushort y = 0; y < height; y++)
            {
                for (ushort x = 0; x < width; x++)
                {
                    float p = noise.GetCellular(x * caveScale, y * caveScale);

                    if (p > airThreshold)
                    {
                        world[x, y].blockType = BlocksRegistry.GetBlock("Air");
                        if(random.Next(0,100) <= basaltCaveChanceToSpawn && y < basaltTopLimit && y > basaltCaveTopLimit) //no need to generate basalt in caves when in basalt underground layer
                            GenerateBasaltStepCave(GetSolid(Neighboring4(x, y)), 0);
                    }
                }
            }
        }

        protected virtual void GenerateBasaltStepCave(GenTile[] genTiles, int step)
        {
            if (step > basaltCaveSteps) return;
            if (new Random().Next(0, basaltCaveRoll) != 1) return; 
            for(int i = 0; i < genTiles.Length; i++)
            {
                GenTile g = genTiles[i];
                world[g.x, g.y].blockType = BlocksRegistry.GetBlock("Basalt");
                GenerateBasaltStepCave(GetSolid(Neighboring4(g.x, g.y)), step + 1);
            }
        }

        protected virtual void GenerateBasaltUnderground()
        {
            Random r = new Random(seed);
            for(int y = basaltTopLimit; y < height; y++)
            {
                int chance = y - basaltTopLimit;
                for (int x = 0; x < width; x++)
                {
                    if (world[x, y].blockType.Name == "Air") continue;            

                    int roll = r.Next(0, basaltBorder);
                    if (roll < chance) world[x, y].blockType = BlocksRegistry.GetBlock("Basalt");
                }
            }
        }
    }
}
