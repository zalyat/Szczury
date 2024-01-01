using System;
using System.Collections.Generic;
using System.Linq;

namespace Szczury.Blocks
{
    public static class BlocksRegistry
    {
        private static List<Block> blocks = new List<Block>();
        public static void AddBlock(Block block, string textureName)
        {
            block.mainTexture = TextureSet.GetTexture(textureName);
            blocks.Add(block);
        }

        public static void AddBlock(Block block)
        {
            blocks.Add(block);
        }

        public static Block GetBlock(string name)
        {
            return blocks.First(x => x.Name == name);
        }
    }
}
