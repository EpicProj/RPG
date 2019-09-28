using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sim_FrameWork.UI
{
    public class WareHouseDialogContent : WindowBase {

        public WareHouseDialog m_dialog;
        public PlayerData.WareHouseInfo info;
        public List<string> mainTagList;

        //当前选中的主页签
        public Dictionary<GameObject, MaterialConfig.MaterialType> currentMainTagDic = new Dictionary<GameObject, MaterialConfig.MaterialType>();
        public Dictionary<GameObject, MaterialConfig.MaterialType.MaterialSubType> currentSubTagDic = new Dictionary<GameObject, MaterialConfig.MaterialType.MaterialSubType>();


        public override void Awake(params object[] paralist)
        {
            info = (PlayerData.WareHouseInfo)paralist[0];
            m_dialog = GameObject.GetComponent<WareHouseDialog>();
            AddBtnListener();
            InitMainTag();
        }


        public override void OnShow(params object[] paralist)
        {
            info = (PlayerData.WareHouseInfo)paralist[0];
            //TODO
            InitSotrageItem();
        }

        public override bool OnMessage(UIMessage msg)
        {
            switch (msg.type)
            {
                case UIMsgType.UpdateWarehouseData:
                    MaterialStorageData data = (MaterialStorageData)msg.content[0];
                    UpdateStorageItem(data);
                    return true;
            }
         
            return false;
        }

        public override void OnUpdate()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                UIManager.Instance.HideWnd(UIPath.WAREHOURSE_DIALOG);
            }
        }


        private void AddBtnListener()
        {
            AddButtonClickListener(m_dialog.CloseBtn, delegate ()
            {
                UIManager.Instance.HideWnd(UIPath.WAREHOURSE_DIALOG);
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
                element.InitWareHouseMainTag(info.materialTagList[i], m_dialog.MainTagContent.transform.transform);
                //Add Btn
                AddButtonClickListener(element.btn, delegate () {
                    SelectMainTag(mainTag);
                });
                currentMainTagDic.Add(mainTag, info.materialTagList[i]);
            }
            //Set Default Select Tag
            info.currentSelectMainType = info.materialTagList[0];
            InitSubTag(info.currentSelectMainType);
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
                foreach(Transform trans in m_dialog.SubTagContent.transform)
                {
                    GameObject.Destroy(trans.gameObject);
                }
                currentSubTagDic.Clear();
                for (int i = 0; i < subList.Count; i++)
                {
                    GameObject subTag = ObjectManager.Instance.InstantiateObject(UIPath.WareHouse_Subtag_Prefab_Path);
                    GeneralTagElement element = subTag.GetComponent<GeneralTagElement>();
                    element.InitWareHouseSubTag(subList[i], m_dialog.SubTagContent.transform);
                    AddButtonClickListener(element.btn, delegate ()
                    {
                        SelectSubTag(subTag);
                    });
                    currentSubTagDic.Add(subTag, subList[i]);
                }
                //Set Default Select Sub Tag
                if (subList.Count == 0)
                {
                    info.currentSelectSubType = new MaterialConfig.MaterialType.MaterialSubType
                    {
                        Type = "Total"
                    };
                }
                else
                {
                    info.currentSelectSubType = subList[0];
                }
                InitSotrageItem();
            }
        }

        public void SelectMainTag(GameObject obj)
        {
            foreach (Transform trans in m_dialog.MainTagContent.transform)
            {
                trans.GetComponent<GeneralTagElement>().DimObj();
            }
            MaterialConfig.MaterialType type = null;
            currentMainTagDic.TryGetValue(obj, out type);
            if (type != null)
            {
                info.currentSelectMainType = type;
                //Init Select Info
                obj.GetComponent<GeneralTagElement>().HighlightObj();
                //Init Sub Tag
                InitSubTag(info.currentSelectMainType);
            }
        }

        public void SelectSubTag(GameObject obj)
        {
            foreach(Transform trans in m_dialog.SubTagContent.transform)
            {
                trans.GetComponent<GeneralTagElement>().DimObj();
            }
            MaterialConfig.MaterialType.MaterialSubType type = null;
            currentSubTagDic.TryGetValue(obj, out type);
            if (type != null)
            {
                info.currentSelectSubType = type;
                obj.GetComponent<GeneralTagElement>().HighlightObj();
                InitSotrageItem();
            }
        }

        private void InitSotrageItem()
        {
            WarehouseSlotPanel panel = m_dialog.MaterialContent.GetComponent<WarehouseSlotPanel>();
            panel.InitMaterialContent(info);
        }

        private void UpdateStorageItem(MaterialStorageData data)
        {
            WarehouseSlotPanel panel = m_dialog.MaterialContent.GetComponent<WarehouseSlotPanel>();
            panel.AddMaterial(data);
        }
    }
}