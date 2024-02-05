using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Szczury
{
    public class ItemContainer
    {
        private static Texture2D slotTexture;

        public ItemContainer(string name, int slotAmount)
        {
            if (slotTexture == null) slotTexture = TextureSet.GetTexture("containerSlot1");

            this.name = name;
            slots = new ItemSlot[slotAmount];
        }

        public ItemSlot[] slots;
        public string name;
        public struct ItemSlot
        {
            public Item.Stack stack;
        }

        public void DrawSlot(Color slotColor, Point point, int size, int slotIndex, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(slotTexture, new Rectangle(point, new Point(size)), slotColor);
        }
    }
}
