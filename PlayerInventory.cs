using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Szczury
{
    public partial class PlayerGameObject
    {
        public bool showInventory = false;


        private int Margin => 6;
        private Color slotColor => Color.PeachPuff;
        private Color toolbarColor => Color.Orange;

        public ItemContainer inventoryContainer = new ItemContainer("Player's container", 12);

        public int toolbarLength = 4;

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

        public void IntentoryInput()
        {

        }
    }
}
