using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sim_FrameWork
{
    public class WareHousePage:MonoBehaviour
    {
        public GameObject MaterialContent;
        public GameObject MaterialScrollView;

        public GameObject TypeFilterContent;

        [Header("Detail")]
        public GameObject DetailContent;
        public Image DetailBG;
        public Text DetailName;
        public Text DetailDesc;
        public GameObject DetailRarity;

        [Header("Button")]
        public Button CloseBtn;
    }
}