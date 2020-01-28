using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sim_FrameWork.UI
{
    public partial class ExploreMainPageContext : WindowBase
    {
        private const float mission_Appear_time = 0.1f;

        private int currentSelectAreaID = 0;
        private int currentSelectMissionID = 0;


        #region Override Method
        public override void Awake(params object[] paralist)
        {
            base.Awake(paralist);
            AddBtnClick();
        }

        public override bool OnMessage(UIMessage msg)
        {
            if(msg.type== UIMsgType.ExplorePage_ShowArea_Mission)
            {
                ExploreAreaData data = (ExploreAreaData)msg.content[0];
                return ShowAreaMissionPanel(data);
            }else if(msg.type== UIMsgType.ExplorePage_Show_MissionDetail)
            {
                ExploreRandomItem item = (ExploreRandomItem)msg.content[0];
                return ShowMissionAreaDetailPanel(item);
            }
            return false;
        }

        public override void OnShow(params object[] paralist)
        {
            AudioManager.Instance.PlaySound(AudioClipPath.UISound.Page_Open);
            
            InitAreaExploreList(ExploreAreaType.space);
            InitMissionPanelElement();
            InitMissionTeam();

            if (pageAnim != null)
                pageAnim.Play();

        }

        public override void OnClose()
        {
            UIGuide.Instance.ShowGameMainPage();
        }


        public void AddBtnClick()
        {
            AddButtonClickListener(Transform.FindTransfrom("Back").SafeGetComponent<Button>(), () =>
            {
                UIGuide.Instance.ShowGameMainPage();
            });
            AddButtonClickListener(exploreBtn, OnExploreBtnClick);
            AddButtonClickListener(energyAddBtn, OnEnergyAddBtnClick);
            AddButtonClickListener(energyReduceBtn, OnEnergyReduceBtnClick);
        }


        #endregion


        bool RefreshAreaExploreProgress(ExploreAreaData data)
        {
            if (data != null)
            {
                progressText.text = ((int)(data.areaTotalProgress * 100)).ToString() + "%";
                progressSlider.value = ((int)data.areaTotalProgress) * 100;
                return true;
            }
            return false;
        }

        void InitAreaExploreList(ExploreAreaType type)
        {
            var list = ExploreEventManager.Instance.CurrentExploreAreaList(type);
            areaSelectTrans.InitObj(UIPath.PrefabPath.Explore_Area_Select_Btn, list.Count);

            for (int i = 0; i < areaSelectTrans.childCount; i++)
            {
                var cmpt = areaSelectTrans.GetChild(i).SafeGetComponent<ExploreAreaSelectBtn>();
                cmpt.InitAreaItem(list[i]);
            }

        }

        bool ShowAreaMissionPanel(ExploreAreaData data)
        {
            if(data != null)
            {
                if (exploreMissionAnim != null)
                {
                    exploreMissionAnim.Play();
                }
                currentSelectAreaID = data.areaID;
                RefreshAreaExploreProgress(data);

                areaTitle.text = data.areaName;
                areaDesc.text = data.areaDesc;
                if (areaDescTypeEffect != null)
                    areaDescTypeEffect.StartEffect();

                ///RefreshMission
                missionPanelTrans.SafeSetActiveAllChild(false);
                ApplicationManager.Instance.StartCoroutine(ShowMission(data, mission_Appear_time));
              
                return true;
            }
            return false;
        }

        IEnumerator ShowMission(ExploreAreaData data, float waitTime)
        {
            if (data.currentMissionList == null)
                yield return null;
            for (int i = 0; i < data.currentMissionList.Count; i++)
            {
                if (i < Config.GlobalConfigData.ExplorePage_Mission_Max_Count)
                {
                    var element = missionPanelTrans.GetChild(i).SafeGetComponent<ExploreAreaMissionElement>();
                    if (element != null)
                    {
                        yield return new WaitForSeconds(waitTime);
                        element.SetUpElement(data.currentMissionList[i]);
                        element.gameObject.SetActive(true);
                        element.ShowMission();
                    }
                }
            }
        }

        void InitMissionPanelElement()
        {
            if (missionPanelTrans.childCount == Config.GlobalConfigData.ExplorePage_Mission_Max_Count)
                return;
            for(int i = 0; i < Config.GlobalConfigData.ExplorePage_Mission_Max_Count; i++)
            {
                var obj = ObjectManager.Instance.InstantiateObject(UIPath.PrefabPath.Explore_Mission_Element);
                if (obj != null)
                {
                    obj.transform.SetParent(missionPanelTrans);
                    obj.name="Mission_"+i;
                }
            }
        }

        bool ShowMissionAreaDetailPanel(ExploreRandomItem item)
        {
            if (item == null)
                return false;
            currentSelectMissionID = item.exploreID;

            teamGoodsValue.text = "0";
            missionDetailText.text = item.missionDesc;
            missionDetailBG.sprite = item.missionBG;
            if (missionDetailTypeEffect != null)
                missionDetailTypeEffect.StartEffect();

            ///SetUp Team
            exploreTeamContentTrans.SafeSetActiveAllChild(false);

            for (int i = 0; i < item.maxTeamNum; i++)
            {
                var element = exploreTeamContentTrans.GetChild(i);
                if (element != null)
                {
                    element.gameObject.SetActive(true);
                }
            }

            if (exploreAreaDetailAnim != null)
                exploreAreaDetailAnim.Play();
            return true;
        }

        void InitMissionTeam()
        {
            if (exploreTeamContentTrans.childCount == Config.GlobalConfigData.Explore_Mission_Max_Team_Count)
                return;
            for(int i = 0; i < Config.GlobalConfigData.Explore_Mission_Max_Team_Count; i++)
            {
                var obj = ObjectManager.Instance.InstantiateObject(UIPath.PrefabPath.Explore_Mission_Team_Obj);
                if (obj != null)
                {
                    obj.transform.SetParent(exploreTeamContentTrans, false);
                    obj.name = "Team_" + i;
                }
            }
        }

        void OnExploreBtnClick()
        {
            PlayerExploreTeamData teamData = new PlayerExploreTeamData(
                (ushort)Utility.TryParseInt(energyInputField.text),
                (ushort)Utility.TryParseInt(teamGoodsValue.text)
                );
            AudioManager.Instance.PlaySound(AudioClipPath.UISound.Button_Click);

            ExploreEventManager.Instance.StartExplore(currentSelectAreaID,currentSelectMissionID,teamData);
        }

        void OnEnergyReduceBtnClick()
        {
            AudioManager.Instance.PlaySound(AudioClipPath.UISound.Button_General);
            if ((ushort)Utility.TryParseInt(energyInputField.text) <= 0)
                return;
            energyInputField.text= ((ushort)Utility.TryParseInt(energyInputField.text) -1).ToString() ;
        }
        void OnEnergyAddBtnClick()
        {
            AudioManager.Instance.PlaySound(AudioClipPath.UISound.Button_General);
            if ((ushort)Utility.TryParseInt(energyInputField.text) >= 999)
                return;
            energyInputField.text = ((ushort)Utility.TryParseInt(energyInputField.text) +1).ToString();
        }


    }



    public partial class ExploreMainPageContext : WindowBase
    {

        private Text areaTitle;
        private Text areaDesc;
        /// <summary>
        /// Mission Detail
        /// </summary>
        private Text missionDetailText;
        private Image missionDetailBG;
        private Transform exploreTeamContentTrans;

        private Slider progressSlider;
        private Text progressText;

        private Transform areaSelectTrans;
        private Transform missionPanelTrans;

        private Animation exploreMissionAnim;
        private Animation exploreAreaDetailAnim;
        private Animation pageAnim;

        private TypeWriterEffect areaDescTypeEffect;
        private TypeWriterEffect missionDetailTypeEffect;

        private Button exploreBtn;
        private InputField energyInputField;
        private Button energyReduceBtn;
        private Button energyAddBtn;
        private Text teamGoodsValue;

        protected override void InitUIRefrence()
        {
            pageAnim = Transform.SafeGetComponent<Animation>();

            areaTitle = Transform.FindTransfrom("Content/LeftPanel/AreaName").SafeGetComponent<Text>();
            areaDesc= Transform.FindTransfrom("Content/LeftPanel/Desc").SafeGetComponent<Text>();
            progressSlider = Transform.FindTransfrom("Content/LeftPanel/Progress/Slider").SafeGetComponent<Slider>();
            progressText = Transform.FindTransfrom("Content/LeftPanel/Progress/Value").SafeGetComponent<Text>();

            missionDetailText = Transform.FindTransfrom("Content/RightPanel/DetailContent/Desc").SafeGetComponent<Text>();
            missionDetailBG = Transform.FindTransfrom("Content/RightPanel/DetailContent/AreaBG").SafeGetComponent<Image>();
            exploreTeamContentTrans = Transform.FindTransfrom("Content/RightPanel/TeamContent/Team");

            areaSelectTrans = Transform.FindTransfrom("Content/BottomPanel/AreaSelect");
            missionPanelTrans = Transform.FindTransfrom("Content/LeftPanel/Content");

            exploreMissionAnim = Transform.FindTransfrom("Content/LeftPanel").SafeGetComponent<Animation>();
            exploreAreaDetailAnim = Transform.FindTransfrom("Content/RightPanel").SafeGetComponent<Animation>();

            areaDescTypeEffect = areaDesc.transform.SafeGetComponent<TypeWriterEffect>();
            missionDetailTypeEffect = missionDetailText.transform.SafeGetComponent<TypeWriterEffect>();

            exploreBtn = Transform.FindTransfrom("Content/RightPanel/Confirm/Btn").SafeGetComponent<Button>();
            energyInputField = Transform.FindTransfrom("Content/RightPanel/TeamContent/Detail/Energy/InputField").SafeGetComponent<InputField>();
            energyReduceBtn= Transform.FindTransfrom("Content/RightPanel/TeamContent/Detail/Energy/InputField/Reduce").SafeGetComponent<Button>();
            energyAddBtn = Transform.FindTransfrom("Content/RightPanel/TeamContent/Detail/Energy/InputField/Add").SafeGetComponent<Button>();
            teamGoodsValue = Transform.FindTransfrom("Content/RightPanel/TeamContent/Detail/Goods/Value").SafeGetComponent<Text>();
        }

    }
}