using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork {
    public class BaseBlockEvents : BlockEvents {

        public override void OnMouseDown(int btn, BlockInfo info)
        {
            if (btn == 0)
            {
                BaseBlock.DestoryBlock(info);
            }
        }
    }
}