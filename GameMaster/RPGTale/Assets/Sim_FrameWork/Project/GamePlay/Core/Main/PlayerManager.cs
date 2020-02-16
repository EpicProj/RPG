using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
/*
 * PlayerManager
 * SOMA
 * 
 */
namespace Sim_FrameWork
{
    public enum ResourceType
    {
        All,
        Currency,
        Research,
        Energy,
        AIRobot_Maintenance,
        AIRobot_Builder,
        AIRobot_Operator,
        RoCore,
    }
    public class PlayerManager : Singleton<PlayerManager>
    {
        public PlayerData playerData;

        private float updateFrequency = 1.0f;
        private int currentSettleIndex = 0;

        public void InitPlayerData()
        {
            playerData = new PlayerData();
            if (playerData.InitData() == false)
            {
                DebugPlus.LogError("[PlayerManager] : PlayerData Init Fail!");
            }
            InitAssembleData();

            updateFrequency = playerData.timeData.realSecondsPerDay / Config.GlobalConfigData.StateUpdateTimeUnit;
            if (updateFrequency < Config.GlobalConfigData.StateUpdateTimeUnit)
                updateFrequency = Config.GlobalConfigData.StateUpdateTimeUnit;
        }

        void InitAssembleData()
        {
            InitAssembleShipPresetUnlockState();
            InitUnlockAssembleShipList();
        }

        public void LoadGameSaveData(GameSaveData saveData)
        {
            playerData = new PlayerData();
            playerData.LoadPlayerSaveData(saveData.playerSaveData, saveData.assembleSaveData.partSaveData);
        }


        #region Resource Manager
        /// <summary>
        /// Currency
        /// </summary>
        /// <param name="num"></param>
        public void AddCurrency_Current(int num)
        {
            playerData.resourceData.AddCurrency(num);
            UIManager.Instance.SendMessage(new UIMessage(UIMsgType.Res_Currency));
        }
        public void AddCurrency_Max(ModifierDetailRootType_Simple rootType,int num)
        {
            playerData.resourceData.AddCurrencyMax(rootType,num);
        }
        public void AddCurrency_PerDay(ModifierDetailRootType_Simple rootType,int num)
        {
            playerData.resourceData.AddCurrencyPerDay(rootType,num);
            UIManager.Instance.SendMessage(new UIMessage(UIMsgType.Res_DailyCurrency));
        }

        /// <summary>
        /// Energy
        /// </summary>
        /// <param name="num"></param>
        public void AddEnergy_Current(float num)
        {
            playerData.resourceData.AddEnergy(num);
            UIManager.Instance.SendMessage(new UIMessage(UIMsgType.Res_Energy));
        }
        public void AddEnergy_Max(ModifierDetailRootType_Simple rootType,float num)
        {
            playerData.resourceData.AddEnergyMax(rootType, num);
        }
        public void AddEnergy_PerDay(ModifierDetailRootType_Simple rootType,float num,bool CoverData)
        {
            playerData.resourceData.AddEnergyPerDay(rootType,num,CoverData);
            UIManager.Instance.SendMessage(new UIMessage(UIMsgType.Res_DailyEnergy));
        }

        /// <summary>
        /// Research
        /// </summary>
        /// <param name="num"></param>
        public void AddResearch_Current(float num)
        {
            playerData.resourceData.AddResearch(num);
            UIManager.Instance.SendMessage(new UIMessage(UIMsgType.Res_Research));
        }
        public void AddResearch_Max(ModifierDetailRootType_Simple rootType,float num)
        {
            playerData.resourceData.AddResearchMax(rootType,num);
        }
        public void AddResearch_PerDay(ModifierDetailRootType_Simple rootType,float num)
        {
            playerData.resourceData.AddResearchPerMonth(rootType,num);
            UIManager.Instance.SendMessage(new UIMessage(UIMsgType.Res_DailyResearch));
        }

        /// <summary>
        /// Builder
        /// </summary>
        /// <param name="num"></param>
        public void AddAIRobot_Maintenance(ushort num)
        {
            playerData.resourceData.AddAIRobot(ShipAIRobotType.Maintenance, num);
            UIManager.Instance.SendMessage(new UIMessage(UIMsgType.Res_AIRobot_Maintenance));
        }
        public void AddAIRobot_Builder(ushort num)
        {
            playerData.resourceData.AddAIRobot(ShipAIRobotType.Builder, num);
            UIManager.Instance.SendMessage(new UIMessage(UIMsgType.Res_AIRobot_Builder));
        }
        public void AddAIRobot_Operator(ushort num)
        {
            playerData.resourceData.AddAIRobot(ShipAIRobotType.Operator, num);
            UIManager.Instance.SendMessage(new UIMessage(UIMsgType.Res_AIRobot_Operator));
        }

        public void AddAIRobot_Maintenance_Max(ModifierDetailRootType_Simple rootType, ushort num)
        {
            playerData.resourceData.AddAIRobot_Maintenance_Max(rootType,num);
        }
        public void AddAIRobot_Builder_Max(ModifierDetailRootType_Simple rootType,ushort num)
        {
            playerData.resourceData.AddAIRobot_Builder_Max(rootType, num);
        }
        public void AddAIRobot_Operator_Max(ModifierDetailRootType_Simple rootType,ushort num)
        {
            playerData.resourceData.AddAIRobot_Operator_Max(rootType, num);
        }

        /// <summary>
        /// Ro Core
        /// </summary>
        /// <param name="num"></param>
        public void AddRoCore_Current(ushort num)
        {
            playerData.resourceData.AddRoCore(num);
            UIManager.Instance.SendMessage(new UIMessage(UIMsgType.Res_RoCore));
        }
        public void AddRoCore_Max(ModifierDetailRootType_Simple rootType,ushort num)
        {
            playerData.resourceData.AddRoCoreMax(rootType,num);
        }



        public void AddMaterialData(int materialId, ushort count)
        {
            playerData.materialStorageData.AddMaterialStoreData(materialId, count);
        }

        /// <summary>
        /// 获取仓库材料数量
        /// </summary>
        /// <param name="materialID"></param>
        /// <returns></returns>
        public int GetMaterialStoreCount(int materialID)
        {
            if (playerData.materialStorageData.materialStorageDataDic.ContainsKey(materialID))
            {
                return playerData.materialStorageData.materialStorageDataDic[materialID];
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

        public List<BuildingPanelData> GetBuildDataByMainType(FunctionBlockType type)
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

        public List<BaseDataModel> GetBuildPanelModelData(FunctionBlockType type)
        {
            List<BaseDataModel> result = new List<BaseDataModel>();
           
            for (int i = 0; i < playerData.UnLockBuildingPanelDataList.Count; i++)
            {
                if (FunctionBlockModule.GetFunctionBlockType(playerData.UnLockBuildingPanelDataList[i].FunctionBlockID) == type)
                {
                    BuildPanelModel model = new BuildPanelModel();
                    if (model.Create(playerData.UnLockBuildingPanelDataList[i].BuildID))
                    {
                        result.Add( model);
                    }
                }
            }
            return result;
        }

        #endregion

        #region Game Time Update
        public void UpdateTime()
        {
            playerData.timeData.timer += Time.deltaTime;

            if (playerData.timeData.timer > Config.GlobalConfigData.StateUpdateTimeUnit)
            {
                ///Do Settle Pool
                ApplicationManager.Instance.EnqueueTask(DoDailySettlePool_Resource());

                playerData.timeData.timer = 0;
                currentSettleIndex++;
                if (currentSettleIndex >= updateFrequency)
                {
                    
                    DateTime newTime = playerData.timeData.date.AddDays(1);
                    playerData.timeData.date = newTime;
                    currentSettleIndex = 0;
                    ///Daily Settle Queue
                    ApplicationManager.Instance.EnqueueTask(DoDailySettle_Resource());
                    ApplicationManager.Instance.EnqueueTask(DoDailySettle_Order());
   
                    UIManager.Instance.SendMessageToWnd(UIPath.WindowPath.MainMenu_Page, new UIMessage(UIMsgType.UpdateTime));
                    
                }
            }

           
        }

        IEnumerator DoDailySettlePool_Resource()
        {
            playerData.resourceData.UpdateEnergyDailySettlePool(updateFrequency);
            UIManager.Instance.SendMessage(new UIMessage(UIMsgType.Res_Daily_Total));
            yield return null;
        }

        void ResetDailySettlePool()
        {
            playerData.resourceData.ResetEnergyDailySettlePool();
        }

        /// <summary>
        /// 月底结算
        /// </summary>
        IEnumerator DoDailySettle_Resource()
        {
            AddEnergy_Current(playerData.resourceData.EnergyDailySettlePool);
            ResetDailySettlePool();
            UIManager.Instance.SendMessage(new UIMessage(UIMsgType.Res_Daily_Total));
            yield return null;
        }

        IEnumerator DoDailySettle_Order()
        {
            GlobalEventManager.Instance.DoPlayerOrderMonthSettle();
            yield return null;
        }


        /// <summary>
        /// 获取当前时间
        /// </summary>
        /// <returns></returns>
        public TimeData getCurrentTime()
        {
            return playerData.timeData;
        }





        #endregion

        #region Assmeble Part Manager

        public void AddAssemblePartDesign(AssemblePartInfo info)
        {
            playerData.assemblePartData.AddAssemblePartDesign(info);
        }

        public AssemblePartInfo GetAssemblePartDesignInfo(ushort uid)
        {
            return playerData.assemblePartData.GetAssemblePartDesignInfo(uid);
        }

        /// <summary>
        /// 检测名称是否重复
        /// </summary>
        /// <param name="partName"></param>
        /// <param name="customName"></param>
        /// <returns></returns>
        public bool CheckAssemblePartCustomNameRepeat(string partName, string customName)
        {
            return playerData.assemblePartData.CheckAssemblePartCustomNameRepeat(partName, customName);
        }
        public void AddUnlockAssemblePartID(int partID)
        {
            playerData.assemblePartData.AddUnlockAssemblePartID(partID);
        }

        public List<string> GetTotalUnlockAssemblePartTypeList()
        {
            return playerData.assemblePartData.GetTotalUnlockAssemblePartTypeList();
        }
        public List<Config.AssemblePartMainType> GetTotalUnlockAssembleTypeData()
        {
            return playerData.assemblePartData.GetTotalUnlockAssembleTypeData();
        }
        public void GetAssemblePartMainTypeData()
        {
        }

        public List<AssemblePartInfo> GetAssemblePartInfoByTypeID(string typeID)
        {
            return playerData.assemblePartData.GetAssemblePartInfoByTypeID(typeID);
        }

        public List<BaseDataModel> GetAssemblePartPresetModelList(string typeID)
        {
            return playerData.assemblePartData.GetAssemblePartPresetModelList(typeID);
        }

        /// <summary>
        /// Add Assmeble Part Storage
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public bool AddAssmebleStorageInfo(AssemblePartInfo info)
        {
            info.currentState = AssmblePartState.Storage;
            return playerData.assemblePartData.AddAssemblePartStorage(info);
        }

        /// <summary>
        /// Add part Equiped
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public bool AddAssmebleEquipedInfo(AssemblePartInfo info)
        {
            if (playerData.assemblePartData.isAssemblePartStorageExist(info.UID))
            {
                //Remove From Storage
                playerData.assemblePartData.RemoveAssemblePartStorage(info.UID);
                info.currentState = AssmblePartState.Equiped;
                playerData.assemblePartData.AddAssemblePartEquiped(info);
            }
            return false;
        }
        

        #endregion

        #region Assemble Ship Design

        /// <summary>
        /// 部件种类解锁情况
        /// </summary>
        public Dictionary<string, Config.AssembleShipMainType> AssembleShipMainTypeDic = new Dictionary<string, Config.AssembleShipMainType>();

        private void InitAssembleShipPresetUnlockState()
        {
            var configData = Config.ConfigData.AssembleConfig.assembleShipMainType;
            for (int i = 0; i < configData.Count; i++)
            {
                if (!AssembleShipMainTypeDic.ContainsKey(configData[i].Type))
                {
                    AssembleShipMainTypeDic.Add(configData[i].Type, configData[i]);
                }
            }
        }

        public Config.AssembleShipMainType GetAssembleShipPresetData(string type)
        {
            Config.AssembleShipMainType typeData = null;
            AssembleShipMainTypeDic.TryGetValue(type, out typeData);
            return typeData;
        }

        public void AssembleShipPresetSetUnlock(string type, bool unlock)
        {
            var data = GetAssembleShipPresetData(type);
            if (data != null)
            {
                data.DefaultUnlock = unlock;
            }
        }

        /// <summary>
        /// 获取所有解锁的舰船类型
        /// </summary>
        /// <returns></returns>
        public List<Config.AssembleShipMainType> GetTotalUnlockAssembleShipTypeData()
        {
            List<Config.AssembleShipMainType> result = new List<Config.AssembleShipMainType>();
            foreach (var data in AssembleShipMainTypeDic)
            {
                if (data.Value.DefaultUnlock == true)
                    result.Add(data.Value);
            }
            return result;
        }

        public List<string> GetTotalUnlockAssembleShipTypeList()
        {
            List<string> result = new List<string>();
            foreach(var type in AssembleShipMainTypeDic)
            {
                result.Add(type.Key);
            }
            return result;
        }

        /// <summary>
        /// 已解锁部件模板信息
        /// </summary>
        private List<int> _currentUnlockShipList = new List<int>();
        public List<int> CurrentUnlockShipList
        {
            get { return _currentUnlockShipList; }
        }

        private void InitUnlockAssembleShipList()
        {
            _currentUnlockShipList = AssembleModule.GetAllUnlockShipPresetID();
        }

        public void AddUnlockAssembleShipID(int shipID)
        {
            if (!_currentUnlockShipList.Contains(shipID))
            {
                if (AssembleModule.GetWarshipDataByKey(shipID) != null)
                    _currentUnlockShipList.Add(shipID);
            }
        }


        public List<int> GetUnlockAssembleShipTypeListByTypeID(string typeID)
        {
            List<int> result = new List<int>();
            for (int i = 0; i < _currentUnlockShipList.Count; i++)
            {
                var meta = AssembleModule.GetWarshipDataByKey(_currentUnlockShipList[i]);
                if (meta != null)
                {
                    if (meta.MainType == typeID)
                        result.Add(_currentUnlockShipList[i]);
                }
            }
            return result;
        }
        public List<int> GetUnlockAssembleShipTypeListByTypeIDList(List<string> typeIDList)
        {
            List<int> result = new List<int>();
            for (int i = 0; i < typeIDList.Count; i++)
            {
                for (int j = 0; j < _currentUnlockShipList.Count; j++)
                {
                    var meta = AssembleModule.GetWarshipDataByKey(_currentUnlockShipList[j]);
                    if (meta != null)
                    {
                        if (meta.MainType == typeIDList[i])
                            result.Add(_currentUnlockShipList[j]);
                    }
                }
            }
            return result;
        }


        private Dictionary<ushort, AssembleShipInfo> _assembleShipDesignDataDic = new Dictionary<ushort, AssembleShipInfo>();
        public Dictionary<ushort,AssembleShipInfo> AssembleShipDesignDataDic
        {
            get { return _assembleShipDesignDataDic; }
        }

        public AssembleShipInfo GetAssembleShipInfo(ushort UID)
        {
            AssembleShipInfo info = null;
            _assembleShipDesignDataDic.TryGetValue(UID, out info);
            return info;
        }

        public void AddAssembleShipDesign(AssembleShipInfo info)
        {
            ushort guid = getShipUnUsedInstanceID();
            info.UID = guid;
            _assembleShipDesignDataDic.Add(guid, info);
        }

        private ushort getShipUnUsedInstanceID()
        {
            ushort instanceId = (ushort)UnityEngine.Random.Range(ushort.MinValue, ushort.MaxValue);
            if (_assembleShipDesignDataDic.ContainsKey(instanceId))
            {
                return getShipUnUsedInstanceID();
            }
            return instanceId;
        }

        /// <summary>
        /// GetModelList
        /// </summary>
        /// <param name="typeIDList"></param>
        /// <returns></returns>
        public List<BaseDataModel> GetAssembleShipPresetModelList(List<string> typeIDList)
        {
            List<BaseDataModel> result = new List<BaseDataModel>();

            var list = GetUnlockAssembleShipTypeListByTypeIDList(typeIDList);
            for (int i = 0; i < list.Count; i++)
            {
                AssembleShipTypePresetModel model = new AssembleShipTypePresetModel();
                if (model.Create(list[i]))
                {
                    result.Add(model);
                }
            }
            return result;
        }
        public List<BaseDataModel> GetAssembleShipPresetModelList(string typeID)
        {
            List<BaseDataModel> result = new List<BaseDataModel>();

            var list = GetUnlockAssembleShipTypeListByTypeID(typeID);
            for (int i = 0; i < list.Count; i++)
            {
                AssembleShipTypePresetModel model = new AssembleShipTypePresetModel();
                if (model.Create(list[i]))
                {
                    result.Add(model);
                }
            }
            return result;
        }

        /// <summary>
        /// 检测自定义名字是否重复
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public bool CheckAssembleShipCustomNameRepeat(string shipClassName, string customName)
        {
            foreach (var part in _assembleShipDesignDataDic)
            {
                if (part.Value.presetData.shipClassName == shipClassName && part.Value.customData.customNameText == customName)
                    return true;
            }
            return false;
        }

        #endregion

        #region SaveData

        public void LoadAssembleShipSaveData()
        {
            //var saveData = GameDataSaveManager.Instance.currentSaveData.assembleSaveData;
            //if (saveData != null)
            //{
            //    for(int i = 0; i < saveData.shipSaveData.currentSaveShip.Count; i++)
            //    {
            //        AssembleShipInfo shipInfo = new AssembleShipInfo();
            //        shipInfo.LoadSaveData(saveData.shipSaveData.currentSaveShip[i]);

            //        AddAssembleShipDesign(shipInfo);
            //    }
            //}
        }

        #endregion

    }
    #region AssembleSaveData
    /// <summary>
    /// Assemble Ship
    /// </summary>
    public class AssembleShipGeneralSaveData
    {
        public List<AssembleShipSingleSaveData> currentSaveShip;
        public List<string> currentUnlockShipTypeList;

        public static AssembleShipGeneralSaveData CreateSave()
        {
            AssembleShipGeneralSaveData data = new AssembleShipGeneralSaveData();
            data.currentSaveShip = new List<AssembleShipSingleSaveData>();
            foreach(var info in PlayerManager.Instance.AssembleShipDesignDataDic.Values)
            {
                var singleSave = info.CreateSaveData();
                data.currentSaveShip.Add(singleSave);
            }

            data.currentUnlockShipTypeList = PlayerManager.Instance.GetTotalUnlockAssembleShipTypeList();

            return data;
        }
    }
    #endregion

}