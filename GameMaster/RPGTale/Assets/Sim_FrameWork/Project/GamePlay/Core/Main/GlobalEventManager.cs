using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

namespace Sim_FrameWork {

    /// <summary>
    /// Manager Events && Order && Organization
    /// </summary>
    public class GlobalEventManager : MonoSingleton<GlobalEventManager>
    {
        /// <summary>
        /// 列表中订单
        /// </summary>
        public Dictionary<string, OrderItemBase> AllOrderDic = new Dictionary<string, OrderItemBase>();
        /// <summary>
        /// 接取的订单
        /// </summary>
        public Dictionary<string, OrderItemBase> PlayerReceivedOrders = new Dictionary<string, OrderItemBase>();

        /// <summary>
        /// statistics   key  :orderID   value: count
        /// </summary>
        public Dictionary<int, OrderStatisticsItem> OrderStatisticsDic = new Dictionary<int, OrderStatisticsItem>();

        protected override void Awake()
        {
            base.Awake();
        }
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                UIManager.Instance.SendMessageToWnd(UIPath.Order_Receive_Main_Page, new UIMessage(UIMsgType.Order_Receive_Main, AllOrderDic));
            }
        }

        void Start()
        {
            //For test
            RegisterOrder(new OrderItemBase(1));

        }

        /// <summary>
        /// 注册订单
        /// </summary>
        /// <param name="order"></param>
        public void RegisterOrder(OrderItemBase order)
        {
        generate: string strGUID = Guid.NewGuid().ToString();
            if (AllOrderDic.ContainsKey(strGUID))
                goto generate;
            order.GUID = strGUID;
            AllOrderDic.Add(strGUID, order);
        }

        /// <summary>
        /// 接取订单
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public bool ReceiveOrder(OrderItemBase order)
        {
            if (AllOrderDic.ContainsKey(order.GUID))
            {
                order.orderState = OrderItemBase.OrderState.Receive;
                if (PlayerReceivedOrders.ContainsKey(order.GUID))
                {
                    Debug.LogError("Order Error , Can not receive same order with Same GUID");
                    return false;
                }
                PlayerReceivedOrders.Add(order.GUID, order);
                AllOrderDic.Remove(order.GUID);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 完成订单
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public bool OrderComplete(OrderItemBase order)
        {
            /// bool= is overTime
            Action<int,bool> GenerateOrderStatistics =(i,b) =>
            {
                var time = PlayerManager.Instance.getCurrentTime();
                if (b)
                {
                    OrderStatisticsItem item = new OrderStatisticsItem(i,null,1);
                    OrderStatisticsDic.Add(i, item);
                }
                else
                {
                    OrderStatisticsItem item = new OrderStatisticsItem(i, time);
                    OrderStatisticsDic.Add(i, item);
                }             
            };

            if (!PlayerReceivedOrders.ContainsKey(order.GUID))
            {
                ///首个订单超时
                if(order.orderState== OrderItemBase.OrderState.OverTime)
                {                   
                    GenerateOrderStatistics(order.OrderID, true);
                    return true;
                }///首个订单完成
                else if(order.orderState== OrderItemBase.OrderState.Complete)
                {
                    AddOrderCompleteReward(order);
                    GenerateOrderStatistics(order.OrderID, false);
                    return true;
                }
            }
            else///已有订单数据
            {
                var item = GetOrderStatisticsItem(order.OrderID);
                if (item == null)
                    return false;
                if(order.orderState == OrderItemBase.OrderState.OverTime)
                {
                    item.OverTimeCount++;
                    return true;
                }else if (order.orderState== OrderItemBase.OrderState.Complete)
                {
                    var time = PlayerManager.Instance.getCurrentTime();
                    AddOrderCompleteReward(order);
                    item.orderCompleteData.Add(new OrderStatisticsItem.OrderCompleteItem
                    {
                        completeID = item.OrderCompleteCount + 1,
                        completeMonth = time.currentMonth,
                        completeYear=time.currentYear
                    });
                    return true;
                }
            }
            return false;
        }

        public void AddOrderCompleteReward(OrderItemBase order)
        {
            if (order.rewardData.Reward_Currency != 0)
            {
                PlayerManager.Instance.AddCurrency(order.rewardData.Reward_Currency, PlayerManager.ResourceAddType.current);
            }
            if(order.rewardData.Reward_Reputation != 0)
            {
                PlayerManager.Instance.AddReputation(order.rewardData.Reward_Reputation, PlayerManager.ResourceAddType.max);
            }
            //Add material Reward
            var ma = OrderModule.Instance.GetOrderRewardMaterial(order.rewardData);
            foreach(KeyValuePair<Material, ushort> kvp in ma)
            {
                PlayerManager.Instance.AddMaterialData(kvp.Key.MaterialID, kvp.Value);
            }
        }


        public OrderStatisticsItem GetOrderStatisticsItem(int orderID)
        {
            OrderStatisticsItem item = null;
            OrderStatisticsDic.TryGetValue(orderID, out item);
            if (item == null)
            {
                Debug.LogError("orderStatistics Item get Error ,orderID=" + orderID);
            }
            return item;
        }

        #region Sizer

        public OrderItemBase[] FilterOrderByType(string type)
        {
            var results = AllOrderDic.Where(kvp => kvp.Value.typeInfo.TypeID == type).Select(kvp => kvp.Value).ToArray();
            if (results != null)
            {
                return results;
            }
            return null;
        }


        #endregion

        #region Order Appear

        public void OrderAppear(OrderAppearData.Appear_PreOrder preOrder)
        {

        }


        #endregion

    }

    public class OrderStatisticsItem
    {
        public int orderID;
        
        public List<OrderCompleteItem> orderCompleteData =new List<OrderCompleteItem> ();

        public int OrderCompleteCount { get { return orderCompleteData.Count; } }

        /// <summary>
        /// 超时次数
        /// </summary>
        public int OverTimeCount;

        public class OrderCompleteItem
        {
            public int completeID;
            public int completeYear;
            public int completeMonth;
        }

        public OrderStatisticsItem(int orderID,TimeData data=null,int overTime=0)
        {
            this.orderID = orderID;
            if(data != null)
            {
                OrderCompleteItem item = new OrderCompleteItem
                {
                    completeID = 1,
                    completeYear = data.currentYear,
                    completeMonth = data.currentMonth
                };
                orderCompleteData.Add(item);
            }       
            OverTimeCount = overTime;
        }

    }
}