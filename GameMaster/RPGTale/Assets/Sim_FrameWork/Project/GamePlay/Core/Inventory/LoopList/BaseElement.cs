using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Sim_FrameWork
{
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
        /// <summary>
        /// Data
        /// </summary>
        private List<BaseDataModel> _model;
        private LoopList.LayoutType _layoutType;

        public RectTransform Rect
        {
            get
            {
                if (_rect == null)
                    _rect = UIUtility.SafeGetComponent<RectTransform>(transform);
                return _rect;
            }
        }

        public void Init(int id, float offset , int showNum , LoopList.LayoutType type)
        {
            _id = -1;
            _content = UIUtility.SafeGetComponent<RectTransform>(transform.parent);
            _offset = offset;
            _showNum = showNum;
            _layoutType = type;
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
                    startID = Mathf.FloorToInt(- _content.anchoredPosition.x / (Rect.rect.width + _offset));
                    endID = startID + _showNum - 1;
                    break;
                case LoopList.LayoutType.Vertical:
                    startID = Mathf.FloorToInt( _content.anchoredPosition.y / (Rect.rect.height + _offset));
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
            switch (_layoutType)
            {
                case LoopList.LayoutType.Horizontal:
                    Rect.anchoredPosition = new Vector2( _id * (Rect.rect.width + _offset), 0);
                    break;
                case LoopList.LayoutType.Vertical:
                    Rect.anchoredPosition = new Vector2(0,  _id * (Rect.rect.height + _offset));
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// ID是否合法
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private bool JudgeIDValid(int id)
        {
            return !(_getData(id)==null);
        }

        public virtual void OnPointerExit(PointerEventData eventData){}
        public virtual void OnPointerEnter(PointerEventData eventData){}
        public virtual void OnPointerDown(PointerEventData eventData){}
    }
}