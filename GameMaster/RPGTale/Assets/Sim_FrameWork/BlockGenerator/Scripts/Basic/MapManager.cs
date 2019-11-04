using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork
{
    public class MapManager : MonoBehaviour
    {
        private Index lastIndex;

        private void Start()
        {
            InvokeRepeating("InitMap", 1, 0.5f);
        }

        private void InitMap()
        {
            if (MapGenerator.Inited == false || ChunkManager.Inited == false)
                return;
            Index currentIndex = MapGenerator.PositionToChunkIndex(transform.position);
            if (lastIndex != currentIndex)
            {
                ChunkManager.SpawnChunks(transform.position);
                lastIndex = currentIndex;
            }
        }
    }
}