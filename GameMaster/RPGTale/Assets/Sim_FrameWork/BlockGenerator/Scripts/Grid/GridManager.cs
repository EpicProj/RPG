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

        private Dictionary<Vector2, ShipMapGridInfo> _gridInfo;
        public Dictionary<Vector2,ShipMapGridInfo> GridInfo
        {
            get { return _gridInfo; }
        }

        protected override void Awake()
        {
            base.Awake();
            _gridInfo = new Dictionary<Vector2, ShipMapGridInfo>();
        }

        public bool showNodes;

        private ushort nodeWidth;
        private ushort nodeLength;


        void OnDrawGizmos()
        {
            if (!showNodes)
                return;
        }

        public void InitMapGrid(ushort length,ushort width)
        {
            nodeWidth = width;
            nodeLength = length;
            InitAllNodes();
        }

        public ShipMapGridInfo GetGridInfoByCoordinate(Vector2 v)
        {
            ShipMapGridInfo info = null;
            _gridInfo.TryGetValue(v, out info);
            return info;
        }

        /// <summary>
        /// 初始化格
        /// </summary>
        public void InitAllNodes()
        {
            for(int x = 0; x < nodeLength; x++)
            {
                for(int z = 0; z < nodeWidth; z++)
                {
                    Vector2 v2 = new Vector2(x, z);
                    _gridInfo.Add(v2, new ShipMapGridInfo
                    {
                        blockInstanceID = 0,
                        canPlace = true,
                        coordinate = v2,
                        isBarrier = false
                    });
                }
            }

            foreach( KeyValuePair<uint,FunctionBlockBase> kvp in FunctionBlockManager.Instance.GetBlockInstancesDic())
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

            int sizeX = (int)block.info.districtInfo.size.x;
            int sizeZ = (int)block.info.districtInfo.size.y;

            for (int indexX = x; indexX < x + sizeX; indexX++)
            {
                for(int indexZ = z; indexZ < z + sizeZ; indexZ++)
                {
                    if (action == Action.ADD)
                    {
                        var info = GetGridInfoByCoordinate(new Vector2(indexX, indexZ));
                        if (info != null)
                        {
                            info.blockInstanceID = block.instanceID;
                            info.canPlace = false;
                        }
                    }
                    else if (action == Action.REMOVE)
                    {
                        var info = GetGridInfoByCoordinate(new Vector2(indexX, indexZ));
                        if (info != null)
                        {
                            if (info.blockInstanceID == block.instanceID)
                            {
                                info.blockInstanceID = 0;
                                info.canPlace = true;
                            }
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
                    var info = GetGridInfoByCoordinate(new Vector2(x, z));
                    if (info != null)
                    {
                        if (info.canPlace == false)
                            return false;
                    }
                }
            }
            return true;
        }
    }


    public class ShipMapGridInfo
    {
        public Vector2 coordinate;

        public bool isBarrier;
        public bool canPlace;
        public uint blockInstanceID;
    }
}