using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Sim_FrameWork
{
    public class ExploreAreaSelectBtn : MonoBehaviour
    {
        private ExploreAreaData _data;

        private Image _icon;
        private Text _name;
        private Button _btn;

        private Image _progressImage;
        private Text _progressText;

        void Awake()
        {
            _btn = UIUtility.SafeGetComponent<Button>(transform);
            _icon = UIUtility.SafeGetComponent<Image>(UIUtility.FindTransfrom(transform, "Item/Icon"));
            _name = UIUtility.SafeGetComponent<Text>(UIUtility.FindTransfrom(transform, "Name"));

            _progressImage = UIUtility.SafeGetComponent<Image>(UIUtility.FindTransfrom(transform, "Item/Progress"));
            _progressText = UIUtility.SafeGetComponent<Text>(UIUtility.FindTransfrom(transform, "Item/Progress/Text"));
        }

        public void InitAreaItem(ExploreAreaData data)
        {
            if (data != null)
            {
                _data = data;
                _name.text = data.areaName;
                _icon.sprite = data.areaIcon;
                _progressImage.fillAmount = data.areaTotalProgress;
                _progressText.text = ((int)(data.areaTotalProgress * 100)).ToString()+"%";

                _btn.onClick.AddListener(OnBtnClick);
            }
        }

        void OnBtnClick()
        {
            AudioManager.Instance.PlaySound(AudioClipPath.UISound.Button_Click);
            UIManager.Instance.SendMessage(new UIMessage(UIMsgType.ExplorePage_ShowArea_Mission, new List<object>(1) { _data }));
        }




    }
}