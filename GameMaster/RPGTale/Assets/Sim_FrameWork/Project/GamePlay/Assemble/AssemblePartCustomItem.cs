using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace Sim_FrameWork
{
    public class AssemblePartCustomItem : MonoBehaviour
    {
        private Transform Line;
        private Text Name;
        private Slider slider;
        private Text Value;

        public PartsCustomConfig.ConfigData _config;

        public float CurrentValue
        {
            get { return slider.value/10; }
        }

        private void Awake()
        {
            Line = UIUtility.FindTransfrom(transform, "Line");
            Name = UIUtility.SafeGetComponent<Text>(UIUtility.FindTransfrom(Line, "Name"));
            slider = UIUtility.SafeGetComponent<Slider>(UIUtility.FindTransfrom(Line, "Slider"));
            Value = UIUtility.SafeGetComponent<Text>(UIUtility.FindTransfrom(Line, "Value"));

            
        }

        public void SetUpItem(PartsCustomConfig.ConfigData config)
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

        }

        void OnSliderValueChanged(float value)
        {
            Value.text = string.Format("{0:N2}", value / 10);
            UIManager.Instance.SendMessageToWnd(UIPath.WindowPath.Assemble_Part_Design_Page, new UIMessage(UIMsgType.Assemble_Part_PropertyChange, new List<object>(1) { _config ,CurrentValue}));
        }

      
    }
}