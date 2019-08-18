using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork
{
    public class FunctionBlockBase : MonoBehaviour
    {
        public int functionBlockID;
        public string functionBlockUID;
        public FunctionBlock functionBlock;


        public virtual void Update() { }
        public virtual void Awake() { }
        

        public virtual void InitData()
        {
            FunctionBlockModule.Instance.InitData();
            functionBlock = FunctionBlockModule.Instance.GetFunctionBlockByFacotryID(functionBlockID);
        }

        //Action
        public virtual void OnPlaceFunctionBlock()
        {
            functionBlockUID = FunctionBlockModule.Instance.GenerateGUID();
            OnAddFacotry();
        }
        public virtual void OnHoldFunctionBlock() { }
        public virtual void OnDestoryFunctionBlock() { }
        public virtual void OnSelectFunctionBlock() { }

        public virtual void OnAddFacotry()
        {
            FunctionBlockModule.Instance.AddFunctionBlock(functionBlockUID, functionBlock);
        }
        
    }
}