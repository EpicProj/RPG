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

        public Dictionary<GameObject, MaterialStorageData> allObjDic = new Dictionary<GameObject, MaterialStorageData>();

        public override void Awake()
        {


        }


        public void InitMaterialContent(PlayerData.WareHouseInfo info)
        {
            currentSelectMainType = info.currentSelectMainType;
            currentSelectSubType = info.currentSelectSubType;
            if (info.currentSelectMainType == null || info.currentSelectSubType == null || info.materialStorageDataList == null)
                return;
            List<MaterialStorageData> currentData = GetCurrentSubTypeMaterial(info);
            //Clear Content
            foreach (Transform trans in this.transform)
            {
                GameObject.Destroy(trans.gameObject);
            }
            allObjDic.Clear();
            for (int i = 0; i < currentData.Count; i++)
            {
                GameObject obj = ObjectManager.Instance.InstantiateObject(UIPath.MATERIAL_WAREHOUSE_PREFAB_PATH);
                MaterialSlot slot = obj.GetComponent<MaterialSlot>();
                slot.SetUpMaterialItem(currentData[i]);
                obj.transform.SetParent(this.transform, false);
                allObjDic.Add(obj, currentData[i]);
            }
        }

        public void AddMaterial(MaterialStorageData data)
        {
            if (allObjDic.ContainsValue(data) == false && currentSelectMainType.Type == "Total")
            {
                GameObject obj = ObjectManager.Instance.InstantiateObject(UIPath.MATERIAL_WAREHOUSE_PREFAB_PATH);
                MaterialSlot slot = obj.GetComponent<MaterialSlot>();
                slot.SetUpMaterialItem(data);
                obj.transform.SetParent(this.transform, false);
                allObjDic.Add(obj, data);
            }
            foreach(KeyValuePair<GameObject,MaterialStorageData> kvp in allObjDic)
            {
                if (kvp.Value == data)
                {
                    kvp.Key.GetComponent<MaterialSlot>().AddMaterialNum(data);
                }
            }
        }


        public List<MaterialStorageData> GetCurrentSubTypeMaterial(PlayerData.WareHouseInfo info)
        {
            List<MaterialStorageData>  currentTypeList = new List<MaterialStorageData>();
            if (info.currentSelectMainType.Type == "Total")
            {
                //全部
                currentTypeList = info.materialStorageDataList;
                return currentTypeList;
            }else if (info.currentSelectSubType.Type == "Total")
            {
                foreach(var item in info.materialStorageDataList)
                {
                    if (item.mainType == info.currentSelectMainType)
                    {
                        currentTypeList.Add(item);
                    }
                }
                return currentTypeList;
            }
            else
            {
                foreach(var item in info.materialStorageDataList)
                {
                    if (item.subType == info.currentSelectSubType)
                    {
                        currentTypeList.Add(item);
                    }
                }
                return currentTypeList;
            }
        }

    }
}