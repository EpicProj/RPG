using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork
{
    public class MaterialBase
    {

        public int MaterialID;
        public int MaterialUID;

        public virtual void InitData() { }

        public virtual void Awake() { }

        //Action
        public virtual void OnSelectMaterial() { }
        public virtual void OnHoldMaterial() { }
        public virtual void OnDestoryMaterial() { }

    }

    public class MaterialInfo
    {
        public int ID;
        public Material material;
        public MaterialDataModel dataModel;
        public MaterialType mainType;
        public MaterialSubType subType;


        public MaterialInfo(int id)
        {
            material = MaterialModule.GetMaterialByMaterialID(id);
            if (material != null)
            {
                ID = id;
                dataModel = new MaterialDataModel();
                dataModel.Create(ID);
                mainType = MaterialModule.GetMaterialType(id);
                subType = MaterialModule.GetMaterialSubType(id);
            }
       
        }
    }

    public class MaterialStorageItem
    {
        public MaterialInfo info;
        public int count;

        public MaterialStorageItem(MaterialInfo ma,int count)
        {
            this.info = ma;
            this.count = count;
        }
    }

}