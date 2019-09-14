using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;

[System.Serializable]
public class TerrainMetaData : ExcelBase {

#if UNITY_EDITOR
    public override void Construction()
    {
        AllTerrainDataList = new List<TerrainData>();
        for (int i = 0; i < 2; i++)
        {
            TerrainData data = new TerrainData();
            data.TerrainID = i;
            data.TerrainName = "";
            data.TerrainDesc = "";
            data.TerrainIcon = "";
            data.TerrainModelPath = "";
            data.TerrainType = "";
            data.CanBuild = true;

            AllTerrainDataList.Add(data);
        }
        AllBiotaDataList = new List<BiotaData>();
        for(int i = 0; i < 2; i++)
        {
            BiotaData data = new BiotaData();
            data.BiotaID = i;
            data.BiotaName = "";
            data.BiotaDesc = "";
            data.BiotaIcon = "";
            data.Type = "";
            data.AvgElevation = "";
            data.RainFall = "";
            data.Temperature = "";
            AllBiotaDataList.Add(data);
        }
    }
#endif

    public override void Init()
    {
        AllTerrainDataDic.Clear();
        AllBiotaDataDic.Clear();
        foreach (var data in AllTerrainDataList)
        {
            if (AllTerrainDataDic.ContainsKey(data.TerrainID))
            {
                Debug.LogError("Find Same TerrainID , TerrainID  = " + data.TerrainID);
            }
            else
            {
                AllTerrainDataDic.Add(data.TerrainID, data);
            }
        }
        foreach (var data in AllBiotaDataList)
        {
            if (AllBiotaDataDic.ContainsKey(data.BiotaID))
            {
                Debug.LogError("Find Same BiotaID , BiotaID  = " + data.BiotaID);
            }
            else
            {
                AllBiotaDataDic.Add(data.BiotaID, data);
            }
        }

    }

    [XmlIgnore]
    public Dictionary<int, TerrainData> AllTerrainDataDic = new Dictionary<int, TerrainData>();
    [XmlIgnore]
    public Dictionary<int, BiotaData> AllBiotaDataDic = new Dictionary<int, BiotaData>();

    [XmlElement]
    public List<TerrainData> AllTerrainDataList { get; set; }
    [XmlElement]
    public List<BiotaData> AllBiotaDataList { get; set; }
}

[System.Serializable] 
public class TerrainData
{
    [XmlAttribute]
    public int TerrainID { get; set; }
    [XmlAttribute]
    public string TerrainName { get; set; }
    [XmlAttribute]
    public string TerrainDesc { get; set; }
    [XmlAttribute]
    public string TerrainIcon { get; set; }
    [XmlAttribute]
    public string TerrainModelPath { get; set; }
    [XmlAttribute]
    public string TerrainType { get; set; }
    [XmlAttribute]
    public bool CanBuild { get; set; }
}
[System.Serializable]
public class BiotaData
{
    //生物群系
    [XmlAttribute]
    public int BiotaID { get; set; }
    [XmlAttribute]
    public string BiotaName { get; set; }
    [XmlAttribute]
    public string BiotaDesc { get; set; }
    [XmlAttribute]
    public string BiotaIcon { get; set; }
    [XmlAttribute]
    public string Type { get; set; }
    [XmlAttribute]
    public string AvgElevation { get; set; }
    [XmlAttribute]
    public string RainFall { get; set; }
    [XmlAttribute]
    public string Temperature { get; set; }
}