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
            foreach(Transform trans in _btnTrans)
            {
                trans.gameObject.SetActive(false);
                UIUtility.SafeGetComponent<Button>(trans).onClick.RemoveAllListeners();
            }

            AddButtonClickListener(_btn01, () =>
            {
                _item._action1?.Invoke();
                AudioManager.Instance.PlaySound(AudioClipPath.UISound.Button_Click, false);
            });
            AddButtonClickListener(_btn02, () =>
            {
                _item._action2?.Invoke();
                AudioManager.Instance.PlaySound(AudioClipPath.UISound.Button_Click, false);
            });
            AddButtonClickListener(_btn03, () =>
            {
                _item._action3?.Invoke();
                AudioManager.Instance.PlaySound(AudioClipPath.UISound.Button_Click, false);
            });

            _btn01Text.text = _item._btnText1;
            _btn02Text.text = _item._btnText2;
            _btn03Text.text = _item._btnText3;


            for (int i = 0; i < _item.ButtonNum; i++)
            {
                if (i > 2)
                    break;
                _btnTrans.GetChild(i).gameObject.SetActive(true);
            }
        }
    }

    public partial class GeneralConfirmDialogContext : WindowBase
    {
        private Text _titleText;
        private Text _contentText;

        private Transform _btnTrans;
        private Button _btn01;
        private Button _btn02;
        private Button _btn03;

        private Text _btn01Text;
        private Text _btn02Text;
        private Text _btn03Text;

        private const string ConfirmText = "GeneralConfirmDialog_Confirm_Text";
        private const string CancelText = "GeneralConfirmDialog_Cancel_Text";

        protected override void InitUIRefrence()
        {
            _titleText = UIUtility.SafeGetComponent<Text>(UIUtility.FindTransfrom(Transform, "Content/Title/Text"));
            _contentText = UIUtility.SafeGetComponent<Text>(UIUtility.FindTransfrom(Transform, "Content/Content/text"));

            _btnTrans = UIUtility.FindTransfrom(Transform, "Content/Action");
            _btn01 = UIUtility.SafeGetComponent<Button>(UIUtility.FindTransfrom(_btnTrans, "Btn1"));
            _btn02 = UIUtility.SafeGetComponent<Button>(UIUtility.FindTransfrom(_btnTrans, "Btn2"));
            _btn03 = UIUtility.SafeGetComponent<Button>(UIUtility.FindTransfrom(_btnTrans, "Btn3"));

            _btn01Text= UIUtility.SafeGetComponent<Text>(UIUtility.FindTransfrom(_btn01.transform, "Text"));
            _btn02Text = UIUtility.SafeGetComponent<Text>(UIUtility.FindTransfrom(_btn02.transform, "Text"));
            _btn03Text = UIUtility.SafeGetComponent<Text>(UIUtility.FindTransfrom(_btn03.transform, "Text"));
        }
    }






}


