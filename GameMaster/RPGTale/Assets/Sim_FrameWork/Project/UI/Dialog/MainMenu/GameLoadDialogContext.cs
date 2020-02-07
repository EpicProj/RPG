using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sim_FrameWork.UI
{
    public enum GameSaveMode
    {
        None,
        LoadGame,
        CoverSave
    }
    public class GameLoadDialogContext : WindowBase
    {
        private GameSaveMode currentMode = GameSaveMode.None;

        private SaveElement[] saveGroupElementList;
        private SaveSubElement[] saveSubElementList;

        private int currentGroupID = -1;
        private int currentSaveIndexID = -1;

        private const string GameLoad_Confirm_Dialog_Title = "GameLoad_Confirm_Dialog_Title";
        private const string GameLoad_Confirm_Dialog_Cancel = "GameLoad_Confirm_Dialog_Cancel";
        private const string GameLoad_Confirm_Dialog_Confirm = "GameLoad_Confirm_Dialog_Confirm";
        private const string GameLoad_Confirm_Dialog_Content = "GameLoad_Confirm_Dialog_Content";

        private const string GameLoad_SaveDamaged_Dialog_Title = "GameLoad_SaveDamaged_Dialog_Title";
        private const string GameLoad_SaveDamaged_Dialog_Content = "GameLoad_SaveDamaged_Dialog_Content";
        private const string GameLoad_SaveDamaged_Dialog_Confrim = "GameLoad_SaveDamaged_Dialog_Confrim";

        private const string GameLoad_Hint_NeedSelectSave = "GameLoad_Hint_NeedSelectSave";

        #region Override Method
        public override void Awake(params object[] paralist)
        {
            base.Awake(paralist);
            AddBtnListener();
        }

        public override void OnShow(params object[] paralist)
        {
            base.OnShow(paralist);
            InitSaveList();
        }

        public override bool OnMessage(UIMessage msg)
        {
            if(msg.type== UIMsgType.GameSave_Select_Group)
            {
                int groupID = (int)msg.content[0];
                return OnGroupElementSelect(groupID);
            }
            else if(msg.type == UIMsgType.GameSave_Select_Save)
            {
                int indexID = (int)msg.content[0];
                return OnSelectItemSelect(indexID);
            }
            return false;
        }
        #endregion

        void AddBtnListener()
        {
            AddButtonClickListener(Transform.FindTransfrom("Back").SafeGetComponent<Button>(), () =>
            {
                UIManager.Instance.HideWnd(UIPath.WindowPath.MainMenu_GameLoad_Dialog);
                UIManager.Instance.SendMessageToWnd(UIPath.WindowPath.Game_Entry_Page, new UIMessage(UIMsgType.PlayMenuAnim));
            });
            AddButtonClickListener(Transform.FindTransfrom("Detail/Button/Load").SafeGetComponent<Button>(), OnGameLoadBtnClick);
            AddButtonClickListener(Transform.FindTransfrom("Detail/Button/Delete").SafeGetComponent<Button>(), OnSaveDeleteBtnClick);
        }

        void UpdateSaveElementList()
        { 
            saveSubElementList = null;
            saveSubElementList = Transform.FindTransfrom("Detail/Content/Scroll View/Viewport/Content").GetComponentsInChildren<SaveSubElement>();
        }
        void UpdateSaveGroupList()
        {
            saveGroupElementList = null;
            saveGroupElementList = Transform.FindTransfrom("Content/Content/Scroll View/Viewport/Content").GetComponentsInChildren<SaveElement>();
        }

        void UpdateNoDataInfo(bool active)
        {
            Transform.FindTransfrom("Detail/Content/Scroll View/Viewport/Content").SafeSetActiveAllChild(!active);
            if (active)
            {
                Transform.FindTransfrom("Detail/Content/NoDataInfo").SafeGetComponent<CanvasGroup>().DoCanvasFade(1, 0.5f);
            }
            else
            {
                Transform.FindTransfrom("Detail/Content/NoDataInfo").SafeGetComponent<CanvasGroup>().DoCanvasFade(0, 0.5f);
            }
        }

        void InitSaveList()
        {
            var loopList = Transform.FindTransfrom("Content/Content/Scroll View").SafeGetComponent<LoopList>();
            loopList.InitData(GameDataSaveManager.Instance.GetSaveGroupModel());
            UpdateSaveGroupList();
            currentGroupID = -1;
            currentSaveIndexID = -1;
            UpdateNoDataInfo(true);
            HideAllGroupElementSelect();
        }

        void InitSaveSubList(int groupID)
        {
            var loopList = Transform.FindTransfrom("Detail/Content/Scroll View").SafeGetComponent<LoopList>();
            var navList = GameDataSaveManager.Instance.GetSaveNavigatorDataList(groupID);
            List<BaseDataModel> modelList = new List<BaseDataModel>();
            for(int i = 0; i < navList.Count; i++)
            {
                SaveDataItemModel model = new SaveDataItemModel();
                if (model.Create(groupID, navList[i].Index))
                {
                    modelList.Add(model);
                }
            }
            loopList.InitData(modelList);
            UpdateSaveElementList();
            HideAllElementSelect();
        }

        SaveElement GetSaveElement(int groupID)
        {
            if (saveGroupElementList != null)
            {
                for(int i = 0; i < saveGroupElementList.Length; i++)
                {
                    if (saveGroupElementList[i]._model.ID == groupID)
                        return saveGroupElementList[i];
                }
            }
            return null;
        }

        SaveSubElement GetSaveSubElement(int indexID)
        {
            if (saveSubElementList != null)
            {
                for(int i = 0; i < saveSubElementList.Length; i++)
                {
                    if (saveSubElementList[i]._model._groupID == currentGroupID && saveSubElementList[i]._model._indexID == indexID)
                        return saveSubElementList[i];
                }
            }
            return null;
        }

        /// <summary>
        /// Select GroupItem
        /// </summary>
        /// <param name="groupID"></param>
        /// <returns></returns>
        bool OnGroupElementSelect(int groupID)
        {
            var element = GetSaveElement(groupID);
            if (element == null)
                return false;

            if (currentGroupID == groupID)
            {
                element.SetSelect(false);
                currentGroupID = -1;
                UpdateNoDataInfo(true);
            }
            else
            {
                currentGroupID = groupID;
                HideAllGroupElementSelect();
                element.SetSelect(true);

                UpdateNoDataInfo(false);
                InitSaveSubList(groupID);
            }
            return true;
        }
        void HideAllGroupElementSelect()
        {
            if (saveGroupElementList != null)
            {
                for (int i = 0; i < saveGroupElementList.Length; i++)
                    saveGroupElementList[i].SetSelect(false);
            }
        }

        /// <summary>
        /// Select SubItem
        /// </summary>
        /// <param name="saveIndex"></param>
        /// <returns></returns>
        bool OnSelectItemSelect(int saveIndex)
        {
            var element = GetSaveSubElement(saveIndex);
            if (element == null)
                return false;

            if(currentSaveIndexID == saveIndex)
            {
                //Show Dialog
                ShowConfirmDialog();
            }
            else
            {
                currentSaveIndexID = saveIndex;
                HideAllElementSelect();
                element.SetSelect(true);
            }
            return true;
        }
        void HideAllElementSelect()
        {
            if (saveSubElementList != null)
            {
                for (int i = 0; i < saveSubElementList.Length; i++)
                    saveSubElementList[i].SetSelect(false);
            }
        }

        void OnGameLoadBtnClick()
        {
            ShowConfirmDialog();
        }

        void OnSaveDeleteBtnClick()
        {

        }

        void ShowConfirmDialog()
        {
            if(currentGroupID!=-1 && currentSaveIndexID != -1)
            {
                if (GameDataSaveManager.Instance.CheckSaveDataComplete(currentGroupID,currentSaveIndexID))
                {
                    List<GeneralConfrimBtnItem> btns = new List<GeneralConfrimBtnItem>();
                    btns.Add(new GeneralConfrimBtnItem(
                        MultiLanguage.Instance.GetTextValue(GameLoad_Confirm_Dialog_Cancel),
                        () =>
                        {
                            UIManager.Instance.HideWnd(UIPath.WindowPath.General_Confirm_Dialog);
                        },
                         GeneralConfrimBtnItem.btnColor.Red));
                    btns.Add(new GeneralConfrimBtnItem(
                         MultiLanguage.Instance.GetTextValue(GameLoad_Confirm_Dialog_Confirm),
                         LoadGame));

                    GeneralConfirmDialogItem item = new GeneralConfirmDialogItem(
                        MultiLanguage.Instance.GetTextValue(GameLoad_Confirm_Dialog_Title),
                        MultiLanguage.Instance.GetTextValue(GameLoad_Confirm_Dialog_Content),
                        btns);
                       
                    UIGuide.Instance.ShowGeneralConfirmDialog(item);
                }
                else
                {
                    //Date Error!
                    List<GeneralConfrimBtnItem> btns = new List<GeneralConfrimBtnItem>();
                    btns.Add(new GeneralConfrimBtnItem(
                        MultiLanguage.Instance.GetTextValue(GameLoad_SaveDamaged_Dialog_Confrim),
                        () =>
                        {
                            UIManager.Instance.HideWnd(UIPath.WindowPath.General_Confirm_Dialog);
                        }));
                    GeneralConfirmDialogItem item = new GeneralConfirmDialogItem(
                        MultiLanguage.Instance.GetTextValue(GameLoad_SaveDamaged_Dialog_Title),
                        MultiLanguage.Instance.GetTextValue(GameLoad_SaveDamaged_Dialog_Content),
                        btns);

                    UIGuide.Instance.ShowGeneralConfirmDialog(item);
                }
            }
            else
            {
                //Not Select
                GeneralHintDialogItem hintItem = new GeneralHintDialogItem(
                    MultiLanguage.Instance.GetTextValue(GameLoad_Hint_NeedSelectSave), 2f);
                UIGuide.Instance.ShowGeneralHint(hintItem);
            }
        }
        void LoadGame()
        {
            var saveData = GameDataSaveManager.Instance.GetSaveData(currentGroupID, currentSaveIndexID);
            DebugPlus.Log("LoadSave Start");
            
            ScenesManager.Instance.LoadSceneStartCallBack = () =>
            {
                GameDataSaveManager.Instance.LoadAllSave(saveData);
            };
            ScenesManager.Instance.LoadingScene(UIPath.ScenePath.Scene_Test);
        }
    }


}