﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sim_FrameWork.UI
{
    public partial class ExplorePointPageContext : WindowBase
    {
        private ExploreRandomItem _item;
        private List<ExplorePointCmpt> _pointTransList;


        #region OverrideMethod
        public override void Awake(params object[] paralist)
        {
            base.Awake(paralist);
            _item = (ExploreRandomItem)paralist[0];
            _pointTransList = new List<ExplorePointCmpt>();
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

            }else if(msg.type == UIMsgType.ExplorePage_Update_PointTimer)
            {
                ExplorePointData pointData = (ExplorePointData)msg.content[0];
                return RefreshPointTimer(pointData);
            }
            return true;
        }

        public override void OnShow(params object[] paralist)
        {
            base.OnShow(paralist);
            _item = (ExploreRandomItem)paralist[0];

            SepUpBaseInfo();
            RefreshPoint();
        }


        void AddBtnClick()
        {
            AddButtonClickListener(Transform.FindTransfrom("Back").SafeGetComponent<Button>(), () =>
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
            Transform.FindTransfrom("Title/Text/Icon/Text").SafeGetComponent<Text>().text = _item.areaHardLevel.ToString();
            Transform.FindTransfrom("Title/Text").SafeGetComponent<Text>().text = _item.missionName;
            currentEnergyValue.text = _item.teamData.EnergyStartNum.ToString();
        }

        void RefreshPoint()
        {
            _pointTransList.Clear();
            if (_item == null || _item.currentPointlist == null)
                return;
            for(int i = 0; i < _item.currentPointlist.Count; i++)
            {
                var obj = ObjectManager.Instance.InstantiateObject(UIPath.PrefabPath.Explore_Point_Element);
                if (obj != null)
                {
                    var cmpt = obj.transform.SafeGetComponent<ExplorePointCmpt>();
                    if (cmpt != null)
                    {
                        cmpt.InitPoint(_item.currentPointlist[i]);
                        obj.transform.SetParent(pointContentTrans, false);
                        _pointTransList.Add(cmpt);
                    }
                }
            }
        }

        /// <summary>
        /// 刷新点位倒计时
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        bool RefreshPointTimer(ExplorePointData point)
        {
            if (point == null)
                return false;
            foreach(var cmpt in _pointTransList)
            {
                if(cmpt.pointData.PointID == point.PointID)
                {
                    cmpt.RefreshPointTimer(point);
                }
            }
            return true;         
        }

        bool OnClickPoint(ExplorePointData point)
        {
            if (point == null)
                return false;
            pointDetailDescText.text = point.pointDesc;
            pointDetailTitleText.text = point.pointName;
            pointDetailTimeCostText.text = point.TimeCost.ToString() + " " + MultiLanguage.Instance.GetTextValue(Config.GeneralTextData.Game_Time_Text_Day);
            pointDetailEnergyCostText.text = point.EnergyCost.ToString();

            pointDetailWarningLevelTrans.SafeSetActiveAllChild(false);
            for (int i = 0; i < point.HardLevel; i++)
            {
                pointDetailWarningLevelTrans.GetChild(i).gameObject.SetActive(true);
            }

            ///Set Btn States
            if (point.currentState == ExplorePointData.PointState.Unlock)
            {
                ///Check Energy
                if (point.EnergyCost > _item.teamData.EnergyCurrentNum)
                {
                    pointDetailConfirmBtn.interactable = false;
                    pointDetailConfirmBtnText.text = MultiLanguage.Instance.GetTextValue(Explore_Point_DetailBtn_EnergyLack);
                    return true;
                }
                else
                {
                    pointDetailConfirmBtn.interactable = true;
                    pointDetailConfirmBtnText.text = MultiLanguage.Instance.GetTextValue(Explore_Point_DetailBtn_Ready);
                    pointDetailConfirmBtn.onClick.AddListener(
                        () => {
                            StartExplore(point);
                        });
                }
               
            }
            else if(point.currentState == ExplorePointData.PointState.Lock || point.currentState== ExplorePointData.PointState.None)
            {
                pointDetailConfirmBtn.interactable = false;
                pointDetailConfirmBtnText.text = MultiLanguage.Instance.GetTextValue(Explore_Point_DetailBtn_Lock);
            }
            else if(point.currentState == ExplorePointData.PointState.Finish)
            {
                pointDetailConfirmBtn.interactable = false;
                pointDetailConfirmBtnText.text = MultiLanguage.Instance.GetTextValue(Explore_Point_DetailBtn_Finish);
            }
            else if(point.currentState == ExplorePointData.PointState.Doing)
            {
                pointDetailConfirmBtn.interactable = false;
                pointDetailConfirmBtnText.text = MultiLanguage.Instance.GetTextValue(Explore_Point_DetailBtn_Doing);
            }

            if (pointDetailPanelAnim != null)
                pointDetailPanelAnim.Play();

            return true;
        }

        void StartExplore(ExplorePointData point)
        {
            if (point == null)
                return;

            ExploreEventManager.Instance.StartExplorePoint(point.AreaID, point.ExploreID, point.PointID);
            foreach (var cmpt in _pointTransList)
            {
                if (cmpt.pointData.PointID == point.PointID)
                {
                    cmpt.RefreshPointTimer(point);
                }
            }
            ///Show Tip
            UIGuide.Instance.ShowGeneralHint(new GeneralHintDialogItem(
                Utility.ParseStringParams(MultiLanguage.Instance.GetTextValue(Explore_Point_StartExplore_Hint), new string[1] { point.pointName }),
                1.5f));
            ///Change Btn States
            pointDetailConfirmBtn.interactable = false;
            pointDetailConfirmBtnText.text = MultiLanguage.Instance.GetTextValue(Explore_Point_DetailBtn_Doing);
        }


        bool ExplorePointFinish(ExplorePointData point)
        {



            return true;
        }



    }


    public partial class ExplorePointPageContext : WindowBase
    {

        private Text currentEnergyValue;

        private Transform pointContentTrans;

        private Transform pointDetailPanel;
        private Animation pointDetailPanelAnim;
        private Text pointDetailDescText;
        private Transform pointDetailWarningLevelTrans;
        private Button pointDetailConfirmBtn;
        private Text pointDetailConfirmBtnText;
        private Text pointDetailTitleText;
        private Text pointDetailTimeCostText;
        private Text pointDetailEnergyCostText;

        private const string Explore_Point_DetailBtn_Ready = "Explore_Point_DetailBtn_Ready";
        private const string Explore_Point_DetailBtn_Lock = "Explore_Point_DetailBtn_Lock";
        private const string Explore_Point_DetailBtn_Finish = "Explore_Point_DetailBtn_Finish";
        private const string Explore_Point_DetailBtn_Doing = "Explore_Point_DetailBtn_Doing";
        private const string Explore_Point_DetailBtn_EnergyLack = "Explore_Point_DetailBtn_EnergyLack";

        private const string Explore_Point_StartExplore_Hint = "Explore_Point_StartExplore_Hint";

        protected override void InitUIRefrence()
        {
            currentEnergyValue = Transform.FindTransfrom("Content/LeftPanel/TeamState/Energy/Value").SafeGetComponent<Text>();
            pointContentTrans = Transform.FindTransfrom("Content/PointContent");
            pointDetailPanel = Transform.FindTransfrom("Content/RightPanel");
            pointDetailPanelAnim = pointDetailPanel.SafeGetComponent<Animation>();

            pointDetailDescText = pointDetailPanel.FindTransfrom("Content/PointDesc").SafeGetComponent<Text>();
            pointDetailWarningLevelTrans = pointDetailPanel.FindTransfrom("Content/WaringLevel/Level");
            pointDetailConfirmBtn = pointDetailPanel.FindTransfrom("Content/Confirm/Btn").SafeGetComponent<Button>();
            pointDetailConfirmBtnText = pointDetailConfirmBtn.transform.FindTransfrom("Text").SafeGetComponent<Text>();
            pointDetailTitleText= pointDetailPanel.FindTransfrom("Content/Title").SafeGetComponent<Text>();
            pointDetailTimeCostText= pointDetailPanel.FindTransfrom("Content/TimeCost/Text").SafeGetComponent<Text>();
            pointDetailEnergyCostText= pointDetailPanel.FindTransfrom("Content/EnergyCost/Text").SafeGetComponent<Text>();
        }
    }
}