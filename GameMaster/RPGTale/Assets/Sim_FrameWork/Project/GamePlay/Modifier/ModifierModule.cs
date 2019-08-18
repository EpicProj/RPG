using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork {

    public class ModifierModule : Singleton<ModifierModule> {





        public List<ModifierData> modifierData = new List<ModifierData>();
        public Dictionary<string, ModifierData> modifierDataDic = new Dictionary<string, ModifierData>();
        public GeneralModifier generalModifier = new GeneralModifier();

        public void InitData()
        {
            generalModifier.ReadModifierData();
            modifierData = generalModifier.ModifierData;
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
    public class ModifierData
    {
        public string ModifierName;
        public int ActionType;
        public List<ModifierEffect> Effects;
    }
    public class ModifierEffect
    {
        public string AttributeName;
        public float AddValue;
        public List<string> paramList;
    }

}