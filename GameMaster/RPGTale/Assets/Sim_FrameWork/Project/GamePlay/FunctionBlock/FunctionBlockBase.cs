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
        public Dictionary<Vector2, DistrictAreaInfo> _currentDistrictDataDic = new Dictionary<Vector2, DistrictAreaInfo>();

        public int currentBlockLevel;
        public int currentBlockExp;

        private BoxCollider BlockCollider;
        RaycastHit hit;

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
            BlockCollider = gameObject.GetComponent<BoxCollider>();
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
            _currentDistrictDataDic = FunctionBlockModule.Instance.GetFuntionBlockAreaDetailDefaultData<FunctionBlock_Manufacture>(functionBlock);

        }

        public void CheckMouseButtonDown(string UIPath, params object[] param )
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit))
                {
                    Debug.Log(hit.collider.gameObject.name);
                    UIManager.Instance.PopUpWnd( UIPath, true, param);
                }
            }
        }


        public void SetBlockColliderSize(Vector3 size)
        {
            BlockCollider.size = size;
        }


    }
}