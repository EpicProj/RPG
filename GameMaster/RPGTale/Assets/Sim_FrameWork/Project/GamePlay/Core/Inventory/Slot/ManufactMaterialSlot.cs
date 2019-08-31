﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork
{
    public class ManufactMaterialSlot : Slot
    {
        public FormulaModule.MaterialProductType SlotType;

        private const string MATERIAL_PREFAB_PATH = "Assets/Prefabs/Object/MaterialItem.prefab";

        public void InitManuMaterialSlot(Material ma,int amount)
        {
            if (transform.childCount == 0)
            {
                GameObject itemObj = ObjectManager.Instance.InstantiateObject(MATERIAL_PREFAB_PATH);
                itemObj.transform.SetParent(transform, false);
                itemObj.transform.localScale = Vector3.one;
                itemObj.transform.localPosition = Vector3.zero;
                itemObj.GetComponent<SlotItem>().SetMaterialData(ma,amount);
            }

        }





    }
}