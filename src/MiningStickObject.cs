using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Szczury
{
    public class MiningStickObject : GameObject
    {
        /// <summary>
        /// Use this in GameplayState.Main to make the object be able to appear in the world
        /// </summary>
        public MiningStickObject(Vector2 startingPosition, Vector2 targetPosition, TileWorld world, float useDelay) : base(startingPosition, world)
        {
            mainTexture = TextureSet.GetTexture("mining_stick_object");
            _targetPosition = targetPosition;
            _halfAnimLength = useDelay / 2;
        }

        private float _animTimer = 0f;
        private float _maxAnimTime = 1f;
        private float _halfAnimLength;
        private Vector2 _targetPosition;

        public override void Start()
        {            

        }

        public override void Update(GameTime gameTime)
        {
            if (_animTimer > _maxAnimTime)
            {
                GameplayState.Main.DeleteGameObject(this);
            }
            _animTimer += Util.deltaTime / _halfAnimLength;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Vector2 origin = new Vector2(16f, 4f);
            Vector2 offset = new Vector2(8f, 16f);
            Vector2 delta = _targetPosition - (Position + offset);
            Vector2 direction = Vector2.Normalize(delta);
            Vector2 position = Position + delta * (-MathF.Abs(_animTimer*2 - _maxAnimTime) + _maxAnimTime);
            float rotation = -MathF.Atan2(direction.X, direction.Y) + MathHelper.PiOver2;
            spriteBatch.Draw(mainTexture, new Rectangle(Camera.OnScreen(position.ToPoint(offset)), new Point(64, 8)), null, Color.White, rotation, origin, SpriteEffects.None, 1f);
        }
    }
}
