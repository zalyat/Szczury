using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Szczury.Monsters;

namespace Szczury
{
    public class WorldMonsterSpawner : GameObject
    {
        private float timer = 0f;
        private int spawnerHeight = 0;
        private int xRange = 16;
        private int spawningIntensity = 1;

        public List<Monster> monsters = new List<Monster>();

        public WorldMonsterSpawner(Vector2 startingPosition, TileWorld world) : base(startingPosition, world)
        {
            drawable = false;
        }

        public override void Start()
        {

        }

        public override void Update(GameTime gameTime)
        {
            
        }

        public void SpawnMonsters(int playerTileX)
        {
            for (int i = 0; i < spawningIntensity; i++)
            {
                //var watch = Stopwatch.StartNew();
                SpawnRandomMonster(playerTileX);
                //watch.Stop();
                //Debug.WriteLine($"{watch.ElapsedMilliseconds}");
            }
        }

        private void SpawnRandomMonster(int playerTileX)
        {
            int x = new Random().Next(xRange) - (xRange/2);
            Point location = currentWorld.getClosestGround(new Point(playerTileX + x, spawnerHeight));

            if (currentWorld.isInWorldBoundaries(location) == false) return;

            GameplayState.Main.CreateGameObject(new MonsterToxicSnail(currentWorld.TilePositionToWorldPosition(location + new Point(0, -1)), currentWorld, false));
        }
    }
}
