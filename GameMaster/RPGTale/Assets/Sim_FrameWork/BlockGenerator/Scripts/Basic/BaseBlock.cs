using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Sim_FrameWork
{
    public class BaseBlock : MonoBehaviour
    {

        //基础方块信息
        public string BlockName;
        public Mesh mesh;
        public bool m_CustomMesh;
        public bool m_CuntonSides;
        public Vector2[] m_Texture;

        public Transparency m_Transparency;
        public ColliderType m_ColliderType;

        public static void DestoryBlock(BlockInfo blockInfo)
        {
            GameObject blockObj = Instantiate(MapGenerator.GetBlockGameObj(blockInfo.GetBlock())) as GameObject;
        }
    }
}
