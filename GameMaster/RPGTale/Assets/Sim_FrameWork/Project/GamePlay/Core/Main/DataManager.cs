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
            FunctionBlockModule.Instance.Register();
            MaterialModule.Instance.Register();
            DistrictModule.Instance.Register();
            FormulaModule.Instance.Register();
            PlayerModule.Instance.Register();
            CampModule.Instance.Register();
            OrganizationModule.Instance.Register();
            OrderModule.Instance.Register();
        }

        public void RegisterUI()
        {
            UIManager.Instance.Register<BlockInfoDialogContent>(UIPath.FUNCTIONBLOCK_INFO_DIALOG);
            UIManager.Instance.Register<WareHouseDialogContent>(UIPath.WAREHOURSE_DIALOG);
            UIManager.Instance.Register<MainMenuPageContext>(UIPath.MainMenu_Page);
            UIManager.Instance.Register<GameEntryPageContext>(UIPath.Game_Entry_Page);
            UIManager.Instance.Register<LoadingPageContext>(UIPath.Loading_Scene_Page);
            UIManager.Instance.Register<GeneralConfirmDialogContext>(UIPath.General_Confirm_Dialog);
            UIManager.Instance.Register<GeneralHintDialogContent>(UIPath.General_Hint_Dialog);
            UIManager.Instance.Register<ConsolePageContext>(UIPath.Console_Page); 
        }

    }
}