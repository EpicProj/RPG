using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sim_FrameWork.UI
{
    public partial class NewGamePreparePageContext : WindowBase
    {
        private List<LeaderPrepareCard> leaderCardList = new List<LeaderPrepareCard>();

        bool hasInit = false;

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
            else if(msg.type== UIMsgType.LeaderPrepare_SelectLeader)
            {
                LeaderInfo info = (LeaderInfo)msg.content[0];
                return AddLeader(info);
            }
            else if(msg.type== UIMsgType.NewGamePage_RemoveLeader)
            {
                LeaderInfo info = (LeaderInfo)msg.content[0];
                return RemoveLeader(info);
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

            AddButtonClickListener(Transform.FindTransfrom("BtnPanel/StartBtn").SafeGetComponent<Button>(), OnGameStartBtnClick);

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
            if(!hasInit)
            {
                var info = GameManager.Instance.GetCampInfoData(Config.ConfigData.GlobalSetting.CampChoosePage_DefaultSelect_CampID);
                if (info != null)
                {
                    CampManager.Instance.PreparePage_CurrentSelect_CampID = info.CampID;
                    UpdateCampPanel(info);
                    RefreshLeaderPanel(info);
                }
            }
            hasInit = true;
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
            CampManager.Instance.PreparePage_CurrentSelect_CampID = info.CampID;
            ///Select Camp
            DataManager.Instance.ChangeSelectCamp(info);

            return true;
        }

        void OnCampSelectBtnClick()
        {
            UIManager.Instance.HideWnd(this);
            UIGuide.Instance.ShowCampSelectMainPage();
        }

        bool RefreshLeaderPanel(CampInfo info)
        {
            //UpdateLeaderSelect
            DataManager.Instance.gamePrepareData.RefreshSelectLeaderInfo(info.CampID);
            leaderCardList.Clear();
            if (info != null)
            {
                var crewList = info.campLeaderList;
                var trans = Transform.FindTransfrom("Content/CrewPanel/CrewContent");
                trans.InitObj(UIPath.PrefabPath.Leader_Prepare_Card, Config.GlobalConfigData.GamePrepare_Crew_Leader_Max);
                for (int i = 0; i < crewList.Count; i++)
                {
                    var item = trans.GetChild(i).SafeGetComponent<LeaderPrepareCard>();
                    item.SetUpItem( LeaderPrepareCard.State.ForceSelect, crewList[i]);
                    leaderCardList.Add(item);
                }
                ///Init Empty
                for(int j = crewList.Count; j< Config.GlobalConfigData.GamePrepare_Crew_Leader_Max; j++)
                {
                    var item = trans.GetChild(j).SafeGetComponent<LeaderPrepareCard>();
                    item.SetUpItem(LeaderPrepareCard.State.Empty,null, CampManager.Instance.PreparePage_CurrentSelect_CampID);
                    leaderCardList.Add(item);
                }
                return true;
            }
            return false;
        }

        bool AddLeader(LeaderInfo info)
        {
            ///Add First
            bool isFull = true;
            for(int i = 0; i < leaderCardList.Count; i++)
            {
                if(leaderCardList[i].currentState== LeaderPrepareCard.State.Empty)
                {
                    leaderCardList[i].SetUpItem(LeaderPrepareCard.State.Select_Prepare, info);
                    leaderCardList[i].ShowRemoveBtn();
                    DataManager.Instance.gamePrepareData.AddSelectLeaderInfo(info);
                    isFull = false;
                    break;
                }
            }
            return !isFull;
        }

        bool RemoveLeader(LeaderInfo info)
        {
            for(int i = 0; i < leaderCardList.Count; i++)
            {
                if(leaderCardList[i].currentState== LeaderPrepareCard.State.Select_Prepare && leaderCardList[i]._info==info)
                {
                    leaderCardList[i].SetUpItem(LeaderPrepareCard.State.Empty, null, CampManager.Instance.PreparePage_CurrentSelect_CampID);
                    DataManager.Instance.gamePrepareData.RemoveSelectLeaderInfo(info);
                    break;
                }
            }
            return true;
        }

        void SetUpGamePreparePanel()
        {
            InitPrepareItem();
            SetUpAIPanel();
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
                    var item = ObjectManager.Instance.InstantiateObject(UIPath.PrefabPath.SliderSelectItem_Prepare);
                    if (item != null)
                    {
                        item.transform.SetParent(trans,false);

                        var config = PlayerModule.GetGamePrepareConfigItem(list[i].configID);
                        var iconTrans = item.transform.FindTransfrom("Icon").SafeGetComponent<Image>().sprite = Utility.LoadSprite(config.configIconPath);
                        var nameTrans = item.transform.FindTransfrom("Text").SafeGetComponent<Text>().text = MultiLanguage.Instance.GetTextValue(config.configNameText);

                        var itemTrans= item.transform.FindTransfrom("GeneralSliderSelectItem").SafeGetComponent<SliderSelectItem>();
                        ///Init List
                        
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

                            itemTrans.SetUpItem_GamePrepare(config.defaultSelectLevel,elementList,config.configID);
                        }
                    }
                }
            }
        }


        void SetUpAIPanel()
        {
            var maintainSlider = Transform.FindTransfrom("Content/AIPanel/CrewContent/Maintenance/GeneralSliderSelectItem").SafeGetComponent<SliderSelectItem>();
            var maintainConfig = PlayerModule.GetAIPrepareConfigItem(Config.ConfigData.PlayerConfig.gamePrepareConfig.GamePrepareConfig_PropertyLink_AI_Maintenance);
            if (maintainConfig != null)
            {
                List<GeneralSliderSelectElement> elementList = new List<GeneralSliderSelectElement>();
                for (int i = 0; i < maintainConfig.levelMap.Count; i++)
                {
                    GeneralSliderSelectElement element = new GeneralSliderSelectElement
                    {
                        showScaleSymbol = maintainConfig.showScaleSymbol,
                        index = maintainConfig.levelMap[i].Level,
                        linkParam = maintainConfig.levelMap[i].hardLevelChange,
                        value = (float)maintainConfig.levelMap[i].numParam
                    };
                    elementList.Add(element);
                }
                maintainSlider.SetUpItem_AIPrepare(maintainConfig.defaultSelectLevel, elementList, maintainConfig.configID);
            }

            var builderSlider= Transform.FindTransfrom("Content/AIPanel/CrewContent/Builder/GeneralSliderSelectItem").SafeGetComponent<SliderSelectItem>();
            var builderConfig= PlayerModule.GetAIPrepareConfigItem(Config.ConfigData.PlayerConfig.gamePrepareConfig.GamePrepareConfig_PropertyLink_AI_Builder);
            if (builderConfig != null)
            {
                List<GeneralSliderSelectElement> elementList = new List<GeneralSliderSelectElement>();
                for (int i = 0; i < builderConfig.levelMap.Count; i++)
                {
                    GeneralSliderSelectElement element = new GeneralSliderSelectElement
                    {
                        showScaleSymbol = builderConfig.showScaleSymbol,
                        index = builderConfig.levelMap[i].Level,
                        linkParam = builderConfig.levelMap[i].hardLevelChange,
                        value = (float)builderConfig.levelMap[i].numParam
                    };
                    elementList.Add(element);
                }
                builderSlider.SetUpItem_AIPrepare(builderConfig.defaultSelectLevel, elementList, builderConfig.configID);
            }

            var operatorSlider= Transform.FindTransfrom("Content/AIPanel/CrewContent/Operator/GeneralSliderSelectItem").SafeGetComponent<SliderSelectItem>();
            var operatorConfig= PlayerModule.GetAIPrepareConfigItem(Config.ConfigData.PlayerConfig.gamePrepareConfig.GamePrepareConfig_PropertyLink_AI_Operator);
            if (operatorConfig != null)
            {
                List<GeneralSliderSelectElement> elementList = new List<GeneralSliderSelectElement>();
                for (int i = 0; i < operatorConfig.levelMap.Count; i++)
                {
                    GeneralSliderSelectElement element = new GeneralSliderSelectElement
                    {
                        showScaleSymbol = operatorConfig.showScaleSymbol,
                        index = operatorConfig.levelMap[i].Level,
                        linkParam = operatorConfig.levelMap[i].hardLevelChange,
                        value = (float)operatorConfig.levelMap[i].numParam
                    };
                    elementList.Add(element);
                }
                operatorSlider.SetUpItem_AIPrepare(operatorConfig.defaultSelectLevel, elementList, operatorConfig.configID);
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