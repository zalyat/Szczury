using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
        private Rectangle[] inventoryUIRects;

        private Item.Stack stackOnCursor;
        private bool isStackOnCursor;

        public int toolbarLength = 4;
        public int currentToolbarIndex = 0;


        public void CalculateInventoryUI()
        {
            int invSize = inventoryContainer.slots.Length;
            int row = toolbarLength * UIStyle.containerSlotSize;
            int slotSize = UIStyle.containerSlotSize;
            int slotMargin = UIStyle.conatinerSlotMargin;
            Rectangle[] uiBuffer = new Rectangle[invSize];

            for (int i = 0; i < invSize; i++)
            {
                int collumn = (int)MathF.Floor(i / toolbarLength);
                int xInCollumn = i % toolbarLength;
                int x = i * slotSize + Margin + (slotMargin * xInCollumn) - (collumn * row);
                int y = collumn * slotSize + Margin + (slotMargin * collumn);

                uiBuffer[i] = new Rectangle(x, y, UIStyle.containerSlotSize, UIStyle.containerSlotSize);
            }

            ItemContainerUIStorage.AddEntry(inventoryContainer.name, uiBuffer);
            inventoryUIRects = uiBuffer;
        }

        private void DrawInventory(SpriteBatch spriteBatch)
        {
            if(inventoryUIRects == null)
            {
                CalculateInventoryUI();
            }

            for(int i = 0; i < inventoryContainer.slots.Length; i++)
            {
                Rectangle rectangle = inventoryUIRects[i];
                inventoryContainer.DrawSlot(slotColor, rectangle.Location, rectangle.Width, i, spriteBatch);
            }
        }

        private void DrawToolbar(SpriteBatch spriteBatch)
        {
            int slotSize = UIStyle.containerSlotSize;
            int slotMargin = UIStyle.conatinerSlotMargin;
            for (int i = 0; i < toolbarLength; i++)
            {
                int x = i * slotSize + Margin + (slotMargin * i);
                inventoryContainer.DrawSlot(toolbarColor, new Point(x, Margin), slotSize, i, spriteBatch);
            }
        }

        private void DrawToolbarMarker(SpriteBatch spriteBatch)
        {
            int slotSize = UIStyle.containerSlotSize;
            int slotMargin = UIStyle.conatinerSlotMargin;

            int markerSizeOffset = (markerSize - slotSize) / 2;
            int x = currentToolbarIndex * slotSize + Margin + (slotMargin * currentToolbarIndex) - markerSizeOffset;
            int y = Margin - markerSizeOffset;
            spriteBatch.Draw(markerTexture, new Rectangle(new Point(x, y), new Point(markerSize)), Color.Gold);
        }

        private void DrawStackOnCursor(SpriteBatch spriteBatch)
        {
            Texture2D texture = stackOnCursor.itemType.mainTexture;
            int slotSize = UIStyle.containerSlotSize + 16;
            spriteBatch.Draw(texture, new Rectangle(CursorPosition, new Point(slotSize)), Color.White);
            spriteBatch.DrawString(TextureSet.debugFont, stackOnCursor.amount.ToString(), 
                new Vector2(CursorPosition.X + slotSize-8, CursorPosition.Y + slotSize-8), Color.White);
        }

        /// <summary>
        /// Input related to inventory (mostly mouse clicks on item slots)
        /// </summary>
        private void InventoryInput()
        {
            if (isLeftMouseButtonPressed == true && leftMouseButtonPressedLastFrame == false)
            { 
                SlotClickCheck(); 
            }
            
        }

        private void SlotClickCheck()
        {           
            for(int i = 0; i < inventoryUIRects.Length; i++)
            {
                Rectangle rectangle = inventoryUIRects[i];
                if (rectangle.Contains(CursorPosition))
                {
                    SlotClickAction(rectangle, i);
                    break;
                }
            }
        }

        private void SlotClickAction(Rectangle slotRect, int slotIndex)
        {
            Item.Stack stack = inventoryContainer.slots[slotIndex].stack;
            if (stack.itemType != null)
            {
                if (isStackOnCursor == false)
                {
                    stackOnCursor = stack;
                    inventoryContainer.RemoveItemStack(slotIndex);
                    isStackOnCursor = true;
                }
                if(isStackOnCursor == true)
                {
                    Item.Stack oldStack = stack;
                    stack = stackOnCursor;
                    stackOnCursor = stack;
                }
            }
            if(stack.itemType == null)
            {
                if(isStackOnCursor == true)
                {
                    inventoryContainer.AddItemStack(new Item.Stack(stackOnCursor.itemType, stackOnCursor.amount), slotIndex, false);
                    RemoveStackOnCursor();
                }
            }
        }

        private void RemoveStackOnCursor()
        {
            stackOnCursor.Clear();
            isStackOnCursor = false;
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
