using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace Szczury
{
    static public class TextureSet
    {
        private static Dictionary<string, Texture2D> textures = new Dictionary<string, Texture2D>();
        private static ContentManager contentManager;

        public static SpriteFont debugFont;

        public static void Initialize(ContentManager contentManager)
        {
            TextureSet.contentManager = contentManager;
            TextureSet.debugFont = contentManager.Load<SpriteFont>("debugFont");

            Add("default", "default");
            Add("dirt_block", "tile_dirt");

        }

        private static void Add(string name, string pipepath)
        {
            Texture2D texture;
            try
            {
                texture = contentManager.Load<Texture2D>(pipepath);
                textures.Add(name, texture);
            }
            catch(Exception e)
            {
                Debug.WriteLine($"TEXTURE-SET ------------>{e}");
            }
        }

        public static Texture2D GetTexture(string name)
        {
            Texture2D output = null;
            try
            {
                output = textures[name];
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
            return output;
        }
    }
}
