﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

namespace Sim_FrameWork
{
    public class Slot : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler,IPointerDownHandler
    {
        private const string FUNCTIONBLOCK_PREFAB_PATH= "Assets/Prefabs/Object/ItemUIPrefab.prefab";
        private const string DISTRICT_PREFAB_PATH = "Assets/Prefabs/Object/District.prefab";




        public void InitFunctionBlockSlot(FunctionBlock block)
        {
            if (transform.childCount == 0)
            {
                GameObject itemObj = ObjectManager.Instance.InstantiateObject(FUNCTIONBLOCK_PREFAB_PATH);
                itemObj.transform.SetParent(transform,false);
                itemObj.transform.localScale = Vector3.one;
                itemObj.transform.localPosition = Vector3.zero;
                itemObj.GetComponent<SlotItem>().SetFunctionBlock(block);
            }
            else
            {
                transform.GetChild(0).GetComponent<SlotItem>().AddAmount();
            }
        }


        public void InitDistrictAreaSlot(DistrictData data ,DistrictSlotType slotType ,Sprite sp)
        {
            if (transform.childCount == 1)
            {
                //Contain Empty Info
                GameObject itemObj = ObjectManager.Instance.InstantiateObject(DISTRICT_PREFAB_PATH);
                itemObj.transform.SetParent(transform, false);
                itemObj.transform.localScale = Vector3.one;
                itemObj.transform.localPosition = Vector3.zero;
                itemObj.GetComponent<SlotItem>().SetDistrictArea(data, slotType, sp);
            }
            else
            {

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
                if(InventoryManager.Instance.IsPickedItem==false && transform.childCount > 0)
                {
                    SlotItem currentItem = transform.GetChild(0).GetComponent<SlotItem>();
                }
            }

            if (eventData.button != PointerEventData.InputButton.Left)
                return;
        }


    }
}