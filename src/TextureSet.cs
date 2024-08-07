﻿using System;
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
            Add("tile_damage", "tile_damage");
            Add("dirt_block", "tile_dirt");
            Add("basalt_block", "basalt_block");

            Add("containerSlot1", "containerSlot1");

            Add("item_mining_stick", "mining_stick_item");
            Add("mining_stick_object", "mining_stick_object");
            Add("item_dirt", "item_dirt");
            Add("item_basalt", "item_basalt");

            Add("toolbarMarker", "toolbarMarker");

            Add("monster_toxic_snail", "monster_toxic_snail");

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
                output = GetTexture("default"); //return error texture
            }
            return output;
        }
    }
}
