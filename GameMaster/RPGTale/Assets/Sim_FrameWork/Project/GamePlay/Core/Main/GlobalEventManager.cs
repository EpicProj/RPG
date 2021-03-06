﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

namespace Sim_FrameWork {

    /// <summary>
    /// Manager Events && Order && Organization
    /// </summary>
    public class GlobalEventManager : Singleton<GlobalEventManager>
    {

        public void InitData()
        {

            ExploreEventManager.Instance.InitData();
            TechnologyDataManager.Instance.InitData();
            //For test
            RegisterOrder(new OrderItemBase(1));
            RegisterOrder(new OrderItemBase(2));
            RegisterOrder(new OrderItemBase(1));
            RegisterOrder(new OrderItemBase(2));

            AddOrganization(1);

            InitOrganizationModelData();
        }

        #region Order
        public enum OrderDicType
        {
            All,
            Received,
        }
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

        public Dictionary<int, OrganizationInfo> CurrentOrganization = new Dictionary<int, OrganizationInfo>();

        /// <summary>
        /// 注册订单
        /// </summary>
        /// <param name="order"></param>
        public bool RegisterOrder(OrderItemBase order)
        {
            if (AllOrderDic.ContainsKey(order.dataModel.GUID))
                return false;
            AllOrderDic.Add(order.dataModel.GUID, order);
            return true;
        }

        public void UnRegisterOrder(string GUID,OrderDicType type)
        {
            switch (type)
            {
                case OrderDicType.All:
                    if (AllOrderDic.ContainsKey(GUID))
                    {
                        AllOrderDic.Remove(GUID);
                    }
                    break;
                case OrderDicType.Received:
                    if (PlayerReceivedOrders.ContainsKey(GUID))
                    {
                        PlayerReceivedOrders.Remove(GUID);
                    }
                    break;
            }
        }

        private OrderItemBase GetOrderItem(string GUID,OrderDicType type)
        {
            OrderItemBase item = null;
            switch (type)
            {
                case OrderDicType.All:
                    AllOrderDic.TryGetValue(GUID, out item);
                    break;
                case OrderDicType.Received:
                    PlayerReceivedOrders.TryGetValue(GUID, out item);
                    break;
            }
            if (item == null)
            {
                Debug.LogError("OrderItem not Exists! GUID=" + GUID);
            }
            return item;
        }

     
        /// <summary>
        /// 接取订单
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public bool ReceiveOrder(string GUID)
        {
            var order = GetOrderItem(GUID, OrderDicType.All);
            if(order != null)
            {
                order.orderState = OrderItemBase.OrderState.Receive;
                if (PlayerReceivedOrders.ContainsKey(GUID))
                {
                    Debug.LogError("Order Error , Can not receive same order with Same GUID");
                    return false;
                }
                PlayerReceivedOrders.Add(GUID, order);
                UnRegisterOrder(GUID, OrderDicType.All);            
                //UpdateUI
                UIManager.Instance.SendMessageToWnd(UIPath.WindowPath.Order_Receive_Main_Page, new UIMessage(UIMsgType.RefreshOrder));
                
                Debug.Log("成功接取订单");
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

            if (!PlayerReceivedOrders.ContainsKey(order.dataModel.GUID))
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
                        completeMonth = time.date.Month,
                        completeYear=time.date.Year
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
                PlayerManager.Instance.AddCurrency_Current(order.rewardData.Reward_Currency);
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


        #region OrderSettle
        /// <summary>
        /// 订单月底结算
        /// </summary>
        public void DoPlayerOrderMonthSettle()
        {
            foreach(var item in PlayerReceivedOrders.Values)
            {
                item.remainTime -= 1;
                if (item.remainTime <= 0)
                {
                    //TODO 订单超时
                    
                }
            }
        }


        #endregion


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
        #endregion

        #region Organization
        public void AddOrganization (int id)
        {
            if (CurrentOrganization.ContainsKey(id))
                return;
            OrganizationInfo info = new OrganizationInfo(id);
            CurrentOrganization.Add(id,info);
        }

        public void RemoveOrganization(int id)
        {
            if (CurrentOrganization.ContainsKey(id))
            {
                CurrentOrganization.Remove(id);
            }
        }

        private List<BaseDataModel> GetOrganizationBaseData(OrganizationInfo info)
        {
            List<BaseDataModel> model = new List<BaseDataModel>();
            model.Add(info.dataModel);
            return model;
        }

        private void InitOrganizationModelData()
        {
            Func<Dictionary<int,OrganizationInfo >, List<List<BaseDataModel>>> getData = (d) =>
            {
                List<List<BaseDataModel>> result = new List<List<BaseDataModel>>();
                int index = 0;
                foreach (KeyValuePair<int, OrganizationInfo> kvp in d)
                {
                    result.Add(GetOrganizationBaseData(kvp.Value));
                    index++;
                }
                return result;
            };
        }






        #endregion


        #region Reward

        /// <summary>
        /// 奖励处理
        /// </summary>
        /// <param name="itemList"></param>
        public void HandleRewardDataItem(List<GeneralRewardItem> itemList)
        {
            for(int i = 0; i < itemList.Count; i++)
            {
                if(itemList[i].type == GeneralRewardItem.RewardType.Currency)
                {
                    ///Add Currency
                    PlayerManager.Instance.AddCurrency_Current(itemList[i].count);
                }
                else if (itemList[i].type == GeneralRewardItem.RewardType.Ro_Core)
                {
                    PlayerManager.Instance.AddRoCore_Current((ushort)itemList[i].count);
                }
                else if(itemList[i].type == GeneralRewardItem.RewardType.Material)
                {
                    ///Add Material
                    PlayerManager.Instance.AddMaterialData(itemList[i].ItemID, (ushort)itemList[i].count);
                }
                else if(itemList[i].type == GeneralRewardItem.RewardType.TechPoints)
                {
                    ///Add Tech Points
                    PlayerManager.Instance.AddResearch_Current(itemList[i].count);
                }
                else if(itemList[i].type == GeneralRewardItem.RewardType.Tech_Unlock)
                {
                    ///UnLock Tech
                    
                }
                else
                {
                    continue;
                }
            }
        }

        public void HandleRewardDataItem(int rewardGroupID)
        {
            ///Empty Reward
            if (rewardGroupID == 0)
                return;
            var data = GeneralModule.GetRewardItem(rewardGroupID);
            if (data.Count != 0)
            {
                HandleRewardDataItem(data);
            }
        }

        #endregion

        #region GlobalFlag

        private Dictionary<string, GlobalFlagItem> _allGlobalFlagDic = new Dictionary<string, GlobalFlagItem>();
        public Dictionary<string, GlobalFlagItem> AllGlobalFlagDic
        {
            get
            {
                return _allGlobalFlagDic;
            }
        }

        public void AddGlobalFlag(GlobalFlagItem item)
        {
            if (!_allGlobalFlagDic.ContainsKey(item.flagName))
            {
                _allGlobalFlagDic.Add(item.flagName, item);
            }
        }

        public void RemoveGlobalFlag(string flagName)
        {
            if (_allGlobalFlagDic.ContainsKey(flagName))
                _allGlobalFlagDic.Remove(flagName);
        }

        public bool CheckGlobalFlagExist(GlobalFlagItem item)
        {
            return _allGlobalFlagDic.ContainsKey(item.flagName);
        }
        public bool CheckGlobalFlagExist(string flagName)
        {
            return _allGlobalFlagDic.ContainsKey(flagName);
        }

        #endregion


        #region Leader Manager

        public Dictionary<ushort, LeaderInfo> LeaderInfoDic = new Dictionary<ushort, LeaderInfo>();




        #endregion


    }


    public class GlobalFlagItem
    {
        public string flagName;
        public int durationTime;
        public GlobalFlagItem(string flagName,int durationTime)
        {
            this.flagName = flagName;
            this.durationTime = durationTime;
        }
    }


    public class OrderStatisticsItem
    {
        public int orderID;

        public List<OrderCompleteItem> orderCompleteData = new List<OrderCompleteItem>();

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

        public OrderStatisticsItem(int orderID, TimeData data = null, int overTime = 0)
        {
            this.orderID = orderID;
            if (data != null)
            {
                OrderCompleteItem item = new OrderCompleteItem
                {
                    completeID = 1,
                    completeYear = data.date.Year,
                    completeMonth = data.date.Month
                };
                orderCompleteData.Add(item);
            }
            OverTimeCount = overTime;
        }

    }
}