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

        private bool HasInitDetailElement = false;
        public override void ChangeAction(List<BaseDataModel> model)
        {
            var orderModel = model[0];
            InitOrderReceiveElement((OrderDataModel)orderModel);

        }
        public void InitOrderReceiveElement(OrderDataModel orderModel)
        {
            TitleName.text = orderModel.Name;
            TitleDesc.text = orderModel.Desc;
            OrderBG.sprite = orderModel.Icon;
            if(!HasInitDetailElement)
                InitOrderDetailElment(orderModel);

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