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


        public BlockInfo(int x, int y, Chunk chunk)
        {
            this.index.x = x;
            this.index.y = y;
            this.chunk = chunk;
        }
        public BlockInfo(int x,int y,int xn,int yn,Chunk chunk)
        {
            this.index.x = x;
            this.index.y = y;

            this.nearbyIndex.x = xn;
            this.nearbyIndex.y = yn;

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
        public void SetBlock(ushort data)
        {
            chunk.SetBlock(index, data);
        }




    }
}