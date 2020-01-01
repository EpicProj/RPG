using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sim_FrameWork.UI
{
    public partial class AssemblePartDesignPageContext : WindowBase
    {
        private AssemblePartInfo _info;

        #region Override Method
        public override void Awake(params object[] paralist)
        {
            base.Awake(paralist);
            _info = (AssemblePartInfo)paralist[0];
        }

        public override void OnShow(params object[] paralist)
        {
            _info = (AssemblePartInfo)paralist[0];
            SetUpContent();
        }

        public override bool OnMessage(UIMessage msg)
        {
            return base.OnMessage(msg);
        }

        #endregion

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

            MapManager.Instance.InitAssemblePartsModel(_info);

            InitPartPropertyContent();
            InitPartCustomContent();
        }

        void InitPartPropertyContent()
        {
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
                    var configData = _info.partsPropertyConfig.configData[i];
                    itemCmpt.SetUpItem(
                        configData.Name,
                        Utility.LoadSprite(configData.PropertyIcon, Utility.SpriteType.png),
                        MultiLanguage.Instance.GetTextValue(configData.PropertyName),
                        (float)configData.PropertyRangeMin,
                        (float)configData.PropertyRangeMax);
                }
            }
        }

        void InitPartCustomContent()
        {
            if (customValueContentTrans.childCount != Config.GlobalConfigData.AssemblePart_Max_CustomNum)
            {
                for (int i = 0; i < Config.GlobalConfigData.AssemblePart_Max_CustomNum; i++)
                {
                    var obj = ObjectManager.Instance.InstantiateObject(UIPath.PrefabPath.Assemble_Part_CustomItem);
                    if (obj != null)
                    {
                        obj.transform.SetParent(customValueContentTrans, false);
                        obj.name = "CustomItem_" + i;
                    }
                }
            }

            foreach(Transform trans in customValueContentTrans)
            {
                UIUtility.SafeSetActive(trans, false);
            }

            for(int i = 0; i < _info.partsConfig.configData.Count; i++)
            {
                if (i > Config.GlobalConfigData.AssemblePart_Max_CustomNum)
                    return;
                var trans = customValueContentTrans.GetChild(i);
                UIUtility.SafeSetActive(trans, true);
                var itemCmpt = UIUtility.SafeGetComponent<AssemblePartCustomItem>(trans);
                if (itemCmpt != null)
                {
                    var configData = _info.partsConfig.configData[i];
                    itemCmpt.SetUpItem(configData);
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

        private Transform partPropertyContentTrans;
        private Transform customValueContentTrans;

        protected override void InitUIRefrence()
        {
            m_page = UIUtility.SafeGetComponent<AssemblePartDesignPage>(Transform);
            partTypeImage = UIUtility.SafeGetComponent<Image>(UIUtility.FindTransfrom(m_page.leftPanel, "PartInfo/Content/Type/Icon"));
            partTypeName = UIUtility.SafeGetComponent<Text>(UIUtility.FindTransfrom(m_page.leftPanel, "PartInfo/Content/Type/Name"));
            partModelType= UIUtility.SafeGetComponent<Text>(UIUtility.FindTransfrom(m_page.leftPanel, "PartInfo/Content/ModelType/Name"));
            partDescText = UIUtility.SafeGetComponent<Text>(UIUtility.FindTransfrom(m_page.leftPanel, "PartDesc/Text"));
            partDescTypeEffect= UIUtility.SafeGetComponent<TypeWriterEffect>(UIUtility.FindTransfrom(m_page.leftPanel, "PartDesc/Text"));
            partPropertyContentTrans = UIUtility.FindTransfrom(m_page.rightPanel, "PartProperty/Content");
            customValueContentTrans = UIUtility.FindTransfrom(m_page.customPanel, "Content");
        }


    }
}