using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Sim_FrameWork
{
    public enum ModifierTarget
    {

        /// <summary>
        /// 基础资源
        /// </summary>
        BaseResource,
        /// <summary>
        /// 区块作用
        /// </summary>
        FunctionBlock,
        MainShipPowerArea,
        MainShipWorkingArea,
        /// <summary>
        /// 地形
        /// </summary>
        Terrian,
        /// <summary>
        /// 订单
        /// </summary>
        Order,
        /// <summary>
        /// Error
        /// </summary>
        None
    }

    public class GeneralModifier
    {
        public List<ModifierBase> ModifierBase;
        public GeneralModifier LoadModifierData()
        {
            Config.JsonReader reader = new Config.JsonReader();
            var modifer = reader.LoadJsonDataConfig<GeneralModifier>(Config.JsonConfigPath.ModifierDataConfigJsonPath);
            ModifierBase = modifer.ModifierBase;
            return modifer;
        }
    }


    [Serializable]
    public class ModifierBase 
    {

        //Row Data
        public string ModifierName ;
        /// <summary>
        /// 作用目标
        /// </summary>
        public string Target;
        /// <summary>
        /// 区块作用类型
        /// </summary>
        public string effectType;
        /// <summary>
        /// 基础资源作用类型
        /// </summary>
        public ModifierBaseResourceType BaseResType;
        public ModifierOverlapType OverlapType = ModifierOverlapType.TimeStack;
        public ModifierRemoveType RemoveType = ModifierRemoveType.All;
        public ModifierAddType AddType = ModifierAddType.Loop;


        public int MaxLimit = 0;
        public float Time = 0;
        public float CallFrequency = 1;
        /// <summary>
        /// 效果值
        /// </summary>
        public float Value;


        public ModifierTarget ParseTargetType(string target)
        {
            ModifierTarget modifierTarget = ModifierTarget.None;
            Enum.TryParse<ModifierTarget>(target, out modifierTarget);
            if (modifierTarget == ModifierTarget.None)
            {
                Debug.LogError("ModiferType Error! Type=" + target);
            }
            return modifierTarget;
        }

        public ModifierFunctionBlockType ParseModifierFunctionBlockType(string blockType)
        {
            ModifierFunctionBlockType modifierFunctionBlockType = ModifierFunctionBlockType.None;
            Enum.TryParse<ModifierFunctionBlockType>(blockType, out modifierFunctionBlockType);
            if (modifierFunctionBlockType == ModifierFunctionBlockType.None)
            {
                Debug.LogError("ModiferType Error! Type=" + blockType);
            }
            return modifierFunctionBlockType;
        }

        public ModifierMainShip_PowerArea ParseModifierPowerAreaType(string typeName)
        {
            ModifierMainShip_PowerArea type = ModifierMainShip_PowerArea.None;
            Enum.TryParse<ModifierMainShip_PowerArea>(typeName, out type);
            if(type== ModifierMainShip_PowerArea.None)
            {
                Debug.LogError("ModifierType Error! Type=" + typeName);
            }
            return type;  
        }

    }
    [Serializable]
    public class ModifierData
    {
        /// <summary>
        /// Stack
        /// </summary>
        private static Stack<ModifierData> modifierCache = new Stack<ModifierData>();
        public static int modifierIndex { get; private set; }

        public int modifierDataID;

        public string modifierName;
        /// <summary>
        /// Modifier Type
        /// </summary>
        public ModifierTarget target;
        /// <summary>
        /// 叠加类型
        /// </summary>
        public ModifierOverlapType modifierOverlapType;
        /// <summary>
        /// 增加类型
        /// </summary>
        public ModifierAddType modifierAddType;
        /// <summary>
        /// 移除类型
        /// </summary>
        public ModifierRemoveType modifierRemoveType;

        public int MaxLimit ;

        [SerializeField]
        //层级时间
        private int _Limit;
        public int Limit { get { return _Limit; } }

        [SerializeField]
        //持续时间
        private float _persistentTime;
        public float PersistentTime { get { return _persistentTime; } }

        /// <summary>
        /// 当前时间
        /// </summary>
        [SerializeField]
        private float _currentTime;
        /// <summary>
        /// Param
        /// </summary>
        public object Data;

        /// <summary>
        /// 调用频率
        /// </summary>
        public float CallFrequency{ get; set; }
        /// <summary>
        /// 当前频率
        /// </summary>
        private float _curCallFrequency { get; set; }

        /// <summary>
        /// 执行次数
        /// </summary>
        [SerializeField]
        private int index =0;

        /// <summary>
        /// CallFrequency 结束后callback   传递data param
        /// </summary>
        public Action<object> CallBackParam;
        /// <summary>
        /// callback index
        /// </summary>
        public Action<object, int> CallBackParamIndex;

        /// <summary>
        /// CallFrequency 结束后callback
        /// </summary>
        public Action OnCallBack;
        /// <summary>
        /// 结束后callback index
        /// </summary>
        public Action<int> OnCallBackIndex;

        /// <summary>
        /// while Change Time
        /// </summary>
        public Action<ModifierData> OnChangeTime;
        /// <summary>
        /// While Add Layer
        /// </summary>
        public Action<ModifierData> OnAddLayer;
        /// <summary>
        /// while remove layer
        /// </summary>
        public Action<ModifierData> OnDeleteLayer;
        /// <summary>
        /// Start Action
        /// </summary>
        public Action Onstart;
        /// <summary>
        /// Finish Action
        /// </summary>
        public Action OnFinish;
        [SerializeField]
        private bool _isFinish;


        private ModifierData()
        {
            modifierDataID = modifierIndex++;
            CallFrequency = 1;
            _persistentTime = 0;
        }

        private ModifierData (float persistentTime,Action callback)
        {
            _persistentTime = persistentTime;
            OnCallBack = callback;
            modifierDataID = modifierIndex++;
        }

        public void ResetTime()
        {
            _currentTime = 0;
        }

        /// <summary>
        /// Change PersistentTime
        /// </summary>
        /// <param name="time"></param>
        public void ChangePersistentTime(float time)
        {
            _persistentTime = time;
            if (_persistentTime >= MaxLimit)
                _persistentTime = MaxLimit;
            if (OnChangeTime != null)
                OnChangeTime(this);
        }
        /// <summary>
        /// Add Layer
        /// </summary>
        public void AddLayer()
        {
            _Limit++;
            _currentTime = 0;
            if (_Limit > MaxLimit)
            {
                _Limit = MaxLimit;
                return;
            }
            if (OnAddLayer != null)
            {
                OnAddLayer(this);
            }
        }
        /// <summary>
        /// Delete Layer
        /// </summary>
        public void DeleteLayer()
        {
            _Limit--;
            if (OnDeleteLayer != null)
            {
                OnDeleteLayer(this);
            }
        }

        public void StartModifier()
        {
            _isFinish = false;
            if (Onstart != null)
            {
                Onstart();
            }
        }

        /// <summary>
        /// 执行Modifier
        /// </summary>
        /// <param name="delayTime"></param>
        public void OnTick(float delayTime)
        {
            _currentTime += delayTime;
            //大于持续时间
            if (_currentTime >= PersistentTime)
            {
                CallBack();
                if (modifierRemoveType == ModifierRemoveType.Layer) //以层数移除
                {
                    DeleteLayer();
                    if (_Limit <= 0)
                    {
                        _isFinish = true;
                        return;
                    }
                    _curCallFrequency = 0;
                    _currentTime = 0;
                    return;

                }
                _isFinish = true;
                return;
            }
            //如果频率调用
            if (CallFrequency > 0)
            {
                _curCallFrequency += delayTime;
                if (_curCallFrequency >= CallFrequency)
                {
                    _curCallFrequency = 0;
                    CallBack();
                }
                return;
            }

            CallBack();
        }

        public float GetCurrentModifierTime
        {
             get { return _currentTime; }
        }

        public bool IsFinish
        {
            get { return _isFinish; }
        }


        private void CallBack()
        {
            index++;
            if (modifierAddType == ModifierAddType.Once)
            {
                if (index > 1)
                {
                    index = 2;
                    return;
                }
            }
            if (OnCallBack != null)
                OnCallBack();
            if (OnCallBackIndex != null)
                OnCallBackIndex(index);
            if (CallBackParam != null)
                CallBackParam(Data);
            if (CallBackParamIndex != null)
                CallBackParamIndex(Data, index);
        }


        public void CloseModifier()
        {
            if (OnFinish != null)
                OnFinish();
            
        }

        public void ClearModifierData()
        {
            _Limit = 0;
            modifierName ="";
            index = 0;
            _persistentTime = 0;
            _currentTime = 0;
            Data = null;
            CallFrequency = 0;
            _curCallFrequency = 0;
            CallBackParam = null;
            OnCallBack = null;
            Onstart = null;
            OnFinish = null;
            _isFinish = false;
            AddStack(this);
        }


        public static ModifierData Create()
        {
            if (modifierCache.Count < 1)
                return new ModifierData();
            ModifierData md = modifierCache.Pop();
            return md;
        }

        public static ModifierData Create(ModifierBase modifier,Action callback)
        {
            return Create(modifier, callback, null, null);
        }

        public static ModifierData Create(ModifierBase modifier,Action callback,Action<ModifierData> addLayerAction,Action<ModifierData> removeLayerAction)
        {
            ModifierData data = Pop();
            data.modifierAddType = modifier.AddType;
            data.modifierName = modifier.ModifierName;
            data.CallFrequency = modifier.CallFrequency;
            data._persistentTime = modifier.Time;
            data.modifierOverlapType = modifier.OverlapType;
            data.modifierRemoveType = modifier.RemoveType;
            data.target = modifier.ParseTargetType(modifier.Target);
            data.MaxLimit = modifier.MaxLimit;
            data.OnCallBack = callback;
            data.OnAddLayer = addLayerAction;
            data.OnDeleteLayer = removeLayerAction;
            data._Limit = 1;
            return data;

        }


        /// <summary>
        /// Pop Data
        /// </summary>
        /// <returns></returns>
        private static ModifierData Pop()
        {
            if (modifierCache.Count < 1)
            {
                ModifierData data = new ModifierData();
                return data;
            }
            ModifierData md = modifierCache.Pop();
            return md;
        }

        /// <summary>
        /// Push Stack
        /// </summary>
        /// <param name="data"></param>
        private static void AddStack(ModifierData data)
        {
            modifierCache.Push(data);
        }

    }

    /// <summary>
    /// 叠加类型
    /// </summary>
    public enum ModifierOverlapType
    {
        None,
        /// <summary>
        /// 按层数堆叠
        /// </summary>
        TimeStack,
        /// <summary>
        /// 重置
        /// </summary>
        TimeReset
    }

    public enum ModifierAddType
    {
        Once,
        Loop
    }

    /// <summary>
    /// Remove Type
    /// </summary>
    public enum ModifierRemoveType
    {
        All,
        Layer
    }
}