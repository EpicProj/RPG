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
        AllMaterialTypeList = new List<MaterialType>();
        for(int i = 0; i< 2; i++)
        {
            MaterialType type = new MaterialType();
            type.Type = "";
            type.DefaultShow = true;
            type.TypeName = "";
            type.TypeDesc = "";
            type.TypeIcon = "";
            type.SubTypeList = "";
            AllMaterialTypeList.Add(type);
        }
        AllMaterialSubTypeList = new List<MaterialSubType>();
        for (int i = 0; i < 2; i++)
        {
            MaterialSubType type = new MaterialSubType();
            type.SubType = "";
            type.DefaultShow = true;
            type.TypeName = "";
            type.TypeDesc = "";
            type.TypeIcon = "";
            AllMaterialSubTypeList.Add(type);
        }
    }

#endif
    public override void Init()
    {
        AllMaterialDic.Clear();
        AllMaterialTypeDic.Clear();
        AllMaterialSubTypeDic.Clear();

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
        foreach (var data in AllMaterialTypeList)
        {
            if (AllMaterialTypeDic.ContainsKey(data.Type))
            {
                Debug.LogError("Find Same MaterialType , Type= " + data.Type);
            }
            else
            {
                AllMaterialTypeDic.Add(data.Type, data);
            }
        }
        foreach (var data in AllMaterialSubTypeList)
        {
            if (AllMaterialSubTypeDic.ContainsKey(data.SubType))
            {
                Debug.LogError("Find Same MaterialSubType , SubType= " + data.SubType);
            }
            else
            {
                AllMaterialSubTypeDic.Add(data.SubType, data);
            }
        }
    }



    [XmlIgnore]
    public Dictionary<int, Material> AllMaterialDic = new Dictionary<int, Material>();
    [XmlIgnore]
    public Dictionary<string, MaterialType> AllMaterialTypeDic = new Dictionary<string, MaterialType>();
    [XmlIgnore]
    public Dictionary<string, MaterialSubType> AllMaterialSubTypeDic = new Dictionary<string, MaterialSubType>();

    [XmlElement]
    public List<Material> AllMaterialList { get; set; }
    [XmlElement]
    public List<MaterialType> AllMaterialTypeList { get; set; }
    [XmlElement]
    public List<MaterialSubType> AllMaterialSubTypeList { get; set; }
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

[System.Serializable]
public class MaterialType
{
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
public class MaterialSubType
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