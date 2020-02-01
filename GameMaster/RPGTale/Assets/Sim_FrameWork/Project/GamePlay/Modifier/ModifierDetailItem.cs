using System.Collections;
using System.Collections.Generic;

namespace Sim_FrameWork
{
    public enum ModifierDetailRootType_Simple
    {
        /// <summary>
        /// 初始化资源
        /// </summary>
        OriginConfig,

        /// <summary>
        /// MainShipArea
        /// </summary>
        PowerArea,
        LivingArea,
        WorkingArea,
        Hangar,
        ControlTower,

    }

    public enum ModifierDetailRootType_Block
    {
        OriginConfig,

        None,
        /// 储能单元
        EnergyStorageUnit,
        /// 反应舱
        ReactionTank,
        /// 湮灭反应堆
        AnnihilationReactor,
        /// 冷却固化堆
        CooledReactor,
        /// 过载缓存引擎
        OverloadEngine,
        /// 共振分束器
        ResonanceBeamSplitter,
    }

    /// <summary>
    /// Detail Package
    /// </summary>
    public class ModifierDetailPackage
    {
        public Dictionary<ModifierDetailRootType_Simple, ModifierDetailItem_Simple> detailDic = new Dictionary<ModifierDetailRootType_Simple, ModifierDetailItem_Simple>();
        public void ValueChange(ModifierDetailRootType_Simple rootType, float num)
        {
            if (detailDic.ContainsKey(rootType))
            {
                detailDic[rootType].value += num;
            }
            else
            {
                ModifierDetailItem_Simple item = new ModifierDetailItem_Simple(rootType, num);
                detailDic.Add(rootType, item);
            }
        }
    }

    public class ModifierDetailPackage_Block
    {
        public Dictionary<ModifierDetailRootType_Block, ModifierDetailItem_Block> detailDic = new Dictionary<ModifierDetailRootType_Block, ModifierDetailItem_Block>();
        public void ValueChange(ModifierDetailRootType_Block rootType, uint instanceID, int blockID, float num)
        {
            if (detailDic.ContainsKey(rootType))
            {
                detailDic[rootType].value += num;
            }
            else
            {
                ModifierDetailItem_Block item = new ModifierDetailItem_Block(rootType,instanceID,blockID,num);
                detailDic.Add(rootType, item);
            }
        }
        public void ValueChange(ModifierDetailRootType_Block rootType,float num)
        {
            if (detailDic.ContainsKey(rootType))
            {
                detailDic[rootType].value += num;
            }
            else
            {
                ModifierDetailItem_Block item = new ModifierDetailItem_Block(rootType, num);
                detailDic.Add(rootType, item);
            }
        }
    }

    public class ModifierDetailItem_Simple
    {
        /// <summary>
        /// 加成来源
        /// </summary>
        public ModifierDetailRootType_Simple rootType;
        public float value;

        public ModifierDetailItem_Simple(ModifierDetailRootType_Simple rootType, float value)
        {
            this.rootType = rootType;
            this.value = value;
        }
    }
    public class ModifierDetailItem_Block
    {
        public ModifierDetailRootType_Block blockType;
        public uint instanceID;
        public int blockID;
        public float value;

        public ModifierDetailItem_Block(ModifierDetailRootType_Block rootType,uint instanceID,int blockID,float value)
        {
            this.blockType = rootType;
            this.instanceID = instanceID;
            this.blockID = blockID;
            this.value = value;
        }

        public ModifierDetailItem_Block(ModifierDetailRootType_Block rootType, float value)
        {
            this.blockType = rootType;
            this.value = value;
        }
    }



}