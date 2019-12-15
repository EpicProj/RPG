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

        protected override void Awake()
        {
            base.Awake();
        }


        void Start()
        {
            //For test
            RegisterOrder(new OrderItemBase(1));
            RegisterOrder(new OrderItemBase(2));
            RegisterOrder(new OrderItemBase(1));
            RegisterOrder(new OrderItemBase(2));

            AddOrganization(1);

            InitOrderModelData();
            InitOrganizationModelData();

            InitAllTechInfo();
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
        public List<List<BaseDataModel>> AllOrderDataModelList = new List<List<BaseDataModel>>();
        /// <summary>
        /// 接取的订单
        /// </summary>
        public Dictionary<string, OrderItemBase> PlayerReceivedOrders = new Dictionary<string, OrderItemBase>();
        public List<List<BaseDataModel>> PlayerReceivedModelList = new List<List<BaseDataModel>>();

        /// <summary>
        /// statistics   key  :orderID   value: count
        /// </summary>
        public Dictionary<int, OrderStatisticsItem> OrderStatisticsDic = new Dictionary<int, OrderStatisticsItem>();

        public Dictionary<int, OrganizationInfo> CurrentOrganization = new Dictionary<int, OrganizationInfo>();
        public List<List<BaseDataModel>> CurrrentOrganizationModel = new List<List<BaseDataModel>>();

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
                        RemoveOrderModel(GUID, OrderDicType.All);
                    }
                    break;
                case OrderDicType.Received:
                    if (PlayerReceivedOrders.ContainsKey(GUID))
                    {
                        PlayerReceivedOrders.Remove(GUID);
                        RemoveOrderModel(GUID, OrderDicType.Received);
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


        private List<BaseDataModel> GetOrderBaseData(OrderItemBase item)
        {
            List<BaseDataModel> model = new List<BaseDataModel>();
            model.Add(item.dataModel);
            model.Add(item.belongDataModel);
            return model;
        }

        private void InitOrderModelData()
        {
            Func<Dictionary<string, OrderItemBase>,List<List<BaseDataModel>>> getData = (d) =>
            {
                List<List<BaseDataModel>> result = new List<List<BaseDataModel>>();
                int index = 0;
                foreach (KeyValuePair<string, OrderItemBase> kvp in d)
                {
                    result.Add(GetOrderBaseData(kvp.Value));
                    index++;
                }
                return result;
            };
            AllOrderDataModelList = getData(AllOrderDic);
            PlayerReceivedModelList = getData(PlayerReceivedOrders);
        }

        

        /// <summary>
        /// 移除订单数据
        /// </summary>
        /// <param name="GUIDList"></param>
        /// <param name="type"></param>
        private void RemoveOrderModel(List<string> GUIDList,OrderDicType type)
        {
            switch (type)
            {
                case OrderDicType.All:
                    for(int i = 0; i < GUIDList.Count; i++)
                    {
                        foreach (var item in AllOrderDataModelList)
                        {
                            var model = (OrderDataModel)item[0];
                            if (model.GUID == GUIDList[i])
                            {
                                AllOrderDataModelList.Remove(item);
                                break;
                            }    
                        }
                    }
                    break;
                case OrderDicType.Received:
                    for(int i = 0; i < GUIDList.Count; i++)
                    {
                        foreach (var item in PlayerReceivedModelList)
                        {
                            var model = (OrderDataModel)item[0];
                            if (model.GUID == GUIDList[i])
                            {
                                AllOrderDataModelList.Remove(item);
                                break;
                            }      
                        }
                    }
                    break;
            }
        }
        private void RemoveOrderModel(string GUID, OrderDicType type)
        {
            switch (type)
            {
                case OrderDicType.All:
                    foreach (var item in AllOrderDataModelList)
                    {
                        var model = (OrderDataModel)item[0];
                        if (model.GUID == GUID)
                        {
                            AllOrderDataModelList.Remove(item);
                            break;
                        }
                            
                    }
                    break;
                case OrderDicType.Received:
                    foreach (var item in PlayerReceivedModelList)
                    {
                        var model = (OrderDataModel)item[0];
                        if (model.GUID == GUID)
                        {
                            AllOrderDataModelList.Remove(item);
                            break;
                        }
                           
                    }
                    break;
            }
        }

        /// <summary>
        /// 增加订单数据
        /// </summary>
        /// <param name="GUIDList"></param>
        /// <param name="type"></param>
        private void AddOrderModel(List<string> GUIDList,OrderDicType type)
        {
            switch (type)
            {
                case OrderDicType.All:
                    for(int i = 0; i < GUIDList.Count; i++)
                    {
                        var item = GetOrderItem(GUIDList[i], OrderDicType.All);
                        if(item != null)
                        {
                            AllOrderDataModelList.Add(GetOrderBaseData(item));
                        }  
                    }
                    break;
                case OrderDicType.Received:
                    for(int i = 0; i < GUIDList.Count; i++)
                    {
                        var item = GetOrderItem(GUIDList[i], OrderDicType.Received);
                        if (item != null)
                        {
                            PlayerReceivedModelList.Add(GetOrderBaseData(item));
                        }
                    }
                    break;
            }
        }
        private void AddOrderModel(string GUID,OrderDicType type)
        {
            switch (type)
            {
                case OrderDicType.All:
                    var item = GetOrderItem(GUID, OrderDicType.All);
                    if (item != null)
                    {
                        AllOrderDataModelList.Add(GetOrderBaseData(item));                       
                    }
                    break;
                case OrderDicType.Received:
                    var item2 = GetOrderItem(GUID, OrderDicType.Received);
                    if (item2 != null)
                    {
                        PlayerReceivedModelList.Add(GetOrderBaseData(item2));                      
                    }
                    break;
            }
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
                AddOrderModel(GUID, OrderDicType.Received);
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
            CurrrentOrganizationModel = getData(CurrentOrganization);
        }


        #endregion

        #region Technology

        public Dictionary<int, TechnologyInfo> AllTechDataDic = new Dictionary<int, TechnologyInfo>();

        public List<TechnologyInfo> TechOnResearchList = new List<TechnologyInfo>();


        public void InitAllTechInfo()
        {
            var list = TechnologyModule.Instance.GetAllTech();
            for (int i = 0; i < list.Count; i++)
            {
                TechnologyInfo info = new TechnologyInfo(list[i]);
                AllTechDataDic.Add(info.techID, info);
            }
        }

        public TechnologyInfo GetTechInfo(int techID)
        {
            TechnologyInfo info = null;
            AllTechDataDic.TryGetValue(techID, out info);
            return info;
        }

        public List<int> GetTechStateInfoList(TechnologyInfo.TechState state)
        {
            List<int> result = new List<int>();
            foreach(var info in AllTechDataDic)
            {
                if (info.Value.currentState == state)
                    result.Add(info.Key);
            }
            return result;
        }

        /// <summary>
        /// 检查研究前置条件
        /// </summary>
        /// <param name="techID"></param>
        /// <returns></returns>
        public bool CheckTechCanResearch(int techID)
        {
            bool canResearch = true;

            var requireList = TechnologyModule.Instance.GetTechRequireList(techID);
            for(int i = 0; i < requireList.Count; i++)
            {
                var type = TechnologyModule.Instance.GetTechRequireType(requireList[i]);
                switch (type)
                {
                    case TechRequireType.PreTech:
                        var techList = TechnologyModule.ParseTechParam_Unlock_Tech(requireList[i].Param);
                        for (int j = 0; j < techList.Count; j++)
                        {
                            var info = GetTechInfo(techList[j]);
                            if (info.currentState == TechnologyInfo.TechState.Lock)
                                canResearch = false;
                        }
                        continue;
                    case TechRequireType.Material:
                        var materialDic = TechnologyModule.parseTechParam_Require_Material(requireList[i].Param);
                        foreach(KeyValuePair<int,int> kvp in materialDic)
                        {
                            if (PlayerManager.Instance.GetMaterialStoreCount(kvp.Key) < kvp.Value)
                                canResearch = false;
                        }
                        continue;
                }
            }

            return canResearch;
        }


        /// <summary>
        /// 开始研究
        /// </summary>
        /// <param name="techID"></param>
        public bool OnTechResearchStart(int techID)
        {
            if (CheckTechCanResearch(techID))
            {
                var info = GetTechInfo(techID);
                if (info.currentState == TechnologyInfo.TechState.Unlock)
                {
                    TechOnResearchList.Add(info);
                    info.currentState = TechnologyInfo.TechState.OnResearch;
                    return true;
                }
            }
            return false;
        }


        /// <summary>
        /// 科技研究完成
        /// </summary>
        /// <param name="techID"></param>
        public void OnTechResearchFinish(int techID)
        {
            var info = GetTechInfo(techID);
            if (info != null && info.researchProgress >= 100 && info.currentState == TechnologyInfo.TechState.OnResearch)
            {
                if (TechOnResearchList.Contains(info))
                {
                    TechOnResearchList.Remove(info);
                }
                info.currentState = TechnologyInfo.TechState.Done;
                switch (info.baseType)
                {
                    case TechnologyInfo.TechType.Unique:
                        HandleTechCompleteEvent(info.techID);
                        break;
                    case TechnologyInfo.TechType.Series:
                        break;
                }

                UIManager.Instance.SendMessageToWnd(UIPath.WindowPath.Technology_Page, new UIMessage(UIMsgType.Tech_Research_Finish, new List<object>() { techID }));
            }
        }

        private void HandleTechCompleteEvent(int techID)
        {
            var effect = TechnologyModule.Instance.GetTechCompleteEffect(techID);
            for(int i = 0; i < effect.Count; i++)
            {
                var type = TechnologyModule.Instance.GetTechCompleteType(effect[i]);
                switch (type)
                {
                    case TechCompleteEffect.Unlock_Tech:
                        var techList = TechnologyModule.ParseTechParam_Unlock_Tech(effect[i].effectParam);
                        for (int j = 0; j < techList.Count; j++)
                        {
                            var info = GetTechInfo(techList[j]);
                            info.currentState = TechnologyInfo.TechState.Unlock;
                        }
                        break;
                    case TechCompleteEffect.Unlock_Block:
                        var blockList = TechnologyModule.ParseTechParam_Unlock_Block(effect[i].effectParam);
                        for(int j = 0; j < blockList.Count; j++)
                        {
                            var buildData = PlayerModule.GetBuildingPanelDataByKey(blockList[i]);
                            if (buildData != null)
                            {
                                PlayerManager.Instance.AddUnLockBuildData(buildData);
                            }
                        }
                        break;
                }
            }

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
                    PlayerManager.Instance.AddCurrency(itemList[i].count, PlayerManager.ResourceAddType.current);
                }
                else if (itemList[i].type == GeneralRewardItem.RewardType.Ro_Core)
                {
                    PlayerManager.Instance.AddRoCore((ushort)itemList[i].count, PlayerManager.ResourceAddType.current);
                }
                else if(itemList[i].type == GeneralRewardItem.RewardType.Material)
                {
                    ///Add Material
                    PlayerManager.Instance.AddMaterialData(itemList[i].ItemID, (ushort)itemList[i].count);
                }
                else if(itemList[i].type == GeneralRewardItem.RewardType.TechPoints)
                {
                    ///Add Tech Points
                    PlayerManager.Instance.AddResearch(itemList[i].count, PlayerManager.ResourceAddType.current);
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