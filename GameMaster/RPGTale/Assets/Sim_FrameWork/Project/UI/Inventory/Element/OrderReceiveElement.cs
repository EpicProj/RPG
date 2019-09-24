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
        public GameObject ScrollView;
        public GameObject OrderRewardContent;

        [Header("Button")]
        public Button OrderBtn;


        private OrderDataModel _model;

        private const string Order_Receive_Confirm_Title_Text = "Order_Receive_Confirm_Title";
        private const string Order_Receive_Confirm_Content_Text = "Order_Receive_Confirm_Content";
        private string Confirm_Title_Text;
        private string Confirm_Content_Text;

        private bool HasInitDetailElement = false;
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

            Confirm_Title_Text = MultiLanguage.Instance.GetTextValue(Order_Receive_Confirm_Title_Text);
            Confirm_Content_Text =MultiLanguage.Instance.GetTextValue(Order_Receive_Confirm_Content_Text);

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
                    GlobalEventManager.Instance.UnRegisterOrder(_model.GUID);
                    UIManager.Instance.HideWnd(UIPath.General_Confirm_Dialog);
                    //销毁界面订单，TODO
                    //UIManager.Instance.SendMessageToWnd(UIPath.Order_Receive_Main_Page,new UIMessage ( UIMsgType.))
                }
               
            };

            GeneralConfirmDialogItem item = new GeneralConfirmDialogItem(Confirm_Title_Text, Confirm_Content_Text,confirmAction);
            UIManager.Instance.PopUpWnd(UIPath.General_Confirm_Dialog, WindowType.Dialog, true, item);
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
                //var loopList = UIUtility.SafeGetComponent<LoopList>(ScrollView.transform);
                //loopList.InitData()
                var obj = ObjectManager.Instance.InstantiateObject(UIPath.Test);
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