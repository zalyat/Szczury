using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Szczury.Blocks;

namespace Szczury
{
    public class FlatWorldGenerator : WorldGenerator
    {
        public int terrainHeight = 74;
        public string mainBlock = "Dirt";
        public override TileWorld.Tile[,] Generate(ushort width, ushort height)
        {
            this.width = width;
            this.height = height;
            world = new TileWorld.Tile[width, height];
            GenerateTerrain();
            return world;
        }

        private void GenerateTerrain()
        {
            Block block = BlocksRegistry.GetBlock(mainBlock);
            Block air = BlocksRegistry.GetBlock("Air");
            for (ushort y = 0; y < height; y++)
            {               
                for (ushort x = 0; x < width; x++)
                {
                    if(y < terrainHeight)
                        world[x, y].blockType = air;
                    else
                        world[x, y].blockType = block;                   
                }
            }
        }
    }
}
