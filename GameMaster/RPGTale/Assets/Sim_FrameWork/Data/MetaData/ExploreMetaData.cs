using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;

[System.Serializable]
public class ExploreMetaData : ExcelBase {

#if UNITY_EDITOR
    public override void Construction()
    {
        AllExploreAreaList = new List<ExploreArea>();
        for(int i = 0; i < 2; i++)
        {
            ExploreArea area = new ExploreArea();
            area.AreaID = i;
            area.Name = "";
            area.NameTitle = "";
            area.Desc = "";
            area.IconPath = "";
            area.Unlock = true;
            area.DefaultMissionCount = (ushort)i;
            area.ExploreList = "";
            AllExploreAreaList.Add(area);
        }

        AllExploreDataList = new List<ExploreData>();
        for(int i = 0; i < 2; i++)
        {
            ExploreData data = new ExploreData();
            data.ExploreID = i;
            data.MissionName = "";
            data.AreaName = "";
            data.MissionDesc = "";
            data.BGPath = "";
            data.CameraPos = "";
            data.CameraRotation = "";
            data.TeamMaxNum = (ushort)i;
            data.UnLockValue = (ushort)i;
            data.RequirePreID = i;
            data.HardLevel = (ushort)i;
            data.Weight = (ushort)i;
            data.Depth = (ushort)i;
            data.SeriesID = i;
            AllExploreDataList.Add(data);
        }

        AllExplorePointList = new List<ExplorePoint>();
        for(int i = 0; i < 2; i++)
        {
            ExplorePoint point = new ExplorePoint();
            point.PointID = i;
            point.SeriesID = i;
            point.Name = "";
            point.Desc = "";
            point.ExploreValue = (ushort)i;
            point.PointNevigator = "";
            point.DepthLevel = (ushort)i;
            point.HardLevel = (ushort)i;
            point.PrePoint = i;
            point.EnergyCost= (ushort)i;
            point.Time = (ushort)i;
            point.EventID = i;
            AllExplorePointList.Add(point);
        }

        AllExploreEventList = new List<ExploreEvent>();
        for(int i = 0; i < 2; i++)
        {
            ExploreEvent e = new ExploreEvent();
            e.EventID = i;
            e.Name = "";
            e.TitleName = "";
            e.Desc = "";
            e.EventBG = "";
            e.ChooseList = "";
            AllExploreEventList.Add(e);
        }

        AllExploreChooseList = new List<ExploreChoose>();
        for(int i = 0; i < 2; i++)
        {
            ExploreChoose choose = new ExploreChoose();
            choose.ChooseID = i;
            choose.Content = "";
            choose.RewardID = i;
            choose.NextEvent = i;
            AllExploreChooseList.Add(choose);
        }
    }
#endif
    public override void Init()
    {
        AllExploreAreaDic.Clear();
        AllExploreChooseDic.Clear();
        AllExploreDataDic.Clear();
        AllExploreEventDic.Clear();
        AllExplorePointDic.Clear();
        foreach (var data in AllExploreAreaList)
        {
            if (AllExploreAreaDic.ContainsKey(data.AreaID))
            {
                Debug.LogError("Find Same ExploreArea , AreaID  = " + data.AreaID);
            }
            else
            {
                AllExploreAreaDic.Add(data.AreaID, data);
            }
        }
        foreach (var data in AllExploreDataList)
        {
            if (AllExploreDataDic.ContainsKey(data.ExploreID))
            {
                Debug.LogError("Find Same ExploreID , ExploreID  = " + data.ExploreID);
            }
            else
            {
                AllExploreDataDic.Add(data.ExploreID, data);
            }
        }
        foreach (var data in AllExplorePointList)
        {
            if (AllExplorePointDic.ContainsKey(data.PointID))
            {
                Debug.LogError("Find Same ExplorePointID , PointID  = " + data.PointID);
            }
            else
            {
                AllExplorePointDic.Add(data.PointID, data);
            }
        }
        foreach (var data in AllExploreEventList)
        {
            if (AllExploreEventDic.ContainsKey(data.EventID))
            {
                Debug.LogError("Find Same ExploreEventID , EventID  = " + data.EventID);
            }
            else
            {
                AllExploreEventDic.Add(data.EventID, data);
            }
        }
        foreach (var data in AllExploreChooseList)
        {
            if (AllExploreChooseDic.ContainsKey(data.ChooseID))
            {
                Debug.LogError("Find Same ExploreChooseID , EventID  = " + data.ChooseID);
            }
            else
            {
                AllExploreChooseDic.Add(data.ChooseID, data);
            }
        }
    }

    [XmlIgnore]
    public Dictionary<int, ExploreArea> AllExploreAreaDic = new Dictionary<int, ExploreArea>();
    [XmlIgnore]
    public Dictionary<int, ExploreData> AllExploreDataDic = new Dictionary<int, ExploreData>();
    [XmlIgnore]
    public Dictionary<int, ExplorePoint> AllExplorePointDic = new Dictionary<int, ExplorePoint>();
    [XmlIgnore]
    public Dictionary<int, ExploreEvent> AllExploreEventDic = new Dictionary<int, ExploreEvent>();
    [XmlIgnore]
    public Dictionary<int, ExploreChoose> AllExploreChooseDic = new Dictionary<int, ExploreChoose>();

    [XmlElement]
    public List<ExploreArea> AllExploreAreaList { get; set; }
    [XmlElement]
    public List<ExploreData> AllExploreDataList { get; set; }
    [XmlElement]
    public List<ExplorePoint> AllExplorePointList { get; set; }
    [XmlElement]
    public List<ExploreEvent> AllExploreEventList { get; set; }
    [XmlElement]
    public List<ExploreChoose> AllExploreChooseList { get; set; }

}

[System.Serializable]
public class ExploreArea
{
    [XmlElement]
    public int AreaID { get; set; }
    [XmlElement]
    public string Name { get; set; }
    [XmlElement]
    public string NameTitle { get; set; }
    [XmlElement]
    public string Desc { get; set; }
    [XmlElement]
    public string IconPath { get; set; }
    [XmlElement]
    public bool Unlock { get; set; }
    [XmlElement]
    public ushort DefaultMissionCount { get; set; }
    [XmlElement]
    public string ExploreList { get; set; }

    
}

[System.Serializable]
public class ExploreData
{
    [XmlElement]
    public int ExploreID { get; set; }
    [XmlElement]
    public string MissionName { get; set; }
    [XmlElement]
    public string AreaName { get; set; }
    [XmlElement]
    public string MissionDesc { get; set; }
    [XmlElement]
    public string BGPath { get; set; }
    [XmlElement]
    public string CameraPos { get; set; }
    [XmlElement]
    public string CameraRotation { get; set; }
    [XmlElement]
    public ushort TeamMaxNum { get; set; }
    [XmlElement]
    public ushort UnLockValue { get; set; }
    [XmlElement]
    public int RequirePreID { get; set; }
    [XmlElement]
    public ushort HardLevel { get; set; }
    [XmlElement]
    public ushort Weight { get; set; }
    [XmlElement]
    public ushort Depth { get; set; }
    [XmlElement]
    public int SeriesID { get; set; }
}

[System.Serializable]
public class ExplorePoint
{
    [XmlElement]
    public int PointID { get; set; }
    [XmlElement]
    public int SeriesID { get; set; }
    [XmlElement]
    public string Name { get; set; }
    [XmlElement]
    public string Desc { get; set; }
    [XmlElement]
    public int ExploreValue { get; set; }
    [XmlElement]
    public string PointNevigator { get; set; }
    [XmlElement]
    public ushort DepthLevel { get; set; }
    [XmlElement]
    public ushort HardLevel { get; set; }
    [XmlElement]
    public int PrePoint { get; set; }
    [XmlElement]
    public ushort EnergyCost { get; set; }
    [XmlElement]
    public ushort Time { get; set; }
    [XmlElement]
    public int EventID { get; set; }
}

[System.Serializable]
public class ExploreEvent
{
    [XmlElement]
    public int EventID { get; set; }
    [XmlElement]
    public string Name { get; set; }
    [XmlElement]
    public string TitleName { get; set; }
    [XmlElement]
    public string Desc { get; set; }
    [XmlElement]
    public string EventBG { get; set; }
    [XmlElement]
    public string ChooseList { get; set; }
}

[System.Serializable]
public class ExploreChoose
{
    [XmlElement]
    public int ChooseID { get; set; }
    [XmlElement]
    public string Content { get; set; }
    [XmlElement]
    public int RewardID { get; set; }
    [XmlElement]
    public int NextEvent { get; set; }
}