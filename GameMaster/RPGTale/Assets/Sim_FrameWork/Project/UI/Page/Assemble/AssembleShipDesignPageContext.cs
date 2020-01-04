using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sim_FrameWork.UI
{
    public partial class AssembleShipDesignPageContext : WindowBase
    {
        private AssembleShipInfo _info;
        private List<AssembleShipCustomPartItem> _partItemList;

        #region Override Method
        public override void Awake(params object[] paralist)
        {
            base.Awake(paralist);
            _info = (AssembleShipInfo)paralist[0];
            _partItemList = new List<AssembleShipCustomPartItem>();
        }

        public override bool OnMessage(UIMessage msg)
        {
            return base.OnMessage(msg);
        }

        public override void OnShow(params object[] paralist)
        {
            AudioManager.Instance.PlaySound(AudioClipPath.UISound.Page_Open);
            _info = (AssembleShipInfo)paralist[0];
            SetUpPage();
        }


        #endregion


        void SetUpPage()
        {
            if (_info == null)
                return;

            _shipTypeIcon.sprite = _info.typeIcon;
            _shipTypeText.text = _info.typeName;
            _shipClassText.text = _info.className;
            _shipSizeText.text = _info.shipSizeText;
            _shipClassDesc.text = _info.classDesc;
            if (shipClassDescTypeEffect != null)
                shipClassDescTypeEffect.StartEffect();
            MapManager.Instance.InitAssembleModel(_info.modelPath);

            refreshProperty();
            InitShipPartItem();
        }

        bool refreshProperty()
        {
            _propertyDurabilityText.text = _info.shipDurability.ToString();
            _propertySpeedText.text = _info.shipSpeed.ToString();
            _propertyFirePowerText.text = _info.shipFirePower.ToString();
            _propertyExploreText.text = _info.shipDetect.ToString();
            _propertyMemberText.text = _info.shipCrewMax.ToString();
            _propertyStorageText.text = _info.shipStorage.ToString();
            return true;
        }

        void InitShipPartItem()
        {
            if (_info == null)
                return;
            var configData = _info.partConfig.configData;
            for (int i=0;i< configData.Count; i++)
            {
                GameObject obj = null;
                if (string.Compare(configData[i].PosType, "Top") == 0)
                {
                    obj = ObjectManager.Instance.InstantiateObject(UIPath.PrefabPath.Assemble_Ship_Part_Top);
                }
                else if(string.Compare(configData[i].PosType, "Bottom") == 0)
                {
                    obj = ObjectManager.Instance.InstantiateObject(UIPath.PrefabPath.Assemble_Ship_Part_Bottom);
                }

                if (obj != null)
                {
                    AssembleShipCustomPartItem item = UIUtility.SafeGetComponent<AssembleShipCustomPartItem>(obj.transform);
                    if (item != null)
                    {
                        item.SetUpItem(configData[i]);
                        item.transform.SetParent(customContentTrans,false);
                        item.name = "Assemble_Parts_" + i;
                        _partItemList.Add(item);
                    }
                }
            }
        }

    }


    public partial class AssembleShipDesignPageContext : WindowBase
    {
        private AssembleShipDesignPage m_page;

        private Text _propertyDurabilityText;
        private Text _propertySpeedText;
        private Text _propertyFirePowerText;
        private Text _propertyExploreText;
        private Text _propertyMemberText;
        private Text _propertyStorageText;

        private Image _shipTypeIcon;
        private Text _shipTypeText;
        private Text _shipClassText;
        private Text _shipSizeText;
        private Text _shipClassDesc;

        private TypeWriterEffect shipClassDescTypeEffect;

        private Transform customContentTrans;

        protected override void InitUIRefrence()
        {
            m_page = UIUtility.SafeGetComponent<AssembleShipDesignPage>(Transform);

            _propertyDurabilityText = UIUtility.SafeGetComponent<Text>(UIUtility.FindTransfrom(m_page.RightPanel, "PartProperty/Content/Durability/Value"));
            _propertySpeedText = UIUtility.SafeGetComponent<Text>(UIUtility.FindTransfrom(m_page.RightPanel, "PartProperty/Content/Speed/Value"));
            _propertyFirePowerText= UIUtility.SafeGetComponent<Text>(UIUtility.FindTransfrom(m_page.RightPanel, "PartProperty/Content/FirePower/Value"));
            _propertyExploreText= UIUtility.SafeGetComponent<Text>(UIUtility.FindTransfrom(m_page.RightPanel, "PartProperty/Content/Explore/Value"));
            _propertyMemberText= UIUtility.SafeGetComponent<Text>(UIUtility.FindTransfrom(m_page.RightPanel, "PartProperty/Content/Member/Value"));
            _propertyStorageText= UIUtility.SafeGetComponent<Text>(UIUtility.FindTransfrom(m_page.RightPanel, "PartProperty/Content/Storage/Value"));

            _shipTypeIcon = UIUtility.SafeGetComponent<Image>(UIUtility.FindTransfrom(m_page.leftPanel, "ShipInfo/Content/Type/Icon"));
            _shipTypeText = UIUtility.SafeGetComponent<Text>(UIUtility.FindTransfrom(m_page.leftPanel, "ShipInfo/Content/Type/Name"));
            _shipClassText = UIUtility.SafeGetComponent<Text>(UIUtility.FindTransfrom(m_page.leftPanel, "ShipInfo/Content/Class/Name"));
            _shipSizeText= UIUtility.SafeGetComponent<Text>(UIUtility.FindTransfrom(m_page.leftPanel, "ShipInfo/Content/Size/Name"));
            _shipClassDesc = UIUtility.SafeGetComponent<Text>(UIUtility.FindTransfrom(m_page.leftPanel, "ShipDesc/Text"));
            shipClassDescTypeEffect = UIUtility.SafeGetComponent<TypeWriterEffect>(_shipClassDesc.transform);

            customContentTrans = UIUtility.FindTransfrom(m_page.CustomPanel, "Content");
        }


    }
}