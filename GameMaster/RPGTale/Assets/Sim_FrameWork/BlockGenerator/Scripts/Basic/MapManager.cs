using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork
{
    public class MapManager : MonoSingleton<MapManager>
    {
        private Transform ContentObj;

        public int currentSelectBuildID = -1;
        public bool isSelectBlock_Panel=false;

        private FunctionBlockBase currentInitBlock = null;

        public bool _hasAddBlockToMap = false;

        protected override void Awake()
        {
            base.Awake();
            ContentObj = UIUtility.FindTransfrom(transform, "Content");
            UIUtility.SafeSetActive(ContentObj, false);

        }

        private void Start()
        {
            CameraManager.Instance.OnBlockSelect += OnBlockSelect;
            CameraManager.Instance.OnBlockDragStart += OnBlockDragStart;
            CameraManager.Instance.OnBlockDrag += OnBlockDrag;
            CameraManager.Instance.OnBlockDragEnd += OnBlockDragEnd;
            CameraManager.Instance.OnGroundSelect += OnGroundSelect;

            GridManager.Instance.UpdateAllNodes();
            //InvokeRepeating("InitMap", 1, 0.5f);
        }

        void Update()
        {
            HandleBlockPanelSelect();
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

        #endregion

    }


}