using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Sim_FrameWork
{
    public class PlayerManager : MonoSingleton<PlayerManager>
    {
        public enum ResourceAddType
        {
            current,
            month,
            max
        }

        public PlayerData playerData;

        /// <summary>
        /// Time Manager
        /// </summary>
        private float timer;


        protected override void Awake()
        {
            base.Awake();
            playerData = PlayerModule.Instance.InitPlayerData();
        }

        private void Start()
        {
            GameManager.Instance.InitBaseData();
            UIManager.Instance.PopUpWnd(UIPath.MainMenu_Page, WindowType.Page, true, playerData);
        }

        private void Update()
        {
            if(GameManager.Instance.gameStates== GameManager.GameStates.Start)
            {
                UpdateTime();
            }

        }


        #region Resource Manager
        public void AddCurrency(float num, ResourceAddType type, Action callback = null)
        {
            switch (type)
            {
                case ResourceAddType.current:
                    playerData.resourceData.AddCurrency(num);
                    UIManager.Instance.SendMessageToWnd(UIPath.MainMenu_Page, new UIMessage(UIMsgType.Res_Currency));
                    callback?.Invoke();
                    break;
                case ResourceAddType.max:
                    playerData.resourceData.AddCurrencyMax(num);
                    callback?.Invoke();
                    break;
                default:
                    break;
            }

        }
        public void AddFood(float num, ResourceAddType type, Action callback = null)
        {
            switch (type)
            {
                case ResourceAddType.current:
                    playerData.resourceData.AddFood(num);
                    UIManager.Instance.SendMessageToWnd(UIPath.MainMenu_Page, new UIMessage(UIMsgType.Res_Food));
                    callback?.Invoke();
                    break;
                case ResourceAddType.max:
                    playerData.resourceData.AddFoodMax(num);
                    callback?.Invoke();
                    break;
                case ResourceAddType.month:
                    playerData.resourceData.AddFoodPerMonth(num);
                    UIManager.Instance.SendMessageToWnd(UIPath.MainMenu_Page, new UIMessage(UIMsgType.Res_MonthFood));
                    callback?.Invoke();
                    break;
                default:
                    break;
            }

        }
        public void AddEnergy(float num, ResourceAddType type, Action callback = null)
        {
            switch (type)
            {
                case ResourceAddType.current:
                    playerData.resourceData.AddEnergy(num);
                    UIManager.Instance.SendMessageToWnd(UIPath.MainMenu_Page, new UIMessage(UIMsgType.Res_Energy));
                    callback?.Invoke();
                    break;
                case ResourceAddType.max:
                    playerData.resourceData.AddEnergyMax(num);
                    callback?.Invoke();
                    break;
                case ResourceAddType.month:
                    playerData.resourceData.AddEnergyPerMonth(num);
                    UIManager.Instance.SendMessageToWnd(UIPath.MainMenu_Page, new UIMessage(UIMsgType.Res_MonthEnergy));
                    callback?.Invoke();
                    break;
                default:
                    break;
            }

        }
        public void AddLabor(float num, ResourceAddType type, Action callback = null)
        {
            switch (type)
            {
                case ResourceAddType.current:
                    playerData.resourceData.AddLabor(num);
                    UIManager.Instance.SendMessageToWnd(UIPath.MainMenu_Page, new UIMessage(UIMsgType.Res_Labor));
                    callback?.Invoke();
                    break;
                case ResourceAddType.max:
                    playerData.resourceData.AddLaborMax(num);
                    callback?.Invoke();
                    break;
                case ResourceAddType.month:
                    playerData.resourceData.AddLaborPerMonth(num);
                    UIManager.Instance.SendMessageToWnd(UIPath.MainMenu_Page, new UIMessage(UIMsgType.Res_MonthLabor));
                    callback?.Invoke();
                    break;
                default:
                    break;
            }

        }

        public void AddReputation(int num,ResourceAddType type,Action callback = null)
        {
            switch (type)
            {
                case ResourceAddType.current:
                    playerData.resourceData.AddReputation(num);
                    callback?.Invoke();
                    break;
                case ResourceAddType.max:
                    playerData.resourceData.AddReputationMax(num);
                    callback?.Invoke();
                    break;
                default:
                    break;
            }
        }


        public void AddMaterialData(int materialId, ushort count)
        {
            playerData.AddMaterialStoreData(materialId, count);
        }

        /// <summary>
        /// 获取仓库材料数量
        /// </summary>
        /// <param name="materialID"></param>
        /// <returns></returns>
        public int GetMaterialStoreCount(int materialID)
        {
            if (playerData.wareHouseInfo.materialStorageDataDic.ContainsKey(materialID))
            {
                return playerData.wareHouseInfo.materialStorageDataDic[materialID].count;
            }
            return 0;
        }
        #endregion

        public void AddUnLockBuildData(BuildingPanelData data)
        {
            if (!playerData.UnLockBuildingPanelDataList.Contains(data))
            {
                playerData.UnLockBuildingPanelDataList.Add(data);
            }
            //UpdateUI
            UIManager.Instance.SendMessageToWnd(UIPath.MainMenu_Page, new UIMessage(UIMsgType.UpdateBuildPanelData));
        }

        public void UpdateTime()
        {
            timer += Time.deltaTime;
            if (timer >= playerData.timeData.realSecondsPerMonth)
            {
                timer = 0;
                playerData.timeData.currentMonth++;
                //MonthSettle
                DoMonthSettle();
                if (playerData.timeData.currentMonth >= 13)
                {
                    playerData.timeData.currentMonth = 1;
                    playerData.timeData.currentYear++;
                }
                playerData.timeData.currentSeason = PlayerModule.ConvertMonthToSeason(playerData.timeData.currentMonth);
                UIManager.Instance.SendMessageToWnd(UIPath.MainMenu_Page, new UIMessage(UIMsgType.UpdateTime));
            }
        }

        /// <summary>
        /// 获取当前时间
        /// </summary>
        /// <returns></returns>
        public TimeData getCurrentTime()
        {
            return playerData.timeData;
        }

        public int GetCurrentYearTime()
        {
            return playerData.timeData.currentYear;
        }
        public ushort GetCurrentMonthTime()
        {
            return playerData.timeData.currentMonth;
        }


        /// <summary>
        /// 月底结算
        /// </summary>
        private void DoMonthSettle()
        {
            AddFood(playerData.resourceData.FoodPerMonth, ResourceAddType.current);
            AddEnergy(playerData.resourceData.EnergyPerMonth, ResourceAddType.current);
            AddLabor(playerData.resourceData.LaborPerMonth, ResourceAddType.current);
            GlobalEventManager.Instance.DoPlayerOrderMonthSettle();
            
        }

        
    }
}