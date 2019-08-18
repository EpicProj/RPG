using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Sim_FrameWork {

    public class ModifierModule : Singleton<ModifierModule> {

        #region enum
        public enum ModifierAttributeType
        {
            BaseResouces,
            FunctionalBlock,
            Material,
            Error,
        }

        public enum ModifierBaseResources
        {
            Currency,
            Food,
            Energy,
            Laber,
            Reputation,
            Error
        }
      
        public enum ActionType
        {
            OneOff=0,
            Passive=1,
        }
        #endregion


        public List<ModifierData> modifierDataList = new List<ModifierData>();
        public Dictionary<string, ModifierData> modifierDataDic;
        public GeneralModifier generalModifier = new GeneralModifier();

        public void InitData()
        {
            generalModifier.ReadModifierData();
            modifierDataList = generalModifier.ModifierData;
            AddModifierDataToDic();
        }

        private void AddModifierDataToDic()
        {
            modifierDataDic = new Dictionary<string, ModifierData>();
            for(int i = 0; i < modifierDataList.Count; i++)
            {
                modifierDataDic.Add(modifierDataList[i].ModifierName, modifierDataList[i]);
            }
        }
        private bool CheckModifierAttributesValid(string typestr)
        {
            if (Enum.IsDefined(typeof(ModifierBaseResources), typestr) == false)
            {
                Debug.LogError("ModifierAttributes InValid! Type=" + typestr);
                return false;
            }
            return true;
        }
        private bool CheckModifierAttributeTypeValid(string typestr)
        {
            if (Enum.IsDefined(typeof(ModifierAttributeType), typestr) == false)
            {
                Debug.LogError("ModifierAttributeType InValid! Type=" + typestr);
                return false;
            }
            return true;
        }

        public ModifierBaseResources GetModifierAttributes(ModifierEffect effect)
        {
            if (CheckModifierAttributesValid(effect.AttributeName) == false)
                return ModifierBaseResources.Error;
            return (ModifierBaseResources)Enum.Parse(typeof(ModifierBaseResources), effect.AttributeName);
        }
        public ModifierAttributeType GetModifierAttributeType(ModifierEffect effect)
        {
            if (CheckModifierAttributeTypeValid(effect.AttributeType) == false)
                return ModifierAttributeType.Error;
            return (ModifierAttributeType)Enum.Parse(typeof(ModifierAttributeType), effect.AttributeName);
        }

        public ActionType GetModifierType(ModifierEffect effect)
        {
            switch (effect.ActionType)
            {
                case 0:
                    return ActionType.OneOff;
                case 1:
                    return ActionType.Passive;
                default:
                    Debug.LogError("GetModifierType Error!  ActionType="+effect.ActionType);
                    return ActionType.OneOff;
            }
        }
        public ModifierData GetModifierDataByName(string modifierName)
        {
            ModifierData data = null;
            modifierDataDic.TryGetValue(modifierName, out data);
            if (data == null)
                Debug.LogError("Get ModifierData Error  ,Name=" + modifierName);
            return data;
        }

        #region Function
        public void OnAddModifier(string modifierName)
        {
            ModifierData data= GetModifierDataByName(modifierName);
            foreach(var effect in data.Effects)
            {
                switch (GetModifierAttributes(effect))
                {
                    case ModifierBaseResources.Currency:
                        OnAddCurrency(GetModifierType(effect), effect.AddValue);
                        break;
                    case ModifierBaseResources.Food:
                        OnAddFood(GetModifierType(effect), effect.AddValue);
                        break;

                }
            }
        }

        public void OnAddCurrency(ActionType type,float addValue)
        {
            switch (type)
            {
                case ActionType.OneOff:
                    PlayerModule.Instance.Currency += addValue;
                    break;
                case ActionType.Passive:
                    break;
                default:
                    break;
            }
        }
        public void OnAddFood(ActionType type,float addValue)
        {
            switch (type)
            {
                case ActionType.OneOff:
                    PlayerModule.Instance.Food +=(int)addValue;
                    break;
            }
        }

        //Material
        public void OnAddMaterial(ActionType type,float addValue,int materialID)
        {
            switch (type)
            {
                case ActionType.OneOff:
                    //TODO
                    break;
            }
        }
        #endregion
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
    public class ModifierData
    {
        public string ModifierName;
        public List<ModifierEffect> Effects;
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