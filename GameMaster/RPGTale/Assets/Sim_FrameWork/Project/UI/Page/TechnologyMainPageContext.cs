using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork.UI
{
    public class TechnologyMainPageContext : WindowBase
    {
        private TechnologyMainPage m_page;

        private Transform scrollViewContentTrans;

        #region Override Method
        public override void Awake(params object[] paralist)
        {
            m_page = UIUtility.SafeGetComponent<TechnologyMainPage>(Transform);
            scrollViewContentTrans = UIUtility.FindTransfrom(m_page.TechScrollView, "Viewport/Content");
            AddBtnClick();
            InitTechGroup();
        }

        public override void OnShow(params object[] paralist)
        {
            AudioManager.Instance.PlaySound(AudioClipPath.UISound.Page_Open);
            RefreshTechGroup();
        }

        public override bool OnMessage(UIMessage msg)
        {
            switch (msg.type)
            {
                case UIMsgType.Tech_Research_Start:
                    int techID = (int)msg.content[0];
                    return TechResearchStart(techID);
                default:
                    return false;
            }
        }

        public override void OnClose()
        {
            AudioManager.Instance.PlaySound(AudioClipPath.UISound.Btn_Close);
        }

        #endregion

        private void AddBtnClick()
        {
            AddButtonClickListener(m_page.backBtn, () =>
            {
                UIManager.Instance.HideWnd(this);
            });
        }


        private void InitTechGroup()
        {
            var groupList = TechnologyModule.Instance.config.InitGroupIndexList;
            for(int i = 0; i < groupList.Count; i++)
            {
                var config = TechnologyModule.Instance.GetTechGroupConfig(groupList[i]);
                if (config != null)
                {
                    GameObject obj = ObjectManager.Instance.InstantiateObject(config.elementPath);
                    TechnologyGroup group = UIUtility.SafeGetComponent<TechnologyGroup>(obj.transform);
                    if (group != null)
                    {
                        if (!group.InitGroup(config.groupIndex))
                            break;
                        obj.transform.SetParent(scrollViewContentTrans, false);
                        Vector3 configPos = new Vector3(config.posX, config.posY, 0);
                        Debug.Log(configPos);
                        var rect = UIUtility.SafeGetComponent<RectTransform>(obj.transform);
                        rect.anchoredPosition = configPos;
                        
                    }
                }
            }
        }

        private bool TechResearchStart(int techID)
        {
            if (GlobalEventManager.Instance.GetTechInfo(techID) == null)
                return false;
            GlobalEventManager.Instance.OnTechResearchStart(techID);
            RefreshTechGroup();
            return true;
        }

        private void RefreshTechGroup()
        {
            var group= scrollViewContentTrans.gameObject.GetComponentsInChildren<TechnologyGroup>();
            for(int i = 0; i < group.Length; i++)
            {
                group[i].RefreshGroup();
            }
        }


    }
}