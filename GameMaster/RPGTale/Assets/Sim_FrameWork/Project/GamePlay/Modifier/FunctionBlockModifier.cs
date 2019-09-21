using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork
{
    public class FunctionBlockModifier :MonoBehaviour
    {
        [SerializeField]
        private List<ModifierData> modifierList;



        public virtual void Init()
        {
            InitModifier();
        }


        void InitModifier()
        {

        }

        void FixedUpdate()
        {
            for(int i = modifierList.Count - 1; i >= 0; i--)
            {
                modifierList[i].OnTick(Time.deltaTime);
                if (modifierList[i].IsFinish)
                {
                    modifierList[i].CloseModifier();
                    modifierList.Remove(modifierList[i]);
                }
            }
        }


        public void DoManufactModifier(ManufactoryInfo manuInfo, FunctionBlockInfoData baseInfo, string modifierName)
        {
            ModifierManager.Instance.DoManufactBlockModifier(manuInfo, baseInfo, modifierName);
        }

        public void OnAddModifier(ModifierData data)
        {
            if (!modifierList.Contains(data))
            {
                modifierList.Add(data);
                data.StartModifier();
            }
            else
            {
                Debug.LogWarning("Modifier Exists!  name=" + data.modifierName);
            }
        }

        public void OnRemoveModifier(ModifierData data)
        {
            if (modifierList.Contains(data))
            {
                data.CloseModifier();
            }
        }




        public void OnRemoveModifier(int modifierDataID)
        {
            ModifierData data = GetModifierData(modifierDataID);
            if (data != null)
                data.CloseModifier();
        }

        public ModifierData GetModifierData(int modifierDataID)
        {
            for(int i = 0; i < modifierList.Count; i++)
            {
                if (modifierList[i].modifierDataID == modifierDataID)
                {
                    return modifierList[i];
                }
            }
            return null;
        }

        public ModifierData GetModifierByID(string name)
        {
            for(int i = 0; i < modifierList.Count; i++)
            {
                if (modifierList[i].modifierName == name)
                {
                    return modifierList[i];
                }
            }
            return null;

        }

        
    }
}