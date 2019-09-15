using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Sim_FrameWork.UI
{
    public class MaterialSlot : Slot
    {
        public MaterialStorageData storeData;
        public Material materialData;
        public Text Name;
        public Image Icon;
        public Text Num;

        public void SetUpMaterialItem(MaterialStorageData itemData)
        {
            storeData = itemData;
            Name.text = MaterialModule.GetMaterialName(itemData.material);
            Icon.sprite = MaterialModule.GetMaterialSprite(itemData.material.MaterialID);
            Num.text = itemData.count.ToString();
        }

        public void AddMaterialNum(MaterialStorageData itemData)
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
            InventoryManager.Instance.ShowMaterialInfoTip(storeData.material);
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            InventoryManager.Instance.HideMaterialInfoTip();
        }


        

    }
}