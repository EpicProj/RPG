using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork
{
    public enum FunctionBlockType
    {
        None,
        Industry,
        Public,
        Research,
        Energy,
        Arms,
        Unique
    }

    public enum FunctionBlockType_WorkingArea
    {
        /// <summary>
        /// 虹普分析仪
        /// </summary>
        RainbowAnalyzer,
        /// <summary>
        /// 阵列扫描器
        /// </summary>
        ArrayScanner,
        /// <summary>
        /// 反馈天线
        /// </summary>
        FeedbackAntenna,
        /// <summary>
        /// 发射器协调室
        /// </summary>
        TransmitterCoordination,
        /// <summary>
        /// 成像分析光度计
        /// </summary>
        ImagingAnalysisPhotometer,
        /// <summary>
        /// 偏导护盾投射器
        /// </summary>
        DeflectorShieldProjector
    }

    public enum FunctionBlock_LivingArea
    {
        /// <summary>
        /// 训练室
        /// </summary>
        TrainingRoom,
        /// <summary>
        /// 休息室
        /// </summary>
        Lounge,
        /// <summary>
        /// 货舱
        /// </summary>
        Cargo,
        /// <summary>
        /// 娱乐吧台
        /// </summary>
        EntertainmentBar,
        /// <summary>
        /// 光合舱
        /// </summary>
        PhotosyntheticTank,
        /// <summary>
        /// 生命保障系统
        /// </summary>
        lifeSupportSystem,
    }
}