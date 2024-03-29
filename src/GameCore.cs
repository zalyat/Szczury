﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Szczury.Core
{
    public class GameCore : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public GameCore()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();

            _graphics.IsFullScreen = false;
            _graphics.PreferredBackBufferWidth = /*640*/ Util.screenWidth;
            _graphics.PreferredBackBufferHeight = /*480*/ Util.screenHeight;
            _graphics.ApplyChanges();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            TextureSet.Initialize(Content);
            UIStyle.Initialize();
            GameState.ChangeState(new GameplayState(_spriteBatch, GraphicsDevice));
        }

        protected override void Update(GameTime gameTime)
        {
            Util.gameTime = gameTime;
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            GameState.currentState.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
            GameState.currentState.Draw(_spriteBatch);
            _spriteBatch.DrawString(TextureSet.debugFont, $"fps:{1 / gameTime.ElapsedGameTime.TotalSeconds}", new Vector2(1200, 0), Color.Gainsboro);
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
