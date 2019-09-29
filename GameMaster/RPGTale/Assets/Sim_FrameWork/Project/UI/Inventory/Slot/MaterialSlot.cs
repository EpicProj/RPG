using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Sim_FrameWork.UI
{
    public class MaterialSlot : Slot
    {
        public MaterialStorageItem storeData;
        public Text Name;
        public Image Icon;
        public Text Num;

        public void SetUpMaterialItem(MaterialStorageItem itemData)
        {
            storeData = itemData;
            Name.text = itemData.info.dataModel.Name;
            Icon.sprite = itemData.info.dataModel.Icon;
            Num.text = itemData.count.ToString();
        }

        public void AddMaterialNum(MaterialStorageItem itemData)
        {
            storeData = itemData;
            if (itemData.count <= 0)
            {
                Destroy(this.gameObject);
                return;
            }
            Num.text = itemData.count.ToString();
        }


        public override void OnPointerEnter(PointerEventData eventData)
        {
            InventoryManager.Instance.ShowMaterialInfoTip(storeData.info.dataModel);
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            InventoryManager.Instance.HideMaterialInfoTip();
        }


        

    }
}