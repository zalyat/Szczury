using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Szczury
{
    internal class ChunkBuffering
    {
        public const int renderRadiusHorizontal = 2;
        public const int renderRadiusVertical = 1;
        public TileWorld.Chunk[,] chunkBuffer = new TileWorld.Chunk[renderRadiusHorizontal*2+1, renderRadiusVertical*2+1];
        private TileWorld _world;

        public ChunkBuffering(TileWorld tileWorld)
        {
            _world = tileWorld;
        }

        /// <summary>
        /// Change chunks in chunk buffer
        /// </summary>
        /// <param name="center">Center of buffering (in tiles)</param>
        public void UpdateChunkBuffer(Point center)
        {
            for (int i = -renderRadiusHorizontal; i < renderRadiusHorizontal+1; i++)
                for (int j = -renderRadiusVertical; j < renderRadiusVertical+1; j++)
                {                  
                    TileWorld.Chunk chunk = _world.GetChunkAtTilePosition(
                        new Point(center.X + i * (Util.chunkSize),
                        center.Y + j * (Util.chunkSize)));
                    chunkBuffer[i+renderRadiusHorizontal, j+renderRadiusVertical] = chunk; //adding renderRadius so the code won't look for negative index
                }

            
            /*for(int j = 0; j < chunkBuffer.GetLength(1); j++)
            {
                Debug.Write("\n");
                for(int i = 0; i < chunkBuffer.GetLength(0); i++)
                {
                    Debug.Write($"{chunkBuffer[i,j].i} {chunkBuffer[i,j].j}  ");
                }
            }
            Debug.WriteLine("/////////////////");*/
        }
    }
}
