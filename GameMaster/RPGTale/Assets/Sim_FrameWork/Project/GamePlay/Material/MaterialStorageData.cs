using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Sim_FrameWork
{
    public class MaterialStorageData
    {
        public Dictionary<int, int> materialStorageDataDic = new Dictionary<int, int>();

        public void AddMaterialStoreData(int materialID, ushort count)
        {
            Action<MaterialStorageItem> sendMsg = (m) =>
            {
                UIManager.Instance.SendMessage( new UIMessage(UIMsgType.UpdateWarehouseData, new List<object>(1) { m }));
            };

            if (materialStorageDataDic.ContainsKey(materialID))
            {
                var maCount = materialStorageDataDic[materialID];
                maCount += count;
                //if (maCount > material.info.material.BlockCapacity)
                //{
                //    //超出上限 ,TODO
                //    sendMsg(material);
                //}
                //else if (material.count <= 0)
                //{
                //    materialStorageDataDic.Remove(materialID);
                //}
                //else
                //{
                //    sendMsg(material);
                //}
            }
            else
            {
                if (MaterialModule.GetMaterialByMaterialID(materialID) != null)
                {
                    materialStorageDataDic.Add(materialID, count);
                    //sendMsg(data);
                }
            }
        }

        public MaterialStorageData() { }
        public MaterialStorageData LoadSaveData(MaterialStorageSaveData saveData)
        {
            MaterialStorageData data = new MaterialStorageData();
            data.materialStorageDataDic = saveData.materialStorageSaveDic;
            return data;
        }
    }

    /// <summary>
    /// Save Data
    /// </summary>
    public class MaterialStorageSaveData
    {
        public Dictionary<int, int> materialStorageSaveDic = new Dictionary<int, int>();

        public MaterialStorageSaveData()
        {
            materialStorageSaveDic = PlayerManager.Instance.playerData.materialStorageData.materialStorageDataDic;
        }
    }
}