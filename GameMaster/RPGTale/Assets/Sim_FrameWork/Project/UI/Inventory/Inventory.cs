using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork.UI
{
    public class InventoryBase : MonoBehaviour
    {
        protected BaseElement[] _elementList;
        private CanvasGroup canvasGroup;

        public virtual void Awake()
        {
        }

        public void InitData()
        {
            _elementList = GetComponentsInChildren<BaseElement>();
            canvasGroup = GetComponent<CanvasGroup>();
        }

        void Update() { }


        public BaseElement FindEmptySlot()
        {
            foreach(BaseElement element in _elementList)
            {
                if (element.transform.childCount == 0)
                {
                    return element;
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