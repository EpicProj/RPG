using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sim_FrameWork.UI
{
    public partial class NewGamePreparePageContext : WindowBase
    {
        private List<LeaderPrepareCard> leaderCardList = new List<LeaderPrepareCard>();

        private int currentCampID =-1;

        #region Override Method

        public override void Awake(params object[] paralist)
        {
            base.Awake(paralist);
            AddBtnClick();
            SetUpGamePreparePanel();
        }

        public override bool OnMessage(UIMessage msg)
        {
            if(msg.type== UIMsgType.NewGamePage_UpdateCamp)
            {
                int campID = (int)msg.content[0];
                CampInfo campInfo = GameManager.Instance.GetCampInfoData(campID);
                if (campInfo == null)
                    return false;
                return UpdateCampPanel(campInfo) && RefreshLeaderPanel(campInfo);
            }

            return true;
        }

        public override void OnShow(params object[] paralist)
        {
            base.OnShow(paralist);
            SetUpCampPanel();
        }


        #endregion

        void AddBtnClick()
        {
            AddButtonClickListener(Transform.FindTransfrom("Back").SafeGetComponent<Button>(), () =>
            {
                UIManager.Instance.HideWnd(this);
                UIGuide.Instance.ShowGameEntryPage();
            });
            AddButtonClickListener(campContentTrans.FindTransfrom("Icon").SafeGetComponent<Button>(), OnCampSelectBtnClick);
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
            var info = GameManager.Instance.GetCampInfoData(Config.ConfigData.GlobalSetting.CampChoosePage_DefaultSelect_CampID);
            if (info != null)
            {
                currentCampID = info.CampID;
                UpdateCampPanel(info);
                RefreshLeaderPanel(info);
            }
        }

        bool UpdateCampPanel(CampInfo info)
        {
            if (info == null)
                return false;
            //Base Info
            campContentTrans.FindTransfrom("Icon").SafeGetComponent<Image>().sprite = Utility.LoadSprite(info.campBGSmallPath);
            campContentTrans.FindTransfrom("Detail/Title/Icon").SafeGetComponent<Image>().sprite = Utility.LoadSprite(info.campIconPath);
            campContentTrans.FindTransfrom("Detail/Title/Name").SafeGetComponent<Text>().text = info.campName;
            campContentTrans.FindTransfrom("Detail/Desc").SafeGetComponent<Text>().text = info.campDesc;
            //Creed
            var creedTrans = campContentTrans.FindTransfrom("Detail/Property/Creed/Content");
            creedTrans.InitObj(UIPath.PrefabPath.General_InfoItem, 1);
            creedTrans.GetChild(0).SafeGetComponent<GeneralInfoItem>().SetUpItem(GeneralInfoItemType.Camp_Creed, info.creedInfo);

            //Attribute
            if (info.attributeInfo == null)
                return false;
            var attributeTrans = campContentTrans.FindTransfrom("Detail/Property/Attribute/Content");
            attributeTrans.InitObj(UIPath.PrefabPath.General_InfoItem, info.attributeInfo.Count);
            for (int i = 0; i < info.attributeInfo.Count; i++)
            {
                attributeTrans.GetChild(i).SafeGetComponent<GeneralInfoItem>().SetUpItem(GeneralInfoItemType.Camp_Attribute, info.attributeInfo[i]);
            }

            return true;
        }

        void OnCampSelectBtnClick()
        {
            UIManager.Instance.HideWnd(this);
            UIGuide.Instance.ShowCampSelectMainPage();
        }

        bool RefreshLeaderPanel(CampInfo info)
        {
            leaderCardList.Clear();
            if (info != null)
            {
                var crewList = info.campLeaderList;
                var trans = Transform.FindTransfrom("Content/CrewPanel/CrewContent");
                trans.InitObj(UIPath.PrefabPath.Leader_Prepare_Card, crewList.Count);
                for (int i = 0; i < crewList.Count; i++)
                {
                    var item = trans.GetChild(i).SafeGetComponent<LeaderPrepareCard>();
                    item.SetUpItem(crewList[i]);
                    leaderCardList.Add(item);
                }
                return true;
            }
            return false;
        }

        void SetUpGamePreparePanel()
        {
            InitPrepareItem();
        }


        void InitPrepareItem()
        {
            var list = DataManager.Instance.gamePrepareData.preparePropertyDataList;

            var trans = Transform.FindTransfrom("Content/HardSetPanel/Content");

            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].configType == 1)
                {
                    var item = ObjectManager.Instance.InstantiateObject(UIPath.PrefabPath.General_DropDownChooseItem);
                    if (item != null)
                    {
                        item.transform.SetParent(trans, false);
                        var itemTrans = item.transform.SafeGetComponent<DropDownChooseItem>();
                        var config = PlayerModule.GetGamePrepareConfigItem(list[i].configID);
                        if (config != null)
                        {
                            List<GeneralDropDownChooseElement> elementList = new List<GeneralDropDownChooseElement>();
                            for(int j = 0; j < config.levelMap.Count; j++)
                            {
                                GeneralDropDownChooseElement element = new GeneralDropDownChooseElement
                                {
                                    index = config.levelMap[j].Level,
                                    desc = config.levelMap[j].strParam,
                                    linkParam = config.levelMap[j].hardLevelChange
                                };
                                elementList.Add(element);
                            }
                            itemTrans.SetUpItem(config.configID,
                                config.configIconPath, config.configNameText,config.defaultSelectLevel,
                                elementList);
                        }
                    }
                }
                else if (list[i].configType == 2)
                {
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
                            itemTrans.SetUpItem(config.configID,
                                config.configIconPath, config.configNameText,
                                config.defaultSelectLevel,elementList);   
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