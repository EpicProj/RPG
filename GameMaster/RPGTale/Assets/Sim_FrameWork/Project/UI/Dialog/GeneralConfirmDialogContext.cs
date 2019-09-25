using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork.UI
{
    public  class GeneralConfirmDialogContext :WindowBase
    {
        private GeneralConfirmDialog m_dialog;

        private GeneralConfirmDialogItem _item;


        private const string ConfirmText = "GeneralConfirmDialog_Confirm_Text";
        private const string CancelText = "GeneralConfirmDialog_Cancel_Text";

        public override void Awake(params object[] paralist)
        {
            m_dialog = UIUtility.SafeGetComponent<GeneralConfirmDialog>(Transform);   
        }

        public override void OnShow(params object[] paralist)
        {
            _item = (GeneralConfirmDialogItem)paralist[0];
            AddBtnListener();
            InitText();
        }


        private void InitText()
        {
            m_dialog.ContentText.text = _item.ContentText;
            m_dialog.TitleText.text = _item.TitleText;
            m_dialog.ConfirmText.text = MultiLanguage.Instance.GetTextValue(ConfirmText);
            m_dialog.CancelText.text = MultiLanguage.Instance.GetTextValue(CancelText);
        }

        private void AddBtnListener()
        {
            AddButtonClickListener(m_dialog.ConfirmBtn, () =>
            {
                _item.OnConfirmClick?.Invoke();
            });
            AddButtonClickListener(m_dialog.CancelBtn, () =>
            {
                _item.OnCancelClick?.Invoke();
            });
        }



    }







}


