using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sim_FrameWork
{
    public class WareHouseMainTagCmpt : MonoBehaviour
    {
        public Toggle toggle;
        public Image Icon;
        public Text Name;
        public GameObject SubTagContent;

        private Rect rect;
        private ToggleGroup subToggleGroup;

        private float subTagHeight;
        private List<WareHouseSubTagCmpt> subTagCmptList;

        void Awake()
        {
            subTagCmptList = new List<WareHouseSubTagCmpt>();
            rect = UIUtility.SafeGetComponent<RectTransform>(transform).rect;
            subToggleGroup = UIUtility.SafeGetComponent<ToggleGroup>(SubTagContent.transform);
        }
        void Update()
        {
            if (toggle.isOn)
            {
                SubTagContent.gameObject.SetActive(true);
            }
            else
            {
                SubTagContent.gameObject.SetActive(false);
            }
        }

        public void InitTagElement(MaterialType type)
        {
            Icon.sprite = MaterialModule.GetMaterialTypeSprite(type);
            Name.text = MaterialModule.GetMaterialTypeName(type);
            //Init SubType
            InitSubTag(type);
        }

        void InitSubTag(MaterialType type)
        {
            var list = MaterialModule.GetMaterialSubTypeList(type);

            for (int i = 0; i < list.Count; i++)
            {
                var obj = ObjectManager.Instance.InstantiateObject(UIPath.WareHouse_Subtag_Prefab_Path);
                var cmpt = UIUtility.SafeGetComponent<WareHouseSubTagCmpt>(obj.transform);
                subTagHeight = UIUtility.SafeGetComponent<RectTransform>(obj.transform).rect.height;
                cmpt.toggle.group = subToggleGroup;
                //Init Info
                cmpt.InitSubTag(list[i]);
                obj.transform.SetParent(SubTagContent.transform, false);
                subTagCmptList.Add(cmpt);
            }
        }


    }
}