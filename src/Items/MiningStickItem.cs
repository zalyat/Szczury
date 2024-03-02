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
            //if (Vector2.Distance(player.Center, player.CursorPositionToWorldPosition()) > Range) return;
            
            //old single tile method
            /*Point tileLocation = player.CursorPositionToTilePosition();
            TileWorld.Tile tile = player.currentWorld.GetTile(tileLocation);
            if (tile.blockType.Name != "Air" && tile.blockType.Name != "Border" && tile.blockType.Hardness <= Power)
            {
                player.currentWorld.DamageTile(tileLocation, MiningDamage);
            }*/

            //new multiple lines destroying method
            Point startLocation = new Point(player.PositionInTiles.X, player.PositionInTiles.Y + 1);
            Vector2 targetVector = GetLimitedTargetVector(player.Center, player.CursorPositionToWorldPosition());
            Point endLocation = TileWorld.WorldPositionToTilePosition(targetVector);

            Point[] tilePoints = Util.Bresenham(startLocation, endLocation);
            for(int i = 0; i < tilePoints.Length; i++)
            {
                TileWorld.Tile tile = player.currentWorld.GetTile(tilePoints[i]);
                if (tile.blockType.Name != "Air" && tile.blockType.Name != "Border" && tile.blockType.Hardness <= Power)
                    player.currentWorld.DamageTile(tilePoints[i], MiningDamage);
            }

            GameplayState.Main.CreateGameObject(new MiningStickObject(player.Position, targetVector, player.currentWorld, UseDelay));
        }

        /// <summary>
        /// This allows player to click anywhere on screen and swing his tool without caring about the distance
        /// </summary>
        private Vector2 GetLimitedTargetVector(Vector2 playerPos, Vector2 targetPos)
        {
            if(Vector2.Distance(playerPos, targetPos) > Range)
            {
                Debug.WriteLine($"{playerPos} {targetPos} {Vector2.Normalize(targetPos - playerPos)} {Vector2.Normalize(targetPos - playerPos) * Range}");
                return playerPos + Vector2.Normalize(targetPos - playerPos) * Range;
            }
            return targetPos;
        }
    }
}
