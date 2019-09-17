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
        public string Name;
        public int ID;
        public string Desc;
        public Sprite Icon;



        public MaterialInfo(Material ma)
        {
            Name = MaterialModule.GetMaterialName(ma);
            ID = ma.MaterialID;
            Desc = MaterialModule.GetMaterialDesc(ma);
            Icon = MaterialModule.GetMaterialSprite(ma.MaterialID);

        }





    }

}