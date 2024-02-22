using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Szczury.Blocks
{
    internal class BorderBlock : Block
    {
        public override string Name => "Border";

        public override float Hardness => 2024f;
        public override float MaxDamage => 2024f;
        public override Color ColorRepresentation => Color.Red;
    }
}
