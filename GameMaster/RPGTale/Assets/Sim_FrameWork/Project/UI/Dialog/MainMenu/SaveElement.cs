using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sim_FrameWork.UI
{
    public class SaveElement : BaseElement
    {
        private Text Name;

        private Text Date;
        private Text GameTime;

        public SaveDataGroupModel _model;

        public override void Awake()
        {
            Name = transform.FindTransfrom("Name/Text").SafeGetComponent<Text>();
            Date = transform.FindTransfrom("Date/Text").SafeGetComponent<Text>();
            GameTime = transform.FindTransfrom("GameTime/Text").SafeGetComponent<Text>();
        }

        public override void ChangeAction(BaseDataModel model)
        {
            _model = (SaveDataGroupModel)model;
            InitSaveElement();
        }

        private void InitSaveElement()
        {
            Name.text = _model.Name;
            Date.text = _model.Date;
            GameTime.text = _model.GameTime;
            AddButtonClickListener(transform.SafeGetComponent<Button>(), () =>
            {
                OnClickSave();
            });
        }

        void OnClickSave()
        {
            UIManager.Instance.SendMessageToWnd(UIPath.WindowPath.MainMenu_GameLoad_Dialog, new UIMessage(UIMsgType.GameSave_Select_Group, new List<object>() { _model.ID }));
        }

        public void SetSelect(bool select)
        {
            transform.FindTransfrom("SelectEffect").SafeSetActive(select);
        }
    }
}