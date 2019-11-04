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
                _itemHeight = UIUtility.SafeGetComponent<RectTransform>(_item.transform).rect.height;
                _itemWidth = UIUtility.SafeGetComponent<RectTransform>(_item.transform).rect.width;
            }  
            _content = UIUtility.SafeGetComponent<RectTransform>(transform.Find("Viewport/Content"));
            
        }

        public void InitData(List<List<BaseDataModel>> modelData)
        {
            _modelList = modelData;
            int num = GetItemNum(_itemHeight, _itemWidth, offSet);
            if (_content.childCount == num)
                return;
            if (_modelList.Count < num )
            {
                SpawnItem(_modelList.Count, ItemPrefabPath);
            }
            else
            {
                SpawnItem(num, ItemPrefabPath);
            }
            SetContentSize();
            UIUtility.SafeGetComponent<ScrollRect>(transform).onValueChanged.AddListener(ValueChanged);
        }

        public void RefrshData(List<List<BaseDataModel>> modelData)
        {
            Action<int,int,int> SpawnItem = (count, startID, total) =>
            {
                for(int i = 0; i < count; i++)
                {
                    var element = ObjectManager.Instance.InstantiateObject("Assets/Prefabs/" + ItemPrefabPath + ".prefab");
                    element.transform.SetParent(_content, false);
                    var elementcpt = UIUtility.SafeGetComponent<BaseElement>(element.transform);
                    /// Get  Data
                    elementcpt.AddGetDataListener(GetData);
                    elementcpt.Init(i+startID, offSet, total,sepConfig, layoutType);
                    _elementList.Add(elementcpt);
                }
            };
            _modelList = modelData;
            int num = GetItemNum(_itemHeight, _itemWidth, offSet);
            Debug.Log(num);
            if (_modelList.Count<num && _content.childCount<_modelList.Count)
            {
                //小于实际数量,生成多的
                SpawnItem(_modelList.Count - _content.childCount, _content.childCount - 1, num);
                RefreshItem();
            }
            else if(_modelList.Count<num && _content.childCount > _modelList.Count)
            {
                //大于实际数量,销毁
                for(int i = 0; i < _content.childCount - _modelList.Count;i++)
                {
                    var obj = _content.GetChild(_content.childCount - i - 1);
                    var elementcpt = UIUtility.SafeGetComponent<BaseElement>(obj.transform);
                    _elementList.Remove(elementcpt);
                    ObjectManager.Instance.ReleaseObject(_content.GetChild(_content.childCount - i - 1).gameObject);
                }
                RefreshItem();
            }
        }

        /// <summary>
        /// Listener
        /// </summary>
        /// <param name="v"></param>
        private void ValueChanged(Vector2 v)
        {
            foreach(var element in _elementList)
            {
                element.OnValueChange();
            }
        }

        private int GetItemNum(float itemHeight,float itemWidth, float offset)
        {
            var ScrollRect = UIUtility.SafeGetComponent<RectTransform>(transform).rect;
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

        private void SpawnItem(int num,string path)
        {
            GameObject element = null;
            BaseElement elementcpt = null;
            for(int i = 0; i < num; i++)
            {
                element= ObjectManager.Instance.InstantiateObject("Assets/Prefabs/" + path+".prefab");
                element.transform.SetParent(_content, false);
                elementcpt= UIUtility.SafeGetComponent<BaseElement>(element.transform);
                /// Get  Data
                elementcpt.AddGetDataListener(GetData);
                elementcpt.Init(i, offSet, num , sepConfig, layoutType);
                _elementList.Add(elementcpt);
            }
        }
        private void RefreshItem()
        {
            int i = 0;
            int num = GetItemNum(_itemHeight, _itemWidth, offSet);
            foreach (Transform item in _content.transform)
            {
                var cpt= UIUtility.SafeGetComponent<BaseElement>(item);
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