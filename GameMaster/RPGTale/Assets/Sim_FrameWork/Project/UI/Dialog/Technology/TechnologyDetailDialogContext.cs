using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sim_FrameWork.UI
{
    public class TechnologyDetailDialogContext : WindowBase
    {
        private TechnologyDetailDialog m_dialog;

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
            m_dialog = UIUtility.SafeGetComponent<TechnologyDetailDialog>(Transform);
            techInfo = (TechnologyInfo)paralist[0];
            InitRef();
            AddBtnClick();
            InitTechEffectElement();
            InitTechRequireElement();
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
            _techNameText = UIUtility.SafeGetComponent<Text>(UIUtility.FindTransfrom(m_dialog.ContextTrans, "Name"));
            _techIcon = UIUtility.SafeGetComponent<Image>(UIUtility.FindTransfrom(m_dialog.ContextTrans, "Slot/Icon"));
            _techCost = UIUtility.SafeGetComponent<Text>(UIUtility.FindTransfrom(m_dialog.ContextTrans, "Detail/Cost/Value"));
            _techTimeCost = UIUtility.SafeGetComponent<Text>(UIUtility.FindTransfrom(m_dialog.ContextTrans, "Detail/Time/Value"));
            _techDesc = UIUtility.SafeGetComponent<Text>(UIUtility.FindTransfrom(m_dialog.ContextTrans, "Desc"));
            _confirmBtnText = UIUtility.SafeGetComponent<Text>(UIUtility.FindTransfrom(m_dialog.transform, "Content/ButtonGeneral/Text"));
            descTypewriterEffect = UIUtility.SafeGetComponent<TypeWriterEffect>(UIUtility.FindTransfrom(m_dialog.ContextTrans, "Desc"));
            _rarityImage = UIUtility.SafeGetComponent<Image>(UIUtility.FindTransfrom(m_dialog.ContextTrans, "Slot/Rarity"));
        }

        #endregion

        #region Method

        private void AddBtnClick()
        {
            AddButtonClickListener(m_dialog.backBtn, () =>
            {
                UIManager.Instance.HideWnd(this);
            });
            AddButtonClickListener(m_dialog.confirmBtn, () =>
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

            PlayDescTypeWriterEffect();
            return true;
        }

        void PlayDescTypeWriterEffect()
        {
            if (descTypewriterEffect != null)
                descTypewriterEffect.StartEffect();
        }


        /// <summary>
        /// 初始化按钮状态
        /// </summary>
        private void InitConfirmBtnState()
        {
            if (techInfo == null)
                return;

            _confirmBtnText.text = "";
            if (techInfo.currentState== TechnologyInfo.TechState.Lock)
            {
                _confirmBtnText.text = MultiLanguage.Instance.GetTextValue(Research_ConfirmBtn_Locked_Text);
                m_dialog.confirmBtn.interactable = false;
            }else if (techInfo.currentState== TechnologyInfo.TechState.Unlock)
            {
                _confirmBtnText.text = MultiLanguage.Instance.GetTextValue(Research_ConfirmBtn_Research_Text);
                m_dialog.confirmBtn.interactable = true;
            }
            else if (techInfo.currentState== TechnologyInfo.TechState.Done)
            {
                _confirmBtnText.text = MultiLanguage.Instance.GetTextValue(Research_ConfirmBtn_Done_Text);
                m_dialog.confirmBtn.interactable = false;
            }else if (techInfo.currentState == TechnologyInfo.TechState.OnResearch)
            {
                _confirmBtnText.text = MultiLanguage.Instance.GetTextValue(Research_ConfirmBtn_Researching_Text);
                m_dialog.confirmBtn.interactable = false;
            }
        }


        private void InitTechEffectElement()
        {
            for (int i = 0; i < GlobalConfigData.TechDetail_Dialog_MaxEffect_Count; i++)
            {
                var obj= ObjectManager.Instance.InstantiateObject(UIPath.PrefabPath.Tech_Effect_Element);
                obj.transform.SetParent(m_dialog.EffectContentTrans,false);
                obj.name = "TechEffectObj" + i;
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
            foreach(Transform trans in m_dialog.EffectContentTrans)
            {
                trans.gameObject.SetActive(false);
            }

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
                                var element = m_dialog.EffectContentTrans.GetChild(totalIndex);
                                if (element != null)
                                {
                                    totalIndex++;
                                    var cmpt = UIUtility.SafeGetComponent<TechEffectElement>(element.transform);
                                    cmpt.SetUpElement(model.Icon, name, model.Name, Color.white);
                                    element.gameObject.SetActive(true);
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
                                var element = m_dialog.EffectContentTrans.GetChild(totalIndex);
                                if (element != null)
                                {
                                    totalIndex++;
                                    var cmpt = UIUtility.SafeGetComponent<TechEffectElement>(element.transform);
                                    cmpt.SetUpElement(model.Icon, name, model.Name, model.Rarity.color);
                                    element.gameObject.SetActive(true);
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

        private void InitTechRequireElement()
        {
            for (int i = 0; i < GlobalConfigData.TechDetail_Dialog_MaxRequire_Count; i++)
            {
                var obj = ObjectManager.Instance.InstantiateObject(UIPath.PrefabPath.Tech_Require_Element);
                obj.transform.SetParent(m_dialog.RequireContentTrans,false);
                obj.name = "TechRequireElement" + i;
            }
        }

        private void SetUpTechRequire()
        {
            if (techInfo == null)
                return;
            foreach(Transform trans in m_dialog.RequireContentTrans)
            {
                trans.gameObject.SetActive(false);
            }

            var requireList = techInfo.techRequireList;

            int index = 0;

            //Init Cost
            var costObj = m_dialog.RequireContentTrans.GetChild(index);
            if (costObj != null)
            {
                index++;
                var cmpt = UIUtility.SafeGetComponent<TechRequireElement>(costObj);
                cmpt.SetUpElement( TechRequireElement.RequireType.ResearchPoint, new object[] { techInfo._model.TechCost },false);
                costObj.gameObject.SetActive(true);
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
                                var obj = m_dialog.RequireContentTrans.GetChild(index);
                                if (obj != null)
                                {
                                    index++;
                                    var element = UIUtility.SafeGetComponent<TechRequireElement>(obj);
                                    bool warning = GlobalEventManager.Instance.GetTechInfo(techList[j]).currentState == TechnologyInfo.TechState.Lock ? true : false;
                                    element.SetUpElement( TechRequireElement.RequireType.PreTech,new object[] { techModel.ID }, warning);
                                    obj.gameObject.SetActive(true);
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
                                var obj = m_dialog.RequireContentTrans.GetChild(index);
                                if (obj != null)
                                {
                                    index++;
                                    var element = UIUtility.SafeGetComponent<TechRequireElement>(obj);
                                    bool warning = PlayerManager.Instance.GetMaterialStoreCount(kvp.Key) < kvp.Value ? true : false;
                                    element.SetUpElement( TechRequireElement.RequireType.Material,new object[] { maModel.ID,kvp.Value }, warning);
                                    obj.gameObject.SetActive(true);
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
            if (GlobalEventManager.Instance.CheckTechCanResearch(techInfo.techID))
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