using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sim_FrameWork
{
    public class GeneralSliderSelectElement
    {
        public int index;
        public float value;
        public float linkParam;
        public bool showScaleSymbol;
    }

    public class SliderSelectItem : MonoBehaviour
    {

        private List<GeneralSliderSelectElement> elementList;
        private Slider slider;
        private Text valueText;

        private string configID;

        private const string SliderDot_PrefabPath = "Assets/Prefabs/Object/Main/General/SliderDot.prefab";

        private void Awake()
        {
            slider = transform.FindTransfrom("Slider").SafeGetComponent<Slider>();
            valueText = transform.FindTransfrom("Value").SafeGetComponent<Text>();
        }

        public void SetUpItem_General(int currentLevel, List<GeneralSliderSelectElement> elementList)
        {
            this.elementList = elementList;
            slider.onValueChanged.RemoveAllListeners();

            if (elementList==null || elementList.Count == 0)
            {
                DebugPlus.LogError("SliderSelectItem Init Fail List is null or 0");
                return;
            }
            InitSliderNood();
            slider.minValue = 1;
            slider.maxValue = elementList.Count;
           
            //Init Update
            slider.value = currentLevel;
        }
        void InitSliderNood()
        {
            if (elementList == null || elementList.Count<=1)
                return;

            var trans = transform.FindTransfrom("Slider/Nood");
            float sliderLengh = transform.FindTransfrom("Slider").SafeGetComponent<RectTransform>().rect.width;
            float delta = sliderLengh / (elementList.Count - 1);
            trans.InitObj(SliderDot_PrefabPath, elementList.Count);
            for(int i = 0; i < elementList.Count; i++)
            {
                var item = trans.GetChild(i);
                item.SetAnchoredPosX(i * delta);
            }
        }
        void OnSliderValueChange_General(float value)
        {
            int currentValue = (int)value;
            if (currentValue > elementList.Count)
                return;
            var element = elementList[currentValue - 1];
            if (element.showScaleSymbol)
            {
                valueText.text = "X " + element.value.ToString();
            }
            else
            {
                valueText.text = element.value.ToString();
            }
        }

        /// <summary>
        /// Game Prepare Data
        /// </summary>
        /// <param name="currentLevel"></param>
        /// <param name="elementList"></param>
        /// <param name="configID"></param>
        public void SetUpItem_GamePrepare(int currentLevel, List<GeneralSliderSelectElement> elementList, string configID)
        {
            SetUpItem_General(currentLevel, elementList);
            this.configID = configID;
            slider.onValueChanged.AddListener((float value) => OnSliderValueChange_GamePrepare(value));
            OnSliderValueChange_GamePrepare(currentLevel);
        }

        void OnSliderValueChange_GamePrepare(float value)
        {
            OnSliderValueChange_General(value);
            ///UpdateValue
            DataManager.Instance.ChangeGamePrepareValue(configID, (byte)value);
        }

        public void SetUpItem_AIPrepare(int currentLevel, List<GeneralSliderSelectElement> elementList, string configID)
        {
            SetUpItem_General(currentLevel, elementList);
            this.configID = configID;
            slider.onValueChanged.AddListener((float value) => OnSliderValueChange_AIPrepare(value));
            OnSliderValueChange_AIPrepare(currentLevel);
        }
        void OnSliderValueChange_AIPrepare(float value)
        {
            OnSliderValueChange_General(value);
            DataManager.Instance.ChangeAIPrepareValue(configID, (byte)value);
        }
    }
}