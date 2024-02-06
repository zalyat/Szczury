using Szczury.Blocks;
using Szczury.Items;

namespace Szczury
{
    public partial class GameplayState : IState
    {
        public void InitializeBlocks()
        {
            BlocksRegistry.AddBlock(new DirtBlock(), "dirt_block");
            BlocksRegistry.AddBlock(new AirBlock());
            BlocksRegistry.AddBlock(new BorderBlock());
            BlocksRegistry.AddBlock(new BasaltBlock(), "basalt_block");
        }

        public void InitializeItems()
        {
            ItemsRegistry.AddItem(new MiningStickItem(), "item_mining_stick");
        }
    }
}
