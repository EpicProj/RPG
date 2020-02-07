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
        public bool LoadSaveData(MaterialStorageSaveData saveData)
        {
            materialStorageDataDic = saveData.materialStorageSaveDic;
            return true;
        }
    }

    /// <summary>
    /// Save Data
    /// </summary>
    public class MaterialStorageSaveData
    {
        public Dictionary<int, int> materialStorageSaveDic = new Dictionary<int, int>();

        public static MaterialStorageSaveData CreateSave()
        {
            MaterialStorageSaveData data = new MaterialStorageSaveData();
            data.materialStorageSaveDic = PlayerManager.Instance.playerData.materialStorageData.materialStorageDataDic;
            return data;
        }
    }
}