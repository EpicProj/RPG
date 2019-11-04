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
        public bool m_CuntomSides;
        public Vector2[] m_Texture;

        public Transparency m_Transparency;
        public ColliderType m_ColliderType;
        public int m_submeshIndex;
        public MeshRotation m_Rotation;

        public static void DestoryBlock(BlockInfo blockInfo)
        {
            GameObject blockObj = Instantiate(MapGenerator.GetBlockGameObj(blockInfo.GetBlock())) as GameObject;
            if (blockObj.GetComponent<BlockEvents>() != null)
            {
                blockObj.GetComponent<BlockEvents>().OnBlockDestory(blockInfo);
            }
            blockInfo.chunk.SetBlock(blockInfo.index, 0, true);
            Destroy(blockObj);

        }

        public static void PlaceBlock(BlockInfo info,ushort data)
        {
            info.chunk.SetBlock(info.index, data, true);
            GameObject blockObj = Instantiate(MapGenerator.GetBlockGameObj(data)) as GameObject;
            if (blockObj.GetComponent<BlockEvents>() != null)
            {
                blockObj.GetComponent<BlockEvents>().OnBlockPlace(info);
            }
            Destroy(blockObj);
        }

        public static void ChangeBlock(BlockInfo info ,ushort data)
        {
            info.chunk.SetBlock(info.index, data, true);
            GameObject blockObj = Instantiate(MapGenerator.GetBlockGameObj(data)) as GameObject;
            if (blockObj.GetComponent<BlockEvents>() != null)
            {
                blockObj.GetComponent<BlockEvents>().OnBlockChange(info);
            }
            Destroy(blockObj);
        }
        

        //Editor
        public ushort GetID()
        {
            return ushort.Parse(gameObject.name.Split('_')[1]);
        }
        public void SetID(ushort id)
        {
            gameObject.name = "Block_" + id.ToString();
        }
        
    }
}
