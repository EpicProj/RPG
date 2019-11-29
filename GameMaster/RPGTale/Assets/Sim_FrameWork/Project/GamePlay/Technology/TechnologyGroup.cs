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

        public GroupType groupType;

        public int groupIndex=0;
        public TechObjectElement[] _techObjList;


       
        public void Awake()
        {
        }


        public bool InitGroup(int index)
        {
            groupIndex = index;
            var config = TechnologyModule.Instance.GetTechGroupConfig(index);
            if (config != null)
            {
                _techObjList = GetComponentsInChildren<TechObjectElement>();
                for(int i = 0; i < _techObjList.Length; i++)
                {
                    TechnologyDataModel model = new TechnologyDataModel();
                    if (!model.Create(config.techIDList[i]))
                    {
                        return false;
                    }
                    _techObjList[i].SetUpTech(model);
                }
            }
            return true;
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
        }

        public class GroupConfig
        {
            public int groupIndex;
            public string groupType;
            public string elementPath;
            public int posX;
            public int posY;
            public int ObjectNum;
            public List<int> techIDList;
        }
    }
}
