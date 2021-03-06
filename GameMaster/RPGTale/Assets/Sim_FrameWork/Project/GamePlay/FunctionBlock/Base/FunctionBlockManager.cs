﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Sim_FrameWork
{
    public class FunctionBlockManager : MonoSingleton<FunctionBlockManager>
    {
        private Dictionary<uint, FunctionBlockBase> _functionBlockInstances;

        private GameObject FunctionBlockContainer;
        private string BaseFunctionBlockPath = "Assets/Prefabs/FunctionBlock/FunctionBlockBase.prefab";

        protected override void Awake()
        {
            base.Awake();
            _functionBlockInstances = new Dictionary<uint, FunctionBlockBase>();
            FunctionBlockContainer = Utility.SafeFindGameobject("FunctionBlockContainer");
        }

        public Dictionary<uint,FunctionBlockBase> GetBlockInstancesDic()
        {
            return _functionBlockInstances;
        }

        #region FunctionBlock

        /// <summary>
        /// Add Block
        /// </summary>
        /// <param name="functionBlockID"></param>
        /// <param name="instanceID"></param>
        /// <returns></returns>
        public FunctionBlockBase AddFunctionBlock(int functionBlockID,uint instanceID,int posX,int posZ)
        {
            if (instanceID == 0)
            {
                instanceID = getUnUsedInstanceID();
            }

            FunctionBlockBase instance = Utility.CreateInstace(BaseFunctionBlockPath, FunctionBlockContainer, true).transform.SafeGetComponent<FunctionBlockBase>();

            instance.instanceID = instanceID;
            _functionBlockInstances.Add(instanceID, instance);

            instance.InitData(functionBlockID,posX,posZ);

            return instance;
        }
        public FunctionBlockBase AddFunctionBlock(int functionBlockID,int posX,int posZ)
        {
            return AddFunctionBlock(functionBlockID, 0, posX, posZ);
        }


        /// <summary>
        /// 获取工厂物体
        /// </summary>
        /// <param name="instanceID"></param>
        /// <returns></returns>
        public GameObject GetFunctionBlockObject(uint instanceID)
        {
            var obj= FunctionBlockContainer.transform.Find(instanceID.ToString()+ "[Block]");
            if (obj != null)
            {
                return obj.gameObject;
            }
            return null;
        }

        public FunctionBlockBase GetFunctionBlockBase(uint instanceID)
        {
            if (_functionBlockInstances.ContainsKey(instanceID))
            {
                return _functionBlockInstances[instanceID];
            }
            return null;
        }


        public void RemoveItem(FunctionBlockBase block)
        {
            if (_functionBlockInstances.ContainsKey(block.instanceID))
            {
                _functionBlockInstances.Remove(block.instanceID);
            }
            if (block != null)
            {
                ObjectManager.Instance.ReleaseObject(block.gameObject,0,true);
            }
          
        }



        private uint getUnUsedInstanceID()
        {
            uint instanceId = (uint)UnityEngine.Random.Range(1, float.MaxValue);
            if (_functionBlockInstances.ContainsKey(instanceId))
            {
                return getUnUsedInstanceID();
            }
            return instanceId;
        }

        #endregion

        public List<FunctionBlockBase> GetAllBlocks()
        {
            List<FunctionBlockBase> blocks = new List<FunctionBlockBase>();
            foreach(KeyValuePair<uint,FunctionBlockBase> kvp in _functionBlockInstances)
            {
                blocks.Add(kvp.Value);
            }
            return blocks;
        }

        #region ManuBlock
        public ManufactoryBase GetManuBlockBase(uint instanceID)
        {
            var obj = GetFunctionBlockObject(instanceID);
            if (obj != null)
            {
                var manuBase = UIUtility.SafeGetComponent<ManufactoryBase>(obj.transform);
                if (manuBase != null)
                    return manuBase;
            }
            return null;
        }


        #endregion


    }
}