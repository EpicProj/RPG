using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork
{
    public class BlockInfo
    {
        public Index index;
        public Index nearbyIndex;
        public Chunk chunk;


        public BlockInfo(int x, int y, int z, Chunk chunk)
        {
            this.index.x = x;
            this.index.y = y;
            this.index.z = z;
            this.chunk = chunk;
        }
        public BlockInfo(int x,int y,int z,int xn,int yn,int zn,Chunk chunk)
        {
            this.index.x = x;
            this.index.y = y;
            this.index.z = z;

            this.nearbyIndex.x = xn;
            this.nearbyIndex.y = yn;
            this.nearbyIndex.z = zn;

            this.chunk = chunk;
        }

        public BlockInfo(Index index, Chunk chunk)
        {
            this.index = index;
            this.chunk = chunk;
        }
        public BlockInfo(Index index,Index nindex,Chunk chunk)
        {
            this.index = index;
            this.nearbyIndex = nindex;
            this.chunk = chunk;
        }

        public ushort GetBlock()
        {
            return chunk.GetBlock(index);
        }
        public BaseBlock GetBlockType()
        {
            return MapGenerator.GetBlockType(chunk.GetBlock(index));
        }
        public ushort GetNearbyBlock()
        {
            return chunk.GetBlock(nearbyIndex);
        }
        public BaseBlock GetNearbyBlockType()
        {
            return MapGenerator.GetBlockType(chunk.GetBlock(nearbyIndex));
        }
        public void SetBlock(ushort data,bool updateMesh)
        {
            chunk.SetBlock(index, data, updateMesh);
        }




    }
}