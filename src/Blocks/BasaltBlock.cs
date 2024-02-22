using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Szczury.Blocks
{
    public class BasaltBlock : Block
    {
        public override string Name => "Basalt";

        public override float Hardness => 6.0f;
        public override float MaxDamage => 2.0f;
        public override Color ColorRepresentation => Color.Gray;
    }
}
