using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Szczury.Items
{
    public class MagicStickItem : MiningStickItem
    {
        public override string Name => "Magic Stick";
        public override float UseDelay => 0.0000002f;
        public override float Range => 64f * Util.tileSize;
    }
}
