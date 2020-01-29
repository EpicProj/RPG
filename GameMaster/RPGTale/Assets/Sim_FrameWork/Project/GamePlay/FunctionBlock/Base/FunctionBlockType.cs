using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork
{
    public enum FunctionBlockType
    {
        None,
        #region WorkingArea
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
        DeflectorShieldProjector,
        #endregion
        #region LivingArea
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
        #endregion
        #region WorkingArea
        /// <summary>
        /// 通讯站
        /// </summary>
        OperatingOffice,
        /// <summary>
        /// 主控分析室
        /// </summary>
        MainControlAnalysisRoom,
        /// <summary>
        /// 太空对接环
        /// </summary>
        DockingRing,
        /// <summary>
        /// 拆解室
        /// </summary>
        DismantlingRoom,
        /// <summary>
        /// 元素舱
        /// </summary>
        ElementCapsule,
        /// <summary>
        /// 研发实验室
        /// </summary>
        ResearchLaboratory,
        #endregion
        #region PowerArea
        /// <summary>
        /// 储能单元
        /// </summary>
        EnergyStorageUnit,
        /// <summary>
        /// 反应舱
        /// </summary>
        ReactionTank,
        /// <summary>
        /// 湮灭反应堆
        /// </summary>
        AnnihilationReactor,
        /// <summary>
        /// 冷却固化堆
        /// </summary>
        CooledReactor,
        /// <summary>
        /// 过载缓存引擎
        /// </summary>
        OverloadEngine,
        /// <summary>
        /// 共振分束器
        /// </summary>
        ResonanceBeamSplitter,
        #endregion
    }

    public enum FunctionBlockType_WorkingArea
    {
    
    }

    public enum FunctionBlock_LivingArea
    {
       
    }
}