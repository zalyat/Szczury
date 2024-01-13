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
        public PlayerGameObject(Vector2 startingPosition) : base(startingPosition)
        {
            textureBox = new Rectangle(new Point(0, 0), new Point(16, 48));
            hitbox = new Rectangle(new Point(0, 0), new Point(16, 48));
        }

        public float gravity = -400f;
        private float gravityPull = 0f;
        private float gravityPullTime = 0.3f;
        public float baseWalkingSpeed = 50f;
        public float baseJumpForce = 9000f;

        public float walkingInput = 0f;
        private float walkingInputDecrease = 7f;
        private float acceleration = 2f;

        private TileWorld _world;
        public TileWorld.Tile? tileBelow;/// <summary>A tile that is exactly 1 cell below the player</summary>

       

        private bool flyingCheat = false;
        private bool flyingCheatKeyPressedLastFrame = false;




        public float FinalWalkingSpeed
        {
            get => baseWalkingSpeed;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            DebugInfoDraw(spriteBatch);
            
        }

        public void DebugInfoDraw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(TextureSet.debugFont, 
                $"X:{Position.X}   Y:{Position.Y}      |      worldPos: X: {PositionInTiles.X}   Y: {PositionInTiles.Y}",
                new Vector2(0, 0), Color.NavajoWhite);
            spriteBatch.DrawString(TextureSet.debugFont, $"isGrounded: {isGrounded()}", new Vector2(0, 20), Color.NavajoWhite);

            if(tileBelow != null)
            spriteBatch.DrawString(TextureSet.debugFont, $"tileBelow: {tileBelow.Value.blockType.Name}", new Vector2(0, 40), Color.NavajoWhite);

            spriteBatch.DrawString(TextureSet.debugFont, $"FTGD: {FeetToGroundDistance}", new Vector2(500, 0), Color.NavajoWhite);

            spriteBatch.DrawString(TextureSet.debugFont, $"gravityPull: {gravityPull}", new Vector2(500, 20), Color.NavajoWhite);

            spriteBatch.DrawString(TextureSet.debugFont, $"PhysicsVelocityY: {PhysicsVelocityY}", new Vector2(500, 40), Color.NavajoWhite);
            spriteBatch.DrawString(TextureSet.debugFont, $"PhysicsVelocityX: {PhysicsVelocityX}", new Vector2(300, 40), Color.NavajoWhite);

            if (flyingCheat)
            spriteBatch.DrawString(TextureSet.debugFont, $"flyingCheat enabled", new Vector2(1000, 0), Color.NavajoWhite);

            spriteBatch.DrawString(TextureSet.debugFont, $"cameraPosition: X: {Camera.cameraPosition.X}     Y: {Camera.cameraPosition.Y}", new Vector2(0, 100), Color.White);
        }

        public override void Start()
        {
            base.Start();

            SetPosition(TileWorld.width/2 * Util.tileSize, 288);

            GameplayState gs = GameState.currentState as GameplayState;
            _world = gs.tileWorld;
        }

        public override void Update(GameTime gameTime)
        {
            Physics();
            PlayerInput();
            KeyPressCheck();
            SetTileBelow();
        }

        public void Physics()
        {
            if (flyingCheat == false)
            {
                VelocityPhysics();
                CollisionDetection();
                TileBelowUnstuck();               
            }
        }

        public void VelocityPhysics()
        {
            Move(PhysicsVelocityX * Util.deltaTime, 0f);
            Move(0f, -PhysicsVelocityY);
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

        public float PhysicsVelocityX { get
            {
                float output = 0f;
                output = MathHelper.Lerp(0, walkingInput, acceleration) * baseWalkingSpeed;
                return output;
            } }

        public void PlayerInput() //player input that will influence movement and position
        {
            KeyboardState state = Keyboard.GetState();

            if (state.IsKeyDown(Keys.D))
                walkingInput += 1f * Util.deltaTime;
            else if (state.IsKeyDown(Keys.A))
                walkingInput -= 1f * Util.deltaTime;
            else walkingInput = MathHelper.SmoothStep(walkingInput, 0f, walkingInputDecrease * Util.deltaTime); //deacceleration probably shouldn't be here (it shouldn't directly affect walkingInput)

            if (state.IsKeyDown(Keys.W) && flyingCheat == true)
                Move(0f, baseWalkingSpeed * Util.deltaTime);

            if (state.IsKeyDown(Keys.Space) && isGrounded())
                Jump();
            if (state.IsKeyDown(Keys.S) && flyingCheat == true)
                Move(0f, baseWalkingSpeed * Util.deltaTime);

            if (state.IsKeyDown(Keys.F1) && flyingCheatKeyPressedLastFrame == false)
                flyingCheat = !flyingCheat;

            Math.Clamp(walkingInput, -1f, 1f);
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
            get => new Vector2(Position.X + 8, Position.Y + 24);
        }

        public Point PositionInTiles
        {
            get => _world.WorldPositionToTilePosition(new Vector2(Position.X + 8, Position.Y - 8));
        }

        /// <summary>
        /// y position measured relatively to feet of the player
        /// </summary> <returns>World pos of player's feet position (2x tile pos, 3rd tile of the player)</returns>
        public Vector2 FeetPosition //y 
        {
            get => new Vector2(Position.X + 8, Position.Y + 32);
        }

        /// <summary>
        /// y position measured relatively to bottom of the player (Lower than FeetPosition)
        /// </summary>
        public Vector2 BottomPosition
        {
            get => new Vector2(Position.X + 8, Position.Y + 40);
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
            return hitbox.Intersects(rectangle);
        }

        /// <summary>
        /// Tile variant. Will return 'false' if the tile is air type
        /// </summary>
        /// <param name="location">Tile position</param>
        private bool isCollidingWith(Point location)
        {

            if (isCollidingWith(_world.GetTileRectangle(location)) == true && tileAirCheck(location) == false)
                return true;

            return false;
        }

        public bool isGrounded()
        {
            if (flyingCheat == true)
                return false;

            //if (tileBelow == null || tileBelow.Value.blockType == BlocksRegistry.GetBlock("Air")) return false;
            if ((tileAirCheck(Position.X+1, BottomPosition.Y) == false
                || tileAirCheck(Position.X + 8, BottomPosition.Y) == false
                || tileAirCheck(Position.X + 15, BottomPosition.Y) == false) //leave 1 pixel margin to prevent sticking to walls
                && (FeetToGroundDistance < 0.1f || FeetToGroundDistance < 0))
                return true;

            return false;
        }

        /// <summary>
        /// Detect collision and unstuck the player from the wall/ceiling
        /// TO DO:
        /// Fix ceiling unstucking
        /// </summary>
        private void CollisionDetection() //collision check with walls and ceiling.
        {
            for (int y = -1; y < 3; y++) // off by 1 included
                for (int x = -2; x < 3; x++) // off by 1 included
                {
                    Point point = new Point(PositionInTiles.X + x, PositionInTiles.Y + y);
                    if (_world.isInWorldBoundaries(point) == false) continue; //must be here, because of TilePositionToWorldPosition() from TileWorld.cs
                    if (isCollidingWith(point) == false) continue;

                    Rectangle rect = Rectangle.Intersect(hitbox, _world.GetTileRectangle(point));

                    if (PositionInTiles.Y > point.Y)
                    {
                        gravityPull = 0f;
                        Move(0f, rect.Height);
                        continue;
                    }
                    if (PositionInTiles.X < point.X)
                    {
                        Move(-rect.Width, 0f);
                        walkingInput = 0f; // temp
                    }
                    if (PositionInTiles.X > point.X)
                    {
                        Move(rect.Width, 0f);
                        walkingInput = 0f; // temp
                    }                   
                }
        }       

        private void SetTileBelow() // get tile that is below the player
        {
            Point tileBelowCoord;
            tileBelowCoord.X = PositionInTiles.X;
            tileBelowCoord.Y = PositionInTiles.Y + 3;
            tileBelow = _world.GetTile(tileBelowCoord);
        }

        private void TileBelowUnstuck()
        {
            if (tileBelow == null || tileBelow.Value.blockType == BlocksRegistry.GetBlock("Air")) return;

            if(FeetToGroundDistance < 0)
            {
                Move(0f, FeetToGroundDistance);
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
        /// <summary>
        /// Tile variant
        /// </summary>
        /// <returns></returns>
        private bool tileAirCheck(Point location) => _world.isAir(location);

        private void KeyPressCheck()
        {
            KeyboardState state = Keyboard.GetState();
            flyingCheatKeyPressedLastFrame = state.IsKeyDown(Keys.F1);
        }
    }
}
