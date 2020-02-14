using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sim_FrameWork {
    public class LeaderStoryRootItem : MonoBehaviour
    {
        private LeaderStoryInfo _info;

        private int currentSelcetID =-1;

        private Color unSelect = new Color(1, 0.456f, 0);
        private Color Select = new Color(0, 0.7647f, 1f);

        private Image dot;

        public void Awake()
        {
            dot = transform.FindTransfrom("Dot").SafeGetComponent<Image>();
            var btn = transform.SafeGetComponent<Button>();
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(OnDotClick);
        }

        public void SetUpItem(LeaderStoryInfo info)
        {
            if (info != null)
            {
                _info = info;
                transform.FindTransfrom("Year").SafeGetComponent<Text>().text = info.year.ToString();

            }
            UpdateSelectState(false);
        }

        public void UpdateSelectState(bool select, int currentSelectID =-1)
        {
            if (select)
            {
                if (_info.storyID == currentSelectID)
                {
                    //SetSelect
                    dot.color = Select;
                }
                else
                {
                    dot.color = unSelect;
                }
                this.currentSelcetID = currentSelectID;
            }
        }

        void OnDotClick()
        {
            if (_info != null)
            {
                UIManager.Instance.SendMessageToWnd(UIPath.WindowPath.Leader_Detail_Dialog, new UIMessage(UIMsgType.LeaderDetail_Story_Select, new List<object>() { _info.storyID }));
            }
        }
    }
}