using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sim_FrameWork.UI
{
    public class LoadingPage : MonoBehaviour
    {
        private Image _bg;
        private Slider _progressSlider;
        private Text _progressText;

        private Text _title;
        private Text _desc;

        private List<LoadingPageItem> _itemList;
        

        void Awake()
        {
            _itemList = new List<LoadingPageItem>();
            _bg = UIUtility.SafeGetComponent<Image>(UIUtility.FindTransfrom(transform, "BG"));
            _progressSlider = UIUtility.SafeGetComponent<Slider>(UIUtility.FindTransfrom(transform, "Progress/Slider"));
            _progressText = UIUtility.SafeGetComponent<Text>(UIUtility.FindTransfrom(transform, "Progress/TextProress"));
            _title = UIUtility.SafeGetComponent<Text>(UIUtility.FindTransfrom(transform, "Content/Title"));
            _desc = UIUtility.SafeGetComponent<Text>(UIUtility.FindTransfrom(transform, "Content/Desc"));
        }

        void Update()
        {
            _progressSlider.value = ScenesManager.LoadingProgress;
            _progressText.text = ScenesManager.LoadingProgress.ToString() + "%";
        }

        void SetUpContent()
        {
        }
    }
}