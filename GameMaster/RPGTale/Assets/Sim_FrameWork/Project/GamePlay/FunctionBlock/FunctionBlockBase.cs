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
        public List<List<DistrictData>> _currentDistrictDataList =new List<List<DistrictData>> ();

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
            MaterialModule.Instance.InitData();
            DistrictModule.Instance.InitData();
            functionBlock = FunctionBlockModule.Instance.GetFunctionBlockByBlockID(functionBlockID);
            InitAreaDetail();
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

        public void InitAreaDetail()
        {
            _currentDistrictDataList = FunctionBlockModule.Instance.GetFuntionBlockAreaDetailDefaultData<FunctionBlock_Manufacture>(functionBlock);

        }




    }
}