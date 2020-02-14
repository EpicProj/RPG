using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sim_FrameWork
{
    public class LeaderPrepareCard : MonoBehaviour
    {
        public enum State
        {
            Empty,
            Added
        }

        public LeaderInfo _info;
        public State currentState = State.Empty;

        public void SetUpItem(State cardState, LeaderInfo info =null)
        {
            var contentCanvas = transform.FindTransfrom("Content").SafeGetComponent<CanvasGroup>();
            var emptyCanvas = transform.FindTransfrom("Choose/").SafeGetComponent<CanvasGroup>();
            if(cardState== State.Empty)
            {
                contentCanvas.ActiveCanvasGroup(false);
                emptyCanvas.ActiveCanvasGroup(true);
                var btn = transform.FindTransfrom("Choose").SafeGetComponent<Button>();
                btn.onClick.RemoveAllListeners();
                btn.onClick.AddListener(OnEmptyCardClick);
            }
            else if(cardState== State.Added)
            {
                contentCanvas.ActiveCanvasGroup(true);
                emptyCanvas.ActiveCanvasGroup(false);

                _info = info;
                transform.FindTransfrom("Content/LeaderPortrait").SafeGetComponent<LeaderPortraitUI>().SetUpItem(info.portraitInfo);
                transform.FindTransfrom("Content/NameBG/Text").SafeGetComponent<Text>().text = info.leaderName;

                SetUpCreed(info.creedInfo);
                SetUpSkill(info.skillInfoList);

                //DoAnim
                contentCanvas.alpha = 0;
                contentCanvas.DoCanvasFade(1, 0.8f);

                ///Test
                var btn = transform.SafeGetComponent<Button>();
                btn.onClick.RemoveAllListeners();
                btn.onClick.AddListener(OnCardClick);
            }
            
        }

        void SetUpCreed(LeaderCreedInfo creedInfo)
        {
            transform.FindTransfrom("Content/LeaderCreed/Name").SafeGetComponent<Text>().text = creedInfo.creedName;
            transform.FindTransfrom("Content/LeaderCreed/Name/Icon").SafeGetComponent<Image>().sprite = Utility.LoadSprite(creedInfo.creedIconPath);
        }

        void SetUpSkill(List<LeaderSkillInfo> infoList)
        {
            if (infoList == null || infoList.Count == 0)
                return;

            var trans = transform.FindTransfrom("Content/SkillContent");
            trans.InitObj(UIPath.PrefabPath.General_InfoItem, infoList.Count);
            for(int i = 0; i < infoList.Count; i++)
            {
                var item = trans.GetChild(i).SafeGetComponent<GeneralInfoItem>();
                item.SetUpItem(GeneralInfoItemType.Leader_Skill, infoList[i],false);
            }

        }

        void OnEmptyCardClick()
        {

        }

        void OnCardClick()
        {
            UIGuide.Instance.ShowLeaderDetailDialog(_info);
        }
    }
}