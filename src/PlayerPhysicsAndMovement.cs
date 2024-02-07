using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Szczury.Blocks;

namespace Szczury
{
    public partial class PlayerGameObject : GameObject
    {
        public float gravity = -500f;
        private float gravityPull = 0f;
        private float gravityPullTime = 0.6f;
        public float baseWalkingSpeed = 50f;
        public float baseJumpForce = 18000f;
        public float maximumWalkingSpeed = 200f;

        public int walkingInput = 0;
        public float walkingVelocity = 0f;
        private float walkingVelocityDecrease = 7f;
        private float acceleration = 4f;

        private TileWorld _world;
        public TileWorld.Tile? tileBelow;/// <summary>A tile that is exactly 1 cell below the player</summary>



        private bool flyingCheat = false;
        private bool flyingCheatKeyPressedLastFrame = false;




        public float FinalWalkingSpeed
        {
            get => baseWalkingSpeed;
        }
        

        public void DebugInfoDraw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(TextureSet.debugFont,
                $"X:{Position.X}   Y:{Position.Y}      |      worldPos: X: {PositionInTiles.X}   Y: {PositionInTiles.Y}",
                new Vector2(0, 0), Color.NavajoWhite);
            TileWorld.Chunk c = _world.GetChunkAtTilePosition(PositionInTiles);
            spriteBatch.DrawString(TextureSet.debugFont, $"chunkPos: i: {c.i} j: {c.j}", new Vector2(150, 20), Color.NavajoWhite);
            spriteBatch.DrawString(TextureSet.debugFont, $"isGrounded: {isGrounded()}", new Vector2(0, 20), Color.NavajoWhite);

            if (tileBelow != null)
                spriteBatch.DrawString(TextureSet.debugFont, $"tileBelow: {tileBelow.Value.blockType.Name}", new Vector2(0, 40), Color.NavajoWhite);

            spriteBatch.DrawString(TextureSet.debugFont, $"FTGD: {FeetToGroundDistance}", new Vector2(500, 0), Color.NavajoWhite);

            spriteBatch.DrawString(TextureSet.debugFont, $"gravityPull: {gravityPull}", new Vector2(500, 20), Color.NavajoWhite);

            spriteBatch.DrawString(TextureSet.debugFont, $"PhysicsVelocityY: {PhysicsVelocityY}", new Vector2(500, 40), Color.NavajoWhite);
            spriteBatch.DrawString(TextureSet.debugFont, $"PhysicsVelocityX: {PhysicsVelocityX}", new Vector2(300, 40), Color.NavajoWhite);

            if (flyingCheat)
                spriteBatch.DrawString(TextureSet.debugFont, $"flyingCheat enabled", new Vector2(1000, 0), Color.NavajoWhite);

            spriteBatch.DrawString(TextureSet.debugFont, $"cameraPosition: X: {Camera.cameraPosition.X}     Y: {Camera.cameraPosition.Y}", new Vector2(0, 100), Color.White);
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
            float increase = 1f;
            if (isGrounded() == false) increase = 0.2f;

            if (walkingInput != 0)
            {
                walkingVelocity = MathHelper.SmoothStep(walkingVelocity, walkingInput, acceleration * Util.deltaTime * increase);
            }
            else walkingVelocity = MathHelper.SmoothStep(walkingVelocity, 0, walkingVelocityDecrease * Util.deltaTime);

            Math.Clamp(walkingVelocity, -1f, 1f);

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

        public float PhysicsVelocityY
        {
            get
            {
                float output = 0f;
                output += gravityPull * Util.deltaTime;
                return output;
            }
        }

        public float PhysicsVelocityX
        {
            get
            {
                float output = 0f;
                output = Math.Clamp(MathHelper.Lerp(0, walkingVelocity, acceleration) * baseWalkingSpeed, -maximumWalkingSpeed, maximumWalkingSpeed);
                return output;
            }
        }

        public void PlayerMovementInput() //player input that will influence movement and position
        {
            KeyboardState state = Keyboard.GetState();         

            if (state.IsKeyDown(Keys.D))
                walkingInput = 1;
            else if (state.IsKeyDown(Keys.A))
                walkingInput = -1;
            else walkingInput = 0; 

            if (state.IsKeyDown(Keys.W) && flyingCheat == true)
                Move(0f, baseWalkingSpeed * Util.deltaTime);

            if (state.IsKeyDown(Keys.Space) && isGrounded())
                Jump();
            if (state.IsKeyDown(Keys.S) && flyingCheat == true)
                Move(0f, baseWalkingSpeed * Util.deltaTime);

            if (state.IsKeyDown(Keys.F1) && flyingCheatKeyPressedLastFrame == false)
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
            get => new Vector2(Position.X + 8, Position.Y + 24);
        }

        public Point PositionInTiles
        {
            get => TileWorld.WorldPositionToTilePosition(new Vector2(Position.X + 8, Position.Y + 8));
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
            get => new Vector2(Position.X + 8, Position.Y + 48);
        }

        public Point FeetPositionInTiles //change it to tiles
        {
            get => TileWorld.WorldPositionToTilePosition(FeetPosition);
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
            if ((tileAirCheck(Position.X + 1, BottomPosition.Y+1) == false
                || tileAirCheck(Position.X + 8, BottomPosition.Y+1) == false
                || tileAirCheck(Position.X + 15, BottomPosition.Y+1) == false) //leave 1 pixel margin to prevent sticking to walls
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

                    if(point.Y == PositionInTiles.Y + 2)
                    {
                        if (stepCheck(point) == true && walkingInput != 0) // (only if player is giving some input)
                        {
                            if (PositionInTiles.X < point.X)
                                Step(1);
                            if (PositionInTiles.X > point.X)
                                Step(-1);

                            continue;
                        }
                    }

                    if (PositionInTiles.Y > point.Y)
                    {
                        gravityPull = 0f;
                        Move(0f, rect.Height);
                        continue;
                    }
                    if (PositionInTiles.X < point.X)
                    {
                        Move(-rect.Width, 0f);
                        walkingVelocity = 0f; // temp
                    }
                    if (PositionInTiles.X > point.X)
                    {
                        Move(rect.Width, 0f);
                        walkingVelocity = 0f; // temp
                    }
                }
        }

        /// <summary>
        /// Step on a higher tile
        /// TO DO: Smoothing
        /// </summary>
        /// <param name="dir">-1 left, 1 right</param>
        private void Step(int dir)
        {
            Move(0f, -Util.tileSize - 1f);
            walkingVelocity += 0.2f * dir;
        }

        /// <summary>
        /// Check if the player can step on the higher tile
        /// </summary>
        private bool stepCheck(Point point)
        {
            //check if the space above the point is free
            if (_world.isAir(new Point(point.X, point.Y - 1)) == true
                && _world.isAir(new Point(point.X, point.Y - 2)) == true
                && _world.isAir(new Point(point.X, point.Y - 3)) == true)
                return true;

            return false;
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

            if (FeetToGroundDistance < 0)
            {
                Debug.WriteLine("Our world");
                Move(0f, MathF.Ceiling(FeetToGroundDistance));
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
        private bool tileAirCheck(Vector2 atPosition) => _world.isAir(TileWorld.WorldPositionToTilePosition(atPosition));
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
    }
}