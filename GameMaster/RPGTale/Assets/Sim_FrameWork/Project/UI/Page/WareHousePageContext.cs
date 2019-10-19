using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sim_FrameWork.UI
{
    public class WareHousePageContext : WindowBase {

        public WareHousePage m_page;
        public PlayerData.WareHouseInfo info;
        public List<string> mainTagList;

        //当前选中的主页签
        public Dictionary<GameObject, MaterialConfig.MaterialType> currentMainTagDic = new Dictionary<GameObject, MaterialConfig.MaterialType>();
        public Dictionary<GameObject, MaterialConfig.MaterialType.MaterialSubType> currentSubTagDic = new Dictionary<GameObject, MaterialConfig.MaterialType.MaterialSubType>();


        public MaterialConfig.MaterialType currentSelectMainType;
        public MaterialConfig.MaterialType.MaterialSubType currentSelectSubType;
        public override void Awake(params object[] paralist)
        {
            info = (PlayerData.WareHouseInfo)paralist[0];
            m_page = GameObject.GetComponent<WareHousePage>();
            //AddBtnListener();
            //InitMainTag();
        }


        public override void OnShow(params object[] paralist)
        {
            info = (PlayerData.WareHouseInfo)paralist[0];
            //TODO
            //InitSotrageItem();
        }

        public override bool OnMessage(UIMessage msg)
        {
            switch (msg.type)
            {
                case UIMsgType.UpdateWarehouseData:
                    MaterialStorageItem item = (MaterialStorageItem)msg.content[0];
                    UpdateStorageItem(item);
                    return true;
            }
         
            return false;
        }

        public override void OnUpdate()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                UIManager.Instance.HideWnd(UIPath.WareHouse_Page);
            }
        }


        private void AddBtnListener()
        {
            AddButtonClickListener(m_page.CloseBtn, delegate ()
            {
                UIManager.Instance.HideWnd(UIPath.WareHouse_Page);
            });
        }


  


        /// <summary>
        /// 生成主标签
        /// </summary>
        public void InitMainTag()
        {
            for(int i = 0; i < info.materialTagList.Count; i++)
            {
                GameObject mainTag = ObjectManager.Instance.InstantiateObject(UIPath.WareHouse_Maintag_Prefab_Path);
                GeneralTagElement element = mainTag.GetComponent<GeneralTagElement>();
                //element.InitWareHouseMainTag(info.materialTagList[i], m_page.MainTagContent.transform.transform);
                //Add Btn
                AddButtonClickListener(element.btn, delegate () {
                    SelectMainTag(mainTag);
                });
                currentMainTagDic.Add(mainTag, info.materialTagList[i]);
            }
            //Set Default Select Tag
            currentSelectMainType = info.materialTagList[0];
            InitSubTag(currentSelectMainType);
        }

        /// <summary>
        /// 生成副标签
        /// </summary>
        /// <param name="type"></param>
        public void InitSubTag(MaterialConfig.MaterialType type)
        {
            List<MaterialConfig.MaterialType.MaterialSubType> subList = new List<MaterialConfig.MaterialType.MaterialSubType>();
            info.materialSubTagDic.TryGetValue(type, out subList);
            if (subList != null)
            {
                //foreach(Transform trans in m_page.SubTagContent.transform)
                //{
                //    GameObject.Destroy(trans.gameObject);
                //}
                currentSubTagDic.Clear();
                for (int i = 0; i < subList.Count; i++)
                {
                    GameObject subTag = ObjectManager.Instance.InstantiateObject(UIPath.WareHouse_Subtag_Prefab_Path);
                    GeneralTagElement element = subTag.GetComponent<GeneralTagElement>();
                    //element.InitWareHouseSubTag(subList[i], m_page.SubTagContent.transform);
                    AddButtonClickListener(element.btn, delegate ()
                    {
                        SelectSubTag(subTag);
                    });
                    currentSubTagDic.Add(subTag, subList[i]);
                }
                //Set Default Select Sub Tag
                if (subList.Count == 0)
                {
                    currentSelectSubType = new MaterialConfig.MaterialType.MaterialSubType
                    {
                        Type = "Total"
                    };
                }
                else
                {
                    currentSelectSubType = subList[0];
                }
                InitSotrageItem();
            }
        }

        public void SelectMainTag(GameObject obj)
        {
            //foreach (Transform trans in m_page.MainTagContent.transform)
            //{
            //    trans.GetComponent<GeneralTagElement>().DimObj();
            //}
            MaterialConfig.MaterialType type = null;
            currentMainTagDic.TryGetValue(obj, out type);
            if (type != null)
            {
                currentSelectMainType = type;
                //Init Select Info
                obj.GetComponent<GeneralTagElement>().HighlightObj();
                //Init Sub Tag
                InitSubTag(currentSelectMainType);
            }
        }

        public void SelectSubTag(GameObject obj)
        {
            //foreach(Transform trans in m_page.SubTagContent.transform)
            //{
            //    trans.GetComponent<GeneralTagElement>().DimObj();
            //}
            MaterialConfig.MaterialType.MaterialSubType type = null;
            currentSubTagDic.TryGetValue(obj, out type);
            if (type != null)
            {
                currentSelectSubType = type;
                obj.GetComponent<GeneralTagElement>().HighlightObj();
                InitSotrageItem();
            }
        }

        private void InitSotrageItem()
        {

        }

        private void UpdateStorageItem(MaterialStorageItem item)
        {
        }
    }
}