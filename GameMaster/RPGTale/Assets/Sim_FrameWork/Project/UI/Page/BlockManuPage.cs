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
        public Button FormulaChange;
        public Button FormulaChooseBtn;

        [Header("Info")]
        public Transform Title;
        public Transform BlockInfoContent;
        public Image BlockBG;
        public Text BlockDesc;
        public Transform BlockLevelInfo;
        public Transform LevelEffectContent;
        public Transform EXPContent;


        [Header("Product")]
        public Transform Product_LeftTab;
        public Transform ManuContent;
        public Transform ProgressTrans;

        [Header("District")]
        public Transform DistrictPanel;


    }
}