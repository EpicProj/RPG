using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;

namespace Sim_FrameWork
{
    public class AssemblePartCustomItem : BaseElementSimple
    {
        private Transform Line;
        private Text Name;
        private Slider slider;
        private Text Value;

        private Transform detailContentTrans;
        private Text detailDescText;
        private Transform contentTrans;

        public Config.PartsCustomConfig.ConfigData _config;

        private const string AssemblePartPropertyItem_Value_Max_Text = "AssemblePartPropertyItem_Value_Max_Text";
        private const string AssemblePartPropertyItem_Value_Min_Text = "AssemblePartPropertyItem_Value_Min_Text";

        public float CurrentValue
        {
            get { return slider.value/10; }
        }

        public override void Awake()
        {
            Line = UIUtility.FindTransfrom(transform, "Line");
            Name = UIUtility.SafeGetComponent<Text>(UIUtility.FindTransfrom(Line, "Name"));
            slider = UIUtility.SafeGetComponent<Slider>(UIUtility.FindTransfrom(Line, "Slider"));
            Value = UIUtility.SafeGetComponent<Text>(UIUtility.FindTransfrom(Line, "Value"));
            detailContentTrans = UIUtility.FindTransfrom(Line, "DetailContent");
            detailDescText = UIUtility.SafeGetComponent<Text>(UIUtility.FindTransfrom(detailContentTrans, "Desc"));
            contentTrans = UIUtility.FindTransfrom(detailContentTrans, "Content");
            detailContentTrans.gameObject.SetActive(false);

        }

        public void SetUpItem(Config.PartsCustomConfig.ConfigData config)
        {
            if (config == null)
                return;
            _config = config;
            Name.text = MultiLanguage.Instance.GetTextValue(config.CustomDataName);
            transform.localPosition = new Vector3((float)config.PosX, (float)config.PosY, 0);
            Line.GetComponent<RectTransform>().sizeDelta = new Vector2((float)config.LineWidth, 1);

            slider.minValue = (float)config.CustomDataRangeMin*10;
            slider.maxValue = (float)config.CustomDataRangeMax*10;
            slider.value = (float)config.CustomDataDefaultValue * 10;

            Value.text = string.Format("{0:N2}", config.CustomDataDefaultValue);
            slider.onValueChanged.AddListener((float value) => { OnSliderValueChanged(value); });

            detailDescText.text = MultiLanguage.Instance.GetTextValue(_config.LinkDesc);
            InitDetialContent();
        }

        void InitDetialContent()
        {
            foreach(Transform trans in contentTrans)
            {
                ObjectManager.Instance.ReleaseObject(trans.gameObject, 0);
            }

            for(int i = 0; i < _config.propertyLinkData.Count; i++)
            {
                var data = _config.propertyLinkData[i];

                var typeData = AssembleModule.GetAssemblePartPropertyTypeData(data.Name);
                if (typeData == null)
                    continue;

                if (data.PropertyChangeType == 1)
                {
                    if (data.PropertyChangePerUnitValue != 0)
                    {
                        SetUpPropertyItemSmall(
                            Utility.LoadSprite(typeData.PropertyIcon, Utility.SpriteType.png),
                            MultiLanguage.Instance.GetTextValue(typeData.PropertyName),
                            data.PropertyChangePerUnitValue.ToString());
                    }
                }
                else if (_config.propertyLinkData[i].PropertyChangeType == 2)
                {

                    if(data.PropertyChangePerUnitMin != 0)
                    {
                        ///Init Min
                        SetUpPropertyItemSmall(
                            Utility.LoadSprite(typeData.PropertyIcon, Utility.SpriteType.png),
                            Utility.ParseStringParams(MultiLanguage.Instance.GetTextValue(AssemblePartPropertyItem_Value_Min_Text),
                            new string[] { MultiLanguage.Instance.GetTextValue(typeData.PropertyName)}),
                            data.PropertyChangePerUnitMin.ToString());
                    }

                    if(data.PropertyChangePerUnitMax != 0)
                    {
                        ///Init Max
                        SetUpPropertyItemSmall(
                           Utility.LoadSprite(typeData.PropertyIcon, Utility.SpriteType.png),
                           Utility.ParseStringParams(MultiLanguage.Instance.GetTextValue(AssemblePartPropertyItem_Value_Max_Text),
                           new string[] { MultiLanguage.Instance.GetTextValue(typeData.PropertyName) }),
                           data.PropertyChangePerUnitMax.ToString());
                    }
                }
            }
        }

        void SetUpPropertyItemSmall(Sprite icon,string name,string value)
        {
            var obj = ObjectManager.Instance.InstantiateObject(UIPath.PrefabPath.Assemble_Part_PropertyItemSmall);
            if (obj != null)
            {
                var cmpt = UIUtility.SafeGetComponent<AssemblePartPropertyItemSmall>(obj.transform);
                cmpt.SetUpItem(icon, name, value);
                obj.transform.SetParent(contentTrans, false);
            }
        }

        void OnSliderValueChanged(float value)
        {
            Value.text = string.Format("{0:N2}", value / 10);
            UIManager.Instance.SendMessageToWnd(UIPath.WindowPath.Assemble_Part_Design_Page, new UIMessage(UIMsgType.Assemble_Part_PropertyChange, new List<object>(1) { _config ,CurrentValue}));
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            detailContentTrans.gameObject.SetActive(true);
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            detailContentTrans.gameObject.SetActive(false);
        }

    }
}