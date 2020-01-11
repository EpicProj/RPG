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

        public PartsPropertyConfig.ConfigData _configData;

        public float CurrentValueMin
        {
            get { return Utility.TryParseFloat(ValueMin.text); }
        }

        public float CurrentValueMax
        {
            get { return Utility.TryParseFloat(ValueMax.text); }
        }


        private void Awake()
        {
            Icon = UIUtility.SafeGetComponent<Image>(UIUtility.FindTransfrom(transform, "Icon"));
            Name= UIUtility.SafeGetComponent<Text>(UIUtility.FindTransfrom(transform, "Name"));
            ValueMin= UIUtility.SafeGetComponent<Text>(UIUtility.FindTransfrom(transform, "ValueMin"));
            ValueMax = UIUtility.SafeGetComponent<Text>(UIUtility.FindTransfrom(transform, "ValueMax"));
        }

        public void SetUpItem(PartsPropertyConfig.ConfigData config)
        {
            if (config == null)
                return;
            _configData = config;
            var typeData = AssembleModule.GetAssemblePartPropertyTypeData(config.Name);
            if (typeData != null)
            {
                Icon.sprite = Utility.LoadSprite(typeData.PropertyIcon, Utility.SpriteType.png);
                Name.text = MultiLanguage.Instance.GetTextValue(typeData.PropertyName);
            }
        }

        public void ChangeValueMin(float value)
        {
            ValueMin.text = string.Format("{0:N2}", value);
        }

        public void ChangeValueMax(float value)
        {

            ValueMax.text = string.Format("{0:N2}", value);
        }

    }
}