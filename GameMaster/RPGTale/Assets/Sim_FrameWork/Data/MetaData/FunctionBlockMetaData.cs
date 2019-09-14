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
            fac.SubType = "";
            fac.FunctionBlockTypeIndex = (ushort)i;
            fac.MaxLevel = (ushort)i;
            fac.AreaDetailDefault = "";
            fac.OriginArea = "";
            fac.AreaMax = "";
            fac.EXPDataJsonIndex = "";
            fac.DistrictData = "";
            AllFunctionBlockList.Add(fac);
        }
        AllFunctionBlock_LaborList = new List<FunctionBlock_Labor>();
        for(int i = 0; i < 2; i++)
        {
            FunctionBlock_Labor labor = new FunctionBlock_Labor();
            labor.LaborID = i;
            labor.InherentLevel = "";
            labor.BasePopulation = i;
            labor.FoodConsumBase = i;
            labor.EnergyBase = i;
            AllFunctionBlock_LaborList.Add(labor);
        }

        AllFunctionBlock_RawList = new List<FunctionBlock_Raw>();
        for(int i = 0; i < 2; i++)
        {
            FunctionBlock_Raw row = new FunctionBlock_Raw();
            row.RawID = i;
            row.RawType = (ushort)i;
            row.InherentLevel = "";
            row.CollectMaterialList = "";
            AllFunctionBlock_RawList.Add(row);
        }

        AllFunctionBlock_IndustryList = new List<FunctionBlock_Industry>();
        for(int i = 0; i < 2; i++)
        {
            FunctionBlock_Industry manu = new FunctionBlock_Industry();
            manu.ID = i;
            manu.Type = "";
            manu.InherentLevel = "";
            manu.SpeedBase = i;
            manu.FormulaInfoID = i;
            manu.MaintenanceBase = "";
            manu.WorkerBase = i;
            manu.EnergyConsumptionBase = "";
            AllFunctionBlock_IndustryList.Add(manu);
        }

        AllFunctionBlock_ScienceList = new List<FunctionBlock_Science>();
        for(int i = 0; i < 2; i++)
        {
            FunctionBlock_Science science = new FunctionBlock_Science();
            science.ScienceID = i;
            science.InherentLevel = "";
    
            AllFunctionBlock_ScienceList.Add(science);
        }

        AllFunctionBlock_EnergyList = new List<FunctionBlock_Energy>();
        for(int i = 0; i < 2; i++)
        {
            FunctionBlock_Energy energy = new FunctionBlock_Energy();
            energy.EnergyID = i;
            energy.InherentLevel = "";
            energy.EnergyType = (ushort)i;
            AllFunctionBlock_EnergyList.Add(energy);
        }

        AllFunctionBlockTypeDataList = new List<FunctionBlockTypeData>();
        for(int i = 0; i < 2; i++)
        {
            FunctionBlockTypeData type = new FunctionBlockTypeData();
            type.Type = "";
            type.DefaultShow = true;
            type.TypeName = "";
            type.TypeDesc = "";
            type.TypeIcon = "";
            type.SubTypeList = "";
            AllFunctionBlockTypeDataList.Add(type);
        }
        AllFunctionBlockSubTypeDataList = new List<FunctionBlockSubTypeData>();
        for(int i = 0; i < 2; i++)
        {
            FunctionBlockSubTypeData type = new FunctionBlockSubTypeData();
            type.SubType = "";
            type.DefaultShow = true;
            type.TypeName = "";
            type.TypeDesc = "";
            type.TypeIcon = "";
            AllFunctionBlockSubTypeDataList.Add(type);
        }
    }
#endif
    public override void Init()
    {
        AllFunctionBlockDic.Clear();
        AllFunctionBlock_LaborDic.Clear();
        AllFunctionBlockTypeDataDic.Clear();
        AllFunctionBlock_EnergyDic.Clear();
        AllFunctionBlock_IndustryDic.Clear();
        AllFunctionBlock_RawDic.Clear();
        AllFunctionBlock_ScienceDic.Clear();
        AllFunctionBlockSubTypeDataDic.Clear();

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
        foreach (var data in AllFunctionBlock_LaborList)
        {
            if (AllFunctionBlock_LaborDic.ContainsKey(data.LaborID))
            {
                Debug.LogError("Find Same LaborID , LaborID  = " + data.LaborID);
            }
            else
            {
                AllFunctionBlock_LaborDic.Add(data.LaborID, data);
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
        foreach (var data in AllFunctionBlock_IndustryList)
        {
            if (AllFunctionBlock_IndustryDic.ContainsKey(data.ID))
            {
                Debug.LogError("Find Same IndustryID , IndustryID  = " + data.ID);
            }
            else
            {
                AllFunctionBlock_IndustryDic.Add(data.ID, data);
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
        foreach (var data in AllFunctionBlockSubTypeDataList)
        {
            if (AllFunctionBlockSubTypeDataDic.ContainsKey(data.SubType))
            {
                Debug.LogError("Find Same SubType , Type  = " + data.SubType);
            }
            else
            {
                AllFunctionBlockSubTypeDataDic.Add(data.SubType, data);
            }
        }

    }


    [XmlIgnore]
    public Dictionary<int, FunctionBlock> AllFunctionBlockDic = new Dictionary<int, FunctionBlock>();
    [XmlIgnore]
    public Dictionary<int, FunctionBlock_Labor> AllFunctionBlock_LaborDic = new Dictionary<int, FunctionBlock_Labor>();
    [XmlIgnore]
    public Dictionary<int, FunctionBlock_Raw> AllFunctionBlock_RawDic = new Dictionary<int, FunctionBlock_Raw>();
    [XmlIgnore]
    public Dictionary<int, FunctionBlock_Industry> AllFunctionBlock_IndustryDic = new Dictionary<int, FunctionBlock_Industry>();
    [XmlIgnore]
    public Dictionary<int, FunctionBlock_Science> AllFunctionBlock_ScienceDic = new Dictionary<int, FunctionBlock_Science>();
    [XmlIgnore]
    public Dictionary<int, FunctionBlock_Energy> AllFunctionBlock_EnergyDic = new Dictionary<int, FunctionBlock_Energy>();
    [XmlIgnore]
    public Dictionary<string, FunctionBlockTypeData> AllFunctionBlockTypeDataDic = new Dictionary<string, FunctionBlockTypeData>();
    [XmlIgnore]
    public Dictionary<string, FunctionBlockSubTypeData> AllFunctionBlockSubTypeDataDic = new Dictionary<string, FunctionBlockSubTypeData>();

    [XmlElement]
    public List<FunctionBlock> AllFunctionBlockList { get; set; }
    [XmlElement]
    public List<FunctionBlock_Labor> AllFunctionBlock_LaborList { get; set; }
    [XmlElement]
    public List<FunctionBlock_Raw> AllFunctionBlock_RawList { get; set; }
    [XmlElement]
    public List<FunctionBlock_Industry> AllFunctionBlock_IndustryList { get; set; }
    [XmlElement]
    public List<FunctionBlock_Science> AllFunctionBlock_ScienceList { get; set; }
    [XmlElement]
    public List<FunctionBlock_Energy> AllFunctionBlock_EnergyList { get; set; }
    [XmlElement]
    public List<FunctionBlockTypeData> AllFunctionBlockTypeDataList { get; set; }
    [XmlElement]
    public List<FunctionBlockSubTypeData> AllFunctionBlockSubTypeDataList { get; set; }


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
    public string SubType { get; set; }
    [XmlAttribute]
    public ushort FunctionBlockTypeIndex { get; set; }
    [XmlAttribute]
    public ushort MaxLevel { get; set; }
    [XmlAttribute]
    public string AreaDetailDefault { get; set; }
    [XmlAttribute]
    public string OriginArea { get; set; }
    [XmlAttribute]
    public string AreaMax { get; set; }
    [XmlAttribute]
    public string EXPDataJsonIndex { get; set; }
    [XmlAttribute]
    public string DistrictData { get; set; }


}

[System.Serializable]
public class FunctionBlock_Labor
{
    //劳动力
    [XmlAttribute]
    public int LaborID { get; set; }
    [XmlAttribute]
    public string InherentLevel { get; set; }
    [XmlAttribute]
    public float BasePopulation { get; set; }
    [XmlAttribute]
    public float FoodConsumBase { get; set; }
    [XmlAttribute]
    public float MaintainBase { get; set; }
    [XmlAttribute]
    public float EnergyBase { get; set; }
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

}

[System.Serializable] 
public class FunctionBlock_Industry
{
    //制造
    [XmlAttribute]
    public int ID { get; set; }
    [XmlAttribute]
    public string Type { get; set; }
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
    public float WorkerBase { get; set; }
    [XmlAttribute]
    public string EnergyConsumptionBase { get; set; }
}

[System.Serializable] 
public class FunctionBlock_Science
{
    //科研
    [XmlAttribute]
    public int ScienceID { get; set; }
    [XmlAttribute]
    public string InherentLevel { get; set; }
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

}

[System.Serializable]
public class FunctionBlockTypeData
{
    //类型数据
    [XmlAttribute]
    public string Type { get; set; }
    [XmlAttribute]
    public bool DefaultShow { get; set; }
    [XmlAttribute]
    public string TypeName { get; set; }
    [XmlAttribute]
    public string TypeDesc { get; set; }
    [XmlAttribute]
    public string TypeIcon { get; set; }
    [XmlAttribute]
    public string SubTypeList { get; set; }
}
[System.Serializable]
public class FunctionBlockSubTypeData
{
    [XmlAttribute]
    public string SubType { get; set; }
    [XmlAttribute]
    public bool DefaultShow { get; set; }
    [XmlAttribute]
    public string TypeName { get; set; }
    [XmlAttribute]
    public string TypeDesc { get; set; }
    [XmlAttribute]
    public string TypeIcon { get; set; }
}