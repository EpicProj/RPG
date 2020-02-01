using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork {
    public class ModifierManager : MonoSingleton<ModifierManager> {

        public List<MainShipAreaModifier> shipAreaModifierList;
        public List<FunctionBlockModifier> blockModifierList;

        private GeneralModifier generalModifier;
  
        protected override void Awake()
        {
            base.Awake();
            InitData();
        }

        public void InitData()
        {
            shipAreaModifierList = new List<MainShipAreaModifier>();
            blockModifierList = new List<FunctionBlockModifier>();
            generalModifier = new GeneralModifier();   
            generalModifier.LoadModifierData();
        }

        private void LateUpdate()
        {
            UpdateAreaModifier();
            UpdateBlockModifier();
        }

        void UpdateAreaModifier()
        {
            for (int i = 0; i < shipAreaModifierList.Count; i++)
            {
                if (shipAreaModifierList[i].target == ModifierTarget.MainShipPowerArea && MainShipManager.Instance.PowerArea_Active)
                {
                    shipAreaModifierList[i].UpdateModifier();
                }
                if (shipAreaModifierList[i].target == ModifierTarget.MainShipWorkingArea && MainShipManager.Instance.WorkingArea_Active)
                {
                    shipAreaModifierList[i].UpdateModifier();
                }
            }
        }

        void UpdateBlockModifier()
        {
            for(int i = 0; i < blockModifierList.Count; i++)
            {
                blockModifierList[i].UpdateModifier();
            }
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

        /// <summary>
        /// Register ShipArea
        /// </summary>
        /// <param name="modifier"></param>
        public void RegisterShipAreaModifier(MainShipAreaModifier modifier)
        {
            shipAreaModifierList.Add(modifier);
        }

        public void UnRegisterShipAreaModifier(MainShipAreaModifier modifier)
        {
            if (shipAreaModifierList.Contains(modifier))
                shipAreaModifierList.Remove(modifier);
        }


        #region Block
        public FunctionBlockModifier GetBlockModifierByInstanceID(int instanceID)
        {
            return blockModifierList.Find(x => x.instanceID == instanceID);
        }

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

        public void AddMainShipPowerAreaModifier(FunctionBlockBase block, MainShipPowerAreaInfo areaInfo, string modifierName)
        {
            AddMainShipPowerAreaModifier(block, areaInfo, GetModifierBase(modifierName));
        }

        public void AddMainShipPowerAreaModifier(FunctionBlockBase block, MainShipPowerAreaInfo areaInfo, ModifierBase modifierBase)
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
                    if (!IsAddPowerAreaModifier(areaInfo, modifierBase))
                    {
                        data = ModifierData.Create(modifierBase, delegate
                        {
                            areaInfo.AddMaxStoragePower(block.info.modifierRootType, block.instanceID,block.info.BlockID,(int)modifierBase.Value);
                        });
                    }
                    break;
                case ModifierMainShip_PowerArea.AreaDurability:
                    if (!IsAddPowerAreaModifier(areaInfo, modifierBase))
                    {
                        data = ModifierData.Create(modifierBase, delegate
                         {
                             areaInfo.ChangeAreaDurability_Max(block.info.modifierRootType, block.instanceID, block.info.BlockID, (int)modifierBase.Value);
                         });
                    }
                    break;
            }

            if (data != null)
            {
                ///ADD modifier
                areaInfo.areaModifier.OnAddModifier(data);
            }
        }

        private bool IsAddPowerAreaModifier(MainShipPowerAreaInfo info, ModifierBase modifier)
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