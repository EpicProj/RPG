using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

namespace Sim_FrameWork.UI
{
    public class WareHousePageContext : WindowBase {

        public WareHousePage m_page;

        //当前选中的主页签

        public MaterialType currentSelectMainType;
        public MaterialSubType currentSelectSubType;

        private ToggleGroup filterToggleGroup;

        /// <summary>
        /// Detail Content
        /// </summary>
        private CanvasGroup detailCanvasGroup;

        public override void Awake(params object[] paralist)
        {
            m_page = GameObject.GetComponent<WareHousePage>();
            filterToggleGroup = UIUtility.SafeGetComponent<ToggleGroup>(m_page.TypeFilterContent.transform);
            detailCanvasGroup = UIUtility.SafeGetComponent<CanvasGroup>(m_page.DetailContent.transform);
            ResetDetailContent();
            AddBtnListener();
            InitMainTag();
            
        }


        public override void OnShow(params object[] paralist)
        {
            ResetDetailContent();
            InitSotrageItem();
        }

        public override bool OnMessage(UIMessage msg)
        {
            switch (msg.type)
            {
                case UIMsgType.UpdateWarehouseData:
                    MaterialStorageItem item = (MaterialStorageItem)msg.content[0];
                    UpdateStorageItem(item);
                    return true;
                case UIMsgType.WareHouse_Refresh_Detail:
                    return RefreshDetail((MaterialDataModel)msg.content[0]);
                case UIMsgType.WareHouse_Hide_Detail:
                    return ResetDetailContent();
            }
         
            return false;
        }

        public override void OnUpdate()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                UIManager.Instance.HideWnd(UIPath.WindowPath.WareHouse_Page);
            }
        }


        private void AddBtnListener()
        {
            AddButtonClickListener(m_page.CloseBtn, delegate ()
            {
                UIManager.Instance.HideWnd(UIPath.WindowPath.WareHouse_Page);
            });
        }


  


        /// <summary>
        /// 生成主标签
        /// </summary>
        public void InitMainTag()
        {
            var List = MaterialModule.MaterialTypeList;
            for(int i = 0; i < List.Count; i++)
            {
                var mainTag = ObjectManager.Instance.InstantiateObject(UIPath.WareHouse_Maintag_Prefab_Path);
                var cmpt = UIUtility.SafeGetComponent<WareHouseMainTagCmpt>(mainTag.transform);
                cmpt.InitTagElement(List[i]);
                cmpt.toggle.group = filterToggleGroup;
                mainTag.transform.SetParent(m_page.TypeFilterContent.transform,false);
            }

        }

        private void InitSotrageItem()
        {
            var allDic = PlayerManager.Instance.storageData.materialStorageDataDic;
            var loopList = UIUtility.SafeGetComponent<GridLoopList>(m_page.MaterialScrollView.transform);
            if (currentSelectMainType == null)
            {
                //Init All 
                var modelList = MaterialModule.Instance.InitMaterialStorageModel(allDic.Select(x => x.Value).ToList());
                loopList.InitData(modelList);
            }
            else
            {

            }
        }

        private void UpdateStorageItem(MaterialStorageItem item)
        {



        }

        /// <summary>
        /// 重置详情
        /// </summary>
        bool ResetDetailContent()
        {
            detailCanvasGroup.alpha = 0;
            return true;
        }

        bool RefreshDetail(MaterialDataModel ma)
        {
            m_page.DetailBG.sprite = ma.BG;
            m_page.DetailDesc.text = ma.Desc;
            m_page.DetailName.text = ma.Name;
            //Rarity TODO
            detailCanvasGroup.alpha = 1;
            return true;
        }
    }
}