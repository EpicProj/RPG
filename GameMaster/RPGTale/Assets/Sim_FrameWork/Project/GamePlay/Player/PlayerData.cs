using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Sim_FrameWork
{
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

            resourceData = new PlayerResourceData();
            
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

        //结算
        private float _energyPerDay;
        public float EnergyPerDay { get { return _energyPerDay; } }

        public ModifierDetailPackage energyPerDayDetailPac = new ModifierDetailPackage();

        public void AddEnergyPerDay(ModifierDetailRootType_Simple rootType, float num)
        {
            energyPerDayDetailPac.ValueChange(rootType, num);
            _energyPerDay += num;
        }

        #endregion

        #region Builder
        private ushort _builder;
        public ushort Builder { get { return _builder; } protected set { } }
        public void AddBuilder(ushort num)
        {
            _builder += num;
            if (_builder >= _builderMax)
                _builder = _builderMax;
        }

        private ushort _builderMax;
        public ushort BuilderMax { get { return _builderMax; } protected set { } }

        public ModifierDetailPackage builderMaxDetailPac = new ModifierDetailPackage();
        public void AddBuilderMax(ModifierDetailRootType_Simple rootType, ushort num)
        {
            builderMaxDetailPac.ValueChange(rootType, num);
            _builderMax += num;
            if (_builderMax < 0)
                _builderMax = 0;
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
        public bool InitData()
        {
            
            return false;
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
                data._energyPerDay = saveData.energyPerDay;
                data.energyMaxDetailPac = saveData.energyMaxDetailPac;
                data.energyPerDayDetailPac = saveData.energyPerDayDetailPac;

                data._roCore = saveData.currentRoCore;
                data._roCoreMax = saveData.currentRoCoreMax;
                data.roCoreMaxDetailPac = saveData.roCoreMaxDetailPac;

                data._builder = saveData.currentBuilder;
                data._builderMax = saveData.currentBuilderMax;
                data.builderMaxDetailPac = saveData.builderMaxDetailPac;
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
        public float energyPerDay;
        public ModifierDetailPackage energyPerDayDetailPac;

        public ushort currentBuilder;
        public ushort currentBuilderMax;
        public ModifierDetailPackage builderMaxDetailPac;

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
            res.energyPerDay = data.EnergyPerDay;
            res.energyMaxDetailPac = data.energyMaxDetailPac;
            res.energyPerDayDetailPac = data.energyPerDayDetailPac;

            res.currentBuilder = data.Builder;
            res.currentBuilderMax = data.BuilderMax;
            res.builderMaxDetailPac = data.builderMaxDetailPac;

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

}