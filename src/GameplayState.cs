using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Szczury
{
    public partial class GameplayState : IState
    {
        private List<GameObject> gameObjects = new List<GameObject>();
        public TileWorld tileWorld = new TileWorld();
        private ChunkBuffering _chunkBuffering;

        private PlayerGameObject player;
        private TileWorld.Chunk _lastPlayerChunk;

        private GraphicsDevice _graphicsDevice;

        public GameplayState(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {
            tileWorld.SetSpriteBatch(spriteBatch);

            _chunkBuffering = new ChunkBuffering(tileWorld);
            _graphicsDevice = graphicsDevice;
        }

        public void Draw(SpriteBatch _spriteBatch)
        {
            if (TileWorld.isDifferentChunk(tileWorld.GetChunkAtTilePosition(player.PositionInTiles), _lastPlayerChunk))
                _chunkBuffering.UpdateChunkBuffer(player.PositionInTiles);

            DrawWorld();
            

            //game objects
            foreach (GameObject go in gameObjects)
            {
                go.Draw(_spriteBatch);
            }
            //PerFrameCounter.Report();
            //PerFrameCounter.Clear();
            _lastPlayerChunk = tileWorld.GetChunkAtTilePosition(player.PositionInTiles);
        }

        private void DrawWorld()
        {           
            for (int i = 0; i < _chunkBuffering.chunkBuffer.GetLength(0); i++)
                for (int j = 0; j < _chunkBuffering.chunkBuffer.GetLength(1); j++)
                {
                    //Debug.WriteLine($"Attempting to render {i} {j}");
                    tileWorld.DrawChunk(_chunkBuffering.chunkBuffer[i, j]);
                }
        }

        public void Initialize(ContentManager content)
        {
            InitializeBlocks();
            InitializeItems();

            tileWorld.Initialize();
            tileWorld.SaveAs("maps/", "scr" + System.DateTime.Now.Ticks+".png", _graphicsDevice);
        }

        public void Start()
        {
            player = CreateGameObject(new PlayerGameObject(new Vector2(5f, 10f), tileWorld)) as PlayerGameObject;
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
