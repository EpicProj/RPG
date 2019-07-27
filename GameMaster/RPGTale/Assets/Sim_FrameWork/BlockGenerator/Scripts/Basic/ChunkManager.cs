using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork
{
    public class ChunkManager : MonoSingleton<ChunkManager>
    {

        public GameObject ChunkObject;

        public static Dictionary<string, Chunk> Chunks;
        private static List<Chunk> ChunkUpdateList; //储存序列
        private static List<Chunk> ChunksToDestory;

        public static bool isSpawningChunks;
        public static bool StopSpawning;

        private bool isFinish;


        void Start()
        {
            Chunks = new Dictionary<string, Chunk>();
            ChunkUpdateList = new List<Chunk>();


            isFinish = true;
        }

        //添加到更新序列
        public static void AddChunkToUpdateList(Chunk chunk)
        {
            if (!ChunkUpdateList.Contains(chunk))
            {
                ChunkUpdateList.Add(chunk);
            }
        }

        public static void RegisterChunk(Chunk chunk)
        {
            Chunks.Add(chunk.ChunkIndex.ToString(), chunk);
        }
        public static void UnRegisterChunk(Chunk chunk)
        {
            Chunks.Remove(chunk.ChunkIndex.ToString());
        }

        public static GameObject GetChunk(int x,int y,int z)
        {
            return GetChunk(new Index(x, y, z));
        }
        public static GameObject GetChunk(Index index)
        {
            Chunk chunk = GetChunkComponent(index);
            if (chunk == null)
            {
                return null;
                Debug.LogWarning("Chunk is null ,index=" + index);
            }
            else
            {
                return chunk.gameObject;
            }
        }

        public static Chunk GetChunkComponent(int x,int y,int z)
        {
            return GetChunkComponent(new Index(x, y, z));
        }

        public static Chunk GetChunkComponent(Index index)
        {
            string indexStr = index.ToString();
            if (Chunks.ContainsKey(indexStr))
            {
                return Chunks[indexStr];
            }
            else
            {
                return null;
            }
        }

        GameObject GenerateChunkObj(Index index)
        {
            GameObject chunkObj = Instantiate(ChunkObject, index.ToVector3(), transform.rotation) as GameObject;
            Chunk chunk = ChunkObject.GetComponent<Chunk>();
            AddChunkToUpdateList(chunk);
            return chunkObj;
        }

        public static void SpawnChunks(int x,int y,int z)
        {
            MapGenerator.ChunkManagerInstance.TrySpawnChunks(x,y,z);
        }
        public static void SpawnChunks(Index index)
        {
            MapGenerator.ChunkManagerInstance.TrySpawnChunks(index.x, index.y, index.z);
        }

        private void TrySpawnChunks(Index index)
        {
            TrySpawnChunks(index.x, index.y, index.z);
        }
        private void TrySpawnChunks(int x,int y,int z)//XYZ为生成基点
        {
            if (isFinish == true)
            {
                StartSpawnChunks(x, y, z);
            }
            else
            {
                ChunkUpdateList.Clear();
            }
        }

        private void StartSpawnChunks(int x,int y,int z)
        {
            isSpawningChunks = true;
            isFinish = false;
            int range = MapGenerator.SpawnDistance;
            StartCoroutine(GenerateCurrentChunks(x, y, z, range));
        }

        //生成Chunk
        private IEnumerator GenerateCurrentChunks(int originX,int originY,int originZ,int range)
        {
            int heightRange = MapGenerator.HeightRange;//Height
            ChunkUpdateList = new List<Chunk>();

            ChunksToDestory = new List<Chunk>();//销毁超范围块
            foreach(Chunk chunk in Chunks.Values)
            {
                if(Vector2.Distance(new Vector2 (chunk.ChunkIndex.x,chunk.ChunkIndex.z),new Vector2(originX, originZ)) > range + MapGenerator.SpawnDistance)//超range加入销毁队列
                {
                    ChunksToDestory.Add(chunk);
                }else if (Mathf.Abs(chunk.ChunkIndex.y - originY) > range + MapGenerator.SpawnDistance)
                    //高度上超出销毁
                {
                    ChunksToDestory.Add(chunk);
                }
            }
            
            for(int currentLoop = 0; currentLoop <= range; currentLoop++)
            {
                for(var x = originX - currentLoop; x <= originX + currentLoop; x++)
                {
                    //X范围双向循环
                    for(var y = originY - currentLoop; y < originY + currentLoop; y++)
                    {
                    //Y范围双向循环
                    for(var z = originZ - currentLoop; z < originZ + currentLoop; z++)
                        {
                            if (Mathf.Abs(y) <= heightRange)
                            {
                                if (Mathf.Abs(originX - x) + Mathf.Abs(originZ - z) < range * 1.3f)
                                {
                                    while (ChunkUpdateList.Count > 0)
                                    {
                                        
                                    }
                                }
                            }
                        }
                    }
                }
            }
            yield return new WaitForEndOfFrame();
            EndSequence();
        }
        private void EndSequence()
        {
            isSpawningChunks = false;
            Resources.UnloadUnusedAssets();
            isFinish = true;
            StopSpawning = false;
            foreach(Chunk chunk in ChunksToDestory)
            {
                chunk.FlagToRemove();
            }
            
        }

        public void ProcessChunkList()
        {
            Chunk currentChunk = ChunkUpdateList[0];
            if(!currentChunk.isEmpty&& !currentChunk.DisableMesh)
            {
                //TODO
            }
        }
    }
}
