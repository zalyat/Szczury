using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Szczury.Items
{
    public class MiningStickItem : Item
    {
        public override string Name => "Mining Stick";
        public override float UseDelay => 0.5f;
        public virtual float Power => 2f; //max hardness of a block that it can break
        public virtual float MiningDamage => 0.24f; //how much damage it gives to a block
        public virtual float Range => 4 * Util.tileSize;
        protected Texture2D objectTexture;

        public override void OnUse(bool singleTime, float lastUse, PlayerGameObject player)
        {
            base.OnUse(singleTime, lastUse, player);

            if (lastUse > UseDelay)
            {         
                Mine(player);
                player.ResetItemUseDelay();
            }
        }

        private void Mine(PlayerGameObject player)
        {
            if (Vector2.Distance(player.Center, player.CursorPositionToWorldPosition()) > Range) return;

            GameplayState.Main.CreateGameObject(new MiningStickObject(player.Position, player.CursorPositionToWorldPosition(), player.currentWorld, UseDelay));
            Point tileLocation = player.CursorPositionToTilePosition();
            TileWorld.Tile tile = player.currentWorld.GetTile(tileLocation);
            if (tile.blockType.Name != "Air" && tile.blockType.Name != "Border" && tile.blockType.Hardness <= Power)
            {
                player.currentWorld.DamageTile(tileLocation, MiningDamage);
            }
        }
    }
}
