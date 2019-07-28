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

        //File
        public static string WorldName;
        public static string WorldPath;
        public static string BlocksPath;
        public static int Seed;
        public string m_WorldName="Test";
        public string m_BlockPath;

        //基础方块
        public static GameObject[] Blocks;
        public GameObject[] m_Blocks;

        //生成设置
        public static int HeightRange, SpawnDistance, SideLegth;
        //区块大小
        public static int ChunkSideLegth;

        //Setting
        public static bool SaveBlockData;

        public static int MaxChunkSaves;
        public static int TargetFPS;

        public static ChunkManager ChunkManagerInstance;
        public static Vector3 ChunkScale;


        public static bool Inited;
        public void Awake()
        {
            ChunkManagerInstance = GetComponent<ChunkManager>();
            Blocks = m_Blocks;

            WorldName = m_WorldName;

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


            Inited = true;

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


        #region Position Change
        public static Index PositionToChunkIndex(Vector3 position)
        {
            Index index = new Index(Mathf.RoundToInt(position.x / ChunkScale.x) / ChunkSideLegth, Mathf.RoundToInt(position.y / ChunkScale.y) / ChunkSideLegth, Mathf.RoundToInt(position.z / ChunkScale.z) / ChunkSideLegth);
            return index;
        }

        #endregion




        #region World Data
        private static void GenWorldPath()
        {
            WorldPath = Application.dataPath + "/../Worlds/" + WorldName + "/";
        }

        public static void SetWorldName(string worldName)
        {
            WorldName = worldName;
            Seed = 0;
            GenWorldPath();
        }

        public static void GenWorldSeed()
        {
            if (File.Exists(WorldPath + "seed"))
            {
                StreamReader reader = new StreamReader(WorldPath + "seed");
                Seed = int.Parse(reader.ReadToEnd());
                reader.Close();
            }
            else
            {
                while (Seed == 0)
                {
                    Seed = Random.Range(ushort.MinValue, ushort.MaxValue);
                }
                Directory.CreateDirectory(WorldPath);
                StreamWriter writer = new StreamWriter(WorldPath + "seed");
                writer.Write(Seed.ToString());
                writer.Flush();
                writer.Close();
            }
        }

        public static void SaveWorld()
        {
            MapGenerator.Instance.StartCoroutine(ChunkDataFile.SaveAllChunks());
        }
        #endregion


    }
}
