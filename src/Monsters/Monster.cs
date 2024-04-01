using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Szczury.Monsters
{
    public abstract class Monster : GameObject
    {
        public abstract float MaxHealth { get; }
        public Monster(Vector2 startingPosition, TileWorld world) : base(startingPosition, world)
        {
        }
    }
}
