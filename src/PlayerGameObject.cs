using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.XInput;
using Microsoft.Xna.Framework.Input;
using Szczury.Items;

namespace Szczury
{
    public partial class PlayerGameObject : GameObject
    {
        private const bool debugMovement = false;
        private bool inventoryKeyPressedLastFrame = false;
        private bool leftMouseButtonPressedLastFrame = false;
        private int lastMouseScroll;

        private float itemChangeDelayTimer = 0.0f;
        private float itemChangeMinimumDelay = 0.1f;

        private float itemUseDelayTimer = 0f;


        public PlayerGameObject(Vector2 startingPosition, TileWorld world) : base(startingPosition, world)
        {
            textureBox = new Rectangle(new Point(0, 0), new Point(16, 48));
            hitbox = new Rectangle(new Point(0, 0), new Point(16, 48));
            markerTexture = TextureSet.GetTexture("toolbarMarker");
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            DrawToolbarMarker(spriteBatch);

            if (showInventory == true)
                DrawInventory(spriteBatch);
            if (showInventory == false)
                DrawToolbar(spriteBatch);

            //spriteBatch.Draw(TextureSet.GetTexture("Ay"), new Rectangle(Camera.OnScreen(new Point((int)MathF.Floor(Center.X), 
              //  (int)MathF.Floor(Center.Y))), new Point(16)), Color.Aqua);

            if(debugMovement == true)
                DebugInfoDraw(spriteBatch);
        }

        public override void Start()
        {
            base.Start();

            SetPosition(TileWorld.width / 2 * Util.tileSize, 70 * Util.tileSize);
            inventoryContainer.AddItemStack(new Item.Stack(ItemsRegistry.GetItem("Magic Stick"), 2), 0, true);

            GameplayState gs = GameState.currentState as GameplayState;
            _world = gs.tileWorld;
        }

        public override void Update(GameTime gameTime)
        {
            Physics();
            PlayerMovementInput();
            DoTimers();
            PlayerInput();
            KeyPressCheck();
            SetTileBelow();
        }            

        /// <summary>
        /// Update every timer
        /// </summary>
        private void DoTimers()
        {
            itemUseDelayTimer = Math.Clamp(itemUseDelayTimer + Util.deltaTime, 0, 100f);
            itemChangeDelayTimer = Math.Clamp(itemChangeDelayTimer + Util.deltaTime, 0, itemChangeMinimumDelay * 2);
        }

        /// <summary>
        /// Use this after an item was used
        /// </summary>
        public void ResetItemUseDelay()
        {
            itemUseDelayTimer = 0f;
        }
    }
}
