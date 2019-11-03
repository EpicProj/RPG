using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork
{
    public class Chunk : MonoBehaviour
    {
        public ushort[] BlockData;

        public Index ChunkIndex;
        public Chunk[] NearbyChunks;
        public bool isEmpty;


        public bool Fresh = true;

        //区块生成时长
        public float LifeTime;

        //区块大小
        public int SideLength;
        private int SquaredSideLength;

        //Queue
        public bool BlockDown;
        public bool FlaggedToUpdate;

        public GameObject MeshContainer;
        public GameObject ChunkCollider;

        public void Awake()
        {
            ChunkIndex = new Index(transform.position);
            SideLength = MapGenerator.ChunkSideLegth;
            SquaredSideLength = SideLength * SideLength;
            //[0] right ; [1] left ; [2] forward ; [3] back
            NearbyChunks = new Chunk[4];

            Fresh = true;
            ChunkManager.RegisterChunk(this);

            BlockData = new ushort[SideLength * SideLength * SideLength];
            transform.position = ChunkIndex.ToVector3() * SideLength;
            //Scale
            transform.position = new Vector3(transform.position.x * transform.localScale.x, transform.position.y * transform.localScale.y, transform.position.z * transform.localScale.z);

            //GEN Block
            GenerateBlockData();


        }
        public void GenerateBlockData()
        {
            GetComponent<BaseTerrainGenerator>().Init();
        }

        public void Update()
        {
            ChunkManager.SavesThisFrame = 0;
        }
        public void LateUpdate()
        {


            if(FlaggedToUpdate && BlockDown && MapGenerator.GenerateMeshs)
            {
                FlaggedToUpdate = false;

            }
        }
        

        public void FlagToUpdate()
        {
            FlaggedToUpdate = true;
        }


        public void SetBlock(int x,int y,ushort data)
        {
            if (x < 0)
            {
                if (NearbyChunks[(int)Direction.left] != null)
                {
                    NearbyChunks[(int)Direction.left].SetBlock(x + SideLength, y, data);
                    return;
                }
            }else if (x >= SideLength)
            {
                if (NearbyChunks[(int)Direction.right] != null)
                {
                    NearbyChunks[(int)Direction.right].SetBlock(x - SideLength, y, data);
                    return;
                }
            }else if (y < 0)
            {
                if (NearbyChunks[(int)Direction.down] != null)
                {
                    NearbyChunks[(int)Direction.down].SetBlock(x, y + SideLength, data);
                    return;
                }
            }else if (y >= SideLength)
            {
                if (NearbyChunks[(int)Direction.up] != null)
                {
                    NearbyChunks[(int)Direction.up].SetBlock(x, y - SideLength, data);
                    return;
                }
            }
            
            BlockData[(y * SideLength) + x] = data;


        }
        public void SetBlock(Index index,ushort data)
        {
            SetBlock(index.x, index.y, data);
        }


        /// <summary>
        /// 数据好了之后增加到序列
        /// </summary>
        public void AddToQueueWhenReady()
        {
            StartCoroutine(DoAddToQueueWhenReady());
        }
        public IEnumerator DoAddToQueueWhenReady()
        {
            while (BlockDown == false || AllNearbyChunksHaveData() == false)
            {
                if (ChunkManager.StopSpawning)
                {
                    yield break;
                }
                yield return new WaitForEndOfFrame();
            }
            ChunkManager.AddChunkToUpdateList(this);
        }

        private bool AllNearbyChunksHaveData()
        {
            foreach (var chunk in NearbyChunks)
            {
                if (NearbyChunks != null)
                {
                    if (chunk.BlockDown == false)
                        return false;
                }
            }
            return true;
        }

        ///根据坐标获取方块所处的区块
        public ushort GetBlock(int x,int y)
        {
            if (x < 0)
            {
                if (NearbyChunks[(int)Direction.left] != null)
                {
                    return NearbyChunks[(int)Direction.left].GetBlock(x + SideLength, y);
                }
                else return ushort.MaxValue;
            }else if (x >= SideLength)
            {
                if (NearbyChunks[(int)Direction.right] != null)
                {
                    return NearbyChunks[(int)Direction.right].GetBlock(x - SideLength, y);
                }
                else return ushort.MaxValue;
            }else if (y < 0)
            {
                if (NearbyChunks[(int)Direction.down] != null)
                {
                    return NearbyChunks[(int)Direction.down].GetBlock(x , y + SideLength);
                }
                else return ushort.MaxValue;
            }else if (y >= SideLength)
            {
                if (NearbyChunks[(int)Direction.up] != null)
                {
                    return NearbyChunks[(int)Direction.up].GetBlock(x, y - SideLength);
                }
                else return ushort.MaxValue;
            }
            else
            {
                return BlockData[(y * SideLength) + x];
            } 
        }

        public ushort GetBlock(Index index)
        {
            return GetBlock(index.x, index.y);
        }
        public ushort GetBlockSimpleData(int index)
        {
            return BlockData[index];
        }
        public ushort GetBlockSimpleData(int x,int y , int z)
        {
            return BlockData[(z * SquaredSideLength) + (y * SideLength) + x];
        }
        public ushort GetBlockSimpleData(Index index)
        {
            return BlockData[ (index.y * SideLength) + index.x];
        }

        //set Block
        public void SetBlockSimple(int rawIndex,ushort data)
        {
            BlockData[rawIndex] = data;
        }

        public void SetBlockSimple(int x,int y,int z,ushort data)
        {
            BlockData[(z * SquaredSideLength) + (y * SideLength) + x] = data;
        }
        public void SetBlockSimple(Index index,ushort data)
        {
            BlockData[(index.y * SideLength) + index.x] = data;
        }

        //Pos

        public Vector3 BlockIndexToPosition(Index index)
        {
            Vector3 localPos = index.ToVector3();
            return transform.TransformPoint(localPos);
        }

        public Vector3 BlockIndexToPosition(int x,int y,int z)
        {
            Vector3 localPos = new Vector3(x, y, z);
            return transform.TransformPoint(localPos);
        }

        #region NearbyChunk
        //获取周边区块信息
        public void GetNearbyChunks()
        {
            int x = ChunkIndex.x;
            int y = ChunkIndex.y;
            if (NearbyChunks[0] == null)
                NearbyChunks[0] = ChunkManager.GetChunkComponent(x, y + 1);
            if (NearbyChunks[1] == null)
                NearbyChunks[1] = ChunkManager.GetChunkComponent(x, y - 1);
            if (NearbyChunks[2] == null)
                NearbyChunks[2] = ChunkManager.GetChunkComponent(x + 1, y);
            if (NearbyChunks[3] == null)
                NearbyChunks[3] = ChunkManager.GetChunkComponent(x - 1, y);

        }
        public Index GetNearbyChunkIndex(Index index,Direction direction)
        {
            return GetNearbyChunkIndex(index.x, index.y, direction);
        }

        public Index GetNearbyChunkIndex(int x, int y, Direction direction)
        {
            if (direction == Direction.down)
                return new Index(x, y - 1);
            else if (direction == Direction.up)
                return new Index(x, y + 1);
            else if (direction == Direction.left)
                return new Index(x - 1, y);
            else if (direction == Direction.right)
                return new Index(x + 1, y);
            else
            {
                Debug.LogError("Get Nearby Chunk Error");
                return new Index(x, y);
            }
        }



        #endregion


        #region Data
        public int GetDataLength()
        {
            return BlockData.Length;
        }

        private void SaveData()
        {
            if (MapGenerator.SaveBlockData == false)
                return;
            GetComponent<ChunkDataFile>().SaveData();
        }
        #endregion
    }
}