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
            UIManager.Instance.PopUpWnd(UIPath.MainMenu_Page, true, playerData);
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
                    UIManager.Instance.SendMessageToWnd(UIPath.MainMenu_Page, new UIMessage(UIMsgType.Res_Currency, playerData.resourceData));
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
                    UIManager.Instance.SendMessageToWnd(UIPath.MainMenu_Page, new UIMessage(UIMsgType.Res_Food, playerData.resourceData));
                    callback?.Invoke();
                    break;
                case ResourceAddType.max:
                    playerData.resourceData.AddFoodMax(num);
                    callback?.Invoke();
                    break;
                case ResourceAddType.month:
                    playerData.resourceData.AddFoodPerMonth(num);
                    UIManager.Instance.SendMessageToWnd(UIPath.MainMenu_Page, new UIMessage(UIMsgType.Res_MonthFood, playerData.resourceData));
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
                    UIManager.Instance.SendMessageToWnd(UIPath.MainMenu_Page, new UIMessage(UIMsgType.Res_Energy, playerData.resourceData));
                    callback?.Invoke();
                    break;
                case ResourceAddType.max:
                    playerData.resourceData.AddEnergyMax(num);
                    callback?.Invoke();
                    break;
                case ResourceAddType.month:
                    playerData.resourceData.AddEnergyPerMonth(num);
                    UIManager.Instance.SendMessageToWnd(UIPath.MainMenu_Page, new UIMessage(UIMsgType.Res_MonthEnergy, playerData.resourceData));
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
                    UIManager.Instance.SendMessageToWnd(UIPath.MainMenu_Page, new UIMessage(UIMsgType.Res_Labor, playerData.resourceData));
                    callback?.Invoke();
                    break;
                case ResourceAddType.max:
                    playerData.resourceData.AddLaborMax(num);
                    callback?.Invoke();
                    break;
                case ResourceAddType.month:
                    playerData.resourceData.AddLaborPerMonth(num);
                    UIManager.Instance.SendMessageToWnd(UIPath.MainMenu_Page, new UIMessage(UIMsgType.Res_MonthLabor, playerData.resourceData));
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
        #endregion

        public void AddUnLockBuildData(BuildingPanelData data)
        {
            if (!playerData.UnLockBuildingPanelDataList.Contains(data))
            {
                playerData.UnLockBuildingPanelDataList.Add(data);
            }
            //UpdateUI
            UIManager.Instance.SendMessageToWnd(UIPath.MainMenu_Page, new UIMessage(UIMsgType.UpdateBuildPanelData, playerData.UnLockBuildingPanelDataList));
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
                UIManager.Instance.SendMessageToWnd(UIPath.MainMenu_Page, new UIMessage(UIMsgType.UpdateTime, playerData.timeData));
            }
        }


        /// <summary>
        /// 月底结算
        /// </summary>
        private void DoMonthSettle()
        {
            AddFood(playerData.resourceData.FoodPerMonth, ResourceAddType.current);
            AddEnergy(playerData.resourceData.EnergyPerMonth, ResourceAddType.current);
            AddLabor(playerData.resourceData.LaborPerMonth, ResourceAddType.current);

        }

    }
}