using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Text;
using System.Diagnostics;

namespace Szczury.Items
{
    /// <summary>
    /// TO DO: Change Stack from struct to class
    /// </summary>
    public abstract class Item
    {
        public Texture2D mainTexture;
        public abstract string Name { get; }
        public abstract float UseDelay { get; }

        public struct Stack
        {
            public Stack(Item itemType, int amount)
            {
                this.itemType = itemType;
                this.amount = amount;
            }
            public void Clear()
            {
                itemType = null;
                amount = 0;
            }

            public Item itemType;
            public int amount;
        }

        public Item()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="singleTime">is this the first frame when mouse button was pressed?</param>
        /// <param name="lastUse">timer of the last player's item use</param>
        /// <param name="player">a player that is using the item</param>
        /// <param name="alternativeUse">did player used the right mouse button</param>
        public virtual void OnUse(bool singleTime, float lastUse, PlayerGameObject player, bool alternativeUse)
        {
            
        }

        public static void DrawStack(Stack stack, Point point, int size, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(stack.itemType.mainTexture, new Rectangle(point, new Point(size)), Color.White);
            if(stack.amount > 1)
                spriteBatch.DrawString(TextureSet.debugFont, stack.amount.ToString(), new Vector2(point.X + size-size/3, point.Y + size-size/3), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        }
    }
}
