using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork
{
    public class FactoryBase : MonoBehaviour
    {
        public int factoryID;
        public int factoryUID;



        public virtual void Update() { }
        public virtual void Awake() { }
        

        public virtual void InitData()
        {
            FactoryModule.Instance.InitData();
        }

        //Action
        public virtual void OnPlaceFactory() { }
        public virtual void OnHoldFactory() { }
        public virtual void OnDestoryFactory() { }
        public virtual void OnSelectFactory() { }
        
    }
}