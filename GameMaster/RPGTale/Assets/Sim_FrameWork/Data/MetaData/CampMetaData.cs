using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;

[System.Serializable]
public class CampMetaData : ExcelBase {

#if UNITY_EDITOR
    public override void Construction()
    {
        AllCampDataList = new List<CampData>();
        for(int i = 0; i < 2; i++)
        {
            CampData data = new CampData();
            data.CampID = i;
            data.CampName = "";
            data.CampDesc = "";
            data.IconCircle = "";
            data.ValueMin = i;
            data.ValueMax = i;
            AllCampDataList.Add(data);
        }
        AllCampLevelDataList = new List<CampLevelData>();
        for(int i = 0; i < 2; i++)
        {
            CampLevelData data = new CampLevelData();
            data.LevelID = i;
            data.CampID = i;
            data.LevelName = "";
            data.Justice = i;
            AllCampLevelDataList.Add(data);
        }
    }

#endif

    public override void Init()
    {
        AllCampDataDic.Clear();
        AllCampLevelDataDic.Clear();
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
        foreach (var data in AllCampLevelDataList)
        {
            if (AllCampLevelDataDic.ContainsKey(data.LevelID))
            {
                Debug.LogError("Find Same LevelID , LevelID  = " + data.LevelID);
            }
            else
            {
                AllCampLevelDataDic.Add(data.LevelID, data);
            }
        }
    }

    [XmlIgnore]
    public Dictionary<int, CampData> AllCampDataDic = new Dictionary<int, CampData>();
    [XmlIgnore]
    public Dictionary<int, CampLevelData> AllCampLevelDataDic = new Dictionary<int, CampLevelData>();

    [XmlElement]
    public List<CampData> AllCampDataList { get; set; }
    [XmlElement]
    public List<CampLevelData> AllCampLevelDataList { get; set; }
}

[System.Serializable]
public class CampData
{
    [XmlAttribute]
    public int CampID { get; set; }
    [XmlAttribute]
    public string CampName { get; set; }
    [XmlAttribute]
    public string CampDesc { get; set; }
    [XmlAttribute]
    public string IconCircle { get; set; }
    [XmlAttribute]
    public float ValueMin { get; set; }
    [XmlAttribute]
    public float ValueMax { get; set; }
}
[System.Serializable]
public class CampLevelData
{
    [XmlAttribute]
    public int LevelID { get; set; }
    [XmlAttribute]
    public int CampID { get; set; }
    [XmlAttribute]
    public string LevelName { get; set; }
    [XmlAttribute]
    public float Justice { get; set; }
}
