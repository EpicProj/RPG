using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sim_FrameWork.UI
{
    public class LoadingPage : MonoBehaviour
    {
        public Image BG;
        public Slider progressSlider;
        public Text progressText;

        void Update()
        {
            progressSlider.value = ScenesManager.LoadingProgress;
            progressText.text = ScenesManager.LoadingProgress.ToString() + "%";
        }

    }
}