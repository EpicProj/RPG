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
    }
#endif

    public override void Init()
    {
        AllTerrainDataDic.Clear();
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
    }

    [XmlIgnore]
    public Dictionary<int, TerrainData> AllTerrainDataDic = new Dictionary<int, TerrainData>();

    [XmlElement]
    public List<TerrainData> AllTerrainDataList { get; set; }
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