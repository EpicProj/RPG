﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sim_FrameWork.UI
{
    public partial class NewGamePreparePageContext : WindowBase
    {
        private List<CampInfo> totalInfoList = new List<CampInfo>();

        private List<LeaderPrepareCard> leaderCardList = new List<LeaderPrepareCard>();
        #region Override Method

        public override void Awake(params object[] paralist)
        {
            base.Awake(paralist);
            totalInfoList = CampModule.GetAllCampInfo();
            AddBtnClick();
            SetUpGamePreparePanel();
        }

        public override bool OnMessage(UIMessage msg)
        {
            return base.OnMessage(msg);
        }

        public override void OnShow(params object[] paralist)
        {
            base.OnShow(paralist);
            SetUpCampPanel();
            SetUpCrewPanel();
        }


        #endregion

        void AddBtnClick()
        {
            AddButtonClickListener(Transform.FindTransfrom("Back").SafeGetComponent<Button>(), () =>
            {
                UIManager.Instance.HideWnd(this);
                UIGuide.Instance.ShowGameEntryPage();
            });
        }

        void OnGameStartBtnClick()
        {
            ScenesManager.Instance.LoadSceneStartCallBack = () =>
            {
                DataManager.Instance.InitGameData();
            };
            ScenesManager.Instance.LoadingScene(UIPath.ScenePath.Scene_Test);
        }


        void SetUpCampPanel()
        {
            if (totalInfoList == null && totalInfoList.Count < 1)
                return;
            var info = totalInfoList[0];

            //Base Info
            campContentTrans.FindTransfrom("Icon").SafeGetComponent<Image>().sprite = Utility.LoadSprite(info.campBGSmallPath );
            campContentTrans.FindTransfrom("Detail/Title/Icon").SafeGetComponent<Image>().sprite = Utility.LoadSprite(info.campIconPath);
            campContentTrans.FindTransfrom("Detail/Title/Name").SafeGetComponent<Text>().text = info.campName;
            campContentTrans.FindTransfrom("Detail/Desc").SafeGetComponent<Text>().text = info.campDesc;
            //Creed
            var creedTrans = campContentTrans.FindTransfrom("Detail/Property/Creed/Content");
            creedTrans.InitObj(UIPath.PrefabPath.General_InfoItem, 1);
            creedTrans.GetChild(0).SafeGetComponent<GeneralInfoItem>().SetUpItem(GeneralInfoItemType.Camp_Creed, info.creedInfo);

            //Attribute
            if (info.attributeInfo == null)
                return;
            var attributeTrans = campContentTrans.FindTransfrom("Detail/Property/Attribute/Content");
            attributeTrans.InitObj(UIPath.PrefabPath.General_InfoItem, info.attributeInfo.Count);
            for(int i = 0; i < info.attributeInfo.Count; i++)
            {
                attributeTrans.GetChild(i).SafeGetComponent<GeneralInfoItem>().SetUpItem(GeneralInfoItemType.Camp_Attribute, info.attributeInfo[i]);
            }
        }

        void SetUpCrewPanel()
        {
            leaderCardList.Clear();
            var crewList = totalInfoList[0].campLeaderList;
            var trans = Transform.FindTransfrom("Content/CrewPanel/CrewContent");
            trans.InitObj(UIPath.PrefabPath.Leader_Prepare_Card, crewList.Count);
            for(int i=0;i<crewList.Count; i++)
            {
                var item = trans.GetChild(i).SafeGetComponent<LeaderPrepareCard>();
                item.SetUpItem(crewList[i]);
                leaderCardList.Add(item);
            }
        }

        void SetUpGamePreparePanel()
        {
            InitDropDownPrepareItem();
            InitSliderPrepareItem();
        }

        void InitDropDownPrepareItem()
        {

        }

        void InitSliderPrepareItem()
        {
            var list = DataManager.Instance.gamePrepareData.preparePropertyDataList;
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].configType == 2)
                {
                    var trans = Transform.FindTransfrom("Content/HardSetPanel/Content");
                    var item = ObjectManager.Instance.InstantiateObject(UIPath.PrefabPath.General_SliderSelectItem);
                    if (item != null)
                    {
                        item.transform.SetParent(trans,false);
                        var itemTrans= item.transform.SafeGetComponent<SliderSelectItem>();
                        ///Init List
                        var config = PlayerModule.GetGamePrepareConfigItem(list[i].configID);
                        if (config != null)
                        {
                            List<GeneralSliderSelectElement> elementList = new List<GeneralSliderSelectElement>();
                            for(int j = 0; j < config.levelMap.Count; j++)
                            {
                                GeneralSliderSelectElement element = new GeneralSliderSelectElement
                                {
                                    showScaleSymbol = config.showScaleSymbol,
                                    index = config.levelMap[j].Level,
                                    linkParam = config.levelMap[j].hardLevelChange,
                                    value = (float)config.levelMap[j].numParam
                                };
                                elementList.Add(element);
                            }
                            itemTrans.SetUpItem(config.configIconPath, config.configNameText,config.defaultSelectLevel,elementList);   
                        }
                    }
                }
            }
        }
    }




    public partial class NewGamePreparePageContext : WindowBase
    {
        private Transform campContentTrans;

        protected override void InitUIRefrence()
        {
            campContentTrans = Transform.FindTransfrom("Content/CampPanel/Content");
        }
    }
}