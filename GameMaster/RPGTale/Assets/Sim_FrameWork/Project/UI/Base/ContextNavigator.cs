using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork {
    public class ContextNavigator : MonoBehaviour
    {
        public List<ContextItem> nodeList;

        public Transform GetNode(string nodeName)
        {
            var item= nodeList.Find(x => x.nodeName == nodeName);
            if (item != null)
                return item.nodeTrans;
            return null;
        }

    }

    [System.Serializable]
    public class ContextItem
    {
        public string nodeName;
        public Transform nodeTrans;
    }
}