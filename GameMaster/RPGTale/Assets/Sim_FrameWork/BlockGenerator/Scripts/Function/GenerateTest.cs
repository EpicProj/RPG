using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork
{
    public class GenerateTest : BaseTerrainGenerator
    {

        public override void GenerateBlockData()
        {
            int chunky = chunk.ChunkIndex.y;
            int sideLength = MapGenerator.ChunkSideLegth;

            int random = Random.Range(0, 10);
            for(int x = 0; x< sideLength; x++)
            {
                for(int y = 0; y < sideLength; y++)
                {
                    for(int z = 0; z < sideLength; z++)
                    {
                        //TODO
                    }
                }
            }
        }

    }
}
