using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sim_FrameWork.UI
{
    public partial class ExploreMainPageContext : WindowBase
    {

        #region Override Method
        public override void Awake(params object[] paralist)
        {
            base.Awake(paralist);

        }

        public override bool OnMessage(UIMessage msg)
        {
            if(msg.type== UIMsgType.ExplorePage_ShowArea_Mission)
            {
                ExploreAreaData data = (ExploreAreaData)msg.content[0];
                return ShowAreaMissionPanel(data);
            }
            return false;
        }

        public override void OnShow(params object[] paralist)
        {
            AudioManager.Instance.PlaySound(AudioClipPath.UISound.Page_Open);
            InitAreaExploreList(ExploreAreaType.space);

            if (pageAnim != null)
                pageAnim.Play();

        }

        public override void OnClose()
        {
            UIGuide.Instance.ShowGameMainPage(true);
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
                RefreshAreaExploreProgress(data);



                return true;
            }
            return false;
        }



    }



    public partial class ExploreMainPageContext : WindowBase
    {

        private ExploreMainPage m_page;

        private Text areaTitle;
        private Text areaDesc;

        private Slider progressSlider;
        private Text progressText;

        private Transform areaSelectTrans;

        private Animation exploreMissionAnim;
        private Animation pageAnim;


        protected override void InitUIRefrence()
        {
            m_page = UIUtility.SafeGetComponent<ExploreMainPage>(Transform);
            pageAnim = UIUtility.SafeGetComponent<Animation>(m_page.transform);
            areaTitle = UIUtility.SafeGetComponent<Text>(UIUtility.FindTransfrom(m_page.LeftPanel, "AreaName"));
            areaDesc= UIUtility.SafeGetComponent<Text>(UIUtility.FindTransfrom(m_page.LeftPanel, "Desc"));
            progressSlider = UIUtility.SafeGetComponent<Slider>(UIUtility.FindTransfrom(m_page.LeftPanel, "Progress/Slider"));
            progressText = UIUtility.SafeGetComponent<Text>(UIUtility.FindTransfrom(m_page.LeftPanel, "Progress/Value"));

            areaSelectTrans = UIUtility.FindTransfrom(m_page.BottomPanel, "AreaSelect");

            exploreMissionAnim = UIUtility.SafeGetComponent<Animation>(m_page.LeftPanel);
        }

    }
}