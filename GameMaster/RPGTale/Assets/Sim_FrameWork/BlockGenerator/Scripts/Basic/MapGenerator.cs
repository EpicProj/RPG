using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace Sim_FrameWork
{
    public enum Facing
    {
        up,
        down,
        left,
        right,
        forward,
        back
    }
    public enum Direction
    {
        up,
        down,
        left,
        right,
        forward,
        back
    }
    public enum Transparency //透明度
    {
        solid, //不透明
        semiTransparent,//半透明
        transparent//透明

    }
    public enum ColliderType
    {
        cube,
        mesh,
        none
    }


    public class MapGenerator : MonoSingleton<MapGenerator> {

        //基础方块
        public static GameObject[] Blocks;
        public GameObject[] lBlocks;

        //生成设置
        public static int HeightRange, SpawnDistance, SideLegth;
        //区块大小
        public static int ChunkSideLegth;


        public static string WorldName;
        public static string BlocksPath;

        public static ChunkManager ChunkManagerInstance;

        public void Awake()
        {
            ChunkManagerInstance = GetComponent<ChunkManager>();
            Blocks = lBlocks;


            GameObject chunkPrefab = GetComponent<ChunkManager>().ChunkObject;
            int materialCount = chunkPrefab.GetComponent<Renderer>().sharedMaterials.Length - 1;

            for(ushort i = 0; i < Blocks.Length; i++)
            {
                if (Blocks[i] != null)
                {
                    BaseBlock block = Blocks[i].GetComponent<BaseBlock>();
                  //TODO
                }
            }

        }


        public static void SaveWorld()
        {
  
        }

        public static GameObject GetBlockGameObj(ushort BlockID)
        {
            try
            {
                if (BlockID == ushort.MaxValue)
                {
                    BlockID = 0;
                }
                GameObject blockObj = Blocks[BlockID];
                if (blockObj.GetComponent<BaseBlock>() == null)
                {
                    Debug.LogError("Block does not have component , block id =" + BlockID);
                    return Blocks[0];
                }
                else
                {
                    return blockObj;
                }
            }
            catch (System.Exception)
            {
                Debug.LogError("Invalid Block id , id =" + BlockID);
                return Blocks[0];
            }
           
        }


        public static BaseBlock GetBlockType(ushort BlockID)
        {
            try
            {
                if (BlockID == ushort.MaxValue)
                {
                    BlockID = 0;
                }
                BaseBlock block = Blocks[(int)BlockID].GetComponent<BaseBlock>();
                if (block == null)
                {
                    Debug.LogError("Block (BaseBlock) is null , id=" + BlockID);
                    return null;
                }
                else
                {
                    return block;
                }
            }
            catch (System.Exception)
            {
                Debug.LogError("Invalid Block ID , ID=" + BlockID);
                return null;
            }
        }

        
    }
}
