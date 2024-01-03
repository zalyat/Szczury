using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

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
            spriteBatch.Draw(mainTexture, new Rectangle(
                new Point(textureBox.X - (int)MathF.Ceiling(Camera.cameraPosition.X),
                          textureBox.Y - (int)MathF.Ceiling(Camera.cameraPosition.Y)
                ), new Point(textureBox.Width, textureBox.Height)), Color.Magenta);

            //Debug.WriteLine($"{textureBox.X + (int)MathF.Ceiling(Camera.cameraPosition.X)}   |   {textureBox.Y + (int)MathF.Ceiling(Camera.cameraPosition.Y)}");
        }

        public virtual void Update(GameTime gameTime)
        {

        }
    }
}
