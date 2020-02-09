using Sim_FrameWork.UI;

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
            GlobalEventManager.Instance.InitData();
            ModifierManager.Instance.InitData();
            PlayerManager.Instance.InitPlayerData();
            MainShipManager.Instance.InitData();
        }
    }
}