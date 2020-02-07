using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sim_FrameWork.UI
{
    public partial class GeneralConfirmDialogContext :WindowBase
    {

        private GeneralConfirmDialogItem _item;

        public override void Awake(params object[] paralist)
        {
            base.Awake();
        }

        public override void OnShow(params object[] paralist)
        {
            _item = (GeneralConfirmDialogItem)paralist[0];
            if (_item != null)
            {
                InitBtn();
                InitText();
            }
            AudioManager.Instance.PlaySound(AudioClipPath.UISound.Hint_Open, false);
        }

        private void InitText()
        {
            _contentText.text = _item.ContentText;
            _titleText.text = _item.TitleText;
        }

        private void InitBtn()
        {
            _btnTrans.SafeSetActiveAllChild(false);
            if (_item.btnList != null)
            {
                for(int i = 0; i < _item.btnList.Count; i++)
                {
                    if (i > 2)
                        break;
                    var item = _btnTrans.GetChild(i);
                    item.FindTransfrom("Text").SafeGetComponent<Text>().text = _item.btnList[i]._btnText;
                    item.FindTransfrom("BG").SafeGetComponent<Image>().color = GetBtnColor(_item.btnList[i]._btnColor);
                    item.SafeSetActive(true);
                    //Anim
                    var canvasGroup = item.SafeGetComponent<CanvasGroup>();
                    canvasGroup.alpha = 0;
                    canvasGroup.DoCanvasFade(1, 0.5f);
                }
            }
            AddBtnClick();
        }

        void AddBtnClick()
        {
            ///AddBtn
            AddButtonClickListener(_btnTrans.GetChild(0).SafeGetComponent<Button>(), GetActionIndex(0));
            AddButtonClickListener(_btnTrans.GetChild(1).SafeGetComponent<Button>(), GetActionIndex(1));
            AddButtonClickListener(_btnTrans.GetChild(2).SafeGetComponent<Button>(), GetActionIndex(2));
        }

        UnityEngine.Events.UnityAction GetActionIndex(int index)
        {
            return _item.btnList.Count <= index ? null : _item.btnList[index]._action;
        }

        Color GetBtnColor(GeneralConfrimBtnItem.btnColor color)
        {
            if (color == GeneralConfrimBtnItem.btnColor.Blue)
                return new Color(0, 0.43f, 0.7f, 0.82f);
            else if (color == GeneralConfrimBtnItem.btnColor.Red)
                return new Color(0.5f, 0, 0.017f, 0.82f);
            else
            {
                return Color.red;
            }
        }
    }

    public partial class GeneralConfirmDialogContext : WindowBase
    {
        private Text _titleText;
        private Text _contentText;

        private Transform _btnTrans;

        private const string ConfirmText = "GeneralConfirmDialog_Confirm_Text";
        private const string CancelText = "GeneralConfirmDialog_Cancel_Text";

        protected override void InitUIRefrence()
        {
            _titleText = Transform.FindTransfrom("Content/Title/Text").SafeGetComponent<Text>();
            _contentText = Transform.FindTransfrom("Content/Content/text").SafeGetComponent<Text>();

            _btnTrans = Transform.FindTransfrom("Content/Action");
        }
    }






}