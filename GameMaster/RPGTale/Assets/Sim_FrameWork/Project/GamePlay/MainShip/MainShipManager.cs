﻿using System.Collections;
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

        private void Awake()
        {
            
        }

        public void InitData()
        {
            mainShipInfo = new MainShipInfo();
            mainShipInfo = mainShipInfo.InitInfo();
            InitPlayerEnergyData();
        }
      
        public bool ChangeAreaPowerLevel(short changeValue,MainShipAreaType type)
        {
            if (type == MainShipAreaType.ControlTower)
            {
                if (mainShipInfo.powerAreaInfo.ChangeEnergyLoadValue((short)-changeValue))
                {
                    mainShipInfo.controlTowerInfo.ChangePowerLevel(changeValue);
                    mainShipInfo.powerAreaInfo.RefreshEnergyLoadDetail(MainShipAreaType.PowerArea, mainShipInfo.controlTowerInfo.powerLevelCurrent);
                    return true;
                }
                return false; 
            }
            else if (type == MainShipAreaType.hangar)
            {
                if (mainShipInfo.powerAreaInfo.ChangeEnergyLoadValue((short)-changeValue))
                {
                    mainShipInfo.hangarAreaInfo.ChangePowerLevel(changeValue);
                    mainShipInfo.powerAreaInfo.RefreshEnergyLoadDetail(MainShipAreaType.hangar, mainShipInfo.hangarAreaInfo.powerLevelCurrent);
                    return true;
                }
                return false;
            }
            else if (type == MainShipAreaType.LivingArea)
            {
                if (mainShipInfo.powerAreaInfo.ChangeEnergyLoadValue((short)-changeValue))
                {
                    mainShipInfo.livingAreaInfo.ChangePowerLevel(changeValue);
                    mainShipInfo.powerAreaInfo.RefreshEnergyLoadDetail(MainShipAreaType.LivingArea, mainShipInfo.livingAreaInfo.powerLevelCurrent);
                    return true;
                }
                return false;
            }
            else if (type == MainShipAreaType.WorkingArea)
            {
                if (mainShipInfo.powerAreaInfo.ChangeEnergyLoadValue((short)-changeValue))
                {
                    mainShipInfo.workingAreaInfo.ChangePowerLevel(changeValue);
                    mainShipInfo.powerAreaInfo.RefreshEnergyLoadDetail(MainShipAreaType.WorkingArea, mainShipInfo.workingAreaInfo.powerLevelCurrent);
                    return true;
                }
                return false;
            }
            return false;
        }

        void InitPlayerEnergyData()
        {
            PlayerManager.Instance.AddEnergy(mainShipInfo.powerAreaInfo.PowerGenerateValue, ResourceAddType.month);
            PlayerManager.Instance.AddEnergy(mainShipInfo.powerAreaInfo.MaxStoragePower, ResourceAddType.max);
        }

        #region Modifier
        public void AddPowerAreaModifier(ModifierBase modifier)
        {
            ModifierManager.Instance.AddMainShipPowerAreaModifier(mainShipInfo.powerAreaInfo, modifier);
        }
        #endregion
    }
}