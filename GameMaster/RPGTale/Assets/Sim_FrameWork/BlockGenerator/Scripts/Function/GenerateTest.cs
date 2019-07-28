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
                        Vector3 blockPos = chunk.BlockIndexToPosition(x, y, z);
                        blockPos = new Vector3(blockPos.x + seed, blockPos.y, blockPos.z + seed);

                        float major = Mathf.PerlinNoise(blockPos.x * 0.010f, blockPos.z * 0.010f) * 70.1f;
                        float minor = Mathf.PerlinNoise(blockPos.x * 0.085f, blockPos.z * 0.085f) * 9.1f;

                        int currentHeight = y + (sideLength * chunky);

                        //grass
                        if (major > currentHeight)
                        {
                            if (major > minor + currentHeight)
                            {
                                chunk.SetBlockSimple(x, y, z, 2);
                            }
                        }

                        //dirt
                        currentHeight = currentHeight + 1;
                        if (major > currentHeight)
                        {
                            if (major > minor + currentHeight)
                            {
                                chunk.SetBlockSimple(x, y, z, 1);
                            }
                        }
                    }
                }
            }
        }

    }
}
