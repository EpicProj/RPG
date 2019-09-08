using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork
{
    public class MapManager : MonoBehaviour
    {
        private Transform center;
        private Index lastIndex;

        private void Start()
        {
            center = GameObject.Find("Center").transform;
            InvokeRepeating("InitMap", 1, 0.5f);
        }

        private void InitMap()
        {
            if (MapGenerator.Inited == false || ChunkManager.Inited == false)
                return;
            Index currentIndex = MapGenerator.PositionToChunkIndex(center.position);
            if (lastIndex != currentIndex)
            {
                ChunkManager.SpawnChunks(center.position);
                lastIndex = currentIndex;
            }
        }
    }
}