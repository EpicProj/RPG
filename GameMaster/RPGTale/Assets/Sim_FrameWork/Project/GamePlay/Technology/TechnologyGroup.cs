using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork
{
    public class TechnologyGroup : MonoBehaviour
    {
        public enum GroupType
        {
            Panel_3_1_1,
        }

        public enum TechElementType
        {
            Simple,
            Complex
        }

        public GroupType groupType;

        public int groupIndex=0;
        public List<TechObjectElement> _techObjList;


        public void Awake()
        {
        }


        public bool InitGroup(int index)
        {
            groupIndex = index;
            var config = TechnologyModule.Instance.GetTechGroupConfig(index);
            if (config != null)
            {
                for(int i = 0; i < config.ObjectNum; i++)
                {
                    var element = config.techElementList[i];
                    GameObject obj = ObjectManager.Instance.InstantiateObject(GetElementTypePrefabPath(element));
                    obj.name = "TechObject" + element.Index.ToString();
                    ///Set Pos
                    Vector3 newPos = new Vector3(element.posX, element.posY,0);
                    Debug.Log(newPos);
                    obj.transform.SetParent(transform, false);
                    var rect = UIUtility.SafeGetComponent<RectTransform>(obj.transform).anchoredPosition = newPos;


                    TechObjectElement objEle = UIUtility.SafeGetComponent<TechObjectElement>(obj.transform);
                    if (objEle != null)
                    {
                        _techObjList.Add(objEle);
                        TechnologyDataModel model = new TechnologyDataModel();
                        if (!model.Create(element.TechID))
                        {
                            return false;
                        }
                        _techObjList[i].SetUpTech(model);
                    }

                }
            }
            return true;
        }

        public void RefreshGroup(bool total, int techID = -1)
        {
            if(total && techID == -1)
            {
                for (int i = 0; i < _techObjList.Count; i++)
                {
                    _techObjList[i].RefreshTech();
                }
            }
            else if(total ==false && techID != -1)
            {
                var element = GetTechObj(techID);
                if (element != null)
                {
                    element.RefreshTech();
                }
            }
        }

        private TechObjectElement GetTechObj(int techID)
        {
            return _techObjList.Find(x => x._dataModel.ID == techID);
        }

        private TechElementType GetTechElementType(TechGroupConfig.TechElement element)
        {
            if (element.ElementType == 1)
            {
                return TechElementType.Simple;
            }else if (element.ElementType == 2)
            {
                return TechElementType.Complex;
            }
            Debug.LogError("Parse Tech ElementType Error!  Index=" + element.Index);
            return TechElementType.Simple;
        }
        private string GetElementTypePrefabPath(TechGroupConfig.TechElement element)
        {
            var type = GetTechElementType(element);
            switch (type)
            {
                case TechElementType.Simple:
                    return UIPath.PrefabPath.Tech_Element_Simple;
                default:
                    return null;
            }
        }

    }

    public class TechGroupConfig
    {
        public List<int> InitGroupIndexList;
        public List<GroupConfig> configList;


        public void LoadData()
        {
            Config.JsonReader reader = new Config.JsonReader();
            TechGroupConfig config = reader.LoadJsonDataConfig<TechGroupConfig>(Config.JsonConfigPath.TechGroupConfigJsonPath);
            InitGroupIndexList = config.InitGroupIndexList;
            configList = config.configList;

            for(int i = 0; i < configList.Count; i++)
            {
                if (configList[i].ObjectNum != configList[i].techElementList.Count)
                {
                    Debug.LogError("Parse TechGroupConfig Error! Num not Matching!  groupIndex=" + config.configList[i].groupIndex);
                }
            }
            if(InitGroupIndexList.Count!= configList.Count)
            {
                Debug.LogError("Parse TechGroupConfig Error! Num not Matching!  InitGroupIndexListCount="+InitGroupIndexList.Count);
            }

        }

        public class GroupConfig
        {
            public int groupIndex;
            public string groupType;
            public string elementPath;
            public int posX;
            public int posY;
            public int ObjectNum;
            public List<TechElement> techElementList;

        }

        public class TechElement
        {
            public int Index;
            public int TechID;
            public int posX;
            public int posY;
            public ushort ElementType;
        }
    }
}
