using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    /*
    * MainShip Manager
    * 
    */
namespace Sim_FrameWork
{
    public class MainShipManager :Singleton<MainShipManager>
    {

        public MainShipInfo mainShipInfo;

        public bool PowerArea_Active = true;
        public bool WorkingArea_Active = true;

        public void InitData()
        {
            mainShipInfo = new MainShipInfo();
            mainShipInfo.InitInfo();
            InitPlayerEnergyData();
        }


        /// <summary>
        /// 更换区域电量分配
        /// </summary>
        /// <param name="changeValue"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool ChangeAreaPowerLevel(short changeValue,ModifierDetailRootType_Simple type)
        {
            if (type == ModifierDetailRootType_Simple.ControlTower)
            {
                if (mainShipInfo.powerAreaInfo.ChangeEnergyLoadValue((short)-changeValue))
                {
                    mainShipInfo.controlTowerInfo.ChangePowerLevel(changeValue);
                    mainShipInfo.powerAreaInfo.RefreshEnergyLoadDetail(ModifierDetailRootType_Simple.PowerArea, changeValue);
                    return true;
                }
                return false; 
            }
            else if (type == ModifierDetailRootType_Simple.Hangar)
            {
                if (mainShipInfo.powerAreaInfo.ChangeEnergyLoadValue((short)-changeValue))
                {
                    mainShipInfo.hangarAreaInfo.ChangePowerLevel(changeValue);
                    mainShipInfo.powerAreaInfo.RefreshEnergyLoadDetail(ModifierDetailRootType_Simple.Hangar, changeValue);
                    return true;
                }
                return false;
            }
            else if (type == ModifierDetailRootType_Simple.LivingArea)
            {
                if (mainShipInfo.powerAreaInfo.ChangeEnergyLoadValue((short)-changeValue))
                {
                    mainShipInfo.livingAreaInfo.ChangePowerLevel(changeValue);
                    mainShipInfo.powerAreaInfo.RefreshEnergyLoadDetail(ModifierDetailRootType_Simple.LivingArea, changeValue);
                    return true;
                }
                return false;
            }
            else if (type == ModifierDetailRootType_Simple.WorkingArea)
            {
                if (mainShipInfo.powerAreaInfo.ChangeEnergyLoadValue((short)-changeValue))
                {
                    mainShipInfo.workingAreaInfo.ChangePowerLevel(changeValue);
                    mainShipInfo.powerAreaInfo.RefreshEnergyLoadDetail(ModifierDetailRootType_Simple.WorkingArea, changeValue);
                    return true;
                }
                return false;
            }
            return false;
        }

        public void EquipedShield()
        {

        }

        void InitPlayerEnergyData()
        {
            PlayerManager.Instance.AddEnergy_PerDay( ModifierDetailRootType_Simple.PowerArea,mainShipInfo.powerAreaInfo.PowerGenerateValue);
            PlayerManager.Instance.AddEnergy_Max( ModifierDetailRootType_Simple.PowerArea,mainShipInfo.powerAreaInfo.storagePower_max);
        }

        #region Modifier
        public void AddPowerAreaModifier(FunctionBlockBase block, ModifierBase modifier)
        {
            ModifierManager.Instance.AddMainShipPowerAreaModifier(block,mainShipInfo.powerAreaInfo, modifier);
        }

        public void AddShieldModifier(bool addAll,AssemblePartInfo partInfo,ModifierBase modifier,MainShip_ShieldDirection direction)
        {
            if (addAll)
            {
                foreach(var info in mainShipInfo.shieldInfoDic.Values)
                    ModifierManager.Instance.AddMainShipShieldModifier(partInfo, info, modifier);
            }
            else
            {
                ModifierManager.Instance.AddMainShipShieldModifier(partInfo, mainShipInfo.shieldInfoDic[direction], modifier);
            }
            
        }
        #endregion


        #region Battle
        public void ShipDamageTake(int damageValue)
        {

        }
        

        #endregion
    }
}