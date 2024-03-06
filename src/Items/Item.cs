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
        /// <summary>
        /// Specifies stack drawing mode (default: DrawingMode.FullSlot)
        /// </summary>
        public virtual DrawingMode StackDrawingMode { get => DrawingMode.FullSlot; }

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

        public enum DrawingMode { FullSlot, TextureSize }

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
            int[] drawingInfo = stack.itemType.DrawingModeInfo(size);
            Point posPoint = point; //used for texture position
            if (stack.itemType.StackDrawingMode != DrawingMode.FullSlot)
                posPoint = new Point(point.X + drawingInfo[0], point.Y + drawingInfo[0]);

            spriteBatch.Draw(stack.itemType.mainTexture, new Rectangle(posPoint, new Point(drawingInfo[1])), Color.White);
            if(stack.amount > 1)
                spriteBatch.DrawString(TextureSet.debugFont, stack.amount.ToString(), new Vector2(point.X + size-size/3, point.Y + size-size/3), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        }

        /// <summary>
        /// Returns values to specified StackDrawingMode indexes:[0 - size, 1 - position offset]
        /// </summary>
        /// <returns></returns>
        private int[] DrawingModeInfo(int originalSize)
        {
            switch(StackDrawingMode)
            {
                case DrawingMode.FullSlot:
                    return new int[] { 0, originalSize };
                case DrawingMode.TextureSize:
                    return new int[] { (originalSize - mainTexture.Width) / 2, mainTexture.Width };
            }
            return new int[] { 0, originalSize };
        }
    }
}
