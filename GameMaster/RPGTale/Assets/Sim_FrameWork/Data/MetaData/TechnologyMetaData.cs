using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;

[System.Serializable]
public class TechnologyMetaData : ExcelBase {

#if UNITY_EDITOR
    public override void Construction()
    {
        AllTechnologyList = new List<Technology>();
        for(int i = 0; i < 2; i++)
        {
            Technology te = new Technology();
            te.TechnologyID = i;
            te.TechType = "";
            te.TechGetType = "";
            te.TechName = "";
            te.TechDesc = "";
            te.TechIcon = "";
            te.Rarity = (ushort)i;
            te.TechCostMaterialList = "";
            te.Cost = (ushort)i;
            te.PreTechList = "";
            AllTechnologyList.Add(te);
        }

        AllTechnologyTypeDataList = new List<TechnologyTypeData>();
        for(int i = 0; i < 2; i++)
        {
            TechnologyTypeData tt = new TechnologyTypeData();
            tt.Type = "";
            tt.TypeName = "";
            tt.TypeDesc = "";
            tt.TypeIcon = "";
            AllTechnologyTypeDataList.Add(tt);
        }
    }

#endif
    public override void Init()
    {
        AllTechnologyDic.Clear();
        AllTechnologyTypeDataDic.Clear();

        foreach (var data in AllTechnologyList)
        {
            if (AllTechnologyDic.ContainsKey(data.TechnologyID))
            {
                Debug.LogError("Find Same TechnologyID , TechnologyID= " + data.TechnologyID);
            }
            else
            {
                AllTechnologyDic.Add(data.TechnologyID, data);
            }
        }

        foreach (var data in AllTechnologyTypeDataList)
        {
            if (AllTechnologyTypeDataDic.ContainsKey(data.Type))
            {
                Debug.LogError("Find Same Type , Type= " + data.Type);
            }
            else
            {
                AllTechnologyTypeDataDic.Add(data.Type, data);
            }
        }
    }


    [XmlIgnore]
    public Dictionary<int, Technology> AllTechnologyDic = new Dictionary<int, Technology>();
    [XmlIgnore]
    public Dictionary<string, TechnologyTypeData> AllTechnologyTypeDataDic = new Dictionary<string, TechnologyTypeData>();

    [XmlElement]
    public List<Technology> AllTechnologyList { get; set; }
    [XmlElement]
    public List<TechnologyTypeData> AllTechnologyTypeDataList { get; set; }
}

[System.Serializable] 
public class Technology
{
    [XmlAttribute]
    public int TechnologyID { get; set; }
    [XmlAttribute]
    public string TechType { get; set; }
    [XmlAttribute]
    public string TechGetType { get; set; }
    [XmlAttribute]
    public string TechName { get; set; }
    [XmlAttribute]
    public string TechDesc { get; set; }
    [XmlAttribute]
    public string TechIcon { get; set; }
    [XmlAttribute]
    public ushort Rarity { get; set; }
    [XmlAttribute]
    public string TechCostMaterialList { get; set; }
    [XmlAttribute]
    public ushort Cost { get; set; }
    [XmlAttribute]
    public string PreTechList { get; set; }
}

[System.Serializable] 
public class TechnologyTypeData
{
    [XmlAttribute]
    public string Type { get; set; }
    [XmlAttribute]
    public string TypeName { get; set; }
    [XmlAttribute]
    public string TypeDesc { get; set; }
    [XmlAttribute]
    public string TypeIcon { get; set; }
}