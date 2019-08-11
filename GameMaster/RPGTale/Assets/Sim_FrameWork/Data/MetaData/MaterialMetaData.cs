using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;

[System.Serializable]
public class MaterialMetaData : ExcelBase {

    public override void Construction()
    {
        AllMaterialList = new List<Material>();
        for(int i = 0; i < 2; i++)
        {
            Material mat = new Material();
            mat.MaterialID = i;
            mat.MaterialName = "";
            mat.MaterialDesc = "";
            mat.Type = (ushort)i;
            mat.TypeIndex = i;
            mat.MaterialIcon = "";
            mat.Rarity = (ushort)i;
            mat.ModelPath = "";
            AllMaterialList.Add(mat);
        }

        AllMaterial_ArtifactList = new List<Material_Artifact>();
        for(int i = 0; i < 2; i++)
        {
            Material_Artifact ma = new Material_Artifact();
            ma.ArtifactID = i;
            ma.ArtifactQuality = (ushort)i;
            ma.ProcessingTime = i;
            ma.ProcessingRawList = "";
            AllMaterial_ArtifactList.Add(ma);
        }

        AllMaterial_FluidList = new List<Material_Fluid>();
        for(int i = 0; i < 2; i++)
        {
            Material_Fluid mf = new Material_Fluid();
            mf.FluidID = i;
            mf.Comment = "";
            AllMaterial_FluidList.Add(mf);
        }

        AllTextMap_MaterialList = new List<TextMap_Material>();
        for(int i = 0; i < 2; i++)
        {
            TextMap_Material tm = new TextMap_Material();
            tm.TextID = "";
            tm.Value_CN = "";
            AllTextMap_MaterialList.Add(tm);
        }
    }

    public override void Init()
    {
        AllMaterialDic.Clear();
        AllMaterial_ArtifactDic.Clear();
        AllMaterial_FluidDic.Clear();
        AllTextMap_MaterialDic.Clear();

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
        foreach (var data in AllMaterial_ArtifactList)
        {
            if (AllMaterial_ArtifactDic.ContainsKey(data.ArtifactID))
            {
                Debug.LogError("Find Same ArtifactID , ArtifactID = " + data.ArtifactID);
            }
            else
            {
                AllMaterial_ArtifactDic.Add(data.ArtifactID, data);
            }
        }
        foreach (var data in AllMaterial_FluidList)
        {
            if (AllMaterial_FluidDic.ContainsKey(data.FluidID))
            {
                Debug.LogError("Find Same FluidID , FluidID= " + data.FluidID);
            }
            else
            {
                AllMaterial_FluidDic.Add(data.FluidID, data);
            }
        }
        foreach (var data in AllTextMap_MaterialList)
        {
            if (AllTextMap_MaterialDic.ContainsKey(data.TextID))
            {
                Debug.LogError("Find Same TextID , TextID= " + data.TextID);
            }
            else
            {
                AllTextMap_MaterialDic.Add(data.TextID, data);
            }
        }

    }



    [XmlIgnore]
    public Dictionary<int, Material> AllMaterialDic = new Dictionary<int, Material>();
    [XmlIgnore]
    public Dictionary<int, Material_Artifact> AllMaterial_ArtifactDic = new Dictionary<int, Material_Artifact>();
    [XmlIgnore]
    public Dictionary<int, Material_Fluid> AllMaterial_FluidDic = new Dictionary<int, Material_Fluid>();
    [XmlIgnore]
    public Dictionary<string, TextMap_Material> AllTextMap_MaterialDic = new Dictionary<string, TextMap_Material>();


    [XmlElement]
    public List<Material> AllMaterialList { get; set; }
    [XmlElement]
    public List<Material_Artifact> AllMaterial_ArtifactList { get; set; }
    [XmlElement]
    public List<Material_Fluid> AllMaterial_FluidList { get; set; }
    [XmlElement]
    public List<TextMap_Material> AllTextMap_MaterialList { get; set; }
}

[System.Serializable]
public class Material
{
    [XmlAttribute]
    public int MaterialID { get; set; }
    [XmlAttribute]
    public string MaterialName { get; set; }
    [XmlAttribute]
    public string MaterialDesc { get; set; }
    [XmlAttribute]
    public ushort Type { get; set; }
    [XmlAttribute]
    public int TypeIndex { get; set; }
    [XmlAttribute]
    public string MaterialIcon { get; set; }
    [XmlAttribute]
    public ushort Rarity { get; set; }
    [XmlAttribute]
    public string ModelPath { get; set; }
}

[System.Serializable] 
public class Material_Artifact
{
    [XmlAttribute]
    public int ArtifactID { get; set; }
    [XmlAttribute]
    public ushort ArtifactQuality { get; set; }
    [XmlAttribute]
    public float ProcessingTime { get; set; }
    [XmlAttribute]
    public string ProcessingRawList { get; set; }
}

[System.Serializable] 
public class Material_Fluid
{
    [XmlAttribute]
    public int FluidID { get; set; }
    [XmlAttribute]
    public string Comment { get; set; }
}

[System.Serializable]
public class TextMap_Material
{
    [XmlAttribute]
    public string TextID { get; set; }
    [XmlAttribute]
    public string Value_CN { get; set; }
}