using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sim_FrameWork
{
    public class LoopList : MonoBehaviour
    {
        public enum LayoutType
        {
            Vertical,
            Horizontal
        }

        [Header("Config")]
        public float offSet;
        public LayoutType layoutType = LayoutType.Horizontal;
        
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
        private List<BaseElementModel> _modelList;

        private void Awake()
        {
            _elementList = new List<BaseElement>();
            _modelList = new List<BaseElementModel>();
            _item = ResourceManager.Instance.LoadResource<GameObject>("Assets/Prefabs/"+ItemPrefabPath+".prefab");
            _content = UIUtility.SafeGetComponent<RectTransform>(transform.Find("Viewport/Content"));
            _itemHeight = UIUtility.SafeGetComponent<RectTransform>(_item.transform).rect.height;
            _itemWidth = UIUtility.SafeGetComponent<RectTransform>(_item.transform).rect.width;
           
        }

        public void InitData(List<BaseElementModel> list)
        {
            _modelList = list;
            int num = GetItemNum(_itemHeight, _itemWidth, offSet);
            if (_modelList.Count < num)
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
            var rect = UIUtility.SafeGetComponent<RectTransform>(transform).rect;
            switch (layoutType)
            {
                case LayoutType.Horizontal:
                    var width = rect.width;
                    return Mathf.CeilToInt(width / (itemWidth + offset)) + 1;
                case LayoutType.Vertical:
                    var height = rect.height;
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
                elementcpt.Init(i, offSet, num ,layoutType);
                _elementList.Add(elementcpt);
            }
        }

        private BaseElementModel GetData(int id)
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