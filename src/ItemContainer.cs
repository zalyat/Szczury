using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Szczury.Items;

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

        public void AddItemStack(Item.Stack stack, int index, bool overrideExisting)
        {
            if(index < 0 || index >= slots.Length) { Debug.WriteLine($"{index} is a too high index for {name}"); return; };

            if (slots[index].stack.itemType != null && overrideExisting == false)
                return;

            slots[index].stack = stack;
        }

        public void DrawSlot(Color slotColor, Point point, int size, int slotIndex, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(slotTexture, new Rectangle(point, new Point(size)), slotColor);
            if (slots[slotIndex].stack.itemType != null)
                Item.DrawStack(slots[slotIndex].stack, point, size, spriteBatch);
        }
    }
}
