using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sim_FrameWork
{
    public class AssemblePartPropertyItem : MonoBehaviour
    {
        private Image Icon;
        private Text Name;
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
            Icon = UIUtility.SafeGetComponent<Image>(UIUtility.FindTransfrom(transform, "Icon"));
            Name= UIUtility.SafeGetComponent<Text>(UIUtility.FindTransfrom(transform, "Name"));
            ValueMin= UIUtility.SafeGetComponent<Text>(UIUtility.FindTransfrom(transform, "ValueMin"));
            ValueMax = UIUtility.SafeGetComponent<Text>(UIUtility.FindTransfrom(transform, "ValueMax"));
            DotTrans = UIUtility.FindTransfrom(transform, "Dot");

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

                ValueMax.text = string.Format("{0:N2}", config.PropertyValue);
            }
            else if (config.PropertyType == 2)
            {
                ///Value Range
                ValueMin.gameObject.SetActive(true);
                DotTrans.gameObject.SetActive(true);

                ValueMax.text = string.Format("{0:N2}", config.PropertyRangeMax);
                ValueMin.text = string.Format("{0:N2}", config.PropertyRangeMin);
            }

            var typeData = AssembleModule.GetAssemblePartPropertyTypeData(config.Name);
            if (typeData != null)
            {
                Icon.sprite = Utility.LoadSprite(typeData.PropertyIcon, Utility.SpriteType.png);
                Name.text = MultiLanguage.Instance.GetTextValue(typeData.PropertyName);
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

            ValueMin.text = string.Format("{0:N2}", CurrentValueMin);
            ValueMax.text = string.Format("{0:N2}", CurrentValueMax);

        }

    }
}