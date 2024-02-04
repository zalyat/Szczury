using Microsoft.Xna.Framework;

namespace Szczury.Blocks
{
    public class DirtBlock : Block
    {
        public override string Name { get => "Dirt"; }
        public override float Hardness { get => 2.0f; }
        public override Color ColorRepresentation => Color.SandyBrown;
    }
}
