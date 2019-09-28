using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sim_FrameWork
{
    public class OrderReceiveMainPage : MonoBehaviour
    {
        [Header("Button")]
        public Button BackBtn;


        [Header("Content")]
        ///订单中心
        public GameObject OrderContentScroll;
        public GameObject Organization_No_Info;
        public GameObject Organization_Detail;
        ///组织
        public GameObject Organization_ContentScroll;
    }
}