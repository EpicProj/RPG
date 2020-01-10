using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sim_FrameWork
{
    public class AssembleShipDesignPage : MonoBehaviour
    {
        [Header("Button")]
        public Button backBtn;
        public Button presetChooseBtn;

        [Header("Content")]
        public Transform leftPanel;
        public Transform RightPanel;
        public Transform CustomPanel;
        public Transform ChooseContent;
    }
}