using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Szczury.Monsters
{
    public class MonsterToxicSnail : MonsterSnail
    {
        public override float MaxHealth => throw new NotImplementedException();

        public MonsterToxicSnail(Vector2 startingPosition, TileWorld world, bool invertedDirection) : base(startingPosition, world, invertedDirection)
        {
            mainTexture = TextureSet.GetTexture("monster_toxic_snail");
        }        
    }
}
