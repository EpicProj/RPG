using Sim_FrameWork.UI;

namespace Sim_FrameWork {
    public class DataManager : Singleton<DataManager> {


        public void InitData() { }


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
        
        }

        public void RegisterUI()
        {
            UIManager.Instance.Register<MainMenuPageContext>(UIPath.WindowPath.MainMenu_Page);
            UIManager.Instance.Register<GameEntryPageContext>(UIPath.WindowPath.Game_Entry_Page);
            UIManager.Instance.Register<ConsolePageContext>(UIPath.WindowPath.Console_Page);
            UIManager.Instance.Register<GameLoadDialogContext>(UIPath.WindowPath.MainMenu_GameLoad_Dialog);
            UIManager.Instance.Register<ProductLineChangeDialogContext>(UIPath.WindowPath.ProductLine_Change_Dialog);
            UIManager.Instance.Register<BuildPanelDetailContext>(UIPath.WindowPath.BuildPanel_Detail_UI);
        }

    }
}