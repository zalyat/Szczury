using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Szczury.Items
{
    public class ItemsRegistry
    {
        private static List<Item> items = new List<Item>();
        public static void AddItem(Item item, string textureName)
        {
            item.mainTexture = TextureSet.GetTexture(textureName);
            items.Add(item);
        }

        public static void AddItem(Item item)
        {
            items.Add(item);
        }

        public static Item GetItem(string name)
        {
            return items.First(x => x.Name == name);
        }
    }
}
