using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork
{
    public class MapManager : MonoSingleton<MapManager>
    {


        protected override void Awake()
        {
            base.Awake();

        }

        private void Start()
        {
            //InvokeRepeating("InitMap", 1, 0.5f);
        }

        private void InitBaseData()
        {
        }

        private void InitMap()
        {
            if (MapGenerator.Inited == false || ChunkManager.Inited == false)
                return;
            ChunkManager.SpawnChunks(transform.position);
        }


    }


}