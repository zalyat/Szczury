using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Szczury
{
    public class GameObject
    {
        public Vector2 position;
        public Rectangle textureBox;
        protected Texture2D mainTexture;

        public GameObject(Vector2 startingPosition)
        {
            position = startingPosition;
            Start();
        }

        public virtual void Start()
        {
            mainTexture = TextureSet.GetTexture("default");
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            textureBox.X = (int)Math.Floor(position.X);
            textureBox.Y = (int)Math.Floor(position.Y);
            spriteBatch.Draw(mainTexture, textureBox, Color.Magenta);
        }

        public virtual void Update(GameTime gameTime)
        {

        }
    }
}
