using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork
{
    public class BlockInfo
    {

        public Index index;
        public Chunk chunk;


        public BlockInfo(int x, int y, int z, Chunk chunk)
        {
            this.index.x = x;
            this.index.y = y;
            this.index.z = z;
            this.chunk = chunk;
        }

        public BlockInfo(Index index, Chunk chunk)
        {
            this.index = index;
            this.chunk = chunk;
        }

        public ushort GetBlock()
        {
            return chunk.GetBlock(index);
        }

    }
}