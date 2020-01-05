using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork.UI
{
    public class AssemblePartChooseDialogContext : WindowBase
    {

        private AssemblePartChooseDialog m_dialog;

        private Transform noInfoTrans;

        private List<string> _sortTypeList;
        private GridLoopList _loopList;

        #region OverrideMethod
        public override void Awake(params object[] paralist)
        {
            base.Awake(paralist);
            _sortTypeList = (List<string>)paralist[0];
            AddBtnClick();
        }

        public override bool OnMessage(UIMessage msg)
        {
            return base.OnMessage(msg);
        }

        public override void OnShow(params object[] paralist)
        {
            AudioManager.Instance.PlaySound(AudioClipPath.UISound.Page_Open);
            _sortTypeList = (List<string>)paralist[0];
            SetUpDialog();
        }

        protected override void InitUIRefrence()
        {
            m_dialog = UIUtility.SafeGetComponent<AssemblePartChooseDialog>(Transform);
            _loopList = UIUtility.SafeGetComponent<GridLoopList>(UIUtility.FindTransfrom(m_dialog.contentTrans, "Scroll View"));
            noInfoTrans = UIUtility.FindTransfrom(m_dialog.contentTrans, "NoInfo");
        }

        #endregion

        void AddBtnClick()
        {
            AddButtonClickListener(m_dialog.closeBtn, () =>
            {
                UIManager.Instance.HideWnd(this);
                AudioManager.Instance.PlaySound(AudioClipPath.UISound.Btn_Close);
            });
        }

        void SetUpDialog()
        {
            var partInfoList = PlayerManager.Instance.GetAssemblePartInfoByTypeList(_sortTypeList);
            if (partInfoList.Count == 0 )
            {
                noInfoTrans.gameObject.SetActive(true);
            }
            else
            {
                noInfoTrans.gameObject.SetActive(false);
                var dataModelList = PlayerManager.Instance.GetAssemblePartChooseModel(_sortTypeList);
                _loopList.InitData(dataModelList);
            }

        }

    }
}