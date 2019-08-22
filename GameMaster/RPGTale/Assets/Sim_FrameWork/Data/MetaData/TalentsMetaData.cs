using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;

[System.Serializable]
public class TalentsMetaData : ExcelBase {

#if UNITY_EDITOR
    public override void Construction()
    {
        AllTalentsList = new List<Talents>();
        for(int i = 0; i < 2; i++)
        {
            Talents ta = new Talents();
            ta.TalentID = i;
            ta.TalentType = (ushort)i;
            ta.TalentName = "";
            ta.TalentDesc = "";
            ta.TalentIcon = "";
            ta.LevelBase = (ushort)i;
            ta.LevelMax = (ushort)i;
            ta.TalentRarity = (ushort)i;
            ta.SkillBaseList = "";
            ta.SkillExtendParam = "";
            ta.TalentTagList = "";
            AllTalentsList.Add(ta);
        }

        AllTalentTraitDataList = new List<TalentTraitData>();
        for(int i = 0; i < 2; i++)
        {
            TalentTraitData tt = new TalentTraitData();
            tt.TraitID = i;
            tt.TraitName = "";
            tt.TraitDesc = "";
            tt.TraitIcon = "";
            tt.TraitEffect = "";
            AllTalentTraitDataList.Add(tt);
        }

        AllTalentSkillDataList = new List<TalentSkillData>();
        for(int i = 0; i < 2; i++)
        {
            TalentSkillData ts = new TalentSkillData();
            ts.SkillID = i;
            ts.MaxLevel = (ushort)i;
            ts.SkillRarity = (ushort)i;
            ts.SkillName = "";
            ts.SkillDesc = "";
            ts.SkillIcon = "";
            ts.SkillEffect = "";
            ts.SkillGrowth = i;
            AllTalentSkillDataList.Add(ts);
        }

        AllTalentTypeDataList = new List<TalentTypeData>();
        for(int i = 0; i < 2; i++)
        {
            TalentTypeData ttype = new TalentTypeData();
            ttype.Type = "";
            ttype.TypeName = "";
            ttype.TypeDesc = "";
            ttype.TypeIcon = "";
            AllTalentTypeDataList.Add(ttype);
        }
    }

#endif
    public override void Init()
    {
        AllTalentSkillDataDic.Clear();
        AllTalentTraitDataDic.Clear();
        AllTalentTypeDataDic.Clear();
        AllTalentsDic.Clear();

        foreach (var data in AllTalentsList)
        {
            if (AllTalentsDic.ContainsKey(data.TalentID))
            {
                Debug.LogError("Find Same TalentID , TalentID= " + data.TalentID);
            }
            else
            {
                AllTalentsDic.Add(data.TalentID, data);
            }
        }

        foreach (var data in AllTalentTraitDataList)
        {
            if (AllTalentTraitDataDic.ContainsKey(data.TraitID))
            {
                Debug.LogError("Find Same TraitID , TraitID= " + data.TraitID);
            }
            else
            {
                AllTalentTraitDataDic.Add(data.TraitID, data);
            }
        }

        foreach (var data in AllTalentSkillDataList)
        {
            if (AllTalentSkillDataDic.ContainsKey(data.SkillID))
            {
                Debug.LogError("Find Same SkillID , SkillID= " + data.SkillID);
            }
            else
            {
                AllTalentSkillDataDic.Add(data.SkillID, data);
            }
        }

        foreach (var data in AllTalentTypeDataList)
        {
            if (AllTalentTypeDataDic.ContainsKey(data.Type))
            {
                Debug.LogError("Find Same SkillID , SkillID= " + data.Type);
            }
            else
            {
                AllTalentTypeDataDic.Add(data.Type, data);
            }
        }

    }


    [XmlIgnore]
    public Dictionary<int, Talents> AllTalentsDic = new Dictionary<int, Talents>();
    [XmlIgnore]
    public Dictionary<int, TalentTraitData> AllTalentTraitDataDic = new Dictionary<int, TalentTraitData>();
    [XmlIgnore]
    public Dictionary<int, TalentSkillData> AllTalentSkillDataDic = new Dictionary<int, TalentSkillData>();
    [XmlIgnore]
    public Dictionary<string, TalentTypeData> AllTalentTypeDataDic = new Dictionary<string, TalentTypeData>();

    [XmlElement]
    public List<Talents> AllTalentsList { get; set; }
    [XmlElement]
    public List<TalentTraitData> AllTalentTraitDataList { get; set; }
    [XmlElement]
    public List<TalentSkillData> AllTalentSkillDataList { get; set; }
    [XmlElement]
    public List<TalentTypeData> AllTalentTypeDataList { get; set; }
}


[System.Serializable] 
public class Talents
{
    [XmlAttribute]
    public int TalentID { get; set; }
    [XmlAttribute]
    public ushort TalentType { get; set; }
    [XmlAttribute]
    public string TalentName { get; set; }
    [XmlAttribute]
    public string TalentDesc { get; set; }
    [XmlAttribute]
    public string TalentIcon { get; set; }
    [XmlAttribute]
    public ushort LevelBase { get; set; }
    [XmlAttribute]
    public ushort LevelMax { get; set; }
    [XmlAttribute]
    public ushort TalentRarity { get; set; }
    [XmlAttribute]
    public string SkillBaseList { get; set; }
    [XmlAttribute]
    public string SkillExtendParam { get; set; }
    [XmlAttribute]
    public string TalentTagList { get; set; }
}

[System.Serializable] 
public class TalentTraitData
{
    [XmlAttribute]
    public int TraitID { get; set; }
    [XmlAttribute]
    public string TraitName { get; set; }
    [XmlAttribute]
    public string TraitDesc { get; set; }
    [XmlAttribute]
    public string TraitIcon { get; set; }
    [XmlAttribute]
    public string TraitEffect { get; set; }
}

[System.Serializable] 
public class TalentSkillData
{
    [XmlAttribute]
    public int SkillID { get; set; }
    [XmlAttribute]
    public ushort MaxLevel { get; set; }
    [XmlAttribute]
    public ushort SkillRarity { get; set; }
    [XmlAttribute]
    public string SkillName { get; set; }
    [XmlAttribute]
    public string SkillDesc { get; set; }
    [XmlAttribute]
    public string SkillIcon { get; set; }
    [XmlAttribute]
    public string SkillEffect { get; set; }
    [XmlAttribute]
    public float SkillGrowth { get; set; }
}

[System.Serializable] 
public class TalentTypeData
{
    [XmlAttribute]
    public string Type { get; set; }
    [XmlAttribute]
    public string TypeName { get; set; }
    [XmlAttribute]
    public string TypeDesc { get; set; }
    [XmlAttribute]
    public string TypeIcon { get; set; }
}