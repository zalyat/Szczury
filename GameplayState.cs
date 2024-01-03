using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Szczury
{
    public partial class GameplayState : IState
    {
        private List<GameObject> gameObjects = new List<GameObject>();
        public TileWorld tileWorld = new TileWorld();

        private PlayerGameObject player;

        public void Draw(SpriteBatch _spriteBatch)
        {
            //tiles
            for (ushort y = 0; y < TileWorld.height; y++)
            {
                for (ushort x = 0; x < TileWorld.width; x++)
                {
                    tileWorld.DrawTile(x, y, _spriteBatch);
                }
            }

            //game objects
            foreach (GameObject go in gameObjects)
            {
                go.Draw(_spriteBatch);
            }
            
        }

        public void Initialize(ContentManager content)
        {
            InitializeBlocks();

            tileWorld.Initialize();
        }

        public void Start()
        {
            player = CreateGameObject(new PlayerGameObject(new Vector2(5f, 10f))) as PlayerGameObject;
        }

        public void Update(GameTime gameTime)
        {
            foreach(GameObject go in gameObjects)
            {
                go.Update(gameTime);
            }

            Camera.CenterCameraOn(player.Center);
        }

        public GameObject CreateGameObject(GameObject gameObject)
        {
            gameObjects.Add(gameObject);
            return gameObject;
        }
    }
}
