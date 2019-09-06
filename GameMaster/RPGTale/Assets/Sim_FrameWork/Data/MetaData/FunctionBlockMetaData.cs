using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;

[System.Serializable]
public class FunctionBlockMetaData : ExcelBase {

#if UNITY_EDITOR
    public override void Construction()
    {

        AllFunctionBlockList = new List<FunctionBlock>();
        for(int i = 0; i < 2; i++)
        {
            FunctionBlock fac = new FunctionBlock();
            fac.FunctionBlockID = i;
            fac.BlockName = "";
            fac.BlockBG = "";
            fac.BlockIcon = "";
            fac.BlockDesc = "";
            fac.PreLevelBlock = i;
            fac.FunctionBlockType = "";
            fac.FunctionBlockTypeIndex = (ushort)i;
            fac.Level = (ushort)i;
            fac.EXPDataJsonIndex = "";
            fac.DistrictData = "";
            AllFunctionBlockList.Add(fac);
        }

        AllFunctionBlock_RawList = new List<FunctionBlock_Raw>();
        for(int i = 0; i < 2; i++)
        {
            FunctionBlock_Raw row = new FunctionBlock_Raw();
            row.RawID = i;
            row.RawType = (ushort)i;
            row.InherentLevel = "";
            row.CollectMaterialList = "";
            row.AreaDetailDefault = "";
            row.OriginArea = "";
            row.AreaMax = "";
            AllFunctionBlock_RawList.Add(row);
        }

        AllFunctionBlock_ManufactureList = new List<FunctionBlock_Manufacture>();
        for(int i = 0; i < 2; i++)
        {
            FunctionBlock_Manufacture manu = new FunctionBlock_Manufacture();
            manu.ManufactureID = i;
            manu.InherentLevel = "";
            manu.SpeedBase = i;
            manu.FormulaInfoID = i;
            manu.MaintenanceBase = "";
            manu.EnergyConsumptionBase = "";
            manu.AreaDetailDefault = "";
            manu.OriginArea = "";
            manu.AreaMax = "";
            AllFunctionBlock_ManufactureList.Add(manu);
        }

        AllFunctionBlock_ScienceList = new List<FunctionBlock_Science>();
        for(int i = 0; i < 2; i++)
        {
            FunctionBlock_Science science = new FunctionBlock_Science();
            science.ScienceID = i;
            science.InherentLevel = "";
            science.AreaDetailDefault = "";
            science.OriginArea = "";
            science.AreaMax = "";
            AllFunctionBlock_ScienceList.Add(science);
        }

        AllFunctionBlock_EnergyList = new List<FunctionBlock_Energy>();
        for(int i = 0; i < 2; i++)
        {
            FunctionBlock_Energy energy = new FunctionBlock_Energy();
            energy.EnergyID = i;
            energy.InherentLevel = "";
            energy.EnergyType = (ushort)i;
            energy.AreaDetailDefault = "";
            energy.OriginArea = "";
            energy.AreaMax = "";
            AllFunctionBlock_EnergyList.Add(energy);
        }

        AllFunctionBlockTypeDataList = new List<FunctionBlockTypeData>();
        for(int i = 0; i < 2; i++)
        {
            FunctionBlockTypeData type = new FunctionBlockTypeData();
            type.Type = "";
            type.TypeName = "";
            type.TypeDesc = "";
            type.TypeIcon = "";
            AllFunctionBlockTypeDataList.Add(type);
        }
    }
#endif
    public override void Init()
    {
        AllFunctionBlockDic.Clear();
        AllFunctionBlockTypeDataDic.Clear();
        AllFunctionBlock_EnergyDic.Clear();
        AllFunctionBlock_ManufactureDic.Clear();
        AllFunctionBlock_RawDic.Clear();
        AllFunctionBlock_ScienceDic.Clear();

        foreach(var data in AllFunctionBlockList)
        {
            if (AllFunctionBlockDic.ContainsKey(data.FunctionBlockID))
            {
                Debug.LogError("Find Same FunctionBlockID , FunctionBlock ID = " + data.FunctionBlockID);
            }
            else
            {
                AllFunctionBlockDic.Add(data.FunctionBlockID, data);
            }
        }
        foreach (var data in AllFunctionBlock_RawList)
        {
            if (AllFunctionBlock_RawDic.ContainsKey(data.RawID))
            {
                Debug.LogError("Find Same Row , Row ID = " + data.RawID);
            }
            else
            {
                AllFunctionBlock_RawDic.Add(data.RawID, data);
            }
        }
        foreach (var data in AllFunctionBlock_ManufactureList)
        {
            if (AllFunctionBlock_ManufactureDic.ContainsKey(data.ManufactureID))
            {
                Debug.LogError("Find Same ManufactureID , ManufactureID  = " + data.ManufactureID);
            }
            else
            {
                AllFunctionBlock_ManufactureDic.Add(data.ManufactureID, data);
            }
        }
        foreach (var data in AllFunctionBlock_ScienceList)
        {
            if (AllFunctionBlock_ScienceDic.ContainsKey(data.ScienceID))
            {
                Debug.LogError("Find Same ScienceID , ScienceID  = " + data.ScienceID);
            }
            else
            {
                AllFunctionBlock_ScienceDic.Add(data.ScienceID, data);
            }
        }
        foreach (var data in AllFunctionBlock_EnergyList)
        {
            if (AllFunctionBlock_EnergyDic.ContainsKey(data.EnergyID))
            {
                Debug.LogError("Find Same EnergyID , EnergyID  = " + data.EnergyID);
            }
            else
            {
                AllFunctionBlock_EnergyDic.Add(data.EnergyID, data);
            }
        }
        foreach (var data in AllFunctionBlockTypeDataList)
        {
            if (AllFunctionBlockTypeDataDic.ContainsKey(data.Type))
            {
                Debug.LogError("Find Same Type , Type  = " + data.Type);
            }
            else
            {
                AllFunctionBlockTypeDataDic.Add(data.Type, data);
            }
        }

    }


    [XmlIgnore]
    public Dictionary<int, FunctionBlock> AllFunctionBlockDic = new Dictionary<int, FunctionBlock>();
    [XmlIgnore]
    public Dictionary<int, FunctionBlock_Raw> AllFunctionBlock_RawDic = new Dictionary<int, FunctionBlock_Raw>();
    [XmlIgnore]
    public Dictionary<int, FunctionBlock_Manufacture> AllFunctionBlock_ManufactureDic = new Dictionary<int, FunctionBlock_Manufacture>();
    [XmlIgnore]
    public Dictionary<int, FunctionBlock_Science> AllFunctionBlock_ScienceDic = new Dictionary<int, FunctionBlock_Science>();
    [XmlIgnore]
    public Dictionary<int, FunctionBlock_Energy> AllFunctionBlock_EnergyDic = new Dictionary<int, FunctionBlock_Energy>();
    [XmlIgnore]
    public Dictionary<string, FunctionBlockTypeData> AllFunctionBlockTypeDataDic = new Dictionary<string, FunctionBlockTypeData>();

    [XmlElement]
    public List<FunctionBlock> AllFunctionBlockList { get; set; }
    [XmlElement]
    public List<FunctionBlock_Raw> AllFunctionBlock_RawList { get; set; }
    [XmlElement]
    public List<FunctionBlock_Manufacture> AllFunctionBlock_ManufactureList { get; set; }
    [XmlElement]
    public List<FunctionBlock_Science> AllFunctionBlock_ScienceList { get; set; }
    [XmlElement]
    public List<FunctionBlock_Energy> AllFunctionBlock_EnergyList { get; set; }
    [XmlElement]
    public List<FunctionBlockTypeData> AllFunctionBlockTypeDataList { get; set; }


}

[System.Serializable]
public class FunctionBlock
{
    [XmlAttribute]
    public int FunctionBlockID { get; set; }
    [XmlAttribute]
    public string BlockName { get; set; }
    [XmlAttribute]
    public string BlockBG { get; set; }
    [XmlAttribute]
    public string BlockIcon { get; set; }
    [XmlAttribute]
    public string BlockDesc { get; set; }
    [XmlAttribute]
    public int PreLevelBlock { get; set; }
    [XmlAttribute]
    public string FunctionBlockType { get; set; }
    [XmlAttribute]
    public ushort FunctionBlockTypeIndex { get; set; }
    [XmlAttribute]
    public ushort Level { get; set; }
    [XmlAttribute]
    public string EXPDataJsonIndex { get; set; }
    [XmlAttribute]
    public string DistrictData { get; set; }


}

[System.Serializable]
public class FunctionBlock_Raw 
{
    //原料生产
    [XmlAttribute]
    public int RawID { get; set; }
    [XmlAttribute]
    public ushort RawType { get; set; }
    [XmlAttribute]
    public string InherentLevel { get; set; }
    [XmlAttribute]
    public string CollectMaterialList { get; set; }
    [XmlAttribute]
    public string AreaDetailDefault { get; set; }
    [XmlAttribute]
    public string OriginArea { get; set; }
    [XmlAttribute]
    public string AreaMax { get; set; }
}

[System.Serializable] 
public class FunctionBlock_Manufacture
{
    //制造
    [XmlAttribute]
    public int ManufactureID { get; set; }
    [XmlAttribute]
    public string InherentLevel { get; set; }
    [XmlAttribute]
    public float SpeedBase { get; set; }
    [XmlAttribute]
    public int FormulaInfoID { get; set; }
    [XmlAttribute]
    //维护成本
    public string MaintenanceBase { get; set; }
    [XmlAttribute]
    public string EnergyConsumptionBase { get; set; }
    [XmlAttribute]
    public string AreaDetailDefault { get; set; }
    [XmlAttribute]
    public string OriginArea { get; set; }
    [XmlAttribute]
    public string AreaMax { get; set; }
}

[System.Serializable] 
public class FunctionBlock_Science
{
    //科研
    [XmlAttribute]
    public int ScienceID { get; set; }
    [XmlAttribute]
    public string InherentLevel { get; set; }
    [XmlAttribute]
    public string AreaDetailDefault { get; set; }
    [XmlAttribute]
    public string OriginArea { get; set; }
    [XmlAttribute]
    public string AreaMax { get; set; }
}

[System.Serializable]
public class FunctionBlock_Energy
{
    //能源
    [XmlAttribute]
    public int EnergyID { get; set; }
    [XmlAttribute]
    public string InherentLevel { get; set; }
    [XmlAttribute]
    public ushort EnergyType { get; set; }
    [XmlAttribute]
    public string AreaDetailDefault { get; set; }
    [XmlAttribute]
    public string OriginArea { get; set; }
    [XmlAttribute]
    public string AreaMax { get; set; }

}

[System.Serializable]
public class FunctionBlockTypeData
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