using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork
{
    public class OrderItemBase
    {
        public string GUID;
        public string Name;
        public string Desc;
        public Sprite BG;
        

        public Dictionary<Material, int> OrderContentDic;
        public OrderContent.OrderReward rewardData;

        /// <summary>
        /// 订单出现类型
        /// </summary>
        public OrderAppearType appearType = OrderAppearType.None;
        /// <summary>
        /// 订单销毁类型
        /// </summary>
        public OrderDisAppearType disAppearType = OrderDisAppearType.None;

        ///Order Time
        public float OrderTime;
        ///剩余时间
        public float remainTime;


        public OrderItemBase(int orderID)
        {
            var data = OrderModule.GetOrderDataByID(orderID);
            Name = OrderModule.GetOrderName(data);
            Desc = OrderModule.GetOrderDesc(data);
            BG = OrderModule.GetOrderBGPath(data);
            OrderContentDic = OrderModule.GetOrderContent(data);
            OrderTime = data.TimeLimit;
            remainTime = OrderTime;

            rewardData = OrderModule.Instance.GetOrderRewardData(data);
        }
    }

    public enum OrderAppearType
    {
        /// <summary>
        /// 无
        /// </summary>
        None,
        /// <summary>
        /// 根据时间刷新
        /// </summary>
        Time,
        /// <summary>
        /// 根据前置订单刷新
        /// </summary>
        PreOrder,
        /// <summary>
        /// 根据组织情况
        /// </summary>
        OrganizationRelation,
        /// <summary>
        /// 全局参数
        /// </summary>
        GlobalEventFlag,
    }

    public enum OrderDisAppearType
    {
        /// <summary>
        /// 无
        /// </summary>
        None,
        /// <summary>
        /// 根据时间刷新
        /// </summary>
        Time,
        /// <summary>
        /// 根据前置订单刷新
        /// </summary>
        PreOrder,
        /// <summary>
        /// 根据组织情况
        /// </summary>
        OrganizationRelation,
        /// <summary>
        /// 全局参数
        /// </summary>
        GlobalEventFlag,
    }
}