using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;

[System.Serializable]
public class CampMetaData : ExcelBase
{
#if UNITY_EDITOR
    public override void Construction()
    {
        AllCampDataList = new List<CampData>();
        for (int i = 0; i < 2; i++)
        {
            CampData data = new CampData();
            data.CampID = i;
            data.CampName = "";
            data.CampDesc = "";
            data.CampIcon = "";
            AllCampDataList.Add(data);
        }
        AllCampCreedDataList = new List<CampCreedData>();
        for (int i = 0; i < 2; i++)
        {
            CampCreedData data = new CampCreedData();
            data.CreedDesc = "";
            data.CreedID = i;
            AllCampCreedDataList.Add(data);
        }
        AllCampAttributeDataList = new List<CampAttributeData>();
        for (int i = 0; i < 2; i++)
        {
            CampAttributeData data = new CampAttributeData();
            data.AttributeID = i;
            AllCampAttributeDataList.Add(data);
        }
    }
#endif

    public override void Init()
    {
        AllCampDataDic.Clear();
        AllCampCreedDataDic.Clear();
        AllCampAttributeDataDic.Clear();

        foreach (var data in AllCampDataList)
        {
            if (AllCampDataDic.ContainsKey(data.CampID))
            {
                Debug.LogError("Find Same CampID , CampID  = " + data.CampID);
            }
            else
            {
                AllCampDataDic.Add(data.CampID, data);
            }
        }
        foreach (var data in AllCampCreedDataList)
        {
            if (AllCampCreedDataDic.ContainsKey(data.CreedID))
            {
                Debug.LogError("Find Same CampCreedID , CampCreedID  = " + data.CreedID);
            }
            else
            {
                AllCampCreedDataDic.Add(data.CreedID, data);
            }
        }
        foreach (var data in AllCampAttributeDataList)
        {
            if (AllCampAttributeDataDic.ContainsKey(data.AttributeID))
            {
                Debug.LogError("Find Same CampAttributeID , CampAttributeID  = " + data.AttributeID);
            }
            else
            {
                AllCampAttributeDataDic.Add(data.AttributeID, data);
            }
        }
    }


    [XmlIgnore]
    public Dictionary<int, CampData> AllCampDataDic = new Dictionary<int, CampData>();
    [XmlIgnore]
    public Dictionary<int, CampCreedData> AllCampCreedDataDic = new Dictionary<int, CampCreedData>();
    [XmlIgnore]
    public Dictionary<int, CampAttributeData> AllCampAttributeDataDic = new Dictionary<int, CampAttributeData>();

    [XmlElement]
    public List<CampData> AllCampDataList { get; set; }
    [XmlElement]
    public List<CampCreedData> AllCampCreedDataList { get; set; }
    [XmlElement]
    public List<CampAttributeData> AllCampAttributeDataList { get; set; }
}


[System.Serializable]
public class CampData
{
    [XmlAttribute]
    public int CampID { get; set; }
    [XmlAttribute]
    public ushort CampType { get; set; }
    [XmlAttribute]
    public string CampName { get; set; }
    [XmlAttribute]
    public string CampDesc { get; set; }
    [XmlAttribute]
    public string CampIcon { get; set; }
    [XmlAttribute]
    public string CampBGSmall { get; set; }
    [XmlAttribute]
    public ushort HardLevel { get; set; }
    [XmlAttribute]
    public ushort DefaultHardLevel { get; set; }
    [XmlAttribute]
    public string AttributeIDList { get; set; }
    [XmlAttribute]
    public int CreedID { get; set; }
    [XmlAttribute]
    public string LeaderPresetList { get; set; }
}

[System.Serializable]
public class CampCreedData
{
    [XmlAttribute]
    public int CreedID { get; set; }
    [XmlAttribute]
    public string CreedName { get; set; }
    [XmlAttribute]
    public string CreedDesc { get; set; }
    [XmlAttribute]
    public string CreedIcon { get; set; }
}

[System.Serializable]
public class CampAttributeData
{
    [XmlAttribute]
    public int AttributeID { get; set; }
    [XmlAttribute]
    public string Name { get; set; }
    [XmlAttribute]
    public string Desc { get; set; }
    [XmlAttribute]
    public string Icon { get; set; }
}
