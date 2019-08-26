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

        public FuntionBlockInfoData info;

        public int currentBlockLevel;
        public int currentBlockExp;

        private BoxCollider BlockCollider;
        RaycastHit hit;

        //Base Info
        public Vector3 BlockPos;


        public virtual void Update() { }
        public virtual void FixedUpdate() { }
        public virtual void Awake()
        {
            InitData();
        }
        

        public virtual void InitData()
        {
            FunctionBlockModule.Instance.InitData();
            MaterialModule.Instance.InitData();
            DistrictModule.Instance.InitData();
            FormulaModule.Instance.InitData();
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



    [System.Serializable]
    public class FuntionBlockInfoData
    {
        //BaseInfo
        public Vector3 BlockPos;

        public string BlockUID;
        public int BlockID;

        public FunctionBlock block;
        public Vector2 districtAreaMax;
        public Dictionary<Vector2, DistrictAreaInfo> currentDistrictDataDic;

        public FunctionBlockModifier blockModifierList;


        //Manufactory
        public int CurrentFormulaID;
        /// <summary>
        /// current Manu Speed
        /// </summary>
        private float _currentSpeed;
        public float CurrentSpeed;


        public List<Dictionary<Material, ushort>> InputMaterialFormulaList;
        public List<Dictionary<Material, ushort>> OutputMaterialFormulaList;
        public List<Dictionary<Material, ushort>> BypruductMaterialFormulaList;


        public void AddCurrentSpeed(float speed)
        {
            _currentSpeed += speed;
            if (_currentSpeed <= 0)
                _currentSpeed = 0;
        }


    }
}