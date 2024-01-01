using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Szczury
{
    public class PlayerGameObject : GameObject
    {
        public float gravity = -400f;
        public float baseWalkingSpeed = 200f;

        public float FinalWalkingSpeed
        {
            get => baseWalkingSpeed;
        }



        public PlayerGameObject(Vector2 startingPosition) : base(startingPosition)
        {
            textureBox = new Rectangle(new Point(0, 0), new Point(16, 48));
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
        public override void Start()
        {
            base.Start();
        }

        public override void Update(GameTime gameTime)
        {
            Physics();
            PlayerInput();
        }

        public void Physics()
        {
            position.Y -= PhysicsVelocityY;
        }

        public float PhysicsVelocityY { get => gravity * Util.deltaTime; }

        public void PlayerInput()
        {
            KeyboardState state = Keyboard.GetState();

            if (state.IsKeyDown(Keys.D))
                position.X += baseWalkingSpeed * Util.deltaTime;
            if (state.IsKeyDown(Keys.A))
                position.X -= baseWalkingSpeed * Util.deltaTime;
        }
    }
}
