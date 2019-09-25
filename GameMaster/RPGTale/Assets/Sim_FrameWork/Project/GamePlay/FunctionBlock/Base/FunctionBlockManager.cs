using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Sim_FrameWork
{
    public class FunctionBlockManager : MonoSingleton<FunctionBlockManager>
    {
        public Dictionary<string, FunctionBlockBase> FunctionBlockDic = new Dictionary<string, FunctionBlockBase>();


        protected override void Awake()
        {
            base.Awake();
        }


        #region FunctionBlock


        public void AddFunctionBlockData(string blockUID,FunctionBlockBase blockBase)
        {
            if (!FunctionBlockDic.ContainsKey(blockUID))
            {
                FunctionBlockDic.Add(blockUID,blockBase);
            }
        }

        public FunctionBlockBase GetBlockByUID(string UID)
        {
            FunctionBlockBase block = null;
            FunctionBlockDic.TryGetValue(UID, out block);
            if (block == null)
                Debug.LogError("GetBlock Error UID=" + UID);
            return block;
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