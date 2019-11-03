using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork
{
    public class BlockGrass : BaseBlockEvents
    {
        public override void OnBlockPlace(BlockInfo info)
        {
            Index nearbyIndex = info.chunk.GetNearbyChunkIndex(info.index, Direction.up);
            if (info.chunk.GetBlock(nearbyIndex) != 0)
            {
                info.chunk.SetBlock(info.index, 1);
            }

            Index below = new Index(info.index.x, info.index.y - 1);
            if (info.GetBlockType().m_Transparency == Transparency.solid && info.chunk.GetBlock(below) == 2)
            {
                info.chunk.SetBlock(below, 1);
            }
        }

    }
}