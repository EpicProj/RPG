using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork
{
    public class InventoryBase : MonoBehaviour
    {
        protected Slot[] slotList;
        private CanvasGroup canvasGroup;

        public virtual void Start()
        {
            slotList = GetComponentsInChildren<Slot>();
            canvasGroup = GetComponent<CanvasGroup>();
        }

        void Update() { }

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
                slot.InitFunctionBlock(block);
            }
            return true;
        }


        private Slot FindEmptySlot()
        {
            foreach(Slot slot in slotList)
            {
                if (slot.transform.childCount == 0)
                {
                    return slot;
                }
            }
            return null;
        }

        public void Show()
        {
            canvasGroup.blocksRaycasts = true;
            canvasGroup.alpha = 1;
        }
        public void Hide()
        {
            canvasGroup.blocksRaycasts = false;
            canvasGroup.alpha = 0;
        }


    }

}