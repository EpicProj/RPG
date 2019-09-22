using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork
{
    public class OrderItemBase
    {
        public enum OrderState
        {
            /// <summary>
            /// 创建
            /// </summary>
            Create,
            /// <summary>
            /// 接取
            /// </summary>
            Receive,
            /// <summary>
            /// 完成
            /// </summary>
            Complete,
            /// <summary>
            /// 超时
            /// </summary>
            OverTime,
        }

        public enum OrderRarity
        {
            Common,
            Rare,
            Epic
        }

        /// <summary>
        /// Base Info
        /// </summary>
        public string GUID;
        public int OrderID;

        /// <summary>
        /// 归属组织信息
        /// </summary>
        public OrganizationDataModel belongDataModel;
        public OrderDataModel dataModel;

        public OrderRarity orderRarity;
        public OrderState orderState;

        public OrderTypeInfo typeInfo;
        
        /// <summary>
        /// 订单内容
        /// </summary>
        public OrderContent.OrderReward rewardData;

        /// <summary>
        /// 订单出现条件
        /// </summary>
        public OrderAppearData appearData;
        /// <summary>
        /// 订单销毁条件
        /// </summary>
        public OrderDisAppearData disAppearData;

        ///Order Time
        public float OrderTime;
        ///剩余时间
        public float remainTime;


        public OrderItemBase(int orderID)
        {
            var data = OrderModule.GetOrderDataByID(orderID);
            if (data == null)
                return;
            OrderID = data.OrderID;
            belongDataModel = OrderModule.GetOrganizationBelong(OrderID);
            dataModel = new OrderDataModel();
            dataModel.Create(OrderID);

            typeInfo = OrderModule.GetOrderTypeInfo(data);
            orderState = OrderState.Create;
            OrderTime = data.TimeLimit;
            remainTime = OrderTime;

            rewardData = OrderModule.Instance.GetOrderRewardData(data);

            appearData = OrderModule.Instance.GetOrderAppearData(data.OrderID);
            disAppearData = OrderModule.Instance.GetOrderDisAppearData(data.OrderID);
        }

    }

    public class OrderAppearData
    {
        public Appear_Time appear_Time;
        public Appear_PreOrder appear_PreOrder;


        public class Appear_Time
        {
            public OrderAppearType type = OrderAppearType.Time;
            public int Time_Year;
            public int Time_Month;
            public float Weight_Origin;
            public float Weight_Month_Add;

            public Appear_Time(OrderAppearConfig.Appear_Time_Config config)
            {
                Time_Year = config.Time_Year;
                if (Utility.CheckValueInRange(0, 12, config.Time_Month, "Order Appear Time_Month"))
                {
                    Time_Month = config.Time_Month;
                }
                Weight_Origin = config.Weight_Origin;
                Weight_Month_Add = config.Weight_Month_Add;
            }
        }

        public class Appear_PreOrder
        {
            public OrderAppearType type = OrderAppearType.PreOrder;
            public Dictionary<OrderData, int> OrderContent;

            public Appear_PreOrder(OrderAppearConfig.Appear_PreOrder_Config config)
            {
                OrderContent = new Dictionary<OrderData, int>();
                foreach(KeyValuePair<string,int> kvp in config.OrderContent)
                {
                    var o = OrderModule.GetOrderDataByID(Utility.TryParseInt(kvp.Key));
                    if (o != null)
                    {
                        OrderContent.Add(o, kvp.Value);
                    }
                }
            }

        }


        public OrderAppearData(OrderAppearConfig config)
        {
            if (config.appear_Time != null)
            {
                appear_Time = new Appear_Time(config.appear_Time);
            }
            if (config.appear_PreOrder != null)
            {
                appear_PreOrder = new Appear_PreOrder(config.appear_PreOrder);
            }
           
        }

    }

    public class OrderDisAppearData
    {
        public DisAppear_Time disAppear_Time;
        public DisAppear_PreOrder disAppear_PreOrder;

        public class DisAppear_Time
        {
            public OrderDisAppearType type =  OrderDisAppearType.Time;
            public ushort Time_Year;
            public ushort Time_Month;
            public float Weight_Origin;
            public float Weight_Month_Add;

            public DisAppear_Time(OrderDisAppearConfig.DisAppear_Time_Config config)
            {
                Time_Year = config.Time_Year;
                if (Utility.CheckValueInRange(0, 12, config.Time_Month, "Order DisAppear Time_Month"))
                {
                    Time_Month = config.Time_Month;
                }        
                Weight_Origin = config.Weight_Origin;
                Weight_Month_Add = config.Weight_Month_Add;
            }
        }

        public class DisAppear_PreOrder
        {
            public OrderDisAppearType type =  OrderDisAppearType.PreOrder;
            public Dictionary<OrderData, int> OrderContent;

            public DisAppear_PreOrder(OrderDisAppearConfig.DisAppear_PreOrder_Config config)
            {
                OrderContent = new Dictionary<OrderData, int>();
                foreach (KeyValuePair<string, int> kvp in config.OrderContent)
                {
                    var o = OrderModule.GetOrderDataByID(Utility.TryParseInt(kvp.Key));
                    if (o != null)
                    {
                        OrderContent.Add(o, kvp.Value);
                    }
                }
            }

        }

        public OrderDisAppearData(OrderDisAppearConfig config)
        {
            if (config.disAppear_Time != null)
            {
                disAppear_Time = new DisAppear_Time(config.disAppear_Time);
            }
            if (config.disAppear_Preorder != null)
            {
                disAppear_PreOrder = new DisAppear_PreOrder(config.disAppear_Preorder);
            }
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