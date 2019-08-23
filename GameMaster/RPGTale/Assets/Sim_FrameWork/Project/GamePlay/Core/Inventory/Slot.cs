using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

namespace Sim_FrameWork
{
    public class Slot : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler,IPointerDownHandler
    {
        public GameObject ItemObj;
     

        public void InitFunctionBlock(FunctionBlock block)
        {
            if (transform.childCount == 0)
            {
                GameObject itemObj = Instantiate(ItemObj) as GameObject;
                itemObj.transform.SetParent(transform);
                itemObj.transform.localScale = Vector3.one;
                itemObj.transform.localPosition = Vector3.zero;
                itemObj.GetComponent<SlotItem>().SetFunctionBlock(block);
            }
            else
            {
                transform.GetChild(0).GetComponent<SlotItem>().AddAmount();
            }
        }


        public void OnPointerExit(PointerEventData eventData)
        {

        }

        public void OnPointerEnter(PointerEventData eventData)
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
                if(GameManager.Instance.IsPickedItem==false && transform.childCount > 0)
                {
                    SlotItem currentItem = transform.GetChild(0).GetComponent<SlotItem>();
                }
            }

            if (eventData.button != PointerEventData.InputButton.Left)
                return;
        }


    }
}