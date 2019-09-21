using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

namespace Sim_FrameWork
{
    public class FunctionBlockBase : MonoBehaviour
    {

        public FunctionBlock functionBlock;

        public FunctionBlockInfoData info;

        private BoxCollider BlockCollider;
        private FunctionBlockModifier blockModifier;
        RaycastHit hit;


        public virtual void Update() { }
        public virtual void FixedUpdate() { }
        public virtual void Awake()
        {
            InitData();
        }
        

        public virtual void InitData()
        {
            //TODO
            functionBlock = FunctionBlockModule.GetFunctionBlockByBlockID(100);
            //TODO
            blockModifier = GetComponent<FunctionBlockModifier>();
            info = FunctionBlockInfoData.CreateBaseInfo(transform.position,functionBlock,blockModifier);
            InitBaseInfo();
        }

        private void InitBaseInfo()
        {
            //Set Collider
            BlockCollider = gameObject.GetComponent<BoxCollider>();
            SetBlockColliderSize(FunctionBlockModule.Instance.InitFunctionBlockBoxCollider(functionBlock));

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


        #endregion 
        public void CheckMouseButtonDown(Action callback)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (CheckUIRaycast())
                    return ;
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if(Physics.Raycast(ray, out hit))
                {
                    callback();
                }
            }
        }
        bool CheckUIRaycast()
        {
            PointerEventData data = new PointerEventData(EventSystem.current);
            data.pressPosition = Input.mousePosition;
            data.position = Input.mousePosition;
            List<RaycastResult> result = new List<RaycastResult>();
            GameManager.Instance.raycaster.Raycast(data, result);
            return result.Count > 0;
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

        public FunctionBlockLevelInfo levelInfo;


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
        public FunctionBlockModifier blockModifier;

        public List<BlockDistrictUnlockData.DistrictUnlockData> districtUnlockDataList;
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
            info.districtUnlockDataList = FunctionBlockModule.GetManuBlockDistrictUnlockData(blockBase.FunctionBlockID);

            info.levelInfo = new FunctionBlockLevelInfo(blockBase);

            //District
            info.districtAreaMax = FunctionBlockModule.GetFunctionBlockAreaMax(blockBase);
            info.currentDistrictDataDic = FunctionBlockModule.GetFuntionBlockOriginAreaInfo(blockBase); ;
            info.currentDistrictBaseDic = FunctionBlockModule.GetFuntionBlockAreaDetailDefaultDataInfo(blockBase);

            //Set active district build
            for (int i = 0; i < info.districtUnlockDataList.Count; i++)
            {
                if (info.districtUnlockDataList[i].UnlockDefault == true)
                {
                    info.ActiveDistrictBuildList.Add(DistrictModule.GetDistrictDataByKey(info.districtUnlockDataList[i].DistrictID));
                }
            }
            //TODO
            return info;
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
            int currentMaxEXP = FunctionBlockModule.GetCurrentLevelEXP(BlockEXPMap, currentBlockLevel);
            if (_currentBlockExp > currentMaxEXP)
            {
                currentBlockLevel++;
                CurrentBlockMaxEXP = FunctionBlockModule.GetCurrentLevelEXP(BlockEXPMap, currentBlockLevel);
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

        public FunctionBlockLevelInfo(FunctionBlock block)
        {
            BlockEXPMap = FunctionBlockModule.GetBlockEXPMapData(block.FunctionBlockID);
            CurrentBlockMaxEXP = FunctionBlockModule.GetCurrentLevelEXP(BlockEXPMap, currentBlockLevel);
        }
       

    }

}