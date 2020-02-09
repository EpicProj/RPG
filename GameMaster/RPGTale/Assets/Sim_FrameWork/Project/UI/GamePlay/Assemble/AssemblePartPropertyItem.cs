using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sim_FrameWork
{
    public class AssemblePartPropertyItem : MonoBehaviour
    {
        private Text ValueMin;
        private Text ValueMax;
        private Transform DotTrans;

        public Config.PartsPropertyConfig.ConfigData _configData;

        public Dictionary<string, AssemblePartPropertyDetailInfo> detailInfoDic;

        public float CurrentValueMin
        {
            get
            {
                float value = (float)_configData.PropertyValue;
                foreach (var detailInfo in detailInfoDic.Values)
                    value += detailInfo.modifyValueMin;
                return value;
            }
        }

        public float CurrentValueMax
        {
            get
            {
                float value = (float)_configData.PropertyValue;
                if (_configData.PropertyType == 1)
                {
                    foreach (var detailInfo in detailInfoDic.Values)
                        value += detailInfo.modifyValueFix;
                }
                else if (_configData.PropertyType == 2)
                {
                    foreach (var detailInfo in detailInfoDic.Values)
                        value += detailInfo.modifyValueMax;
                }
                return value;
            }
        }


        private void Awake()
        {
            ValueMin= transform.FindTransfrom("ValueMin").SafeGetComponent<Text>();
            ValueMax = transform.FindTransfrom("ValueMax").SafeGetComponent<Text>();
            DotTrans = transform.FindTransfrom("Dot");

            detailInfoDic = new Dictionary<string, AssemblePartPropertyDetailInfo>();
        }

        public void SetUpItem(Config.PartsPropertyConfig.ConfigData config)
        {
            if (config == null)
                return;
            _configData = config;

            if (config.PropertyType == 1)
            {
                ///Fix Value
                ValueMin.gameObject.SetActive(false);
                DotTrans.gameObject.SetActive(false);

                ValueMax.text = ValueFormat(config, (float)config.PropertyValue);
            }
            else if (config.PropertyType == 2)
            {
                ///Value Range
                ValueMin.gameObject.SetActive(true);
                DotTrans.gameObject.SetActive(true);

                ValueMax.text = ValueFormat(config, (float)config.PropertyRangeMax);
                ValueMin.text = ValueFormat(config, (float)config.PropertyRangeMin); 
            }

            var typeData = AssembleModule.GetAssemblePartPropertyTypeData(config.Name);
            if (typeData != null)
            {
                transform.FindTransfrom("Icon").SafeGetComponent<Image>().sprite = Utility.LoadSprite(typeData.PropertyIcon);
                transform.FindTransfrom("Name").SafeGetComponent<Text>().text = MultiLanguage.Instance.GetTextValue(typeData.PropertyName);
            }
        }

        /// <summary>
        /// For Fix Value & Range Min
        /// </summary>
        /// <param name="value"></param>
        public void ChangeValue(AssemblePartPropertyDetailInfo info)
        {
            if (detailInfoDic.ContainsKey(info.customDataName))
            {
                var detailInfo = detailInfoDic[info.customDataName];
                if (detailInfo.modifyType == 1)
                {
                    detailInfo.modifyValueFix = info.modifyValueFix;
                }
                else if(detailInfo.modifyType == 2)
                {
                    detailInfo.modifyValueMin = info.modifyValueMin;
                    detailInfo.modifyValueMax = info.modifyValueMax;
                }
            }
            else
            {
                detailInfoDic.Add(info.customDataName, info);
            }

            var propertyData = AssembleModule.GetAssemblePartPropertyTypeData(info.propertyLinkName);

            ValueMin.text = ValueFormat(propertyData, CurrentValueMin);
            ValueMax.text = ValueFormat(propertyData, CurrentValueMax);
        }

        string ValueFormat(Config.PartsPropertyConfig.ConfigData config,float value)
        {
            var propertyData = AssembleModule.GetAssemblePartPropertyTypeData(config.Name);
            return ValueFormat(propertyData, value);
        }

        string ValueFormat(AssemblePartPropertyTypeData type, float value)
        {
            if (type.Type == 1)
            {
                ///Two decimal places
                return string.Format("{0:N2}", value);
            }
            else if (type.Type == 2)
            {
                ///One decimal places
                return string.Format("{0:N1}", value);
            }
            else if(type.Type == 3)
            {
                return ((int)value).ToString();
            }
            return string.Empty;
        }



    }
}