﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Szczury.Items;

namespace Szczury
{
    public partial class PlayerGameObject
    {
        public bool showInventory = false;

        private Texture2D markerTexture;
        private int markerSize = (int)Math.Ceiling(UIStyle.containerSlotSize * 1.1f);
        private int Margin => 6;
        private Color slotColor => Color.PeachPuff;
        private Color toolbarColor => Color.Orange;

        public ItemContainer inventoryContainer = new ItemContainer("Player's inventory", 12);

        public int toolbarLength = 4;
        private int currentToolbarIndex = 0;

        public void DrawInventory(SpriteBatch spriteBatch)
        {
            int invSize = inventoryContainer.slots.Length;
            int row = toolbarLength * UIStyle.containerSlotSize;
            int slotSize = UIStyle.containerSlotSize;
            int slotMargin = UIStyle.conatinerSlotMargin;
                
            for(int i = 0; i < invSize; i++)
            {
                int collumn = (int)MathF.Floor(i / toolbarLength);
                int xInCollumn = i % toolbarLength;
                int x =  i * slotSize + Margin + (slotMargin * xInCollumn) - (collumn * row);
                int y = collumn * slotSize + Margin + (slotMargin * collumn);

                inventoryContainer.DrawSlot(slotColor, new Point(x, y), UIStyle.containerSlotSize, i, spriteBatch);
            }
        }

        public void DrawToolbar(SpriteBatch spriteBatch)
        {
            int slotSize = UIStyle.containerSlotSize;
            int slotMargin = UIStyle.conatinerSlotMargin;
            for (int i = 0; i < toolbarLength; i++)
            {
                int x = i * slotSize + Margin + (slotMargin * i);
                inventoryContainer.DrawSlot(toolbarColor, new Point(x, Margin), slotSize, i, spriteBatch);
            }
        }

        public void DrawToolbarMarker(SpriteBatch spriteBatch)
        {
            int slotSize = UIStyle.containerSlotSize;
            int slotMargin = UIStyle.conatinerSlotMargin;

            int markerSizeOffset = (markerSize - slotSize) / 2;
            int x = currentToolbarIndex * slotSize + Margin + (slotMargin * currentToolbarIndex) - markerSizeOffset;
            int y = Margin - markerSizeOffset;
            spriteBatch.Draw(markerTexture, new Rectangle(new Point(x, y), new Point(markerSize)), Color.Gold);
        }

        /// <summary>
        /// Input related to inventory (mostly mouse clicks on item slots)
        /// </summary>
        private void IntentoryInput()
        {

        }

        public Item.Stack GetCurrentItem()
        {
            if (inventoryContainer.slots[currentToolbarIndex].stack.itemType == null)
            {
                return new Item.Stack();
            }
            return inventoryContainer.slots[currentToolbarIndex].stack;
        }
    }
}