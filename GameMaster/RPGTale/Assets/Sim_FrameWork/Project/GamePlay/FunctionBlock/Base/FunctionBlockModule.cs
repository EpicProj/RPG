using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sim_FrameWork {
    public class FunctionBlockModule : BaseModule<FunctionBlockModule> {

        /// <summary>
        /// 格子类型
        /// </summary>
        public enum FunctionBlockManuMaterialType
        {
            Input,
            Output,
            Byproduct
        }
        protected static Dictionary<int, FunctionBlock> FunctionBlockDic;
        protected static Dictionary<string, FunctionBlockTypeData> FunctionBlockTypeDataDic;


        #region Data

        public override void InitData()
        {
            var config = ConfigManager.Instance.LoadData<FunctionBlockMetaData>(ConfigPath.TABLE_FUNCTIONBLOCK_METADATA_PATH);
            if (config == null)
            {
                Debug.LogError("FunctionBlockMetaData Read Error");
                return;
            }
            FunctionBlockDic = config.AllFunctionBlockDic;
            FunctionBlockTypeDataDic = config.AllFunctionBlockTypeDataDic;
        }

        public override void Register()
        {
            
        }

        public FunctionBlockModule()
        {
            InitData();
        }
        #endregion

        #region Type
        private static bool CheckTypeValid(string type)
        {
            if (Enum.IsDefined(typeof(FunctionBlockType), type) == false)
            {
                Debug.LogError("FactoryType InValid! Type=" + type);
                return false;
            }
            return true;
        }

        //Get FunctionBlockType
        public static FunctionBlockType GetFunctionBlockType(int blockID)
        {
            var block = GetFunctionBlockByBlockID(blockID);
            if (CheckTypeValid(block.FunctionBlockType) == false)
                return FunctionBlockType.None;
            return (FunctionBlockType)Enum.Parse(typeof(FunctionBlockType), block.FunctionBlockType);
        }

        public static ModifierDetailRootType_Mix FetchBlockModifierRootType(FunctionBlockType type)
        {
            ModifierDetailRootType_Mix result = ModifierDetailRootType_Mix.None;
            if(!Enum.TryParse<ModifierDetailRootType_Mix>(type.ToString(),out result))
            {
                DebugPlus.LogError("[ModifierDetailRootType_Block] fetchType Error! type= " + type);
            }
            return result;
        }

        //Get Type Data
        public static FunctionBlockTypeData GetFacotryTypeData(FunctionBlockType type)
        {
            FunctionBlockTypeData typeData = null;
            FunctionBlockTypeDataDic.TryGetValue(type.ToString(), out typeData);
            if (typeData != null)
            {
                return typeData;
            }
            else
            {
                Debug.LogError("GetFunctionBlockType Error Type= " + type);
                return null;
            }
        }

        public static FunctionBlockType GetBlockType(FunctionBlockTypeData data)
        {
            FunctionBlockType result = FunctionBlockType.None;
            Enum.TryParse<FunctionBlockType>(data.Type, out result);
            return result;
        }

        public static FunctionBlockTypeData GetFacotryTypeData(int functionBlockID)
        {
            return GetFacotryTypeData(GetFunctionBlockType(functionBlockID));
        }

        public static List<FunctionBlockTypeData> GetInitMainType()
        {
            List<FunctionBlockTypeData> result = new List<FunctionBlockTypeData>();
            foreach(var item in FunctionBlockTypeDataDic.Values)
            {
                if (item.DefaultShow)
                    result.Add(item);
            }
            return result;
        }

        public static Sprite GetMainTypeSprite(FunctionBlockTypeData data)
        {
            return Utility.LoadSprite(data.TypeIcon, Utility.SpriteType.png);
        }
        public static string GetMainTypeName(FunctionBlockTypeData data)
        {
            return MultiLanguage.Instance.GetTextValue(data.TypeName);
        }


        #endregion

        #region Method Data

        public static Sprite GetFunctionBlockIcon(int functionBlockID)
        {
            string path = GetFunctionBlockByBlockID(functionBlockID).BlockIcon;
            return Utility.LoadSprite(path,Utility.SpriteType.png);
         
        }
        public static Sprite GetFunctionBlockBG(int functionBlockID)
        {
            string path = GetFunctionBlockByBlockID(functionBlockID).BlockBG;
            return Utility.LoadSprite(path, Utility.SpriteType.png);
        }

        public static Sprite GetFunctionBlockTypeIcon(int functionBlockID)
        {
            var type = GetFunctionBlockType(functionBlockID);
            if(type!= FunctionBlockType.None)
            {
                var typedata = GetFacotryTypeData(type);
                if (typedata != null)
                    return Utility.LoadSprite(typedata.TypeIcon, Utility.SpriteType.png);
            }
            return null;
        }

        public static FunctionBlock GetFunctionBlockByBlockID(int functionBlockID)
        {
            FunctionBlock functionBlock = null;
            FunctionBlockDic.TryGetValue(functionBlockID, out functionBlock);
            if (functionBlock == null)
                Debug.LogError("Get FunctionBlock Error , ID=" + functionBlockID);
            return functionBlock;
        }

        public static string GetFunctionBlockName(int functionBlockID)
        {
            return MultiLanguage.Instance.GetTextValue(GetFunctionBlockByBlockID(functionBlockID).BlockName);
        }
        public static string GetFunctionBlockDesc(int functionBlockID)
        {
            return MultiLanguage.Instance.GetTextValue(GetFunctionBlockByBlockID(functionBlockID).BlockDesc);
        }


        public static int GetDistrictAreaIndex(Vector2 areaMax, Vector2 currentVector)
        {
            return (int)(currentVector.x * areaMax.x + currentVector.y);
        }

        /// <summary>
        /// Get DistrictConfig
        /// </summary>
        /// <param name="blockID"></param>
        /// <returns></returns>
        public static Config.Block_District_Config GetBlockDistrictConfig(int blockID)
        {
            var block = GetFunctionBlockByBlockID(blockID);
            if (block != null)
            {
                var config = Config.ConfigData.BlockConfigData.configData.Find(x => x.configName == block.BlockConfig);
                if (config != null)
                {
                    if (config.hasDistrict)
                        return config.districtConfig;
                    else
                        DebugPlus.LogError("[Block District Config] : block has no district ,but still try get it blockID=" + blockID);
                }
            }
            DebugPlus.LogError("[Block District Config] : Find District Config Error! blockid=" + blockID);
            return null;
        }


        /// <summary>
        /// 生成所有初始区划信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="block"></param>
        /// <returns></returns>
        public static Dictionary<Vector2, DistrictAreaInfo> GetBlockDistictInfo(Config.Block_District_Config config)
        {
            ///Config already Check! not deal exception!
            Dictionary<Vector2, DistrictAreaInfo> result = new Dictionary<Vector2, DistrictAreaInfo>();
            if (config != null)
            {
                for(int i = 0; i < config.gridConfig.Count; i++)
                {
                    Vector2 pos = new Vector2(config.gridConfig[i].coordinate[0], config.gridConfig[i].coordinate[1]);
                    DistrictAreaInfo info = new DistrictAreaInfo();
                    info = info.InitData(config.gridConfig[i]);

                    result.Add(pos, info);
                }
            }
            return result;
        }
     
        #endregion

        #region BlockInfoData

        public static List<FormulaData> GetBlockFormulaList(int blockID)
        {
            List<FormulaData> result = new List<FormulaData>();
            var block = GetFunctionBlockByBlockID(blockID);
            if (block != null)
            {
                var config = Config.ConfigData.BlockConfigData.configData.Find(x => x.configName == block.BlockName);
                if (config == null)
                    return null;
                if (config.manuConfig != null)
                {
                    for (int i = 0; i < config.manuConfig.formulaIDList.Count; i++)
                    {
                        var formulaData = FormulaModule.GetFormulaDataByID(config.manuConfig.formulaIDList[i]);
                        if (formulaData != null)
                            result.Add(formulaData);                  
                    }
                    return result;
                }
            }
            return null;
        }

        public static List<int> GetBlockEXPMapData(int blockid)
        {
            var block = GetFunctionBlockByBlockID(blockid);
            if (block != null)
            {
                var config = Config.ConfigData.BlockConfigData.configData.Find(x => x.configName == block.BlockConfig);
                if (config != null)
                    return config.levelConfig.EXPMap;
            }
            return null;
        }

        public static int GetCurrentLevelEXP(List<int> expMap ,int currentLevel)
        {
            if (expMap == null)
            {
                Debug.LogError("EXPMAP IS NULL");
                return 0;
            }
               
            if (currentLevel > expMap.Count)
            {
                Debug.LogError("EXP data not found , index out of range, currentLevel=" + currentLevel + "expMap count=" + expMap.Count);
                return 0;
            }
            else
            {
                return expMap[currentLevel - 1];
            }
        }

        public static List<Config.BlockDistrictUnlockData.DistrictUnlockData> GetBlockDistrictUnlockData(int blockID)
        {
            var block = GetFunctionBlockByBlockID(blockID);
            if (block != null)
            {
                var config = Config.ConfigData.BlockConfigData.configData.Find(x => x.configName == block.BlockConfig);
                if (config != null)
                    return config.levelConfig.districtUnlock.UnlockData;
            }
            return null;
        }

        public static List<BlockNormalInfoData.BlockModifier> GetBlockEffectList(int blockID)
        {
            List<BlockNormalInfoData.BlockModifier> result = new List<BlockNormalInfoData.BlockModifier>();
            var block = GetFunctionBlockByBlockID(blockID);
            if (block != null)
            {
                var config = Config.ConfigData.BlockConfigData.configData.Find(x=>x.configName==block.BlockConfig);
                if (config == null)
                    return result;
                if (config.effectConfig != null)
                {
                    for(int i = 0; i < config.effectConfig.Count; i++)
                    {
                        BlockNormalInfoData.BlockModifier modifier = new BlockNormalInfoData.BlockModifier
                        {
                            iconPath = config.effectConfig[i].iconPath,
                            isDestory = false,
                            modiferBase = ModifierManager.Instance.GetModifierBase(config.effectConfig[i].modifierName)
                        };
                        result.Add(modifier);
                    }
                }
            }
            return result;
        }

        #endregion
      
    }

    public class FunctionBlockHistory
    {
        public string FunctionBlockGUID;
        public int FacotoryID;
        public string BuildDate;

        public FunctionBlockHistory(FunctionBlockInfoData blockInfo ,string buildData)
        {
            FacotoryID = blockInfo.BlockID;
            FunctionBlockGUID = blockInfo.dataModel.GUID;
            this.BuildDate = buildData;

        }

    }
}