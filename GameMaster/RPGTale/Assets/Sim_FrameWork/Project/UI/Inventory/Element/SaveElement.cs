using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sim_FrameWork.UI
{
    public class SaveElement : BaseElement
    {
        [Header("Base Info")]
        public Text Name;
        public Text Organization;
        public Image OrganizationSprite;
        public Text Date;
        public Text GameTime;
        [Header("Button")]
        public Button Btn;

        private SaveDataModel _model;

        public override void ChangeAction(List<BaseDataModel> model)
        {
            _model = (SaveDataModel)model[0];
            InitSaveElement();
        }

        private void InitSaveElement()
        {
            Name.text = _model.Name;
            Organization.text = _model.Organization;
            Date.text = _model.Date;
            GameTime.text = _model.GameTime;
            AddButtonClickListener(Btn, () =>
            {
                OnClickSave();
            });
        }

        void OnClickSave()
        {

        }
    }
}