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

        public bool DisableMesh;
        public bool Fresh = true;
        public bool EnableTimeout;

        private bool FlaggedToRemove;
        //区块生成时长
        public float LifeTime;

        //区块大小
        public int SideLength;

        //Queue
        public bool BlockDown;
        public bool FlaggedToUpdate;

        public GameObject ChunkCollider;

        public void Awake()
        {
            ChunkIndex = new Index(transform.position);
            SideLength = MapGenerator.ChunkSideLegth;
            // [1] right ; [2] left ; [3] forward ; [4] back
            NearbyChunks = new Chunk[4];

            Fresh = true;
            ChunkManager.RegisterChunk(this);

            BlockData = new ushort[SideLength * SideLength];
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

        }
        


        public void FlagToUpdate()
        {
            FlaggedToUpdate = true;
        }


        public void SetBlock(int x,int y,int z,ushort data,bool updateMesh)
        {
            if (x < 0)
            {
                if (NearbyChunks[(int)Direction.left] != null)
                {
                    NearbyChunks[(int)Direction.left].SetBlock(x + SideLength, y, z, data, updateMesh);
                    return;
                }
            }else if (x >= SideLength)
            {
                if (NearbyChunks[(int)Direction.right] != null)
                {
                    NearbyChunks[(int)Direction.right].SetBlock(x - SideLength, y, z, data, updateMesh);
                    return;
                }
            }else if (z < 0)
            {
                if (NearbyChunks[(int)Direction.back] != null)
                {
                    NearbyChunks[(int)Direction.back].SetBlock(x, y, z + SideLength, data, updateMesh);
                    return;
                }
            }else if (z >= SideLength)
            {
                if (NearbyChunks[(int)Direction.forward] != null)
                {
                    NearbyChunks[(int)Direction.forward].SetBlock(x, y, z - SideLength, data, updateMesh);
                    return;
                }
            }
            
            BlockData[ (z * SideLength) + x] = data;
            if (updateMesh)
            {
                UpdateNearbyIfNeeded(x,y,z);
                //TODO
            }

        }
        public void SetBlock(Index index,ushort data,bool updateMesh)
        {
            SetBlock(index.x, index.y, index.z, data, updateMesh);
        }

        public void UpdateNearbyIfNeeded(int x,int y,int z)
        {
            if(x==0 && NearbyChunks[(int)Direction.left] != null)
            {
                NearbyChunks[(int)Direction.left].GetComponent<Chunk>().FlagToUpdate();
            }
            else if (x == SideLength-1 && NearbyChunks[(int)Direction.right] != null)
            {
                NearbyChunks[(int)Direction.right].GetComponent<Chunk>().FlagToUpdate();
            }
            else if (z == 0 && NearbyChunks[(int)Direction.back] != null)
            {
                NearbyChunks[(int)Direction.back].GetComponent<Chunk>().FlagToUpdate();
            }
            else if (z == SideLength - 1 && NearbyChunks[(int)Direction.forward] != null)
            {
                NearbyChunks[(int)Direction.forward].GetComponent<Chunk>().FlagToUpdate();
            }
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

        //根据坐标获取方块所处的区块
        public ushort GetBlock(int x,int y,int z)
        {
            if (x < 0)
            {
                if (NearbyChunks[(int)Direction.left] != null)
                {
                    return NearbyChunks[(int)Direction.left].GetBlock(x + SideLength, y, z);
                }
                else return ushort.MaxValue;
            }else if (x >= SideLength)
            {
                if (NearbyChunks[(int)Direction.right] != null)
                {
                    return NearbyChunks[(int)Direction.right].GetBlock(x - SideLength, y, z);
                }
                else return ushort.MaxValue;
            }else if (z < 0)
            {
                if (NearbyChunks[(int)Direction.back] != null)
                {
                    return NearbyChunks[(int)Direction.back].GetBlock(x, y , z + SideLength);
                }
                else return ushort.MaxValue;
            }else if (z >= SideLength)
            {
                if (NearbyChunks[(int)Direction.back] != null)
                {
                    return NearbyChunks[(int)Direction.forward].GetBlock(x, y, z - SideLength);
                }
                else return ushort.MaxValue;
            }
            else
            {
                return BlockData[ (z * SideLength) + x];
            } 
        }

        public ushort GetBlock(Index index)
        {
            return GetBlock(index.x, index.y, index.z);
        }
        public ushort GetBlockSimpleData(int index)
        {
            return BlockData[index];
        }
        public ushort GetBlockSimpleData(int x,int y , int z)
        {
            return BlockData[(z * SideLength) + x];
        }
        public ushort GetBlockSimpleData(Index index)
        {
            return BlockData[(index.z * SideLength) + index.x];
        }

        //set Block
        public void SetBlockSimple(int rawIndex,ushort data)
        {
            BlockData[rawIndex] = data;
        }

        public void SetBlockSimple(int x,int y,int z,ushort data)
        {
            BlockData[ (z * SideLength) + x] = data;
        }
        public void SetBlockSimple(Index index,ushort data)
        {
            BlockData[(index.z * SideLength) + index.x] = data;
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
            int z = ChunkIndex.z;
            if (NearbyChunks[0] == null)
                NearbyChunks[0] = ChunkManager.GetChunkComponent(x + 1, y, z);
            if (NearbyChunks[1] == null)
                NearbyChunks[1] = ChunkManager.GetChunkComponent(x - 1, y, z);
            if (NearbyChunks[2] == null)
                NearbyChunks[2] = ChunkManager.GetChunkComponent(x, y, z + 1);
            if (NearbyChunks[3] == null)
                NearbyChunks[3] = ChunkManager.GetChunkComponent(x, y, z - 1);

        }
        public Index GetNearbyChunkIndex(Index index,Direction direction)
        {
            return GetNearbyChunkIndex(index.x, index.y, index.z, direction);
        }

        public Index GetNearbyChunkIndex(int x, int y, int z, Direction direction)
        {
            if (direction == Direction.left)
                return new Index(x - 1, y, z);
            else if (direction == Direction.right)
                return new Index(x + 1, y, z);
            else if (direction == Direction.back)
                return new Index(x, y, z - 1);
            else if (direction == Direction.forward)
                return new Index(x, y, z + 1);
            else
            {
                Debug.LogError("Get Nearby Chunk Error");
                return new Index(x, y, z);
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