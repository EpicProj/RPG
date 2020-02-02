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

    public enum ModifierDetailRootType_Mix
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

        #region Assemble
        Assemble_Engine,
        Assemble_Shield,
        #endregion
    }

    /// <summary>
    /// Detail Package
    /// </summary>
    public class ModifierDetailPackage
    {
        public Dictionary<ModifierDetailRootType_Simple, ModifierDetailItem_Simple> detailDic = new Dictionary<ModifierDetailRootType_Simple, ModifierDetailItem_Simple>();
        public void ValueChange(ModifierDetailRootType_Simple rootType, float num)
        {
            if (num == 0)
                return;
            if (detailDic.ContainsKey(rootType))
            {
                detailDic[rootType].value += num;
                if (detailDic[rootType].value == 0)
                {
                    //Remove
                    detailDic.Remove(rootType);
                }
            }
            else
            {
                ModifierDetailItem_Simple item = new ModifierDetailItem_Simple(rootType, num);
                detailDic.Add(rootType, item);
            }
        }
    }

    public class ModifierDetailPackage_Mix
    {
        public Dictionary<ModifierDetailRootType_Mix, ModifierDetailItem_Mix> detailDic = new Dictionary<ModifierDetailRootType_Mix, ModifierDetailItem_Mix>();
        public void ValueChange_Block(ModifierDetailRootType_Mix rootType, uint instanceID, int blockID, float num)
        {
            if (num == 0)
                return;
            if (detailDic.ContainsKey(rootType))
            {
                detailDic[rootType].value += num;
                if (detailDic[rootType].value == 0)
                {
                    //Remove
                    detailDic.Remove(rootType);
                }
            }
            else
            {
                ModifierDetailItem_Mix item = new ModifierDetailItem_Mix(rootType, ModifierPackType.Block,instanceID,blockID,num);
                detailDic.Add(rootType, item);
            }
        }

        public void ValueChange_Assemble(ModifierDetailRootType_Mix rootType, ushort UID, int partID, float num)
        {
            if (num == 0)
                return;
            if (detailDic.ContainsKey(rootType))
            {
                detailDic[rootType].value += num;
                if (detailDic[rootType].value == 0)
                {
                    //Remove
                    detailDic.Remove(rootType);
                }
            }
            else
            {
                ModifierDetailItem_Mix item = new ModifierDetailItem_Mix(rootType, ModifierPackType.Assemble, UID, partID, num);
                detailDic.Add(rootType, item);
            }
        }

        public void ValueChange(ModifierDetailRootType_Mix rootType,float num)
        {
            if (num == 0)
                return;
            if (detailDic.ContainsKey(rootType))
            {
                detailDic[rootType].value += num;
                if (detailDic[rootType].value == 0)
                {
                    //Remove
                    detailDic.Remove(rootType);
                }
            }
            else
            {
                ModifierDetailItem_Mix item = new ModifierDetailItem_Mix(rootType, ModifierPackType.Normal, num);
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

    public enum ModifierPackType
    {
        Block,
        Assemble,
        Normal
    }
    public class ModifierDetailItem_Mix
    {
        public ModifierDetailRootType_Mix blockType;
        public ModifierPackType packType;
        public uint uid;
        public int typeID;
        public float value;

        public ModifierDetailItem_Mix(ModifierDetailRootType_Mix rootType, ModifierPackType packType,uint uid, int typeID,float value)
        {
            this.blockType = rootType;
            this.uid = uid;
            this.typeID = typeID;
            this.value = value;
        }

        public ModifierDetailItem_Mix(ModifierDetailRootType_Mix rootType, ModifierPackType packType, float value)
        {
            this.packType = packType;
            this.blockType = rootType;
            this.value = value;
        }
    }



}