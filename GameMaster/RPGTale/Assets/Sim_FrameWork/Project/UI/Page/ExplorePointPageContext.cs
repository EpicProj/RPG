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
            if(msg.type == UIMsgType.ExplorePage_Show_PointDetail)
            {
                ExplorePointData pointData = (ExplorePointData)msg.content[0];
                return OnClickPoint(pointData);
            }
            else if(msg.type == UIMsgType.ExplorePage_Finish_Point)
            {

            }
            return true;
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
            if (_item == null || _item.currentPointlist == null)
                return;
            for(int i = 0; i < _item.currentPointlist.Count; i++)
            {
                var obj = ObjectManager.Instance.InstantiateObject(UIPath.PrefabPath.Explore_Point_Element);
                if (obj != null)
                {
                    var cmpt = UIUtility.SafeGetComponent<ExplorePointCmpt>(obj.transform);
                    if (cmpt != null)
                    {
                        cmpt.InitPoint(_item.currentPointlist[i]);
                        obj.transform.SetParent(pointContentTrans, false);
                    }
                }
            }
        }

        bool OnClickPoint(ExplorePointData point)
        {
            if (point == null)
                return false;
            pointDetailDescText.text = point.pointDesc;
            pointDetailTitleText.text = point.pointName;
            
            foreach(Transform trans in pointDetailWarningLevelTrans)
            {
                trans.gameObject.SetActive(false);
            }
            for(int i = 0; i < point.HardLevel; i++)
            {
                pointDetailWarningLevelTrans.GetChild(0).gameObject.SetActive(true);
            }

            ///Set Btn States
            if(point.currentState == ExplorePointData.PointState.Unlock)
            {
                pointDetailConfirmBtnText.text = MultiLanguage.Instance.GetTextValue(Explore_Point_DetailBtn_Ready);
                pointDetailConfirmBtn.onClick.AddListener(() =>
                {
                    UIGuide.Instance.ShowRandomEventDialog(point.eventID,point.AreaID,point.ExploreID,point.PointID);
                });
            }else if(point.currentState == ExplorePointData.PointState.Lock || point.currentState== ExplorePointData.PointState.None)
            {
                pointDetailConfirmBtnText.text = MultiLanguage.Instance.GetTextValue(Explore_Point_DetailBtn_Lock);
                pointDetailConfirmBtn.onClick.AddListener(() =>
                {
                    UIGuide.Instance.ShowGeneralHint(new GeneralHintDialogItem
                        (MultiLanguage.Instance.GetTextValue(Explore_Point_DetailBtn_Lock_Hint),
                        3));

                });
            }
            if (pointDetailPanelAnim != null)
                pointDetailPanelAnim.Play();


            return true;

        }

    }


    public partial class ExplorePointPageContext : WindowBase
    {
        private ExplorePointPage m_page;

        private Text currentEnergyValue;

        private Transform pointContentTrans;

        private Transform pointDetailPanel;
        private Animation pointDetailPanelAnim;
        private Text pointDetailDescText;
        private Transform pointDetailWarningLevelTrans;
        private Button pointDetailConfirmBtn;
        private Text pointDetailConfirmBtnText;
        private Text pointDetailTitleText;

        private const string Explore_Point_DetailBtn_Ready = "Explore_Point_DetailBtn_Ready";
        private const string Explore_Point_DetailBtn_Lock = "Explore_Point_DetailBtn_Lock";

        private const string Explore_Point_DetailBtn_Lock_Hint = "Explore_Point_DetailBtn_Lock_Hint";

        protected override void InitUIRefrence()
        {
            m_page = UIUtility.SafeGetComponent<ExplorePointPage>(Transform);
            currentEnergyValue = UIUtility.SafeGetComponent<Text>(UIUtility.FindTransfrom(m_page.leftPanel, "TeamState/Energy/Value"));
            pointContentTrans = UIUtility.FindTransfrom(Transform, "Content/PointContent");
            pointDetailPanel = UIUtility.FindTransfrom(Transform, "Content/RightPanel");
            pointDetailPanelAnim = UIUtility.SafeGetComponent<Animation>(pointDetailPanel);

            pointDetailDescText = UIUtility.SafeGetComponent<Text>(UIUtility.FindTransfrom(pointDetailPanel, "Content/PointDesc"));
            pointDetailWarningLevelTrans = UIUtility.FindTransfrom(pointDetailPanel, "Content/WaringLevel/Level");
            pointDetailConfirmBtn = UIUtility.SafeGetComponent<Button>(UIUtility.FindTransfrom(pointDetailPanel, "Content/Confirm/Btn"));
            pointDetailConfirmBtnText = UIUtility.SafeGetComponent<Text>(UIUtility.FindTransfrom(pointDetailConfirmBtn.transform, "Text"));
            pointDetailTitleText= UIUtility.SafeGetComponent<Text>(UIUtility.FindTransfrom(pointDetailPanel, "Content/Title"));
        }
    }
}