using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;

    [System.Serializable]
    public class BuildingPanelMetaData : ExcelBase
    {
#if UNITY_EDITOR
        public override void Construction()
        {
            AllBuildingPanelDataList = new List<BuildingPanelData>();
            for(int i = 0; i < 2; i++)
            {
                BuildingPanelData data = new BuildingPanelData();
                data.BuildID = i;
                data.FunctionBlockID = i;
                data.Desc = "";
                data.TimeCost = (ushort)i;
                data.CurrencyCost = (ushort)i;
                data.MaterialCost = "";
                data.UnLockParam = "";
                AllBuildingPanelDataList.Add(data);
            }
        }
#endif


        public override void Init()
        {
            AllBuildingPanelDataDic.Clear();
            foreach (var data in AllBuildingPanelDataList)
            {
                if (AllBuildingPanelDataDic.ContainsKey(data.BuildID))
                {
                    Debug.LogError("Find Same BuildID , BuildID  = " + data.BuildID);
                }
                else
                {
                    AllBuildingPanelDataDic.Add(data.BuildID, data);
                }
            }
        }

        [XmlIgnore]
        public Dictionary<int, BuildingPanelData> AllBuildingPanelDataDic = new Dictionary<int, BuildingPanelData>();

        [XmlElement]
        public List<BuildingPanelData> AllBuildingPanelDataList { get; set; }

    }

    [System.Serializable]
    public class BuildingPanelData
    {
        [XmlAttribute]
        public int BuildID { get; set; }
        [XmlAttribute]
        public int FunctionBlockID { get; set; }
        [XmlAttribute]
        public string Desc { get; set; }
        [XmlAttribute]
        public ushort TimeCost { get; set; }
        [XmlAttribute]
        public ushort CurrencyCost { get; set; }
        [XmlAttribute]
        public string MaterialCost { get; set; }
        [XmlAttribute]
        public string UnLockParam { get; set; }
    }