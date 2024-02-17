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
        private bool debugMode = false;

        private static GameplayState _mainSingleton;
        public static GameplayState Main
        {
            get => _mainSingleton;
        }

        private List<GameObject> gameObjects = new List<GameObject>();
        private List<GameObject> gameObjectsToDelete = new List<GameObject>();
        private List<GameObject> gameObjectsToCreate = new List<GameObject>();
        public TileWorld tileWorld = new TileWorld();
        private ChunkBuffering _chunkBuffering;

        private PlayerGameObject player;
        private TileWorld.Chunk _lastPlayerChunk;

        private GraphicsDevice _graphicsDevice;

        public GameplayState(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {
            if (_mainSingleton == null) _mainSingleton = this;

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

            if(debugMode == true)
                DrawDebug(_spriteBatch);

            //PerFrameCounter.Report();
            //PerFrameCounter.Clear();
            _lastPlayerChunk = tileWorld.GetChunkAtTilePosition(player.PositionInTiles);
        }

        private void DrawDebug(SpriteBatch _spriteBatch)
        {
            _spriteBatch.DrawString(TextureSet.debugFont, $"{gameObjects.Count}", new Vector2(Util.screenHeight / 2, Util.screenWidth / 2), Color.Wheat);
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

            DeleteObjects();
            CreateObjects();
        }

        public GameObject CreateGameObject(GameObject gameObject)
        {
            gameObjectsToCreate.Add(gameObject);
            return gameObject;
        }

        public void DeleteGameObject(GameObject gameObject)
        {
            gameObjectsToDelete.Add(gameObject);
        }

        private void CreateObjects()
        {
            foreach(GameObject go in gameObjectsToCreate)
            {
                gameObjects.Add(go);
            }
            gameObjectsToCreate.Clear();
        }

        private void DeleteObjects()
        {
            foreach(GameObject go in gameObjectsToDelete)
            {
                gameObjects.Remove(go);
            }
            gameObjectsToDelete.Clear();
        }
    }
}
