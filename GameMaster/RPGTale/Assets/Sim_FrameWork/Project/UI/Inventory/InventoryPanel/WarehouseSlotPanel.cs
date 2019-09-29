using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sim_FrameWork.UI
{
    public class WarehouseSlotPanel : InventoryBase
    {

        public MaterialConfig.MaterialType currentSelectMainType;
        public MaterialConfig.MaterialType.MaterialSubType currentSelectSubType;

        public Dictionary<GameObject, MaterialStorageItem> allObjDic = new Dictionary<GameObject, MaterialStorageItem>();

        public override void Awake()
        {


        }


        public void InitMaterialContent(PlayerData.WareHouseInfo info)
        {
            currentSelectMainType = info.currentSelectMainType;
            currentSelectSubType = info.currentSelectSubType;
            if (info.currentSelectMainType == null || info.currentSelectSubType == null || info.materialStorageDataDic == null)
                return;
            //Clear Content
            foreach (Transform trans in this.transform)
            {
                GameObject.Destroy(trans.gameObject);
            }
            allObjDic.Clear();
            //for (int i = 0; i < currentData.Count; i++)
            //{
            //    GameObject obj = ObjectManager.Instance.InstantiateObject(UIPath.MATERIAL_WAREHOUSE_PREFAB_PATH);
            //    MaterialSlot slot = obj.GetComponent<MaterialSlot>();
            //    slot.SetUpMaterialItem(currentData[i]);
            //    obj.transform.SetParent(this.transform, false);
            //    allObjDic.Add(obj, currentData[i]);
            //}
        }

        public void AddMaterial(MaterialStorageItem item)
        {
            //if (allObjDic.ContainsValue(item == false && currentSelectMainType.Type == "Total")
            //{
            //    GameObject obj = ObjectManager.Instance.InstantiateObject(UIPath.MATERIAL_WAREHOUSE_PREFAB_PATH);
            //    MaterialSlot slot = obj.GetComponent<MaterialSlot>();
            //    slot.SetUpMaterialItem(item);
            //    obj.transform.SetParent(this.transform, false);
            //    allObjDic.Add(obj, item);
            //}
            //foreach(KeyValuePair<GameObject, MaterialStorageItem> kvp in allObjDic)
            //{
            //    if (kvp.Value == item)
            //    {
            //        kvp.Key.GetComponent<MaterialSlot>().AddMaterialNum(data);
            //    }
            //}
        }


    }
}