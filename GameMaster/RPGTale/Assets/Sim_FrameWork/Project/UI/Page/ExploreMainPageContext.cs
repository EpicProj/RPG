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
        }

        public override void OnClose()
        {
            base.OnClose();
        }

        #endregion

        void InitAreaExploreList(ExploreAreaType type)
        {
            List<ExploreAreaData> list = new List<ExploreAreaData>();
            if(type == ExploreAreaType.space)
            {
                list = GlobalEventManager.Instance.ExploreAreaListSpace;
            }else if(type == ExploreAreaType.earth)
            {
                list = GlobalEventManager.Instance.ExploreAreaListEarth;
            }

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
            if (data == null)
                return false;
            if (exploreMissionAnim != null)
            {
                exploreMissionAnim.Play();
            }
            return true;
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


        protected override void InitUIRefrence()
        {
            m_page = UIUtility.SafeGetComponent<ExploreMainPage>(Transform);
            areaTitle = UIUtility.SafeGetComponent<Text>(UIUtility.FindTransfrom(m_page.LeftPanel, "AreaName"));
            areaDesc= UIUtility.SafeGetComponent<Text>(UIUtility.FindTransfrom(m_page.LeftPanel, "Desc"));
            progressSlider = UIUtility.SafeGetComponent<Slider>(UIUtility.FindTransfrom(m_page.LeftPanel, "Progress/Slider"));
            progressText = UIUtility.SafeGetComponent<Text>(UIUtility.FindTransfrom(m_page.LeftPanel, "Progress/Value"));

            areaSelectTrans = UIUtility.FindTransfrom(m_page.BottomPanel, "AreaSelect");

            exploreMissionAnim = UIUtility.SafeGetComponent<Animation>(m_page.LeftPanel);
        }

    }
}