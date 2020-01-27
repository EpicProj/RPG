using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Sim_FrameWork
{
    public enum ResourceType
    {
        All,
        Currency,
        Research,
        Energy,
        Builder,
        RoCore,
    }
    public enum ResourceAddType
    {
        current,
        month,
        max
    }
    public class PlayerManager : Singleton<PlayerManager>
    {
        public PlayerData playerData;
        public MaterialStorageData _storageData;
        /// <summary>
        /// Time Manager
        /// </summary>
        private float timer;

        public void InitPlayerData()
        {
            playerData = PlayerModule.Instance.InitPlayerData();
            _storageData = new MaterialStorageData();
            InitAssembleData();
        }

        void InitAssembleData()
        {
            InitAssemblePartTypeUnlockState();
            InitAssembleShipPresetUnlockState();
            InitUnlockAssemblePartList();
            InitUnlockAssembleShipList();
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
                    UIManager.Instance.SendMessage(new UIMessage(UIMsgType.Res_Builder));
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
                    UIManager.Instance.SendMessage(new UIMessage(UIMsgType.Res_RoCore));
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
            if (timer >= playerData.timeData.realSecondsPerDay)
            {
                int currentMonth = playerData.timeData.date.Month;
                timer = 0;
                DateTime newTime= playerData.timeData.date.AddDays(1);
                playerData.timeData.date = newTime;
                if(playerData.timeData.date.Month!= currentMonth)
                {
                    //MonthSettle
                    DoMonthSettle();
                }
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


        #region Assemble Part Design

        /// <summary>
        /// 部件种类解锁情况
        /// </summary>
        public Dictionary<string, Config.AssemblePartMainType> AssemblePartMainTypeDic = new Dictionary<string, Config.AssemblePartMainType>();

        private void InitAssemblePartTypeUnlockState()
        {
            var configData = Config.ConfigData.AssembleConfig.assemblePartMainType;
            for(int i = 0; i < configData.Count; i++)
            {
                if (!AssemblePartMainTypeDic.ContainsKey(configData[i].Type))
                {
                    AssemblePartMainTypeDic.Add(configData[i].Type, configData[i]);
                }
            }
        }

        public Config.AssemblePartMainType GetAssemblePartMainTypeData(string type)
        {
            Config.AssemblePartMainType typeData = null;
            AssemblePartMainTypeDic.TryGetValue(type, out typeData);
            return typeData;
        }

        public void AssemblePartTypeSetUnlock(string type, bool unlock)
        {
            var data = GetAssemblePartMainTypeData(type);
            if (data != null)
            {
                data.DefaultUnlock = unlock;
            }
        }

        /// <summary>
        /// 获取所有解锁的部件类型
        /// </summary>
        /// <returns></returns>
        public List<Config.AssemblePartMainType> GetTotalUnlockAssembleTypeData()
        {
            List<Config.AssemblePartMainType> result = new List<Config.AssemblePartMainType>();
            foreach (var data in AssemblePartMainTypeDic)
            {
                if (data.Value.DefaultUnlock == true)
                    result.Add(data.Value);
            }
            return result;
        }

        public List<string> GetTotalUnlockAssemblePartTypeList()
        {
            List<string> list = new List<string>();
            foreach(var type in AssemblePartMainTypeDic)
            {
                list.Add(type.Key);
            }
            return list;
        }

        /// <summary>
        /// 已解锁部件模板信息
        /// </summary>
        private List<int> _currentUnlockPartList = new List<int>();
        public List<int> CurrentUnlockPartList
        {
            get { return _currentUnlockPartList; }
        }

        private void InitUnlockAssemblePartList()
        {
            _currentUnlockPartList = AssembleModule.GetAllUnlockPartTypeID();
        }

        /// <summary>
        /// GetModelList
        /// </summary>
        /// <param name="typeIDList"></param>
        /// <returns></returns>
        public List<List<BaseDataModel>> GetAssemblePartPresetModelList(List<string> typeIDList)
        {
            List<List<BaseDataModel>> result = new List<List<BaseDataModel>>();

            var list = GetUnlockAssemblePartTypeListByTypeIDList(typeIDList);
            for (int i = 0; i < list.Count; i++)
            {
                AssembleTypePresetModel model = new AssembleTypePresetModel();
                if (model.Create(list[i]))
                {
                    result.Add(new List<BaseDataModel>() { model });
                }
            }
            return result;
        }
        public List<List<BaseDataModel>> GetAssemblePartPresetModelList(string typeID)
        {
            List<List<BaseDataModel>> result = new List<List<BaseDataModel>>();

            var list = GetUnlockAssemblePartTypeListByTypeID(typeID);
            for (int i = 0; i < list.Count; i++)
            {
                AssembleTypePresetModel model = new AssembleTypePresetModel();
                if (model.Create(list[i]))
                {
                    result.Add(new List<BaseDataModel>() { model });
                }
            }
            return result;
        }

        public void AddUnlockAssemblePartID(int partID)
        {
            if(! _currentUnlockPartList.Contains(partID))
            {
                if (AssembleModule.GetAssemblePartDataByKey(partID) != null)
                    _currentUnlockPartList.Add(partID);
            }
        }

        public bool CheckAssemblePartIDUnlock(int partID)
        {
            return _currentUnlockPartList.Contains(partID);
        }

        public List<int> GetUnlockAssemblePartTypeListByTypeID(string typeID)
        {
            List<int> result = new List<int>();
            for(int i = 0; i < _currentUnlockPartList.Count; i++)
            {
                var meta = AssembleModule.GetAssemblePartDataByKey(_currentUnlockPartList[i]);
                if (meta != null)
                {
                    var typeMeta = AssembleModule.GetAssemblePartTypeByKey(meta.ModelTypeID);
                    if (typeMeta != null)
                    {
                        if (typeMeta.TypeID == typeID)
                            result.Add(_currentUnlockPartList[i]);
                    }
                }
            }
            return result;
        }
        public List<int> GetUnlockAssemblePartTypeListByTypeIDList(List<string> typeIDList)
        {
            List<int> result = new List<int>();
            for(int i = 0; i < typeIDList.Count; i++)
            {
                for(int j = 0; j < _currentUnlockPartList.Count; j++)
                {
                    var meta = AssembleModule.GetAssemblePartDataByKey(_currentUnlockPartList[j]);
                    if (meta != null)
                    {
                        var TypeMeta = AssembleModule.GetAssemblePartTypeByKey(meta.ModelTypeID);
                        if (TypeMeta != null)
                        {
                            if (TypeMeta.TypeID == typeIDList[i])
                                result.Add(_currentUnlockPartList[j]);
                        }
                    }
                }
            }
            return result;
        }


        private Dictionary<ushort, AssemblePartInfo> _assemblePartDesignDataDic=new Dictionary<ushort, AssemblePartInfo> ();
        public Dictionary<ushort,AssemblePartInfo> AssemblePartDesignDataDic
        {
            get { return _assemblePartDesignDataDic; }
        }

        public AssemblePartInfo GetAssemblePartInfo(ushort uid)
        {
            AssemblePartInfo info = null;
            _assemblePartDesignDataDic.TryGetValue(uid, out info);
            if (info == null)
                Debug.LogError("Get Assemble PartInfo Empty, UID=" + uid);
            return info;
        }


        /// <summary>
        /// 根据类型获取全部件[已设计部件]
        /// </summary>
        /// <param name="typeID"></param>
        /// <returns></returns>
        public List<AssemblePartInfo> GetAssemblePartInfoByTypeID(string typeID)
        {
            List<AssemblePartInfo> result = new List<AssemblePartInfo>();
            foreach(var info in _assemblePartDesignDataDic)
            {
                if (info.Value.typePresetData.TypeID == typeID)
                {
                    result.Add(info.Value);
                }
            }
            return result;
        }

        public List<AssemblePartInfo> GetAssemblePartInfoByTypeList(List<string> typeList)
        {
            List<AssemblePartInfo> result = new List<AssemblePartInfo>();
            for(int i = 0; i < typeList.Count; i++)
            {
                result.AddRange(GetAssemblePartInfoByTypeID(typeList[i]));
            }
            return result;
        }

        public List<List<BaseDataModel>> GetAssemblePartChooseModel(string typeID)
        {
            List<List<BaseDataModel>> result = new List<List<BaseDataModel>>();

            var list = GetAssemblePartInfoByTypeID(typeID);
            for (int i = 0; i < list.Count; i++)
            {
                AssembleChooseItemModel model = new AssembleChooseItemModel();
                if (model.Create(list[i].UID))
                {
                    result.Add(new List<BaseDataModel>() { model });
                }
            }
            return result;
        }


        public void AddAssemblePartDesign(AssemblePartInfo info)
        {
            ushort guid = getPartUnUsedInstanceID();
            info.UID = guid;
            _assemblePartDesignDataDic.Add(guid, info);
        }

        private ushort getPartUnUsedInstanceID()
        {
            ushort instanceId = (ushort)UnityEngine.Random.Range(ushort.MinValue, ushort.MaxValue);
            if (_assemblePartDesignDataDic.ContainsKey(instanceId))
            {
                return getPartUnUsedInstanceID();
            }
            return instanceId;
        }

        /// <summary>
        /// 检测自定义名字是否重复
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public bool CheckAssemblePartCustomNameRepeat(string partName,string customName)
        {
            foreach(var part in _assemblePartDesignDataDic)
            {
                if (part.Value.typePresetData.partName == partName && part.Value.customDataInfo.partNameCustomText == customName)
                    return true;
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
            if (!_currentUnlockPartList.Contains(shipID))
            {
                if (AssembleModule.GetWarshipDataByKey(shipID) != null)
                    _currentUnlockPartList.Add(shipID);
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
        public List<List<BaseDataModel>> GetAssembleShipPresetModelList(List<string> typeIDList)
        {
            List<List<BaseDataModel>> result = new List<List<BaseDataModel>>();

            var list = GetUnlockAssembleShipTypeListByTypeIDList(typeIDList);
            for (int i = 0; i < list.Count; i++)
            {
                AssembleShipTypePresetModel model = new AssembleShipTypePresetModel();
                if (model.Create(list[i]))
                {
                    result.Add(new List<BaseDataModel>() { model });
                }
            }
            return result;
        }
        public List<List<BaseDataModel>> GetAssembleShipPresetModelList(string typeID)
        {
            List<List<BaseDataModel>> result = new List<List<BaseDataModel>>();

            var list = GetUnlockAssembleShipTypeListByTypeID(typeID);
            for (int i = 0; i < list.Count; i++)
            {
                AssembleShipTypePresetModel model = new AssembleShipTypePresetModel();
                if (model.Create(list[i]))
                {
                    result.Add(new List<BaseDataModel>() { model });
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

    }
}