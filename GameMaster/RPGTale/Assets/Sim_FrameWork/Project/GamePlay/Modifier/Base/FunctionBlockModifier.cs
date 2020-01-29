using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork
{
    public class FunctionBlockModifier :ModifierGeneral
    {
        public virtual void Init()
        {
            InitModifier();
        }

        void InitModifier()
        {

        }

        public void DoManufactModifier(ManufactoryInfo manuInfo, FunctionBlockInfoData baseInfo, string modifierName)
        {
            ModifierManager.Instance.DoManufactBlockModifier(manuInfo, baseInfo, modifierName);
        }

    }
}