using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sim_FrameWork.UI
{
    public class CampSelectMainPageContext : WindowBase
    {
        private int currentSelectCampID =-1;
        private List<CampSelectItem> _selectItemList;

        #region OverrideMethod
        public override void Awake(params object[] paralist)
        {
            base.Awake(paralist);
            _selectItemList = new List<CampSelectItem>();
            AddBtnClick();
           
        }

        public override void OnShow(params object[] paralist)
        {
            base.OnShow(paralist);
            SetUpCampSelectPanel();
        }

        public override bool OnMessage(UIMessage msg)
        {
            if(msg.type == UIMsgType.CampSelectPage_SelectCamp)
            {
                int selectID = (int)msg.content[0];
                return OnCampSelect(selectID);
            }
            return false;
        }


        #endregion

        void AddBtnClick()
        {
            AddButtonClickListener(Transform.FindTransfrom("Back").SafeGetComponent<Button>(), () =>
            {
                UIManager.Instance.HideWnd(this);
                UIGuide.Instance.ShowNewGamePreparePage();
            });
            AddButtonClickListener(Transform.FindTransfrom("BtnPanel/ChooseBtn").SafeGetComponent<Button>(), OnChooseBtnClick);
            AddButtonClickListener(Transform.FindTransfrom("BtnPanel/CustomBtn").SafeGetComponent<Button>(), OnCustomBtnClick);
        }

        void OnChooseBtnClick()
        {
            if (currentSelectCampID != -1)
            {
                UIManager.Instance.HideWnd(this);
                UIGuide.Instance.ShowNewGamePreparePage();
                UIManager.Instance.SendMessageToWnd(UIPath.WindowPath.NewGame_Prepare_Page, new UIMessage(UIMsgType.NewGamePage_UpdateCamp, new List<object>() { currentSelectCampID }));
            }
        }

        void OnCustomBtnClick()
        {

        }
        
        void SetUpCampSelectPanel()
        {
            _selectItemList.Clear();
            var loopList = Transform.FindTransfrom("Content/LeftPanel/SelectContent/Scroll View").SafeGetComponent<LoopList>();
            var modelList = CampModule.GetCampInfoModel();
            loopList.InitData(modelList);

            ///Init Select
            foreach(Transform trans in loopList.transform.FindTransfrom("Viewport/Content"))
            {
                var item = trans.SafeGetComponent<CampSelectItem>();
                _selectItemList.Add(item);
            }
            
        }

        bool OnCampSelect(int selectID)
        {
            CampSelectItem item = null;
            item=_selectItemList.Find(x => x._model.ID == selectID);
            if (item == null)
                return false;

            for(int i = 0; i < _selectItemList.Count; i++)
            {
                _selectItemList[i].SetSelect(selectID);
            }
            currentSelectCampID = selectID;
            return true;
        }
    }
}