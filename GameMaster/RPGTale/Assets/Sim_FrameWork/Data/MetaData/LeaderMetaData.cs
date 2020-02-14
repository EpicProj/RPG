using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;

[System.Serializable]
public class LeaderMetaData : ExcelBase {

#if UNITY_EDITOR
    public override void Construction()
    {

        AllLeaderPresetDataList = new List<LeaderPresetData>();
        for(int i = 0; i < 2; i++)
        {
            LeaderPresetData tt = new LeaderPresetData();
            tt.LeaderID = i;
            AllLeaderPresetDataList.Add(tt);
        }
    }

#endif
    public override void Init()
    {
        AllLeaderPresetDataDic.Clear();
        AllLeaderSkillDataDic.Clear();
        AllLeaderDutyDataDic.Clear();
        AllLeaderSpeciesDataDic.Clear();

        foreach (var data in AllLeaderPresetDataList)
        {
            if (AllLeaderPresetDataDic.ContainsKey(data.LeaderID))
            {
                Debug.LogError("[LeaderPresetData] Find Same LeaderID , LeaderID= " + data.LeaderID);
            }
            else
            {
                AllLeaderPresetDataDic.Add(data.LeaderID, data);
            }
        }

        foreach (var data in AllLeaderSpeciesDataList)
        {
            if (AllLeaderSpeciesDataDic.ContainsKey(data.SpeciesID))
            {
                Debug.LogError("[LeaderSpeciesData] Find Same SpeciesID , SpeciesID= " + data.SpeciesID);
            }
            else
            {
                AllLeaderSpeciesDataDic.Add(data.SpeciesID, data);
            }
        }

        foreach (var data in AllLeaderSkillDataList)
        {
            if (AllLeaderSkillDataDic.ContainsKey(data.SkillID))
            {
                Debug.LogError("[LeaderSkillData] Find Same SkillID , SkillID= " + data.SkillID);
            }
            else
            {
                AllLeaderSkillDataDic.Add(data.SkillID, data);
            }
        }

        foreach (var data in AllLeaderDutyDataList)
        {
            if (AllLeaderDutyDataDic.ContainsKey(data.DutyID))
            {
                Debug.LogError("[leaderDuty] Find Same DutyID , DutyID= " + data.DutyID);
            }
            else
            {
                AllLeaderDutyDataDic.Add(data.DutyID, data);
            }
        }

        foreach (var data in AllLeaderCreedDataList)
        {
            if (AllLeaderCreedDataDic.ContainsKey(data.CreedID))
            {
                Debug.LogError("[LeaderCreed] Find Same CreedID , CreedID= " + data.CreedID);
            }
            else
            {
                AllLeaderCreedDataDic.Add(data.CreedID, data);
            }
        }

        foreach (var data in AllLeaderAttributeDataList)
        {
            if (AllLeaderAttributeDataDic.ContainsKey(data.AttributeID))
            {
                Debug.LogError("[LeaderAttribute] Find Same AttributeID , AttributeID= " + data.AttributeID);
            }
            else
            {
                AllLeaderAttributeDataDic.Add(data.AttributeID, data);
            }
        }

        foreach (var data in AllLeaderBirthlandDataList)
        {
            if (AllLeaderBirthlandDataDic.ContainsKey(data.LandID))
            {
                Debug.LogError("[LeaderBirthland] Find Same LandID , LandID= " + data.LandID);
            }
            else
            {
                AllLeaderBirthlandDataDic.Add(data.LandID, data);
            }
        }

        foreach (var data in AllLeaderStoryDataList)
        {
            if (AllLeaderStoryDataDic.ContainsKey(data.StoryID))
            {
                Debug.LogError("[LeaderStory] Find Same StoryID , StoryID= " + data.StoryID);
            }
            else
            {
                AllLeaderStoryDataDic.Add(data.StoryID, data);
            }
        }

    }


    [XmlIgnore]
    public Dictionary<int, LeaderPresetData> AllLeaderPresetDataDic = new Dictionary<int, LeaderPresetData>();
    [XmlIgnore]
    public Dictionary<int, LeaderSpeciesData> AllLeaderSpeciesDataDic = new Dictionary<int, LeaderSpeciesData>();
    [XmlIgnore]
    public Dictionary<int, LeaderSkillData> AllLeaderSkillDataDic = new Dictionary<int, LeaderSkillData>();
    [XmlIgnore]
    public Dictionary<int, LeaderDutyData> AllLeaderDutyDataDic = new Dictionary<int, LeaderDutyData>();
    [XmlIgnore]
    public Dictionary<int, LeaderCreedData> AllLeaderCreedDataDic = new Dictionary<int, LeaderCreedData>();
    [XmlIgnore]
    public Dictionary<int, LeaderAttributeData> AllLeaderAttributeDataDic = new Dictionary<int, LeaderAttributeData>();
    [XmlIgnore]
    public Dictionary<int, LeaderBirthlandData> AllLeaderBirthlandDataDic = new Dictionary<int, LeaderBirthlandData>();
    [XmlIgnore]
    public Dictionary<int, LeaderStoryData> AllLeaderStoryDataDic = new Dictionary<int, LeaderStoryData>();

    [XmlElement]
    public List<LeaderPresetData> AllLeaderPresetDataList { get; set; }
    [XmlElement]
    public List<LeaderSpeciesData> AllLeaderSpeciesDataList { get; set; }
    [XmlElement]
    public List<LeaderSkillData> AllLeaderSkillDataList { get; set; }
    [XmlElement]
    public List<LeaderDutyData> AllLeaderDutyDataList { get; set; }
    [XmlElement]
    public List<LeaderCreedData> AllLeaderCreedDataList { get; set; }
    [XmlElement]
    public List<LeaderAttributeData> AllLeaderAttributeDataList { get; set; }
    [XmlElement]
    public List<LeaderBirthlandData> AllLeaderBirthlandDataList { get; set; }
    [XmlElement]
    public List<LeaderStoryData> AllLeaderStoryDataList { get; set; }
}


[System.Serializable] 
public class LeaderPresetData
{
    [XmlAttribute]
    public int LeaderID { get; set; }
    [XmlAttribute]
    public string LeaderName { get; set; }
    [XmlAttribute]
    public string LeaderDesc { get; set; }
    [XmlAttribute]
    public int SpeciesID { get; set; }
    [XmlAttribute]
    public ushort Gender { get; set; }
    [XmlAttribute]
    public ushort Age { get; set; }
    [XmlAttribute]
    public int CreedID { get; set; }
    [XmlAttribute]
    public string LeaderSkillIDList { get; set; }
    [XmlAttribute]
    public int BirthlandID { get; set; }
    [XmlAttribute]
    public string AttributeIDList { get; set; }
    [XmlAttribute]
    public string StoryList { get; set; }
    [XmlAttribute]
    public int Portrait_BG { get; set; }
    [XmlAttribute]
    public int Portrait_Cloth { get; set; }
    [XmlAttribute]
    public int Portrait_Ear { get; set; }
    [XmlAttribute]
    public int Portrait_Hair { get; set; }
    [XmlAttribute]
    public int Portrait_Eyes { get; set; }
    [XmlAttribute]
    public int Portrait_Face { get; set; }
    [XmlAttribute]
    public int Portrait_Mouth { get; set; }
    [XmlAttribute]
    public int Portrait_Nose { get; set; }
}

[System.Serializable]
public class LeaderSpeciesData
{
    [XmlAttribute]
    public int SpeciesID { get; set; }
    [XmlAttribute]
    public string SpeciesName { get; set; }
    [XmlAttribute]
    public string SpeciesDesc { get; set; }
    [XmlAttribute]
    public string Age_DevideParam { get; set; }
    [XmlAttribute]
    public string Portrait_MaleGroup { get; set; }
    [XmlAttribute]
    public string Portrait_FemaleGroup { get; set; }
    [XmlAttribute]
    public string Name_MaleGroup { get; set; }
    [XmlAttribute]
    public string Name_FemaleGroup { get; set; }
}

[System.Serializable] 
public class LeaderSkillData
{
    [XmlAttribute]
    public int SkillID { get; set; }
    [XmlAttribute]
    public ushort MaxLevel { get; set; }
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
public class LeaderDutyData
{
    [XmlAttribute]
    public int DutyID { get; set; }
    [XmlAttribute]
    public string DutyName { get; set; }
    [XmlAttribute]
    public string DutyDesc { get; set; }
    [XmlAttribute]
    public string DutyIcon { get; set; }
}

[System.Serializable]
public class LeaderCreedData
{
    [XmlAttribute]
    public int CreedID { get; set; }
    [XmlAttribute]
    public string CreedName { get; set; }
    [XmlAttribute]
    public string CreedDesc { get; set; }
    [XmlAttribute]
    public string IconPath { get; set; }
    [XmlAttribute]
    public int CampCreedID { get; set; }
}

[System.Serializable]
public class LeaderAttributeData
{
    [XmlAttribute]
    public int AttributeID { get; set; }
    [XmlAttribute]
    public string Name { get; set; }
    [XmlAttribute]
    public string Desc { get; set; }
    [XmlAttribute]
    public string IconPath { get; set; }
}
[System.Serializable]
public class LeaderBirthlandData
{
    [XmlAttribute]
    public int LandID { get; set; }
    [XmlAttribute]
    public string LandName { get; set; }
    [XmlAttribute]
    public string LandDesc { get; set; }
    [XmlAttribute]
    public string LandIconPath { get; set; }
    [XmlAttribute]
    public string LandBGPath { get; set; }
}

[System.Serializable]
public class LeaderStoryData
{
    [XmlAttribute]
    public int StoryID { get; set; }
    [XmlAttribute]
    public string Content { get; set; }
    [XmlAttribute]
    public ushort PoolLevel { get; set; }
    [XmlAttribute]
    public ushort StoryYearStart { get; set; }
    [XmlAttribute]
    public ushort StoryYearEnd { get; set; }
    [XmlAttribute]
    public string LinkAttributeParam { get; set; }
    [XmlAttribute]
    public string BirthlandLimitList { get; set; }

}