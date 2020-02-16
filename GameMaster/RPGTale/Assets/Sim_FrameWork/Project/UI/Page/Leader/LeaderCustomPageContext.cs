using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sim_FrameWork.UI
{
    public partial class LeaderCustomPageContext : WindowBase
    {
        private List<LeaderSpeciesInfo> totalSpeciesInfoList;
        private Dictionary<LeaderPortraitType, List<Config.LeaderPortraitItemConfig>> portraitSpriteDic;

        private List<LeaderPortraitCustomItem> customItemList;
        private int currentSelectSpeciesID = -1;


        #region OverrideMethod
        public override void Awake(params object[] paralist)
        {
            base.Awake(paralist);
            AddBtnClick();
            InitBaseData();
        }

        public override void OnShow(params object[] paralist)
        {
            base.OnShow(paralist);
        }

        public override bool OnMessage(UIMessage msg)
        {
            if(msg.type == UIMsgType.LeaderCustom_Refresh_Portrait)
            {
                LeaderPortraitType type = (LeaderPortraitType)msg.content[0];
                int index = (int)msg.content[1];
                return UpdatePortraitSprite(type, index);
            }
            return true;
        }

        void InitBaseData()
        {
            totalSpeciesInfoList = LeaderModule.GetAllSpecies();
            portraitSpriteDic = new Dictionary<LeaderPortraitType, List<Config.LeaderPortraitItemConfig>>();
            customItemList = new List<LeaderPortraitCustomItem>();

            InitCustomPortraitContent();
            InitSpeciesDropDown();

            ///InitSpeciesDefault
            if(totalSpeciesInfoList!=null && totalSpeciesInfoList.Count != 0)
            {
                OnSpeciesDropDownValueChange(0);
            } 
        }
        #endregion

        void AddBtnClick()
        {
            AddButtonClickListener(Transform.FindTransfrom("Back").SafeGetComponent<Button>(), () =>
            {
                UIManager.Instance.HideWnd(this);
            });
        }

        void InitCustomPortraitContent()
        {
            var trans = Transform.FindTransfrom("Content/CustomPortrait/CustomPanel/Content");
            foreach(LeaderPortraitType type in Enum.GetValues(typeof(LeaderPortraitType)))
            {
                var customItem = ObjectManager.Instance.InstantiateObject(LeaderPortraitCustomItem);
                if (customItem != null)
                {
                    customItem.transform.SetParent(trans, false);
                    var item= customItem.transform.SafeGetComponent<LeaderPortraitCustomItem>();
                    item.SetUpItem(type, currentSelectSpeciesID, 1);
                    customItemList.Add(item);
                }
            }
        }

        void InitSpeciesDropDown()
        {
            var dropDown = Transform.FindTransfrom("Content/CustomSpecies/Species/Dropdown").SafeGetComponent<Dropdown>();
            dropDown.ClearOptions();
            dropDown.onValueChanged.RemoveAllListeners();

            List<Dropdown.OptionData> optionList = new List<Dropdown.OptionData>();
            for(int i = 0; i < totalSpeciesInfoList.Count; i++)
            {
                Dropdown.OptionData data = new Dropdown.OptionData
                {
                    text = totalSpeciesInfoList[i].speciesName
                };
                optionList.Add(data);
            }
            dropDown.AddOptions(optionList);

            dropDown.onValueChanged.AddListener((index) => OnSpeciesDropDownValueChange(index));
        }

        void OnSpeciesDropDownValueChange(int index)
        {
            if (index > totalSpeciesInfoList.Count - 1)
                return;
            var info = totalSpeciesInfoList[index];
            currentSelectSpeciesID = info.speciesID;
            ///Refresh ImageDic
            portraitSpriteDic.Clear();
            foreach (LeaderPortraitType type in Enum.GetValues(typeof(LeaderPortraitType)))
            {
                var list = LeaderModule.GetLeagalPortraitImte(type, currentSelectSpeciesID, 1);
                portraitSpriteDic.Add(type, list);
            }
            RefreshCustomPortraitContent();
        }

        void RefreshCustomPortraitContent()
        {
            int index = 0;
            foreach (LeaderPortraitType type in Enum.GetValues(typeof(LeaderPortraitType)))
            {
                if (index > customItemList.Count - 1)
                    return;
                customItemList[index].SetUpItem(type, currentSelectSpeciesID, 1);
            }
        }

        #region Portrait
        bool UpdatePortraitSprite(LeaderPortraitType type ,int index)
        {
            var list = portraitSpriteDic[type];
            if (index > list.Count - 1)
                return false;
            var sp = Utility.LoadSprite(list[index].spritePath);
            _portraitUI.RefreshSprite(type, sp);
            return true;
        }

        #endregion
    }


    public partial class LeaderCustomPageContext : WindowBase
    {
        private const string LeaderPortraitCustomItem = "Assets/Prefabs/Object/Leader/LeaderPortraitCustomItem.prefab";
        private LeaderPortraitUI _portraitUI;
        protected override void InitUIRefrence()
        {
            _portraitUI = Transform.FindTransfrom("Content/CustomPortrait/LeaderPortrait").SafeGetComponent<LeaderPortraitUI>();
        }
    }
}