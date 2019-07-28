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

        //区块大小
        public int SideLength;
        private int SquaredSideLength;

        //Queue
        public bool BlockDown;
        public bool FlaggedToUpdate;

        private ChunkMeshCreator MeshCreator;
        public GameObject MeshContainer;
        public GameObject ChunkCollider;

        public void Awake()
        {
            ChunkIndex = new Index(transform.position);
            SideLength = MapGenerator.ChunkSideLegth;
            SquaredSideLength = SideLength * SideLength;
            //[0] Up ; [1] down ; [2] right ; [3] left ; [4] forward ; [5] back
            NearbyChunks = new Chunk[6];

            MeshCreator = GetComponent<ChunkMeshCreator>();

            ChunkManager.RegisterChunk(this);
        }


        public void FlagToRemove()
        {
            FlaggedToRemove = true;
        }
        public void FlagToUpdate()
        {
            FlaggedToUpdate = true;
        }

        public void RebuildMesh()
        {
            MeshCreator.RebuildMesh();
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
            }else if (y < 0)
            {
                if (NearbyChunks[(int)Direction.down] != null)
                {
                    NearbyChunks[(int)Direction.down].SetBlock(x, y + SideLength, z, data, updateMesh);
                    return;
                }
            }else if (y >= SideLength)
            {
                if (NearbyChunks[(int)Direction.up] != null)
                {
                    NearbyChunks[(int)Direction.up].SetBlock(x, y - SideLength, z, data, updateMesh);
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
            
            BlockData[(z * SquaredSideLength) + (y * SideLength) + x] = data;
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
            else if (y == 0 && NearbyChunks[(int)Direction.down] != null)
            {
                NearbyChunks[(int)Direction.down].GetComponent<Chunk>().FlagToUpdate();
            }
            else if (y == SideLength - 1 && NearbyChunks[(int)Direction.up] != null)
            {
                NearbyChunks[(int)Direction.up].GetComponent<Chunk>().FlagToUpdate();
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
            }else if (y < 0)
            {
                if (NearbyChunks[(int)Direction.down] != null)
                {
                    return NearbyChunks[(int)Direction.down].GetBlock(x , y + SideLength, z);
                }
                else return ushort.MaxValue;
            }else if (y >= SideLength)
            {
                if (NearbyChunks[(int)Direction.up] != null)
                {
                    return NearbyChunks[(int)Direction.up].GetBlock(x, y - SideLength, z);
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
                return BlockData[(z * SquaredSideLength) + (y * SideLength) + x];
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
            return BlockData[(z * SquaredSideLength) + (y * SideLength) + x];
        }
        public ushort GetBlockSimpleData(Index index)
        {
            return BlockData[(index.z * SquaredSideLength) + (index.y * SideLength) + index.x];
        }

        //获取周边区块信息
        public void GetNearbyChunks()
        {
            int x = ChunkIndex.x;
            int y = ChunkIndex.y;
            int z = ChunkIndex.z;
            if (NearbyChunks[0] == null)
                NearbyChunks[0] = ChunkManager.GetChunkComponent(x, y + 1, z);
            if (NearbyChunks[1] == null)
                NearbyChunks[1] = ChunkManager.GetChunkComponent(x, y - 1, z);
            if (NearbyChunks[2] == null)
                NearbyChunks[2] = ChunkManager.GetChunkComponent(x + 1, y, z);
            if (NearbyChunks[3] == null)
                NearbyChunks[3] = ChunkManager.GetChunkComponent(x - 1, y, z);
            if (NearbyChunks[4] == null)
                NearbyChunks[4] = ChunkManager.GetChunkComponent(x, y, z + 1);
            if (NearbyChunks[5] == null)
                NearbyChunks[5] = ChunkManager.GetChunkComponent(x, y, z - 1);

        }



        #region Data
        public int GetDataLength()
        {
            return BlockData.Length;
        }

        #endregion
    }
}