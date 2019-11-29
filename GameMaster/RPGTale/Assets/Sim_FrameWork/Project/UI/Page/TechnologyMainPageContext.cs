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

            InitTechGroup();
        }

        public override void OnShow(params object[] paralist)
        {
            
        }

        public override bool OnMessage(UIMessage msg)
        {
            return base.OnMessage(msg);
        }
        #endregion

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


    }
}