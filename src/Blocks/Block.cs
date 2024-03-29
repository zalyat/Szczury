﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Szczury.Blocks
{
    public abstract class Block
    {
        public Texture2D mainTexture;
        public abstract string Name { get; }
        public abstract float Hardness { get; }
        public abstract float MaxDamage { get; }
        public abstract Color ColorRepresentation { get; }

        public virtual void OnBreak(Point location, TileWorld world)
        {

        }
    }
}
