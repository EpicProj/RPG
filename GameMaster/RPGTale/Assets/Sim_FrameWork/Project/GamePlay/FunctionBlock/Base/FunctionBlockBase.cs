using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

namespace Sim_FrameWork
{
    public class FunctionBlockBase : MonoBehaviour
    {
        public enum BlockState
        {
            Idle,
            Building,
            Build_Wait,
            Move
        }
        public FunctionBlock functionBlock;

        [HideInInspector]
        public FunctionBlockInfoData info;

        private BoxCollider BlockCollider;
        private GameObject ModelRoot;
        /// <summary>
        /// UI ROOT
        /// </summary>
        public GameObject UIRoot;
        private BlockUIScriptInfo UIinfo;

        public BlockState currentState = BlockState.Idle;

        public int instanceID;
        public Action OnBlockSelectAction;
        public Action<bool> OnBlockAreaEnterAction;

        public Vector3 CenterPositionScreen
        {
            get
            {
                return CameraManager.Instance.WorldToViewportPoint(transform.position);
            }
        }
      

        public void InitData(int blockID,int posX,int posZ)
        {
            functionBlock = FunctionBlockModule.GetFunctionBlockByBlockID(blockID);

            UIinfo = UIUtility.SafeGetComponent<BlockUIScriptInfo>(transform);
            UIinfo.SetData(this);
            ModelRoot = UIUtility.FindTransfrom(transform, "Root/ModelRoot").gameObject;
            gameObject.name = instanceID + "[Block]";

            SetPosition(new Vector3( posX, transform.localScale.y/2 , posZ));

            info = FunctionBlockInfoData.CreateBaseInfo(transform.position,functionBlock,
                new FunctionBlockModifier(ModifierTarget.FunctionBlock,instanceID));

            var blockType = info.dataModel.BlockType;
            if(blockType== FunctionBlockType.ElementCapsule)
            {
                var manuBase = transform.SafeAddCmpt<ManufactoryBase>();
                manuBase.SetData();
            }else if(blockType == FunctionBlockType.EnergyStorageUnit)
            {
                var normalBase = transform.SafeAddCmpt<BlockNormalBase>();
                normalBase.SetData();
            }

            InitBase();
        }

        private void InitBase()
        {
            //Set Collider
            BlockCollider = UIUtility.SafeGetComponent<BoxCollider>(transform);
            SetBlockColliderSize(FunctionBlockModule.Instance.InitFunctionBlockBoxCollider(functionBlock,3.0f));
            InitDistrictModel();
        }


        #region BaseInfo

        private void SetBlockColliderSize(Vector3 size)
        {
            BlockCollider.size = size;
        }

        public Vector3 GetPosition()
        {
            return transform.localPosition;
        }

        /// <summary>
        /// Set Postion
        /// </summary>
        /// <param name="pos"></param>
        public void SetPosition(Vector3 pos)
        {
            transform.localPosition = pos;
        }
        public int GetPosX()
        {
            return (int)GetPosition().x;
        }

        public int GetPosZ()
        {
            return (int)GetPosition().z;
        }


        /// <summary>
        /// Set Select State
        /// </summary>
        /// <param name="select"></param>
        public void SetSelect(bool select)
        {
            UIinfo.ShowSelectionUI(select);

            if (!select)
            {
                if (!InPlacablePosition())
                {
                    SetPosition(_oldPosition);
                    OnBlockDragEnd(null);
                }
            }
            if (select)
            {
                if (currentState != BlockState.Move)
                {
                    OnBlockSelectAction?.Invoke();
                }
            }
        }

        public void SetBlockAreaEnter(bool enter)
        {
            if (enter && currentState != BlockState.Move)
            {
                OnBlockAreaEnterAction?.Invoke(true);
            }

            if (!enter)
            {
                OnBlockAreaEnterAction?.Invoke(false);
            }
        }

        #endregion

        #region Model
        private void InitDistrictModel()
        {
            var vec2 = FunctionBlockModule.GetFunctionBlockAreaMax(info.block);
            ModelRoot.transform.localPosition = new Vector3(-vec2.x / 2 + 0.5f, 0, -vec2.y / 2 + 0.5f);
            foreach(KeyValuePair<Vector2,DistrictAreaInfo> kvp in info.currentDistrictDataDic)
            {
                if (!string.IsNullOrEmpty(kvp.Value.prefabModelPath))
                {
                    try
                    {
                        var obj = ObjectManager.Instance.InstantiateObject("Assets/" + kvp.Value.prefabModelPath + ".prefab");
                        obj.transform.SetParent(ModelRoot.transform, false);
                        Vector3 pos = new Vector3(kvp.Value.OriginCoordinate.x, 0, kvp.Value.OriginCoordinate.y);
                        obj.transform.localPosition = pos;
                    }catch(Exception e)
                    {
                        Debug.LogError(e);
                        continue;
                    }
                }
            }
        }

        #endregion


        #region Action

        private Vector3 _oldPosition;
        private Vector3 _deltaDistance;

        private bool InPlacablePosition()
        {
            bool canPlace = GridManager.Instance.PositionCanPlace(GetPosition(), (int)info.districtAreaMax.x, (int)info.districtAreaMax.y, instanceID);
            return canPlace;
        }

        /// <summary>
        /// Move Block
        /// </summary>
        public void OnBlockDragStart(CameraManager.CameraEvent camera)
        {
            _deltaDistance = GetPosition() - camera.point;
            GridManager.Instance.UpdateFunctionBlockNodes(this, GridManager.Action.REMOVE);

            bool canPlace = InPlacablePosition();
            if (canPlace)
            {
                if (UIinfo.selectionUIInstance != null)
                {
                    UIinfo.selectionUIInstance.ShowGrid(true);
                }
                _oldPosition = GetPosition();
            }
        }

        public void OnBlockDrag(CameraManager.CameraEvent camera)
        {
            var point = camera.point + _deltaDistance;
            
            point.x = Mathf.Floor(point.x);
            point.z = Mathf.Floor(point.z);

            if(point!= transform.localPosition)
            {
                SetPosition(new Vector3(Mathf.Floor(point.x), transform.localScale.y/2 , Mathf.Floor(point.z)));
                bool canPlace = InPlacablePosition();
                if (canPlace)
                {
                    UIinfo.selectionUIInstance.SetGridColor(SelectionUIObject.PlaceState.CanPlace);
                }
                else
                {
                    UIinfo.selectionUIInstance.SetGridColor(SelectionUIObject.PlaceState.CanNotPlace);
                }

                AudioManager.Instance.PlaySound(AudioClipPath.ItemEffect.Block_Move);
            }
        }

        public void OnBlockDragEnd(CameraManager.CameraEvent camera)
        {
            bool canPlace = InPlacablePosition();
            if (canPlace)
            {
                if (UIinfo.selectionUIInstance != null)
                {
                    UIinfo.selectionUIInstance.ShowGrid(false);
                }

                GridManager.Instance.UpdateFunctionBlockNodes(this, GridManager.Action.ADD);

                AudioManager.Instance.PlaySound(AudioClipPath.ItemEffect.Block_Place);
            }
        }


        #endregion

    }


    public class FunctionBlockInfoData
    {
        public int BlockID;
        public Vector3 BlockPos;

        /// <summary>
        /// 区划历史记录
        /// </summary>
        public FunctionBlockHistory blockHistory;

        public FunctionBlockLevelInfo levelInfo;


        public FunctionBlock block;
        public FunctionBlockDataModel dataModel;

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

        public List<Config.BlockDistrictUnlockData.DistrictUnlockData> districtUnlockDataList;
        public List<DistrictData> ActiveDistrictBuildList=new List<DistrictData> ();


        public static FunctionBlockInfoData CreateBaseInfo(Vector3 blockPos, FunctionBlock blockBase , FunctionBlockModifier modifier)
        {

            FunctionBlockInfoData info = new FunctionBlockInfoData();
            info.BlockID = blockBase.FunctionBlockID;
            info.block = blockBase;
            info.dataModel = new FunctionBlockDataModel();
            info.dataModel.Create(info.BlockID);
            info.BlockPos = blockPos;

            info.blockModifier = modifier;
            info.districtUnlockDataList = FunctionBlockModule.GetBlockDistrictUnlockData(blockBase.FunctionBlockID);

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