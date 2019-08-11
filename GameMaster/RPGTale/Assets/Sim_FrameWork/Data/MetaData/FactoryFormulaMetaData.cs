﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;

[System.Serializable]
public class FactoryFormulaMetaData : ExcelBase
{

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
            data.InputMaterialList = "";
            data.OutputMaterialList = "";
            data.ByProductList = "";
            AllFormulaDataList.Add(data);
        }

        AllTextMap_FormulaList = new List<TextMap_Formula>();
        for (int i = 0; i < 2; i++)
        {
            TextMap_Formula tf = new TextMap_Formula();
            tf.TextID = "";
            tf.Value_CN = "";
            AllTextMap_FormulaList.Add(tf);
        }

    }
    public override void Init()
    {
        AllFormulaDataDic.Clear();
        AllTextMap_FormulaDic.Clear();

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
        foreach (var data in AllTextMap_FormulaList)
        {
            if (AllTextMap_FormulaDic.ContainsKey(data.TextID))
            {
                Debug.LogError("Find Same TextID , TextID  = " + data.TextID);
            }
            else
            {
                AllTextMap_FormulaDic.Add(data.TextID, data);
            }
        }

    }

    [XmlIgnore]
    public Dictionary<int, FormulaData> AllFormulaDataDic = new Dictionary<int, FormulaData>();
    [XmlIgnore]
    public Dictionary<string, TextMap_Formula> AllTextMap_FormulaDic = new Dictionary<string, TextMap_Formula>();

    [XmlElement]
    public List<FormulaData> AllFormulaDataList { get; set; }
    [XmlElement]
    public List<TextMap_Formula> AllTextMap_FormulaList { get; set; }
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
    public string InputMaterialList { get; set; }
    [XmlElement]
    public string OutputMaterialList { get; set; }
    [XmlElement]
    public string ByProductList { get; set; }

}

[System.Serializable] 
public class TextMap_Formula
{
    [XmlElement]
    public string TextID { get; set; }
    [XmlElement]
    public string Value_CN { get; set; }


}