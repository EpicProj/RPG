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
        private string BaseFunctionBlockPath = "";

        protected override void Awake()
        {
            base.Awake();
            _functionBlockInstances = new Dictionary<int, FunctionBlockBase>();
            FunctionBlockContainer = Utility.SafeFindGameobject("MapContainer/FunctionBlockContainer");
        }

        public Dictionary<int,FunctionBlockBase> GetBlockInstances()
        {
            return _functionBlockInstances;
        }

        #region FunctionBlock


        public FunctionBlockBase AddFunctionBlock(int functionBlockID,int instanceID)
        {

            FunctionBlockBase instance = UIUtility.SafeGetComponent<FunctionBlockBase>(Utility.CreateInstace(BaseFunctionBlockPath, FunctionBlockContainer, true).transform);

            if (instanceID == -1)
            {
                instanceID = getUnUsedInstanceID();
            }
            instance.instanceID = instanceID;
            _functionBlockInstances.Add(instanceID, instance);

            instance.InitData();

            return instance;
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


        //Place Block
        public void PlaceFunctionBlock(int blockID, Vector3 checkpos)
        {
            var block = FunctionBlockModule.GetFunctionBlockByBlockID(blockID);
            if (block == null)
                return;


        }

        #endregion
    }
}