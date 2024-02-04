using Szczury.Blocks;

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
;        }
    }
}
