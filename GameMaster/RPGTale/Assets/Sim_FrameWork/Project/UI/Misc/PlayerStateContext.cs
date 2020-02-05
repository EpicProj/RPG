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
            UpdateResMonthData(ResourceType.All);
        }

        public override bool OnMessage(UIMessage msg)
        {
            switch (msg.type)
            {
                case UIMsgType.Res_Currency:
                    return UpdateResData(ResourceType.Currency);
                case UIMsgType.Res_MonthCurrency:
                    return UpdateResMonthData(ResourceType.Currency);
                case UIMsgType.Res_Research:
                    return UpdateResData(ResourceType.Research);
                case UIMsgType.Res_MonthResearch:
                    return UpdateResMonthData(ResourceType.Research);
                case UIMsgType.Res_Energy:
                    return UpdateResData(ResourceType.Energy);
                case UIMsgType.Res_MonthEnergy:
                    return UpdateResMonthData(ResourceType.Energy);
                case UIMsgType.Res_Builder:
                    return UpdateResData(ResourceType.Builder);
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
                    _researchPointText.text = data.Research.ToString();
                    _energyNumText.text = data.Energy.ToString();
                    _builderText.text = data.Builder.ToString();
                    _roCoreText.text = data.RoCore.ToString();
                    return true;
                case ResourceType.Currency:
                    _currencyNumText.text = data.Currency.ToString();
                    return true;
                case ResourceType.Research:
                    _researchPointText.text = data.Research.ToString();
                    return true;
                case ResourceType.Energy:
                    _energyNumText.text = data.Energy.ToString();
                    return true;
                case ResourceType.Builder:
                    _builderText.text = data.Builder.ToString();
                    return true;
                case ResourceType.RoCore:
                    _roCoreText.text = data.RoCore.ToString();
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// 更新当前资源增加值
        /// </summary>
        /// <param name="type"></param>
        public bool UpdateResMonthData(ResourceType type)
        {
            var data = PlayerManager.Instance.playerData.resourceData;
            if (data == null)
                return false;
            switch (type)
            {
                case ResourceType.All:
                    _energyAddNumText.text = "+" + data.EnergyPerDay.ToString();
                    _currencyAddNumText.text = "+" + data.CurrencyPerDay.ToString();
                    return true;
                case ResourceType.Energy:
                    _energyAddNumText.text = "+" + data.EnergyPerDay.ToString();
                    return true;
                case ResourceType.Research:
                    _researchPointAddText.text = "+" + data.ResearchPerDay.ToString();
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

        private Text _researchPointText;
        private Text _researchPointAddText;

        private Text _energyNumText;
        private Text _energyAddNumText;

        private Text _builderText;
        private Text _roCoreText;

        protected override void InitUIRefrence()
        {
            //Resource
            _currencyNumText = Transform.FindTransfrom("Resource/ResourceLeft/Currency/Value").SafeGetComponent<Text>();
            _currencyAddNumText = Transform.FindTransfrom("Resource/ResourceLeft/Currency/Value/AddValue").SafeGetComponent<Text>();

            _researchPointText = Transform.FindTransfrom("Resource/ResourceLeft/Research/Value").SafeGetComponent<Text>();
            _researchPointAddText = Transform.FindTransfrom("Resource/ResourceLeft/Research/Value/AddValue").SafeGetComponent<Text>();

            _energyNumText = Transform.FindTransfrom("Resource/ResourceLeft/Energy/Value").SafeGetComponent<Text>();
            _energyAddNumText = Transform.FindTransfrom("Resource/ResourceLeft/Energy/Value/AddValue").SafeGetComponent<Text>();

            _builderText = Transform.FindTransfrom("Resource/ResourceRight/Builder/Value").SafeGetComponent<Text>();
            _roCoreText = Transform.FindTransfrom("Resource/ResourceRight/RoCore/Value").SafeGetComponent<Text>();
        }

        void InitResImage()
        {
            Transform.FindTransfrom("Resource/ResourceLeft/Currency/Icon").SafeGetComponent<Image>().sprite = Utility.LoadSprite(Config.ConfigData.GlobalSetting.Resource_Currency_Icon_Path, Utility.SpriteType.png);
            Transform.FindTransfrom("Resource/ResourceLeft/Energy/Icon").SafeGetComponent<Image>().sprite = Utility.LoadSprite(Config.ConfigData.GlobalSetting.Resource_Energy_Icon_Path, Utility.SpriteType.png);
            Transform.FindTransfrom("Resource/ResourceLeft/Research/Icon").SafeGetComponent<Image>().sprite = Utility.LoadSprite(Config.ConfigData.GlobalSetting.Resource_Research_Icon_Path, Utility.SpriteType.png);
            Transform.FindTransfrom("Resource/ResourceRight/Builder/Icon").SafeGetComponent<Image>().sprite = Utility.LoadSprite(Config.ConfigData.GlobalSetting.Resource_Builder_Icon_Path, Utility.SpriteType.png);
            Transform.FindTransfrom("Resource/ResourceRight/RoCore/Icon").SafeGetComponent<Image>().sprite = Utility.LoadSprite(Config.ConfigData.GlobalSetting.Resource_Rocore_Icon_Path, Utility.SpriteType.png);
        }

    }
}