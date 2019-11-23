using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sim_FrameWork
{
    public class ProductLineChangeDialog : MonoBehaviour
    {
        [Header("Button")]
        public Button ConfirmBtn;
        public Button CloseBtn;
        [Header("Content")]
        public Text Title;
        public Transform ScrollViewTrans;
        public Transform ManuContent;

    }
}