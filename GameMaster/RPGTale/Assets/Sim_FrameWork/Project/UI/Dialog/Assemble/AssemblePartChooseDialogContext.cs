using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sim_FrameWork.UI
{
    public class AssemblePartChooseDialogContext : WindowBase
    {

        private Transform noInfoTrans;
        private Animation noInfoAnim;
        private Transform tabContentTrans;

        private List<string> _sortTypeList=new List<string> ();
        private string currentSelcetTab;
        private LoopList _loopList;

        /// <summary>
        /// 展示方式
        /// 0=null
        /// 1=查看
        /// 2=选择
        /// </summary>
        byte dialogShowType = 0;
        int configID = 0;

        #region OverrideMethod
        public override void Awake(params object[] paralist)
        {
            base.Awake(paralist);
            _sortTypeList = (List<string>)paralist[0];
            currentSelcetTab = (string)paralist[1];
            dialogShowType = (byte)paralist[2];
            AddBtnClick();
        }

        public override bool OnMessage(UIMessage msg)
        {
            if(msg.type == UIMsgType.Assemble_PartTab_Select_ChooseDialog)
            {
                currentSelcetTab = (string)msg.content[0];
                return RefreshContent();
            }

            return true;
        }

        public override void OnShow(params object[] paralist)
        {
            base.OnShow(paralist);
            _sortTypeList = (List<string>)paralist[0];
            currentSelcetTab = (string)paralist[1];
            dialogShowType = (byte)paralist[2];
            configID = (int)paralist[3];
            SetUpDialog();
        }

        protected override void InitUIRefrence()
        {
            _loopList = Transform.FindTransfrom("Content/Context/Scroll View").SafeGetComponent<LoopList>();
            noInfoTrans = Transform.FindTransfrom("Content/EmptyInfo");
            noInfoAnim = noInfoTrans.SafeGetComponent<Animation>();
            tabContentTrans = Transform.FindTransfrom("Content/TabContent");
        }

        #endregion

        void AddBtnClick()
        {
            AddButtonClickListener(Transform.FindTransfrom("BG").SafeGetComponent<Button>(), () =>
            {
                UIManager.Instance.HideWnd(this);
            });
        }

        void SetUpDialog()
        {
            noInfoTrans.SafeSetActive(false);
            RefreshTab();
            InitDefaultSelectTab();
        }

        bool RefreshContent()
        {
            var partInfoList = PlayerManager.Instance.GetAssemblePartInfoByTypeID(currentSelcetTab);
            if (partInfoList.Count == 0)
            {
                noInfoTrans.SafeSetActive(true);
                if (noInfoAnim != null)
                    noInfoAnim.Play();
            }
            else
            {
                noInfoTrans.SafeSetActive(false);
                var dataModelList = PlayerManager.Instance.playerData.assemblePartData.GetAssemblePartChooseModel(currentSelcetTab);
                _loopList.InitData(dataModelList, new List<object>() { dialogShowType,configID });
            }
            return true;
        }

        void RefreshTab()
        {
            tabContentTrans.ReleaseAllChildObj();

            for (int i = 0; i < _sortTypeList.Count; i++)
            {
                var typeData = AssembleModule.GetAssemblePartMainType(_sortTypeList[i]);
                if (typeData != null)
                {
                    var obj = ObjectManager.Instance.InstantiateObject(UIPath.PrefabPath.General_ChooseTab);
                    if (obj != null)
                    {
                        var cmpt = obj.transform.SafeGetComponent<GeneralChooseTab>();
                        cmpt.SetUpTab(typeData,false);
                        cmpt.transform.SetParent(tabContentTrans, false);
                    }
                }
            }
        }

        void InitDefaultSelectTab()
        {
            if (AssembleModule.GetAssemblePartMainType(currentSelcetTab) != null)
            {
                RefreshContent();
            }
        }

    }
}