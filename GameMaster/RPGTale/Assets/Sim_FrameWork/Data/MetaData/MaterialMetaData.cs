using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;

[System.Serializable]
public class MaterialMetaData : ExcelBase {

#if UNITY_EDITOR
    public override void Construction()
    {
        AllMaterialList = new List<Material>();
        for(int i = 0; i < 2; i++)
        {
            Material mat = new Material();
            mat.MaterialID = i;
            mat.MaterialName = "";
            mat.NameEn = "";
            mat.MaterialDesc = "";
            mat.Type = "";
            mat.SubType = "";
            mat.BlockCapacity = (ushort)i;
            mat.PriceBase = i;
            mat.MaterialIcon = "";
            mat.BG = "";
            mat.UnitName = "";
            mat.Rarity = "";
            mat.ModelPath = "";
            AllMaterialList.Add(mat);
        }

    }

#endif
    public override void Init()
    {
        AllMaterialDic.Clear();

        foreach (var data in AllMaterialList)
        {
            if (AllMaterialDic.ContainsKey(data.MaterialID))
            {
                Debug.LogError("Find Same MaterialID , MaterialID= " + data.MaterialID);
            }
            else
            {
                AllMaterialDic.Add(data.MaterialID, data);
            }
        }
    }



    [XmlIgnore]
    public Dictionary<int, Material> AllMaterialDic = new Dictionary<int, Material>();


    [XmlElement]
    public List<Material> AllMaterialList { get; set; }
}

[System.Serializable]
public class Material
{
    [XmlAttribute]
    public int MaterialID { get; set; }
    [XmlAttribute]
    public string MaterialName { get; set; }
    [XmlAttribute]
    public string NameEn { get; set; }
    [XmlAttribute]
    public string MaterialDesc { get; set; }
    [XmlAttribute]
    public string Type { get; set; }
    [XmlAttribute]
    public string SubType { get; set; }
    [XmlAttribute]
    public ushort BlockCapacity { get; set; }
    [XmlAttribute]
    public int PriceBase { get; set; }
    [XmlAttribute]
    public string MaterialIcon { get; set; }
    [XmlAttribute]
    public string BG { get; set; }
    [XmlAttribute]
    public string UnitName { get; set; }
    [XmlAttribute]
    public string Rarity { get; set; }
    [XmlAttribute]
    public string ModelPath { get; set; }
}

