using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Szczury.Items
{
    /// <summary>
    /// TO DO: Don't let the player build in the air
    /// </summary>
    public abstract class BlockItem : Item
    {
        public override float UseDelay => 0.2f;

        public Blocks.Block MainBlock { get => block; }
        protected Blocks.Block block;

        public BlockItem(Blocks.Block block)
        {
            this.block = block;
        }

        public override void OnUse(bool singleTime, float lastUse, PlayerGameObject player, bool alternativeUse)
        {
            base.OnUse(singleTime, lastUse, player, alternativeUse);

            if (alternativeUse == true && lastUse >= UseDelay) 
            {
                PlaceBlock(player);
                player.ResetItemUseDelay();
            }
        }

        public virtual void PlaceBlock(PlayerGameObject player)
        {
            Vector2 playerPos = player.Center;
            Vector2 clickPos = player.CursorPositionToWorldPosition();
            Point location = player.CursorPositionToTilePosition();
            float distance = MathF.Ceiling(Vector2.Distance(clickPos, playerPos));

            if (distance > player.placingDistance) return;

            if (player.currentWorld.isAir(location) == false) return;

            player.currentWorld.ChangeTile(location, MainBlock);
            player.inventoryContainer.ChangeItemStackAmount(player.currentToolbarIndex, -1);
        }
    }
}
