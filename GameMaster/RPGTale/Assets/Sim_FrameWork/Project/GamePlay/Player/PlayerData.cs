using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Sim_FrameWork
{
    public enum ShipAIRobotType
    {
        None,
        Builder,
        Operator,
        Maintenance
    }

    public class PlayerData 
    {

        public List<BuildingPanelData> UnLockBuildingPanelDataList = new List<BuildingPanelData>();

        public PlayerResourceData resourceData;
        public PlayerAssemblePartData assemblePartData;
        public MaterialStorageData materialStorageData;

        public TimeData timeData;

        public PlayerData() { }
        public bool InitData()
        {
            var config = Config.ConfigData.PlayerConfig;
            if (config == null || config.timeConfig == null)
            {
                DebugPlus.LogError("[PlayerData] : playerConfig is null!");
                return false;
            }

            timeData = TimeData.InitData(config.timeConfig);

            resourceData = PlayerResourceData.InitData();

            assemblePartData = PlayerAssemblePartData.InitData();

            materialStorageData = new MaterialStorageData();
            //Init BuildPanel
            UnLockBuildingPanelDataList = PlayerModule.GetUnLockBuildData();
            return true;
        }
        
        /// <summary>
        /// Game Save
        /// </summary>
        /// <param name="saveData"></param>
        public void LoadPlayerSaveData(PlayerSaveData saveData,AssemblePartGeneralSaveData partSaveData)
        {
            resourceData = PlayerResourceData.LoadSave(saveData.playerSaveData_Resource);

            timeData = TimeData.LoadGameSave(saveData.timeSave);

            materialStorageData = MaterialStorageData.LoadSaveData(saveData.materialSaveData);
            assemblePartData = PlayerAssemblePartData.LoadSaveData(partSaveData);
        }
    }

    public class PlayerResourceData
    {
        #region Currency
        //当前货币
        private int _currency;
        public int Currency { get { return _currency; } protected set { } }

        //Add Currenct
        public void AddCurrency(int num)
        {
            _currency += num;
            if (_currency > _currencyMax)
                _currency = _currencyMax;
        }

        //最大货币储量
        private int _currencyMax;
        public int CurrencyMax { get { return _currencyMax; } protected set { } }

        public ModifierDetailPackage currencyMaxDetailPac = new ModifierDetailPackage();

        public void AddCurrencyMax(ModifierDetailRootType_Simple rootType, int num)
        {
            currencyMaxDetailPac.ValueChange(rootType, num);
            _currencyMax += num;
            if (_currencyMax < 0)
                _currencyMax = 0;
        }

        private int _currencyPerDay;
        public int CurrencyPerDay { get { return _currencyPerDay; } protected set { } }

        public ModifierDetailPackage currencyPerDayDetailPac = new ModifierDetailPackage();
        public void AddCurrencyPerDay(ModifierDetailRootType_Simple rootType, int num)
        {
            currencyPerDayDetailPac.ValueChange(rootType, num);
            _currencyPerDay += num;
        }
        #endregion

        #region Research
        //当前研究点
        private float _research;
        public float Research { get { return _research; } }
        //Add research
        public void AddResearch(float num)
        {
            _research += num;
            if (_research > _researchMax)
                _research = _researchMax;
        }

        //研究点最大值
        private float _researchMax;
        public float ResearchMax { get { return _researchMax; } }

        public ModifierDetailPackage researchMaxDetailPac = new ModifierDetailPackage();
        public void AddResearchMax(ModifierDetailRootType_Simple rootType, float num)
        {
            researchMaxDetailPac.ValueChange(rootType, num);
            _researchMax += num;
            if (_researchMax < 0)
                _researchMax = 0;
        }

        private float _researchPerDay;
        public float ResearchPerDay { get { return _researchPerDay; } }

        public ModifierDetailPackage researchPerDayDetailPac = new ModifierDetailPackage();

        public void AddResearchPerMonth(ModifierDetailRootType_Simple rootType, float num)
        {
            researchPerDayDetailPac.ValueChange(rootType, num);
            _researchPerDay += num;
        }
        #endregion

        #region Energy
        //当前能源
        private float _energy;
        public float Energy { get { return _energy; } }
        //Add Energy
        public void AddEnergy(float num)
        {
            _energy += num;
            if (_energy >= _energyMax)
                _energy = _energyMax;
        }

        //能源最大值
        private float _energyMax;
        public float EnergyMax { get { return _energyMax; } }

        public ModifierDetailPackage energyMaxDetailPac = new ModifierDetailPackage();

        public void AddEnergyMax(ModifierDetailRootType_Simple rootType, float num)
        {
            energyMaxDetailPac.ValueChange(rootType, num);
            _energyMax += num;
            if (_energyMax < 0)
                _energyMax = 0;
        }

        /// <summary>
        /// Daily Settle
        /// </summary>
        public float EnergyDailySettlePool;
        public void ResetEnergyDailySettlePool()
        {
            EnergyDailySettlePool = 0;
        }
        public void UpdateEnergyDailySettlePool(float totalFrequency)
        {
            float result = 0;
            foreach (var item in energyPerDayDetailPac.detailDic.Values)
                result += item.value / totalFrequency;
            EnergyDailySettlePool += result;
        }

        public int EnergyDailySettleDisplay
        {
            get
            {
                int result = 0;
                foreach (var item in energyPerDayDetailPac.detailDic.Values)
                    result += (int)item.value;
                return result;
            }
        }

        public ModifierDetailPackage energyPerDayDetailPac = new ModifierDetailPackage();

        public void AddEnergyPerDay(ModifierDetailRootType_Simple rootType, float num,bool CoverData)
        {
            energyPerDayDetailPac.ValueChange(rootType, num,CoverData);
        }

        #endregion

        #region AIRobot
        public Dictionary<ShipAIRobotType, ushort> AIRobotInfo = new Dictionary<ShipAIRobotType, ushort>();
        public int AIRobotTotalNum
        {
            get
            {
                int num = 0;
                foreach (var item in AIRobotInfo.Values)
                    num += item;
                return num;
            }
        }

        public void AddAIRobot(ShipAIRobotType type,ushort num)
        {
            if (!AIRobotInfo.ContainsKey(type))
                return;
            AIRobotInfo[type] += num;
            if (type== ShipAIRobotType.Builder)
            {
                if (AIRobotInfo[type] > AIRobot_Builder_Max)
                    AIRobotInfo[type] = AIRobot_Builder_Max;
            }
            else if(type== ShipAIRobotType.Maintenance)
            {
                if (AIRobotInfo[type] > AIRobot_Maintenance_Max)
                    AIRobotInfo[type] = AIRobot_Maintenance_Max;
            }
            else if(type == ShipAIRobotType.Operator)
            {
                if (AIRobotInfo[type] > AIRobot_Operator_Max)
                    AIRobotInfo[type] = AIRobot_Operator_Max;
            }
        }

        /// <summary>
        /// Maintenance_Max
        /// </summary>
        private ushort _AIRobot_Maintenance_Max;
        public ushort AIRobot_Maintenance_Max { get { return _AIRobot_Maintenance_Max; } protected set { } }

        public ModifierDetailPackage AIRobot_MaintenanceMax_DetailPac = new ModifierDetailPackage();
        public void AddAIRobot_Maintenance_Max(ModifierDetailRootType_Simple rootType, ushort num)
        {
            AIRobot_MaintenanceMax_DetailPac.ValueChange(rootType, num);
            _AIRobot_Maintenance_Max += num;
            if (_AIRobot_Maintenance_Max < 0)
                _AIRobot_Maintenance_Max = 0;
        }

        /// <summary>
        /// Builder_Max
        /// </summary>
        private ushort _AIRobot_Builder_Max;
        public ushort AIRobot_Builder_Max { get { return _AIRobot_Builder_Max; } protected set { } }

        public ModifierDetailPackage AIRobot_Builder_Max_DetailPac = new ModifierDetailPackage();
        public void AddAIRobot_Builder_Max(ModifierDetailRootType_Simple rootType,ushort num)
        {
            AIRobot_Builder_Max_DetailPac.ValueChange(rootType, num);
            _AIRobot_Builder_Max += num;
            if (_AIRobot_Builder_Max < 0)
                _AIRobot_Builder_Max = 0;
        }

        private ushort _AIRobot_Operator_Max;
        public ushort AIRobot_Operator_Max { get { return _AIRobot_Operator_Max; }protected set { } }

        public ModifierDetailPackage AIRobot_Operator_Max_DetailPac = new ModifierDetailPackage();
        public void AddAIRobot_Operator_Max(ModifierDetailRootType_Simple rootType,ushort num)
        {
            AIRobot_Operator_Max_DetailPac.ValueChange(rootType, num);
            _AIRobot_Operator_Max += num;
            if (_AIRobot_Operator_Max < 0)
                _AIRobot_Operator_Max = 0;
        }

        #endregion

        #region RoCore
        private ushort _roCore;
        public ushort RoCore { get { return _roCore; } protected set { } }
        public void AddRoCore(ushort num)
        {
            _roCore += num;
            if (_roCore >= _roCoreMax)
                _roCore = _roCoreMax;
        }

        private ushort _roCoreMax;
        public ushort RoCoreMax { get { return _roCoreMax; } protected set { } }

        public ModifierDetailPackage roCoreMaxDetailPac = new ModifierDetailPackage();
        public void AddRoCoreMax(ModifierDetailRootType_Simple rootType, ushort num)
        {
            roCoreMaxDetailPac.ValueChange(rootType, num);
            _roCoreMax += num;
            if (_roCoreMax < 0)
                _roCoreMax = 0;
        }

        #endregion
        public PlayerResourceData() { }
        public static PlayerResourceData InitData()
        {
            PlayerResourceData data = new PlayerResourceData();
            var config = Config.ConfigData.PlayerConfig.gamePrepareConfig;
            if (config == null)
            {
                DebugPlus.LogError("GamePrepareConfig null! PlayerResourceData InitFail!");
                return null;
            }

            data.AddCurrencyMax(ModifierDetailRootType_Simple.OriginConfig, config.OriginalCurrencyMax);
            data.AddResearchMax(ModifierDetailRootType_Simple.OriginConfig, config.OriginalResearchMax);
            data.AddEnergyMax(ModifierDetailRootType_Simple.OriginConfig, config.OriginalEnergyMax);
            data.AddRoCoreMax(ModifierDetailRootType_Simple.OriginConfig, config.OriginalRoCoreMax);
            data.AddAIRobot_Maintenance_Max(ModifierDetailRootType_Simple.OriginConfig, config.OriginalAIRobot_Maintenance_Max);
            data.AddAIRobot_Builder_Max(ModifierDetailRootType_Simple.OriginConfig, config.OriginalAIRobot_Builder_Max);
            data.AddAIRobot_Operator_Max(ModifierDetailRootType_Simple.OriginConfig, config.OriginalAIRobot_Operator_Max);

            var prepareData = DataManager.Instance.gamePrepareData;
            data.AddCurrency(prepareData.GamePrepare_Currency);
            data.AddRoCore(prepareData.GamePrepare_RoCore);
            data.AddResearch(config.OriginalResearch);
            data.AddEnergy(config.OriginalEnergy);
            

            ///Init AIInfo
            data.AIRobotInfo.Add(ShipAIRobotType.Maintenance, prepareData.GamePrepare_AI_Maintenance);
            data.AIRobotInfo.Add(ShipAIRobotType.Builder, prepareData.GamePrepare_AI_Builder);
            data.AIRobotInfo.Add(ShipAIRobotType.Operator, prepareData.GamePrepare_AI_Operator);

            return data;      
        }

        /// <summary>
        /// Game Save
        /// </summary>
        /// <param name="saveData"></param>
        public static PlayerResourceData LoadSave(PlayerSaveData_Resource saveData)
        {
            PlayerResourceData data = new PlayerResourceData();
            if (saveData != null)
            {
                data._currency = saveData.currentCurrency;
                data._currencyMax = saveData.currentCurrencyMax;
                data._currencyPerDay = saveData.currencyPerDay;
                data.currencyPerDayDetailPac = saveData.currencyPerDayDetailPac;
                data.currencyMaxDetailPac = saveData.currencyMaxDetailPac;

                data._research = saveData.currentResearch;
                data._researchMax = saveData.currentResearchMax;
                data._researchPerDay = saveData.researchPerDay;
                data.researchMaxDetailPac = saveData.researchMaxDetailPac;
                data.researchPerDayDetailPac = saveData.researchPerDayDetailPac;

                data._energy = saveData.currentEnergy;
                data._energyMax = saveData.currentEnergyMax;
                data.energyMaxDetailPac = saveData.energyMaxDetailPac;
                data.energyPerDayDetailPac = saveData.energyPerDayDetailPac;

                data._roCore = saveData.currentRoCore;
                data._roCoreMax = saveData.currentRoCoreMax;
                data.roCoreMaxDetailPac = saveData.roCoreMaxDetailPac;

                data.AIRobotInfo = saveData.currentAIRobotInfo;
                data._AIRobot_Builder_Max = saveData.AIRobot_Builder_Max;
                data.AIRobot_Builder_Max_DetailPac = saveData.AIRobot_Builder_Max_DetailPac;
                data._AIRobot_Maintenance_Max = saveData.AIRobot_Maintenance_Max;
                data.AIRobot_MaintenanceMax_DetailPac = saveData.AIRobot_Maintenance_Max_DetailPac;
                data._AIRobot_Operator_Max = saveData.AIRobot_Operator_Max;
                data.AIRobot_Operator_Max_DetailPac = saveData.AIRobot_Operator_Max_DetailPac;

                return data;
            }
            DebugPlus.LogError("[PlayerSaveData_Resource] : Save is null!");
            return null;
        }
    }
   

    public class PlayerAssemblePartData
    {
        #region UnlockState
        /// <summary>
        /// 部件种类解锁情况
        /// </summary>
        public Dictionary<string, Config.AssemblePartMainType> AssemblePartMainTypeDic = new Dictionary<string, Config.AssemblePartMainType>();

        public void AssemblePartTypeSetUnlock(string type, bool unlock)
        {
            Config.AssemblePartMainType typeData = null;
            AssemblePartMainTypeDic.TryGetValue(type, out typeData);
            if (typeData != null)
            {
                typeData.DefaultUnlock = unlock;
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
            foreach (var type in AssemblePartMainTypeDic)
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

        /// <summary>
        /// GetModelList
        /// </summary>
        /// <param name="typeIDList"></param>
        /// <returns></returns>
        public List<BaseDataModel> GetAssemblePartPresetModelList(List<string> typeIDList)
        {
            List<BaseDataModel> result = new List<BaseDataModel>();

            var list = GetUnlockAssemblePartTypeListByTypeIDList(typeIDList);
            for (int i = 0; i < list.Count; i++)
            {
                AssembleTypePresetModel model = new AssembleTypePresetModel();
                if (model.Create(list[i]))
                {
                    result.Add(model);
                }
            }
            return result;
        }
        public List<BaseDataModel> GetAssemblePartPresetModelList(string typeID)
        {
            List<BaseDataModel> result = new List<BaseDataModel>();

            var list = GetUnlockAssemblePartTypeListByTypeID(typeID);
            for (int i = 0; i < list.Count; i++)
            {
                AssembleTypePresetModel model = new AssembleTypePresetModel();
                if (model.Create(list[i]))
                {
                    result.Add(model);
                }
            }
            return result;
        }

        public void AddUnlockAssemblePartID(int partID)
        {
            if (!_currentUnlockPartList.Contains(partID))
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
            for (int i = 0; i < _currentUnlockPartList.Count; i++)
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
            for (int i = 0; i < typeIDList.Count; i++)
            {
                for (int j = 0; j < _currentUnlockPartList.Count; j++)
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

        #endregion

        #region Assemble Part Design

        /// <summary>
        /// 部件设计图
        /// </summary>
        private Dictionary<ushort, AssemblePartInfo> _assemblePartDesignDataDic = new Dictionary<ushort, AssemblePartInfo>();
        public Dictionary<ushort, AssemblePartInfo> AssemblePartDesignDataDic
        {
            get { return _assemblePartDesignDataDic; }
        }

        public AssemblePartInfo GetAssemblePartDesignInfo(ushort uid)
        {
            AssemblePartInfo info = null;
            _assemblePartDesignDataDic.TryGetValue(uid, out info);
            if (info == null)
                DebugPlus.LogError("[PlayerAssemblePartData] : Get Assemble PartInfo Design Empty, UID=" + uid);
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
            foreach (var info in _assemblePartDesignDataDic)
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
            for (int i = 0; i < typeList.Count; i++)
            {
                result.AddRange(GetAssemblePartInfoByTypeID(typeList[i]));
            }
            return result;
        }

        public List<BaseDataModel> GetAssemblePartChooseModel(string typeID)
        {
            List<BaseDataModel> result = new List<BaseDataModel>();

            var list = GetAssemblePartInfoByTypeID(typeID);
            for (int i = 0; i < list.Count; i++)
            {
                AssembleChooseItemModel model = new AssembleChooseItemModel();
                if (model.Create(list[i].UID))
                {
                    result.Add( model);
                }
            }
            return result;
        }


        public void AddAssemblePartDesign(AssemblePartInfo info)
        {
            ushort guid = getPartUnUsedInstanceID();
            info.UID = guid;
            _assemblePartDesignDataDic.Add(guid, info);
            DebugPlus.LogObject<AssemblePartInfo>(info);
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
        public bool CheckAssemblePartCustomNameRepeat(string partName, string customName)
        {
            foreach (var part in _assemblePartDesignDataDic)
            {
                if (part.Value.typePresetData.partName == partName && part.Value.customDataInfo.partNameCustomText == customName)
                    return true;
            }
            return false;
        }

        #endregion

        #region Assmeble Part Current Storage
        /// <summary>
        /// 部件存储
        /// </summary>
        private Dictionary<ushort, AssemblePartInfo> _assemblePartStorageDic = new Dictionary<ushort, AssemblePartInfo>();
        public Dictionary<ushort, AssemblePartInfo> AssemblePartStorageDic
        {
            get { return _assemblePartStorageDic; }
        }
        /// <summary>
        ///  Part Exist
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public bool isAssemblePartStorageExist(ushort uid)
        {
            return _assemblePartStorageDic.ContainsKey(uid);
        }

        public AssemblePartInfo GetAssemblePartStorageInfo(ushort uid)
        {
            AssemblePartInfo info = null;
            _assemblePartStorageDic.TryGetValue(uid, out info);
            if (info == null)
                DebugPlus.LogError("[PlayerAssemblePartData] : Get Assemble PartInfo Storage Empty, UID=" + uid);
            return info;
        }

        public bool RemoveAssemblePartStorage(ushort uid)
        {
            if (isAssemblePartStorageExist(uid))
            {
                _assemblePartStorageDic.Remove(uid);
                return true;
            }
            return false;
        }

        public bool AddAssemblePartStorage(AssemblePartInfo info)
        {
            if (isAssemblePartStorageExist(info.UID))
                return false;
            _assemblePartStorageDic.Add(info.UID, info);
            return true;
        }

        #endregion

        #region Assemble Part Current Equiped
        /// <summary>
        /// 已装备部件存储
        /// </summary>
        private Dictionary<ushort, AssemblePartInfo> _assemblePartEquipedDic = new Dictionary<ushort, AssemblePartInfo>();
        public Dictionary<ushort, AssemblePartInfo> AssemblePartEquipedDic
        {
            get { return _assemblePartEquipedDic; }
        }
        /// <summary>
        ///  Part Exist
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public bool isAssemblePartEquipedExist(ushort uid)
        {
            return _assemblePartEquipedDic.ContainsKey(uid);
        }

        public AssemblePartInfo GetAssemblePartEquipedInfo(ushort uid)
        {
            AssemblePartInfo info = null;
            _assemblePartEquipedDic.TryGetValue(uid, out info);
            if (info == null)
                DebugPlus.LogError("[PlayerAssemblePartData] : Get Assemble PartInfo Equiped Empty, UID=" + uid);
            return info;
        }

        public bool RemoveAssemblePartEquiped(ushort uid)
        {
            if (isAssemblePartEquipedExist(uid))
            {
                _assemblePartEquipedDic.Remove(uid);
                return true;
            }
            return false;
        }

        public bool AddAssemblePartEquiped(AssemblePartInfo info)
        {
            if (isAssemblePartEquipedExist(info.UID))
                return false;
            _assemblePartEquipedDic.Add(info.UID, info);
            return true;
        }
        #endregion

        public PlayerAssemblePartData() { }
        public static PlayerAssemblePartData InitData()
        {
            PlayerAssemblePartData data = new PlayerAssemblePartData();
            var configData = Config.ConfigData.AssembleConfig.assemblePartMainType;
            for (int i = 0; i < configData.Count; i++)
            {
                if (!data.AssemblePartMainTypeDic.ContainsKey(configData[i].Type))
                {
                    data.AssemblePartMainTypeDic.Add(configData[i].Type, configData[i]);
                }
            }
            data._currentUnlockPartList = AssembleModule.GetAllUnlockPartTypeID();
            return data;
        }

        public static PlayerAssemblePartData LoadSaveData(AssemblePartGeneralSaveData saveData)
        {
            PlayerAssemblePartData data = new PlayerAssemblePartData();
            if (saveData == null)
            {
                DebugPlus.LogError("[AssemblePartGeneralSaveData] saveData is null");
                return null;
            }
            //Load Design
            for (int i = 0; i < saveData.currentSaveDesignPart.Count; i++)
            {
                AssemblePartInfo info = new AssemblePartInfo();
                info.LoadSaveData(saveData.currentSaveDesignPart[i]);

                data.AddAssemblePartDesign(info);
            }
            //Load Storage
            for(int i = 0; i < saveData.currentSaveStoragePart.Count; i++)
            {
                AssemblePartInfo info = new AssemblePartInfo();
                info.LoadSaveData(saveData.currentSaveStoragePart[i]);
                data.AddAssemblePartStorage(info);
            }
            //Load Equiped
            for(int i = 0; i < saveData.currentSaveEquipedPart.Count; i++)
            {
                AssemblePartInfo info = new AssemblePartInfo();
                info.LoadSaveData(saveData.currentSaveEquipedPart[i]);
                data.AddAssemblePartStorage(info);
            }
            return data;
        }
    }

    #region Save Data
    public class PlayerSaveData_Resource
    {
        public int currentCurrency;
        public int currentCurrencyMax;
        public ModifierDetailPackage currencyMaxDetailPac;
        public int currencyPerDay;
        public ModifierDetailPackage currencyPerDayDetailPac;

        public float currentResearch;
        public float currentResearchMax;
        public ModifierDetailPackage researchMaxDetailPac;
        public float researchPerDay;
        public ModifierDetailPackage researchPerDayDetailPac;

        public float currentEnergy;
        public float currentEnergyMax;
        public ModifierDetailPackage energyMaxDetailPac;
        public ModifierDetailPackage energyPerDayDetailPac;

        public Dictionary<ShipAIRobotType,ushort> currentAIRobotInfo;

        public ushort AIRobot_Builder_Max;
        public ModifierDetailPackage AIRobot_Builder_Max_DetailPac;
        public ushort AIRobot_Maintenance_Max;
        public ModifierDetailPackage AIRobot_Maintenance_Max_DetailPac;
        public ushort AIRobot_Operator_Max;
        public ModifierDetailPackage AIRobot_Operator_Max_DetailPac;

        public ushort currentRoCore;
        public ushort currentRoCoreMax;
        public ModifierDetailPackage roCoreMaxDetailPac;



        public static PlayerSaveData_Resource CreateSaveData()
        {
            PlayerSaveData_Resource res = new PlayerSaveData_Resource();

            var data = PlayerManager.Instance.playerData.resourceData;

            res.currentCurrency = data.Currency;
            res.currentCurrencyMax = data.CurrencyMax;
            res.currencyPerDay = data.CurrencyPerDay;
            res.currencyMaxDetailPac = data.currencyMaxDetailPac;
            res.currencyPerDayDetailPac = data.currencyPerDayDetailPac;

            res.currentResearch = data.Research;
            res.currentResearchMax = data.ResearchMax;
            res.researchPerDay = data.ResearchPerDay;
            res.researchMaxDetailPac = data.researchMaxDetailPac;
            res.researchPerDayDetailPac = data.researchPerDayDetailPac;

            res.currentEnergy = data.Energy;
            res.currentEnergyMax = data.EnergyMax;
            res.energyMaxDetailPac = data.energyMaxDetailPac;
            res.energyPerDayDetailPac = data.energyPerDayDetailPac;

            res.currentAIRobotInfo = data.AIRobotInfo;
            res.AIRobot_Builder_Max = data.AIRobot_Builder_Max;
            res.AIRobot_Builder_Max_DetailPac = data.AIRobot_Builder_Max_DetailPac;
            res.AIRobot_Maintenance_Max = data.AIRobot_Maintenance_Max;
            res.AIRobot_Maintenance_Max_DetailPac = data.AIRobot_MaintenanceMax_DetailPac;
            res.AIRobot_Operator_Max = data.AIRobot_Operator_Max;
            res.AIRobot_Operator_Max_DetailPac = data.AIRobot_Operator_Max_DetailPac;

            res.currentRoCore = data.RoCore;
            res.currentRoCoreMax = data.RoCoreMax;
            res.roCoreMaxDetailPac = data.roCoreMaxDetailPac;
            return res;
        }
    }

    /// <summary>
    /// Assemble Part
    /// </summary>
    public class AssemblePartGeneralSaveData
    {
        public List<AssmeblePartSingleSaveData> currentSaveDesignPart;
        public List<string> currentUnlockPartTypeList;

        public List<AssmeblePartSingleSaveData> currentSaveStoragePart;
        public List<AssmeblePartSingleSaveData> currentSaveEquipedPart;

        public static AssemblePartGeneralSaveData CreateSave()
        {
            AssemblePartGeneralSaveData data = new AssemblePartGeneralSaveData();

            //Save Design
            data.currentSaveDesignPart = new List<AssmeblePartSingleSaveData>();
            foreach (var info in PlayerManager.Instance.playerData.assemblePartData.AssemblePartDesignDataDic.Values)
            {
                var singleSave = info.CreatePartSave();
                data.currentSaveDesignPart.Add(singleSave);
            }

            data.currentUnlockPartTypeList = PlayerManager.Instance.GetTotalUnlockAssemblePartTypeList();
            //Save Storage
            data.currentSaveStoragePart = new List<AssmeblePartSingleSaveData>();
            foreach(var info in PlayerManager.Instance.playerData.assemblePartData.AssemblePartStorageDic.Values)
            {
                var singleSave = info.CreatePartSave();
                data.currentSaveStoragePart.Add(singleSave);
            }
            //Save Equiped
            data.currentSaveEquipedPart = new List<AssmeblePartSingleSaveData>();
            foreach(var info in PlayerManager.Instance.playerData.assemblePartData.AssemblePartEquipedDic.Values)
            {
                var singleSave = info.CreatePartSave();
                data.currentSaveEquipedPart.Add(singleSave);
            }

            return data;
        }
    }
    #endregion

    #region GamePrepare
    public class GamePrepareData
    {

        public CampInfo currentCampInfo;

        public List<LeaderInfo> currentLeaderInfoList;

        public int hardLevelValue;

        public void ChangeHardLevelValue(int value)
        {
            hardLevelValue += value;
            if (hardLevelValue < 0)
                hardLevelValue = 0;
        }

        public List<GamePreparePropertyData> preparePropertyDataList = new List<GamePreparePropertyData>();

        public List<GamePreparePropertyData> prepareAIDataList = new List<GamePreparePropertyData>();

        /// <summary>
        /// 维修员
        /// </summary>
        public ushort GamePrepare_AI_Maintenance;
        public void GetGamePrepare_AI_Maintenance(int level)
        {
            var propertyData = prepareAIDataList.Find(x => x.configID == Config.ConfigData.PlayerConfig.gamePrepareConfig.GamePrepareConfig_PropertyLink_AI_Maintenance);
            if (propertyData == null)
            {
                GetPrepare_AI_Maintenance_Default();
                return;
            }
            var data = PlayerModule.GetAIPrepareConfigItem(propertyData.configID);
            var levelData = data.levelMap.Find(x => x.Level == level);
            GamePrepare_AI_Maintenance = (ushort)levelData.numParam;
        }

        protected void GetPrepare_AI_Maintenance_Default()
        {
            DebugPlus.Log("[GamePrepare_AI_Maintenance] : Config not Find! Use Default Value");
            GamePrepare_AI_Maintenance = Config.ConfigData.PlayerConfig.gamePrepareConfig.GamePrepareConfig_AI_Operator_Default;
        }

        /// <summary>
        /// 建筑员
        /// </summary>
        public ushort GamePrepare_AI_Builder;
        public void GetGamePrepare_AI_Builder(int level)
        {
            var propertyData = prepareAIDataList.Find(x => x.configID == Config.ConfigData.PlayerConfig.gamePrepareConfig.GamePrepareConfig_PropertyLink_AI_Builder);
            if (propertyData == null)
            {
                GetGamePrepare_AI_Builder_Default();
                return;
            }
            var data = PlayerModule.GetAIPrepareConfigItem(propertyData.configID);
            var levelData = data.levelMap.Find(x => x.Level == level);
            GamePrepare_AI_Builder = (ushort)levelData.numParam;
        }
        protected void GetGamePrepare_AI_Builder_Default()
        {
            DebugPlus.Log("[GamePrepare_AI_Builder] : Config not Find! Use Default Value");
            GamePrepare_AI_Builder = Config.ConfigData.PlayerConfig.gamePrepareConfig.GamePrepareConfig_AI_Builder_Default;
        }

        /// <summary>
        /// 操作员
        /// </summary>
        public ushort GamePrepare_AI_Operator;
        public void GetGamePrepare_AI_Operator(int level)
        {
            var propertyData = prepareAIDataList.Find(x => x.configID == Config.ConfigData.PlayerConfig.gamePrepareConfig.GamePrepareConfig_PropertyLink_AI_Operator);
            if (propertyData == null)
            {
                GetGamePrepare_AI_Operator_Default();
                return;
            }
            var data = PlayerModule.GetAIPrepareConfigItem(propertyData.configID);
            var levelData = data.levelMap.Find(x => x.Level == level);
            GamePrepare_AI_Operator = (ushort)levelData.numParam;
        }
        protected void GetGamePrepare_AI_Operator_Default()
        {
            DebugPlus.Log("[GamePrepare_AI_Operator] : Config not Find! Use Default Value");
            GamePrepare_AI_Builder = Config.ConfigData.PlayerConfig.gamePrepareConfig.GamePrepareConfig_AI_Operator_Default;
        }


        public int GamePrepare_BornPosition;  //出生地


        public int GamePrepare_ResourceRichness = 0;  //资源丰富度
        public void GetPrepare_ResourceRichness(int level)
        {
            var propertyData = preparePropertyDataList.Find(x => x.configID == Config.ConfigData.PlayerConfig.gamePrepareConfig.GamePrepareConfig_PropertyLink_Resource_Richness);
            if (propertyData == null)
            {
                GetPrepare_ResourceRichness_Default();
                return;
            }
            var data = PlayerModule.GetGamePrepareConfigItem(propertyData.configID);
            var levelData = data.levelMap.Find(x => x.Level == level);
            GamePrepare_ResourceRichness = levelData.Level;
        }
        protected void GetPrepare_ResourceRichness_Default()
        {
            DebugPlus.Log("[GamePrepare_ResourceRichness] : Config not Find! Use Default Value");
            GamePrepare_Currency = Config.ConfigData.PlayerConfig.gamePrepareConfig.GamePrepareConfig_Resource_Richness_Default;
        }

        /// <summary>
        /// 初始资金
        /// </summary>
        public int GamePrepare_Currency = 0;
        public void GetPrepare_Currency(int level)
        {
            var propertyData = preparePropertyDataList.Find(x => x.configID == Config.ConfigData.PlayerConfig.gamePrepareConfig.GamePrepareConfig_PropertyLink_Currency);
            if (propertyData == null)
            {
                GetPrepare_Currency_Default();
                return;
            }
            var data = PlayerModule.GetGamePrepareConfigItem(propertyData.configID);
            var levelData = data.levelMap.Find(x => x.Level == level);
            GamePrepare_Currency = (int)levelData.numParam;
        }
        protected void GetPrepare_Currency_Default()
        {
            DebugPlus.Log("[GamePrepare_Currency] : Config not Find! Use Default Value");
            GamePrepare_Currency = Config.ConfigData.PlayerConfig.gamePrepareConfig.GamePrepareConfig_Currency_Default;
        }

        /// <summary>
        /// Ro Core
        /// </summary>
        public ushort GamePrepare_RoCore = 0;
        public void GetPrepare_RoCore(int level)
        {
            var propertyData = preparePropertyDataList.Find(x => x.configID == Config.ConfigData.PlayerConfig.gamePrepareConfig.GamePrepareConfig_PropertyLink_RoCore);
            if (propertyData == null)
            {
                GetPrepare_RoCore_Default();
                return;
            }
            var data = PlayerModule.GetGamePrepareConfigItem(propertyData.configID);
            var levelData = data.levelMap.Find(x => x.Level == level);
            GamePrepare_RoCore = (ushort)levelData.numParam;
        }
        protected void GetPrepare_RoCore_Default()
        {
            DebugPlus.Log("[GamePrepare_RoCore] : Config not Find! Use Default Value");
            GamePrepare_RoCore = Config.ConfigData.PlayerConfig.gamePrepareConfig.GamePrepareConfig_RoCore_Default;
        }

        /// <summary>
        /// 敌人强度
        /// </summary>
        public float GamePrepare_EnemyHardLevel = 1;
        public void GetPrepare_EnermyHardLevel(int level)
        {
            var propertyData = preparePropertyDataList.Find(x => x.configID == Config.ConfigData.PlayerConfig.gamePrepareConfig.GamePrepareConfig_PropertyLink_EnemyHardLevel);
            if (propertyData == null)
            {
                GetPrepare_EnermyHardLevel_Default();
                return;
            }
            var data = PlayerModule.GetGamePrepareConfigItem(propertyData.configID);
            var levelData = data.levelMap.Find(x => x.Level == level);
            GamePrepare_EnemyHardLevel = (float)levelData.numParam;
        }
        protected void GetPrepare_EnermyHardLevel_Default()
        {
            DebugPlus.Log("[EnermyHardLevel] : Config not Find! Use Default Value");
            GamePrepare_EnemyHardLevel = (float)Config.ConfigData.PlayerConfig.gamePrepareConfig.GamePrepareConfig_EnemyHardLevel_Default;
        }


        public float GamePrepare_Research_Coefficient = 1;  //研究系数
        public void GetPrepare_Research_Coefficient(int level)
        {
            var propertyData = preparePropertyDataList.Find(x => x.configID == Config.ConfigData.PlayerConfig.gamePrepareConfig.GamePrepareConfig_PropertyLink_Research_Coefficient);
            if (propertyData == null)
            {
                GetPrepare_Research_Coefficient_Default();
                return;
            }
            var data = PlayerModule.GetGamePrepareConfigItem(propertyData.configID);
            var levelData = data.levelMap.Find(x => x.Level == level);
            GamePrepare_Research_Coefficient = (float)levelData.numParam;
        }
        protected void GetPrepare_Research_Coefficient_Default()
        {
            DebugPlus.Log("[Research_Coefficient] : Config not Find! Use Default Value");
            GamePrepare_EnemyHardLevel = (float)Config.ConfigData.PlayerConfig.gamePrepareConfig.GamePrepareConfig_Research_Coefficient_Default;
        }

        public static GamePrepareData InitData()
        {
            GamePrepareData data = new GamePrepareData();
            var config = Config.ConfigData.PlayerConfig.gamePrepareConfig;
            if (config == null)
                return null;
            for (int i = 0; i < config.prepareProperty.Count; i++)
            {
                GamePreparePropertyData propertyData = new GamePreparePropertyData
                {
                    configID = config.prepareProperty[i].configID,
                    configType = config.prepareProperty[i].configType,
                    currentSelectLevel = config.prepareProperty[i].defaultSelectLevel
                };
                data.preparePropertyDataList.Add(propertyData);
            }

            for (int i=0; i < config.AIPrepareConfig.Count; i++)
            {
                GamePreparePropertyData propertyData = new GamePreparePropertyData
                {
                    configID = config.AIPrepareConfig[i].configID,
                    currentSelectLevel = config.AIPrepareConfig[i].defaultSelectLevel
                };
                data.prepareAIDataList.Add(propertyData);
            }
            data.currentLeaderInfoList = new List<LeaderInfo>();
            return data;
        }

        /// <summary>
        /// SelectLeaderInfo
        /// </summary>
        /// <param name="selectCampID"></param>
        public void RefreshSelectLeaderInfo(int selectCampID)
        {
            currentLeaderInfoList.Clear();
            currentLeaderInfoList = CampModule.GetCampDefaultLeaderList(selectCampID);
        }

        public bool AddSelectLeaderInfo(LeaderInfo info)
        {
            if(!currentLeaderInfoList.Contains(info)&& !info.forceSelcet)
            {
                currentLeaderInfoList.Add(info);
                return true;
            }
            return false;
        }
        public bool RemoveSelectLeaderInfo(LeaderInfo info)
        {
            if (currentLeaderInfoList.Contains(info) && !info.forceSelcet)
            {
                currentLeaderInfoList.Remove(info);
                return true;
            }
            return false;
        }


        public void RefreshData()
        {
            var config = Config.ConfigData.PlayerConfig.gamePrepareConfig;
            for (int i = 0; i < preparePropertyDataList.Count; i++)
            {
                ///Currency
                if (preparePropertyDataList[i].configID == config.GamePrepareConfig_PropertyLink_Currency)
                {
                    GetPrepare_Currency(preparePropertyDataList[i].currentSelectLevel);
                }
                ///Research Richness
                else if (preparePropertyDataList[i].configID == config.GamePrepareConfig_PropertyLink_Resource_Richness)
                {
                    GetPrepare_ResourceRichness(preparePropertyDataList[i].currentSelectLevel);
                }
                ///Enemy HardLevel
                else if (preparePropertyDataList[i].configID == config.GamePrepareConfig_PropertyLink_EnemyHardLevel)
                {
                    GetPrepare_EnermyHardLevel(preparePropertyDataList[i].currentSelectLevel);
                }
                else if (preparePropertyDataList[i].configID == config.GamePrepareConfig_PropertyLink_RoCore)
                {
                    GetPrepare_RoCore(preparePropertyDataList[i].currentSelectLevel);
                }
            }

            for(int i = 0; i < prepareAIDataList.Count; i++)
            {
                if (prepareAIDataList[i].configID == config.GamePrepareConfig_PropertyLink_AI_Maintenance)
                {
                    GetGamePrepare_AI_Maintenance(prepareAIDataList[i].currentSelectLevel);
                }
                else if (prepareAIDataList[i].configID == config.GamePrepareConfig_PropertyLink_AI_Builder)
                {
                    GetGamePrepare_AI_Builder(prepareAIDataList[i].currentSelectLevel);
                }
                else if (prepareAIDataList[i].configID == config.GamePrepareConfig_PropertyLink_AI_Operator)
                {
                    GetGamePrepare_AI_Operator(prepareAIDataList[i].currentSelectLevel);
                }
            }
        }

        public void ClearData()
        {
            currentCampInfo = null;
            currentLeaderInfoList = null;
            hardLevelValue = 0;
            prepareAIDataList.Clear();
            prepareAIDataList.Clear();
            GamePrepare_AI_Maintenance = 0;
            GamePrepare_AI_Builder = 0;
            GamePrepare_AI_Operator = 0;
            GamePrepare_ResourceRichness = 0;
            GamePrepare_Currency = 0;
            GamePrepare_EnemyHardLevel = 0;
            GamePrepare_Research_Coefficient = 0;
            GamePrepare_RoCore = 0;
        }
    }

    public class GamePreparePropertyData
    {
        public string configID;
        public byte configType;
        public byte currentSelectLevel;
    }

    #endregion

}