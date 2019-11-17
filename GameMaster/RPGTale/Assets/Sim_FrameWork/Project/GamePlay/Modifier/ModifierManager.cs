using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork {
    public class ModifierManager : MonoSingleton<ModifierManager> {

        public List<ModifierBase> modifierBaseList = new List<ModifierBase>();
        public GeneralModifier generalModifier = new GeneralModifier();

        protected override void Awake()
        {
            base.Awake();

            generalModifier.ReadModifierData();
            modifierBaseList = generalModifier.ModifierBase;
        }


        public void DoManufactBlockModifier(ManufactoryInfo manuInfo, FunctionBlockInfoData baseInfo,string modifierName)
        {
            DoManufactoryBlockModifier(manuInfo, baseInfo, GetModifierBase(modifierName));
        }

        /// <summary>
        /// General FuntionBlock
        /// </summary>
        /// <param name="infoData"></param>
        /// <param name="modifierBase"></param>
        public void DoManufactoryBlockModifier(ManufactoryInfo manuInfo,FunctionBlockInfoData baseInfo, ModifierBase modifierBase)
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

            switch (modifierBase.ParseModifierFunctionBlockType(modifierBase.functionBlockType))
            {
                case ModifierFunctionBlockType.ManuSpeed:
                    //Modifier Speed
                    if (!IsAddFcuntionBlockModifier(baseInfo, modifierBase))
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
                    if (!IsAddFcuntionBlockModifier(baseInfo, modifierBase))
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
        private bool IsAddFcuntionBlockModifier(FunctionBlockInfoData info,ModifierBase modifier)
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



        public ModifierBase GetModifierBase(string name)
        {
            var modifier= modifierBaseList.Find(x => x.ModifierName == name);
            if (modifier == null)
            {
                Debug.LogError("Can not Find Modifier,Name=" + name);
            }
            return modifier;
        }

 
    }

  
}