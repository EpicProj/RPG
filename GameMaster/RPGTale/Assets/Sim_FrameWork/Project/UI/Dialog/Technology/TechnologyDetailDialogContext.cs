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

        private const string ResearchStart_Hint_Text = "ResearchStart_Hint_Text";
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

        private int EffectElementCount;

        #region Override Method

        public override void Awake(params object[] paralist)
        {
            m_dialog = UIUtility.SafeGetComponent<TechnologyDetailDialog>(Transform);
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
        }

        public override void OnClose()
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
        }

        #endregion


        private void AddBtnClick()
        {
            AddButtonClickListener(m_dialog.backBtn, () =>
            {
                UIManager.Instance.HideWnd(this);
            });
            AddButtonClickListener(m_dialog.confirmBtn, () =>
            {
                UIManager.Instance.HideWnd(this);
                UIManager.Instance.SendMessageToWnd(UIPath.WindowPath.Technology_Page, new UIMessage(UIMsgType.Tech_Research_Start, new List<object>() { techInfo._model.ID }));
                UIManager.Instance.ShowGeneralHint(ResearchStart_Hint_Text, 1.0f);
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
            return true;
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


        GameObject SetUpEffectElement(Sprite sp, string unlockName, string name)
        {
            var obj = ObjectManager.Instance.InstantiateObject(UIPath.PrefabPath.Tech_Effect_Element);
            if (obj != null)
            {
                var element = UIUtility.SafeGetComponent<TechEffectElement>(obj.transform);
                element.SetUpElement(sp, unlockName, name);
            }
            return obj;
        }
        /// <summary>
        /// 初始化科技完成效果
        /// </summary>
        private void SetUpTechEffect()
        {
            if (techInfo == null)
                return;
            ///Recycle All Element
            foreach(Transform trans in m_dialog.EffectContentTrans)
            {
                ObjectManager.Instance.ReleaseObject(trans.gameObject);
                Debug.Log("ReleaseObject " + trans.name);
            }

            var effectType = TechnologyModule.GetTechCompleteEffect(techInfo.techID);
            switch (effectType)
            {
                case TechCompleteEffect.Unlock_Block:
                    var blockList = TechnologyModule.ParseTechParam_Unlock_Block(techInfo.techID);
                    EffectElementCount = blockList.Count;
                    for (int i = 0; i < blockList.Count; i++)
                    {
                        FunctionBlockDataModel model = new FunctionBlockDataModel();
                        if (model.Create(blockList[i]))
                        {
                            var name = MultiLanguage.Instance.GetTextValue(Research_Effect_Unlock_Text_Block);
                            var element= SetUpEffectElement(model.Icon, name, model.Name);
                            element.transform.SetParent(m_dialog.EffectContentTrans,false);
                        }
                        else
                        {
                            break;
                        }
                    }
                    break;

                case TechCompleteEffect.Unlock_Tech:
                    var techList = TechnologyModule.ParseTechParam_Unlock_Tech(techInfo.techID);
                    EffectElementCount = techList.Count;
                    for (int i = 0; i < techList.Count; i++)
                    {
                        TechnologyDataModel model = new TechnologyDataModel();
                        if (model.Create(techList[i]))
                        {
                            var name = MultiLanguage.Instance.GetTextValue(Research_Effect_Unlock_Text_Tech);
                            var element= SetUpEffectElement(model.Icon, name, model.Name);
                            element.transform.SetParent(m_dialog.EffectContentTrans,false);
                        }
                        else
                        {
                            break;
                        }
                    }
                    break;
            }
        }





    }
}