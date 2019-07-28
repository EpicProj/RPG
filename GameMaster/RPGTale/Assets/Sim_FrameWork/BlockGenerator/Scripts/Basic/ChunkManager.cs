using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

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
        public static bool Inited;

        public static int SavesThisFrame;

        //local
        private bool isFinish;
        private float FrameDuration;
        private int SpawnQueue;
        private Index lastRequest;
        private Stopwatch stopWatch;


        void Start()
        {
            Chunks = new Dictionary<string, Chunk>();
            ChunkUpdateList = new List<Chunk>();
            FrameDuration = 1f / MapGenerator.TargetFPS;
            stopWatch = new Stopwatch();

            MapGenerator.ChunkScale = ChunkObject.transform.localScale;
            ChunkObject.GetComponent<Chunk>().MeshContainer.transform.localScale = ChunkObject.transform.localScale;
            ChunkObject.GetComponent<Chunk>().ChunkCollider.transform.localScale = ChunkObject.transform.localScale;

            isFinish = true;
            isSpawningChunks = false;
            Inited = true;
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
                lastRequest = new Index(x, y, z);
                SpawnQueue = 1;
                StopSpawning = true;
                ChunkUpdateList.Clear();
            }
        }

        //Spawn
        public void Update()
        {
            if(SpawnQueue==1 && isFinish == true)
            {
                SpawnQueue = 0;
                StartSpawnChunks(lastRequest.x, lastRequest.y, lastRequest.z);
            }
            if(!isSpawningChunks&&!ProcessChunkQueueLoopActive && ChunkUpdateList!=null && ChunkUpdateList.Count > 0)
            {
                StartCoroutine(ProcessChunkQueueLoop());
            }
            ResetFrameStopWatch();
        }

        private void ResetFrameStopWatch()
        {
            stopWatch.Stop();
            stopWatch.Reset();
            stopWatch.Start();
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
                                        ProcessChunkList();
                                        if (stopWatch.Elapsed.TotalSeconds >= FrameDuration)
                                        {
                                            yield return new WaitForEndOfFrame();
                                        }
                                    }
                                    Chunk currentChunk = GetChunkComponent(x, y, z);
                                    if (currentChunk != null)
                                    {
                                        //Build Mesh
                                        if(currentChunk.DisableMesh || currentChunk.EnableTimeout)
                                        {
                                            currentChunk.DisableMesh = false;
                                            currentChunk.EnableTimeout = false;
                                            currentChunk.Fresh = true;
                                        }
                                        if (currentChunk.Fresh)
                                        {
                                            //Spwan Nearby Chunks
                                            for(int i = 0; i < 6; i++)
                                            {
                                                Index nearbyIndex = currentChunk.ChunkIndex.GetNearbyChunkIndex((Direction)i);
                                                GameObject nearbyChunk = GetChunk(nearbyIndex);
                                                if (nearbyChunk == null)
                                                {
                                                    nearbyChunk = Instantiate(ChunkObject, nearbyIndex.ToVector3(), transform.rotation) as GameObject;
                                                }
                                                currentChunk.NearbyChunks[i] = nearbyChunk.GetComponent<Chunk>();
                                                if (stopWatch.Elapsed.TotalSeconds >= FrameDuration)
                                                {
                                                    yield return new WaitForEndOfFrame();
                                                }
                                                if (StopSpawning)
                                                {
                                                    EndSequence();
                                                    yield break;
                                                }
                                            }

                                            if (currentChunk != null)
                                                currentChunk.AddToQueueWhenReady();
                                        }
                                    }
                                    else
                                    {
                                        //Create new Chunk
                                        GameObject newChunk = Instantiate(ChunkObject, new Vector3(x, y, z), transform.rotation) as GameObject;
                                        currentChunk = newChunk.GetComponent<Chunk>();
                                        //Spawn NearbyChunk
                                        for(int i = 0; i < 6; i++)
                                        {
                                            Index nearbyIndex = currentChunk.ChunkIndex.GetNearbyChunkIndex((Direction)i);
                                            GameObject nearbyChunk = GetChunk(nearbyIndex);
                                            if (nearbyChunk == null)
                                            {
                                                nearbyChunk = Instantiate(ChunkObject, nearbyIndex.ToVector3(), transform.rotation) as GameObject;
                                            }
                                            currentChunk.NearbyChunks[i] = nearbyChunk.GetComponent<Chunk>();

                                            if (stopWatch.Elapsed.TotalSeconds >= FrameDuration)
                                            {
                                                yield return new WaitForEndOfFrame();
                                            }

                                            if (StopSpawning)
                                            {
                                                EndSequence();
                                                yield break;
                                            }
                                        }
                                        if (currentChunk != null)
                                            currentChunk.AddToQueueWhenReady();
                                    }

                                }
                            }
                            if (stopWatch.Elapsed.TotalSeconds >= FrameDuration)
                            {
                                yield return new WaitForEndOfFrame();
                            }

                            if (StopSpawning)
                            {
                                EndSequence();
                                yield break;
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
                currentChunk.RebuildMesh();
            }
            currentChunk.Fresh = false;
            ChunkUpdateList.RemoveAt(0);
        }

        private bool ProcessChunkQueueLoopActive;
        private IEnumerator ProcessChunkQueueLoop()
        {
            ProcessChunkQueueLoopActive = true;
            while(ChunkUpdateList.Count>0 && !isSpawningChunks && !StopSpawning)
            {
                ProcessChunkList();
                if (stopWatch.Elapsed.TotalSeconds >= FrameDuration)
                {
                    yield return new WaitForEndOfFrame();
                }
            }
            ProcessChunkQueueLoopActive = false;
        }
    }
}
