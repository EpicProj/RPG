using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sim_FrameWork
{
    public class FormulaChooseElement : BaseElement
    {
        public Button Btn;
        public Image Icon;
        public Text Name;
        public Transform SelectTrans;
        private CanvasGroup selectEffect;

        public FormulaDataModel _model;

        public override void Awake()
        {
            selectEffect = UIUtility.SafeGetComponent<CanvasGroup>(SelectTrans);
        }
        public override void ChangeAction(BaseDataModel model)
        {
            _model = (FormulaDataModel)model;
            InitElement();
        }

        void InitElement()
        {
            Icon.sprite = _model.Icon;
            Name.text = _model.Name;
            Select(false);
            AddButtonClickListener(Btn, () =>
            {
                UIManager.Instance.SendMessageToWnd(UIPath.WindowPath.ProductLine_Change_Dialog, new UIMessage(UIMsgType.ProductLine_Formula_Change, new List<object>(1) { _model.ID }));
                AudioManager.Instance.PlaySound(AudioClipPath.UISound.Button_Click);
                Select(true);
            });
        }
        public void Select(bool active)
        {
            if (active)
            {
                selectEffect.alpha = 1;
            }
            else
            {
                selectEffect.alpha = 0;
            }
        }

    }
}