using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

namespace Sim_FrameWork
{
    public class ChunkManager : MonoSingleton<ChunkManager>
    {

        public GameObject ChunkObject;
        private Transform Map;

        public static Dictionary<string, Chunk> Chunks;
        private static List<Chunk> ChunkUpdateList; //储存序列

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
            Map = GameObject.Find("MapManager/MapContainer").transform;
            Chunks = new Dictionary<string, Chunk>();
            ChunkUpdateList = new List<Chunk>();
            FrameDuration = 1f / MapGenerator.TargetFPS;
            stopWatch = new Stopwatch();

            MapGenerator.ChunkScale = ChunkObject.transform.localScale;
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

        #region Spawn Chunk
        /// <summary>
        /// 区块生成
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public static void SpawnChunks(int x,int z)
        {
            MapGenerator.ChunkManagerInstance.TrySpawnChunks(x,z);
        }
        public static void SpawnChunks(Index index)
        {
            MapGenerator.ChunkManagerInstance.TrySpawnChunks(index.x, index.z);
        }
        public static void SpawnChunks(Vector3 pos)
        {
            Index index = MapGenerator.PositionToChunkIndex(pos);
            MapGenerator.ChunkManagerInstance.TrySpawnChunks(index);
        }

        private void TrySpawnChunks(Index index)
        {
            TrySpawnChunks(index.x,index.z);
        }
        private void TrySpawnChunks(int x,int z)//XYZ为生成基点
        {
            if (isFinish == true)
            {
                StartSpawnChunks(x, z);
            }
            else
            {
                lastRequest = new Index(x,0,z);
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
                StartSpawnChunks(lastRequest.x, lastRequest.z);
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

        private void StartSpawnChunks(int x,int z)
        {
            isSpawningChunks = true;
            isFinish = false;
            StartCoroutine(GenerateCurrentChunks(x,z));
        }

        /// <summary>
        /// Chunk Generator
        /// </summary>
        /// <param name="originX"></param>
        /// <param name="originZ"></param>
        /// <returns></returns>
        private IEnumerator GenerateCurrentChunks(int originX,int originZ)
        {

            ChunkUpdateList = new List<Chunk>();
            int mapWidth = MapGenerator.MapWidth;
            int mapLength = MapGenerator.MapLength;

            for (var x = originX - mapWidth ; x <= originX + mapWidth; x++)
            {
                for (var z = originZ - mapLength; z <= originZ + mapLength; z++)
                {

                    while (ChunkUpdateList.Count > 0)
                    {
                        ProcessChunkList();
                        if (stopWatch.Elapsed.TotalSeconds >= FrameDuration)
                        {
                            yield return new WaitForEndOfFrame();
                        }
                    }
                    Chunk currentChunk = GetChunkComponent(x, 0, z);
                    if (currentChunk != null)
                    {
                        if (currentChunk.Fresh)
                        {
                            for (int i = 0; i < 4; i++)
                            {
                                Index nearbyIndex = currentChunk.ChunkIndex.GetNearbyChunkIndex((Direction)i);
                                GameObject nearbyChunk = GetChunk(nearbyIndex);
                                if (nearbyChunk == null)
                                {
                                    nearbyChunk = Instantiate(ChunkObject, nearbyIndex.ToVector3(), transform.rotation) as GameObject;
                                    nearbyChunk.transform.SetParent(Map);
                                    nearbyChunk.name = new Vector2(nearbyChunk.transform.position.x, nearbyChunk.transform.position.z).ToString();
                                }
                                currentChunk.NearbyChunks[i] = UIUtility.SafeGetComponent<Chunk>(nearbyChunk.transform);
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
                        GameObject newChunk = Instantiate(ChunkObject, new Vector3(x, 0, z), transform.rotation) as GameObject;
                        newChunk.transform.SetParent(Map);
                        newChunk.name = new Vector2(newChunk.transform.position.x, newChunk.transform.position.z).ToString();
                        currentChunk = UIUtility.SafeGetComponent<Chunk>(newChunk.transform);
                        for (int i = 0; i < 4; i++)
                        {
                            Index nearbyIndex = currentChunk.ChunkIndex.GetNearbyChunkIndex((Direction)i);
                            GameObject nearbyChunk = GetChunk(nearbyIndex);
                            if (nearbyChunk == null)
                            {
                                nearbyChunk = Instantiate(ChunkObject, nearbyIndex.ToVector3(), transform.rotation) as GameObject;
                                nearbyChunk.transform.SetParent(Map);
                                nearbyChunk.name = new Vector2(nearbyChunk.transform.position.x, nearbyChunk.transform.position.z).ToString();
                            }
                            currentChunk.NearbyChunks[i] = UIUtility.SafeGetComponent<Chunk>(nearbyChunk.transform);

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
            yield return new WaitForEndOfFrame();
            EndSequence();
        }


        private void EndSequence()
        {
            isSpawningChunks = false;
            Resources.UnloadUnusedAssets();
            isFinish = true;
            StopSpawning = false;
            
        }

        public void ProcessChunkList()
        {
            Chunk currentChunk = ChunkUpdateList[0];
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


    #endregion
}
