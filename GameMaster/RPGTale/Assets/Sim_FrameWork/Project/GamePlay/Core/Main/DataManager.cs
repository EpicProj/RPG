﻿using Sim_FrameWork.UI;
using System.Collections.Generic;

namespace Sim_FrameWork {
    public class DataManager : Singleton<DataManager> {

        public GamePrepareData gamePrepareData;
        public void InitManager() { }

        public DataManager()
        {
            RegisterModule();
            RegisterUI();
        }

        public void RegisterModule()
        {
            GeneralModule.Instance.Register();
            FunctionBlockModule.Instance.Register();
            MaterialModule.Instance.Register();
            DistrictModule.Instance.Register();
            FormulaModule.Instance.Register();
            PlayerModule.Instance.Register();
            OrganizationModule.Instance.Register();
            OrderModule.Instance.Register();
            TechnologyModule.Instance.Register();
            RandomEventModule.Instance.Register();
            ExploreModule.Instance.Register();
            AssembleModule.Instance.Register();
            MainShipModule.Instance.Register();
            CampModule.Instance.Register();
            LeaderModule.Instance.Register();
        }

        public void InitGameBaseData()
        {
            gamePrepareData = GamePrepareData.InitData();
        }

        public void RegisterUI()
        {
            UIManager.Instance.Register<ConsolePageContext>(UIPath.WindowPath.Console_Page);
            UIManager.Instance.Register<ProductLineChangeDialogContext>(UIPath.WindowPath.ProductLine_Change_Dialog);
            UIManager.Instance.Register<BuildPanelDetailContext>(UIPath.WindowPath.BuildPanel_Detail_UI);
        }

        /// <summary>
        /// New Game
        /// </summary>
        public void InitGameData()
        {
            RefreshGamePrepareData();
            GlobalEventManager.Instance.InitData();
            ModifierManager.Instance.InitData();
            PlayerManager.Instance.InitPlayerData();
            MainShipManager.Instance.InitData();
        }


        #region GamePrepare Data
        public void ChangeGamePrepareValue(string configID,byte selectLevel)
        {
            for(int i = 0; i < gamePrepareData.preparePropertyDataList.Count; i++)
            {
                if (gamePrepareData.preparePropertyDataList[i].configID == configID)
                {
                    gamePrepareData.preparePropertyDataList[i].currentSelectLevel = selectLevel;
                }
            }
        }

        public void ChangeAIPrepareValue(string configID,byte selectLevel)
        {
            for(int i = 0; i < gamePrepareData.prepareAIDataList.Count; i++)
            {
                if (gamePrepareData.prepareAIDataList[i].configID == configID)
                {
                    gamePrepareData.prepareAIDataList[i].currentSelectLevel = selectLevel;
                }
            }
        }

        public void ChangeSelectCamp(CampInfo info)
        {
            gamePrepareData.currentCampInfo = info;
        }

        public void RefreshGamePrepareData()
        {
            gamePrepareData.RefreshData();
        }

        public List<BaseDataModel> GetCampLeaderSelectModelList(int campID)
        {
            List<BaseDataModel> result = new List<BaseDataModel>();
            var list = CampModule.GetCampLeaderSelectPresetList(campID);

            for(int i = 0; i < gamePrepareData.currentLeaderInfoList.Count; i++)
            {
                ///Remove AlreadySelect
                if (gamePrepareData.currentLeaderInfoList[i].forceSelcet)
                    continue;
                for(int j = 0; j < list.Count; j++)
                {
                    if(list[j].leaderID== gamePrepareData.currentLeaderInfoList[i].leaderID)
                    {
                        list.RemoveAt(j);
                    }
                }
            }

            for (int i = 0; i < list.Count; i++)
            {
                LeaderDataModel model = new LeaderDataModel();
                if (model.CreateLeaderModel(list[i].leaderID))
                {
                    result.Add(model);
                }
            }
            return result;
        }

        #endregion
    }
}