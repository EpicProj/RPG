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
            UIManager.Instance.Register<BlockInfoDialogContent>(UIPath.WindowPath.FUNCTIONBLOCK_INFO_DIALOG);
            UIManager.Instance.Register<WareHousePageContext>(UIPath.WindowPath.WareHouse_Page);
            UIManager.Instance.Register<MainMenuPageContext>(UIPath.WindowPath.MainMenu_Page);
            UIManager.Instance.Register<GameEntryPageContext>(UIPath.WindowPath.Game_Entry_Page);
            UIManager.Instance.Register<GeneralConfirmDialogContext>(UIPath.WindowPath.General_Confirm_Dialog);
            UIManager.Instance.Register<GeneralHintDialogContent>(UIPath.WindowPath.General_Hint_Dialog);
            UIManager.Instance.Register<ConsolePageContext>(UIPath.WindowPath.Console_Page);
            UIManager.Instance.Register<GameLoadDialogContext>(UIPath.WindowPath.MainMenu_GameLoad_Dialog);
        }

    }
}