using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork
{
    public class BlockEvents : MonoBehaviour
    {
        public virtual void OnMouseDown(int btn,BlockInfo info) { }
        public virtual void OnMouseUp(int btn,BlockInfo info) { }
        public virtual void OnMouseHold(int btn, BlockInfo info) { }

        public virtual void OnLook( BlockInfo info) { }

        public virtual void OnBlockDestory( BlockInfo info) { }
        public virtual void OnBlockPlace(BlockInfo info) { }
        public virtual void OnBlockChange(BlockInfo info) { }

        public virtual void OnBlockEnter(GameObject enterObj, BlockInfo info) { }
        public virtual void OnBlockStay(GameObject stayObj, BlockInfo info) { }
    }
}