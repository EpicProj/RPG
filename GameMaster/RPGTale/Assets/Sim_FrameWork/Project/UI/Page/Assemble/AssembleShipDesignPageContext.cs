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
                AssembleShipInfo shipInfo = new AssembleShipInfo();
                shipInfo.InitData(shipID);
                if (shipInfo.presetData._metaData != null)
                {
                    _info = shipInfo;
                    SetUpPage();
                }
            }
            else if (msg.type == UIMsgType.Assemble_ShipDesign_PartSelect)
            {
                ushort UID = (ushort)msg.content[0];
                int configID = (int)msg.content[1];
                return OnPartEquip(UID, configID);
            }
            return true;
        }

        public override void OnShow(params object[] paralist)
        {
            AudioManager.Instance.PlaySound(AudioClipPath.UISound.Page_Open);
            _info = (AssembleShipInfo)paralist[0];

            noDataInfoTrans.SafeSetActive(false);
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
            AddButtonClickListener(Transform.FindTransfrom("Back").SafeGetComponent<Button>(), () =>
            {
                UIManager.Instance.HideWnd(this);
                UIGuide.Instance.ShowGameMainPage();
            });
            AddButtonClickListener(presetChooseBtn, OnPresetChooseBtnClick);
            AddButtonClickListener(shipDesignSaveBtn, OnShipDesignBtnClick);
        }
        void OnPresetChooseBtnClick()
        {
            AudioManager.Instance.PlaySound(AudioClipPath.UISound.Button_Click);
            shipChooseCanvasGroup.ActiveCanvasGroup(true);
            contentCanvasGroup.ActiveCanvasGroup(false);
        }


        void SetUpPage()
        {
            if (_info == null)
                return;
            shipChooseCanvasGroup.ActiveCanvasGroup(false);
            contentCanvasGroup.ActiveCanvasGroup(true);
            presetChooseBtn.transform.SafeSetActive(true);

            _shipTypeIcon.sprite = _info.presetData.TypeIcon;
            _shipTypeText.text = _info.presetData.TypeName;
            _shipClassText.text = _info.presetData.shipClassName;
            _shipSizeText.text = _info.presetData.shipSizeText;
            _shipClassDesc.text = _info.presetData.shipClassDesc;
            if (shipClassDescTypeEffect != null)
                shipClassDescTypeEffect.StartEffect();
            MapManager.Instance.InitAssembleModel(_info.presetData._metaClass.ModelPath);

            presetTotalBtn.onClick.RemoveAllListeners();
            AddButtonClickListener(presetTotalBtn, OnPresetTotalBtnClick);
            refreshProperty(true);
            InitShipPartItem();
            RefreshShipBaseCost();

            if (shipContentAnim != null)
                shipContentAnim.Play();
        }
        void OnPresetTotalBtnClick()
        {
            AudioManager.Instance.PlaySound(AudioClipPath.UISound.Button_Click);
            UIGuide.Instance.ShowAssembleShipChooseDialog(new List<string>() { currentSelectTab }, currentSelectTab);
        }
     
        bool refreshProperty(bool Init)
        {
            if (Init)
            {
                _propertyDurabilityText.text = _info.presetData._metaData.HPBase.ToString();
                _propertySpeedText.text = _info.presetData._metaData.SpeedBase.ToString();
                _propertyFirePowerText.text =_info.presetData._metaData.FirePowerBase.ToString();
                _propertyExploreText.text = _info.presetData._metaData.DetectBase.ToString();
                _propertyMemberText.text = _info.presetData._metaData.CrewMax.ToString();
                _propertyStorageText.text = _info.presetData._metaData.StorageBase.ToString();
            }
            else
            {
                _propertyDurabilityText.text = _info.shipDurability.ToString();
                _propertySpeedText.text = _info.shipSpeed.ToString();
                _propertyFirePowerText.text = _info.shipFirePower.ToString();
                _propertyExploreText.text = _info.shipDetect.ToString();
                _propertyMemberText.text = _info.shipCrewMax.ToString();
                _propertyStorageText.text = _info.shipStorage.ToString();
            } 
            return true;
        }

        void InitShipPartItem()
        {
            _partItemList.Clear();
            if (_info == null)
                return;

            customContentTrans.ReleaseAllChildObj();

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
                    AssembleShipCustomPartItem item = obj.transform.SafeGetComponent<AssembleShipCustomPartItem>();
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

        void RefreshShipBaseCost()
        {
            var costList = _info.presetData.shipCostBase;
            shipBaseCostTrans.InitObj(UIPath.PrefabPath.MaterialCost_Item, costList.Count, Config.GlobalConfigData.Assemble_MaterialCost_MaxNum);

            for(int i = 0; i < shipBaseCostTrans.childCount; i++)
            {
                var cmpt = shipBaseCostTrans.GetChild(i).SafeGetComponent<MaterialCostCmpt>();
                if (cmpt != null)
                {
                    cmpt.SetUpItem(costList[i]);
                    cmpt.name = "MaterialCostItem_" + i;
                }
            }
        }

        bool OnPartEquip(ushort partUID,int configID)
        {
            var partInfo = PlayerManager.Instance.GetAssemblePartInfo(partUID);
            if (partInfo == null)
                return false;
            for(int i = 0; i < _partItemList.Count; i++)
            {
                if (_partItemList[i]._configData.configID == configID)
                {
                    _partItemList[i].AddShipPart(partInfo);
                    _info.customData = GenerateShipCustomData();

                    refreshProperty(false);
                }
            }
            
            return true;
        }


        AssembleShipCustomData GenerateShipCustomData()
        {
            Dictionary<int, AssemblePartInfo> partInfoDic = new Dictionary<int, AssemblePartInfo>();

            for(int i = 0; i < _partItemList.Count; i++)
            {
                if (_partItemList[i].partInfo != null && !partInfoDic.ContainsKey(_partItemList[i]._configData.configID))
                {
                    var partInfo = PlayerManager.Instance.GetAssemblePartInfo(_partItemList[i].partInfo.UID);
                    partInfoDic.Add(_partItemList[i]._configData.configID, partInfo);
                }
            }

            AssembleShipCustomData data = new AssembleShipCustomData(
                _info.warShipID,
                customNameInputField.text,
                partInfoDic);
            return data;
        }

        void OnShipDesignBtnClick()
        {
            AudioManager.Instance.PlaySound(AudioClipPath.UISound.Button_Click);

            if (PlayerManager.Instance.CheckAssembleShipCustomNameRepeat(_info.presetData.shipClassName, customNameInputField.text))
            {
                GeneralConfirmDialogItem item = new GeneralConfirmDialogItem(
                   MultiLanguage.Instance.GetTextValue(Assemble_Design_Ship_CustomName_Repeat_Title),
                   MultiLanguage.Instance.GetTextValue(Assemble_Design_Ship_CustomName_Repeat_Content),
                   2,
                   () => { ConfirmSaveShipDesign(true); },
                   MultiLanguage.Instance.GetTextValue(Assemble_Design_Ship_CustomName_Repeat_Cover),
                   () =>
                   {
                       UIManager.Instance.HideWnd(UIPath.WindowPath.General_Confirm_Dialog);
                   },
                   MultiLanguage.Instance.GetTextValue(Assemble_Design_Ship_CustomName_Repeat_Cancel)
                   );
                UIGuide.Instance.ShowGeneralConfirmDialog(item);
            }
            else
            {
                ConfirmSaveShipDesign(false);
            }
        }

        void ConfirmSaveShipDesign(bool isCover)
        {
            _info.customData = GenerateShipCustomData();
            PlayerManager.Instance.AddAssembleShipDesign(_info);

            if (isCover)
            {
                UIGuide.Instance.ShowGeneralHint(new GeneralHintDialogItem(
                   MultiLanguage.Instance.GetTextValue(Assemble_Ship_Design_Save_Cover_Success_Hint), 1.5f));
                UIManager.Instance.HideWnd(UIPath.WindowPath.General_Confirm_Dialog);
            }
            else
            {
                UIGuide.Instance.ShowGeneralHint(new GeneralHintDialogItem(
                 MultiLanguage.Instance.GetTextValue(Assemble_Ship_Design_Save_Success_Hint), 1.5f));
            }
        }


        #region Choose Content

        void SetUpShipChooseContent()
        {
            contentCanvasGroup.ActiveCanvasGroup(false);
            shipChooseCanvasGroup.ActiveCanvasGroup(true);
            presetChooseBtn.transform.SafeSetActive(false);

            RefreshShipChooseTab();
            InitDefaultTabSelect();
            presetTotalBtn.onClick.RemoveAllListeners();
            AddButtonClickListener(presetTotalBtn, OnPresetTotalBtnClickAll);
        }

        void OnPresetTotalBtnClickAll()
        {
            AudioManager.Instance.PlaySound(AudioClipPath.UISound.Button_Click);
            var typeList = PlayerManager.Instance.GetTotalUnlockAssemblePartTypeList();
            UIGuide.Instance.ShowAssemblePartChooseDialog(typeList, currentSelectTab,1);
        }

        void RefreshShipChooseTab()
        {
            var unlockList = PlayerManager.Instance.GetTotalUnlockAssembleShipTypeData();
            chooseTabTrans.InitObj(UIPath.PrefabPath.General_ChooseTab, unlockList.Count);

            for (int i = 0; i < chooseTabTrans.childCount; i++)
            {
                var cmpt = chooseTabTrans.GetChild(i).SafeGetComponent<GeneralChooseTab>();
                cmpt.SetUpTab(unlockList[i]);
                cmpt.name = "ShipTab_" + i;
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
                noDataInfoTrans.SafeSetActive(true);
                chooseLoopList.transform.SafeSetActive(false);
                if (noDataInfoAnim != null)
                    noDataInfoAnim.Play();
            }
            else
            {
                chooseLoopList.transform.SafeSetActive(true);
                noDataInfoTrans.SafeSetActive(false);
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
        private Transform shipBaseCostTrans;
        private Animation shipContentAnim;

        private Button shipDesignSaveBtn;
        private Button presetChooseBtn;
        private Button presetTotalBtn;

        private InputField customNameInputField;
        private TypeWriterEffect shipClassDescTypeEffect;

        private Transform customContentTrans;
        private Transform chooseTabTrans;
        private Transform noDataInfoTrans;
        private Animation noDataInfoAnim;
        private LoopList chooseLoopList;
        private Animation shipChooseAnim;
        private CanvasGroup shipChooseCanvasGroup;

        private const string Assemble_Ship_Design_Save_Success_Hint = "Assemble_Ship_Design_Save_Success_Hint";
        private const string Assemble_Ship_Design_Save_Cover_Success_Hint = "Assemble_Ship_Design_Save_Cover_Success_Hint";

        private const string Assemble_Design_Ship_CustomName_Repeat_Title = "Assemble_Design_Ship_CustomName_Repeat_Title";
        private const string Assemble_Design_Ship_CustomName_Repeat_Content = "Assemble_Design_Ship_CustomName_Repeat_Content";
        private const string Assemble_Design_Ship_CustomName_Repeat_Cover = "Assemble_Design_Ship_CustomName_Repeat_Cover";
        private const string Assemble_Design_Ship_CustomName_Repeat_Cancel = "Assemble_Design_Ship_CustomName_Repeat_Cancel";


        protected override void InitUIRefrence()
        {

            _propertyDurabilityText = Transform.FindTransfrom("Content/RightPanel/PartProperty/Content/Durability/Value").SafeGetComponent<Text>();
            _propertySpeedText = Transform.FindTransfrom("Content/RightPanel/PartProperty/Content/Speed/Value").SafeGetComponent<Text>();
            _propertyFirePowerText= Transform.FindTransfrom("Content/RightPanel/PartProperty/Content/FirePower/Value").SafeGetComponent<Text>();
            _propertyExploreText= Transform.FindTransfrom("Content/RightPanel/PartProperty/Content/Explore/Value").SafeGetComponent<Text>();
            _propertyMemberText= Transform.FindTransfrom("Content/RightPanel/PartProperty/Content/Member/Value").SafeGetComponent<Text>();
            _propertyStorageText= Transform.FindTransfrom("Content/RightPanel/PartProperty/Content/Storage/Value").SafeGetComponent<Text>();

            _shipTypeIcon = Transform.FindTransfrom("Content/LeftPanel/ShipInfo/Content/Type/Icon").SafeGetComponent<Image>();
            _shipTypeText = Transform.FindTransfrom("Content/LeftPanel/ShipInfo/Content/Type/Name").SafeGetComponent<Text>();
            _shipClassText = Transform.FindTransfrom("Content/LeftPanel/ShipInfo/Content/Class/Name").SafeGetComponent<Text>();
            _shipSizeText= Transform.FindTransfrom("Content/LeftPanel/ShipInfo/Content/Size/Name").SafeGetComponent<Text>();
            _shipClassDesc = Transform.FindTransfrom("Content/LeftPanel/ShipDesc/Text").SafeGetComponent<Text>();
            shipClassDescTypeEffect = _shipClassDesc.transform.SafeGetComponent<TypeWriterEffect>();
            contentCanvasGroup = Transform.FindTransfrom("Content").SafeGetComponent<CanvasGroup>();
            shipBaseCostTrans = Transform.FindTransfrom("Content/LeftPanel/CostBase/Content");
            shipContentAnim = Transform.FindTransfrom("Content").SafeGetComponent<Animation>();

            shipDesignSaveBtn = Transform.FindTransfrom("Content/RightPanel/Btn").SafeGetComponent<Button>();
            presetChooseBtn = Transform.FindTransfrom("BtnPanel/PresetChooseBtn").SafeGetComponent<Button>();
            presetTotalBtn = Transform.FindTransfrom("BtnPanel/TotalPresetBtn").SafeGetComponent<Button>();

            customNameInputField = Transform.FindTransfrom("Content/NameCustom/NameFix/InputField").SafeGetComponent<InputField>();
            customContentTrans = Transform.FindTransfrom("Content/CustomValueContent/Content");
            chooseTabTrans = Transform.FindTransfrom("ShipChooseContent/ChooseTab");
            noDataInfoTrans = Transform.FindTransfrom("ShipChooseContent/EmptyInfo");
            noDataInfoAnim = noDataInfoTrans.SafeGetComponent<Animation>();
            chooseLoopList = Transform.FindTransfrom("ShipChooseContent/ChooseContent/Scroll View").SafeGetComponent<LoopList>();
            shipChooseAnim = Transform.FindTransfrom("ShipChooseContent").SafeGetComponent<Animation>();
            shipChooseCanvasGroup = Transform.FindTransfrom("ShipChooseContent").SafeGetComponent<CanvasGroup>();

        }


    }
}