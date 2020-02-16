using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sim_FrameWork
{
    public class LeaderPrepareCard : BaseElement
    {
        public enum State
        {
            Empty,
            ForceSelect,
            //For LoopList
            Select_Dialog,
            Select_Prepare,
        }

        public LeaderInfo _info;
        public State currentState = State.Empty;

        //LoopList
        private LeaderDataModel _model;
        private int _currentSelectCampID;
        private const string LeaderPrepareCard_ForceSelcet_Info = "LeaderPrepareCard_ForceSelcet_Info";
        private Transform removeBtnTrans;    

        public override void Awake()
        {
            base.Awake();
            removeBtnTrans = transform.FindTransfrom("RemoveBtn");
        }

        public override void ChangeAction(BaseDataModel model)
        {
            _model = (LeaderDataModel)model;
            SetUpItem(State.Select_Dialog, _model.Info);
        }

        public void SetUpItem(State cardState, LeaderInfo info =null ,int currentSelectCampID=-1)
        {
            var contentCanvas = transform.FindTransfrom("Content").SafeGetComponent<CanvasGroup>();
            var emptyCanvas = transform.FindTransfrom("Choose/").SafeGetComponent<CanvasGroup>();

            removeBtnTrans.SafeSetActive(false);
            if (cardState== State.Empty)
            {
                currentState = State.Empty;
                _currentSelectCampID = currentSelectCampID;

                contentCanvas.ActiveCanvasGroup(false);
                emptyCanvas.ActiveCanvasGroup(true);

                _info = null;

                emptyCanvas.alpha = 0;
                emptyCanvas.DoCanvasFade(1, 0.8f);

                var btn = transform.FindTransfrom("Choose").SafeGetComponent<Button>();
                AddButtonClickListener(btn, OnEmptyCardClick);
            }
            else if(cardState== State.ForceSelect)
            {
                currentState = State.ForceSelect;
                contentCanvas.ActiveCanvasGroup(true);
                emptyCanvas.ActiveCanvasGroup(false);

                _info = info;
                SetUpBaseInfo(info);
                //DoAnim
                contentCanvas.alpha = 0;
                contentCanvas.DoCanvasFade(1, 0.8f);

                var btn = transform.SafeGetComponent<Button>();
                AddButtonClickListener(btn, OnCardClick_ShowInfo);
            }
            else if(cardState == State.Select_Dialog)
            {
                currentState = State.Select_Dialog;
                contentCanvas.ActiveCanvasGroup(true);
                emptyCanvas.ActiveCanvasGroup(false);

                SetUpBaseInfo(_model.Info);
                _info = _model.Info;
                //DoAnim
                contentCanvas.alpha = 0;
                contentCanvas.DoCanvasFade(1, 0.8f);

                ///Test
                var btn = transform.SafeGetComponent<Button>();
                AddButtonClickListener(btn, OnCardClick_SelectPage);
            }
            else if(cardState == State.Select_Prepare)
            {
                currentState = State.Select_Prepare;
                contentCanvas.ActiveCanvasGroup(true);
                emptyCanvas.ActiveCanvasGroup(false);
                SetUpBaseInfo(info);
                _info = info;

                //DoAnim
                contentCanvas.alpha = 0;
                contentCanvas.DoCanvasFade(1, 0.8f);

                var btn = transform.SafeGetComponent<Button>();
                AddButtonClickListener(btn, OnCardClick_ShowInfo);
            }
        }

        public void ShowRemoveBtn()
        {
            removeBtnTrans.SafeSetActive(true);
            var removeBtn = removeBtnTrans.SafeGetComponent<Button>();
            AddButtonClickListener(removeBtn, OnRemoveBtnClick);
        }

        void SetUpBaseInfo(LeaderInfo info)
        {
            transform.FindTransfrom("Content/LeaderPortrait").SafeGetComponent<LeaderPortraitUI>().SetUpItem(info.portraitInfo);
            transform.FindTransfrom("Content/NameBG/Text").SafeGetComponent<Text>().text = info.leaderName;

            SetUpCreed(info.creedInfo);
            SetUpSkill(info.skillInfoList);

            ShowInfo(info.forceSelcet, MultiLanguage.Instance.GetTextValue(LeaderPrepareCard_ForceSelcet_Info));
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

        void ShowInfo(bool show,string content = "")
        {
            var trans = transform.FindTransfrom("Content/Info");
            trans.SafeSetActive(show);
            if (show)
                trans.FindTransfrom("InfoText").SafeGetComponent<Text>().text = content;
        }

        void OnEmptyCardClick()
        {
            if (_currentSelectCampID != -1)
            {
                UIGuide.Instance.ShowLeaderSelectDialog();
            }
        }

        void OnCardClick_ShowInfo()
        {
            UIGuide.Instance.ShowLeaderDetailDialog(_info);
        }
        void OnCardClick_SelectPage()
        {
            UIManager.Instance.SendMessageToWnd(UIPath.WindowPath.Leader_Select_Dialog, new UIMessage(UIMsgType.LeaderSelectPage_RefreshSelect, new List<object>() {_model.Info }));
        }
        void OnRemoveBtnClick()
        {
            UIManager.Instance.SendMessageToWnd(UIPath.WindowPath.NewGame_Prepare_Page, new UIMessage(UIMsgType.NewGamePage_RemoveLeader, new List<object>() {_info }));
        }
    }
}