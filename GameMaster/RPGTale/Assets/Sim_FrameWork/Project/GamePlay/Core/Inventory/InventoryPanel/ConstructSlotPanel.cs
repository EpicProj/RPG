using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork
{
    public class ConstructSlotPanel : InventoryBase
    {
        public override void Awake()
        {
            base.Awake();
            InitData();
            InitFunctionBlock(100);
        }


        public bool InitFunctionBlock(int blockID)
        {
            FunctionBlock block = FunctionBlockModule.Instance.GetFunctionBlockByBlockID(blockID);
            return InitFunctionBlock(block);
        }
        public bool InitFunctionBlock(FunctionBlock block)
        {
            if (block == null)
            {
                Debug.Log("Init FunctionBlock Fail Block ID=" + block.FunctionBlockID);
                return false;
            }
            Slot slot = FindEmptySlot();
            if (slot == null)
            {
                Debug.Log("No Empty Slot");
                return false;
            }
            else
            {
                slot.InitFunctionBlockSlot(block);
            }
            return true;
        }

    }
}