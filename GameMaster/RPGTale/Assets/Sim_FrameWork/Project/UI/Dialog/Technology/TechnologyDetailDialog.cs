using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sim_FrameWork
{
    public class TechnologyDetailDialog : MonoBehaviour
    {
        [Header("Button")]
        public Button backBtn;
        public Button confirmBtn;

        [Header("Content")]
        public Transform ContextTrans;
        public Transform EffectContentTrans;
    }
}