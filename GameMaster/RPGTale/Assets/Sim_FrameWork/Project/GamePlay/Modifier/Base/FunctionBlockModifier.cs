using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork
{
    public class FunctionBlockModifier :ModifierGeneral
    {
        public ModifierTarget target;
        public uint instanceID;
        public FunctionBlockModifier(ModifierTarget target,uint instanceID)
        {
            this.target = target;
            this.instanceID = instanceID;
        }
        public virtual void Init()
        {
            InitModifier();
        }

        void InitModifier()
        {

        }

        public void DoManufactModifier(ManufactoryInfo manuInfo, FunctionBlockInfoData baseInfo, string modifierName)
        {
            ModifierManager.Instance.AddManufactBlockModifier(manuInfo, baseInfo, modifierName);
        }

    }
}