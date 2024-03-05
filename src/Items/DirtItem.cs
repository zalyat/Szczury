using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Szczury.Blocks;

namespace Szczury.Items
{
    public class DirtItem : BlockItem
    {
        public override string Name => "Dirt";

        public DirtItem(Block block) : base(block)
        {
            this.block = block;
        }
    }
}
