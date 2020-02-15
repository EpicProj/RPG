using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Sim_FrameWork.UI
{
    public partial class PlayerStateContext : WindowBase
    {

        public override void Awake(params object[] paralist)
        {
            base.Awake(paralist);
            InitResImage();
            AddBtnClick();
        }

        public override void OnShow(params object[] paralist)
        {
            base.OnShow(paralist);
            UpdateResData(ResourceType.All);
            UpdateResPoolDailyData(ResourceType.All);
        }

        public override bool OnMessage(UIMessage msg)
        {
            switch (msg.type)
            {
                case UIMsgType.Res_Currency:
                    return UpdateResData(ResourceType.Currency);
                case UIMsgType.Res_Research:
                    return UpdateResData(ResourceType.Research);
                case UIMsgType.Res_Energy:
                    return UpdateResData(ResourceType.Energy);
                case UIMsgType.Res_Daily_Total:
                    return UpdateResPoolDailyData(ResourceType.All);
                case UIMsgType.Res_AIRobot_Builder:
                    return UpdateResData(ResourceType.AIRobot_Builder);
                case UIMsgType.Res_RoCore:
                    return UpdateResData(ResourceType.RoCore);
            }
            return false;
        }

        private void AddBtnClick()
        {
            AddButtonClickListener(Transform.FindTransfrom("Menu").SafeGetComponent<Button>(), () =>
            {
                UIGuide.Instance.ShowMenuDialog();
            });
     
        }

        /// <summary>
        /// 更新当前资源
        /// </summary>
        /// <param name="type"></param>
        public bool UpdateResData(ResourceType type)
        {
            var data = PlayerManager.Instance.playerData.resourceData;
            if (data == null)
                return false;
            switch (type)
            {
                case ResourceType.All:
                    _currencyNumText.text = data.Currency.ToString();
                    _currencySlider.value = (data.Currency / (float)data.CurrencyMax) * 100;

                    _researchPointText.text = data.Research.ToString();
                    _researchSlider.value = (data.Research / data.ResearchMax) * 100;

                    _energyNumText.text = ((int)data.Energy).ToString();
                    _energySlider.value = (data.Energy / data.EnergyMax) * 100;

                    _AIRobotText.text = data.AIRobotTotalNum.ToString();

                    _roCoreText.text = data.RoCore.ToString();
                    _roCoreSlider.value = (data.RoCore / (float)data.RoCoreMax) * 100;
                    return true;
                case ResourceType.Currency:
                    _currencyNumText.text = data.Currency.ToString();
                    _currencySlider.value = (data.Currency / data.CurrencyMax) * 100;
                    return true;
                case ResourceType.Research:
                    _researchPointText.text = data.Research.ToString();
                    _researchSlider.value = (data.Research / data.ResearchMax) * 100;
                    return true;
                case ResourceType.Energy:
                    _energyNumText.text = data.Energy.ToString();
                    _energySlider.value = (data.Energy / data.EnergyMax) * 100;
                    return true;
                case ResourceType.AIRobot_Builder:
                case ResourceType.AIRobot_Maintenance:
                case ResourceType.AIRobot_Operator:
                    _AIRobotText.text = data.AIRobotTotalNum.ToString();
                    return true;
                case ResourceType.RoCore:
                    _roCoreText.text = data.RoCore.ToString();
                    _roCoreSlider.value = (data.RoCore / data.RoCoreMax) * 100;
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// 更新当前资源增加值
        /// </summary>
        /// <param name="type"></param>
        public bool UpdateResPoolDailyData(ResourceType type)
        {
            var data = PlayerManager.Instance.playerData.resourceData;
            if (data == null)
                return false;
            switch (type)
            {
                case ResourceType.All:
                    var _energyAddValue = data.EnergyDailySettleDisplay;
                    _energyAddNumText.text = _energyAddValue >= 0 ? "+" + _energyAddValue.ToString() : _energyAddValue.ToString();
                    _energyAddNumText.color = _energyAddValue >= 0 ? resource_Green_Color : resource_Red_Color;

                    _currencyAddNumText.text = data.CurrencyPerDay >= 0 ? "+" + data.CurrencyPerDay.ToString() : data.CurrencyPerDay.ToString();
                    _researchPointAddText.text = data.ResearchPerDay >= 0 ? "+" + data.ResearchPerDay.ToString() : data.ResearchPerDay.ToString();
                    return true;
                case ResourceType.Energy:
                    return true;
                case ResourceType.Research:
                    _researchPointAddText.text = data.ResearchPerDay >= 0 ? "+" + data.ResearchPerDay.ToString() : data.ResearchPerDay.ToString();
                    return true;
                case ResourceType.Currency:
                    _currencyAddNumText.text = data.CurrencyPerDay >= 0 ? "+" + data.CurrencyPerDay.ToString() : data.CurrencyPerDay.ToString();
                    return true;
                default:
                    return false;
            }
        }

    }

    public partial class PlayerStateContext: WindowBase
    {
        /// <summary>
        /// Resource
        /// </summary>
        private Text _currencyNumText;
        private Text _currencyAddNumText;
        private Slider _currencySlider;

        private Text _researchPointText;
        private Text _researchPointAddText;
        private Slider _researchSlider;

        private Text _energyNumText;
        private Text _energyAddNumText;
        private Slider _energySlider;

        private Text _AIRobotText;
        private Slider _AIRobotSlider;
        private Text _roCoreText;
        private Slider _roCoreSlider;

        private Color resource_Green_Color = new Color(0, 0.84f, 0.17f);
        private Color resource_Red_Color = new Color(1, 0.13f, 0);


        protected override void InitUIRefrence()
        {
            //Resource
            _currencyNumText = Transform.FindTransfrom("Resource/ResourceLeft/Currency/Value").SafeGetComponent<Text>();
            _currencyAddNumText = Transform.FindTransfrom("Resource/ResourceLeft/Currency/Value/AddValue").SafeGetComponent<Text>();
            _currencySlider = Transform.FindTransfrom("Resource/ResourceLeft/Currency/Slider").SafeGetComponent<Slider>();

            _researchPointText = Transform.FindTransfrom("Resource/ResourceLeft/Research/Value").SafeGetComponent<Text>();
            _researchPointAddText = Transform.FindTransfrom("Resource/ResourceLeft/Research/Value/AddValue").SafeGetComponent<Text>();
            _researchSlider = Transform.FindTransfrom("Resource/ResourceLeft/Research/Slider").SafeGetComponent<Slider>();

            _energyNumText = Transform.FindTransfrom("Resource/ResourceLeft/Energy/Value").SafeGetComponent<Text>();
            _energyAddNumText = Transform.FindTransfrom("Resource/ResourceLeft/Energy/Value/AddValue").SafeGetComponent<Text>();
            _energySlider= Transform.FindTransfrom("Resource/ResourceLeft/Energy/Slider").SafeGetComponent<Slider>();

            _AIRobotText = Transform.FindTransfrom("Resource/ResourceRight/AIRobot/Value").SafeGetComponent<Text>();
            _AIRobotSlider= Transform.FindTransfrom("Resource/ResourceRight/AIRobot/Slider").SafeGetComponent<Slider>();
            _roCoreText = Transform.FindTransfrom("Resource/ResourceRight/RoCore/Value").SafeGetComponent<Text>();
            _roCoreSlider = Transform.FindTransfrom("Resource/ResourceRight/RoCore/Slider").SafeGetComponent<Slider>();
        }

        void InitResImage()
        {
            Transform.FindTransfrom("Resource/ResourceLeft/Currency/Icon").SafeGetComponent<Image>().sprite = Utility.LoadSprite(Config.ConfigData.GlobalSetting.Resource_Currency_Icon_Path);
            Transform.FindTransfrom("Resource/ResourceLeft/Energy/Icon").SafeGetComponent<Image>().sprite = Utility.LoadSprite(Config.ConfigData.GlobalSetting.Resource_Energy_Icon_Path);
            Transform.FindTransfrom("Resource/ResourceLeft/Research/Icon").SafeGetComponent<Image>().sprite = Utility.LoadSprite(Config.ConfigData.GlobalSetting.Resource_Research_Icon_Path);
            Transform.FindTransfrom("Resource/ResourceRight/AIRobot/Icon").SafeGetComponent<Image>().sprite = Utility.LoadSprite(Config.ConfigData.GlobalSetting.Resource_Builder_Icon_Path);
            Transform.FindTransfrom("Resource/ResourceRight/RoCore/Icon").SafeGetComponent<Image>().sprite = Utility.LoadSprite(Config.ConfigData.GlobalSetting.Resource_Rocore_Icon_Path);
        }

    }
}