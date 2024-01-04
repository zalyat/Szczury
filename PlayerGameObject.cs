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
        private bool flyingCheat = false;

        public float gravity = -400f;
        public float baseWalkingSpeed = 50f;
        public float baseJumpForce = 9000f;
        public TileWorld.Tile? tileBelow;/// <summary>A tile that is exactly 1 cell below the player</summary>

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

            if(flyingCheat)
            spriteBatch.DrawString(TextureSet.debugFont, $"flyingCheat enabled", new Vector2(1000, 0), Color.NavajoWhite);

            spriteBatch.DrawString(TextureSet.debugFont, $"cameraPosition: X: {Camera.cameraPosition.X}     Y: {Camera.cameraPosition.Y}", new Vector2(0, 100), Color.White);
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
            if(flyingCheat == false)
                VelocityPhysics();
        }

        public void VelocityPhysics()
        {
            position.Y -= PhysicsVelocityY;
            if (isGrounded() == false)
            {
                gravityPull += gravity * gravityPullTime * Util.deltaTime;
                if (gravityPull < gravity) gravityPull = gravity;
            }

            if (isGrounded() == true)
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

            if (state.IsKeyDown(Keys.W) && flyingCheat == true)
                position.Y -= baseWalkingSpeed * Util.deltaTime;

            if (state.IsKeyDown(Keys.Space) && isGrounded())
                Jump();
            if (state.IsKeyDown(Keys.S))
                position.Y += baseWalkingSpeed * Util.deltaTime;

            if (state.IsKeyDown(Keys.F1))
                flyingCheat = !flyingCheat;
        }

        public void Jump()
        {
            gravityPull += baseJumpForce * Util.deltaTime;
        }

        /// <summary>
        /// Center of the player's GameObject
        /// </summary>
        public Vector2 Center
        {
            get => new Vector2(position.X + 8, position.Y + 24);
        }

        public Point PositionInTiles
        {
            get => _world.WorldPositionToTilePosition(new Vector2(position.X + 8, position.Y - 8));
        }

        /// <summary>
        /// y position measured relatively to feet of the player
        /// </summary> <returns>World pos of player's feet position (2x tile pos, 3rd tile of the player)</returns>
        public Vector2 FeetPosition //y 
        {
            get => new Vector2(position.X + 8, position.Y + 32);
        }

        /// <summary>
        /// y position measured relatively to bottom of the player (Lower than FeetPosition)
        /// </summary>
        public Vector2 BottomPosition
        {
            get => new Vector2(position.X + 8, position.Y + 40);
        }

        public Point FeetPositionInTiles //change it to tiles
        {
            get => _world.WorldPositionToTilePosition(FeetPosition);
        }

        /// <summary>
        /// Simple collision check using rectangle.Intersects
        /// </summary>
        public bool isCollidingWith(Rectangle rectangle) 
        {
            return textureBox.Intersects(rectangle);
        }

        public bool isGrounded()
        {
            if (flyingCheat == true)
                return false;

            //if (tileBelow == null || tileBelow.Value.blockType == BlocksRegistry.GetBlock("Air")) return false;
            if ((tileAirCheck(position.X, BottomPosition.Y) == false
                || tileAirCheck(position.X + 8, BottomPosition.Y) == false
                || tileAirCheck(position.X + 16, BottomPosition.Y) == false)
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
            if (flyingCheat == true || tileBelow == null || tileBelow.Value.blockType == BlocksRegistry.GetBlock("Air")) return;

            if(FeetToGroundDistance < 0)
            {
                position.Y += FeetToGroundDistance;
            }
        }

        /// <summary>
        /// Distance between Feet position and next bottom tile (Warning: this doesn't include tileBelow)
        /// </summary>
        private float FeetToGroundDistance { get => (PositionInTiles.Y + 2) * Util.tileSize - FeetPosition.Y; }


        /// <summary>
        /// A faster way for doing _world.isAir(_...) for world position
        /// </summary>
        /// <returns>boolean = is tile in this world pos Air?</returns>
        private bool tileAirCheck(Vector2 atPosition) => _world.isAir(_world.WorldPositionToTilePosition(atPosition));
        /// <summary>
        /// A faster way for doing _world.isAir(_...) for world position
        /// </summary>
        /// <returns>boolean = is tile in this world pos Air?</returns>
        private bool tileAirCheck(float xPos, float yPos) => tileAirCheck(new Vector2(xPos, yPos));
    }
}
