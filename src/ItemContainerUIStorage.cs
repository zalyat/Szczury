using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Szczury
{
    /// <summary>
    /// Instead of calculating slot UI positions every frame for every container, just save the container's type name and calculated Rectangles for slots
    /// Also useful for input operations (Rectangle.Contains())
    /// </summary>
    public static class ItemContainerUIStorage
    {
        private static Dictionary<string, Rectangle[]> uiStorage = new Dictionary<string, Rectangle[]>();

        public static void AddEntry(string name, Rectangle[] slotInfo)
        {
            if (uiStorage.ContainsKey(name) == true) return;
            uiStorage.Add(name, slotInfo);
        }

        public static Rectangle[] GetEntry(string name)
        {
            Rectangle[] output = null;
            uiStorage.TryGetValue(name, out output);
            return output;
        }
    }
}
