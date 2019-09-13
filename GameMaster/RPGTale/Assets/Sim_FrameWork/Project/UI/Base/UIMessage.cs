using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork
{
    public class UIMessage 
    {
        public UIMsgType type;
        public object content;

        public UIMessage(UIMsgType Msgtype, object Content)
        {
            type = Msgtype;
            content = Content;
        }
    }

    public enum UIMsgType
    {
        UpdateManuSlot,//更新生产格
        UpdateLevelInfo,//更新等级
        UpdateSpeedText,//更新速度
        UpdateWarehouseData,//更新仓库

        UpdateResourceData,//更新主界面资源
        UpdateBuildPanelData, //更新建造列表
        UpdateTime,//更新时间
    }
}