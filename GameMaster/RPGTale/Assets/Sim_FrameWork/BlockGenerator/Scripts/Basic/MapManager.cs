using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork
{
    public class MapManager : MonoSingleton<MapManager>
    {

        private Transform MainShipAreaContainer;
        private Transform AssembleContainer;
        private Transform AssembleContainerContentTrans;

        public int currentSelectBuildID = -1;
        public bool isSelectBlock_Panel=false;

        private FunctionBlockBase currentInitBlock = null;

        public bool _hasAddBlockToMap = false;

        protected override void Awake()
        {
            base.Awake();
            GameManager.Instance.InitBaseData();
            MainShipAreaContainer = transform.FindTransfrom("MainShipAreaContainer");
            AssembleContainer = transform.FindTransfrom("AssembleContainer");
            AssembleContainerContentTrans = AssembleContainer.FindTransfrom("Content");

            UIUtility.SafeSetActive(ContentObj, false);
            UIUtility.SafeSetActive(AssembleContainer, false);
           
        }

        private void Start()
        {
            UIGuide.Instance.ShowGameMainPage();
            UIGuide.Instance.ShowPlayerStatePanel();
            UIGuide.Instance.ShowGameDebugDialog();
            GameManager.Instance.SetGameStates(GameStates.Start);
        }

        void Update()
        {
            HandleBlockPanelSelect();
        }

        private void RegisterCameraAction()
        {
            CameraManager.Instance.OnBlockSelect += OnBlockSelect;
            CameraManager.Instance.OnBlockDragStart += OnBlockDragStart;
            CameraManager.Instance.OnBlockDrag += OnBlockDrag;
            CameraManager.Instance.OnBlockDragEnd += OnBlockDragEnd;
            CameraManager.Instance.OnGroundSelect += OnGroundSelect;
            CameraManager.Instance.OnBlockAreaEnter += OnBlockAreaEnter;
            CameraManager.Instance.OnBlockAreaExit += OnBlockAreaExit;
        }

        private void UnRegisterCameraAction()
        {
            CameraManager.Instance.OnBlockSelect = null;
            CameraManager.Instance.OnBlockDragStart = null;
            CameraManager.Instance.OnBlockDrag = null;
            CameraManager.Instance.OnBlockDragEnd = null;
            CameraManager.Instance.OnGroundSelect = null;
            CameraManager.Instance.OnBlockAreaEnter = null;
            CameraManager.Instance.OnBlockAreaExit = null;
        }

        private void InitMap()
        {
            if (MapGenerator.Inited == false || ChunkManager.Inited == false)
                return;
            ChunkManager.SpawnChunks(transform.position);
        }


        #region Block Action

        public void InitBlockBuildPanelSelect(int buildID ,bool select)
        {
            currentSelectBuildID = buildID;
            isSelectBlock_Panel = select;
        }

        /// <summary>
        /// 选择建筑，面板
        /// </summary>
        void HandleBlockPanelSelect()
        {
            if (isSelectBlock_Panel == true && currentSelectBuildID != -1)
            {
                if (CameraManager.Instance.InBlockPanelPos() == false)
                    return;
                if (_hasAddBlockToMap == false)
                {
                    var pos = CameraManager.Instance.TryGetRaycastHitGround(Input.mousePosition);
                    currentInitBlock = FunctionBlockManager.Instance.AddFunctionBlock(PlayerModule.GetBuildingPanelDataByKey(currentSelectBuildID).FunctionBlockID,(int)pos.x,(int)pos.z);
                    currentInitBlock.currentState = FunctionBlockBase.BlockState.Move;
                    currentInitBlock.SetSelect(true);
                    _hasAddBlockToMap = true;
                    CameraManager.Instance.currentBlockMode = CameraManager.BlockMode.Move;
                }
                else if(_hasAddBlockToMap==true && currentInitBlock != null)
                {
                    CameraManager.Instance.UpdateBlockMove(currentInitBlock);
                }
            }

            /// ESC to  Delete
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (_hasAddBlockToMap == true)
                {
                    FunctionBlockManager.Instance.RemoveItem(currentInitBlock);
                    currentInitBlock = null;
                    _hasAddBlockToMap = false;
                    InitBlockBuildPanelSelect(-1, false);
                    CameraManager.Instance.currentBlockMode = CameraManager.BlockMode.None;
                    CameraManager.Instance.ResetDragState();
                }
            }
        }



        public FunctionBlockBase selectBlock;

        public void OnBlockSelect(CameraManager.CameraEvent camera)
        {
            var block = camera.blockBase;
            if (selectBlock != null)
            {
                selectBlock.SetSelect(false);

            }
            selectBlock = block;
            block.SetSelect(true);
        }

        private FunctionBlockBase _dragBlock;
        /// <summary>
        /// Drag Start
        /// </summary>
        /// <param name="camera"></param>
        public void OnBlockDragStart(CameraManager.CameraEvent camera)
        {
            _dragBlock = camera.blockBase;
            _dragBlock.OnBlockDragStart(camera);

        }

        /// <summary>
        /// Drag 
        /// </summary>
        /// <param name="camera"></param>
        public void OnBlockDrag(CameraManager.CameraEvent camera)
        {
            if (_dragBlock != null)
            {
                _dragBlock.OnBlockDrag(camera);
            }
        }

        /// <summary>
        /// Drag End
        /// </summary>
        /// <param name="camera"></param>
        public void OnBlockDragEnd(CameraManager.CameraEvent camera)
        {
            if (_dragBlock != null)
            {
                _dragBlock.OnBlockDragEnd(camera);
                _dragBlock = null;
            }
        }

        public void OnGroundSelect(CameraManager.CameraEvent camera)
        {
            if (selectBlock != null)
            {
                FunctionBlockBase temp = selectBlock;
                selectBlock = null;
                temp.SetSelect(false);
            }
        }

        private FunctionBlockBase _currentEnterBlock;
        public void OnBlockAreaEnter(CameraManager.CameraEvent camera)
        {
            _currentEnterBlock = camera.blockBase;
            if (_currentEnterBlock != null)
            {
                _currentEnterBlock.SetBlockAreaEnter(true);
            }
        }

        public void OnBlockAreaExit(CameraManager.CameraEvent camera)
        {
            if (_currentEnterBlock != null && camera.blockBase==null)
            {
                FunctionBlockBase temp = _currentEnterBlock;
                _currentEnterBlock = null;
                temp.SetBlockAreaEnter(false);
            }
        }

        #endregion

        #region Assemble

        public void InitAssembleModel(string modelPath)
        {
            UIUtility.SafeSetActive(AssembleContainer, true);
            var Obj= ObjectManager.Instance.InstantiateObject("Assets/"+ modelPath + ".prefab");
            if (Obj != null)
            {
                Obj.transform.SetParent(AssembleContainerContentTrans,false);
            }
        }
        public void ReleaseAssembleModel()
        {
            foreach(Transform trans in AssembleContainerContentTrans)
            {
                ObjectManager.Instance.ReleaseObject(trans.gameObject, 0);
            }
        }

        #endregion

        #region Main Ship
        private const string PowerArea_Map_Container_Path = "Assets/Prefabs/Scene/MainShip/MainShip_PowerArea.prefab";

        private MainShipPowerAreaManager powerAreaManager;
        private GameObject mapObj = null;
        private Transform ContentObj=null;

        public void GeneratePowerAreaContainer()
        {
            mapObj = ObjectManager.Instance.InstantiateObject(PowerArea_Map_Container_Path);
            if (mapObj != null)
            {
                mapObj.transform.SetParent(MainShipAreaContainer, false);
                ContentObj = mapObj.transform.FindTransfrom("Content");
                powerAreaManager = mapObj.transform.SafeGetComponent<MainShipPowerAreaManager>();
                powerAreaManager.LoadPowerArea();
                RegisterCameraAction();
            }
            GameManager.Instance.SwitchAreaState(AreaState.MainShip_PowerArea);
        }

        public void ReleasePowerAreaContainer()
        {
            ObjectManager.Instance.ReleaseObject(mapObj, 0);
            powerAreaManager = null;
            ContentObj = null;
            UnRegisterCameraAction();
        }

        #endregion

    }


}