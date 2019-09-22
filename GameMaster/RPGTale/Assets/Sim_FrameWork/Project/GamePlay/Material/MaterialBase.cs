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

        MaterialDataModel dataModel;



        public MaterialInfo(Material ma)
        {
            ID = ma.MaterialID;
            dataModel = new MaterialDataModel();
            dataModel.Create(ID);

        }





    }

}