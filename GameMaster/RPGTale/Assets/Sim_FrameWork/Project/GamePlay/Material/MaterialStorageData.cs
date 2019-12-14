using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Sim_FrameWork
{
    public class MaterialStorageData
    {
        public Dictionary<int, MaterialStorageItem> materialStorageDataDic = new Dictionary<int, MaterialStorageItem>();

        public void AddMaterialStoreData(int materialID, ushort count)
        {
            Action<MaterialStorageItem> sendMsg = (m) =>
            {
                UIManager.Instance.SendMessage( new UIMessage(UIMsgType.UpdateWarehouseData, new List<object>(1) { m }));
            };

            if (materialStorageDataDic.ContainsKey(materialID))
            {
                var material = materialStorageDataDic[materialID];
                material.count += count;
                if (material.count > material.info.material.BlockCapacity)
                {
                    //超出上限 ,TODO
                    sendMsg(material);
                }
                else if (material.count <= 0)
                {
                    materialStorageDataDic.Remove(materialID);
                }
                else
                {
                    sendMsg(material);
                }
            }
            else
            {
                MaterialInfo info = new MaterialInfo(materialID);
                if (info.ID != 0)
                {
                    MaterialStorageItem data = new MaterialStorageItem(info, count);
                    materialStorageDataDic.Add(materialID, data);
                    sendMsg(data);
                }
            }

        }




    }
}