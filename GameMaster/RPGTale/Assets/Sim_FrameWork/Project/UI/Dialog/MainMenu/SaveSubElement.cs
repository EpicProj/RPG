using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sim_FrameWork
{
    public class SaveSubElement : BaseElement
    {
        private Text _indexText;
        private Text _dateText;
        private Transform _autoSaveTrans;

        public SaveDataItemModel _model;
        public override void Awake()
        {
            _indexText = transform.FindTransfrom("Index/Text").SafeGetComponent<Text>();
            _dateText = transform.FindTransfrom("Date").SafeGetComponent<Text>();
            _autoSaveTrans = transform.FindTransfrom("AutoSave");
            base.Awake();
        }

        public override void ChangeAction(BaseDataModel model)
        {
            _model = (SaveDataItemModel)model;
            InitElement();
        }


        void InitElement()
        {
            _indexText.text = _model._indexID.ToString();
            _dateText.text = _model.Date.ToString();
            AddButtonClickListener(transform.SafeGetComponent<Button>(), OnBtnClick);
        }

        void OnBtnClick()
        {
            UIManager.Instance.SendMessageToWnd(UIPath.WindowPath.MainMenu_GameLoad_Dialog, new UIMessage(UIMsgType.GameSave_Select_Save, new List<object>() { _model._indexID }));
        }

        public void SetSelect(bool select)
        {
            transform.FindTransfrom("SelectEffect").SafeSetActive(select);
        }
    }
}