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


        public override void ChangeAction(BaseElementModel model)
        {
            InitOrderReceiveElement((OrderReceiveElementModel)model);
        }
        public void InitOrderReceiveElement(OrderReceiveElementModel orderModel)
        {
            
            TitleName.text = orderModel.TitleName;
            TitleDesc.text = orderModel.TitleDesc;
            OrderBG.sprite = orderModel.OrderBG;
            //InitOrderDetailElment(orderModel);

            
        }

        /// <summary>
        /// 生成订单详情
        /// </summary>
        /// <param name="info"></param>
        public void InitOrderDetailElment(OrderItemBase item)
        {
            var dic = item.OrderContentDic;
            foreach(KeyValuePair<Material,int> kvp in dic)
            {
                var obj = ObjectManager.Instance.InstantiateObject(UIPath.Order_Detail_Content_Element_Path);
                var element = UIUtility.SafeGetComponent<OrderDetailElement>(obj.transform);
                element.InitOrderDetailElement(new MaterialInfo(kvp.Key), kvp.Value);
                obj.transform.SetParent(OrderContent.transform,false);
            }
        }

        public void InitOrderRewardElement(OrderContent.OrderReward reward)
        {
           

        }


    }
}