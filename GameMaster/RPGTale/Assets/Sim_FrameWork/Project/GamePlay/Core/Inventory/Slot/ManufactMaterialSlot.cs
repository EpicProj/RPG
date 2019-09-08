using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Sim_FrameWork
{
    public class ManufactMaterialSlot : Slot
    {
        public FormulaModule.MaterialProductType SlotType;

      

        public void InitManuMaterialSlot(Material ma,int amount)
        {
            if (transform.childCount == 0)
            {
                GameObject itemObj = ObjectManager.Instance.InstantiateObject(UIPath.MATERIAL_PREFAB_PATH);
                itemObj.transform.SetParent(transform, false);
                itemObj.transform.localScale = Vector3.one;
                itemObj.transform.localPosition = Vector3.zero;
                itemObj.GetComponent<SlotItem>().SetMaterialData(ma,amount);
            }

        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            if(transform.childCount > 0)
            {
                Material ma= transform.GetChild(0).GetComponent<SlotItem>().material;
                InventoryManager.Instance.ShowMaterialInfoTip(ma);
            }
           
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            InventoryManager.Instance.HideMaterialInfoTip();
        }



    }
}