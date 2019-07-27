using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork
{
    public enum MeshRotation
    {
        none,
        back,
        left,
        right
    }
    public class ChunkMeshCreator : MonoBehaviour
    {
        private Chunk chunk;
        private int SideLength;

        //mesh data
        private List<List<int>> Faces = new List<List<int>>();
        private int FaceCount;

        public Mesh cube;

        private bool inited;

        public void Init()
        {
            chunk = GetComponent<Chunk>();
            SideLength = chunk.SideLength;
            for(var i = 0; i < GetComponent<Renderer>().materials.Length; i++)
            {
                Faces.Add(new List<int>());
            }
            inited = true;
        }

        public void RebuildMesh()
        {
            if (!inited)
            {
                Init();
            }
            foreach(Transform trans in transform)
            {
                Destroy(trans.gameObject);
            }
            int x = 0, y = 0, z = 0;
            chunk.GetNearbyChunks();
            while (x < SideLength)
            {
                while (y < SideLength)
                {
                    while (z < SideLength)
                    {
                        var block = chunk.GetBlock(x, y, z);
                        if (block != 0)
                        {
                            var blockType = MapGenerator.GetBlockType(block);
                            if (blockType.m_CustomMesh == false)
                            {
                                Transparency transparency = blockType.m_Transparency;
                                ColliderType colliderType = blockType.m_ColliderType;
                                //TODO
                            }
                        }
                        
                    }
                }
            }
        }



    }
}
