using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;

[System.Serializable]
public class FactoryMetaData : ExcelBase {

    public override void Construction()
    {

        AllFactoryList = new List<Factory>();
        for(int i = 0; i < 2; i++)
        {
            Factory fac = new Factory();
            fac.FactoryID = i;
            fac.FactoryDesc = "";
            fac.FactoryName = "";
            fac.FactoryType = "";
            fac.Level = (ushort)i;
            fac.InputSlotNumBase = (ushort)i;
            fac.OutputSlotNumBase = (ushort)i;
            fac.PlugSlotNumBase = (ushort)i;
            fac.StaffSlotNumBase = (ushort)i;
            AllFactoryList.Add(fac);
        }

        AllTextData_FactoryList = new List<TextData_Factory>();
        for(int i = 0; i < 2; i++)
        {
            TextData_Factory text = new TextData_Factory();
            text.TextID = "";
            text.Value_CN = "";
            AllTextData_FactoryList.Add(text);
        }

    }
    public override void Init()
    {
        AllFactoryDic.Clear();
        AllTextData_FactoryDic.Clear();
        foreach(var data in AllFactoryList)
        {
            if (AllFactoryDic.ContainsKey(data.FactoryID))
            {
                Debug.LogError("Find Same FactoryID , Factory ID = " + data.FactoryID);
            }
            else
            {
                AllFactoryDic.Add(data.FactoryID, data);
            }
        }
        foreach (var data in AllTextData_FactoryList)
        {
            if (AllTextData_FactoryDic.ContainsKey(data.TextID))
            {
                Debug.LogError("Find Same FactoryID , Factory ID = " + data.TextID);
            }
            else
            {
                AllTextData_FactoryDic.Add(data.TextID, data);
            }
        }
    }


    [XmlIgnore]
    public Dictionary<int, Factory> AllFactoryDic = new Dictionary<int, Factory>();
    [XmlIgnore]
    public Dictionary<string, TextData_Factory> AllTextData_FactoryDic = new Dictionary<string, TextData_Factory>();

    [XmlElement]
    public List<Factory> AllFactoryList { get; set; }
    [XmlElement]
    public List<TextData_Factory> AllTextData_FactoryList { get; set; }


}

[System.Serializable]
public class Factory
{
    [XmlAttribute]
    public int FactoryID { get; set; }
    [XmlAttribute]
    public string FactoryName { get; set; }
    [XmlAttribute]
    public string FactoryDesc { get; set; }
    [XmlAttribute]
    public string FactoryType { get; set; }
    [XmlAttribute]
    public ushort Level { get; set; }
    [XmlAttribute]
    public string Area { get; set; }
    [XmlAttribute]
    public ushort InputSlotNumBase { get; set; }
    [XmlAttribute]
    public ushort OutputSlotNumBase { get; set; }
    [XmlAttribute]
    public ushort PlugSlotNumBase { get; set; }
    [XmlAttribute]
    public ushort StaffSlotNumBase { get; set; }

}

[System.Serializable] 
public class TextData_Factory
{
    [XmlAttribute]
    public string TextID { get; set; }
    [XmlAttribute]
    public string Value_CN { get; set; }

}