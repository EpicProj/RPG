using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork
{
    public class InventoryBase : MonoBehaviour
    {
        protected Slot[] slotList;
        private CanvasGroup canvasGroup;

        public virtual void Awake()
        {
        }

        public void InitData()
        {
            slotList = GetComponentsInChildren<Slot>();
            canvasGroup = GetComponent<CanvasGroup>();
        }

        void Update() { }


        public Slot FindEmptySlot()
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