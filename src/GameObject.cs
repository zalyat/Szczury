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
        private Vector2 position;
        public Rectangle textureBox;
        public Rectangle hitbox;
        protected Texture2D mainTexture;
        public TileWorld currentWorld;
        public bool drawable = true;

        public GameObject(Vector2 startingPosition, TileWorld world)
        {
            position = startingPosition;
            currentWorld = world;
            Start();
        }

        public virtual void Start()
        {
            mainTexture = TextureSet.GetTexture("default");
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (drawable == false) return;

            textureBox.X = (int)Math.Floor(position.X);
            textureBox.Y = (int)Math.Floor(position.Y);
            spriteBatch.Draw(mainTexture, new Rectangle(
                Camera.OnScreen(new Point(textureBox.X, textureBox.Y)),
                new Point(textureBox.Width, textureBox.Height)), Color.Magenta);
        }

        public virtual void Update(GameTime gameTime)
        {

        }

        private void UpdateHitboxPosition()
        {
            hitbox.X = (int)MathF.Floor(Position.X);
            hitbox.Y = (int)MathF.Floor(Position.Y);
        }


        public virtual void Move(Vector2 vector)
        {
            position.X += vector.X;
            position.Y += vector.Y;
            UpdateHitboxPosition();
        }

        public virtual void Move(float X, float Y)
        {
            position.X += X;
            position.Y += Y;
            UpdateHitboxPosition();
        }


        public virtual void SetPosition(Vector2 vector)
        {
            position.X = vector.X;
            position.Y = vector.Y;
            UpdateHitboxPosition();
        }

        public virtual void SetPosition(float X, float Y)
        {
            position.X = X;
            position.Y = Y;
            UpdateHitboxPosition();
        }

        public virtual void Destroy()
        {
            GameplayState.Main.DeleteGameObject(this);
        }

        /// <summary>
        /// Public property
        /// </summary>
        public Vector2 Position
        {
            get => position; 
        }
        public Vector2 Center
        {
            get => new Vector2(position.X + textureBox.Width/2, position.Y + textureBox.Height/2);
        }
    }
}
