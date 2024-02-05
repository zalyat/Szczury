using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Szczury
{
    public interface IState
    {
        public void Initialize(ContentManager content);
        public void Start();
        public void Update(GameTime gameTime);
        public void Draw(SpriteBatch _spriteBatch);
    }
}
