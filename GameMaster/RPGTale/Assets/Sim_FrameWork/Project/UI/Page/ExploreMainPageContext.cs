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
            UIGuide.Instance.ShowGameMainPage(true);
        }


        public void AddBtnClick()
        {
            AddButtonClickListener(m_page.backBtn, () =>
            {
                UIGuide.Instance.ShowGameMainPage(true);
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

            if (areaSelectTrans.childCount == list.Count)
                return;
            //TODO 要处理两方数量不相等问题

            for (int i = 0; i < list.Count; i++)
            {
                var exploreAreaBtn = ObjectManager.Instance.InstantiateObject(UIPath.PrefabPath.Explore_Area_Select_Btn);
                if (exploreAreaBtn != null)
                {
                    var cmpt = UIUtility.SafeGetComponent<ExploreAreaSelectBtn>(exploreAreaBtn.transform);
                    cmpt.InitAreaItem(list[i]);
                    cmpt.transform.SetParent(areaSelectTrans, false);
                }
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
                foreach (Transform trans in missionPanelTrans)
                {
                    trans.gameObject.SetActive(false);
                }
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
                    var element = UIUtility.SafeGetComponent<ExploreAreaMissionElement>(missionPanelTrans.GetChild(i));
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
            foreach (Transform trans in exploreTeamContentTrans)
            {
                trans.gameObject.SetActive(false);
            }

            for(int i = 0; i < item.maxTeamNum; i++)
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

        private ExploreMainPage m_page;

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
            m_page = UIUtility.SafeGetComponent<ExploreMainPage>(Transform);
            pageAnim = UIUtility.SafeGetComponent<Animation>(m_page.transform);
            areaTitle = UIUtility.SafeGetComponent<Text>(UIUtility.FindTransfrom(m_page.LeftPanel, "AreaName"));
            areaDesc= UIUtility.SafeGetComponent<Text>(UIUtility.FindTransfrom(m_page.LeftPanel, "Desc"));
            progressSlider = UIUtility.SafeGetComponent<Slider>(UIUtility.FindTransfrom(m_page.LeftPanel, "Progress/Slider"));
            progressText = UIUtility.SafeGetComponent<Text>(UIUtility.FindTransfrom(m_page.LeftPanel, "Progress/Value"));

            missionDetailText = UIUtility.SafeGetComponent<Text>(UIUtility.FindTransfrom(m_page.RightPanel, "DetailContent/Desc"));
            missionDetailBG = UIUtility.SafeGetComponent<Image>(UIUtility.FindTransfrom(m_page.RightPanel, "DetailContent/AreaBG"));
            exploreTeamContentTrans = UIUtility.FindTransfrom(m_page.RightPanel, "TeamContent/Team");

            areaSelectTrans = UIUtility.FindTransfrom(m_page.BottomPanel, "AreaSelect");
            missionPanelTrans = UIUtility.FindTransfrom(m_page.LeftPanel, "Content");

            exploreMissionAnim = UIUtility.SafeGetComponent<Animation>(m_page.LeftPanel);
            exploreAreaDetailAnim = UIUtility.SafeGetComponent<Animation>(m_page.RightPanel);

            areaDescTypeEffect = UIUtility.SafeGetComponent<TypeWriterEffect>(areaDesc.transform);
            missionDetailTypeEffect = UIUtility.SafeGetComponent<TypeWriterEffect>(missionDetailText.transform);

            exploreBtn = UIUtility.SafeGetComponent<Button>(UIUtility.FindTransfrom(m_page.RightPanel, "Confirm/Btn"));
            energyInputField = UIUtility.SafeGetComponent<InputField>(UIUtility.FindTransfrom(m_page.RightPanel, "TeamContent/Detail/Energy/InputField"));
            energyReduceBtn= UIUtility.SafeGetComponent<Button>(UIUtility.FindTransfrom(m_page.RightPanel, "TeamContent/Detail/Energy/InputField/Reduce"));
            energyAddBtn = UIUtility.SafeGetComponent<Button>(UIUtility.FindTransfrom(m_page.RightPanel, "TeamContent/Detail/Energy/InputField/Add"));
            teamGoodsValue = UIUtility.SafeGetComponent<Text>(UIUtility.FindTransfrom(m_page.RightPanel, "TeamContent/Detail/Goods/Value"));
        }

    }
}