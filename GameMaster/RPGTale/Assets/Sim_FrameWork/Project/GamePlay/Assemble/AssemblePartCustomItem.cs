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
        private Dictionary<string,AssemblePartPropertyItemSmall> _propertyItemDic;

        private const string AssemblePartPropertyItem_Value_Max_Text = "AssemblePartPropertyItem_Value_Max_Text";
        private const string AssemblePartPropertyItem_Value_Min_Text = "AssemblePartPropertyItem_Value_Min_Text";

        public float CurrentValue
        {
            get { return slider.value/10; }
        }

        public override void Awake()
        {
            _propertyItemDic = new Dictionary<string, AssemblePartPropertyItemSmall>();

            Line = transform.FindTransfrom("Line");
            Name = Line.FindTransfrom("Name").SafeGetComponent<Text>();
            slider = Line.FindTransfrom("Slider").SafeGetComponent<Slider>();
            Value = Line.FindTransfrom("Value").SafeGetComponent<Text>();
            detailContentTrans = Line.FindTransfrom("DetailContent");
            detailDescText = detailContentTrans.FindTransfrom("Desc").SafeGetComponent<Text>();
            contentTrans = detailContentTrans.FindTransfrom("Content");
            detailContentTrans.SafeSetActive(false);

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

            Value.text = string.Format("{0:N1}", config.CustomDataDefaultValue);
            slider.onValueChanged.AddListener((float value) => { OnSliderValueChanged(value); });

            detailDescText.text = MultiLanguage.Instance.GetTextValue(_config.LinkDesc);
            InitDetialContent();
        }

        void InitDetialContent()
        {
            contentTrans.ReleaseAllChildObj();
            _propertyItemDic.Clear();

            int diffValue = (int)Math.Round((CurrentValue - _config.CustomDataRangeMin) * 10, 0);

            for (int i = 0; i < _config.propertyLinkData.Count; i++)
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
                            Utility.LoadSprite(typeData.PropertyIcon),
                            MultiLanguage.Instance.GetTextValue(typeData.PropertyName),
                            (float)(diffValue*data.PropertyChangePerUnitValue),
                            data.Name);
                    }
                }
                else if (data.PropertyChangeType == 2)
                {

                    if(data.PropertyChangePerUnitMin != 0)
                    {
                        ///Init Min
                        SetUpPropertyItemSmall(
                            Utility.LoadSprite(typeData.PropertyIcon),
                            Utility.ParseStringParams(MultiLanguage.Instance.GetTextValue(AssemblePartPropertyItem_Value_Min_Text),
                            new string[] { MultiLanguage.Instance.GetTextValue(typeData.PropertyName)}),
                            (float)(diffValue*data.PropertyChangePerUnitMin),
                            data.Name);
                    }

                    if(data.PropertyChangePerUnitMax != 0)
                    {
                        ///Init Max
                        SetUpPropertyItemSmall(
                           Utility.LoadSprite(typeData.PropertyIcon),
                           Utility.ParseStringParams(MultiLanguage.Instance.GetTextValue(AssemblePartPropertyItem_Value_Max_Text),
                           new string[] { MultiLanguage.Instance.GetTextValue(typeData.PropertyName) }),
                           (float)(diffValue*data.PropertyChangePerUnitMax),
                           data.Name);
                    }
                }
            }

            ///Init Time
            if (_config.TimeCostPerUnit != 0)
            {
                SetUpPropertyItemSmall(
                    Utility.LoadSprite(Config.ConfigData.GlobalSetting.General_Time_Icon),
                    MultiLanguage.Instance.GetTextValue(Config.ConfigData.GlobalSetting.General_Time_Cost_TextID),
                    (float)(diffValue*_config.TimeCostPerUnit),
                    "Time");
            }

        }

        void SetUpPropertyItemSmall(Sprite icon,string name,float value,string propertyName)
        {
            var obj = ObjectManager.Instance.InstantiateObject(UIPath.PrefabPath.Assemble_Part_PropertyItemSmall);
            if (obj != null)
            {
                var cmpt = UIUtility.SafeGetComponent<AssemblePartPropertyItemSmall>(obj.transform);
                cmpt.SetUpItem(icon, name, value, propertyName);
                obj.transform.SetParent(contentTrans, false);
                if(!_propertyItemDic.ContainsKey(propertyName))
                    _propertyItemDic.Add(propertyName, cmpt);
            }
        }


        void OnSliderValueChanged(float value)
        {
            Value.text = string.Format("{0:N1}", value / 10);
            UIManager.Instance.SendMessageToWnd(UIPath.WindowPath.Assemble_Part_Design_Page, new UIMessage(UIMsgType.Assemble_Part_PropertyChange, new List<object>(1) { _config ,CurrentValue}));
            UpdateDetailValue();
        }

        void UpdateDetailValue()
        {

            int diffValue =(int)Math.Round((CurrentValue - _config.CustomDataRangeMin) * 10,0);

            for (int i = 0; i < _config.propertyLinkData.Count; i++)
            {
                var data = _config.propertyLinkData[i];
                if (_propertyItemDic.ContainsKey(data.Name))
                {
                    if (data.PropertyChangeType == 1)
                    {
                        if (data.PropertyChangePerUnitValue != 0)
                        {
                            _propertyItemDic[data.Name].RefreshValue((float)(diffValue * data.PropertyChangePerUnitValue));
                        }
                    }
                    else if (data.PropertyChangeType == 2)
                    {

                        if (data.PropertyChangePerUnitMin != 0)
                        {
                            ///Refresh Min
                            _propertyItemDic[data.Name].RefreshValue((float)(diffValue * data.PropertyChangePerUnitMin));
                        }

                        if (data.PropertyChangePerUnitMax != 0)
                        {
                            ///Refresh Max
                            _propertyItemDic[data.Name].RefreshValue((float)(diffValue * data.PropertyChangePerUnitMax));
                        }
                    }
                }
            }
            ///UpdateTime
            if (_propertyItemDic.ContainsKey("Time"))
            {
                _propertyItemDic["Time"].RefreshValue((float)(diffValue * _config.TimeCostPerUnit));
            }
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            AudioManager.Instance.PlaySound(AudioClipPath.UISound.Click_Dot);
            detailContentTrans.gameObject.SetActive(true);
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            detailContentTrans.gameObject.SetActive(false);
        }

    }
}