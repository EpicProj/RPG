using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sim_FrameWork
{
    public class BlockManuPage : MonoBehaviour
    {
        [Header("Button")]
        public Button BackBtn;

        [Header("Info")]
        public Transform Title;
        public Transform BlockInfoContent;
        public Image BlockBG;
        public Text BlockDesc;

    }
}