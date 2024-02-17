using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Szczury
{
    public partial class PlayerGameObject
    {
        public void PlayerInput()
        {
            KeyboardState state = Keyboard.GetState();
            if (state.IsKeyDown(Keys.E) && inventoryKeyPressedLastFrame == false)
            {
                showInventory = !showInventory;
            }

            MouseState mState = Mouse.GetState(); //this can not be in KeyPressCheck() for some reason
            _cursorPosition = mState.Position;

            if (isLeftMouseButtonPressed && itemChangeDelayTimer > itemChangeMinimumDelay && GetCurrentItem().itemType != null)
            {
                itemChangeDelayTimer = 0f;
                GetCurrentItem().itemType.OnUse(leftMouseButtonPressedLastFrame, itemUseDelayTimer, this);
            }

            int scrollValue = mState.ScrollWheelValue;
            if (scrollValue - lastMouseScroll < 0)
            {
                currentToolbarIndex = Math.Clamp(currentToolbarIndex + 1, 0, toolbarLength - 1);
            }
            if (scrollValue - lastMouseScroll > 0)
            {
                currentToolbarIndex = Math.Clamp(currentToolbarIndex - 1, 0, toolbarLength - 1);
            }
            lastMouseScroll = mState.ScrollWheelValue;
        }


        private void KeyPressCheck()
        {
            KeyboardState state = Keyboard.GetState();
            flyingCheatKeyPressedLastFrame = state.IsKeyDown(Keys.F1);
            inventoryKeyPressedLastFrame = state.IsKeyDown(Keys.E);

            MouseState mState = Mouse.GetState();
            leftMouseButtonPressedLastFrame = mState.LeftButton == ButtonState.Pressed;
        }

        public bool isLeftMouseButtonPressed => Mouse.GetState().LeftButton == ButtonState.Pressed;

        private Point _cursorPosition;
        public Point CursorPosition => new Point(Math.Clamp(_cursorPosition.X, 0, Util.screenWidth), Math.Clamp(_cursorPosition.Y, 0, Util.screenHeight));

        public Point CursorPositionOnScreen()
        {
            return _cursorPosition;
        }
        
        public Vector2 CursorPositionToWorldPosition()
        {
            float x = Camera.cameraPosition.X + _cursorPosition.X;
            float y = Camera.cameraPosition.Y + _cursorPosition.Y;
            return new Vector2(x, y);
        }

        public Point CursorPositionToTilePosition()
        {
            Point point = TileWorld.WorldPositionToTilePosition(CursorPositionToWorldPosition());
            return new Point(point.X, point.Y);
        }
    }
}
