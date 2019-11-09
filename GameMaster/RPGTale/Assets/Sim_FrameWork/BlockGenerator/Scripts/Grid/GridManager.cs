using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork
{
    public class GridManager : MonoSingleton<GridManager>
    {
        protected override void Awake()
        {
            base.Awake();
        }

        public bool showNodes;

        public int nodeWidth = 50;
        public int nodeLength = 50;

        public int[,] ObjNodes;


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
            ObjNodes = new int[nodeWidth, nodeLength];

            for(int x = 0; x < nodeWidth; x++)
            {
                for(int z = 0; z < nodeLength; z++)
                {
                    /// -1 is empty
                    ObjNodes[x, z] = -1;
                }
            }

            foreach( KeyValuePair<int,FunctionBlockBase> kvp in FunctionBlockManager.Instance.GetBlockInstances())
            {
                FunctionBlockBase block = kvp.Value;
                UpdateFunctionBlockNodes(block);
            }
        }

        /// <summary>
        /// 刷新建筑
        /// </summary>
        /// <param name="block"></param>
        public void UpdateFunctionBlockNodes(FunctionBlockBase block)
        {
            Vector3 pos = block.GetPosition();
            int x = (int)pos.x;
            int z = (int)pos.z;

            var size= block.GetSizeMax();
            int sizeX = (int)size.x;
            int sizeY = (int)size.z;

            for(int indexX = x; indexX < x + sizeX; indexX++)
            {
             
            }


        }



        public bool CanPlace(Vector3 pos,int sizeX,int sizeZ,int ObjID)
        {
            int posx = (int)pos.x;
            int posz = (int)pos.z;

            for (int x = posx; x < posx + sizeX; x++)
            {
                for (int y = posz; y < posz + sizeZ; y++)
                {
                    if (x < 0 || x>=nodeWidth || y<0 || y >= nodeLength)
                    {
                        //Out of Range
                        return false;
                    }
                }

                

            }


            return true;
        }


    }
}