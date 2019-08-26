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
            //modifierBaseList = generalModifier.ModifierData;
        }


        public void DoFunctionBlockModifier(FuntionBlockInfoData info,string modifierName)
        {
            DoFunctionBlockModifier(info, GetModifierBase(modifierName));
        }

        /// <summary>
        /// General FuntionBlock
        /// </summary>
        /// <param name="infoData"></param>
        /// <param name="modifierBase"></param>
        public void DoFunctionBlockModifier(FuntionBlockInfoData info,ModifierBase modifierBase)
        {
            if (modifierBase == null)
            {
                Debug.LogError("Modifier Base Data is null");
                return;
            }
            if (modifierBase.Target != ModifierTarget.FunctionBlock)
            {
                Debug.LogError("ModifierTargetError  Name=" + modifierBase.ModifierName);
                return;
            }

            ModifierData data = null;

            switch (modifierBase.functionBlockType)
            {
                case ModifierFunctionBlockType.ManuSpeed:
                    //Modifier Speed
                    if (!IsAddFcuntionBlockModifier(info, modifierBase))
                    {
                        data = ModifierData.Create(modifierBase, delegate
                         {
                             info.AddCurrentSpeed(modifierBase.Num);
                         });
                    }
                    break;

            }

            if (data != null)
            {
                info.blockModifierList.OnAddModifier(data);
            }
        }


        /// <summary>
        /// 是否已经拥有该Modifier
        /// </summary>
        /// <param name="info"></param>
        /// <param name="modifier"></param>
        /// <returns></returns>
        private bool IsAddFcuntionBlockModifier(FuntionBlockInfoData info,ModifierBase modifier)
        {
            ModifierData oldData = info.blockModifierList.GetModifierByID(modifier.ModifierName);
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
            for(int i = 0; i < modifierBaseList.Count; i++)
            {
                if (modifierBaseList[i].ModifierName == name)
                {
                    return modifierBaseList[i];
                }
            }
            return null;
        }

 
    }

    public class GeneralModifier
    {
        public List<ModifierData> ModifierData;
        public void ReadModifierData()
        {
            Config.JsonReader reader = new Config.JsonReader();
            GeneralModifier modifer = reader.LoadModifierData();
            ModifierData = modifer.ModifierData;
        }
    }

    public class ModifierEffect
    {
        public int ActionType;
        public string AttributeName;
        public string AttributeType;
        public int MaterialID;
        public float AddValue;
        public List<string> paramList;
    }

}