using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork
{
    public class ChunkLoader : MonoBehaviour
    {
        private Index LastPos;
        private Index currentPos;

        public void Update()
        {
            if (!MapGenerator.Inited || !ChunkManager.Inited)
                return;
            currentPos = MapGenerator.PositionToChunkIndex(transform.position);

            if (currentPos.IsEqual(LastPos) == false)
            {
                ChunkManager.SpawnChunks(currentPos.x, currentPos.y, currentPos.z);
            }

            LastPos = currentPos;
        }
      
    }
}