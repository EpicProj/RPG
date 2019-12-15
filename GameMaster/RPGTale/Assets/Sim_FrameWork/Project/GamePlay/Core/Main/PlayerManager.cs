﻿using System.Collections;
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
        public MaterialStorageData _storageData;

        /// <summary>
        /// Time Manager
        /// </summary>
        private float timer;


        protected override void Awake()
        {
            base.Awake();
            playerData = PlayerModule.Instance.InitPlayerData();
            _storageData = new MaterialStorageData();
            //For Test
            AddMaterialData(100, 10);
            AddMaterialData(101, 500);
        }

        private void Start()
        {
            GameManager.Instance.InitBaseData();
            UIManager.Instance.PopUpWnd(UIPath.WindowPath.MainMenu_Page, WindowType.Page, true, playerData);
        }

        private void Update()
        {
            if(GameManager.Instance.gameStates== GameManager.GameStates.Start)
            {
                UpdateTime();
            }

            ///Test
            if (Input.GetKeyDown(KeyCode.R))
            {
                UIGuide.Instance.ShowRandomEventDialog(1);
            }
          
        }


        #region Resource Manager
        public void AddCurrency(int num, ResourceAddType type, Action callback = null)
        {
            switch (type)
            {
                case ResourceAddType.current:
                    playerData.resourceData.AddCurrency(num);
                    UIManager.Instance.SendMessage(new UIMessage(UIMsgType.Res_Currency));
                    callback?.Invoke();
                    break;
                case ResourceAddType.max:
                    playerData.resourceData.AddCurrencyMax(num);
                    callback?.Invoke();
                    break;
                case ResourceAddType.month:
                    playerData.resourceData.AddCurrencyPerMonth(num);
                    UIManager.Instance.SendMessage(new UIMessage(UIMsgType.Res_MonthCurrency));
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
                    UIManager.Instance.SendMessage(new UIMessage(UIMsgType.Res_Energy));
                    callback?.Invoke();
                    break;
                case ResourceAddType.max:
                    playerData.resourceData.AddEnergyMax(num);
                    callback?.Invoke();
                    break;
                case ResourceAddType.month:
                    playerData.resourceData.AddEnergyPerMonth(num);
                    UIManager.Instance.SendMessage(new UIMessage(UIMsgType.Res_MonthEnergy));
                    callback?.Invoke();
                    break;
                default:
                    break;
            }

        }
        public void AddResearch(float num, ResourceAddType type, Action callback = null)
        {
            switch (type)
            {
                case ResourceAddType.current:
                    playerData.resourceData.AddResearch(num);
                    UIManager.Instance.SendMessage(new UIMessage(UIMsgType.Res_Research));
                    callback?.Invoke();
                    break;
                case ResourceAddType.max:
                    playerData.resourceData.AddResearchMax(num);
                    callback?.Invoke();
                    break;
                case ResourceAddType.month:
                    playerData.resourceData.AddResearchPerMonth(num);
                    UIManager.Instance.SendMessage(new UIMessage(UIMsgType.Res_MonthResearch));
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

        public void AddBuilder(ushort num,ResourceAddType type,Action callback = null)
        {
            switch (type)
            {
                case ResourceAddType.current:
                    playerData.resourceData.AddBuilder(num);
                    callback?.Invoke();
                    break;
                case ResourceAddType.max:
                    playerData.resourceData.AddBuilderMax(num);
                    callback?.Invoke();
                    break;
                default:
                    break;
            }
        }

        public void AddRoCore(ushort num,ResourceAddType type,Action callback = null)
        {
            switch (type)
            {
                case ResourceAddType.current:
                    playerData.resourceData.AddRoCore(num);
                    callback?.Invoke();
                    break;
                case ResourceAddType.max:
                    playerData.resourceData.AddRoCoreMax(num);
                    callback?.Invoke();
                    break;
                default:
                    break;
            }
        }

        public void AddMaterialData(int materialId, ushort count)
        {
            _storageData.AddMaterialStoreData(materialId, count);
        }

        /// <summary>
        /// 获取仓库材料数量
        /// </summary>
        /// <param name="materialID"></param>
        /// <returns></returns>
        public int GetMaterialStoreCount(int materialID)
        {
            if (_storageData.materialStorageDataDic.ContainsKey(materialID))
            {
                return _storageData.materialStorageDataDic[materialID].count;
            }
            return 0;
        }
        #endregion

        #region BlockBuild Manager

        /// <summary>
        /// 增加解锁建筑
        /// </summary>
        /// <param name="data"></param>
        public void AddUnLockBuildData(BuildingPanelData data)
        {
            if (!playerData.UnLockBuildingPanelDataList.Contains(data))
            {
                playerData.UnLockBuildingPanelDataList.Add(data);
            }
            //UpdateUI
            UIManager.Instance.SendMessageToWnd(UIPath.WindowPath.MainMenu_Page, new UIMessage(UIMsgType.MenuPage_Add_Build));
        }

        public List<BuildingPanelData> GetBuildDataByMainType(FunctionBlockType.Type type)
        {
            List<BuildingPanelData> result = new List<BuildingPanelData>();
            for(int i = 0; i < playerData.UnLockBuildingPanelDataList.Count; i++)
            {
                var blockType = FunctionBlockModule.GetFunctionBlockType(playerData.UnLockBuildingPanelDataList[i].FunctionBlockID);
                if (blockType == type)
                {
                    result.Add(playerData.UnLockBuildingPanelDataList[i]);
                }
            }
            return result;
        }

        public List<List<BaseDataModel>> GetBuildPanelModelData(FunctionBlockType.Type type)
        {
            List<List<BaseDataModel>> result = new List<List<BaseDataModel>>();
           
            for (int i = 0; i < playerData.UnLockBuildingPanelDataList.Count; i++)
            {
                if (FunctionBlockModule.GetFunctionBlockType(playerData.UnLockBuildingPanelDataList[i].FunctionBlockID) == type)
                {
                    BuildPanelModel model = new BuildPanelModel();
                    if (model.Create(playerData.UnLockBuildingPanelDataList[i].BuildID))
                    {
                        result.Add(new List<BaseDataModel>() { model });
                    }
                }
            }
            return result;
        }

        #endregion

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
                UIManager.Instance.SendMessageToWnd(UIPath.WindowPath.MainMenu_Page, new UIMessage(UIMsgType.UpdateTime));
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
            AddEnergy(playerData.resourceData.EnergyPerMonth, ResourceAddType.current);
            AddResearch(playerData.resourceData.ResearchPerMonth, ResourceAddType.current);
            AddCurrency(playerData.resourceData.CurrencyPerMonth, ResourceAddType.current);
            GlobalEventManager.Instance.DoPlayerOrderMonthSettle();
            
        }

        
    }
}