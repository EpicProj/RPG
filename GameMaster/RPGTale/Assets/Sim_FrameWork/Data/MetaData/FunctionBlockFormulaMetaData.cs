using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;

[System.Serializable]
public class FunctionBlockFormulaMetaData : ExcelBase
{

#if UNITY_EDITOR
    public override void Construction()
    {
        AllFormulaDataList = new List<FormulaData>();
        for (int i = 0; i < 2; i++)
        {
            FormulaData data = new FormulaData();
            data.FormulaID = i;
            data.FormulaName = "";
            data.FormulaDesc = "";
            data.ProductSpeed = i;
            data.EXP = (ushort)i;
            data.InputMaterialList = "";
            data.OutputMaterial = "";
            data.EnhanceMaterial = "";
            AllFormulaDataList.Add(data);
        }
        AllFormulaInfoList = new List<FormulaInfo>();
        for(int i = 0; i < 2; i++)
        {
            FormulaInfo info = new FormulaInfo();
            info.InfoID = i;
            info.InfoType = (ushort)i;
            info.FormulaList = "";
            AllFormulaInfoList.Add(info);
        }
    }
#endif
    public override void Init()
    {
        AllFormulaDataDic.Clear();
        AllFormulaInfoDic.Clear();

        foreach (var data in AllFormulaDataList)
        {
            if (AllFormulaDataDic.ContainsKey(data.FormulaID))
            {
                Debug.LogError("Find Same FormulaID , FormulaID  = " + data.FormulaID);
            }
            else
            {
                AllFormulaDataDic.Add(data.FormulaID, data);
            }
        }
        foreach (var data in AllFormulaInfoList)
        {
            if (AllFormulaInfoDic.ContainsKey(data.InfoID))
            {
                Debug.LogError("Find Same InfoID , InfoID  = " + data.InfoID);
            }
            else
            {
                AllFormulaInfoDic.Add(data.InfoID, data);
            }
        }

    }

    [XmlIgnore]
    public Dictionary<int, FormulaData> AllFormulaDataDic = new Dictionary<int, FormulaData>();
    [XmlIgnore]
    public Dictionary<int, FormulaInfo> AllFormulaInfoDic = new Dictionary<int, FormulaInfo>();

    [XmlElement]
    public List<FormulaData> AllFormulaDataList { get; set; }
    [XmlElement]
    public List<FormulaInfo> AllFormulaInfoList { get; set; }
}

[System.Serializable]
public class FormulaData
{
    [XmlElement]
    public int FormulaID { get; set; }
    [XmlElement]
    public string FormulaName { get; set; }
    [XmlElement]
    public string FormulaDesc { get; set; }
    [XmlElement]
    public float ProductSpeed { get; set; }
    [XmlElement]
    public ushort EXP { get; set; }
    [XmlElement]
    public string InputMaterialList { get; set; }
    [XmlElement]
    public string OutputMaterial { get; set; }
    [XmlElement]
    public string EnhanceMaterial { get; set; }

}
[System.Serializable]
public class FormulaInfo
{
    [XmlElement]
    public int InfoID { get; set; }
    [XmlElement]
    public ushort InfoType { get; set; }
    [XmlElement]
    public string FormulaList { get; set; }
}