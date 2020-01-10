using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sim_FrameWork
{
    public class GridLoopList : MonoBehaviour
    {
        public enum LoopType
        {
            Vertical,
            Horizontal
        }

        [System.Serializable]
        public class SepConfig
        {
            public float LeftSep;
            public float TopSep;
            public float HorizontalSep;
            public float VerticalSep;

            public SepConfig(float left, float top ,float horizontalSep ,float verticalSep)
            {
                LeftSep = left;
                TopSep = top;
                HorizontalSep = horizontalSep;
                VerticalSep = verticalSep;
            }
        }

        public LoopType loopType = LoopType.Vertical;
        public SepConfig sepConfig;

        public string ItemPrefabPath;

        private float _itemHeight;
        private float _itemWidth;

        private RectTransform _content;
        private GameObject _item;
        private List<BaseElement> _elementList;

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
            int totalNum = GetTotalItemNum();
            if (_content.childCount == totalNum)
                return;

            UIUtility.SafeGetComponent<ScrollRect>(transform).onValueChanged.RemoveAllListeners();
            foreach (Transform trans in _content)
            {
                ObjectManager.Instance.ReleaseObject(trans.gameObject, 0);
            }

            if (_modelList.Count < totalNum)
            {
                SpawnItem(_modelList.Count, ItemPrefabPath);
            }
            else
            {
                SpawnItem(totalNum, ItemPrefabPath);
            }
            SetContentSize();
            UIUtility.SafeGetComponent<ScrollRect>(transform).onValueChanged.AddListener(ValueChanged);
        }



        private void SpawnItem(int num,string path)
        {
            GameObject element = null;
            BaseElement elementcpt = null;
            for(int i = 0; i < num; i++)
            {
                element = ObjectManager.Instance.InstantiateObject("Assets/Prefabs/" + path + ".prefab");
                element.transform.SetParent(_content, false);
                elementcpt = UIUtility.SafeGetComponent<BaseElement>(element.transform);
                /// Get  Data
                elementcpt.AddGetDataListener(GetData);
                elementcpt.InitGrid(i, num,GetHorizontalItemNum(),GetVerticalItemNum() ,sepConfig, loopType);
                _elementList.Add(elementcpt);
            }
        }


        private void ValueChanged(Vector2 v)
        {
            foreach(var element in _elementList)
            {
                element.OnValueChange();
            }
        }

        private int GetTotalItemNum()
        {
            var scrollRect = UIUtility.SafeGetComponent<RectTransform>(transform).rect;

            var width = scrollRect.width;
            int horizontalCount = Mathf.CeilToInt(width / (_itemWidth + sepConfig.HorizontalSep)) - 1;

            var height = scrollRect.height;
            int verticalCount = Mathf.CeilToInt(height / (_itemHeight + sepConfig.VerticalSep)) + 1;
            return horizontalCount * verticalCount;
        }

        private int GetHorizontalItemNum()
        {
            var scrollRect = UIUtility.SafeGetComponent<RectTransform>(transform).rect;
            var width = scrollRect.width;
            return Mathf.CeilToInt(width / (_itemWidth + sepConfig.HorizontalSep)) - 1;
        }

        private int GetVerticalItemNum()
        {
            var scrollRect = UIUtility.SafeGetComponent<RectTransform>(transform).rect;
            var height = scrollRect.height;
            return Mathf.CeilToInt(height / (_itemHeight + sepConfig.VerticalSep)) + 1;
        }

        private List<BaseDataModel> GetData(int id)
        {
            if (id < 0 || id >= _modelList.Count)
                return null;

            return _modelList[id];
        }

        private void SetContentSize()
        {
            switch (loopType)
            {
                case LoopType.Vertical:
                    var count = Mathf.CeilToInt((float) _modelList.Count /(float) GetHorizontalItemNum());
                    var y = count * _itemHeight;
                    _content.sizeDelta = new Vector2(_content.sizeDelta.x, y);
                    break;
                default:
                    break;
            }
        }


    }
}