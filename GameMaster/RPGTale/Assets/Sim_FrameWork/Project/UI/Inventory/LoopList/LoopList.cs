using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace Sim_FrameWork
{
    public class LoopList : MonoBehaviour
    {
        public enum LayoutType
        {
            Vertical,
            Horizontal
        }

        [System.Serializable]
        public class SepConfig
        {
            public float LeftSep;
            public float TopSep;
            public SepConfig(float left,float top)
            {
                LeftSep = left;
                TopSep = top;
            }
        }

        [Header("Config")]
        public float offSet;
        public LayoutType layoutType = LayoutType.Horizontal;
        public SepConfig sepConfig;
        
        /// <summary>
        /// 生成物体路径
        /// </summary>
        public string ItemPrefabPath;

        private float _itemHeight;
        private float _itemWidth;

        private RectTransform _content;
        private GameObject _item;
        private List<BaseElement> _elementList;

        public List<BaseElement> ElementList { get { return _elementList; } }

        /// <summary>
        /// Element Data
        /// </summary>
        private List<List<BaseDataModel>> _modelList;

        private void Awake()
        {
            _elementList = new List<BaseElement>();
            _modelList = new List<List<BaseDataModel>>();
            if (!string.IsNullOrEmpty(ItemPrefabPath))
            {
                _item = ResourceManager.Instance.LoadResource<GameObject>("Assets/Prefabs/" + ItemPrefabPath + ".prefab");
                _itemHeight = _item.transform.SafeGetComponent<RectTransform>().rect.height;
                _itemWidth = _item.transform.SafeGetComponent<RectTransform>().rect.width;
            }  
            _content = transform.FindTransfrom("Viewport/Content").SafeGetComponent<RectTransform>();
            
        }

        public void InitData(List<List<BaseDataModel>> modelData, List<object> paramList = null)
        {
            _elementList.Clear();
            _modelList = modelData;
            int num = GetItemNum(_itemHeight, _itemWidth, offSet);

            _content.InitObj("Assets/Prefabs/" + ItemPrefabPath + ".prefab", modelData.Count, num);
            
            for (int i = 0; i < _content.childCount; i++)
            {
                var elementcpt = _content.GetChild(i).SafeGetComponent<BaseElement>();
                /// Get  Data
                elementcpt.AddGetDataListener(GetData);
                elementcpt.Init(i, offSet, num, sepConfig, layoutType, paramList);
                _elementList.Add(elementcpt);
            }

            SetContentSize();
            transform.SafeGetComponent<ScrollRect>().onValueChanged.AddListener(ValueChanged);
        }

        /// <summary>
        /// Listener
        /// </summary>
        /// <param name="v"></param>
        private void ValueChanged(Vector2 v)
        {
            foreach(var element in _elementList)
            {
                if(element!=null)
                    element.OnValueChange();
            }
        }

        private int GetItemNum(float itemHeight,float itemWidth, float offset)
        {
            var ScrollRect = transform.SafeGetComponent<RectTransform>().rect;
            switch (layoutType)
            {
                case LayoutType.Horizontal:
                    var width = ScrollRect.width;
                    return Mathf.CeilToInt(width / (itemWidth + offset)) + 1;
                case LayoutType.Vertical:
                    var height = ScrollRect.height;
                    return Mathf.CeilToInt(height / (itemHeight + offset)) + 1;
                default:
                    return -1;
            }
           
        }

        private void RefreshItem()
        {
            int i = 0;
            int num = GetItemNum(_itemHeight, _itemWidth, offSet);
            foreach (Transform item in _content.transform)
            {
                var cpt= item.SafeGetComponent<BaseElement>();
                cpt.Init(i, offSet, num,sepConfig, layoutType);
                i++;
            }
        }


        private List<BaseDataModel> GetData(int id)
        {
            if (id < 0 || id >= _modelList.Count)
                return null;

            return _modelList[id];
        }


        private void SetContentSize()
        {
            switch (layoutType)
            {
                case LayoutType.Horizontal:
                    var x = _modelList.Count * _itemWidth + (_modelList.Count - 1) * offSet;
                    _content.sizeDelta = new Vector2(x, _content.sizeDelta.y);
                    break;
                case LayoutType.Vertical:
                    var y = _modelList.Count * _itemHeight + (_modelList.Count - 1) * offSet;
                    _content.sizeDelta = new Vector2(_content.sizeDelta.x,y);
                    break;
                default:
                    break;
            }
        }

    }
}