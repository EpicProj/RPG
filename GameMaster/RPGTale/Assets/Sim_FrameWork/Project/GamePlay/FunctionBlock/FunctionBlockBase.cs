﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork
{
    public class FunctionBlockBase : MonoBehaviour
    {

        public FunctionBlock functionBlock;
        public Dictionary<Vector2, DistrictAreaInfo> _currentDistrictDataDic = new Dictionary<Vector2, DistrictAreaInfo>();

        public FunctionBlockInfoData info;

        private BoxCollider BlockCollider;
        private FunctionBlockModifier blockModifier;
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
            //TODO
            functionBlock = FunctionBlockModule.Instance.GetFunctionBlockByBlockID(100);
            //TODO
            blockModifier = GetComponent<FunctionBlockModifier>();
            info = FunctionBlockInfoData.Create(GetBlockPos(),functionBlock,blockModifier);
            InitAreaDetail();

            InitBaseInfo();

        }

        private void InitBaseInfo()
        {
            //Set Collider
            BlockCollider = gameObject.GetComponent<BoxCollider>();
            SetBlockColliderSize(FunctionBlockModule.Instance.InitFunctionBlockBoxCollider<FunctionBlock_Manufacture>(functionBlock));
        }

        //Action
        public virtual void OnPlaceFunctionBlock()
        {
            FunctionBlockModule.Instance.PlaceFunctionBlock(info.BlockID, BlockPos);
        }
        public virtual void OnHoldFunctionBlock() { }
        public virtual void OnDestoryFunctionBlock() { }
        public virtual void OnSelectFunctionBlock()
        {
        }

        #region InitBaseInfo

        public void InitAreaDetail()
        {
            _currentDistrictDataDic = FunctionBlockModule.Instance.GetFuntionBlockAreaDetailDefaultData<FunctionBlock_Manufacture>(functionBlock);

        }
        private void SetBlockColliderSize(Vector3 size)
        {
            BlockCollider.size = size;
        }
        private Vector3 GetBlockPos()
        {
            Vector3 pos = new Vector3();
            pos = transform.position;
            return pos;
        }

        #endregion 
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


       
    }



    [System.Serializable]
    public class FunctionBlockInfoData
    {
        //BaseInfo
        private static Stack<FunctionBlockInfoData> functionBlockInfoCache = new Stack<FunctionBlockInfoData>();
       
        public string BlockUID { get; set; }
        public int BlockID { get; set; }
        public Vector3 BlockPos;
        public int currentBlockLevel;

        /// <summary>
        /// Block EXP
        /// </summary>
        [SerializeField]
        private int _currentBlockExp;
        public int CurrentBlockExp { get { return _currentBlockExp; } set { AddCurrentBlockEXP(value); } }
        private float _baseEXPRatio = 1;
        public float BaseEXPRatio { get { return _baseEXPRatio; } set { AddBaseEXPRatio(value); } }

        public FunctionBlock block;
        public Vector2 districtAreaMax;
        public Dictionary<Vector2, DistrictAreaInfo> currentDistrictDataDic;
        public FunctionBlockModifier blockModifier = new FunctionBlockModifier();


        /// <summary>
        /// WorkerNum
        /// </summary>
        [SerializeField]
        private int _workerNum;
        public int WorkerNum { get { return _workerNum; } set { AddWorkerNum(value); } }

        private int _maintain;
        public int Maintain { get { return _maintain; } set { } }

        /// <summary>
        /// 基础电力消耗
        /// </summary>
        private float _energyCostNormal;
        public float EnergyCostNormal { get { return _energyCostNormal; } set { AddEnergyCostNormal(value); } }

        private float _energyCostMagic;
        public float EnergyCostMagic { get { return _energyCostNormal; } set { AddEnergyCostMagic(value); } }

        //Manufactory
        public int CurrentFormulaID;
        /// <summary>
        /// current Manu Speed
        /// </summary>
        [SerializeField]
        private float _currentSpeed;
        public float CurrentSpeed { get { return _currentSpeed; } set { AddCurrentSpeed(value); } }


        public List<Dictionary<Material, ushort>> InputMaterialFormulaList;
        public List<Dictionary<Material, ushort>> OutputMaterialFormulaList;
        public List<Dictionary<Material, ushort>> BypruductMaterialFormulaList;




        public static FunctionBlockInfoData Create()
        {
            if (functionBlockInfoCache.Count < 1)
                return new FunctionBlockInfoData();
            FunctionBlockInfoData info = functionBlockInfoCache.Pop();
            return info;
        }
        public static FunctionBlockInfoData Create(Vector3 blockPos, FunctionBlock blockBase , FunctionBlockModifier modifier)
        {

            FunctionBlockInfoData info = new FunctionBlockInfoData();
            info.BlockID = blockBase.FunctionBlockID;
            info.block = blockBase;
            info.BlockPos = blockPos;
            info.blockModifier = modifier;
            //TODO
            return info;
        }


        public void AddCurrentSpeed(float speed)
        {
            _currentSpeed += speed;
            if (_currentSpeed <= 0)
                _currentSpeed = 0;
        }
        public void AddWorkerNum(int num)
        {
            _workerNum += num;
            if (_workerNum <= 0)
                _workerNum = 0;
        }

        public void AddMaintain(int num)
        {

        }
        public void AddEnergyCostNormal(float num)
        {
            _energyCostNormal += num;
            if (_energyCostNormal < 0)
            {
                _energyCostNormal = 0;
            }
        }
        public void AddEnergyCostMagic(float num)
        {
            _energyCostMagic += num;
            if (_energyCostMagic < 0)
            {
                _energyCostMagic = 0;
            }
        }


        public void AddCurrentBlockEXP(int value)
        {

        }

        public void AddBaseEXPRatio(float num)
        {
            _baseEXPRatio += num;
            if (num < 0)
            {
                _baseEXPRatio = 0;
            }
        }

        private static FunctionBlockInfoData Pop()
        {
            if (functionBlockInfoCache.Count < 1)
            {
                FunctionBlockInfoData data = new FunctionBlockInfoData();
                return data;
            }
            FunctionBlockInfoData info = functionBlockInfoCache.Pop();
            return info;
        }

        /// <summary>
        /// Push Stack
        /// </summary>
        /// <param name="data"></param>
        private static void AddStack(FunctionBlockInfoData data)
        {
            functionBlockInfoCache.Push(data);
        }

    }
}