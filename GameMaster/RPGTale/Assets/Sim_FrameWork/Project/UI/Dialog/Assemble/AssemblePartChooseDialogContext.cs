using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork.UI
{
    public class AssemblePartChooseDialogContext : WindowBase
    {

        private AssemblePartChooseDialog m_dialog;

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
            AudioManager.Instance.PlaySound(AudioClipPath.UISound.Page_Open);
            _sortTypeList = (List<string>)paralist[0];
            currentSelcetTab = (string)paralist[1];
            dialogShowType = (byte)paralist[2];
            configID = (int)paralist[3];
            SetUpDialog();
        }

        protected override void InitUIRefrence()
        {
            m_dialog = UIUtility.SafeGetComponent<AssemblePartChooseDialog>(Transform);
            _loopList = UIUtility.SafeGetComponent<LoopList>(UIUtility.FindTransfrom(m_dialog.contentTrans, "Scroll View"));
            noInfoTrans = UIUtility.FindTransfrom(Transform, "Content/EmptyInfo");
            noInfoAnim = UIUtility.SafeGetComponent<Animation>(noInfoTrans);
            tabContentTrans = UIUtility.FindTransfrom(Transform, "Content/TabContent");
        }

        #endregion

        void AddBtnClick()
        {
            AddButtonClickListener(m_dialog.closeBtn, () =>
            {
                UIManager.Instance.HideWnd(this);
                AudioManager.Instance.PlaySound(AudioClipPath.UISound.Btn_Close);
            });
        }

        void SetUpDialog()
        {
            noInfoTrans.gameObject.SetActive(false);
            RefreshTab();
            InitDefaultSelectTab();
        }

        bool RefreshContent()
        {
            var partInfoList = PlayerManager.Instance.GetAssemblePartInfoByTypeID(currentSelcetTab);
            if (partInfoList.Count == 0)
            {
                noInfoTrans.gameObject.SetActive(true);
                if (noInfoAnim != null)
                    noInfoAnim.Play();
            }
            else
            {
                noInfoTrans.gameObject.SetActive(false);
                var dataModelList = PlayerManager.Instance.GetAssemblePartChooseModel(currentSelcetTab);
                _loopList.InitData(dataModelList, new List<object>() { dialogShowType,configID });
            }
            return true;
        }

        void RefreshTab()
        {
            foreach(Transform trans in tabContentTrans)
            {
                ObjectManager.Instance.ReleaseObject(trans.gameObject, 0);
            }

            for (int i = 0; i < _sortTypeList.Count; i++)
            {
                var typeData = PlayerManager.Instance.GetAssemblePartMainTypeData(_sortTypeList[i]);
                if (typeData != null)
                {
                    var obj = ObjectManager.Instance.InstantiateObject(UIPath.PrefabPath.General_ChooseTab);
                    if (obj != null)
                    {
                        var cmpt = UIUtility.SafeGetComponent<GeneralChooseTab>(obj.transform);
                        cmpt.SetUpTab(typeData,false);
                        cmpt.transform.SetParent(tabContentTrans, false);
                    }
                }
            }
        }

        void InitDefaultSelectTab()
        {
            if (PlayerManager.Instance.GetAssemblePartMainTypeData(currentSelcetTab) != null)
            {
                RefreshContent();
            }
        }

    }
}