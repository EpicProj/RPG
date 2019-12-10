using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Sim_FrameWork.UI
{
    public class TechnologyMainPageContext : WindowBase
    {
        private TechnologyMainPage m_page;

        private Transform scrollViewContentTrans;

        private List<TechnologyGroup> _groupList;

        #region Override Method
        public override void Awake(params object[] paralist)
        {
            m_page = UIUtility.SafeGetComponent<TechnologyMainPage>(Transform);
            _groupList = new List<TechnologyGroup>();
            scrollViewContentTrans = UIUtility.FindTransfrom(m_page.TechScrollView, "Viewport/Content");
            AddBtnClick();
            InitTechGroup();
        }

        public override void OnShow(params object[] paralist)
        {
            AudioManager.Instance.PlaySound(AudioClipPath.UISound.Page_Open);
            RefreshAllTechGroup();
        }

        public override bool OnMessage(UIMessage msg)
        {
            switch (msg.type)
            {
                case UIMsgType.Tech_Research_Start:
                    ///研究开始
                    int techID = (int)msg.content[0];
                    return TechResearchStart(techID);

                case UIMsgType.Tech_Research_Finish:
                    ///研究结束
                    int techFinishID = (int)msg.content[0];
                    return RefreshTechGroupByTechID(techFinishID);

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
            Func<TechnologyGroup.GroupType, GameObject> getGroupObject = (o) =>
              {
                  switch (o)
                  {
                      case TechnologyGroup.GroupType.Panel_3_1_1:
                          return ObjectManager.Instance.InstantiateObject(UIPath.TechGroupPrefabPath(o));
                      default:
                          return null;
                  }
              };

            var groupList = TechnologyModule.Instance.config.InitGroupIndexList;
            for(int i = 0; i < groupList.Count; i++)
            {
                var config = TechnologyModule.Instance.GetTechGroupConfig(groupList[i]);
                if (config != null)
                {
                    var obj = getGroupObject(TechnologyModule.Instance.GetTechGroupType(config.groupIndex));
                    TechnologyGroup group = UIUtility.SafeGetComponent<TechnologyGroup>(obj.transform);
                    if (group != null)
                    {
                        if (!group.InitGroup(config.groupIndex))
                            break;
                        obj.transform.SetParent(scrollViewContentTrans, false);
                        Vector3 configPos = new Vector3(config.posX, config.posY, 0);
                        var rect = UIUtility.SafeGetComponent<RectTransform>(obj.transform);
                        rect.anchoredPosition = configPos;

                        if (!_groupList.Contains(group))
                        {
                            _groupList.Add(group);
                        }
                    }
                }
            }
        }

        private TechnologyGroup FindTechGroupByID(int index)
        {
            return _groupList.Find(x => x.groupIndex == index);
        }

        private bool TechResearchStart(int techID)
        {
            if (GlobalEventManager.Instance.GetTechInfo(techID) == null)
                return false;
            GlobalEventManager.Instance.OnTechResearchStart(techID);
            RefreshTechGroupByTechID(techID);
            return true;
        }

        /// <summary>
        /// 刷新所有科技状态
        /// </summary>
        private void RefreshAllTechGroup()
        {
            for(int i = 0; i < _groupList.Count; i++)
            {
                _groupList[i].RefreshGroup(true);
            }
        }

        private bool RefreshTechGroupByTechID(int techID)
        {
            int index = TechnologyModule.Instance.GetTechGroupIndex(techID);
            if (index != -1)
            {
                var group = FindTechGroupByID(index);
                if (group != null)
                {
                    group.RefreshGroup(false,techID);
                    return true;
                }
            }
            return false;
        }


    }
}