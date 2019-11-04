using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sim_FrameWork
{
    public class GeneralOrganizationElement : BaseElement
    {
        [Header("Base Info")]
        public Image Icon;
        public Text OrganizationName;
        public Image AreaIcon;
        public Text AreaText;
        public GameObject Light;

        [Header("Button")]
        public Button Btn;

        public OrganizationDataModel _model;

        public override void Awake()
        {
          
        }

        public override void ChangeAction(List<BaseDataModel> model)
        {
            _model = (OrganizationDataModel)model[0];
            InitElement();
        }

        void InitElement()
        {
            Icon.sprite = _model.Icon;
            OrganizationName.text = _model.Name;
            AreaIcon.sprite = _model.TypeModel.Icon;
            AreaText.text = _model.TypeModel.Name;
            AddButtonClickListener(Btn, () =>
            {
                 UIManager.Instance.SendMessageToWnd(UIPath.Order_Receive_Main_Page, new UIMessage(UIMsgType.OrderPage_Select_Organization, new List<object>(1) { _model.ID }));   
            });
        }

    }
}