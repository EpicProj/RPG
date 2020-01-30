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
            data.Type = i;
            data.Level =(ushort) i;
            data.CurrencyCost = i;
            data.MaterialCostList = "";
            data.TimeCost = i;
            data.EffectList = "";
            AllDistrictDataList.Add(data);
        }

        AllDistrictTypeList = new List<DistrictType>();
        for(int i = 0; i < 2; i++)
        {
            DistrictType type = new DistrictType();
            type.TypeID = i;
            type.TypeShape = "";
            type.Icon = "";
            type.ModelPath = "";
            AllDistrictTypeList.Add(type);
        }

    }


#endif

    public override void Init()
    {
        AllDistrictDataDic.Clear();
        AllDistrictTypeDic.Clear();
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
        foreach (var data in AllDistrictTypeList)
        {
            if (AllDistrictTypeDic.ContainsKey(data.TypeID))
            {
                Debug.LogError("Find Same TypeID , TypeID  = " + data.TypeID);
            }
            else
            {
                AllDistrictTypeDic.Add(data.TypeID, data);
            }
        }

    }

    [XmlIgnore]
    public Dictionary<int, DistrictData> AllDistrictDataDic = new Dictionary<int, DistrictData>();
    [XmlIgnore]
    public Dictionary<int, DistrictType> AllDistrictTypeDic = new Dictionary<int, DistrictType>();

    [XmlElement]
    public List<DistrictData> AllDistrictDataList { get; set; }
    [XmlElement]
    public List<DistrictType> AllDistrictTypeList { get; set; }
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
    public int Type { get; set; }
    [XmlAttribute]
    public ushort Level { get; set; }
    [XmlAttribute]
    public int CurrencyCost { get; set; }
    [XmlAttribute]
    public string MaterialCostList { get; set; }
    [XmlAttribute]
    public int TimeCost { get; set; }
    [XmlAttribute]
    public string EffectList { get; set; }

}

[System.Serializable]
public class DistrictType
{
    [XmlAttribute]
    public int TypeID { get; set; }
    [XmlAttribute]
    public string TypeShape { get; set; }
    [XmlAttribute]
    public string Icon { get; set; }
    [XmlAttribute]
    public string ModelPath { get; set; }
}


