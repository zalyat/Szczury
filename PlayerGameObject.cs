using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Szczury
{
    public partial class PlayerGameObject : GameObject
    {
        public PlayerGameObject(Vector2 startingPosition) : base(startingPosition)
        {
            textureBox = new Rectangle(new Point(0, 0), new Point(16, 48));
            hitbox = new Rectangle(new Point(0, 0), new Point(16, 48));
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            DebugInfoDraw(spriteBatch);

        }

        public override void Start()
        {
            base.Start();

            SetPosition(TileWorld.width / 2 * Util.tileSize, 288);

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
    }
}
