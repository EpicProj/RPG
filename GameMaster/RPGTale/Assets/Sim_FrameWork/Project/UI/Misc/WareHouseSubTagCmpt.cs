using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sim_FrameWork
{
    public class WareHouseSubTagCmpt : MonoBehaviour
    {
        public Toggle toggle;
        public Image icon;
        public Text Name;

        private MaterialSubType subType;

        public void InitSubTag(MaterialSubType type)
        {
            icon.sprite = MaterialModule.GetMaterialSubTypeIcon(type);
            Name.text = MaterialModule.GetMaterialSubTypeName(type);
            subType = type;
        }


    }
}