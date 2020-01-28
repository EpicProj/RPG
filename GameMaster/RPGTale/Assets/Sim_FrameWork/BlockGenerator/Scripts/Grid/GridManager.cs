using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork
{
    public class GridManager : MonoSingleton<GridManager>
    {
        public enum Action
        {
            ADD,
            REMOVE
        }

        private GameObject BlockSelectionUI;


        protected override void Awake()
        {
            base.Awake();
            BlockSelectionUI = transform.FindTransfrom("MainShipAreaContainer/Content/UI/SelectionUI").gameObject;
        }

        public bool showNodes;

        public int nodeWidth = 50;
        public int nodeLength = 50;

        public int[,] instanceNodes;


        void OnDrawGizmos()
        {
            if (!showNodes)
                return;
        }


        /// <summary>
        /// 更新格
        /// </summary>
        public void UpdateAllNodes()
        {
            instanceNodes = new int[nodeWidth, nodeLength];

            for(int x = 0; x < nodeWidth; x++)
            {
                for(int z = 0; z < nodeLength; z++)
                {
                    /// -1 is empty
                    instanceNodes[x, z] = -1;
                }
            }

            foreach( KeyValuePair<int,FunctionBlockBase> kvp in FunctionBlockManager.Instance.GetBlockInstancesDic())
            {
                FunctionBlockBase block = kvp.Value;
                UpdateFunctionBlockNodes(block,Action.ADD);
            }
        }

        /// <summary>
        /// 刷新建筑
        /// </summary>
        /// <param name="block"></param>
        public void UpdateFunctionBlockNodes(FunctionBlockBase block,Action action)
        {
            Vector3 pos = block.GetPosition();
            int x = (int)pos.x;
            int z = (int)pos.z;

            int sizeX = (int)block.info.districtAreaMax.x;
            int sizeZ = (int)block.info.districtAreaMax.y;

            for (int indexX = x; indexX < x + sizeX; indexX++)
            {
                for(int indexZ = z; indexZ < z + sizeZ; indexZ++)
                {
                    if (action == Action.ADD)
                    {
                        instanceNodes[indexX, indexZ] = block.instanceID;

                    }
                    else if (action == Action.REMOVE)
                    {
                        if (instanceNodes[indexX, indexZ] == block.instanceID)
                        {
                            instanceNodes[indexX, indexZ] = -1;
                            
                        }
                    }
                }
             
            }


        }


        /// <summary>
        /// 区域是否可放置
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="sizeX"></param>
        /// <param name="sizeZ"></param>
        /// <param name="instanceID"></param>
        /// <returns></returns>
        public bool PositionCanPlace(Vector3 pos,int sizeX,int sizeZ,int instanceID)
        {
            int posx = (int)pos.x;
            int posz = (int)pos.z;

            for (int x = posx; x < posx + sizeX; x++)
            {
                for (int z = posz; z < posz + sizeZ; z++)
                {
                    if (x < 0 || x>=nodeWidth || z<0 || z >= nodeLength)
                    {
                        //Out of Range
                        return false;
                    }

                    if(instanceNodes[x,z] !=-1 && instanceNodes[x,z] != instanceID)
                    {
                        return false;
                    }
                }

            }


            return true;
        }


    }
}