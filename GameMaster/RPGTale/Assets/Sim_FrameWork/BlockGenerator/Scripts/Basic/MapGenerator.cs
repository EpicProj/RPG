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
        public static int  SpawnDistance, ChunkSideLegth;
        public int  m_SpawnDistance, m_ChunkSideLegth;


        //performace
        public static int MaxChunkSaves, TargetFPS;
        public int m_MaxChunkSaves, m_TargetFPS;


        //General Setting
        public static bool GenerateColliders,ShowBorderFaces, SaveBlockData;
        public bool m_GenerateColliders, m_ShowBorderFaces, m_SaveBlockData;


        public static ChunkManager ChunkManagerInstance;
        public static Vector3 ChunkScale;
        public static int SquaredSideLength;


        public static bool Inited;
        protected override void Awake()
        {
            base.Awake();
            ChunkManagerInstance = GetComponent<ChunkManager>();

            InitData();

            ChunkDataFile.LoadedRegions = new Dictionary<string, string[]>();
            ChunkDataFile.TempChunkData = new Dictionary<string, string>();

            //Set layer
            if(LayerMask.LayerToName(26)!=""&&LayerMask.LayerToName(26)!= "UniblocksNoCollide")
            {
                Debug.LogWarning("Layer 26 is reservd for uniblocks");
            }
            for(int i = 0; i < 31; i++)
            {
                Physics.IgnoreLayerCollision(i, 26);
            }
            //Check TODO

            //Check Materia;
            GameObject chunkPrefab = GetComponent<ChunkManager>().ChunkObject;
            int materialCount = chunkPrefab.GetComponent<Renderer>().sharedMaterials.Length - 1;

            for(ushort i = 0; i < Blocks.Length; i++)
            {
                if (Blocks[i] != null)
                {
                    BaseBlock block = Blocks[i].GetComponent<BaseBlock>();
                    if (block.m_submeshIndex < 0)
                    {
                        Debug.LogError("Block " + i + "material index <0");
                        Debug.Break();
                    }
                    if (block.m_submeshIndex > materialCount)
                    {
                        Debug.LogError("Block " + i +"material index >count");
                        Debug.Break();
                    }
                }
            }


            Inited = true;

        }

        private void InitData()
        {
            WorldName = m_WorldName;
            GenWorldPath();

            Blocks = m_Blocks;
            BlocksPath = m_BlockPath;

            TargetFPS = m_TargetFPS;
            MaxChunkSaves = m_MaxChunkSaves;

            SpawnDistance = m_SpawnDistance;

            ChunkSideLegth = m_ChunkSideLegth;

            SaveBlockData = m_SaveBlockData;

            GenerateColliders = m_GenerateColliders;
            ShowBorderFaces = m_ShowBorderFaces;
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

        //Mesh
        public static Vector2 GetTextureOffset(ushort block,Facing facing)
        {
            BaseBlock blockType = GetBlockType(block);
            Vector2[] TextureArray = blockType.m_Texture;
            if (TextureArray.Length == 0)
            {
                //default texture
                return new Vector2(0, 0);
            }else if (blockType.m_CuntomSides == false)
            {
                return TextureArray[0];
            }else if ((int)facing > TextureArray.Length - 1)
            {
                return TextureArray[TextureArray.Length - 1];
            }
            else
            {
                return TextureArray[(int)facing];
            }
        }

    }
}
