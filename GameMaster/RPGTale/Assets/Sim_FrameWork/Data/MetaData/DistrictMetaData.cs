using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;

[System.Serializable]
public class DistrictMetaData : ExcelBase {

#if UNITY_EDITOR
    public override void Construction()
    {
        AllDistrictDataList = new List<DistrictData>();
        for(int i = 0; i < 2; i++)
        {
            DistrictData data = new DistrictData();
            data.DistrictID = i;
            data.DistrictName = "";
            data.DistrictDesc = "";
            data.DistrictIcon = "";
            data.ModelPath = "";
            data.Level =(ushort) i;
            data.CurrencyCost = i;
            data.MaterialCostList = "";
            data.TimeCost = i;
            data.Area = "";
            data.ParentList = "";
            data.EffectList = "";
            AllDistrictDataList.Add(data);
        }
    }


#endif

    public override void Init()
    {
        AllDistrictDataDic.Clear();
        foreach (var data in AllDistrictDataList)
        {
            if (AllDistrictDataDic.ContainsKey(data.DistrictID))
            {
                Debug.LogError("Find Same DistrictID , DistrictID  = " + data.DistrictID);
            }
            else
            {
                AllDistrictDataDic.Add(data.DistrictID, data);
            }
        }
    }

    [XmlIgnore]
    public Dictionary<int, DistrictData> AllDistrictDataDic = new Dictionary<int, DistrictData>();

    [XmlElement]
    public List<DistrictData> AllDistrictDataList { get; set; }
}


[System.Serializable]
public class DistrictData
{
    [XmlAttribute]
    public int DistrictID { get; set; }
    [XmlAttribute]
    public string DistrictName { get; set; }
    [XmlAttribute]
    public string DistrictDesc { get; set; }
    [XmlAttribute]
    public string DistrictIcon { get; set; }
    [XmlAttribute]
    public string ModelPath { get; set; }
    [XmlAttribute]
    public ushort Level { get; set; }
    [XmlAttribute]
    public int CurrencyCost { get; set; }
    [XmlAttribute]
    public string MaterialCostList { get; set; }
    [XmlAttribute]
    public int TimeCost { get; set; }
    [XmlAttribute]
    public string Area { get; set; }
    [XmlAttribute]
    public string ParentList { get; set; }
    [XmlAttribute]
    public string EffectList { get; set; }

}