using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.XInput;
using Microsoft.Xna.Framework.Input;

namespace Szczury
{
    public partial class PlayerGameObject : GameObject
    {
        private const bool debugMovement = false;
        private bool inventoryKeyPressedLastFrame = false;

        public PlayerGameObject(Vector2 startingPosition) : base(startingPosition)
        {
            textureBox = new Rectangle(new Point(0, 0), new Point(16, 48));
            hitbox = new Rectangle(new Point(0, 0), new Point(16, 48));
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            if(showInventory == true)
                DrawInventory(spriteBatch);
            if (showInventory == false)
                DrawToolbar(spriteBatch);

#if (debugMovement == true)
            DebugInfoDraw(spriteBatch);
#endif
        }

        public override void Start()
        {
            base.Start();

            SetPosition(TileWorld.width / 2 * Util.tileSize, 70 * Util.tileSize);

            GameplayState gs = GameState.currentState as GameplayState;
            _world = gs.tileWorld;
        }

        public override void Update(GameTime gameTime)
        {
            Physics();
            PlayerMovementInput();
            PlayerInput();
            KeyPressCheck();
            SetTileBelow();
        }

        public void PlayerInput()
        {
            KeyboardState state = Keyboard.GetState();
            if (state.IsKeyDown(Keys.E) && inventoryKeyPressedLastFrame == false)
            {
                showInventory = !showInventory;
            }
        }

        private void KeyPressCheck()
        {
            KeyboardState state = Keyboard.GetState();
            flyingCheatKeyPressedLastFrame = state.IsKeyDown(Keys.F1);
            inventoryKeyPressedLastFrame = state.IsKeyDown(Keys.E);
        }
    }
}
