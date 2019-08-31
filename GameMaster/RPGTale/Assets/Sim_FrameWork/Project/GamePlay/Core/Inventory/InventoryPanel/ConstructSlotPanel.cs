using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork
{
    public class ConstructSlotPanel : InventoryBase
    {
        private const string FUNCTIONBLOCK_PREFAB_PATH = "Assets/Prefabs/Object/ItemUIPrefab.prefab";
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


        public void InitFunctionBlockSlot(FunctionBlock block)
        {
            if (transform.childCount == 0)
            {
                GameObject itemObj = ObjectManager.Instance.InstantiateObject(FUNCTIONBLOCK_PREFAB_PATH);
                itemObj.transform.SetParent(transform, false);
                itemObj.transform.localScale = Vector3.one;
                itemObj.transform.localPosition = Vector3.zero;
                itemObj.GetComponent<SlotItem>().SetFunctionBlock(block);
            }
            else
            {
                transform.GetChild(0).GetComponent<SlotItem>().AddAmount();
            }
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