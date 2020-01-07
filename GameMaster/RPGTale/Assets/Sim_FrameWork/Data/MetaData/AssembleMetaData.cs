using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;

[System.Serializable]
public class AssembleMetaData : ExcelBase {

#if UNITY_EDITOR
    public override void Construction()
    {
        AllAssembleWarshipList = new List<AssembleWarship>();
        for(int i = 0; i < 2; i++)
        {
            AssembleWarship ship = new AssembleWarship();
            ship.WarShipID = i;
            ship.Type = (ushort)i;
            ship.Class = i;
            ship.ShipScale = (ushort)i;
            ship.MaterialCost = "";
            ship.BaseTimeCost = (ushort)i;
            ship.HPBase = i;
            ship.SpeedBase = i;
            ship.FirePowerBase = i;
            ship.DetectBase = i;
            ship.CrewMax = (ushort)i;
            ship.StorageBase = (ushort)i;
            AllAssembleWarshipList.Add(ship);
        }
    }

#endif

    public override void Init()
    {
        AllAssembleWarshipDic.Clear();
        AllAssemblePartsTypeDic.Clear();
        AllAssembleWarshipClassDic.Clear();
        AllAssemblePartsDic.Clear();
        AllAssemblePartsTypeDic.Clear();

        foreach (var data in AllAssembleWarshipList)
        {
            if (AllAssembleWarshipDic.ContainsKey(data.WarShipID))
            {
                Debug.LogError("Find Same WarShipID , WarShipID  = " + data.WarShipID);
            }
            else
            {
                AllAssembleWarshipDic.Add(data.WarShipID, data);
            }
        }
        foreach (var data in AllAssembleWarShipTypeList)
        {
            if (AllAssembleWarShipTypeDic.ContainsKey(data.TypeID))
            {
                Debug.LogError("Find Same WarShipTypeID , TypeID  = " + data.TypeID);
            }
            else
            {
                AllAssembleWarShipTypeDic.Add(data.TypeID, data);
            }
        }
        foreach (var data in AllAssembleWarshipClassList)
        {
            if (AllAssembleWarshipClassDic.ContainsKey(data.ClassID))
            {
                Debug.LogError("Find Same WarshipClass , WarshipClass  = " + data.ClassID);
            }
            else
            {
                AllAssembleWarshipClassDic.Add(data.ClassID, data);
            }
        }
        foreach (var data in AllAssemblePartsList)
        {
            if (AllAssemblePartsDic.ContainsKey(data.PartID))
            {
                Debug.LogError("Find Same PartID , PartID  = " + data.PartID);
            }
            else
            {
                AllAssemblePartsDic.Add(data.PartID, data);
            }
        }
        foreach (var data in AllAssemblePartsTypeList)
        {
            if (AllAssemblePartsTypeDic.ContainsKey(data.ModelTypeID))
            {
                Debug.LogError("Find Same ModelTypeID , ModelTypeID  = " + data.ModelTypeID);
            }
            else
            {
                AllAssemblePartsTypeDic.Add(data.ModelTypeID, data);
            }
        }
    }

    [XmlIgnore]
    public Dictionary<int, AssembleWarship> AllAssembleWarshipDic = new Dictionary<int, AssembleWarship>();
    [XmlIgnore]
    public Dictionary<int, AssembleWarShipType> AllAssembleWarShipTypeDic = new Dictionary<int, AssembleWarShipType>();
    [XmlIgnore]
    public Dictionary<int, AssembleWarshipClass> AllAssembleWarshipClassDic = new Dictionary<int, AssembleWarshipClass>();
    [XmlIgnore]
    public Dictionary<int, AssembleParts> AllAssemblePartsDic = new Dictionary<int, AssembleParts>();
    [XmlIgnore]
    public Dictionary<int, AssemblePartsType> AllAssemblePartsTypeDic = new Dictionary<int, AssemblePartsType>();

    [XmlElement]
    public List<AssembleWarship> AllAssembleWarshipList { get; set; }
    [XmlElement]
    public List<AssembleWarShipType> AllAssembleWarShipTypeList { get; set; }
    [XmlElement]
    public List<AssembleWarshipClass> AllAssembleWarshipClassList { get; set; }
    [XmlElement]
    public List<AssembleParts> AllAssemblePartsList { get; set; }
    [XmlElement]
    public List<AssemblePartsType> AllAssemblePartsTypeList { get; set; }

}

[System.Serializable]
public class AssembleWarship
{
    [XmlAttribute]
    public int WarShipID { get; set; }
    [XmlAttribute]
    public ushort Type { get; set; }
    [XmlAttribute]
    public string MainType { get; set; }
    [XmlAttribute]
    public int Class { get; set; }
    [XmlAttribute]
    public ushort ShipScale { get; set; }
    [XmlAttribute]
    public string MaterialCost { get; set; }
    [XmlAttribute]
    public ushort BaseTimeCost { get; set; }
    [XmlAttribute]
    public int HPBase { get; set; }
    [XmlAttribute]
    public float SpeedBase { get; set; }
    [XmlAttribute]
    public float FirePowerBase { get; set; }
    [XmlAttribute]
    public float DetectBase { get; set; }
    [XmlAttribute]
    public ushort CrewMax { get; set; }
    [XmlAttribute]
    public ushort StorageBase { get; set; }
    [XmlAttribute]
    public string ConfigData { get; set; }

}

[System.Serializable]
public class AssembleWarShipType
{
    [XmlAttribute]
    public int TypeID { get; set; }
    [XmlAttribute]
    public string Name { get; set; }
    [XmlAttribute]
    public string IconPath { get; set; }

}

[System.Serializable]
public class AssembleWarshipClass
{
    [XmlAttribute]
    public int ClassID { get; set; }
    [XmlAttribute]
    public string ClassName { get; set; }
    [XmlAttribute]
    public string ClassDesc { get; set; }
    [XmlAttribute]
    public string ModelPath { get; set; }
}

[System.Serializable]
public class AssembleParts
{
    [XmlAttribute]
    public int PartID { get; set; }
    [XmlAttribute]
    public int ModelTypeID { get; set; }
    [XmlAttribute]
    public string BaseMaterialCost { get; set; }
    [XmlAttribute]
    public ushort BaseTimeCost { get; set; }
    [XmlAttribute]
    public string AssembleType { get; set; }
    [XmlAttribute]
    public string CustomData { get; set; }
}
[System.Serializable]
public class AssemblePartsType
{
    [XmlAttribute]
    public int ModelTypeID { get; set; }
    [XmlAttribute]
    public bool Unlock { get; set; }
    [XmlAttribute]
    public string PartIconSmall { get; set; }
    [XmlAttribute]
    public string PartSprite { get; set; }
    [XmlAttribute]
    public string ModelTypeName { get; set; }
    [XmlAttribute]
    public string ModelTypeDesc { get; set; }
    [XmlAttribute]
    public string ModelPath { get; set; }
    [XmlAttribute]
    public string TypeID { get; set; }
    [XmlAttribute]
    public string PropertyConfig { get; set; }

}