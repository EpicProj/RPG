using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sim_FrameWork.UI
{
    public partial class ExplorePointPageContext : WindowBase
    {
        private ExploreRandomItem _item;



        #region OverrideMethod
        public override void Awake(params object[] paralist)
        {
            base.Awake(paralist);
            _item = (ExploreRandomItem)paralist[0];
            AddBtnClick();
        }
        public override bool OnMessage(UIMessage msg)
        {
            return base.OnMessage(msg);
        }

        public override void OnShow(params object[] paralist)
        {
            _item = (ExploreRandomItem)paralist[0];
            AudioManager.Instance.PlaySound(AudioClipPath.UISound.Page_Open);

            SepUpBaseInfo();
            RefreshPoint();
        }


        void AddBtnClick()
        {
            AddButtonClickListener(m_page.backBtn, () =>
            {
                UIManager.Instance.HideWnd(this);
                UIGuide.Instance.ShowExploreMainPage();
            });
        }

        #endregion


        void SepUpBaseInfo()
        {
            if (_item == null)
                return;
            m_page.titleValue.text = _item.areaHardLevel.ToString();
            m_page.titleText.text = _item.missionName;
            currentEnergyValue.text = _item.teamData.EnergyStartNum.ToString();
        }

        void RefreshPoint()
        {
            if (_item == null || _item.currentUnlockPointlist == null)
                return;
            for(int i = 0; i < _item.currentUnlockPointlist.Count; i++)
            {
                var obj = ObjectManager.Instance.InstantiateObject(UIPath.PrefabPath.Explore_Point_Element);
                if (obj != null)
                {
                    var cmpt = UIUtility.SafeGetComponent<ExplorePointCmpt>(obj.transform);
                    if (cmpt != null)
                    {
                        cmpt.InitPoint(_item.currentUnlockPointlist[i]);
                        obj.transform.SetParent(pointContentTrans, false);
                    }
                }
            }
        }

    }


    public partial class ExplorePointPageContext : WindowBase
    {
        private ExplorePointPage m_page;

        private Text currentEnergyValue;

        private Transform pointContentTrans;

        protected override void InitUIRefrence()
        {
            m_page = UIUtility.SafeGetComponent<ExplorePointPage>(Transform);
            currentEnergyValue = UIUtility.SafeGetComponent<Text>(UIUtility.FindTransfrom(m_page.leftPanel, "TeamState/Energy/Value"));
            pointContentTrans = UIUtility.FindTransfrom(Transform, "Content/PointContent");
        }
    }
}