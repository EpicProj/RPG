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

        bool showShipChoosePanel = true;
        private string currentSelectTab = "";

        #region Override Method
        public override void Awake(params object[] paralist)
        {
            base.Awake(paralist);
            _info = (AssembleShipInfo)paralist[0];
            _partItemList = new List<AssembleShipCustomPartItem>();
            AddBtnClick();
        }

        public override bool OnMessage(UIMessage msg)
        {
            if(msg.type== UIMsgType.Assemble_ShipTab_Select)
            {
                string typeName = (string)msg.content[0];
                return RefreshChooseContent(typeName);
            }
            else if(msg.type == UIMsgType.Assemble_ShipPreset_Select)
            {
                int shipID=(int)msg.content[0];
                AssembleShipInfo shipInfo = new AssembleShipInfo(shipID);
                if (shipInfo.presetData._metaData != null)
                {
                    _info = shipInfo;
                    SetUpPage();
                }
            }
            return true;
        }

        public override void OnShow(params object[] paralist)
        {
            AudioManager.Instance.PlaySound(AudioClipPath.UISound.Page_Open);
            _info = (AssembleShipInfo)paralist[0];

            noDataInfoTrans.gameObject.SetActive(false);
            if (showShipChoosePanel)
            {
                SetUpShipChooseContent();
            }
            else
            {
                SetUpPage();
            }
           
        }

        public override void OnDisable()
        {
            AudioManager.Instance.PlaySound(AudioClipPath.UISound.Btn_Close);
            MapManager.Instance.ReleaseAssembleModel();
        }

        #endregion

        void AddBtnClick()
        {
            AddButtonClickListener(m_page.backBtn, () =>
            {
                UIManager.Instance.HideWnd(this);
            });
            AddButtonClickListener(m_page.presetChooseBtn, OnPresetChooseBtnClick);
        }
        void OnPresetChooseBtnClick()
        {
            AudioManager.Instance.PlaySound(AudioClipPath.UISound.Button_Click);
            UIUtility.ActiveCanvasGroup(shipChooseCanvasGroup, true);
            UIUtility.ActiveCanvasGroup(contentCanvasGroup, false);
        }


        void SetUpPage()
        {
            if (_info == null)
                return;
            UIUtility.ActiveCanvasGroup(shipChooseCanvasGroup, false);
            UIUtility.ActiveCanvasGroup(contentCanvasGroup, true);
            m_page.presetChooseBtn.gameObject.SetActive(true);

            _shipTypeIcon.sprite = _info.presetData.TypeIcon;
            _shipTypeText.text = _info.presetData.TypeName;
            _shipClassText.text = _info.presetData.shipClassName;
            _shipSizeText.text = _info.presetData.shipSizeText;
            _shipClassDesc.text = _info.presetData.shipClassDesc;
            if (shipClassDescTypeEffect != null)
                shipClassDescTypeEffect.StartEffect();
            MapManager.Instance.InitAssembleModel(_info.presetData._metaClass.ModelPath);

            m_page.presetTotalBtn.onClick.RemoveAllListeners();
            AddButtonClickListener(m_page.presetTotalBtn, OnPresetTotalBtnClick);
            refreshProperty();
            InitShipPartItem();
        }
        void OnPresetTotalBtnClick()
        {
            AudioManager.Instance.PlaySound(AudioClipPath.UISound.Button_Click);
            UIGuide.Instance.ShowAssembleShipChooseDialog(new List<string>() { currentSelectTab }, currentSelectTab);
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
            _partItemList.Clear();
            if (_info == null)
                return;
            
            foreach(Transform trans in customContentTrans)
            {
                ObjectManager.Instance.ReleaseObject(trans.gameObject, 0);
            }

            var configData = _info.presetData.partConfig.configData;
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

        #region Choose Content

        void SetUpShipChooseContent()
        {
            UIUtility.ActiveCanvasGroup(contentCanvasGroup, false);
            UIUtility.ActiveCanvasGroup(shipChooseCanvasGroup, true);
            m_page.presetChooseBtn.gameObject.SetActive(false);

            RefreshShipChooseTab();
            InitDefaultTabSelect();
            m_page.presetTotalBtn.onClick.RemoveAllListeners();
            AddButtonClickListener(m_page.presetTotalBtn, OnPresetTotalBtnClickAll);
        }

        void OnPresetTotalBtnClickAll()
        {
            AudioManager.Instance.PlaySound(AudioClipPath.UISound.Button_Click);
            var typeList = PlayerManager.Instance.GetTotalUnlockAssemblePartTypeList();
            UIGuide.Instance.ShowAssemblePartChooseDialog(typeList, currentSelectTab);
        }

        void RefreshShipChooseTab()
        {
            foreach (Transform trans in chooseTabTrans)
            {
                ObjectManager.Instance.ReleaseObject(trans.gameObject, 0);
            }

            var unlockList = PlayerManager.Instance.GetTotalUnlockAssembleShipTypeData();
            for (int i = 0; i < unlockList.Count; i++)
            {
                var obj = ObjectManager.Instance.InstantiateObject(UIPath.PrefabPath.General_ChooseTab);
                if (obj != null)
                {
                    var cmpt = UIUtility.SafeGetComponent<GeneralChooseTab>(obj.transform);
                    cmpt.SetUpTab(unlockList[i]);
                    obj.name = "ShipTab_" + i;
                    obj.transform.SetParent(chooseTabTrans, false);
                }
            }
        }

        void InitDefaultTabSelect()
        {
            string typeID = Config.ConfigData.AssembleConfig.assembleShipPage_DefaultSelectTab;
            if (AssembleModule.GetAssembleShipMianTypeData(typeID) != null)
            {
                RefreshChooseContent(typeID);
            }
        }
        bool RefreshChooseContent(string chooseType)
        {
            var shipModelList = PlayerManager.Instance.GetAssembleShipPresetModelList(chooseType);
            if (shipModelList.Count == 0)
            {
                noDataInfoTrans.gameObject.SetActive(true);
                chooseLoopList.gameObject.SetActive(false);
                if (noDataInfoAnim != null)
                    noDataInfoAnim.Play();
            }
            else
            {
                chooseLoopList.gameObject.SetActive(true);
                noDataInfoTrans.gameObject.SetActive(false);
                currentSelectTab = chooseType;
                chooseLoopList.InitData(shipModelList);
                if (shipChooseAnim != null)
                    shipChooseAnim.Play();
            }
            return true;
        }

        #endregion
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
        private CanvasGroup contentCanvasGroup;

        private TypeWriterEffect shipClassDescTypeEffect;

        private Transform customContentTrans;
        private Transform chooseTabTrans;
        private Transform noDataInfoTrans;
        private Animation noDataInfoAnim;
        private LoopList chooseLoopList;
        private Animation shipChooseAnim;
        private CanvasGroup shipChooseCanvasGroup;

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
            contentCanvasGroup = UIUtility.SafeGetComponent<CanvasGroup>(UIUtility.FindTransfrom(Transform, "Content"));

            customContentTrans = UIUtility.FindTransfrom(m_page.CustomPanel, "Content");
            chooseTabTrans = UIUtility.FindTransfrom(m_page.ChooseContent, "ChooseTab");
            noDataInfoTrans = UIUtility.FindTransfrom(m_page.ChooseContent, "EmptyInfo");
            noDataInfoAnim = UIUtility.SafeGetComponent<Animation>(noDataInfoTrans);
            chooseLoopList = UIUtility.SafeGetComponent<LoopList>(UIUtility.FindTransfrom(m_page.ChooseContent, "ChooseContent/Scroll View"));
            shipChooseAnim = UIUtility.SafeGetComponent<Animation>(m_page.ChooseContent);
            shipChooseCanvasGroup = UIUtility.SafeGetComponent<CanvasGroup>(m_page.ChooseContent);

        }


    }
}