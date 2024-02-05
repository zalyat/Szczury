using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Szczury
{
    public static class UIStyle
    {
		static private Texture2D _contaierSlotIcon;

		public static Texture2D ContainerSlotIcon
		{
			get { return _contaierSlotIcon; }
		}

		public static int conatinerSlotMargin => 4;
		public static int containerSlotSize => 32;

		public static void Initialize()
		{
			_contaierSlotIcon = TextureSet.GetTexture("containerSlot1");
		}
    }
}
