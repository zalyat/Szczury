using Szczury.Blocks;

namespace Szczury
{
    public partial class GameplayState : IState
    {
        public void InitializeBlocks()
        {
            BlocksRegistry.AddBlock(new DirtBlock(), "dirt_block");
            BlocksRegistry.AddBlock(new AirBlock());
        }
    }
}
