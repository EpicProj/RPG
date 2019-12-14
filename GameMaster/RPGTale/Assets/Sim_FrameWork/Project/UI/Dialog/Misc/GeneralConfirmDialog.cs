using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sim_FrameWork
{
    public class GeneralConfirmDialog:MonoBehaviour
    {
        [Header("Button")]
        public Button CloseBtn;
        public Button ConfirmBtn;
        public Button CancelBtn;

        [Header("Content")]
        public Text TitleText;
        public Text ConfirmText;
        public Text CancelText;
        public Text ContentText;

    }
}