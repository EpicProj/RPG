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

        #region Override Method
        public override void Awake(params object[] paralist)
        {
            base.Awake(paralist);
            _info = (AssemblePartInfo)paralist[0];
            _propertyItem = new List<AssemblePartPropertyItem>();
            _customItem = new List<AssemblePartCustomItem>();
            AddbuttomClick();
        }

        public override void OnShow(params object[] paralist)
        {
            _info = (AssemblePartInfo)paralist[0];
            AudioManager.Instance.PlaySound(AudioClipPath.UISound.Page_Open);
            SetUpContent();
        }

        public override bool OnMessage(UIMessage msg)
        {
            if(msg.type== UIMsgType.Assemble_Part_PropertyChange)
            {
                PartsCustomConfig.ConfigData configData = (PartsCustomConfig.ConfigData)msg.content[0];
                float currentValue = (float)msg.content[1];
                return CalculateFinialProperty(configData,currentValue);
            }
            return true;
        }

        public override void OnDisable()
        {
            AudioManager.Instance.PlaySound(AudioClipPath.UISound.Btn_Close);
            MapManager.Instance.ReleaseAssembleModel();
        }


        #endregion

        private void AddbuttomClick()
        {
            AddButtonClickListener(SaveDesignBtn, OnSaveDesignBtnClick);
            AddButtonClickListener(m_page.backBtn, () =>
            {
                AudioManager.Instance.PlaySound(AudioClipPath.UISound.Btn_Close);
                UIManager.Instance.HideWnd(this);
            });
        }

        void SetUpContent()
        {
            if (_info == null)
                return;
            partTypeImage.sprite = _info.TypeIcon;
            partTypeName.text = _info.TypeName;
            partModelType.text = _info.partName;

            partDescText.text = _info.partDesc;
            if (partDescTypeEffect != null)
                partDescTypeEffect.StartEffect();

            MapManager.Instance.InitAssembleModel(_info.ModelPath);

            InitPartPropertyContent();
            InitPartCustomContent();
            InitAssembleTargetItem();
            InitPartCostPanel();
        }

        void InitPartPropertyContent()
        {
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

            foreach(Transform trans in partPropertyContentTrans)
            {
                UIUtility.SafeSetActive(trans, false);
            }

            for(int i = 0; i < _info.partsPropertyConfig.configData.Count; i++)
            {
                if (i > Config.GlobalConfigData.AssemblePart_Max_PropertyNum)
                    return;
                var trans = partPropertyContentTrans.GetChild(i);
                UIUtility.SafeSetActive(trans, true);
                var itemCmpt = UIUtility.SafeGetComponent<AssemblePartPropertyItem>(trans);
                if (itemCmpt != null)
                {
                    itemCmpt.SetUpItem(_info.partsPropertyConfig.configData[i]);
                    _propertyItem.Add(itemCmpt);
                }
            }
        }

        void InitPartCustomContent()
        {
            _customItem.Clear();

            foreach(Transform trans in customValueContentTrans)
            {
                ObjectManager.Instance.ReleaseObject(trans.gameObject, 0);
            }

            for(int i = 0; i < _info.partsConfig.configData.Count; i++)
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
                    var itemCmpt = UIUtility.SafeGetComponent<AssemblePartCustomItem>(obj.transform);
                    if (itemCmpt != null)
                    {
                        var configData = _info.partsConfig.configData[i];
                        itemCmpt.SetUpItem(configData);
                        CalculateDefaultValue(configData);
                        _customItem.Add(itemCmpt);
                    }
                }
            }
        }


        bool CalculateFinialProperty(PartsCustomConfig.ConfigData config,float currentValue)
        {
            if (config == null)
                return false;

            int delta = (int)(currentValue * 10);
            ushort realTime = (ushort)(_info.baseTimeCost + delta * config.TimeCostPerUnit);
            timeCostText.text = realTime.ToString();

            for (int i = 0; i < config.propertyLinkData.Count; i++)
            {
                var propertyName = config.propertyLinkData[i].Name;
                foreach(var item in _propertyItem)
                {
                    if(item._configData.Name == propertyName)
                    {
                        
                        float currentMin = delta * (float)config.propertyLinkData[i].PropertyChangePerUnitMin;
                        item.ChangeValueMin(currentMin+ (float)item._configData.PropertyRangeMin);

                        float currentMax =delta * (float)config.propertyLinkData[i].PropertyChangePerUnitMax;
                        item.ChangeValueMax(currentMax+(float)item._configData.PropertyRangeMax);
                    }
                }
            }
            return true;
        }

        void CalculateDefaultValue(PartsCustomConfig.ConfigData config)
        {
            if (config == null)
                return;

            for (int i = 0; i < config.propertyLinkData.Count; i++)
            {
                var propertyName = config.propertyLinkData[i].Name;
                foreach (var item in _propertyItem)
                {
                    int delta = (int)(config.CustomDataDefaultValue * 10);
                    if (item._configData.Name == propertyName)
                    {
                        float currentMin = delta * (float)config.propertyLinkData[i].PropertyChangePerUnitMin;
                        item.ChangeValueMin(currentMin + (float)item._configData.PropertyRangeMin);

                        float currentMax = delta * (float)config.propertyLinkData[i].PropertyChangePerUnitMax;
                        item.ChangeValueMax(currentMax + (float)item._configData.PropertyRangeMax);
                    }
                }
            }
        }


        void OnSaveDesignBtnClick()
        {
            AudioManager.Instance.PlaySound(AudioClipPath.UISound.Button_Click);
            _info.customDataInfo = GenerateCustomDataInfo();
            PlayerManager.Instance.AddAssemblePartDesign(_info);

            UIGuide.Instance.ShowGeneralHint(new GeneralHintDialogItem(
                MultiLanguage.Instance.GetTextValue(Assemble_Design_Save_Success_Hint), 1.5f));
        }

        AssemblePartCustomDataInfo GenerateCustomDataInfo()
        {
            Dictionary<string, AssemblePartCustomDataInfo.CustomData> dataDic = new Dictionary<string, AssemblePartCustomDataInfo.CustomData>();

            Dictionary<string, float> customValueDic = new Dictionary<string, float>();

            foreach(var item in _propertyItem)
            {
                string propertyName = item._configData.Name;
                AssemblePartCustomDataInfo.CustomData data = new AssemblePartCustomDataInfo.CustomData(
                    item._configData,
                    item.CurrentValueMin,
                    item.CurrentValueMax);
                ///Get TimeCost
                

                dataDic.Add(propertyName, data);
            }

            foreach(var item in _customItem)
            {
                string customName = item._config.CustomDataName;
                customValueDic.Add(customName, item.CurrentValue);
            }

            return new AssemblePartCustomDataInfo(_info.partID, customNameInput.text, dataDic, customValueDic);
        }

        /// <summary>
        /// Assemble Target
        /// </summary>
        void InitAssembleTargetItem()
        {
            foreach(Transform trans in assembleTargetContentTrans)
            {
                trans.gameObject.SetActive(false);
            }

            for(int i = 0; i < _info.partEquipType.Count; i++)
            {
                if (i >= Config.GlobalConfigData.AssemblePart_Target_MaxNum)
                    break;
                var targetData = AssembleModule.GetAssembleMainTypeData(_info.partEquipType[i]);
                if (targetData != null)
                {
                    var item = assembleTargetContentTrans.GetChild(i);
                    UIUtility.SafeGetComponent<Image>(UIUtility.FindTransfrom(item, "Icon")).sprite = Utility.LoadSprite(targetData.IconPath, Utility.SpriteType.png);
                    UIUtility.SafeGetComponent<Text>(UIUtility.FindTransfrom(item, "Name")).text = MultiLanguage.Instance.GetTextValue(targetData.TypeNameText);
                    item.gameObject.SetActive(true);
                }
            }
        }

        void InitPartCostPanel()
        {
            timeCostText.text = _info.baseTimeCost.ToString();

            if (materialCostTrans.childCount != Config.GlobalConfigData.AssemblePart_MaterialCost_MaxNum)
            {
                for(int i = 0; i < Config.GlobalConfigData.AssemblePart_MaterialCost_MaxNum; i++)
                {
                    var obj = ObjectManager.Instance.InstantiateObject(UIPath.PrefabPath.MaterialCost_Item);
                    if (obj != null)
                    {
                        obj.name = "MaterialCostItem_" + i;
                        obj.transform.SetParent(materialCostTrans, false);
                    }
                }
            }

            foreach(Transform trans in materialCostTrans)
            {
                trans.gameObject.SetActive(false);
            }

            for(int i = 0; i < _info.materialCostItem.Count; i++)
            {
                if (i >= Config.GlobalConfigData.AssemblePart_MaterialCost_MaxNum)
                    break;
                var cmpt = UIUtility.SafeGetComponent<MaterialCostCmpt>(materialCostTrans.GetChild(i));
                if (cmpt != null)
                {
                    cmpt.SetUpItem(_info.materialCostItem[i]);
                    cmpt.gameObject.SetActive(true);
                }
            }

        }

    }

    public partial class AssemblePartDesignPageContext : WindowBase
    {
        private AssemblePartDesignPage m_page;

        private Image partTypeImage;
        private Text partTypeName;
        private Text partModelType;

        private Text partDescText;
        private TypeWriterEffect partDescTypeEffect;
        private InputField customNameInput;

        private Transform partPropertyContentTrans;
        private Transform customValueContentTrans;
        private Transform assembleTargetContentTrans;

        private Text timeCostText;
        private Transform materialCostTrans;

        private Button SaveDesignBtn;

        private const string Assemble_Design_Save_Success_Hint = "Assemble_Design_Save_Success_Hint";

        protected override void InitUIRefrence()
        {
            m_page = UIUtility.SafeGetComponent<AssemblePartDesignPage>(Transform);
            partTypeImage = UIUtility.SafeGetComponent<Image>(UIUtility.FindTransfrom(m_page.leftPanel, "PartInfo/Content/Type/Icon"));
            partTypeName = UIUtility.SafeGetComponent<Text>(UIUtility.FindTransfrom(m_page.leftPanel, "PartInfo/Content/Type/Name"));
            partModelType= UIUtility.SafeGetComponent<Text>(UIUtility.FindTransfrom(m_page.leftPanel, "PartInfo/Content/ModelType/Name"));
            partDescText = UIUtility.SafeGetComponent<Text>(UIUtility.FindTransfrom(m_page.leftPanel, "PartDesc/Text"));
            partDescTypeEffect= UIUtility.SafeGetComponent<TypeWriterEffect>(UIUtility.FindTransfrom(m_page.leftPanel, "PartDesc/Text"));
            customNameInput = UIUtility.SafeGetComponent<InputField>(UIUtility.FindTransfrom(m_page.namecustomPanel, "NameFix/InputField"));

            partPropertyContentTrans = UIUtility.FindTransfrom(m_page.rightPanel, "PartProperty/Content");
            customValueContentTrans = UIUtility.FindTransfrom(m_page.customPanel, "Content");
            assembleTargetContentTrans = UIUtility.FindTransfrom(m_page.leftPanel, "AssembleTarget/Content");
            timeCostText = UIUtility.SafeGetComponent<Text>(UIUtility.FindTransfrom(m_page.leftPanel, "Cost/Time"));
            materialCostTrans = UIUtility.FindTransfrom(m_page.leftPanel, "Cost/Content");

            SaveDesignBtn = UIUtility.SafeGetComponent<Button>(UIUtility.FindTransfrom(m_page.rightPanel, "Btn"));
        }


    }
}