using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Szczury.Blocks;

namespace Szczury.Items
{
    public class BasaltItem : BlockItem
    {
        public override string Name => "Basalt";

        public BasaltItem(Block block) : base(block)
        {
            this.block = block;
        }
    }
}

