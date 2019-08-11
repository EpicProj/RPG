using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork {
    public class ModuleManager : Singleton<ModuleManager> {

        public Dictionary<string, System.Type> m_RegisterModuleDic = new Dictionary<string, System.Type>();
        public Dictionary<string, BaseModule> m_ModuleDic = new Dictionary<string, BaseModule>();
        private List<BaseModule> m_ModuleList = new List<BaseModule>();



    }
}