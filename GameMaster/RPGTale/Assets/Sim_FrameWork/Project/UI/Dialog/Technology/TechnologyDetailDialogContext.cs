using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sim_FrameWork.UI
{
    public class TechnologyDetailDialogContext : WindowBase
    {
        private Text _techNameText;
        private Image _techIcon;
        private Text _techCost;
        private Text _techTimeCost;
        private Text _techDesc;
        private Text _confirmBtnText;
        private Image _rarityImage;

        private TypeWriterEffect descTypewriterEffect;

        private const string ResearchStart_Hint_Text = "ResearchStart_Hint_Text";
        private const string Research_Require_Lack_Hint_Text = "Research_Require_Lack_Hint_Text";
        private const string Research_ConfirmBtn_Research_Text = "Research_ConfirmBtn_Research_Text";
        private const string Research_ConfirmBtn_Stop_Text = "Research_ConfirmBtn_Stop_Text";
        private const string Research_ConfirmBtn_Locked_Text = "Research_ConfirmBtn_Locked_Text";
        private const string Research_ConfirmBtn_Done_Text = "Research_ConfirmBtn_Done_Text";
        private const string Research_ConfirmBtn_Researching_Text = "Research_ConfirmBtn_Researching_Text";

        /// <summary>
        /// UnLock Text
        /// </summary>
        private const string Research_Effect_Unlock_Text_Block = "Research_Effect_Unlock_Text_Block";
        private const string Research_Effect_Unlock_Text_Tech = "Research_Effect_Unlock_Text_Tech";

        private TechnologyInfo techInfo;

        #region Override Method

        public override void Awake(params object[] paralist)
        {
            techInfo = (TechnologyInfo)paralist[0];
            InitRef();
            AddBtnClick();
        }

        public override bool OnMessage(UIMessage msg)
        {
            return base.OnMessage(msg);
        }

        public override void OnShow(params object[] paralist)
        {
            techInfo = (TechnologyInfo)paralist[0];
            AudioManager.Instance.PlaySound(AudioClipPath.UISound.Page_Open);
            SetUpDialog();
            InitConfirmBtnState();
            SetUpTechEffect();
            SetUpTechRequire();
        }

        public override void OnClose()
        {
        
        }
        public override void OnDisable()
        {
            AudioManager.Instance.PlaySound(AudioClipPath.UISound.Btn_Close);

        }

        private void InitRef()
        {
            _techNameText = Transform.FindTransfrom("Content/Context/Name").SafeGetComponent<Text>();
            _techIcon = Transform.FindTransfrom("Content/Context/Slot/Icon").SafeGetComponent<Image>();
            _techCost = Transform.FindTransfrom("Content/Context/Detail/Cost/Value").SafeGetComponent<Text>();
            _techTimeCost = Transform.FindTransfrom("Content/Context/Detail/Time/Value").SafeGetComponent<Text>();
            _techDesc = Transform.FindTransfrom("Desc").SafeGetComponent<Text>();
            _confirmBtnText = Transform.FindTransfrom("Content/ButtonGeneral/Text").SafeGetComponent<Text>();
            descTypewriterEffect = Transform.FindTransfrom("Content/Context/Desc").SafeGetComponent<TypeWriterEffect>();
            _rarityImage = Transform.FindTransfrom("Content/Context/Slot/Rarity").SafeGetComponent<Image>();
        }

        #endregion

        #region Method

        private void AddBtnClick()
        {
            AddButtonClickListener(Transform.FindTransfrom("BG").SafeGetComponent<Button>(), () =>
            {
                UIManager.Instance.HideWnd(this);
            });
            AddButtonClickListener(Transform.FindTransfrom("Content/ButtonGeneral").SafeGetComponent<Button>(), () =>
            {
                OnConfirmBtnClick();
            });
        }


        private bool SetUpDialog()
        {
            if (techInfo == null)
                return false;
            _techNameText.text = techInfo._model.Name;
            _techNameText.color = techInfo._model.Rarity.color;
            _techIcon.sprite = techInfo._model.Icon;
            _techDesc.text = techInfo._model.Desc;
            _techCost.text = techInfo._model.TechCost.ToString();

            _rarityImage.color = new Color(techInfo._model.Rarity.color.r, techInfo._model.Rarity.color.g, techInfo._model.Rarity.color.b, 0.3f);

            if (descTypewriterEffect != null)
                descTypewriterEffect.StartEffect();
            return true;
        }

        /// <summary>
        /// 初始化按钮状态
        /// </summary>
        private void InitConfirmBtnState()
        {
            if (techInfo == null)
                return;
            var btn = Transform.FindTransfrom("Content/ButtonGeneral").SafeGetComponent<Button>();
            _confirmBtnText.text = "";
            if (techInfo.currentState== TechnologyState.Lock)
            {
                _confirmBtnText.text = MultiLanguage.Instance.GetTextValue(Research_ConfirmBtn_Locked_Text);
                btn.interactable = false;
            }else if (techInfo.currentState== TechnologyState.Unlock)
            {
                _confirmBtnText.text = MultiLanguage.Instance.GetTextValue(Research_ConfirmBtn_Research_Text);
                btn.interactable = true;
            }
            else if (techInfo.currentState== TechnologyState.Done)
            {
                _confirmBtnText.text = MultiLanguage.Instance.GetTextValue(Research_ConfirmBtn_Done_Text);
                btn.interactable = false;
            }else if (techInfo.currentState == TechnologyState.OnResearch)
            {
                _confirmBtnText.text = MultiLanguage.Instance.GetTextValue(Research_ConfirmBtn_Researching_Text);
                btn.interactable = false;
            }
        }

        /// <summary>
        /// 初始化科技完成效果
        /// </summary>
        private void SetUpTechEffect()
        {
            if (techInfo == null)
                return;
            ///Init
            var content = Transform.FindTransfrom("Content/Context/EffectContent/Content");
            content.InitObj(UIPath.PrefabPath.Tech_Effect_Element, Config.GlobalConfigData.TechDetail_Dialog_MaxEffect_Count);
            content.SafeSetActiveAllChild(false);

            var effectlist = techInfo.techFinishEffectList;

            int totalIndex = 0;

            for(int i = 0; i < effectlist.Count; i++)
            {
                var type = TechnologyModule.Instance.GetTechCompleteType(effectlist[i]);

                switch (type)
                {
                    case TechCompleteEffect.Unlock_Block:
                        var blockList = TechnologyModule.ParseTechParam_Unlock_Block(effectlist[i].effectParam);
                        for (int j = 0; j < blockList.Count; j++)
                        {
                            FunctionBlockDataModel model = new FunctionBlockDataModel();
                            if (model.Create(blockList[j]))
                            {
                                var name = MultiLanguage.Instance.GetTextValue(Research_Effect_Unlock_Text_Block);
                                var element = content.GetChild(totalIndex);
                                if (element != null)
                                {
                                    totalIndex++;
                                    var cmpt = element.transform.SafeGetComponent<TechEffectElement>();
                                    cmpt.SetUpElement(model.Icon, name, model.Name, Color.white);
                                    element.SafeSetActive(true);
                                }
                            }
                        }
                        break;

                    case TechCompleteEffect.Unlock_Tech:
                        var techList = TechnologyModule.ParseTechParam_Unlock_Tech(effectlist[i].effectParam);
                        for (int j = 0; j < techList.Count; j++)
                        {
                            TechnologyDataModel model = new TechnologyDataModel();
                            if (model.Create(techList[j]))
                            {
                                var name = MultiLanguage.Instance.GetTextValue(Research_Effect_Unlock_Text_Tech);
                                var element = content.GetChild(totalIndex);
                                if (element != null)
                                {
                                    totalIndex++;
                                    var cmpt = element.transform.SafeGetComponent<TechEffectElement>();
                                    cmpt.SetUpElement(model.Icon, name, model.Name, model.Rarity.color);
                                    element.SafeSetActive(true);
                                }
                            }
                        }
                        break;
                }

            }
           
        }

        /// <summary>
        /// 初始化科技需求
        /// </summary>
        private void SetUpTechRequire()
        {
            if (techInfo == null)
                return;
            var content = Transform.FindTransfrom("Content/Context/RequireContent/Content/Scroll View/Viewport/Content");
            content.InitObj(UIPath.PrefabPath.Tech_Require_Element, Config.GlobalConfigData.TechDetail_Dialog_MaxRequire_Count);
            content.SafeSetActiveAllChild(false);

            var requireList = techInfo.techRequireList;

            int index = 0;

            //Init Cost
            //Init Cost
            var costObj = content.GetChild(index);
            if (costObj != null)
            {
                index++;
                var cmpt = costObj.SafeGetComponent<TechRequireElement>();
                cmpt.SetUpElement( TechRequireElement.RequireType.ResearchPoint, new object[] { techInfo._model.TechCost },false);
                costObj.SafeSetActive(true);
            }

            for(int i = 0; i < requireList.Count; i++)
            {
                var type = TechnologyModule.Instance.GetTechRequireType(requireList[i]);
                switch (type)
                {
                    case TechRequireType.PreTech:
                        ///Init PreTech 
                        var techList = TechnologyModule.ParseTechParam_Require_PreTech(requireList[i].Param);
                        for(int j = 0; j < techList.Count; j++)
                        {
                            TechnologyDataModel techModel = new TechnologyDataModel();
                            if (techModel.Create(techList[j]))
                            {
                                var obj = content.GetChild(index);
                                if (obj != null)
                                {
                                    index++;
                                    var element = obj.SafeGetComponent<TechRequireElement>();
                                    bool warning = TechnologyDataManager.Instance.GetTechInfo(techList[j]).currentState == TechnologyState.Lock ? true : false;
                                    element.SetUpElement( TechRequireElement.RequireType.PreTech,new object[] { techModel.ID }, warning);
                                    obj.SafeSetActive(true);
                                }
                            }
                        }
                        break;
                    case TechRequireType.Material:
                        var materialDic = TechnologyModule.parseTechParam_Require_Material(requireList[i].Param);
                        foreach(KeyValuePair<int,int> kvp in materialDic)
                        {
                            MaterialDataModel maModel = new MaterialDataModel();
                            if (maModel.Create(kvp.Key))
                            {
                                var obj = content.GetChild(index);
                                if (obj != null)
                                {
                                    index++;
                                    var element = obj.SafeGetComponent<TechRequireElement>();
                                    bool warning = PlayerManager.Instance.GetMaterialStoreCount(kvp.Key) < kvp.Value ? true : false;
                                    element.SetUpElement( TechRequireElement.RequireType.Material,new object[] { maModel.ID,kvp.Value }, warning);
                                    obj.SafeSetActive(true);
                                }
                            }
                        }

                        break;

                }
            }
        }

        #endregion

        #region BtnClickEvents
        private void OnConfirmBtnClick()
        {
            if (TechnologyDataManager.Instance.CheckTechCanResearch(techInfo.techID))
            {
                UIManager.Instance.HideWnd(this);
                UIManager.Instance.SendMessageToWnd(UIPath.WindowPath.Technology_Page, new UIMessage(UIMsgType.Tech_Research_Start, new List<object>() { techInfo._model.ID }));
                UIManager.Instance.ShowGeneralHint(ResearchStart_Hint_Text, 1.0f);
            }
            else
            {
                UIManager.Instance.ShowGeneralHint(Research_Require_Lack_Hint_Text, 1.0f);
            }
        }


        #endregion
    }
}