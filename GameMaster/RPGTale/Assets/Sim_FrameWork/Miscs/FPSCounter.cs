using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sim_FrameWork
{
    public class FPSCounter : MonoBehaviour
    {
        private Text FPS;

        internal float updateFrequency = 0.5f;
        internal int framePerSec;


        private void Start()
        {
            FPS = UIUtility.SafeGetComponent<Text>(transform);
            StartCoroutine(UpdateFPS());
        }


        private IEnumerator UpdateFPS()
        {
            for(; ; )
            {
                int lastFrame = Time.frameCount;
                float lastTime = Time.realtimeSinceStartup;
                yield return new WaitForSeconds(updateFrequency);
                float timeSpan = Time.realtimeSinceStartup - lastTime;
                int frameCount = Time.frameCount - lastFrame;

                framePerSec = Mathf.RoundToInt(frameCount / timeSpan);
                FPS.text = framePerSec.ToString();
            }
        }

    }
}