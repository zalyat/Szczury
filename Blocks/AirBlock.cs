using System;
using System.Collections.Generic;
using System.Text;

namespace Szczury.Blocks
{
    public class AirBlock : Block
    {
        public override string Name { get => "Air"; }

        public override float Hardness { get => 0f; }
    }
}
