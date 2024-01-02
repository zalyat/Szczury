using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Szczury.Blocks;

namespace Szczury
{
    public class PlayerGameObject : GameObject
    {
        public float gravity = -400f;
        public float baseWalkingSpeed = 50f;
        public float baseJumpForce = 9000f;
        public TileWorld.Tile? tileBelow;

        public float FinalWalkingSpeed
        {
            get => baseWalkingSpeed;
        }

        public PlayerGameObject(Vector2 startingPosition) : base(startingPosition)
        {
            textureBox = new Rectangle(new Point(0, 0), new Point(16, 48));
        }

        private TileWorld _world;
        private float gravityPull = 0f;
        private float gravityPullTime = 0.3f;






        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            DebugInfoDraw(spriteBatch);
            
        }

        public void DebugInfoDraw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(TextureSet.debugFont, 
                $"X:{position.X}   Y:{position.Y}      |      worldPos: X: {PositionInTiles.X}   Y: {PositionInTiles.Y}",
                new Vector2(0, 0), Color.NavajoWhite);
            spriteBatch.DrawString(TextureSet.debugFont, $"isGrounded: {isGrounded()}", new Vector2(0, 20), Color.NavajoWhite);

            if(tileBelow != null)
            spriteBatch.DrawString(TextureSet.debugFont, $"tileBelow: {tileBelow.Value.blockType.Name}", new Vector2(0, 40), Color.NavajoWhite);

            spriteBatch.DrawString(TextureSet.debugFont, $"FTGD: {FeetToGroundDistance}", new Vector2(500, 0), Color.NavajoWhite);

            spriteBatch.DrawString(TextureSet.debugFont, $"gravityPull: {gravityPull}", new Vector2(500, 20), Color.NavajoWhite);

            spriteBatch.DrawString(TextureSet.debugFont, $"PhysicsVelocityY: {PhysicsVelocityY}", new Vector2(500, 40), Color.NavajoWhite);
        }

        public override void Start()
        {
            base.Start();

            position = new Vector2(0, 288);

            GameplayState gs = GameState.currentState as GameplayState;
            _world = gs.tileWorld;
            _world.ChangeTile(new Point(PositionInTiles.X + 10, PositionInTiles.Y + 2), BlocksRegistry.GetBlock("Dirt"));
            _world.ChangeTile(new Point(PositionInTiles.X + 9, PositionInTiles.Y + 1), BlocksRegistry.GetBlock("Dirt"));
            _world.ChangeTile(new Point(PositionInTiles.X + 8, PositionInTiles.Y), BlocksRegistry.GetBlock("Dirt"));
        }

        public override void Update(GameTime gameTime)
        {            
            Physics();
            TileBelowUnstick();
            PlayerInput();
            SetTileBelow();
            //_world.ChangeTile(new Point(PositionInTiles.X, PositionInTiles.Y + 3), BlocksRegistry.GetBlock("Air"));
        }

        public void Physics()
        {
            position.Y -= PhysicsVelocityY;
            if (isGrounded() == false)
            {
                gravityPull += gravity * gravityPullTime * Util.deltaTime;
                if (gravityPull < gravity) gravityPull = gravity;
            }

            if(isGrounded() == true)
            {
                gravityPull = 0;
            }

        }

        public float PhysicsVelocityY { get {
                float output = 0f;
                output += gravityPull * Util.deltaTime;
                return output; } }


        public void PlayerInput() //player input that will influence movement and position
        {
            KeyboardState state = Keyboard.GetState();

            if (state.IsKeyDown(Keys.D))
                position.X += baseWalkingSpeed * Util.deltaTime;
            if (state.IsKeyDown(Keys.A))
                position.X -= baseWalkingSpeed * Util.deltaTime;

            /*if (state.IsKeyDown(Keys.W))
                position.Y -= baseWalkingSpeed * Util.deltaTime;*/

            if (state.IsKeyDown(Keys.Space) && isGrounded())
                Jump();
            if (state.IsKeyDown(Keys.S))
                position.Y += baseWalkingSpeed * Util.deltaTime;
        }

        public void Jump()
        {
            gravityPull += baseJumpForce * Util.deltaTime;
        }



        public Point PositionInTiles
        {
            get => _world.WorldPositionToTilePosition(new Vector2(position.X + 8, position.Y - 8));
        }

        public Vector2 FeetPosition //y position measured relatively to bottom of the player
        {
            get => new Vector2(position.X + 8, position.Y + 32);
        }

        public Vector2 BottomPosition //y position measured relatively to bottom of the player
        {
            get => new Vector2(position.X + 8, position.Y + 40);
        }

        public Point FeetPositionInTiles //change it to tiles
        {
            get => _world.WorldPositionToTilePosition(FeetPosition);
        }

        public bool isCollidingWith(Rectangle rectangle) //simple check
        {
            return textureBox.Intersects(rectangle);
        }

        public bool isGrounded()
        {
            //if (tileBelow == null || tileBelow.Value.blockType == BlocksRegistry.GetBlock("Air")) return false;
            if ((_world.isAir(_world.WorldPositionToTilePosition(new Vector2(position.X, BottomPosition.Y))) == false
                || _world.isAir(_world.WorldPositionToTilePosition(new Vector2(position.X + 8, BottomPosition.Y))) == false
                || _world.isAir(_world.WorldPositionToTilePosition(new Vector2(position.X + 16, BottomPosition.Y))) == false)
                && (FeetToGroundDistance < 0.1f || FeetToGroundDistance < 0))
                return true;

            return false;
        }

        public void SetTileBelow() // get tile that is below the player
        {
            Point tileBelowCoord;
            tileBelowCoord.X = (int)PositionInTiles.X;
            tileBelowCoord.Y = (int)PositionInTiles.Y + 3;
            tileBelow = _world.GetTile(tileBelowCoord);
        }

        public void TileBelowUnstick()
        {
            if (tileBelow == null || tileBelow.Value.blockType == BlocksRegistry.GetBlock("Air")) return;

            if(FeetToGroundDistance < 0)
            {
                position.Y += FeetToGroundDistance;
            }
        }

        private float FeetToGroundDistance { get => (PositionInTiles.Y + 2) * Util.tileSize - FeetPosition.Y; }
    }
}
