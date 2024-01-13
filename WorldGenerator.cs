using Szczury.Blocks;

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
    }
}
