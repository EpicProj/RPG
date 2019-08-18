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
            fac.Comment = "";
            fac.FactoryName = "";
            fac.FactoryIcon = "";
            fac.FactoryDesc = "";
            fac.FactoryType = "";
            fac.FactoryTypeIndex = (ushort)i;
            fac.Level = (ushort)i;
            fac.Area = "";
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

        AllFactory_RawList = new List<Factory_Raw>();
        for(int i = 0; i < 2; i++)
        {
            Factory_Raw row = new Factory_Raw();
            row.RawID = i;
            row.RawType = (ushort)i;
            AllFactory_RawList.Add(row);
        }

        AllFactory_ManufactureList = new List<Factory_Manufacture>();
        for(int i = 0; i < 2; i++)
        {
            Factory_Manufacture manu = new Factory_Manufacture();
            manu.ManufactureID = i;
            manu.SpeedBase = i;
            manu.FormulaInfoID = i;
            manu.MaintenanceBase = "";
            manu.EnergyConsumptionBase = "";
            AllFactory_ManufactureList.Add(manu);
        }

        AllFactory_ScienceList = new List<Factory_Science>();
        for(int i = 0; i < 2; i++)
        {
            Factory_Science science = new Factory_Science();
            science.ScienceID = i;
            science.Comment = "";
            AllFactory_ScienceList.Add(science);
        }

        AllFactory_EnergyList = new List<Factory_Energy>();
        for(int i = 0; i < 2; i++)
        {
            Factory_Energy energy = new Factory_Energy();
            energy.EnergyID = i;
            energy.EnergyType = (ushort)i;
            AllFactory_EnergyList.Add(energy);
        }

        AllFactoryTypeDataList = new List<FactoryTypeData>();
        for(int i = 0; i < 2; i++)
        {
            FactoryTypeData type = new FactoryTypeData();
            type.Type = "";
            type.TypeName = "";
            type.TypeDesc = "";
            type.TypeIcon = "";
            AllFactoryTypeDataList.Add(type);
        }
    }
    public override void Init()
    {
        AllFactoryDic.Clear();
        AllTextData_FactoryDic.Clear();
        AllFactoryTypeDataDic.Clear();
        AllFactory_EnergyDic.Clear();
        AllFactory_ManufactureDic.Clear();
        AllFactory_RawDic.Clear();
        AllFactory_ScienceDic.Clear();

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
        foreach (var data in AllFactory_RawList)
        {
            if (AllFactory_RawDic.ContainsKey(data.RawID))
            {
                Debug.LogError("Find Same Row , Row ID = " + data.RawID);
            }
            else
            {
                AllFactory_RawDic.Add(data.RawID, data);
            }
        }
        foreach (var data in AllFactory_ManufactureList)
        {
            if (AllFactory_ManufactureDic.ContainsKey(data.ManufactureID))
            {
                Debug.LogError("Find Same ManufactureID , ManufactureID  = " + data.ManufactureID);
            }
            else
            {
                AllFactory_ManufactureDic.Add(data.ManufactureID, data);
            }
        }
        foreach (var data in AllFactory_ScienceList)
        {
            if (AllFactory_ScienceDic.ContainsKey(data.ScienceID))
            {
                Debug.LogError("Find Same ScienceID , ScienceID  = " + data.ScienceID);
            }
            else
            {
                AllFactory_ScienceDic.Add(data.ScienceID, data);
            }
        }
        foreach (var data in AllFactory_EnergyList)
        {
            if (AllFactory_EnergyDic.ContainsKey(data.EnergyID))
            {
                Debug.LogError("Find Same EnergyID , EnergyID  = " + data.EnergyID);
            }
            else
            {
                AllFactory_EnergyDic.Add(data.EnergyID, data);
            }
        }
        foreach (var data in AllFactoryTypeDataList)
        {
            if (AllFactoryTypeDataDic.ContainsKey(data.Type))
            {
                Debug.LogError("Find Same Type , Type  = " + data.Type);
            }
            else
            {
                AllFactoryTypeDataDic.Add(data.Type, data);
            }
        }

    }


    [XmlIgnore]
    public Dictionary<int, Factory> AllFactoryDic = new Dictionary<int, Factory>();
    [XmlIgnore]
    public Dictionary<string, TextData_Factory> AllTextData_FactoryDic = new Dictionary<string, TextData_Factory>();
    [XmlIgnore]
    public Dictionary<int, Factory_Raw> AllFactory_RawDic = new Dictionary<int, Factory_Raw>();
    [XmlIgnore]
    public Dictionary<int, Factory_Manufacture> AllFactory_ManufactureDic = new Dictionary<int, Factory_Manufacture>();
    [XmlIgnore]
    public Dictionary<int, Factory_Science> AllFactory_ScienceDic = new Dictionary<int, Factory_Science>();
    [XmlIgnore]
    public Dictionary<int, Factory_Energy> AllFactory_EnergyDic = new Dictionary<int, Factory_Energy>();
    [XmlIgnore]
    public Dictionary<string, FactoryTypeData> AllFactoryTypeDataDic = new Dictionary<string, FactoryTypeData>();

    [XmlElement]
    public List<Factory> AllFactoryList { get; set; }
    [XmlElement]
    public List<TextData_Factory> AllTextData_FactoryList { get; set; }
    [XmlElement]
    public List<Factory_Raw> AllFactory_RawList { get; set; }
    [XmlElement]
    public List<Factory_Manufacture> AllFactory_ManufactureList { get; set; }
    [XmlElement]
    public List<Factory_Science> AllFactory_ScienceList { get; set; }
    [XmlElement]
    public List<Factory_Energy> AllFactory_EnergyList { get; set; }
    [XmlElement]
    public List<FactoryTypeData> AllFactoryTypeDataList { get; set; }


}

[System.Serializable]
public class Factory
{
    [XmlAttribute]
    public int FactoryID { get; set; }
    [XmlAttribute]
    public string Comment { get; set; }
    [XmlAttribute]
    public string FactoryName { get; set; }
    [XmlAttribute]
    public string FactoryIcon { get; set; }
    [XmlAttribute]
    public string FactoryDesc { get; set; }
    [XmlAttribute]
    public string FactoryType { get; set; }
    [XmlAttribute]
    public ushort FactoryTypeIndex { get; set; }
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

[System.Serializable]
public class Factory_Raw
{
    //原料生产
    [XmlAttribute]
    public int RawID { get; set; }
    [XmlAttribute]
    public ushort RawType { get; set; }
}

[System.Serializable] 
public class Factory_Manufacture
{
    //制造
    [XmlAttribute]
    public int ManufactureID { get; set; }
    [XmlAttribute]
    public float SpeedBase { get; set; }
    [XmlAttribute]
    public int FormulaInfoID { get; set; }
    [XmlAttribute]
    //维护成本
    public string MaintenanceBase { get; set; }
    [XmlAttribute]
    public string EnergyConsumptionBase { get; set; }

}

[System.Serializable] 
public class Factory_Science
{
    //科研
    [XmlAttribute]
    public int ScienceID { get; set; }
    [XmlAttribute]
    public string Comment { get; set; }
}

[System.Serializable]
public class Factory_Energy
{
    //能源
    [XmlAttribute]
    public int EnergyID { get; set; }
    [XmlAttribute]
    public ushort EnergyType { get; set; }

}

[System.Serializable]
public class FactoryTypeData
{
    //类型数据
    [XmlAttribute]
    public string Type { get; set; }
    [XmlAttribute]
    public string TypeName { get; set; }
    [XmlAttribute]
    public string TypeDesc { get; set; }
    [XmlAttribute]
    public string TypeIcon { get; set; }
}