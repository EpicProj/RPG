using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sim_FrameWork
{
    public class MaterialSlot : Slot
    {
        public MaterialStorageData storeData;
        public Text Name;
        public Image Icon;
        public Text Num;

        public void SetUpMaterialItem(MaterialStorageData itemData)
        {
            storeData = itemData;
            Name.text = MaterialModule.Instance.GetMaterialName(itemData.material);
            Icon.sprite = MaterialModule.Instance.GetMaterialSprite(itemData.material.MaterialID);
            Num.text = itemData.count.ToString();
        }


    }
}