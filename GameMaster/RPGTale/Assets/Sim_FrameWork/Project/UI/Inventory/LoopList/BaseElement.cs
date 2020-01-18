using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Sim_FrameWork
{
    public enum BaseElementMode
    {
        Normal,
        Grid
    }
    public class BaseElement : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
    {
        /// <summary>
        /// 数据ID索引
        /// </summary>
        private int _id;

        private RectTransform _content;

        private RectTransform _rect;
        private float _offset;
        private int _showNum;

        private LoopList.SepConfig _sepConfig;
        private GridLoopList.SepConfig _gridSepConfig;

        /// <summary>
        /// Data
        /// </summary>
        private List<BaseDataModel> _model;
        private LoopList.LayoutType _layoutType;

        /// <summary>
        /// Grid
        /// </summary>
        private GridLoopList.LoopType _gridLoopType;
        private int _horizontalItemNum;
        private int _verticalItemNum;

        private BaseElementMode mode = BaseElementMode.Normal;
        /// <summary>
        /// UI
        /// </summary>
        protected List<Button> m_AllBtns=new List<Button> ();

        public List<object> paramList = new List<object>();

        public RectTransform Rect
        {
            get
            {
                if (_rect == null)
                    _rect = transform.SafeGetComponent<RectTransform>();
                return _rect;
            }
        }

        public virtual void Awake() { }

        //General Method
        public void Init(int id, float offset , int showNum , LoopList.SepConfig config, LoopList.LayoutType type,List<object> paramList=null)
        {
            _id = -1;
            _content = transform.parent.SafeGetComponent<RectTransform>();
            mode = BaseElementMode.Normal;
            _offset = offset;
            _showNum = showNum;
            _sepConfig = config;
            _layoutType = type;
            this.paramList = paramList;
            ChangeID(id);
        }

        //For GridLayout
        public void InitGrid(int id, int showNum, int horizontalItemNum, int verticalItemNum, GridLoopList.SepConfig config,GridLoopList.LoopType type)
        {
            _id = -1;
            _content = transform.parent.SafeGetComponent<RectTransform>();
            mode = BaseElementMode.Grid;
            _showNum = showNum;
            _horizontalItemNum = horizontalItemNum;
            _verticalItemNum = verticalItemNum;
            _gridSepConfig = config;
            _gridLoopType = type;
            ChangeID(id);
        }


        private Func<int, List<BaseDataModel>> _getData;
        public void AddGetDataListener(Func<int, List<BaseDataModel>> getData)
        {
            ///这里不能用多播
            _getData = getData;
        }


        public virtual void OnValueChange()
        {
            int startID, endID = 0;
            UpdateIDRange(out startID,out endID);
            JudgeSelfID(startID,endID);
        }

        /// <summary>
        /// 刷新ID范围
        /// </summary>
        private void UpdateIDRange(out int startID,out int endID)
        {
            switch (_layoutType)
            {
                case LoopList.LayoutType.Horizontal:
                    startID = Mathf.FloorToInt(-_content.anchoredPosition.x / (Rect.rect.width + _offset));
                    endID = startID + _showNum - 1;
                    break;
                case LoopList.LayoutType.Vertical:
                    startID = Mathf.FloorToInt(_content.anchoredPosition.y / (Rect.rect.height + _offset));
                    endID = startID + _showNum - 1;
                    break;
                default:
                    startID = -1;
                    endID = -1;
                    break;
            }

        }

        /// <summary>
        /// 判断自身ID是否在范围内
        /// </summary>
        private void JudgeSelfID(int _startID,int _endID)
        {
            int offset = 0;
            if (_id < _startID)
            {
                offset = _startID - _id - 1;
                ChangeID(_endID - offset);
            }
            else if (_id > _endID)
            {
                offset = _id - _endID - 1;
                ChangeID(_startID + offset);
            }
        }

        private void ChangeID(int ID)
        {
            if (_id != ID && JudgeIDValid(ID))
            {
                _id = ID;
                _model = _getData(ID);
                gameObject.transform.name = ID.ToString();
                ChangeAction(_model);
                SetPos();
            }
        }

        public virtual void ChangeAction(List<BaseDataModel> model)
        {
            
        }


        private void SetPos()
        {
            if(mode== BaseElementMode.Normal)
            {
                switch (_layoutType)
                {
                    case LoopList.LayoutType.Horizontal:
                        Rect.anchoredPosition = new Vector2(_id * (Rect.rect.width + _offset) + _sepConfig.LeftSep, 0 + _sepConfig.TopSep);
                        break;
                    case LoopList.LayoutType.Vertical:
                        Rect.anchoredPosition = new Vector2(0 + _sepConfig.LeftSep, _id * (Rect.rect.height + _offset) + _sepConfig.TopSep);
                        break;
                    default:
                        break;
                }
            }
            else if(mode== BaseElementMode.Grid)
            {
                switch (_gridLoopType)
                {
                    case GridLoopList.LoopType.Vertical:
                        //TODO 
                        var x = _id * (Rect.rect.width + _gridSepConfig.HorizontalSep);
                        var y = Mathf.CeilToInt(_id / _horizontalItemNum) * Rect.rect.height;
                        Rect.anchoredPosition = new Vector2(x, y);
                        break;
                    default:
                        break;
                }
            }
         
        }

        /// <summary>
        /// ID是否合法
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private bool JudgeIDValid(int id)
        {
            return !(_getData(id) == null);
        }

        #region UI


        public void RemoveAllButtonListener()
        {
            foreach (Button btn in m_AllBtns)
            {
                btn.onClick.RemoveAllListeners();
            }
        }


        public void AddButtonClickListener(Button btn, UnityEngine.Events.UnityAction action)
        {
            if (btn != null)
            {
                if (!m_AllBtns.Contains(btn))
                {
                    m_AllBtns.Add(btn);
                }
                btn.onClick.RemoveAllListeners();
                btn.onClick.AddListener(action);
                btn.onClick.AddListener(PlayBtnSound);
            }
        }
        void PlayBtnSound()
        {

        }

        #endregion

        public virtual void OnPointerExit(PointerEventData eventData){}
        public virtual void OnPointerEnter(PointerEventData eventData){}
        public virtual void OnPointerDown(PointerEventData eventData){}


    }
}