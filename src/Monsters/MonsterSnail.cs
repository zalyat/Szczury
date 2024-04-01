using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Szczury.Monsters
{
    public abstract class MonsterSnail : Monster
    {
        public Direction direction;
        public Direction bottom;
        public virtual float Speed => 10.0f;

        protected bool fallingDown = false;
        protected bool wasInAirPrevStep = false;
        protected float wanderTimer = 0f;
        protected float wanderDelay = 0.5f;
        protected float movingSpeed;
        protected float rotation = 0f;
        protected SpriteEffects spriteEffects = SpriteEffects.None;

        public bool invertedDirection;
        
        private Point nextTile;
        private Vector2 nextTilePos;

        public MonsterSnail(Vector2 startingPosition, TileWorld world, bool invertedDirection) : base(startingPosition, world)
        {
            textureBox = new Rectangle(new Point(0, 0), new Point(16, 16));

            movingSpeed = 1 / wanderDelay;
            this.invertedDirection = invertedDirection;
            if (invertedDirection == true)
            {
                direction = Direction.Left;
            }
            else direction = Direction.Right;
            bottom = Direction.Down;
        }

        public override void Start()
        {
            base.Start();                    
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            wanderTimer += Util.deltaTime;

            if (wanderTimer >= wanderDelay)
            {
                Wander();
                wanderTimer = 0f;
                UpdateRotationInfo();
            }

            /*if (currentWorld.isInWorldBoundaries(nextTile))
            {
                //MoveToNextTile();
            }*/
        }

        protected virtual void Wander()
        {
            if (currentWorld.hasTileNeighbours(PositionInTiles) == false)
            {
                if (fallingDown == false && wasInAirPrevStep == true)
                {
                    fallingDown = true;
                    FallDown();
                    return;
                }
                wasInAirPrevStep = true;
            }
            else wasInAirPrevStep = false;

            if(fallingDown == true)
            {
                Point nextTileLocation = PositionInTiles + new Point(0, 1);
                if(currentWorld.isInWorldBoundaries(nextTileLocation) == false)
                {
                    Destroy();
                }
                if (currentWorld.isAir(nextTileLocation) == false)
                {
                    fallingDown = false;
                    //ChangeDirection(Direction.Left);
                    return;
                }
                FallDown();
                return;
            }

            if (isNextTileFree())
            {
                Step();
            }
            else
            {
                ChangeDirection(bottom.Reverse(), direction);
            }
            
            if(isBottomTileFree())
            {
                ChangeDirection(bottom, direction.Reverse());
            }
        }

        private void ChangeDirection(Direction dir, Direction bot)
        {
            bottom = bot;
            direction = dir;
        }

        private void FallDown()
        {
            SetPosition(currentWorld.TilePositionToWorldPosition(PositionInTiles + new Point(0, 1)));
        }

        private bool isNextTileFree()
        {
            return currentWorld.isAir(PositionInTiles + direction.Vector().ToPoint());
        }
        private bool isBottomTileFree()
        {
            return currentWorld.isAir(PositionInTiles + bottom.Vector().ToPoint());
        }

        protected virtual void Step()
        {
            //Vector2 velocity = direction.Vector() * Speed * Util.deltaTime;
            //Move(velocity);
            //SetPosition(currentWorld.TilePositionToWorldPosition(PositionInTiles + direction.Vector().ToPoint()));
            nextTile = PositionInTiles + direction.Vector().ToPoint();
            nextTilePos = Util.TilePosToWorldPos(nextTile);
            MoveToNextTile();
        }

        protected virtual void MoveToNextTile()
        {
            SetPosition(currentWorld.TilePositionToWorldPosition(nextTile));
        }

        private void UpdateRotationInfo()
        {
            spriteEffects = SpriteEffects.None;
            if (invertedDirection == true) spriteEffects = SpriteEffects.FlipHorizontally;
            if (bottom == Direction.Left)
                rotation = MathHelper.ToRadians(90);

            if (bottom == Direction.Right)
                rotation = MathHelper.ToRadians(-90);

            if (bottom == Direction.Up)
                rotation = MathHelper.ToRadians(180);

            if (bottom == Direction.Down)
                rotation = MathHelper.ToRadians(0);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            textureBox.X = (int)Math.Floor(Position.X);
            textureBox.Y = (int)Math.Floor(Position.Y);            
            

            Rectangle rect = new Rectangle(Camera.OnScreen(new Point(textureBox.X + 8, textureBox.Y + 8)), new Point(textureBox.Width, textureBox.Height));

            spriteBatch.Draw(mainTexture, rect, null, Color.White, rotation, new Vector2(8, 8), spriteEffects, 1f);
            //spriteBatch.DrawString(TextureSet.debugFont, $"{direction} {bottom} {fallingDown} {isNextTileFree()} {isBottomTileFree()} {wasInAirPrevStep}", Camera.OnScreen(Position) + new Vector2(-10f, -15f), Color.BlueViolet);
        }

        public Point PositionInTiles => TileWorld.WorldPositionToTilePosition(Center);
    }
}
