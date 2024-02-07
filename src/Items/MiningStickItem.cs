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
            Point tileLocation = player.CursorPositionToTilePosition();
            TileWorld.Tile tile = player.currentWorld.GetTile(tileLocation);
            if (tile.blockType.Name != "Air" && tile.blockType.Name != "Border")
            {
                player.currentWorld.SetTile(tileLocation, Blocks.BlocksRegistry.GetBlock("Air"));
            }
        }
    }
}
