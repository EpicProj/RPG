using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Sim_FrameWork
{
    public class FunctionBlockManager : MonoSingleton<FunctionBlockManager>
    {
        private Dictionary<int, FunctionBlockBase> _functionBlockInstances;

        private GameObject FunctionBlockContainer;
        private string BaseFunctionBlockPath = "Assets/Prefabs/FunctionBlock/FunctionBlockBase.prefab";

        protected override void Awake()
        {
            base.Awake();
            _functionBlockInstances = new Dictionary<int, FunctionBlockBase>();
            FunctionBlockContainer = Utility.SafeFindGameobject("MapManager/FunctionBlockContainer");
        }

        public Dictionary<int,FunctionBlockBase> GetBlockInstances()
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
        public FunctionBlockBase AddFunctionBlock(int functionBlockID,int instanceID,int posX,int posZ)
        {

            FunctionBlockBase instance = UIUtility.SafeGetComponent<FunctionBlockBase>(Utility.CreateInstace(BaseFunctionBlockPath, FunctionBlockContainer, true).transform);

            if (instanceID == -1)
            {
                instanceID = getUnUsedInstanceID();
            }
            instance.instanceID = instanceID;
            _functionBlockInstances.Add(instanceID, instance);

            instance.InitData(functionBlockID,posX,posZ);

            return instance;
        }

        public FunctionBlockBase AddFunctionBlock(int functionBlockID)
        {
            int posX = 0;
            int posZ = 0;
            return AddFunctionBlock(functionBlockID, -1, posX, posZ);
        }

        public void RemoveItem(FunctionBlockBase block)
        {
            if (_functionBlockInstances.ContainsKey(block.instanceID))
            {
                _functionBlockInstances.Remove(block.instanceID);
            }
            if (block != null)
            {
                ObjectManager.Instance.ReleaseObject(block.gameObject);
            }
           
        }



        private int getUnUsedInstanceID()
        {
            int instanceId = UnityEngine.Random.Range(10000, 99999);
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
            foreach(KeyValuePair<int,FunctionBlockBase> kvp in _functionBlockInstances)
            {
                blocks.Add(kvp.Value);
            }
            return blocks;
        }

    }
}