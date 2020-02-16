using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sim_FrameWork
{
    public class LeaderPortraitCustomItem :MonoBehaviour
    {
        public LeaderPortraitType portraitType;
        private Text SelectContentName;
        private Button preBtn;
        private Button nextBtn;

        private List<Config.LeaderPortraitItemConfig> itemList;

        private int currentSelectIndex=0;

        private void Awake()
        {
            SelectContentName = transform.FindTransfrom("Select/Name").SafeGetComponent<Text>();
            itemList = new List<Config.LeaderPortraitItemConfig>();
            preBtn = transform.FindTransfrom("Select/Left").SafeGetComponent<Button>();
            preBtn.RemoveListenerAndAddBtnClick(OnPreBtnClick);
            nextBtn = transform.FindTransfrom("Select/Right").SafeGetComponent<Button>();
            nextBtn.RemoveListenerAndAddBtnClick(OnNextBtnClick);
        }

        public void SetUpItem(LeaderPortraitType type,int speciesID,int sexID)
        {
            itemList = LeaderModule.GetLeagalPortraitImte(type, speciesID, sexID);
            if (itemList == null || itemList.Count == 0)
                return;
            ///Select Default
            portraitType = type;
            transform.FindTransfrom("Name").SafeGetComponent<Text>().text = LeaderModule.GetPortraitName(type);
            SelectContentName.text= LeaderModule.GetPortraitName(portraitType) + "_" + currentSelectIndex + 1;
            UpdateBtnState();
        }

        void OnPreBtnClick()
        {
            if (currentSelectIndex > 0)
            {
                currentSelectIndex--;
                SelectContentName.text = LeaderModule.GetPortraitName(portraitType) + "_" + currentSelectIndex + 1;
                UpdateBtnState();
                UIManager.Instance.SendMessageToWnd(UIPath.WindowPath.Leader_Custom_Page, new UIMessage(UIMsgType.LeaderCustom_Refresh_Portrait, new List<object>() { portraitType, currentSelectIndex }));
            }
        }

        void OnNextBtnClick()
        {
            if (currentSelectIndex < itemList.Count)
            {
                currentSelectIndex++;
                SelectContentName.text = LeaderModule.GetPortraitName(portraitType) + "_" + currentSelectIndex + 1;
                UpdateBtnState();
            }
        }

        void UpdateBtnState()
        {
            if (currentSelectIndex == 0)
            {
                preBtn.interactable = false;
                return;
            }
               
            if (currentSelectIndex == itemList.Count - 1)
            {
                nextBtn.interactable = false;
                return;
            }

            if (preBtn.interactable == false)
                preBtn.interactable = true;
            if (nextBtn.interactable == false)
                nextBtn.interactable = true;
        }

    }
}