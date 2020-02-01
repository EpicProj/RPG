using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Sim_FrameWork
{
    public class CameraManager : MonoSingleton<CameraManager>
    {

        public enum RaycastTarget
        {
            FunctionBlock,
            Ground
        }

        public enum BlockMode
        {
            None,
            Move,
            Destory
        }
        public class CameraEvent
        {
            public Vector3 point;
            public FunctionBlockBase blockBase;

        }

        /* Camera Action  */
        /* Select Block  */
        public UnityAction<CameraEvent> OnBlockSelect;
        /* Drag Block Start  */
        public UnityAction<CameraEvent> OnBlockDragStart;
        /* Drag Event  */
        public UnityAction<CameraEvent> OnBlockDrag;
        /* Drag Block End  */
        public UnityAction<CameraEvent> OnBlockDragEnd;
        /* Select Ground  */
        public UnityAction<CameraEvent> OnGroundSelect;
        /* Block Area Enter . Show Info Tip  */
        public UnityAction<CameraEvent> OnBlockAreaEnter;
        public UnityAction<CameraEvent> OnBlockAreaExit;


        private Camera MainCamera;
        public EventSystem eventSystems;

        private int _MaskBlockCollider;
        private int _MaskGroundCollider;

        private bool _isSelectFunctionBlock;
        private bool _isDraggingFunctionBlock;
        private bool _isShowBlockTip=false;
        
        /// <summary>
        /// 最小移动距离，开始移动
        /// </summary>
        private float _minMoveDistance = 0.2f;


        private FunctionBlockBase _selectFunctionBlock;
        public BlockMode currentBlockMode = BlockMode.None;


        protected override void Awake()
        {
            base.Awake();
            eventSystems = Utility.SafeFindGameobject("EventSystem").transform.SafeGetComponent<EventSystem>();
            MainCamera = transform.SafeGetComponent<Camera>();
            _MaskBlockCollider = LayerMask.GetMask("FunctionBlockCollider");
            _MaskGroundCollider = LayerMask.GetMask("GroundCollider");
        }

        void Update()
        {
            if (UsingUI())
                return;
            UpdateBlockSelect();
            UpdateGroundSelect();
            UpdateBlockAreaEnter();
        }

        public Vector3 WorldToViewportPoint(Vector3 pos)
        {
            return MainCamera.WorldToViewportPoint(pos);
        }


        public bool UsingUI()
        {
            if (_isSelectFunctionBlock)
                return false;

            return (eventSystems.IsPointerOverGameObject() || eventSystems.IsPointerOverGameObject(0));
        }


        public void UpdateBlockAreaEnter()
        {
            if (UsingUI())
                return;
            if (_isDraggingFunctionBlock || _isSelectFunctionBlock)
                return;
            FunctionBlockBase selectBlock = TryGetRaycastHitBlock(Input.mousePosition);
            if (selectBlock != null && _isShowBlockTip ==false)
            {
                _isShowBlockTip = true;
                CameraEvent camera = new CameraEvent()
                {
                    blockBase = selectBlock,
                };
                if (OnBlockAreaEnter != null)
                {
                    OnBlockAreaEnter.Invoke(camera);
                }
            }
            else if(selectBlock==null)
            {
                CameraEvent camera = new CameraEvent()
                {
                    blockBase = null
                };
                _isShowBlockTip = false;
                OnBlockAreaExit.Invoke(camera);
            }
        }


        public void UpdateBlockSelect()
        {
            if (!Input.GetMouseButtonDown(0))
                return;

            if (_isDraggingFunctionBlock)
                return;

            if (UsingUI())
                return;

            FunctionBlockBase selectBlock = TryGetRaycastHitBlock(Input.mousePosition);
            if (selectBlock != null)
            {
                _isSelectFunctionBlock = true;
                _selectFunctionBlock = selectBlock;

                CameraEvent camera = new CameraEvent()
                {
                    blockBase = selectBlock
                };
                if (OnBlockSelect != null)
                {
                    OnBlockSelect.Invoke(camera);
                }
            }
            else
            {
                _isSelectFunctionBlock = false;
                _selectFunctionBlock = null;
            }
        }

        private FunctionBlockBase _selectStartRaycastBlock = null;
        private bool _isDragStart;

        /// <summary>
        /// Start Pos
        /// </summary>
        private Vector3 _selectBlockStartPos;


        public void UpdateBlockMove(FunctionBlockBase selectBlock)
        {
            if (currentBlockMode != BlockMode.Move)
                return;

            if (selectBlock != null)
            {
                _isSelectFunctionBlock = true;
                selectBlock.currentState = FunctionBlockBase.BlockState.Move;
            }
            else
            {
                return;
            }

            if (_selectBlockStartPos != Config.GlobalConfigData.InfinityVector)
            {
                if (_isSelectFunctionBlock  /*selectBlock == _selectStartRaycastBlock*/)
                {
                    Vector3 currentSelectPos = TryGetRaycastHitGround(Input.mousePosition);
                    if (Vector3.Distance(_selectBlockStartPos, currentSelectPos) >= _minMoveDistance)
                    {
                        ///Start Move
                        CameraEvent camera = new CameraEvent()
                        {
                            point = currentSelectPos,
                            blockBase = selectBlock
                        };

                        if (!_isDragStart)
                        {
                            _isDragStart = true;
                            if (OnBlockDragStart != null)
                            {
                                OnBlockDragStart.Invoke(camera);
                            }
                        }
                        _isDraggingFunctionBlock = true;

                        if (OnBlockDrag != null)
                        {
                            OnBlockDrag.Invoke(camera);
                        }
                    }
                }
            }

            /// 按下 放置方块
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("Place!");
                _selectBlockStartPos = Config.GlobalConfigData.InfinityVector;
                _isSelectFunctionBlock = false;
                if (_isDragStart)
                {
                    _isDragStart = false;
                    _isDraggingFunctionBlock = false;
                    if (OnBlockDragEnd != null)
                    {
                        OnBlockDragEnd.Invoke(null);
                    }
                    currentBlockMode = BlockMode.None;
                    MapManager.Instance.InitBlockBuildPanelSelect(-1, false);
                    MapManager.Instance._hasAddBlockToMap = false;
                    selectBlock.currentState = FunctionBlockBase.BlockState.Idle;
                }
            }


            //if (Input.GetMouseButtonUp(0))
            //{
            //    _selectBlockStartPos = TryGetRaycastHitGround(Input.mousePosition);
            //    _selectStartRaycastBlock = TryGetRaycastHitBlock(Input.mousePosition);
            //    _isDraggingFunctionBlock = false;
            //    _isDragStart = false;
            //}


        }

        public void ResetDragState()
        {
            if (_isDragStart)
            {
                _isDragStart = false;
                _isDraggingFunctionBlock = false;
            }
        }


        public void UpdateGroundSelect()
        {
            if (_isSelectFunctionBlock)
                return;
            if (_isDraggingFunctionBlock)
                return;

            if (!Input.GetMouseButtonUp(0))
                return;

            Vector3 selectPos = TryGetRaycastHitGround(Input.mousePosition);
            if (selectPos != Config.GlobalConfigData.InfinityVector)
            {
                CameraEvent camera = new CameraEvent()
                {
                    point = selectPos
                };
                if (OnGroundSelect != null)
                {
                    OnGroundSelect.Invoke(camera);
                }
            }
        }

        public bool InBlockPanelPos()
        {
            Vector3 currentPos = TryGetRaycastHitGround(Input.mousePosition);
            if (currentPos != Config.GlobalConfigData.InfinityVector && IsPointerOverUI()==false)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 鼠标是否有UI覆盖
        /// </summary>
        /// <returns></returns>
        public bool IsPointerOverUI()
        {
            PointerEventData data = new PointerEventData(EventSystem.current);
            data.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            List<RaycastResult> results = new List<RaycastResult>();
            if (EventSystem.current)
            {
                EventSystem.current.RaycastAll(data, results);
            }
            for(int i = 0; i < results.Count; i++)
            {
                if (results[i].gameObject.layer == LayerMask.NameToLayer("UI"))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Try Get Selecet Block
        /// </summary>
        /// <param name="touch"></param>
        /// <returns></returns>
        private FunctionBlockBase TryGetRaycastHitBlock(Vector2 touch)
        {
            RaycastHit hit;
            if(TryGetRaycastHit(touch,out hit, RaycastTarget.FunctionBlock))
            {
                return UIUtility.SafeGetComponent<FunctionBlockBase>(hit.collider.transform);
            }
            return null;
        }

        public Vector3 TryGetRaycastHitGround(Vector2 touch)
        {
            RaycastHit hit;
            if(TryGetRaycastHit(touch,out hit, RaycastTarget.Ground))
            {
                return hit.point;
            }
            return Config.GlobalConfigData.InfinityVector;
        }

        private bool TryGetRaycastHit(Vector2 touch,out RaycastHit hit,RaycastTarget target)
        {
            Ray ray = MainCamera.ScreenPointToRay(touch);
            return (Physics.Raycast(ray, out hit, 1000, (target == RaycastTarget.FunctionBlock) ? _MaskBlockCollider : _MaskGroundCollider));
        }


    }
}