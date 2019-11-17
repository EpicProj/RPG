using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sim_FrameWork.UI
{
    public class OrderReceiveElement : BaseElement
    {
        [Header("Base Info")]
        public Text TitleName;
        public Text TitleDesc;
        public Image OrderBG;

        [Header("Element")]
        public GameObject OrderContent;
        public GameObject OrderRewardContent;
        public GameObject OrganizationContent;

        [Header("Button")]
        public Button OrderBtn;


        private OrderDataModel _model;

        private const string Order_Receive_Confirm_Title_Text = "Order_Receive_Confirm_Title";
        private const string Order_Receive_Confirm_Content_Text = "Order_Receive_Confirm_Content";
        private const string Order_Receive_Hint_Text = "Order_Receive_Hint";
        private string Confirm_Title_Text;
        private string Confirm_Content_Text;

        private Image OrganizationIcon;
        private Text OrganizationName;
        private Text OrganizationName_En;

        private bool HasInitDetailElement = false;

        public override void Awake()
        {
            Confirm_Title_Text = MultiLanguage.Instance.GetTextValue(Order_Receive_Confirm_Title_Text);
            Confirm_Content_Text = MultiLanguage.Instance.GetTextValue(Order_Receive_Confirm_Content_Text);
            OrganizationIcon = UIUtility.SafeGetComponent<Image>(OrganizationContent.transform.Find("Icon"));
            OrganizationName = UIUtility.SafeGetComponent<Text>(OrganizationContent.transform.Find("Name"));
            OrganizationName_En = UIUtility.SafeGetComponent<Text>(OrganizationContent.transform.Find("Name_EN"));

        }
        public override void ChangeAction(List<BaseDataModel> model)
        {
            _model = (OrderDataModel)model[0];
            InitOrderReceiveElement();

        }
        public void InitOrderReceiveElement()
        {
            TitleName.text = _model.Name;
            TitleDesc.text = _model.Desc;
            OrderBG.sprite = _model.Icon;

            var organization = OrderModule.GetOrderBelongModel(_model.ID);
            OrganizationIcon.sprite = organization.IconBig;
            OrganizationName.text = organization.Name;
            OrganizationName_En.text = organization.Name_En;
          
            AddBtnListener();

            if (!HasInitDetailElement)
                InitOrderDetailElment(_model);

        }

        private void AddBtnListener()
        {
            AddButtonClickListener(OrderBtn, () =>
            {
                OnReceiveOrderClick();
            });
        }

        void OnReceiveOrderClick()
        {
            Action confirmAction = () =>
            {
                if (GlobalEventManager.Instance.ReceiveOrder(_model.GUID))
                {
                    UIManager.Instance.HideWnd(UIPath.WindowPath.General_Confirm_Dialog);
                    GeneralHintDialogItem hint = new GeneralHintDialogItem(Order_Receive_Hint_Text, 1);
                    UIManager.Instance.PopUpWnd(UIPath.WindowPath.General_Hint_Dialog, WindowType.Dialog,true,hint);
                }
               
            };

            var content = Utility.ParseStringParams(Confirm_Content_Text, new string[1] { _model.Name});
            GeneralConfirmDialogItem item = new GeneralConfirmDialogItem(Confirm_Title_Text,content ,confirmAction);
            UIManager.Instance.PopUpWnd(UIPath.WindowPath.General_Confirm_Dialog, WindowType.Dialog, true, item);
        }



        /// <summary>
        /// 生成订单详情
        /// </summary>
        /// <param name="info"></param>
        public void InitOrderDetailElment(OrderDataModel orderModel)
        {
            var dic = orderModel.OrderContentDic;
            foreach(KeyValuePair<MaterialDataModel,int> kvp in dic)
            {
                var obj = ObjectManager.Instance.InstantiateObject(UIPath.PrefabPath.Order_Detail_Element);
                var element = UIUtility.SafeGetComponent<OrderDetailElement>(obj.transform);
                element.InitOrderDetailElement(kvp.Key, kvp.Value);
                obj.transform.SetParent(OrderContent.transform,false);
            }
            HasInitDetailElement = true;
        }

        public void InitOrderRewardElement(OrderContent.OrderReward reward)
        {
           

        }


    }
}