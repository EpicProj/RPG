using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;

[System.Serializable]
public class OrderMetaData : ExcelBase {

#if UNITY_EDITOR
    public override void Construction()
    {
        AllOrderDataList = new List<OrderData>();
        for(int i = 0; i < 2; i++)
        {
            OrderData data = new OrderData();
            data.OrderID = i;
            data.Name = "";
            data.Desc = "";
            data.BGPath = "";
            data.Type = "";
            data.TimeLimit = i;
            data.OrderContent = "";
            data.Reward = i;
            data.Rarity = (ushort)i;
            data.AppearConfig = "";
            data.DisAppearConfig = "";
            AllOrderDataList.Add(data);
        }

        AllOrderTypeDataList = new List<OrderTypeData>();
        for(int i = 0; i < 2; i++)
        {
            OrderTypeData data = new OrderTypeData();
            data.TypeID = "";
            data.Name = "";
            data.Desc = "";
            data.IconPath = "";
            AllOrderTypeDataList.Add(data);
        }
    }

#endif

    public override void Init()
    {
        AllOrderDataDic.Clear();
        AllOrderTypeDataDic.Clear();

        foreach (var data in AllOrderDataList)
        {
            if (AllOrderDataDic.ContainsKey(data.OrderID))
            {
                Debug.LogError("Find Same OrderID , OrderID= " + data.OrderID);
            }
            else
            {
                AllOrderDataDic.Add(data.OrderID, data);
            }
        }
        foreach (var data in AllOrderTypeDataList)
        {
            if (AllOrderTypeDataDic.ContainsKey(data.TypeID))
            {
                Debug.LogError("Find Same Order Type ID , TypeID= " + data.TypeID);
            }
            else
            {
                AllOrderTypeDataDic.Add(data.TypeID, data);
            }
        }
    }

    [XmlIgnore]
    public Dictionary<int, OrderData> AllOrderDataDic = new Dictionary<int, OrderData>();
    [XmlIgnore]
    public Dictionary<string, OrderTypeData> AllOrderTypeDataDic = new Dictionary<string, OrderTypeData>();

    [XmlElement]
    public List<OrderData> AllOrderDataList { get; set; }
    [XmlElement]
    public List<OrderTypeData> AllOrderTypeDataList { get; set; }


}


[System.Serializable] 
public class OrderData
{
    [XmlAttribute]
    public int OrderID { get; set; }
    [XmlAttribute]
    public string Name { get; set; }
    [XmlAttribute]
    public string Desc { get; set; }
    [XmlAttribute]
    public string BGPath { get; set; }
    [XmlAttribute]
    public string Type { get; set; }
    [XmlAttribute]
    public float TimeLimit { get; set; }
    [XmlAttribute]
    public string OrderContent { get; set; }
    [XmlAttribute]
    public int Reward { get; set; }
    [XmlAttribute]
    public ushort Rarity { get; set; }
    [XmlAttribute]
    public string AppearConfig { get; set; }
    [XmlAttribute]
    public string DisAppearConfig { get; set; }
}

[System.Serializable]
public class OrderTypeData
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