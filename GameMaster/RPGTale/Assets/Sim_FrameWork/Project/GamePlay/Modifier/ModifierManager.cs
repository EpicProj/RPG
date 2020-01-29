using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork {
    public class ModifierManager : Singleton<ModifierManager> {

        public GeneralModifier generalModifier;

        public void InitData()
        {
            generalModifier = new GeneralModifier();
            generalModifier.LoadModifierData();
        }

        public ModifierBase GetModifierBase(string name)
        {
            var modifier = generalModifier.ModifierBase.Find(x => x.ModifierName == name);
            if (modifier == null)
            {
                Debug.LogError("Can not Find Modifier,Name=" + name);
            }
            return modifier;
        }

        #region Block
        public void AddManufactBlockModifier(ManufactoryInfo manuInfo, FunctionBlockInfoData baseInfo,string modifierName)
        {
            AddManufactoryBlockModifier(manuInfo, baseInfo, GetModifierBase(modifierName));
        }

        /// <summary>
        /// General FuntionBlock
        /// </summary>
        /// <param name="infoData"></param>
        /// <param name="modifierBase"></param>
        public void AddManufactoryBlockModifier(ManufactoryInfo manuInfo,FunctionBlockInfoData baseInfo, ModifierBase modifierBase)
        {
            if (modifierBase == null)
            {
                Debug.LogError("Modifier Base Data is null");
                return;
            }
            if (modifierBase.ParseTargetType(modifierBase.Target) != ModifierTarget.FunctionBlock)
            {
                Debug.LogError("ModifierTargetError  Name=" + modifierBase.ModifierName);
                return;
            }

            ModifierData data = null;

            switch (modifierBase.ParseModifierFunctionBlockType(modifierBase.effectType))
            {
                case ModifierFunctionBlockType.ManuSpeed:
                    //Modifier Speed
                    if (!IsAddFunctionBlockModifier(baseInfo, modifierBase))
                    {
                        data = ModifierData.Create(modifierBase, delegate
                         {
                             manuInfo.AddCurrentSpeed(modifierBase.Value);
                             UIManager.Instance.SendMessageToWnd(UIPath.WindowPath.BlockManu_Page,new UIMessage(UIMsgType.UpdateSpeedText, new List<object>(1) { manuInfo.CurrentSpeed }));
                         });
                    }
                    break;
                case ModifierFunctionBlockType.EnergyCostNormal:
                    //Modifier EnergyCost
                    if (!IsAddFunctionBlockModifier(baseInfo, modifierBase))
                    {
                        data = ModifierData.Create(modifierBase, delegate
                         {
                             manuInfo.AddEnergyCostNormal(modifierBase.Value);
                         });
                    }
                    break;
            }

            if (data != null)
            {
                baseInfo.blockModifier.OnAddModifier(data);
            }
        }


        /// <summary>
        /// 是否已经拥有该Modifier
        /// </summary>
        /// <param name="info"></param>
        /// <param name="modifier"></param>
        /// <returns></returns>
        private bool IsAddFunctionBlockModifier(FunctionBlockInfoData info,ModifierBase modifier)
        {
            ModifierData oldData = info.blockModifier.GetModifierByID(modifier.ModifierName);
            if(oldData != null)
            {
                switch (modifier.OverlapType)
                {
                    case ModifierOverlapType.TimeReset:
                        oldData.ResetTime();
                        break;
                }
                return true;
            }
            return false;
        }
        #endregion

        #region MainShipArea

        public void AddMainShipPowerAreaModifier(MainShipPowerAreaInfo areaInfo, MainShipPowerArea baseInfo, string modifierName)
        {
            AddMainShipPowerAreaModifier(areaInfo, baseInfo, GetModifierBase(modifierName));
        }

        public void AddMainShipPowerAreaModifier(MainShipPowerAreaInfo areaInfo, MainShipPowerArea baseInfo, ModifierBase modifierBase)
        {
            if (modifierBase == null)
            {
                Debug.LogError("Modifier Base Data is null");
                return;
            }
            if (modifierBase.ParseTargetType(modifierBase.Target) != ModifierTarget.MainShipPowerArea)
            {
                Debug.LogError("ModifierTargetError  Name=" + modifierBase.ModifierName);
                return;
            }

            ModifierData data = null;

            switch (modifierBase.ParseModifierPowerAreaType(modifierBase.effectType))
            {
                case ModifierMainShip_PowerArea.EnergyStorageMax:
                    //Modifier Speed
                    if (!IsAddPowerAreaModifier(baseInfo, modifierBase))
                    {
                        data = ModifierData.Create(modifierBase, delegate
                        {
                            areaInfo.AddMaxStoragePower((int)modifierBase.Value);
                        });
                    }
                    break;
            }

            if (data != null)
            {
                baseInfo.areaModifier.OnAddModifier(data);
            }
        }

        private bool IsAddPowerAreaModifier(MainShipPowerArea info, ModifierBase modifier)
        {
            ModifierData oldData = info.areaModifier.GetModifierByID(modifier.ModifierName);
            if (oldData != null)
            {
                switch (modifier.OverlapType)
                {
                    case ModifierOverlapType.TimeReset:
                        oldData.ResetTime();
                        break;
                }
                return true;
            }
            return false;
        }


        #endregion

    }


}