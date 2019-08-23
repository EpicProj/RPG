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

        public int currentBlockLevel;
        public int currentBlockExp;
         

        //Base Info
        public Vector3 BlockPos;


        public virtual void Update() { }
        public virtual void Awake()
        {
            InitData();
        }
        

        public virtual void InitData()
        {
            FunctionBlockModule.Instance.InitData();
            functionBlock = FunctionBlockModule.Instance.GetFunctionBlockByBlockID(functionBlockID);
        }

        //Action
        public virtual void OnPlaceFunctionBlock()
        {
            FunctionBlockModule.Instance.PlaceFunctionBlock(functionBlockID, BlockPos);
        }
        public virtual void OnHoldFunctionBlock() { }
        public virtual void OnDestoryFunctionBlock() { }
        public virtual void OnSelectFunctionBlock()
        {



        }


        
    }
}