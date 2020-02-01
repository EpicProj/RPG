using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sim_FrameWork.UI
{
    public partial class AssemblePartDesignPageContext : WindowBase
    {
        private AssemblePartInfo _info;
        private List<AssemblePartPropertyItem> _propertyItem;
        private List<AssemblePartCustomItem> _customItem;

        public Dictionary<string, AssemblePartTimeCostDetialInfo> _timeCostInfoDic;

        bool showPartChoosePanel = true;
        private string currentSelectTab = "";
        

        #region Override Method
        public override void Awake(params object[] paralist)
        {
            base.Awake(paralist);
            _info = (AssemblePartInfo)paralist[0];
            _propertyItem = new List<AssemblePartPropertyItem>();
            _customItem = new List<AssemblePartCustomItem>();
            _timeCostInfoDic = new Dictionary<string, AssemblePartTimeCostDetialInfo>();
            AddButtomClick();
        }

        public override void OnShow(params object[] paralist)
        {
            _info = (AssemblePartInfo)paralist[0];
            AudioManager.Instance.PlaySound(AudioClipPath.UISound.Page_Open);

            noDataTrans.gameObject.SetActive(false);
            if (showPartChoosePanel)
            {
                SetUpPartChooseContent();
            }
            else
            {
                SetUpContent();
            }

        }

        public override bool OnMessage(UIMessage msg)
        {
            if (msg.type == UIMsgType.Assemble_Part_PropertyChange)
            {
                Config.PartsCustomConfig.ConfigData configData = (Config.PartsCustomConfig.ConfigData)msg.content[0];
                float currentValue = (float)msg.content[1];
                return CalculateValue(configData, currentValue);
            }
            else if (msg.type == UIMsgType.Assemble_PartTab_Select)
            {
                string typeName = (string)msg.content[0];
                return RefreshChooseContent(typeName);
            }
            else if (msg.type == UIMsgType.Assemble_PartPreset_Select)
            {
                AssemblePartInfo info = new AssemblePartInfo();
                if (info.InitData((int)msg.content[0]))
                {
                    _info = info;
                    SetUpContent();
                }
            }
            return true;
        }

        public override void OnDisable()
        {
            AudioManager.Instance.PlaySound(AudioClipPath.UISound.Btn_Close);
            MapManager.Instance.ReleaseAssembleModel();
        }


        #endregion

        private void AddButtomClick()
        {
            AddButtonClickListener(Transform.FindTransfrom("Content/RightPanel/Btn").SafeGetComponent<Button>(), OnSaveDesignBtnClick);
            AddButtonClickListener(Transform.FindTransfrom("Back").SafeGetComponent<Button>(), () =>
            {
                AudioManager.Instance.PlaySound(AudioClipPath.UISound.Btn_Close);
                UIManager.Instance.HideWnd(this);
                UIGuide.Instance.ShowGameMainPage();
            });

            AddButtonClickListener(Transform.FindTransfrom("BtnPanel/PresetChooseBtn").SafeGetComponent<Button>(), OnPresetBtnClick);
        }

        void OnPresetBtnClick()
        {
            AudioManager.Instance.PlaySound(AudioClipPath.UISound.Button_Click);
            partChooseCanvasGroup.ActiveCanvasGroup(true);
            contentCanvasGroup.ActiveCanvasGroup(false);

        }

   

        #region Content
        void SetUpContent()
        {
            if (_info == null)
                return;

            partChooseCanvasGroup.ActiveCanvasGroup(false);
            contentCanvasGroup.ActiveCanvasGroup(true);
            presetBtn.transform.SafeSetActive(true);
            
            partTypeImage.sprite = _info.typePresetData.TypeIcon;
            partTypeName.text = _info.typePresetData.TypeName;
            partModelType.text = _info.typePresetData.partName;

            partDescText.text = _info.typePresetData.partDesc;
            if (partDescTypeEffect != null)
                partDescTypeEffect.StartEffect();

            MapManager.Instance.ReleaseAssembleModel();
            MapManager.Instance.InitAssembleModel(_info.typePresetData.ModelPath);

            InitPartPropertyContent();
            InitPartCustomContent();
            InitAssembleTargetItem();
            InitPartCostPanel();

            AddButtonClickListener(presetTotalBtn, OnPresetTotalBtnClick);

            if (partContentAnim != null)
                partContentAnim.Play();
        }

        void OnPresetTotalBtnClick()
        {
            AudioManager.Instance.PlaySound(AudioClipPath.UISound.Button_Click);
            UIGuide.Instance.ShowAssemblePartChooseDialog(new List<string>() { currentSelectTab }, currentSelectTab,1);
        }

        void InitPartPropertyContent()
        {
            var partPropertyContentTrans = Transform.FindTransfrom("Content/RightPanel/PartProperty/Content");
            _propertyItem.Clear();
            if (partPropertyContentTrans.childCount != Config.GlobalConfigData.AssemblePart_Max_PropertyNum)
            {
                for(int i = 0; i < Config.GlobalConfigData.AssemblePart_Max_PropertyNum; i++)
                {
                    var obj = ObjectManager.Instance.InstantiateObject(UIPath.PrefabPath.Assemble_Part_PropertyItem);
                    if (obj != null)
                    {
                        obj.transform.SetParent(partPropertyContentTrans, false);
                        obj.name = "Property_" + i;
                    }
                }
            }

            partPropertyContentTrans.SafeSetActiveAllChild(false);

            for (int i = 0; i < _info.typePresetData.partsPropertyConfig.configData.Count; i++)
            {
                if (i > Config.GlobalConfigData.AssemblePart_Max_PropertyNum)
                    return;
                var trans = partPropertyContentTrans.GetChild(i);
                trans.SafeSetActive(true);
                var itemCmpt = trans.SafeGetComponent<AssemblePartPropertyItem>();
                if (itemCmpt != null)
                {
                    itemCmpt.SetUpItem(_info.typePresetData.partsPropertyConfig.configData[i]);
                    _propertyItem.Add(itemCmpt);
                }
            }
        }

        void InitPartCustomContent()
        {
            var customValueContentTrans = Transform.FindTransfrom("Content/CustomValueContent/Content");
            _customItem.Clear();

            customValueContentTrans.ReleaseAllChildObj();

            for (int i = 0; i < _info.partsConfig.configData.Count; i++)
            {
                GameObject obj = null;
                if (string.Compare(_info.partsConfig.configData[i].PosType, "Left") == 0)
                {
                    obj = ObjectManager.Instance.InstantiateObject(UIPath.PrefabPath.Assemble_Part_CustomItem_Left);
                }
                else if (string.Compare(_info.partsConfig.configData[i].PosType, "Right") == 0)
                {
                    obj = ObjectManager.Instance.InstantiateObject(UIPath.PrefabPath.Assemble_Part_CustomItem_Right);
                }

                if (obj != null)
                {
                    obj.transform.SetParent(customValueContentTrans, false);
                    obj.name = "CustomItem_" + i;
                    var itemCmpt = obj.transform.SafeGetComponent<AssemblePartCustomItem>();
                    if (itemCmpt != null)
                    {
                        var configData = _info.partsConfig.configData[i];
                        itemCmpt.SetUpItem(configData);
                        CalculateValue(configData,(float)configData.CustomDataDefaultValue);
                        _customItem.Add(itemCmpt);
                    }
                }
            }
        }

        bool CalculateValue(Config.PartsCustomConfig.ConfigData config, float Value)
        {
            if (config == null)
                return false;

            int diffValue = (int)System.Math.Round((Value - config.CustomDataRangeMin) * 10,0);

            if (config.TimeCostPerUnit != 0)
            {
                AssemblePartTimeCostDetialInfo info = new AssemblePartTimeCostDetialInfo
                {
                    customDataName = config.CustomDataName,
                    modifyTimeValue = diffValue * (float)config.TimeCostPerUnit
                };
                RefreshTimeCostValue(info);
            }

            for (int i = 0; i < config.propertyLinkData.Count; i++)
            {
                var propertyName = config.propertyLinkData[i].Name;
                foreach (var item in _propertyItem)
                {
                    if (item._configData.Name == propertyName)
                    {
                        if (item._configData.PropertyType == 1)
                        {
                            ///Fix Value
                            float currentValue =diffValue * (float)config.propertyLinkData[i].PropertyChangePerUnitValue;
                            AssemblePartPropertyDetailInfo detailInfo = new AssemblePartPropertyDetailInfo
                            {
                                customDataName = config.CustomDataName,
                                propertyLinkName = config.propertyLinkData[i].Name,
                                modifyType = config.propertyLinkData[i].PropertyChangeType,
                                modifyValueFix = currentValue
                            };

                            item.ChangeValue(detailInfo);
                        }
                        else if(item._configData.PropertyType == 2)
                        {
                            float currentValueMin = diffValue * (float)config.propertyLinkData[i].PropertyChangePerUnitMin;
                            float currentValueMax = diffValue * (float)config.propertyLinkData[i].PropertyChangePerUnitMax;
                            AssemblePartPropertyDetailInfo detailInfo = new AssemblePartPropertyDetailInfo
                            {
                                customDataName = config.CustomDataName,
                                propertyLinkName = config.propertyLinkData[i].Name,
                                modifyType = config.propertyLinkData[i].PropertyChangeType,
                                modifyValueMin = currentValueMin,
                                modifyValueMax=currentValueMax
                            };

                            item.ChangeValue(detailInfo);
                        }
                    }
                }
            }

            return true;
        }
        /// <summary>
        /// 刷新时长
        /// </summary>
        /// <param name="timeInfo"></param>
        void RefreshTimeCostValue(AssemblePartTimeCostDetialInfo timeInfo)
        {
            if (_timeCostInfoDic.ContainsKey(timeInfo.customDataName))
            {
                var info = _timeCostInfoDic[timeInfo.customDataName];
                info.modifyTimeValue = timeInfo.modifyTimeValue;
            }
            else
            {
                _timeCostInfoDic.Add(timeInfo.customDataName, timeInfo);
            }
            ///Text Refresh
            float addTime = 0;
            foreach(var info in _timeCostInfoDic.Values)
                addTime += info.modifyTimeValue;
            timeCostText.text = string.Format("{0:N1}", _info._partsMeta.BaseTimeCost + addTime);
        }

        void InitTimeCostValue()
        {
            for(int i = 0; i < _info.partsConfig.configData.Count; i++)
            {
                var data= _info.partsConfig.configData[i];
                if (data.TimeCostPerUnit != 0)
                {
                    int diffValue = (int)((data.CustomDataDefaultValue - data.CustomDataRangeMin) * 10);
                    AssemblePartTimeCostDetialInfo info = new AssemblePartTimeCostDetialInfo
                    {
                        customDataName = data.CustomDataName,
                        modifyTimeValue = diffValue * (float)data.TimeCostPerUnit
                    };
                    RefreshTimeCostValue(info);
                }
            }
        }

        void OnSaveDesignBtnClick()
        {
            AudioManager.Instance.PlaySound(AudioClipPath.UISound.Button_Click);

            ///Custom Name Repeat
            if (PlayerManager.Instance.CheckAssemblePartCustomNameRepeat(_info.typePresetData.partName,customNameInput.text))
            {
                GeneralConfirmDialogItem item = new GeneralConfirmDialogItem(
                    MultiLanguage.Instance.GetTextValue(Assemble_Design_Part_CustomName_Repeat_Title),
                    MultiLanguage.Instance.GetTextValue(Assemble_Design_Part_CustomName_Repeat_Content),
                    2,
                    ()=> { ConfirmSavePartDesignAction(true); },
                    MultiLanguage.Instance.GetTextValue(Assemble_Design_Part_CustomName_Repeat_Cover),
                    () =>
                    {
                        UIManager.Instance.HideWnd(UIPath.WindowPath.General_Confirm_Dialog);
                    },
                    MultiLanguage.Instance.GetTextValue(Assemble_Design_Part_CustomName_Repeat_Cancel)
                    );
                UIGuide.Instance.ShowGeneralConfirmDialog(item);
            }
            else
            {
                ConfirmSavePartDesignAction(false);
            }
        }

        void ConfirmSavePartDesignAction(bool isCover)
        {
            _info.customDataInfo = GenerateCustomDataInfo();
            PlayerManager.Instance.AddAssemblePartDesign(_info);
            if (isCover)
            {
                UIGuide.Instance.ShowGeneralHint(new GeneralHintDialogItem(
                    MultiLanguage.Instance.GetTextValue(Assemble_Design_Save_Cover_Success_Hint), 1.5f));
                UIManager.Instance.HideWnd(UIPath.WindowPath.General_Confirm_Dialog);
            }
            else
            {
                UIGuide.Instance.ShowGeneralHint(new GeneralHintDialogItem(
                   MultiLanguage.Instance.GetTextValue(Assemble_Design_Save_Success_Hint), 1.5f));
            }
          
        }

        AssemblePartCustomDataInfo GenerateCustomDataInfo()
        {
            Dictionary<string, AssemblePartCustomDataInfo.CustomData> dataDic = new Dictionary<string, AssemblePartCustomDataInfo.CustomData>();

            Dictionary<string, float> customValueDic = new Dictionary<string, float>();

            foreach(var item in _propertyItem)
            {
                string propertyName = item._configData.Name;
                AssemblePartCustomDataInfo.CustomData data = new AssemblePartCustomDataInfo.CustomData();
                data.InitData(item._configData,
                    item.CurrentValueMin,
                    item.CurrentValueMax,
                    item.detailInfoDic,
                    _timeCostInfoDic);
                ///Get TimeCost
                
                dataDic.Add(propertyName, data);
            }

            foreach(var item in _customItem)
            {
                string customName = item._config.CustomDataName;
                customValueDic.Add(customName, item.CurrentValue);
            }

            string inputText = customNameInput == null ? "" : customNameInput.text;
            AssemblePartCustomDataInfo info = new AssemblePartCustomDataInfo();
            info.CreateCustomInfo(_info.partID, inputText, dataDic, customValueDic);
            return info;
        }

        /// <summary>
        /// Assemble Target
        /// </summary>
        void InitAssembleTargetItem()
        {
            var assembleTargetContentTrans = Transform.FindTransfrom("Content/LeftPanel/AssembleTarget/Content");
            assembleTargetContentTrans.SafeSetActiveAllChild(false);

            for (int i = 0; i < _info.partEquipType.Count; i++)
            {
                if (i >= Config.GlobalConfigData.AssemblePart_Target_MaxNum)
                    break;
                var targetData = AssembleModule.GetAssembleMainTypeData(_info.partEquipType[i]);
                if (targetData != null)
                {
                    var item = assembleTargetContentTrans.GetChild(i);
                    item.FindTransfrom("Icon").SafeGetComponent<Image>().sprite = Utility.LoadSprite(targetData.IconPath, Utility.SpriteType.png);
                    item.FindTransfrom("Name").SafeGetComponent<Text>().text = MultiLanguage.Instance.GetTextValue(targetData.TypeNameText);
                    item.SafeSetActive(true);
                }
            }
        }

        void InitPartCostPanel()
        {
            InitTimeCostValue();

            var materialCostTrans = Transform.FindTransfrom("Content/LeftPanel/Cost/Content");

            if (materialCostTrans.childCount != Config.GlobalConfigData.Assemble_MaterialCost_MaxNum)
            {
                for(int i = 0; i < Config.GlobalConfigData.Assemble_MaterialCost_MaxNum; i++)
                {
                    var obj = ObjectManager.Instance.InstantiateObject(UIPath.PrefabPath.MaterialCost_Item);
                    if (obj != null)
                    {
                        obj.name = "MaterialCostItem_" + i;
                        obj.transform.SetParent(materialCostTrans, false);
                    }
                }
            }

            materialCostTrans.SafeSetActiveAllChild(false);

            for (int i = 0; i < _info.materialCostItem.Count; i++)
            {
                if (i >= Config.GlobalConfigData.Assemble_MaterialCost_MaxNum)
                    break;
                var cmpt = materialCostTrans.GetChild(i).SafeGetComponent<MaterialCostCmpt>();
                if (cmpt != null)
                {
                    cmpt.SetUpItem(_info.materialCostItem[i]);
                    cmpt.gameObject.SetActive(true);
                }
            }
        }
        #endregion

        #region Part Choose Content

        void SetUpPartChooseContent()
        {
            contentCanvasGroup.ActiveCanvasGroup(false);
            partChooseCanvasGroup.ActiveCanvasGroup(true);
            presetBtn.transform.SafeSetActive(false);

            RefreshPartChooseTab();
            InitDefaultTabSelect();
            AddButtonClickListener(presetTotalBtn, OnPresetTotalBtnClickAll);
        }

        void OnPresetTotalBtnClickAll()
        {
            AudioManager.Instance.PlaySound(AudioClipPath.UISound.Button_Click);
            var typeList = PlayerManager.Instance.GetTotalUnlockAssemblePartTypeList();
            UIGuide.Instance.ShowAssemblePartChooseDialog(typeList, currentSelectTab,1);
        }

        void RefreshPartChooseTab()
        {
            var tabChooseTrans = Transform.FindTransfrom("PartChooseContent/ChooseTab");
            var unlockList = PlayerManager.Instance.GetTotalUnlockAssembleTypeData();

            tabChooseTrans.InitObj(UIPath.PrefabPath.General_ChooseTab, unlockList.Count);

            for (int i = 0; i < tabChooseTrans.childCount; i++)
            {
                var cmpt = tabChooseTrans.GetChild(i).SafeGetComponent<GeneralChooseTab>();
                cmpt.SetUpTab(unlockList[i], true);
                cmpt.transform.name = "PartTab_" + i;
            }
        }

        void InitDefaultTabSelect()
        {
            string typeID = Config.ConfigData.AssembleConfig.assemblePartPage_DefaultSelectTab;
            if (AssembleModule.GetAssemblePartMainType(typeID) != null)
            {
                RefreshChooseContent(typeID);
            }
        }

        bool RefreshChooseContent(string chooseType)
        {
            var chooseLoopList = Transform.FindTransfrom("PartChooseContent/ChooseContent/Scroll View").SafeGetComponent<LoopList>();
            var partModelList = PlayerManager.Instance.GetAssemblePartPresetModelList(chooseType);
            if (partModelList.Count == 0)
            {
                noDataTrans.SafeSetActive(true);
                chooseLoopList.gameObject.SetActive(false);
                if (noDataInfoAnim != null)
                    noDataInfoAnim.Play();
            }
            else
            {
                chooseLoopList.transform.SafeSetActive(true); 
                noDataTrans.SafeSetActive(false);
                currentSelectTab = chooseType;
                chooseLoopList.InitData(partModelList);
                if (partChooseAnim != null)
                    partChooseAnim.Play();
            }
            return true;
        }

        #endregion

    }

    public partial class AssemblePartDesignPageContext : WindowBase
    {

        private CanvasGroup contentCanvasGroup;
        private Image partTypeImage;
        private Text partTypeName;
        private Text partModelType;

        private Text partDescText;
        private TypeWriterEffect partDescTypeEffect;
        private InputField customNameInput;

        private Animation partContentAnim;
        private Text timeCostText;

        private Button presetBtn;
        private Button presetTotalBtn;

        private CanvasGroup partChooseCanvasGroup;
        private Transform noDataTrans;
        private Animation noDataInfoAnim;
        private Animation partChooseAnim;

        private const string Assemble_Design_Save_Success_Hint = "Assemble_Design_Save_Success_Hint";
        private const string Assemble_Design_Save_Cover_Success_Hint = "Assemble_Design_Save_Cover_Success_Hint";
        /// 组件自定义名重复文本
        private const string Assemble_Design_Part_CustomName_Repeat_Title = "Assemble_Design_Part_CustomName_Repeat_Title";
        private const string Assemble_Design_Part_CustomName_Repeat_Content = "Assemble_Design_Part_CustomName_Repeat_Content";
        private const string Assemble_Design_Part_CustomName_Repeat_Cover = "Assemble_Design_Part_CustomName_Repeat_Cover";
        private const string Assemble_Design_Part_CustomName_Repeat_Cancel = "Assemble_Design_Part_CustomName_Repeat_Cancel";

        protected override void InitUIRefrence()
        {
            contentCanvasGroup = Transform.FindTransfrom("Content").SafeGetComponent<CanvasGroup>();

            partTypeImage = Transform.FindTransfrom("Content/LeftPanel/PartInfo/Content/Type/Icon").SafeGetComponent<Image>();
            partTypeName = Transform.FindTransfrom("Content/LeftPanel/PartInfo/Content/Type/Name").SafeGetComponent<Text>();
            partModelType= Transform.FindTransfrom("Content/LeftPanel/PartInfo/Content/ModelType/Name").SafeGetComponent<Text>();
            partDescText = Transform.FindTransfrom("Content/LeftPanel/PartDesc/Text").SafeGetComponent<Text>();
            partDescTypeEffect= Transform.FindTransfrom("Content/LeftPanel/PartDesc/Text").SafeGetComponent<TypeWriterEffect>();
            customNameInput = Transform.FindTransfrom("Content/NameCustom/NameFix/InputField").SafeGetComponent<InputField>();

            timeCostText = Transform.FindTransfrom("Content/LeftPanel/Cost/Time").SafeGetComponent<Text>();
            partContentAnim = Transform.FindTransfrom("Content").SafeGetComponent<Animation>();

            presetBtn = Transform.FindTransfrom("BtnPanel/PresetChooseBtn").SafeGetComponent<Button>();
            presetTotalBtn= Transform.FindTransfrom("BtnPanel/TotalPresetBtn").SafeGetComponent<Button>();
            partChooseCanvasGroup = Transform.FindTransfrom("PartChooseContent").SafeGetComponent<CanvasGroup>();

            noDataTrans = Transform.FindTransfrom("PartChooseContent/EmptyInfo");
            noDataInfoAnim = noDataTrans.SafeGetComponent<Animation>();
            partChooseAnim = Transform.FindTransfrom("PartChooseContent").SafeGetComponent<Animation>();
        }


    }
}