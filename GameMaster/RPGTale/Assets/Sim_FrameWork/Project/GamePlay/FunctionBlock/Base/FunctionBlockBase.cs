using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork
{
    public class FunctionBlockBase : MonoBehaviour
    {

        public FunctionBlock functionBlock;

        public FunctionBlockInfoData info;

        private BoxCollider BlockCollider;
        private FunctionBlockModifier blockModifier;
        RaycastHit hit;

        public int currentFormulaID;

        public virtual void Update() { }
        public virtual void FixedUpdate() { }
        public virtual void Awake()
        {
            InitData();
        }
        

        public virtual void InitData()
        {
            //TODO
            functionBlock = FunctionBlockModule.Instance.GetFunctionBlockByBlockID(100);
            //TODO
            blockModifier = GetComponent<FunctionBlockModifier>();
            info = FunctionBlockInfoData.CreateBaseInfo(GetBlockPos(),functionBlock,blockModifier);
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
            FunctionBlockModule.Instance.PlaceFunctionBlock(info.BlockID, info.BlockPos);
        }
        public virtual void OnHoldFunctionBlock() { }
        public virtual void OnDestoryFunctionBlock() { }
        public virtual void OnSelectFunctionBlock()
        {
        }



        #region InitBaseInfo

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

        public string BlockUID;
        public int BlockID;
        public Vector3 BlockPos;

        /// <summary>
        /// 区划历史记录
        /// </summary>
        public FunctionBlockHistory blockHistory;

        public FunctionBlockLevelInfo levelInfo = new FunctionBlockLevelInfo ();


        public FunctionBlock block;
        public Vector2 districtAreaMax;
        /// <summary>
        /// 当前区划信息
        /// </summary>
        public Dictionary<Vector2, DistrictAreaInfo> currentDistrictDataDic;
        /// <summary>
        /// 区划底信息
        /// </summary>
        public Dictionary<Vector2, DistrictAreaBase> currentDistrictBaseDic;
        public FunctionBlockModifier blockModifier = new FunctionBlockModifier();


        /// <summary>
        /// WorkerNum
        /// </summary>
        [SerializeField]
        private int _workerNum;
        public int WorkerNum { get { return _workerNum; }  set {  } }

        private int _maintain;
        public int Maintain { get { return _maintain; }  set { } }

        /// <summary>
        /// 基础电力消耗
        /// </summary>
        [SerializeField]
        private float _energyCostNormal;
        public float EnergyCostNormal { get { return _energyCostNormal; }  set { } }
        [SerializeField]
        private float _energyCostMagic;
        public float EnergyCostMagic { get { return _energyCostNormal; }  set { } }

    
        /// <summary>
        /// current Manu Speed
        /// </summary>
        [SerializeField]
        private float _currentSpeed;
        public float CurrentSpeed { get { return _currentSpeed; }  set { _currentSpeed = value; } }

        public ManufactFormulaInfo formulaInfo;
        public List<DistrictUnlockData> districtUnlockDataList;
        public List<DistrictData> ActiveDistrictBuildList=new List<DistrictData> ();


        public static FunctionBlockInfoData Create()
        {
            if (functionBlockInfoCache.Count < 1)
                return new FunctionBlockInfoData();
            FunctionBlockInfoData info = functionBlockInfoCache.Pop();
            return info;
        }
        public static FunctionBlockInfoData CreateBaseInfo(Vector3 blockPos, FunctionBlock blockBase , FunctionBlockModifier modifier)
        {

            FunctionBlockInfoData info = new FunctionBlockInfoData();
            info.BlockID = blockBase.FunctionBlockID;
            info.block = blockBase;
            info.BlockPos = blockPos;
            info.blockModifier = modifier;
            info.BlockUID = FunctionBlockModule.Instance.GenerateGUID(blockBase);
            info.districtUnlockDataList = FunctionBlockModule.Instance.GetManuBlockDistrictUnlockData(blockBase.FunctionBlockID);
            info.levelInfo.BlockEXPMap = FunctionBlockModule.Instance.GetBlockEXPMapData(info.block.FunctionBlockID);
            info.levelInfo.CurrentBlockMaxEXP = FunctionBlockModule.Instance.GetCurrentLevelEXP(info.levelInfo.BlockEXPMap, info.levelInfo.currentBlockLevel);
            //Set active district build
            for (int i = 0; i < info.districtUnlockDataList.Count; i++)
            {
                if (info.districtUnlockDataList[i].UnlockDefault == true)
                {
                    info.ActiveDistrictBuildList.Add(DistrictModule.Instance.GetDistrictDataByKey(info.districtUnlockDataList[i].DistrictID));
                }
            }
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


 

        public void ClearBlockInfoData()
        {

            AddStack(this);
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
    public class FunctionBlockLevelInfo
    {

        /// <summary>
        /// Block EXP
        /// </summary>
        [SerializeField]
        private int _currentBlockExp = 0;
        public int CurrentBlockExp { get { return _currentBlockExp; } protected set { } }
        public int CurrentBlockMaxEXP { get; set; }

        private float _baseEXPRatio = 1;
        public float BaseEXPRatio { get { return _baseEXPRatio; } protected set { } }
        // CurrentLevel
        public int currentBlockLevel = 1;
        //EXPMap
        public List<int> BlockEXPMap;
        //InhertLevel
        public string InherentLevel;

        public void AddCurrentBlockEXP(int value)
        {
            _currentBlockExp += (int)(value * _baseEXPRatio);
            int currentMaxEXP = FunctionBlockModule.Instance.GetCurrentLevelEXP(BlockEXPMap, currentBlockLevel);
            if (_currentBlockExp > currentMaxEXP)
            {
                currentBlockLevel++;
                CurrentBlockMaxEXP = FunctionBlockModule.Instance.GetCurrentLevelEXP(BlockEXPMap, currentBlockLevel);
                _currentBlockExp -= currentMaxEXP;
            }
        }

        public void AddBaseEXPRatio(float num)
        {
            _baseEXPRatio += num;
            if (num < 0)
            {
                _baseEXPRatio = 0;
            }
        }


    }

}