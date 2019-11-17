using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sim_FrameWork
{
    public class SliderGroup : MonoBehaviour
    {

        private float _currentProgress;
        public float Progress
        {
            get { return _currentProgress; }
        }

        private List<Slider> sliderList = new List<Slider>();

        private void Awake()
        {
            foreach(Transform trans in transform)
            {
                var slider = UIUtility.SafeGetComponent<Slider>(trans);
                slider.value = 0;
                sliderList.Add(slider);
            }
        }

        public void RefreshSlider(float progress)
        {
            if (progress >= 1)
            {
                _currentProgress = 1;
            }
            else
            {
                _currentProgress = progress;
            }

            float progressPerSlider = 1 / (float)sliderList.Count;

            for(int i = 1; i < sliderList.Count+1; i++)
            {
                if (progress > progressPerSlider * i)
                {
                    sliderList[i-1].value = 1;
                }
                else
                {
                    var value = progress - progressPerSlider * (i - 1);
                    sliderList[i-1].value = value/progressPerSlider;
                    break;
                }
            }
        }

        public void ResetSlider()
        {
            for(int i = 0; i < sliderList.Count; i++)
            {
                sliderList[i].value = 0;
            }
        }


    }
}