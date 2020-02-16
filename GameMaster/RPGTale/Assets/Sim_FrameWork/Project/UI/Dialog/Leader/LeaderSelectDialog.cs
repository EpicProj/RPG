using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sim_FrameWork.UI
{
    public partial class LeaderSelectDialog : WindowBase
    {
        private int currentSelectCampID = -1;
        private LeaderInfo currentSelectLeaderInfo = null;

        #region OverrideMethod
        public override void Awake(params object[] paralist)
        {
            base.Awake(paralist);
            currentSelectCampID = (int)paralist[0];
            AddBtnClick();
        }

        public override void OnShow(params object[] paralist)
        {
            base.OnShow(paralist);
            currentSelectCampID = (int)paralist[0];
            SetUpDialog();
        }

        public override bool OnMessage(UIMessage msg)
        {
            if(msg.type== UIMsgType.LeaderSelectPage_RefreshSelect)
            {
                LeaderInfo info = (LeaderInfo)msg.content[0];
                return RefreshLeaderInfo(info);
            }
            return false;
        }
        #endregion

        void AddBtnClick()
        {
            AddButtonClickListener(Transform.FindTransfrom("Content/BtnPanel/CustomBtn").SafeGetComponent<Button>(), OnCustomBtnClick);
            AddButtonClickListener(Transform.FindTransfrom("Content/BtnPanel/SelectBtn").SafeGetComponent<Button>(), OnSelectBtnClick);
            AddButtonClickListener(Transform.FindTransfrom("BG").SafeGetComponent<Button>(), () =>
            {
                UIManager.Instance.HideWnd(this);
            });
        }

        void OnCustomBtnClick()
        {

        }

        void OnSelectBtnClick()
        {
            if (currentSelectLeaderInfo != null)
            {
                UIManager.Instance.SendMessageToWnd(UIPath.WindowPath.NewGame_Prepare_Page, new UIMessage(UIMsgType.LeaderPrepare_SelectLeader, new List<object>() { currentSelectLeaderInfo }));
            }
            UIManager.Instance.HideWnd(this);
        }

        void SetUpDialog()
        {
            var list = CampModule.GetCampLeaderSelectPresetList(currentSelectCampID);
            if (list.Count == 0)
                return;
            ///DefaultSelect
            var defaultInfo = list[0];
            RefreshLeaderInfo(defaultInfo);
            RefreshSelectContent();
        }

        bool RefreshLeaderInfo(LeaderInfo info)
        {
            if (info == null)
                return false;
            currentSelectLeaderInfo = info;
            _nameText.text = info.leaderName;
            _speciesText.text = info.speciesInfo.speciesName;
            _ageText.text = info.currentAge.ToString();
            _sexText.text = LeaderModule.GetLeaderGenderText(info.Gender);
            _birthlandText.text = info.birthlandInfo.landName;
            _creedText.text = info.creedInfo.creedName;

            SetUpAttribute(info.attributeInfoList);
            SetUpSkill(info.skillInfoList);
            return true;
        }

        void SetUpAttribute(List<LeaderAttributeInfo> info)
        {
            if (info == null || info.Count == 0)
                return;
            var content = Transform.FindTransfrom("Content/Context/Property/Attribute/Content");
            content.InitObj(UIPath.PrefabPath.General_InfoItem, info.Count);
            for (int i = 0; i < info.Count; i++)
            {
                var item = content.GetChild(i).SafeGetComponent<GeneralInfoItem>();
                item.SetUpItem(GeneralInfoItemType.Leader_Attribute, info[i]);
            }
        }

        void SetUpSkill(List<LeaderSkillInfo> info)
        {
            if (info == null || info.Count == 0)
                return;
            var content = Transform.FindTransfrom("Content/Context/Property/Skill/Content");
            content.InitObj(UIPath.PrefabPath.Leader_Skill_Item, info.Count);
            for (int i = 0; i < info.Count; i++)
            {
                var item = content.GetChild(i).SafeGetComponent<LeaderSkillItem>();
                item.SetUpItem(info[i]);
            }
        }

        void RefreshSelectContent()
        {
            var modelList = DataManager.Instance.GetCampLeaderSelectModelList(currentSelectCampID);
            ///RemoveAlreadySelect
            for(int i = 0; i < DataManager.Instance.gamePrepareData.currentLeaderInfoList.Count;i++)
            {
                int existID = DataManager.Instance.gamePrepareData.currentLeaderInfoList[i].leaderID;
            }
            var loopList = Transform.FindTransfrom("Content/Context/Select/Scroll View").SafeGetComponent<LoopList>();
            loopList.InitData(modelList);
        }


    }

    public partial class LeaderSelectDialog : WindowBase
    {
        private Text _nameText;
        private Text _speciesText;
        private Text _ageText;
        private Text _sexText;
        private Text _birthlandText;
        private Text _creedText;


        protected override void InitUIRefrence()
        {
            _nameText = Transform.FindTransfrom("Content/Context/Detail/Name").SafeGetComponent<Text>();
            _speciesText = Transform.FindTransfrom("Content/Context/Detail/Panel/Detail_Species/Text").SafeGetComponent<Text>();
            _ageText = Transform.FindTransfrom("Content/Context/Detail/Panel/Detail_Age/Text").SafeGetComponent<Text>();
            _sexText = Transform.FindTransfrom("Content/Context/Detail/Panel/Detail_Sex/Text").SafeGetComponent<Text>();
            _birthlandText = Transform.FindTransfrom("Content/Context/Detail/Panel/Detail_Birthland/Text").SafeGetComponent<Text>();
            _creedText = Transform.FindTransfrom("Content/Context/Detail/Panel/Detail_Creed/Text").SafeGetComponent<Text>();
        }



    }
}