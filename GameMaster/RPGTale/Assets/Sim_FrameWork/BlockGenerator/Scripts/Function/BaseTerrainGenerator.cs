using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork { 

    public class BaseTerrainGenerator : MonoBehaviour
    {

        protected Chunk chunk;
        protected int seed;

        public void Init()
        {
            while (MapGenerator.Seed == 0)
            {
                MapGenerator.GenWorldSeed();
            }
            seed = MapGenerator.Seed;

            chunk = GetComponent<Chunk>();

            GenerateBlockData();
            chunk.isEmpty = true;
            foreach(var block in chunk.BlockData)
            {
                if (block != 0)
                {
                    chunk.isEmpty = false;
                    break;
                }
            }

            chunk.BlockDown = true;
        }


        public virtual void GenerateBlockData() { }


    }

}
