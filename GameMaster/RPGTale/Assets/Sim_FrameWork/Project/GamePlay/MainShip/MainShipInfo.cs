using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/*
 * Main Ship Info
 * SOMA
 * 
 */
namespace Sim_FrameWork
{
    #region Enums
    public enum MainShipAreaState
    {
        Working,
        LayOff,
        None,
    }
    public enum MainShipAreaEnergyCostType
    {
        EnergyLevel,

    }

    /// <summary>
    /// 护盾方向
    /// </summary>
    public enum MainShip_ShieldDirection
    {
        front,
        back,
        Left,
        Right
    }
    public enum MainShip_ShieldState
    {
        Disable,
        Open,
        Forbid
    }

    #endregion

    #region MainShip
    public class MainShipInfo
    {
        #region Shield
        /*
         * 护盾四个方向
         * 多层护盾
         */

        public Dictionary<MainShip_ShieldDirection, MainShipShieldInfo> shieldInfoDic;
        /// <summary>
        /// 护盾可分配能源最大值
        /// </summary>
        public short shieldEnergy_Max_current;
        public ModifierDetailPackage_Mix shieldEnergyDetailPac = new ModifierDetailPackage_Mix();
        public bool AddShieldEnergy_Max_Block(ModifierDetailRootType_Mix rootType,uint instanceID,int blockID,short value)
        {
            shieldEnergy_Max_current += value;
            if (shieldEnergy_Max_current < 0)
            {
                shieldEnergyDetailPac.ValueChange_Block(rootType, instanceID, blockID, value - shieldEnergy_Max_current);
                shieldEnergy_Max_current = 0;
                return false;
            }
            var max = Config.ConfigData.MainShipConfigData.basePropertyConfig.shield_energy_total_max_limit;
            if (shieldEnergy_Max_current > max)
            {
                shieldEnergyDetailPac.ValueChange_Block(rootType, instanceID, blockID, shieldEnergy_Max_current - max);
                shieldEnergy_Max_current = max;
                return false;
            }
            return true;
        }
        public bool AddShieldEnergy_Max(ModifierDetailRootType_Mix rootType,short value)
        {
            shieldEnergy_Max_current += value;
            if (shieldEnergy_Max_current < 0)
            {
                shieldEnergyDetailPac.ValueChange(rootType, value - shieldEnergy_Max_current);
                shieldEnergy_Max_current = 0;
                return false;
            }
            var max = Config.ConfigData.MainShipConfigData.basePropertyConfig.shield_energy_total_max_limit;
            if (shieldEnergy_Max_current > max)
            {
                shieldEnergyDetailPac.ValueChange(rootType, shieldEnergy_Max_current - max);
                shieldEnergy_Max_current = max;
                return false;
            }
            return true;

        }

        /// <summary>
        /// 护盾当前可分配能源
        /// </summary>
        public short shieldEnergy_current;
        public Dictionary<MainShip_ShieldDirection, short> shieldEnergyDetailDic = new Dictionary<MainShip_ShieldDirection, short>();
        public bool ChangeShieldEnergy(short value)
        {
            return true;
        }

        #endregion

        public MainShipPowerAreaInfo powerAreaInfo;
        public MainShipControlTowerInfo controlTowerInfo;
        public MainShipLivingAreaInfo livingAreaInfo;
        public MainShipHangarInfo hangarAreaInfo;
        public MainShipWorkingAreaInfo workingAreaInfo;


        public MainShipInfo() { }
        public bool InitInfo()
        {
            var config = Config.ConfigData.MainShipConfigData.basePropertyConfig;
            if (config == null)
                return false;

            ///Init Shield Data
            shieldInfoDic = new Dictionary<MainShip_ShieldDirection, MainShipShieldInfo>();
            AddShieldEnergy_Max(ModifierDetailRootType_Mix.OriginConfig, config.shield_energy_total_max_base);
            foreach(MainShip_ShieldDirection direction in Enum.GetValues(typeof(MainShip_ShieldDirection)))
            {
                MainShipShieldInfo info = new MainShipShieldInfo();
                if (info.InitData(direction))
                {
                    shieldInfoDic.Add(direction, info);
                }
            }

            powerAreaInfo = new MainShipPowerAreaInfo();
            livingAreaInfo = new MainShipLivingAreaInfo();
            controlTowerInfo = new MainShipControlTowerInfo();
            hangarAreaInfo = new MainShipHangarInfo();
            workingAreaInfo = new MainShipWorkingAreaInfo();

            if (powerAreaInfo.InitData() == false)
                return false;

            ///InitEnergyLoad
            powerAreaInfo.ChangeEnergyLoadValue((short)-livingAreaInfo.powerLevel_current);
            powerAreaInfo.ChangeEnergyLoadValue((short)-controlTowerInfo.powerLevel_current);
            powerAreaInfo.ChangeEnergyLoadValue((short)-hangarAreaInfo.powerLevel_current);
            powerAreaInfo.ChangeEnergyLoadValue((short)-workingAreaInfo.powerLevel_current);

            return true;
        }

        public void LoadSaveData(MainShipSaveData saveData)
        {
            if (saveData == null)
                return;
            shieldEnergy_Max_current = saveData.ShieldEnergy_Max_current;
            shieldEnergyDetailPac = saveData.ShieldEnergyDetailPac;

            shieldInfoDic = new Dictionary<MainShip_ShieldDirection, MainShipShieldInfo>();

            foreach (var shieldSave in saveData.shieldSaveDataDic)
            {
                MainShipShieldInfo info = new MainShipShieldInfo();
                info.LoadGameSave(shieldSave.Value);
                shieldInfoDic.Add(shieldSave.Key, info);
            }

            powerAreaInfo = new MainShipPowerAreaInfo();
            powerAreaInfo.LoadSaveData(saveData.powerAreaSaveData);

        }
    }

    /// <summary>
    /// Shield Info
    /// </summary>
    public class MainShipShieldInfo
    {
        public string directionName;
        /// <summary>
        /// ShieldState
        /// </summary>
        public MainShip_ShieldState currentState = MainShip_ShieldState.Disable;

        public MainShip_ShieldDirection direction;

        private Config.MainShipShieldLevelMap _shieldConfig_current;
        public Config.MainShipShieldLevelMap ShieldConfig_current
        {
            get
            {
                if (_shieldConfig_current == null)
                {
                    _shieldConfig_current = MainShipModule.GetMainShipShieldLevelData(shieldLevel_current);
                }
                return _shieldConfig_current;
            }
        }

        //当前区域护盾分配能源等级
        public short shieldLevel_current =0;
        //可装备护盾槽数量
        public byte shieldEquip_slotNum_current
        {
            get
            {
                var config = Config.ConfigData.MainShipConfigData.basePropertyConfig.shield_slot_unlock_energycost_map;
                for(int i = 0; i < config.Length; i++)
                {
                    if (shieldLevel_current < config[i])
                        return (byte)i;
                }
                return 0;
            }
            protected set { }
        }
        /// <summary>
        /// 增加护盾等级
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool AddShieldLevel(short value)
        {
            shieldLevel_current += value;
            if (shieldLevel_current <= 0)
            {
                UpdateShieldLevel((short)(shieldLevel_current - value), 0);
                shieldLevel_current = 0;
                return false;
            }
            var max = Config.ConfigData.MainShipConfigData.basePropertyConfig.shield_energy_total_max_limit;
            if (shieldLevel_current > max)
            {
                UpdateShieldLevel((short)(shieldLevel_current - value), max);
                shieldLevel_current = max;
                return false;
            }

            UpdateShieldLevel((short)(shieldLevel_current - value), shieldLevel_current);
            return true;
        }

        public bool ChangeShieldLevel(byte value)
        {
            var max = Config.ConfigData.MainShipConfigData.basePropertyConfig.shield_energy_total_max_limit;
            short originLevel = shieldLevel_current;
            if(value> max)
            {
                shieldLevel_current = max;
                AddShieldLevel((short)(max - shieldLevel_current));
                return false;
            }
            else
            {
                shieldLevel_current = value;
                AddShieldLevel((short)(shieldLevel_current - originLevel));
                return true;
            }
        }

        /// <summary>
        /// 更新护盾等级
        /// </summary>
        /// <param name="preLevel"></param>
        /// <param name="currentLevel"></param>
        /// <returns></returns>
        bool UpdateShieldLevel(short preLevel,short currentLevel)
        {
            if (ShieldConfig_current == null)
                return false;
            var preConfig = MainShipModule.GetMainShipShieldLevelData(preLevel);
            var currentConfig = MainShipModule.GetMainShipShieldLevelData(currentLevel);
            if (preConfig == null || currentConfig == null)
                return false;

            //Update ShieldMax Data & ShieldValue
            AddShieldMax( ModifierDetailRootType_Mix.OriginConfig,currentConfig.shieldMax_base - preConfig.shieldMax_base);
            //Update ShieldOpenInit
            AddShieldOpenInit(ModifierDetailRootType_Mix.OriginConfig, currentConfig.shieldOpenInit_base - preConfig.shieldOpenInit_base);
            //Update ShieldChargeSpeed
            AddShieldChargeSpeed(ModifierDetailRootType_Mix.OriginConfig, currentConfig.shieldChargeSpeed_base - preConfig.shieldChargeSpeed_base);
            //Update EnergyCost
            AddEnergyCostValue(ModifierDetailRootType_Mix.OriginConfig,(short)(currentConfig.shieldEnergyCost_base - preConfig.shieldEnergyCost_base));
            //Update DamageReduce
            AddDamageReduce(ModifierDetailRootType_Mix.OriginConfig, (float)currentConfig.shieldDamageReduce_base - (float)preConfig.shieldDamageReduce_base);
            //Update DamageReudce Probability
            AddDamageReudceProbability(ModifierDetailRootType_Mix.OriginConfig, (float)currentConfig.shieldDamageReduceProbability_base - (float)preConfig.shieldDamageReduceProbability_base);

            DebugPlus.Log("Change Shield Level Success!");
            DebugPlus.LogObject<MainShipShieldInfo>(this);
            return true;
        }

        // 护盾开启时初始值
        public int shield_open_init;
        public ModifierDetailPackage_Mix shieldOpenInitDetailPac = new ModifierDetailPackage_Mix();

        public bool AddShieldOpenInit_Block(ModifierDetailRootType_Mix rootType, uint instanceID,int blockID,int value)
        {
            shield_open_init += value;
            if (shield_open_init < 0)
            {
                shield_open_init = 0;
                shieldOpenInitDetailPac.ValueChange_Block(rootType, instanceID, blockID, value- shield_open_init);
                return false;
            }
            else
            {
                shieldOpenInitDetailPac.ValueChange_Block(rootType, instanceID, blockID, value);
                return true;
            }
        }
        public bool AddShieldOpenInit_Assemble(ModifierDetailRootType_Mix rootType, ushort UID, int partID, int value)
        {
            shield_open_init += value;
            if (shield_open_init < 0)
            {
                shield_open_init = 0;
                shieldOpenInitDetailPac.ValueChange_Assemble(rootType, UID, partID, value - shield_open_init);
                return false;
            }
            else
            {
                shieldOpenInitDetailPac.ValueChange_Assemble(rootType, UID, partID, value);
                return true;
            }
        }
        public bool AddShieldOpenInit(ModifierDetailRootType_Mix rootType,int value)
        {
            shield_open_init += value;
            if (shield_open_init < 0)
            {
                shield_open_init = 0;
                shieldOpenInitDetailPac.ValueChange(rootType, value - shield_open_init);
                return false;
            }
            else
            {
                shieldOpenInitDetailPac.ValueChange(rootType,value);
                return true;
            }
        }

        /// <summary>
        /// Shield Value 
        /// </summary>
        public int shield_max;
        public int shield_current;
        public ModifierDetailPackage_Mix shieldMaxDetailPac = new ModifierDetailPackage_Mix();
        public bool AddShieldMax_Block(ModifierDetailRootType_Mix rootType, uint instanceID, int blockID, int value)
        {
            shield_max += value;
            if (shield_max < 0)
            {
                shieldMaxDetailPac.ValueChange_Block(rootType, instanceID, blockID, value - shield_max);
                ChangeShieldCurrentValue(0);
                shield_max = 0;
                return false;
            }
            else
            {
                shieldMaxDetailPac.ValueChange_Block(rootType, instanceID, blockID, value);
                ChangeShieldCurrentValue(0);
                return true;
            }
        }
        public bool AddShieldMax_Assemble(ModifierDetailRootType_Mix rootType, ushort UID, int partID, int value)
        {
            shield_max += value;
            if (shield_max < 0)
            {
                shieldMaxDetailPac.ValueChange_Assemble(rootType, UID, partID, value - shield_max);
                ChangeShieldCurrentValue(0);
                shield_max = 0;
                return false;
            }
            else
            {
                shieldMaxDetailPac.ValueChange_Assemble(rootType, UID, partID, value);
                ChangeShieldCurrentValue(0);
                return true;
            }
        }
        public bool AddShieldMax(ModifierDetailRootType_Mix rootType, int value)
        {
            shield_max += value;
            if (shield_max < 0)
            {
                shieldMaxDetailPac.ValueChange(rootType,value - shield_max);
                ChangeShieldCurrentValue(0);
                shield_max = 0;
                return false;
            }
            else
            {
                shieldMaxDetailPac.ValueChange(rootType,value);
                ChangeShieldCurrentValue(0);
                return true;
            }
        }

        public bool ChangeShieldCurrentValue(int value)
        {
            shield_current += value;
            if (shield_current > shield_max)
            {
                shield_current = shield_max;
                return false;
            }
            if (shield_current < 0)
            {
                shield_current = 0;
                return false;
            }
            return true;
        }

        // reality Value
        public int shieldCharge_current
        {
            get { return (int)(shieldChargeSpeed * shieldChargeRatio); }
        }

        public int shieldChargeSpeed;
        // 充能速度折损比例
        public float shieldChargeRatio =1.0f;
        public ModifierDetailPackage_Mix shieldChargeSpeedDetailPac = new ModifierDetailPackage_Mix();
        public bool AddShieldChargeSpeed_Block(ModifierDetailRootType_Mix rootType, uint instanceID, int blockID, int value)
        {
            shieldChargeSpeed += value;
            if (shieldChargeSpeed < 0)
            {
                shieldChargeSpeedDetailPac.ValueChange_Block(rootType, instanceID, blockID, value - shieldChargeSpeed);
                shieldChargeSpeed = 0;
                return false;
            }
            else
            {
                shieldChargeSpeedDetailPac.ValueChange_Block(rootType, instanceID, blockID, value);
                return true;
            }
        }
        public bool AddShieldChargeSpeed_Assemble(ModifierDetailRootType_Mix rootType, ushort UID, int partID, int value)
        {
            shieldChargeSpeed += value;
            if (shieldChargeSpeed < 0)
            {
                shieldChargeSpeedDetailPac.ValueChange_Assemble(rootType, UID, partID, value - shieldChargeSpeed);
                shieldChargeSpeed = 0;
                return false;
            }
            else
            {
                shieldChargeSpeedDetailPac.ValueChange_Assemble(rootType, UID, partID, value);
                return true;
            }
        }
        public bool AddShieldChargeSpeed(ModifierDetailRootType_Mix rootType, int value)
        {
            shieldChargeSpeed += value;
            if (shieldChargeSpeed < 0)
            {
                shieldChargeSpeedDetailPac.ValueChange(rootType, value - shieldChargeSpeed);
                shieldChargeSpeed = 0;
                return false;
            }
            else
            {
                shieldChargeSpeedDetailPac.ValueChange(rootType, value);
                return true;
            }
        }

        /// <summary>
        /// 能源消耗
        /// </summary>
        private short energyCost_current;
        public short EnergyCost_current
        {
            get
            {
                if (currentState == MainShip_ShieldState.Open)
                    return energyCost_current;
                return 0;
            }
        }
        //最终能源消耗
        public short EnergyCost_Rarity
        {
            get { return (short)(EnergyCost_current * shieldLayer_energyCost_ratio); }
        }
        public ModifierDetailPackage_Mix energyCostDetailPac = new ModifierDetailPackage_Mix();
        public bool AddEnergyCostValue_Block(ModifierDetailRootType_Mix rootType, uint instanceID, int blockID, short value)
        {
            energyCost_current += value;
            if (energyCost_current < 0)
            {
                energyCostDetailPac.ValueChange_Block(rootType, instanceID, blockID, value - energyCost_current);
                energyCost_current = 0;
                return false;
            }
            else
            {
                energyCostDetailPac.ValueChange_Block(rootType, instanceID, blockID, value);
                return true;
            }
        }
        public bool AddEnergyCostValue_Assemble(ModifierDetailRootType_Mix rootType, ushort UID, int partID, short value)
        {
            energyCost_current += value;
            if (energyCost_current < 0)
            {
                energyCostDetailPac.ValueChange_Assemble(rootType, UID, partID, value - energyCost_current);
                energyCost_current = 0;
                return false;
            }
            else
            {
                energyCostDetailPac.ValueChange_Assemble(rootType, UID, partID, value);
                return true;
            }
        }
        public bool AddEnergyCostValue(ModifierDetailRootType_Mix rootType, short value)
        {
            energyCost_current += value;
            if (energyCost_current < 0)
            {
                energyCostDetailPac.ValueChange(rootType, value - energyCost_current);
                energyCost_current = 0;
                return false;
            }
            else
            {
                energyCostDetailPac.ValueChange(rootType, value);
                return true;
            }
        }
        //仅在护盾装备更换时计算
        public int CalculateShieldAssembleEnergyCost()
        {
            int result = 0;
            for(int i = 0; i < equipShieldAssembleList.Count; i++)
            {
                var propertyKey = Config.ConfigData.AssembleConfig.mainShip_Shield_EnergyCost_Property_Link;
                if (equipShieldAssembleList[i].customDataInfo.propertyDic.ContainsKey(propertyKey))
                {
                    result += (int)equipShieldAssembleList[i].customDataInfo.propertyDic[propertyKey].propertyValueMax;
                }
                else
                {
                    DebugPlus.Log("[MainShip Shield Equip] : Shield_MaxLayer_Property empty!");
                    DebugPlus.LogArray<AssemblePartCustomDataInfo.CustomData>(equipShieldAssembleList[i].customDataInfo.propertyDic.Values.ToArray());
                }
            }
            return result;
        }

        /// <summary>
        /// 装备护盾
        /// </summary>
        public List<AssemblePartInfo> equipShieldAssembleList = new List<AssemblePartInfo>();
        public bool EquipShieldAssemble(AssemblePartInfo info)
        {
            ///Count out of range
            if (equipShieldAssembleList.Count > shieldEquip_slotNum_current)
                return false;
            if(info.partEquipType.Contains(AssembleEquipTarget.MainShip_Shield))
            {
                ///Calculate PropertyChange
                var dic = info.customDataInfo.propertyDic;
                foreach (var property in dic)
                {
                    if (property.Key == Config.ConfigData.AssembleConfig.mainShip_Shield_OpenInit_Property_Link)
                    {
                        ///OpenInit
                        AddShieldOpenInit_Assemble(ModifierDetailRootType_Mix.MainShip_Shield, info.UID, info.partID, 
                            (int)property.Value.propertyValueMax);
                    }else if(property.Key == Config.ConfigData.AssembleConfig.mainShip_Shield_EnergyCost_Property_Link)
                    {
                        ///EnergyCost
                        AddEnergyCostValue_Assemble(ModifierDetailRootType_Mix.MainShip_Shield, info.UID, info.partID,
                            (short)property.Value.propertyValueMax);
                    }else if(property.Key == Config.ConfigData.AssembleConfig.mainShip_Shield_Max_Property_Link)
                    {
                        ///Shield Max
                        AddShieldMax_Assemble(ModifierDetailRootType_Mix.MainShip_Shield, info.UID, info.partID,
                            (int)property.Value.propertyValueMax);
                    }else if(property.Key == Config.ConfigData.AssembleConfig.mainShip_Shield_ChargeSpeed_Property_Link)
                    {
                        ///Charge Speed
                        AddShieldChargeSpeed_Assemble(ModifierDetailRootType_Mix.MainShip_Shield, info.UID, info.partID,
                            (int)property.Value.propertyValueMax);
                    }
                }
                //Damege Reduce
                CalculateDamageReduce();
                //LayerMax
                CalculateShieldLayerMax();
                return true;
            }
            return false;
        }
        /// <summary>
        /// 卸载护盾
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public bool UnEquipShieldAssemble(AssemblePartInfo info)
        {
            if (!equipShieldAssembleList.Contains(info))
                return false;
            if(info.partEquipType.Contains(AssembleEquipTarget.MainShip_Shield))
            {
                ///Calculate PropertyChange
                var dic = info.customDataInfo.propertyDic;
                foreach (var property in dic)
                {
                    if (property.Key == Config.ConfigData.AssembleConfig.mainShip_Shield_OpenInit_Property_Link)
                    {
                        ///OpenInit
                        AddShieldOpenInit_Assemble(ModifierDetailRootType_Mix.MainShip_Shield, info.UID, info.partID,
                            (int)-property.Value.propertyValueMax);
                    }
                    else if (property.Key == Config.ConfigData.AssembleConfig.mainShip_Shield_EnergyCost_Property_Link)
                    {
                        ///EnergyCost
                        AddEnergyCostValue_Assemble(ModifierDetailRootType_Mix.MainShip_Shield, info.UID, info.partID,
                            (short)-property.Value.propertyValueMax);
                    }
                    else if (property.Key == Config.ConfigData.AssembleConfig.mainShip_Shield_Max_Property_Link)
                    {
                        ///Shield Max
                        AddShieldMax_Assemble(ModifierDetailRootType_Mix.MainShip_Shield, info.UID, info.partID,
                            (int)-property.Value.propertyValueMax);
                    }
                    else if (property.Key == Config.ConfigData.AssembleConfig.mainShip_Shield_ChargeSpeed_Property_Link)
                    {
                        ///Charge Speed
                        AddShieldChargeSpeed_Assemble(ModifierDetailRootType_Mix.MainShip_Shield, info.UID, info.partID,
                            (int)-property.Value.propertyValueMax);
                    }
                }
                equipShieldAssembleList.Remove(info);
                ///Add To Storage
                PlayerManager.Instance.AddAssmebleStorageInfo(info);
                //Damage Reduce
                CalculateDamageReduce();
                //LayerMax
                CalculateShieldLayerMax();
                return true;
            }
            return false;
        }

        /// <summary>
        /// 护盾层数
        /// </summary>
        public short shieldLayer_current = 1;
        public short shieldLayer_max = 1;
        public float shieldLayer_energyCost_ratio = 1;
        public Dictionary<int, ShieldLayerInfo> layerInfoDic = new Dictionary<int, ShieldLayerInfo>();
        //更换护盾层数
        public bool ChangeShieldLayer(short layer)
        {
            layerInfoDic.Clear();

            shieldLayer_current = layer;
            if (layer > shieldLayer_max)
                shieldLayer_current = shieldLayer_max;

            var config = MainShipModule.GetMainShipShieldLayerData(shieldLayer_current);
            if (config == null)
            {
                DebugPlus.LogError("[ShieldLayerInfo] : ChangeShieldLayerError! layerNotExist! layer=" + shieldLayer_current);
                return false;
            }
            float average = damageReduce_base / shieldLayer_current;
            int layerIndex = 1;
            for(int i = 0; i < shieldLayer_current; i++)
            {
                ShieldLayerInfo info = new ShieldLayerInfo(layerIndex, average, damageReduce_probability_base);
                layerInfoDic.Add(layerIndex, info);
                layerIndex++;
            }

            shieldLayer_energyCost_ratio = config.energyCostRatio;
            return true;
        }

        public void CalculateShieldLayerMax()
        {
            List<int> layerMaxList = new List<int> ();
            for(int i = 0; i < equipShieldAssembleList.Count; i++)
            {
                var propertyKey = Config.ConfigData.AssembleConfig.mainShip_Shield_MaxLayer_Property_Link;
                if (equipShieldAssembleList[i].customDataInfo.propertyDic.ContainsKey(propertyKey))
                {
                    layerMaxList.Add((int)equipShieldAssembleList[i].customDataInfo.propertyDic[propertyKey].propertyValueMax);
                }
                else
                {
                    DebugPlus.Log("[MainShip Shield Equip] : Shield_MaxLayer_Property empty!");
                    DebugPlus.LogArray<AssemblePartCustomDataInfo.CustomData>(equipShieldAssembleList[i].customDataInfo.propertyDic.Values.ToArray());
                }
            }
            if (layerMaxList.Count != 0)
            {
                layerMaxList.Sort();
                shieldLayer_max = (short)layerMaxList[layerMaxList.Count - 1];
            }
            else
            {
                shieldLayer_max = 1;
            }
        }

        /// <summary>
        /// 护盾基础减伤
        /// </summary>
        public float damageReduce_base;
        public ModifierDetailPackage_Mix damageReduceDetailPac = new ModifierDetailPackage_Mix();
        public bool AddDamageReduce_Assemble(ModifierDetailRootType_Mix rootType,float value)
        {
            damageReduce_base += value;
            if (damageReduce_base < 0)
            {
                damageReduceDetailPac.ValueChange_Assemble(rootType, 0, 0, value - damageReduce_base);
                damageReduce_base = 0;
                return false;
            }
            else if(damageReduce_base > 1)
            {
                damageReduceDetailPac.ValueChange_Assemble(rootType,0,0, damageReduce_base - 1);
                damageReduce_base = 1;
                return false;
            }
            else
            {
                damageReduceDetailPac.ValueChange_Assemble(rootType, 0, 0, value);
                return true;
            }
        }
        public bool AddDamageReduce(ModifierDetailRootType_Mix rootType,float value)
        {
            damageReduce_base += value;
            if (damageReduce_base < 0)
            {
                damageReduceDetailPac.ValueChange(rootType, value - damageReduce_base);
                damageReduce_base = 0;
                return false;
            }
            else if (damageReduce_base > 1)
            {
                damageReduceDetailPac.ValueChange(rootType, damageReduce_base - 1);
                damageReduce_base = 1;
                return false;
            }
            else
            {
                damageReduceDetailPac.ValueChange(rootType, value);
                return true;
            }
        }
        //仅在护盾装备更换时计算
        public void CalculateDamageReduceProbability()
        {
            int totalLevel = 0;
            float totalDamageReduceProbability = 0;
            for (int i = 0; i < equipShieldAssembleList.Count; i++)
            {
                var shieldLevelPropertyKey = Config.ConfigData.AssembleConfig.mainShip_Shield_ShieldLevel_Property_Link;
                int currentLevel = 0;
                var dic = equipShieldAssembleList[i].customDataInfo.propertyDic;
                if (dic.ContainsKey(shieldLevelPropertyKey))
                {
                    currentLevel = (int)dic[shieldLevelPropertyKey].propertyValueMax;
                    totalLevel += currentLevel;
                }
                else
                {
                    DebugPlus.Log("[MainShip Shield Equip] : ShieldLevel_Property empty!");
                    DebugPlus.LogArray<AssemblePartCustomDataInfo.CustomData>(dic.Values.ToArray());
                }

                var shieldDamageReudceProbabilityProperyKey = Config.ConfigData.AssembleConfig.mainShip_Shield_ReduceProbability_Property_Link;
                if (dic.ContainsKey(shieldDamageReudceProbabilityProperyKey))
                {
                    totalDamageReduceProbability += currentLevel * dic[shieldDamageReudceProbabilityProperyKey].propertyValueMax;
                }
                else
                {
                    DebugPlus.Log("[MainShip Shield Equip] : Shield_ReduceProbability empty!");
                    DebugPlus.LogArray<AssemblePartCustomDataInfo.CustomData>(dic.Values.ToArray());
                }
            }
            float result = totalDamageReduceProbability / totalLevel;
            float delta = result - damageReduce_base;
            AddDamageReudceProbability_Assmeble(ModifierDetailRootType_Mix.MainShip_Shield, delta);
        }

        /// <summary>
        /// 护盾基础减伤概率
        /// </summary>
        public float damageReduce_probability_base;
        public ModifierDetailPackage_Mix damageReduceProbabilityDetailPac = new ModifierDetailPackage_Mix();
        public bool AddDamageReudceProbability_Assmeble(ModifierDetailRootType_Mix rootType,float value)
        {
            damageReduce_probability_base += value;
            if (damageReduce_probability_base < 0)
            {
                damageReduceProbabilityDetailPac.ValueChange_Assemble(rootType, 0, 0, value - damageReduce_probability_base);
                damageReduce_probability_base = 0;
                return false;
            }
            else if (damageReduce_probability_base > 1)
            {
                damageReduceProbabilityDetailPac.ValueChange_Assemble(rootType, 0, 0, damageReduce_probability_base - 1);
                damageReduce_probability_base = 1;
                return false;
            }
            else
            {
                damageReduceProbabilityDetailPac.ValueChange_Assemble(rootType, 0, 0, value);
                return true;
            }
        }
        public bool AddDamageReudceProbability(ModifierDetailRootType_Mix rootType,float value)
        {
            damageReduce_probability_base += value;
            if (damageReduce_probability_base < 0)
            {
                damageReduceProbabilityDetailPac.ValueChange(rootType, value - damageReduce_probability_base);
                damageReduce_probability_base = 0;
                return false;
            }
            else if (damageReduce_probability_base > 1)
            {
                damageReduceProbabilityDetailPac.ValueChange(rootType, damageReduce_probability_base - 1);
                damageReduce_probability_base = 1;
                return false;
            }
            else
            {
                damageReduceProbabilityDetailPac.ValueChange(rootType, value);
                return true;
            }
        }
        //仅在护盾装备更换时计算
        public void CalculateDamageReduce()
        {
            int totalLevel = 0;
            float totalDamageReduce = 0;
            for (int i = 0; i < equipShieldAssembleList.Count; i++)
            {
                var shieldLevelPropertyKey = Config.ConfigData.AssembleConfig.mainShip_Shield_ShieldLevel_Property_Link;
                int currentLevel = 0;
                var dic = equipShieldAssembleList[i].customDataInfo.propertyDic;
                if (dic.ContainsKey(shieldLevelPropertyKey))
                {
                    currentLevel = (int)dic[shieldLevelPropertyKey].propertyValueMax;
                    totalLevel += currentLevel;
                }
                else
                {
                    DebugPlus.Log("[MainShip Shield Equip] : ShieldLevel_Property empty!");
                    DebugPlus.LogArray<AssemblePartCustomDataInfo.CustomData>(dic.Values.ToArray());
                }

                var shieldDamageReudceProperyKey = Config.ConfigData.AssembleConfig.mainShip_Shield_DamageReduce_Property_Link;
                if (dic.ContainsKey(shieldDamageReudceProperyKey))
                {
                    totalDamageReduce += currentLevel * dic[shieldDamageReudceProperyKey].propertyValueMax;
                }
                else
                {
                    DebugPlus.Log("[MainShip Shield Equip] : DamageReduce_Property empty!");
                    DebugPlus.LogArray<AssemblePartCustomDataInfo.CustomData>(dic.Values.ToArray());
                }
            }
            float result= totalDamageReduce / totalLevel;
            float delta = result - damageReduce_base;
            AddDamageReduce_Assemble(ModifierDetailRootType_Mix.MainShip_Shield,delta);
        }
          
        public MainShipModifier shipShieldModifier;

        public MainShipShieldInfo() { }
        public bool InitData(MainShip_ShieldDirection direction)
        {
            this.direction = direction;
            directionName = MainShipModule.GetMainShipShieldDirectionName(direction);
            if (ShieldConfig_current == null)
                return false;
            shipShieldModifier = new MainShipModifier(ModifierTarget.MainShipShield);
            return true;
        }

        public void LoadGameSave(MainShipShieldSaveData saveData)
        {
            currentState = saveData.CurrentState;
            direction = saveData.Direction;
            directionName = MainShipModule.GetMainShipShieldDirectionName(direction);
            shieldLevel_current = saveData.ShieldLevel_current;

            shield_open_init = saveData.Shield_open_init;
            shieldOpenInitDetailPac = saveData.ShieldOpenInitDetailPac;

            shield_max = saveData.Shield_max;
            shield_current = saveData.Shield_current;
            shieldMaxDetailPac = saveData.ShieldMaxDetailPac;

            shieldChargeSpeed = saveData.ShieldChargeSpeed;
            shieldChargeSpeedDetailPac = saveData.ShieldChargeSpeedDetailPac;
        
            energyCost_current = saveData.EnergyCost_current;
            energyCostDetailPac = saveData.EnergyCostDetailPac;

            damageReduce_base = saveData.DamageReduce_base;
            damageReduceDetailPac = saveData.DamageReduceDetailPac;

            damageReduce_probability_base = saveData.DamageReduce_probability_base;
            damageReduceProbabilityDetailPac = saveData.DamageReduceProbabilityDetailPac;

            shieldLayer_current = saveData.ShieldLevel_current;
            shieldLayer_max = saveData.ShieldLayer_max;
            ChangeShieldLayer(shieldLayer_current);

            for (int i = 0; i < saveData.equipedShieldAssembleUIDList.Count; i++)
            {
                ushort partUID = saveData.equipedShieldAssembleUIDList[i];
                if (PlayerManager.Instance.playerData.assemblePartData.isAssemblePartEquipedExist(partUID))
                {
                    //Part Exist  直接赋值  不走装备流程
                    var info = PlayerManager.Instance.playerData.assemblePartData.GetAssemblePartEquipedInfo(partUID);
                    equipShieldAssembleList.Add(info);
                }
            }
        }

        public class ShieldLayerInfo
        {
            public int layerIndex;
            public float damageReduce;
            public float damageReduceProbability;

            /// <summary>
            /// 实际折算比率
            /// </summary>
            public float damageReudce_ratio;
            public float damageReduceProbability_ratio;

            ///实际最终数值
            public float damageReduce_reality
            {
                get
                {
                    return damageReduce * damageReudce_ratio;
                }
            }
            public float damageReduceProbability_reality
            {
                get
                {
                    return damageReduceProbability * damageReduceProbability_ratio;
                }
            }

            public ShieldLayerInfo(int layerIndex,float damageReduce,float damageReduceProbability)
            {
                this.layerIndex = layerIndex;
                this.damageReduce = damageReduce;
                this.damageReduceProbability = damageReduceProbability;

                var config = MainShipModule.GetMainShipShieldLayerData(layerIndex);
                if (config == null)
                {
                    DebugPlus.LogError("[ShieldLayerInfo] : layerInfoConfig not find  layerIndex=" + layerIndex);
                    return;
                }
                damageReudce_ratio = config.damageReudce_ratio;
                damageReduceProbability_ratio = config.damageReduceProbability_ratio;
            }
        }
    }
    /// <summary>
    /// Shield Save Data
    /// </summary>
    public class MainShipShieldSaveData
    {
        public MainShip_ShieldState CurrentState;
        public MainShip_ShieldDirection Direction;

        public short ShieldLevel_current;

        public int Shield_open_init;
        public ModifierDetailPackage_Mix ShieldOpenInitDetailPac;

        public int Shield_max;
        public int Shield_current;
        public ModifierDetailPackage_Mix ShieldMaxDetailPac;

        public int ShieldChargeSpeed;
        public ModifierDetailPackage_Mix ShieldChargeSpeedDetailPac;

        public short EnergyCost_current;
        public ModifierDetailPackage_Mix EnergyCostDetailPac;

        public float DamageReduce_base;
        public ModifierDetailPackage_Mix DamageReduceDetailPac;

        public float DamageReduce_probability_base;
        public ModifierDetailPackage_Mix DamageReduceProbabilityDetailPac;

        public short ShieldLayer_current = 1;
        public short ShieldLayer_max = 1;

        public List<ushort> equipedShieldAssembleUIDList;

        public static MainShipShieldSaveData CreateSave(MainShipShieldInfo info)
        {
            MainShipShieldSaveData data = new MainShipShieldSaveData();
            data.CurrentState = info.currentState;
            data.Direction = info.direction;
            data.ShieldLevel_current = info.shieldLevel_current;

            data.Shield_open_init = info.shield_open_init;
            data.ShieldOpenInitDetailPac = info.shieldOpenInitDetailPac;

            data.Shield_max = info.shield_max;
            data.Shield_current = info.shield_current;
            data.ShieldMaxDetailPac = info.shieldMaxDetailPac;

            data.ShieldChargeSpeed = info.shieldChargeSpeed;
            data.ShieldChargeSpeedDetailPac = info.shieldChargeSpeedDetailPac;

            data.EnergyCost_current = info.EnergyCost_current;
            data.EnergyCostDetailPac = info.energyCostDetailPac;

            data.DamageReduce_base = info.damageReduce_base;
            data.DamageReduceDetailPac = info.damageReduceDetailPac;

            data.DamageReduce_probability_base = info.damageReduce_probability_base;
            data.DamageReduceProbabilityDetailPac = info.damageReduceProbabilityDetailPac;

            data.ShieldLayer_current = info.shieldLayer_current;
            data.ShieldLayer_max = info.shieldLayer_max;

            data.equipedShieldAssembleUIDList = new List<ushort>();
            for(int i = 0; i < info.equipShieldAssembleList.Count; i++)
            {
                data.equipedShieldAssembleUIDList.Add(info.equipShieldAssembleList[i].UID);
            }
            return data;
        }

    }

    #endregion
    /*
     * Base Info
     */
    public class MainShipAreaBaseInfo
    {
        public MainShipAreaState areaState = MainShipAreaState.None;

        public string areaIconPath;

        /// <summary>
        /// area durability
        /// </summary>
        public int durability_max;
        public int durability_current;

        public ModifierDetailPackage_Mix durabilityMaxDetailPac = new ModifierDetailPackage_Mix();

        public bool ChangeAreaDurability(int value)
        {
            durability_current += value;
            if (durability_current > durability_max)
            {
                durability_current = durability_max;
                return false;
            }
            if (durability_current < 0)
            {
                durability_current = 0;
                return false;
            }
            return true;
        }

        public bool ChangeAreaDurability_Max(ModifierDetailRootType_Mix rootType, uint instanceID, int blockID, int value)
        {
            durability_max += value;
            if (durability_max < 0)
            {
                durabilityMaxDetailPac.ValueChange_Block(rootType, instanceID, blockID, value - durability_max);
                durability_max = 0;
                return false;
            }
            else
            {
                durabilityMaxDetailPac.ValueChange_Block(rootType, instanceID, blockID, value);
                return true;
            }
                
        }
        public bool ChangeAreaDurability_Max(ModifierDetailRootType_Mix rootType, int value)
        {
            durability_max += value;
            if (durability_max < 0)
            {
                durabilityMaxDetailPac.ValueChange(rootType, value - durability_max);
                durability_max = 0;
                return false;
            }
            else
            {
                durabilityMaxDetailPac.ValueChange(rootType, value);
                return true;
            }
                
        }

        /// <summary>
        /// 能源等级最大值
        /// </summary>
        public byte powerLevel_max;
        /// <summary>
        /// 能源等级当前分配
        /// </summary>
        public short powerLevel_current;
        public ModifierDetailPackage_Mix powerLevelMaxDetailPac = new ModifierDetailPackage_Mix();
        public bool ChangePowerLevelMax(ModifierDetailRootType_Mix rootType, uint instanceID, int blockID, byte value)
        {
            var maxValue = Config.ConfigData.MainShipConfigData.areaEnergyLevelMax;
            powerLevel_max += value;
            if (powerLevel_max > maxValue)
            {
                powerLevelMaxDetailPac.ValueChange_Block(rootType, instanceID, blockID, powerLevel_max- maxValue);
                powerLevel_max = maxValue;
                return false;
            }
            else
            {
                powerLevelMaxDetailPac.ValueChange_Block(rootType, instanceID, blockID, value);
                return true;
            }
        }
        public bool ChangePowerLevelMax(ModifierDetailRootType_Mix rootType,byte value)
        {
            var maxValue = Config.ConfigData.MainShipConfigData.areaEnergyLevelMax;
            powerLevel_max += value;
            if (powerLevel_max > maxValue)
            {
                powerLevelMaxDetailPac.ValueChange(rootType, powerLevel_max- maxValue);
                powerLevel_max = maxValue;
                return false;
            }
            else
            {
                powerLevelMaxDetailPac.ValueChange(rootType, value);
                return true;
            }     
        }

        /// <summary>
        /// EnergyCost
        /// </summary>
        public ushort powerConsumeBase;
        public ushort powerConsumeExtra;
        /// Add Rate
        public float powerConsumeRate
        {
            get
            {
                float ValueInitial = 1.0f;
                foreach(float value in energyCostRateAddDetail.Values)
                {
                    ValueInitial += value;
                }
                return ValueInitial;
            }
        }

        /// <summary>
        /// 倍率加成详情
        /// </summary>
        public Dictionary<MainShipAreaEnergyCostType, float> energyCostRateAddDetail =new Dictionary<MainShipAreaEnergyCostType, float>();

        public ushort powerConsumeCurrent
        {
            get
            {
                if(areaState == MainShipAreaState.Working)
                {
                    return (ushort)(powerConsumeBase* powerConsumeRate + powerConsumeExtra);
                }
                else
                {
                    return 0;
                }
            }
        }

        public void UpdateAreaState()
        {
            if (powerLevel_current == 0)
                areaState = MainShipAreaState.LayOff;
            else if (powerLevel_current > 0)
                areaState = MainShipAreaState.Working;
        }

        /// <summary>
        ///  Same Type will Cover Value
        /// </summary>
        /// <param name="value"></param>
        /// <param name="costType"></param>
        public void ChangePowerConsumeRate(float value,MainShipAreaEnergyCostType costType)
        {
            if (energyCostRateAddDetail.ContainsKey(costType))
            {
                energyCostRateAddDetail[costType] = value;
            }
            else
            {
                energyCostRateAddDetail.Add(costType, value);
            }
        }

        public bool LoadGameSaveData(MainShipAreaGeneralSaveData saveData)
        {
            if (saveData == null)
                return false;
            durability_max = saveData.Durability_max;
            durability_current = saveData.Durability_current;
            durabilityMaxDetailPac = saveData.DurabilityMaxDetailPac;

            return true;
        }
    }

    /// <summary>
    /// General Area SaveData
    /// </summary>
    public class MainShipAreaGeneralSaveData
    {
        public int Durability_max;
        public int Durability_current;
        public ModifierDetailPackage_Mix DurabilityMaxDetailPac;

        public MainShipAreaGeneralSaveData CreateSave(MainShipAreaBaseInfo baseinfo)
        {
            MainShipAreaGeneralSaveData data = new MainShipAreaGeneralSaveData();
            data.Durability_max = baseinfo.durability_max;
            data.Durability_current = baseinfo.durability_current;
            data.DurabilityMaxDetailPac = baseinfo.durabilityMaxDetailPac;
            return data;
        }
    }

    #region Power Area
    public class MainShipPowerAreaInfo
    {

        public enum EnergyGenerateMode
        {
            Normal,
            Overload
        }

        public string areaIconPath;

        /// <summary>
        /// Area duribility
        /// </summary>
        public int durability_max;
        public int durability_current;
        public ModifierDetailPackage_Mix durabilityMaxDetailPac = new ModifierDetailPackage_Mix();
        
        public void ChangeAreaDurability(int value)
        {
            durability_current += value;
            if (durability_current > durability_max)
                durability_current = durability_max;
            if (durability_current < 0)
                durability_current = 0;
        }

        public bool ChangeAreaDurability_Max(ModifierDetailRootType_Mix rootType,uint instanceID,int blockID,int value)
        {
            durability_max += value;
            if (durability_max < 0)
            {
                durabilityMaxDetailPac.ValueChange_Block(rootType, instanceID, blockID, value - durability_max);
                durability_max = 0;
                return false;
            }
            else
            {
                durabilityMaxDetailPac.ValueChange_Block(rootType, instanceID, blockID, value);
                return true;
            }
        }
        public bool ChangeAreaDurability_Max(ModifierDetailRootType_Mix rootType,int value)
        {
            durability_max += value;
            if (durability_max < 0)
            {
                durabilityMaxDetailPac.ValueChange(rootType, value - durability_max);
                durability_max = 0;
                return false;
            }
            else
            {
                durabilityMaxDetailPac.ValueChange(rootType, value);
                return true;
            }
        }

        /// <summary>
        /// 电能产生效率
        /// </summary>
        public short PowerGenerateValue;
        public ModifierDetailPackage_Mix powerGenerateDetailPac = new ModifierDetailPackage_Mix();
        public bool ChangePowerGenerateValue(ModifierDetailRootType_Mix rootType, uint instanceID, int blockID, short value)
        {
            PowerGenerateValue += value;
            if (PowerGenerateValue < 0)
            {
                powerGenerateDetailPac.ValueChange_Block(rootType, instanceID, blockID, value - PowerGenerateValue);
                PowerGenerateValue = 0;
                return false;
            }
            else
            {
                powerGenerateDetailPac.ValueChange_Block(rootType, instanceID, blockID, value);
                return true;
            }
        }
        public bool ChangePowerGenerateValue(ModifierDetailRootType_Mix rootType, short value)
        {
            PowerGenerateValue += value;
            if (PowerGenerateValue < 0)
            {
                powerGenerateDetailPac.ValueChange(rootType, value - PowerGenerateValue);
                PowerGenerateValue = 0;
                return false;
            }
            else
            {
                powerGenerateDetailPac.ValueChange(rootType, value);
                return true;
            }                
        }

        /// <summary>
        /// 能源负载，用于其余舱室负载分配
        /// </summary>
        public short energyLoadValue_max;
        public short energyLoadValue_current;
        public ModifierDetailPackage_Mix energyLoadMaxDetailPac = new ModifierDetailPackage_Mix();
        public bool ChangeEnergyLoadValue_Max(ModifierDetailRootType_Mix rootType,uint instanceID,int blockID, short value)
        {
            energyLoadValue_max += value;
            if (energyLoadValue_max < 0)
            {
                energyLoadMaxDetailPac.ValueChange_Block(rootType, instanceID, blockID, value - energyLoadValue_max);
                energyLoadValue_max = 0;
                return false;
            }
            else
            {
                energyLoadMaxDetailPac.ValueChange_Block(rootType, instanceID, blockID, value);
                return true;
            }
        }
        public bool ChangeEnergyLoadValue_Max(ModifierDetailRootType_Mix rootType,short value)
        {
            energyLoadValue_max += value;
            if (energyLoadValue_max < 0)
            {
                energyLoadMaxDetailPac.ValueChange(rootType, value - energyLoadValue_max);
                energyLoadValue_max = 0;
                return false;
            }
            else
            {
                energyLoadMaxDetailPac.ValueChange(rootType, value);
                return true;
            }
        }
        ///!!!!!! TODO  更换最大值时，如果是减少，目前分配给其他舱室的能源需要更新

        /// <summary>
        /// 能源负载详细分配
        /// </summary>
        public ModifierDetailPackage energyLoadDetailPac = new ModifierDetailPackage();

        /// <summary>
        /// False = Change Faild
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public bool ChangeEnergyLoadValue(short count)
        {
            energyLoadValue_current += count;
            if (energyLoadValue_current > energyLoadValue_max)
            {
                energyLoadValue_current = energyLoadValue_max;
                return true;
            }
            else if (energyLoadValue_current < 0)
            {
                energyLoadValue_current = 0;
                return false;
            }
            return true;
        }
        /// <summary>
        /// 各个区域使用的能源详情
        /// </summary>
        /// <param name="type"></param>
        /// <param name="changeValue"></param>
        public void RefreshEnergyLoadDetail(ModifierDetailRootType_Simple type, short changeValue)
        {
            energyLoadDetailPac.ValueChange(type, changeValue);
        }


        /// <summary>
        /// Power Storage
        /// </summary>
        public int storagePower_max;
        public int storagePower_current=0;
        public ModifierDetailPackage_Mix storageMaxDetailPac = new ModifierDetailPackage_Mix();

        public bool AddMaxStoragePower(ModifierDetailRootType_Mix rootType,uint instanceID,int blockID, int value)
        {
            storagePower_max += value;
            if (storagePower_max < 0)
            {
                storageMaxDetailPac.ValueChange_Block(rootType, instanceID, blockID, value- storagePower_max);
                ChangeCurrentStoragePower(0);
                storagePower_max = 0;
                return false;
            }
            else
            {
                storageMaxDetailPac.ValueChange_Block(rootType, instanceID, blockID, value);
                ChangeCurrentStoragePower(0);
                return true;
            }
                
        }
        public bool AddMaxStoragePower(ModifierDetailRootType_Mix rootType,int value)
        {
            storagePower_max += value;
            if (storagePower_max < 0)
            {
                storageMaxDetailPac.ValueChange(rootType, value -storagePower_max);
                ChangeCurrentStoragePower(0);
                storagePower_max = 0;
                return false;
            }
            else
            {
                storageMaxDetailPac.ValueChange(rootType, value);
                ChangeCurrentStoragePower(0);
                return true;
            }
        }

        public void ChangeCurrentStoragePower(int value)
        {
            storagePower_current += value;
            if (storagePower_current > storagePower_max)
                storagePower_current = storagePower_max;
            if (storagePower_current < 0)
                storagePower_current = 0;
        }

        /// <summary>
        /// Over Load
        /// </summary>
        public short overLoadLevel_current = 0;
        public short overLoadLevel_max;
        public ModifierDetailPackage_Mix overLoadLevelMaxDetailPac = new ModifierDetailPackage_Mix();
        public EnergyGenerateMode currentMode { get; protected set; }
        //是否解锁过载模式
        public bool unLockOverLoadMode = false;

        public bool ChangeOverLoadLevelMax(ModifierDetailRootType_Mix rootType, uint instanceID, int blockID, short value)
        {
            overLoadLevel_max += value;
            if (overLoadLevel_max < 0)
            {
                overLoadLevelMaxDetailPac.ValueChange_Block(rootType, instanceID, blockID, value - overLoadLevel_max);
                ChangeOverLoadLevel(0);
                overLoadLevel_max = 0;
                return false;
            }
            else
            {
                overLoadLevelMaxDetailPac.ValueChange_Block(rootType, instanceID, blockID, value);
                ChangeOverLoadLevel(0);
                return true;
            } 
        }

        public bool ChangeOverLoadLevelMax(ModifierDetailRootType_Mix rootType, short value)
        {
            overLoadLevel_max += value;
            if (overLoadLevel_max < 0)
            {
                overLoadLevelMaxDetailPac.ValueChange(rootType, value - overLoadLevel_max);
                ChangeOverLoadLevel(0);
                overLoadLevel_max = 0;
                return false;
            }
            else
            {
                overLoadLevelMaxDetailPac.ValueChange(rootType, value);
                ChangeOverLoadLevel(0);
                return true;
            }
        }

        /// <summary>
        /// 更改过载等级
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public bool ChangeOverLoadLevel(byte level=1)
        {
            overLoadLevel_current += level;
            if (overLoadLevel_current > overLoadLevel_max)
                overLoadLevel_current = overLoadLevel_max;
            if (energyLoadValue_current < 0)
                energyLoadValue_current = 0;

            if (MainShipModule.GetPowerAreaOverLoadMap(overLoadLevel_current) == null)
            {
                DebugPlus.LogError("[PowerAreaOverLoadMap] : GetMap Error ! currentLevel=" + overLoadLevel_current);
                overLoadLevel_current -= level;
                return false;
            }
            return true;
        }
        public void ChangeEnergyMode(EnergyGenerateMode mode)
        {
            currentMode = mode;
        }

        /// <summary>
        /// Modifier
        /// </summary>
        public MainShipAreaModifier areaModifier;


        public MainShipPowerAreaInfo() { }
        public bool InitData()
        {
            var config = Config.ConfigData.MainShipConfigData.powerAreaConfig;
            if (config != null)
            {
                areaIconPath = config.areaIconPath;
                PowerGenerateValue = config.energyGenerateBase;
                ChangeEnergyLoadValue_Max(ModifierDetailRootType_Mix.OriginConfig,config.energyLoadBase);
                energyLoadValue_current = energyLoadValue_max;
                AddMaxStoragePower(ModifierDetailRootType_Mix.OriginConfig, config.MaxStorageCountBase);
                ChangeEnergyMode(EnergyGenerateMode.Normal);
                unLockOverLoadMode = config.unlockOverLoad;
                ChangeOverLoadLevelMax(ModifierDetailRootType_Mix.OriginConfig, config.overLoadLevelMax);
                areaModifier = new MainShipAreaModifier(ModifierTarget.MainShipPowerArea);
                return true;
            }
            DebugPlus.LogError("[MainShipPowerAreaInfo] : InitData Fail ! config is null!");
            return false;
        }


        public bool LoadSaveData(MainShipPowerAreaSaveData saveData)
        {
            var config = Config.ConfigData.MainShipConfigData.powerAreaConfig;
            if (config == null)
            {
                DebugPlus.LogError("[MainShipPowerAreaSaveData] : save Data Error!");
            }
            areaIconPath = config.areaIconPath;

            durability_current = saveData.Durability_current;
            durability_max = saveData.Durability_current;
            durabilityMaxDetailPac = saveData.DurabilityMaxDetailPac;

            PowerGenerateValue = saveData.PowerGenerateValue;
            powerGenerateDetailPac = saveData.PowerGenerateDetailPac;

            energyLoadValue_max = saveData.EnergyLoadValueMax;
            energyLoadValue_current = saveData.EnergyLoadValueCurrent;
            currentMode = saveData.CurrentMode;
            energyLoadDetailPac = saveData.EnergyLoadDetailPac;
            energyLoadMaxDetailPac = saveData.EnergyLoadMaxDetailPac;

            storagePower_max = saveData.StoragePower_max;
            storagePower_current = saveData.StoragePower_max;
            storageMaxDetailPac = saveData.StorageMaxDetailPac;

            overLoadLevel_current = saveData.OverLoadLevel_current;
            overLoadLevel_max = saveData.OverLoadLevel_max;
            overLoadLevelMaxDetailPac = saveData.OverLoadLevelMaxDetailPac;
            return true;
        }
    }

    /// <summary>
    /// Save Data
    /// </summary>
    public class MainShipPowerAreaSaveData
    {
        public int Durability_max;
        public int Durability_current;
        public ModifierDetailPackage_Mix DurabilityMaxDetailPac;

        /// <summary>
        /// 电能产生效率
        /// </summary>
        public short PowerGenerateValue;
        public ModifierDetailPackage_Mix PowerGenerateDetailPac;
        /// <summary>
        /// 能源负载，用于其余舱室负载分配
        /// </summary>
        public short EnergyLoadValueMax;
        public short EnergyLoadValueCurrent;
        public MainShipPowerAreaInfo.EnergyGenerateMode CurrentMode;
        public ModifierDetailPackage EnergyLoadDetailPac;
        public ModifierDetailPackage_Mix EnergyLoadMaxDetailPac;

        public int StoragePower_max;
        public int StoragePower_current;
        public ModifierDetailPackage_Mix StorageMaxDetailPac;


        public short OverLoadLevel_current;
        public short OverLoadLevel_max;
        public ModifierDetailPackage_Mix OverLoadLevelMaxDetailPac;
        public bool UnLockOverLoadMode;

        public static MainShipPowerAreaSaveData CreateSave(MainShipPowerAreaInfo info)
        {
            MainShipPowerAreaSaveData data = new MainShipPowerAreaSaveData();
            data.Durability_max = info.durability_max;
            data.Durability_current = info.durability_current;
            data.DurabilityMaxDetailPac = info.durabilityMaxDetailPac;

            data.PowerGenerateValue = info.PowerGenerateValue;
            data.PowerGenerateDetailPac = info.powerGenerateDetailPac;

            data.EnergyLoadValueMax = info.energyLoadValue_max;
            data.EnergyLoadValueCurrent = info.energyLoadValue_current;
            data.EnergyLoadDetailPac = info.energyLoadDetailPac;
            data.EnergyLoadMaxDetailPac = info.energyLoadMaxDetailPac;
            data.CurrentMode = info.currentMode;

            data.StoragePower_max = info.storagePower_max;
            data.StoragePower_current = info.storagePower_current;
            data.StorageMaxDetailPac = info.storageMaxDetailPac;

            data.OverLoadLevel_current = info.overLoadLevel_current;
            data.OverLoadLevel_max = info.overLoadLevel_max;
            data.OverLoadLevelMaxDetailPac = info.overLoadLevelMaxDetailPac;
            return data;
        }
    }

    #endregion
    public class MainShipControlTowerInfo: MainShipAreaBaseInfo
    {
        public MainShipControlTowerInfo()
        {
            var config = Config.ConfigData.MainShipConfigData.controlTowerAreaConfig;
            if (config != null)
            {
                areaIconPath = config.baseConfig.areaIconPath;
                durability_max = config.baseConfig.Durability_Initial;
                durability_current = durability_max;
                powerLevel_max = config.baseConfig.PowerLevel_Max_Initial;
                powerLevel_current = config.baseConfig.PowerLevel_Current_Initial;
                powerConsumeBase = (ushort)config.baseConfig.PowerConsumeBase;
                UpdateAreaState();
                RefreshPowerCost();
            }
        }

        public void ChangePowerLevel(short level = 1)
        {
            powerLevel_current += level;
            if (powerLevel_current > powerLevel_max)
                powerLevel_current = powerLevel_max;
            else if (powerLevel_current < 0)
                powerLevel_current = 0;

            if (MainShipModule.GetControlTowerAreaEnergyLevelMapData(powerLevel_current) == null)
            {
                DebugPlus.LogError("[ControlTowerArea] : EnergyLevelMap Error! currentLevel=" + powerLevel_current);
                powerLevel_current -= level;
            }
                
            UpdateAreaState();
            RefreshPowerCost();
        }

        void RefreshPowerCost()
        {
            if (areaState == MainShipAreaState.Working)
            {
                var levelData = MainShipModule.GetControlTowerAreaEnergyLevelMapData(powerLevel_current);
                if (levelData != null)
                {
                    ChangePowerConsumeRate((float)levelData.energyCostRate, MainShipAreaEnergyCostType.EnergyLevel);
                }
            }
            UIManager.Instance.SendMessage(new UIMessage(UIMsgType.MainShip_Area_PowerLevel_Change, new List<object>() { MainShipAreaType.ControlTower }));
        }

    }

    public class MainShipLivingAreaInfo: MainShipAreaBaseInfo
    {

        public MainShipLivingAreaInfo()
        {
            var config = Config.ConfigData.MainShipConfigData.livingAreaConfig;
            if (config != null)
            {
                areaIconPath = config.baseConfig.areaIconPath;
                durability_max = config.baseConfig.Durability_Initial;
                durability_current = durability_max;
                powerLevel_max = config.baseConfig.PowerLevel_Max_Initial;
                powerLevel_current = config.baseConfig.PowerLevel_Current_Initial;
                powerConsumeBase = (ushort)config.baseConfig.PowerConsumeBase;
                UpdateAreaState();
                RefreshPowerCost();
            }
        }
        public void ChangePowerLevel(short level = 1)
        {
            powerLevel_current += level;
            if (powerLevel_current > powerLevel_max)
                powerLevel_current = powerLevel_max;
            else if (powerLevel_current < 0)
                powerLevel_current = 0;

            UpdateAreaState();
            RefreshPowerCost();
        }

        void RefreshPowerCost()
        {
            if (areaState == MainShipAreaState.Working)
            {
                var levelData = MainShipModule.GetLivingAreaEnergyLevelMapData(powerLevel_current);
                if (levelData != null)
                {
                    ChangePowerConsumeRate((float)levelData.energyCostRate, MainShipAreaEnergyCostType.EnergyLevel);
                }
            }
            UIManager.Instance.SendMessage(new UIMessage(UIMsgType.MainShip_Area_PowerLevel_Change, new List<object>() { MainShipAreaType.LivingArea }));
        }

    }

    public class MainShipHangarInfo: MainShipAreaBaseInfo
    {

        public MainShipHangarInfo()
        {
            var config = Config.ConfigData.MainShipConfigData.hangarAreaConfig;
            if (config != null)
            {
                areaIconPath = config.baseConfig.areaIconPath;
                durability_max = config.baseConfig.Durability_Initial;
                durability_current = durability_max;
                powerLevel_max = config.baseConfig.PowerLevel_Max_Initial;
                powerLevel_current = config.baseConfig.PowerLevel_Current_Initial;
                powerConsumeBase = (ushort)config.baseConfig.PowerConsumeBase;
                UpdateAreaState();
                RefreshPowerCost();
            }
        }

        public void ChangePowerLevel(short level = 1)
        {
            powerLevel_current += level;
            if (powerLevel_current > powerLevel_max)
                powerLevel_current = powerLevel_max;
            else if (powerLevel_current < 0)
                powerLevel_current = 0;

            UpdateAreaState();
            RefreshPowerCost();
        }

        void RefreshPowerCost()
        {
            if (areaState == MainShipAreaState.Working)
            {
                var levelData = MainShipModule.GetHangarAreaEnergyLevelMapData(powerLevel_current);
                if (levelData != null)
                {
                    ChangePowerConsumeRate((float)levelData.energyCostRate, MainShipAreaEnergyCostType.EnergyLevel);
                }
            }
            UIManager.Instance.SendMessage(new UIMessage(UIMsgType.MainShip_Area_PowerLevel_Change, new List<object>() { MainShipAreaType.hangar }));
        }
    }

    public class MainShipWorkingAreaInfo: MainShipAreaBaseInfo
    {
        public MainShipAreaModifier areaModifier;
        public MainShipWorkingAreaInfo()
        {
            var config = Config.ConfigData.MainShipConfigData.workingAreaConfig;
            if (config != null)
            {
                areaIconPath = config.baseConfig.areaIconPath;
                durability_max = config.baseConfig.Durability_Initial;
                durability_current = durability_max;
                powerLevel_max = config.baseConfig.PowerLevel_Max_Initial;
                powerLevel_current = config.baseConfig.PowerLevel_Current_Initial;
                powerConsumeBase = (ushort)config.baseConfig.PowerConsumeBase;
                UpdateAreaState();
                RefreshPowerCost();

                areaModifier = new MainShipAreaModifier(ModifierTarget.MainShipWorkingArea);
            }
        }

        public void ChangePowerLevel(short level = 1)
        {
            powerLevel_current += level;
            if (powerLevel_current > powerLevel_max)
                powerLevel_current = powerLevel_max;
            else if (powerLevel_current < 0)
                powerLevel_current = 0;

            UpdateAreaState();
            RefreshPowerCost();
        }

        void RefreshPowerCost()
        {
            if (areaState == MainShipAreaState.Working)
            {
                var levelData = MainShipModule.GetWorkingAreaEnergyLevelMapData(powerLevel_current);
                if (levelData != null)
                {
                    ChangePowerConsumeRate((float)levelData.energyCostRate, MainShipAreaEnergyCostType.EnergyLevel);
                }
            }
            UIManager.Instance.SendMessage(new UIMessage(UIMsgType.MainShip_Area_PowerLevel_Change, new List<object>() { MainShipAreaType.WorkingArea }));
        }
    }

    #region Game SaveData

    public class MainShipSaveData
    {
        public Dictionary<MainShip_ShieldDirection,MainShipShieldSaveData> shieldSaveDataDic =new Dictionary<MainShip_ShieldDirection, MainShipShieldSaveData> ();

        public MainShipPowerAreaSaveData powerAreaSaveData;

        public short ShieldEnergy_Max_current;
        public ModifierDetailPackage_Mix ShieldEnergyDetailPac;

        public static MainShipSaveData CreateSave()
        {
            var info = MainShipManager.Instance.mainShipInfo;
            MainShipSaveData data = new MainShipSaveData();
            data.ShieldEnergy_Max_current = info.shieldEnergy_Max_current;
            data.ShieldEnergyDetailPac = info.shieldEnergyDetailPac;

            foreach(var shield in info.shieldInfoDic)
            {
                MainShipShieldSaveData shieldSaveData = MainShipShieldSaveData.CreateSave(shield.Value);
                data.shieldSaveDataDic.Add(shield.Key, shieldSaveData);
            }
            
            data.powerAreaSaveData = MainShipPowerAreaSaveData.CreateSave(info.powerAreaInfo);
            return data;
        }
    }

    #endregion
}