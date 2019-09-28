using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;

[System.Serializable]
public class OrganizationMetaData : ExcelBase {
#if UNITY_EDITOR
    public override void Construction()
    {
        AllOrganizationDataList = new List<OrganizationData>();
        for(int i = 0; i < 2; i++)
        {
            OrganizationData data = new OrganizationData();
            data.ID = i;
            data.Name = "";
            data.Name_En = "";
            data.AreaType = "";
            data.Icon = "";
            data.IconBig = "";
            data.ScaleDesc = "";
            data.EconomyDesc = "";
            data.BGDescBrief = "";
            AllOrganizationDataList.Add(data);
        }
        AllOrganizationTypeDataList = new List<OrganizationTypeData>();
        for(int i = 0; i < 2; i++)
        {
            OrganizationTypeData data = new OrganizationTypeData();
            data.TypeID = "";
            data.Name = "";
            data.Desc = "";
            data.IconPath = "";
            AllOrganizationTypeDataList.Add(data);
        }
        AllOrganizationEventDataList = new List<OrganizationEventData>();
        for(int i = 0; i < 2; i++)
        {
            OrganizationEventData data = new OrganizationEventData();
            data.EventID = i;
            data.EventTitle = "";
            data.EventDesc = "";
            data.EventTime = "";
            data.EventBG = "";
            data.Effects = "";
            AllOrganizationEventDataList.Add(data);
        }
    }
#endif


    public override void Init()
    {
        AllOrganizationDataDic.Clear();
        AllOrganizationEventDataDic.Clear();

        foreach (var data in AllOrganizationDataList)
        {
            if (AllOrganizationDataDic.ContainsKey(data.ID))
            {
                Debug.LogError("Find Same OrganizationData ID , ID= " + data.ID);
            }
            else
            {
                AllOrganizationDataDic.Add(data.ID, data);
            }
        }
        foreach (var data in AllOrganizationTypeDataList)
        {
            if (AllOrganizationTypeDataDic.ContainsKey(data.TypeID))
            {
                Debug.LogError("Find Same Organization TypeID  , ID= " + data.TypeID);
            }
            else
            {
                AllOrganizationTypeDataDic.Add(data.TypeID, data);
            }
        }
        foreach (var data in AllOrganizationEventDataList)
        {
            if (AllOrganizationEventDataDic.ContainsKey(data.EventID))
            {
                Debug.LogError("Find Same Organization EventID  , ID= " + data.EventID);
            }
            else
            {
                AllOrganizationEventDataDic.Add(data.EventID, data);
            }
        }
    }


    [XmlIgnore]
    public Dictionary<int, OrganizationData> AllOrganizationDataDic = new Dictionary<int, OrganizationData>();
    [XmlIgnore]
    public Dictionary<int, OrganizationEventData> AllOrganizationEventDataDic = new Dictionary<int, OrganizationEventData>();
    [XmlIgnore]
    public Dictionary<string, OrganizationTypeData> AllOrganizationTypeDataDic = new Dictionary<string, OrganizationTypeData>();

    [XmlElement]
    public List<OrganizationData> AllOrganizationDataList { get; set; }
    [XmlElement]
    public List<OrganizationEventData> AllOrganizationEventDataList { get; set; }
    [XmlElement]
    public List<OrganizationTypeData> AllOrganizationTypeDataList { get; set; }




}
[System.Serializable]
public class OrganizationData
{
    [XmlAttribute]
    public int ID { get; set; }
    [XmlAttribute]
    public string Name { get; set; }
    [XmlAttribute]
    public string Name_En { get; set; }
    [XmlAttribute]
    public string AreaType { get; set; }
    [XmlAttribute]
    public string Icon { get; set; }
    [XmlAttribute]
    public string IconBig { get; set; }
    [XmlAttribute]
    public string ScaleDesc { get; set; }
    [XmlAttribute]
    public string EconomyDesc { get; set; }
    [XmlAttribute]
    public string BGDescBrief { get; set; }

}

[System.Serializable]
public class OrganizationTypeData
{
    [XmlAttribute]
    public string TypeID { get; set; }
    [XmlAttribute]
    public string Name { get; set; }
    [XmlAttribute]
    public string Desc { get; set; }
    [XmlAttribute]
    public string IconPath { get; set; }
}

[System.Serializable]
public class OrganizationEventData
{
    [XmlAttribute]
    public int EventID { get; set; }
    [XmlAttribute]
    public string EventTitle { get; set; }
    [XmlAttribute]
    public string EventDesc { get; set; }
    [XmlAttribute]
    public string EventTime { get; set; }
    [XmlAttribute]
    public string EventBG { get; set; }
    [XmlAttribute]
    public string Effects { get; set; }
}
