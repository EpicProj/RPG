using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork {
    public class BaseModule : MonoSingleton<BaseModule> {

        public Dictionary<string, System.Type> m_RegisterModuleDic = new Dictionary<string, System.Type>();

        public void RegisterModule()
        {

        }

        public virtual void InitData() { }

    }
}