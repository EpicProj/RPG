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
            AllFunctionBlockList.Add(fac);
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
            AllFunctionBlockTypeDataList.Add(type);
        }
    }
#endif
    public override void Init()
    {
        AllFunctionBlockDic.Clear();
        AllFunctionBlockTypeDataDic.Clear();

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
    public Dictionary<string, FunctionBlockTypeData> AllFunctionBlockTypeDataDic = new Dictionary<string, FunctionBlockTypeData>();

    [XmlElement]
    public List<FunctionBlock> AllFunctionBlockList { get; set; }
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
    public string BlockConfig { get; set; }
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
}