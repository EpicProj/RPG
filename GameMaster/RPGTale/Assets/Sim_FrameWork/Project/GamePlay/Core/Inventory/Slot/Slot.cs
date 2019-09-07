using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

namespace Sim_FrameWork
{
    public class Slot : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler,IPointerDownHandler,IDragHandler,IBeginDragHandler,IEndDragHandler
    {


        public virtual void OnPointerExit(PointerEventData eventData)
        {

        }

        public virtual void OnPointerEnter(PointerEventData eventData)
        {
            if (transform.childCount > 0)
            {
                //ShowTip
            }
        }

        public virtual void OnPointerDown(PointerEventData eventData)
        {
            if(eventData.button == PointerEventData.InputButton.Right)
            {
                if(InventoryManager.Instance.IsPickedItem==false && transform.childCount > 0)
                {
                    SlotItem currentItem = transform.GetChild(0).GetComponent<SlotItem>();
                }
            }

            if (eventData.button != PointerEventData.InputButton.Left)
                return;
        }
        public virtual void OnBeginDrag(PointerEventData eventData)
        {

        }
        public virtual void  OnDrag(PointerEventData eventData)
        {

        }
        public virtual void OnEndDrag(PointerEventData eventData)
        {

        }
    

    }
}