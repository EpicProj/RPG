using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sim_FrameWork.UI
{
    public partial class LeaderDetailDialog : WindowBase
    {
        private LeaderInfo _info;

        #region Override Method
        public override void Awake(params object[] paralist)
        {
            base.Awake(paralist);
            _info = (LeaderInfo)paralist[0];
            AddBtnClick();
        }

        public override void OnShow(params object[] paralist)
        {
            base.OnShow(paralist);
            _info = (LeaderInfo)paralist[0];
            SetUpDialog();
        }

        public override bool OnMessage(UIMessage msg)
        {
            if(msg.type == UIMsgType.LeaderDetail_Story_Select)
            {
                int selectID = (int)msg.content[0];
                return OnStoryRootSelect(selectID);
            }

            return true;
        }


        #endregion
        void AddBtnClick()
        {
            AddButtonClickListener(Transform.FindTransfrom("BG").SafeGetComponent<Button>(), () =>
            {
                UIManager.Instance.HideWnd(this);
            });
        }

        private void SetUpDialog()
        {
            if (_info == null)
                return;
            //SetUpPortrait
            Transform.FindTransfrom("Content/DetailPanel/LeaderPortrait").SafeGetComponent<LeaderPortraitUI>().SetUpItem(_info.portraitInfo);
            _nameText.text = _info.leaderName;
            _speciesText.text = _info.speciesInfo.speciesName;
            _ageText.text = _info.currentAge.ToString();
            _sexText.text = LeaderModule.GetLeaderGenderText(_info.Gender);
            _birthlandText.text = _info.birthlandInfo.landName;
            _creedText.text = _info.creedInfo.creedName;

            SetUpAttribute(_info.attributeInfoList);
            SetUpSkill(_info.skillInfoList);
            SetUpStory(_info.storyInfoList);
        }

        void SetUpAttribute(List<LeaderAttributeInfo> info)
        {
            if (info == null || info.Count == 0)
                return;
            var content = Transform.FindTransfrom("Content/Property/Attribute/Content");
            content.InitObj(UIPath.PrefabPath.General_InfoItem, info.Count);
            for(int i = 0; i < info.Count; i++)
            {
                var item = content.GetChild(i).SafeGetComponent<GeneralInfoItem>();
                item.SetUpItem(GeneralInfoItemType.Leader_Attribute, info[i]);
            }
        }

        void SetUpSkill(List<LeaderSkillInfo> info)
        {
            if (info == null || info.Count==0)
                return;
            var content = Transform.FindTransfrom("Content/Property/Skill/Content");
            content.InitObj(UIPath.PrefabPath.Leader_Skill_Item, info.Count);
            for(int i = 0; i < info.Count; i++)
            {
                var item = content.GetChild(i).SafeGetComponent<LeaderSkillItem>();
                item.SetUpItem(info[i]);
            }
        }

        void SetUpStory(List<LeaderStoryInfo> info)
        {
            if (info == null || info.Count == 0)
                return;
            var content = Transform.FindTransfrom("Content/Property/Story/Line/Content");
            content.InitObj(UIPath.PrefabPath.Leader_StoryRoot_Item, info.Count);
            for(int i = 0; i < info.Count; i++)
            {
                var item = content.GetChild(i).SafeGetComponent<LeaderStoryRootItem>();
                item?.SetUpItem(info[i]);
                ///Set DefaultSelect
                item.UpdateSelectState(true, info[0].storyID);
            }
            _storyContentTypeEffect?.StartEffect();
        }

        bool OnStoryRootSelect(int storyID)
        {
            var info = _info.storyInfoList.Find(x => x.storyID == storyID);
            if (info == null)
                return false;
            ///Update
            _storyContentText.text = info.storyContent;
            _storyContentTypeEffect?.StartEffect();

            var content = Transform.FindTransfrom("Content/Property/Story/Line/Content");
            foreach(Transform trans in content)
                trans.SafeGetComponent<LeaderStoryRootItem>().UpdateSelectState(true, storyID);

            return true;
        }


    }

    public partial class LeaderDetailDialog : WindowBase
    {
        private Text _nameText;
        private Text _speciesText;
        private Text _ageText;
        private Text _sexText;
        private Text _birthlandText;
        private Text _creedText;

        private Text _storyContentText;
        private TypeWriterEffect _storyContentTypeEffect;

        protected override void InitUIRefrence()
        {
            _nameText = Transform.FindTransfrom("Content/DetailPanel/Detail/Name").SafeGetComponent<Text>();
            _speciesText = Transform.FindTransfrom("Content/DetailPanel/Detail/Panel/Detail_Species/Text").SafeGetComponent<Text>();
            _ageText = Transform.FindTransfrom("Content/DetailPanel/Detail/Panel/Detail_Age/Text").SafeGetComponent<Text>();
            _sexText = Transform.FindTransfrom("Content/DetailPanel/Detail/Panel/Detail_Sex/Text").SafeGetComponent<Text>();
            _birthlandText = Transform.FindTransfrom("Content/DetailPanel/Detail/Panel/Detail_Birthland/Text").SafeGetComponent<Text>();
            _creedText = Transform.FindTransfrom("Content/DetailPanel/Detail/Panel/Detail_Creed/Text").SafeGetComponent<Text>();
            _storyContentText = Transform.FindTransfrom("Content/Property/Story/Desc").SafeGetComponent<Text>();
            _storyContentTypeEffect = _storyContentText.transform.SafeGetComponent<TypeWriterEffect>();
        }
    }

}