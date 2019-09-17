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

        private OrderInfo orderInfo;


        public void InitOrderReceiveElement(OrderInfo info)
        {
            orderInfo = info;
            TitleName.text = info.Name;
            TitleDesc.text = info.Desc;
            OrderBG.sprite = info.BG;
            InitOrderDetailElment(info);

            
        }

        /// <summary>
        /// 生成订单详情
        /// </summary>
        /// <param name="info"></param>
        public void InitOrderDetailElment(OrderInfo info)
        {
            var dic = info.OrderContentDic;
            foreach(KeyValuePair<Material,int> kvp in dic)
            {
                var obj = ObjectManager.Instance.InstantiateObject(UIPath.Order_Detail_Content_Element_Path);
                var element = obj.GetComponent<OrderDetailElement>();
                element.InitOrderDetailElement(new MaterialInfo(kvp.Key), kvp.Value);
                obj.transform.SetParent(OrderContent.transform);
            }
        }

        public void InitOrderRewardElement(OrderContent.OrderReward reward)
        {
           

        }


    }
}