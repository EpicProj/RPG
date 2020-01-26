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

        private Image _progressImage;
        private Text _progressText;

        public bool isSelect = false;

        void Awake()
        {
            _progressImage = transform.FindTransfrom("Item/Progress").SafeGetComponent<Image>();
            _progressText = transform.FindTransfrom("Item/Progress/Text").SafeGetComponent<Text>();
        }

        public void InitAreaItem(ExploreAreaData data)
        {
            if (data != null)
            {
                var btn = transform.SafeGetComponent<Button>();
                btn.onClick.RemoveAllListeners();
                _data = data;
                transform.FindTransfrom("Name").SafeGetComponent<Text>().text = data.areaName;
                transform.FindTransfrom("Item/Icon").SafeGetComponent<Image>().sprite = data.areaIcon;
                _progressImage.fillAmount = data.areaTotalProgress;
                _progressText.text = ((int)(data.areaTotalProgress * 100)).ToString()+"%";

                btn.onClick.AddListener(OnBtnClick);
            }
        }

        void OnBtnClick()
        {
            AudioManager.Instance.PlaySound(AudioClipPath.UISound.Button_Click);
            UIManager.Instance.SendMessage(new UIMessage(UIMsgType.ExplorePage_ShowArea_Mission, new List<object>(1) { _data }));
            foreach (Transform trans in transform.parent)
            {
                var cmpt = trans.SafeGetComponent<ExploreAreaSelectBtn>();
                if (cmpt != null)
                {
                    cmpt.SetSelect(false);
                }
            }
            SetSelect(true);
            SolarSystemManager.Instance.MoveToAreaPoint(_data.areaID);
        }

        public void SetSelect(bool select)
        {
            transform.FindTransfrom("Item/RotateEffect").SafeSetActive(select);
            isSelect = select;
        }



    }
}