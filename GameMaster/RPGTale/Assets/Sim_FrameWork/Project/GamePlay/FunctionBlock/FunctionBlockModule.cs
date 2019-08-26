using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sim_FrameWork {
    public class FunctionBlockModule : BaseModule<FunctionBlockModule> {

        public enum FunctionBlockType
        {
            Manufacture,
            Raw,
            Science,
            Energy
        }
        //Slot类型
        public enum FunctionBlockManuMaterialType
        {
            Input,
            Output,
            Byproduct
        }


        public List<FunctionBlock> FunctionBlockList=new List<FunctionBlock> ();
        public Dictionary<int, FunctionBlock> FunctionBlockDic=new Dictionary<int, FunctionBlock> ();

        public List<FunctionBlock_Raw> FunctionBlock_RawList=new List<FunctionBlock_Raw> ();
        public Dictionary<int, FunctionBlock_Raw> FunctionBlock_RawDic = new Dictionary<int, FunctionBlock_Raw>();
        public List<FunctionBlock_Manufacture> FunctionBlock_ManufactureList = new List<FunctionBlock_Manufacture>();
        public Dictionary<int, FunctionBlock_Manufacture> FunctionBlock_ManufactureDic = new Dictionary<int, FunctionBlock_Manufacture>();
        public List<FunctionBlock_Science> FunctionBlock_ScienceList = new List<FunctionBlock_Science>();
        public Dictionary<int, FunctionBlock_Science> FunctionBlock_ScienceDic = new Dictionary<int, FunctionBlock_Science>();
        public List<FunctionBlock_Energy> FunctionBlock_EnergyList = new List<FunctionBlock_Energy>();
        public Dictionary<int, FunctionBlock_Energy> FunctionBlock_EnergyDic = new Dictionary<int, FunctionBlock_Energy>();

        public List<FunctionBlockTypeData> FunctionBlockTypeDataList=new List<FunctionBlockTypeData> ();
        public Dictionary<string, FunctionBlockTypeData> FunctionBlockTypeDataDic = new Dictionary<string, FunctionBlockTypeData>();


        private bool HasInit = false;
        public string FacotryGUID;
        public List<string> FunctionBlockGUIDList = new List<string>();
        public Dictionary<int, FunctionBlockHistory> FunctionBlockHistoryDic = new Dictionary<int, FunctionBlockHistory>();

        public Dictionary<string, FunctionBlock> CurrentFunctionBlockDataDic = new Dictionary<string, FunctionBlock>();


        #region Data
        public override void InitData()
        {
            if (HasInit)
                return;
            FunctionBlockList = FunctionBlockMetaDataReader.GetFunctionBlockData();
            FunctionBlockDic = FunctionBlockMetaDataReader.GetFunctionBlockDataDic();
            FunctionBlock_RawList = FunctionBlockMetaDataReader.GetFunctionBlockRowData();
            FunctionBlock_RawDic = FunctionBlockMetaDataReader.GetFunctionBlock_RawDic();
            FunctionBlock_ManufactureList = FunctionBlockMetaDataReader.GetFunctionBlock_ManufactureData();
            FunctionBlock_ManufactureDic = FunctionBlockMetaDataReader.GetFunctionBlock_ManufactureDic();
            FunctionBlock_ScienceList = FunctionBlockMetaDataReader.GetFunctionBlock_ScienceData();
            FunctionBlock_ScienceDic = FunctionBlockMetaDataReader.GetFunctionBlock_ScienceDic();
            FunctionBlock_EnergyList = FunctionBlockMetaDataReader.GetFunctionBlock_EnergyData();
            FunctionBlock_EnergyDic = FunctionBlockMetaDataReader.GetFunctionBlock_EnergyDic();

            FunctionBlockTypeDataList = FunctionBlockMetaDataReader.GetFunctionBlockTypeData();
            FunctionBlockTypeDataDic = FunctionBlockMetaDataReader.GetFunctionBlockTypeDataDic();

            HasInit = true;
        }



        private bool CheckTypeValid(string type)
        {
            if (Enum.IsDefined(typeof(FunctionBlockType), type) == false)
            {
                Debug.LogError("FacotyType InValid! Type=" + type);
                return false;
            }
            return true;
        }

        #endregion
        #region Method Data

        //Get FunctionBlockType
        public FunctionBlockType GetFunctionBlockType(int facotryID)
        {
            FunctionBlock functionBlock = GetFunctionBlockByBlockID(facotryID);
            if (CheckTypeValid(functionBlock.FunctionBlockType) == false)
                return FunctionBlockType.Energy;
            return (FunctionBlockType)Enum.Parse(typeof(FunctionBlockType), functionBlock.FunctionBlockType);
        }

        //Get Type Data
        public FunctionBlockTypeData GetFacotryTypeData(FunctionBlockType type)
        {
            FunctionBlockTypeData typeData = null;
            FunctionBlockTypeDataDic.TryGetValue(type.ToString(), out typeData);
            if(typeData != null)
            {
                return typeData;
            }
            else
            {
                Debug.LogError("GetFunctionBlockType Error Type= " + type);
                return null;
            }
        }
        public FunctionBlockTypeData GetFacotryTypeData(int functionBlockID)
        {
            return GetFacotryTypeData(GetFunctionBlockType(functionBlockID));
        }

        public T FetchFunctionBlockTypeIndex<T>(int functionBlockID) where T:class
        {
            switch (GetFunctionBlockType(functionBlockID))
            {
                case FunctionBlockType.Manufacture:
                    return GetFunctionBlock_ManufactureData(GetFunctionBlockByBlockID(functionBlockID).FunctionBlockTypeIndex) as T;
                case FunctionBlockType.Raw:
                    return GetFacotryRawData(GetFunctionBlockByBlockID(functionBlockID).FunctionBlockTypeIndex) as T;
                case FunctionBlockType.Science:
                    return GetFunctionBlock_ScienceData(GetFunctionBlockByBlockID(functionBlockID).FunctionBlockTypeIndex) as T;
                case FunctionBlockType.Energy:
                    return GetFunctionBlock_EnergyData(GetFunctionBlockByBlockID(functionBlockID).FunctionBlockTypeIndex) as T;
                default:
                    Debug.LogError("Fetch FacotryType Error facotryID=" + functionBlockID);
                    return null;
            }
        }

        public FunctionBlock_Manufacture GetFunctionBlock_ManufactureData(int manufactureID)
        {
            FunctionBlock_Manufacture functionBlock_Manufacture = null;
            FunctionBlock_ManufactureDic.TryGetValue(manufactureID, out functionBlock_Manufacture);
            if (functionBlock_Manufacture == null)
            {
                Debug.LogError("Get functionBlock_Manufacture Error , manufactureId=" + manufactureID);
                return null;
            }
            return functionBlock_Manufacture;
        }

        public FunctionBlock_Raw GetFacotryRawData(int rawID)
        {
            FunctionBlock_Raw functionBlock_Raw = null;
            FunctionBlock_RawDic.TryGetValue(rawID, out functionBlock_Raw);
            if (functionBlock_Raw == null)
            {
                Debug.LogError("Get FunctionBlock_Raw Error , rawID=" + rawID);
                return null;
            }
            return functionBlock_Raw;
        }

        public FunctionBlock_Science GetFunctionBlock_ScienceData(int scienceID)
        {
            FunctionBlock_Science functionBlock_Science = null;
            FunctionBlock_ScienceDic.TryGetValue(scienceID, out functionBlock_Science);
            if (functionBlock_Science == null)
            {
                Debug.LogError("Get FunctionBlock_Raw Error , scienceID=" + scienceID);
                return null;
            }
            return functionBlock_Science;
        }

        public FunctionBlock_Energy GetFunctionBlock_EnergyData(int energyID)
        {
            FunctionBlock_Energy functionBlock_Energy = null;
            FunctionBlock_EnergyDic.TryGetValue(energyID, out functionBlock_Energy);
            if (functionBlock_Energy == null)
            {
                Debug.LogError("Get FunctionBlock_Raw Error , energyID=" + energyID);
                return null;
            }
            return functionBlock_Energy;
        }

        public Sprite GetFunctionBlockIcon(int functionBlockID)
        {
            string path = GetFunctionBlockByBlockID(functionBlockID).FunctionBlockIcon;
            return Utility.LoadSprite(path,Utility.SpriteType.png);
         
        }

        public ushort GetFunctionBlockLevel(int functionBlockID)
        {
            return GetFunctionBlockByBlockID(functionBlockID).Level;
        }
        public float GetManufactureSpeed(int functionBlockID)
        {
            return FetchFunctionBlockTypeIndex<FunctionBlock_Manufacture>(functionBlockID).SpeedBase;
        }

        public FunctionBlock GetFunctionBlockByBlockID(int functionBlockID)
        {
            FunctionBlock functionBlock = null;
            FunctionBlockDic.TryGetValue(functionBlockID, out functionBlock);
            if (functionBlock == null)
                Debug.LogError("Get FunctionBlock Error , ID=" + functionBlockID);
            return functionBlock;
        }

        public string GetFunctionBlockName(int functionBlockID)
        {
            return MultiLanguage.Instance.GetTextValue(GetFunctionBlockByBlockID(functionBlockID).FunctionBlockName);
        }
        public string GetFunctionBlockName(FunctionBlock block)
        {
            return MultiLanguage.Instance.GetTextValue(block.FunctionBlockName);
        }
        public string GetFunctionBlockDesc(int functionBlockID)
        {
            return MultiLanguage.Instance.GetTextValue(GetFunctionBlockByBlockID(functionBlockID).FunctionBlockDesc);
        }
        public string GetFunctionBlockDesc(FunctionBlock block)
        {
            return MultiLanguage.Instance.GetTextValue(block.FunctionBlockDesc);
        }

        public Vector2 GetFunctionBlockAreaMax<T>(FunctionBlock block) where T:class
        {
            int id = block.FunctionBlockID;
            switch (GetFunctionBlockType(id))
            {
                case FunctionBlockType.Manufacture:
                    return Utility.TryParseIntVector2(GetFunctionBlock_ManufactureData(GetFunctionBlockByBlockID(id).FunctionBlockTypeIndex).AreaMax,',');
                case FunctionBlockType.Raw:
                    return Utility.TryParseIntVector2(GetFacotryRawData(GetFunctionBlockByBlockID(id).FunctionBlockTypeIndex).AreaMax,',');
                case FunctionBlockType.Science:
                    return Utility.TryParseIntVector2(GetFunctionBlock_ScienceData(GetFunctionBlockByBlockID(id).FunctionBlockTypeIndex).AreaMax,',');
                case FunctionBlockType.Energy:
                    return Utility.TryParseIntVector2(GetFunctionBlock_EnergyData(GetFunctionBlockByBlockID(id).FunctionBlockTypeIndex).AreaMax,',');
                default:
                    Debug.LogError("Fetch AreaMax Error BlockID=" + id);
                    return Vector2.zero;
            }
        }

        /// <summary>
        /// ID=-1  Empty  ;  ID=-2   UnLock;
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="block"></param>
        /// <returns></returns>
        public Dictionary<Vector2, DistrictAreaInfo> GetFuntionBlockAreaDetailDefaultData<T>(FunctionBlock block) where T : class
        {
            Dictionary<Vector2, DistrictAreaInfo> result = new Dictionary<Vector2, DistrictAreaInfo>();
            Dictionary<Vector2, DistrictData> inPutDic = GetFuntionBlockAreaDetailDefaultData(GetFuntionBlockAreaDetailDefault<T>(block));
            //Check Area
            int largeDistrictindex = 0;
            Dictionary<int, List<Vector2>> largeDistrictDic = new Dictionary<int, List<Vector2>>();
            foreach (KeyValuePair<Vector2, DistrictData> kvp in inPutDic)
            {
                bool b = false;
                if (kvp.Value == null || kvp.Value.DistrictID == -1 || kvp.Value.DistrictID == -2)
                {
                    //empty 
                    DistrictAreaInfo info = new DistrictAreaInfo
                    {
                        data = kvp.Value,
                        isLargeDistrict = false
                    };
                    result.Add(kvp.Key, info);
                    continue;
                }
                //Check exists District
                foreach (KeyValuePair<int,List<Vector2>> value in largeDistrictDic)
                {
                    if (value.Value.Contains(kvp.Key))
                    {
                        //Skip
                        DistrictAreaInfo info = new DistrictAreaInfo
                        {
                            data = kvp.Value,
                            isLargeDistrict = true,
                            largeDistrictIndex = value.Value
                        };
                        result.Add(kvp.Key, info);
                        b = true;
                        break;
                    }
                }
                if (b)
                    continue;

                //District Larger than 1X1
                Vector2 area = DistrictModule.Instance.GetDistrictArea(kvp.Value);
                if (area.x == 1 && area.y == 1)
                {
                    DistrictAreaInfo info = new DistrictAreaInfo
                    {
                        data = kvp.Value,
                        isLargeDistrict = false
                    };
                    result.Add(kvp.Key, info);
                    continue;
                }
                else if ((area.x != 1 || area.y != 1) && area.y + kvp.Key.y < GetFunctionBlockAreaMax<T>(block).y && kvp.Key.x+area.x<GetFunctionBlockAreaMax<T>(block).x) //Loop 
                {
                    //Add Area List
                    List<Vector2> districtArea = new List<Vector2>();
                    for (int i = 0; i < area.x; i++)
                    {
                        for (int j = 0; j < area.y; j++)
                        {
                            Vector2 index = new Vector2(kvp.Key.x + i, kvp.Key.y + j);
                            districtArea.Add(index);
                           
                        }
                    }
                    largeDistrictDic.Add(largeDistrictindex, districtArea);
                    largeDistrictindex++;
                    DistrictAreaInfo info = new DistrictAreaInfo
                    {
                        data = kvp.Value,
                        isLargeDistrict = true,
                        largeDistrictIndex = districtArea
                    };
                    result.Add(kvp.Key, info);
                }
                else
                {
                    Debug.LogError("DistrictData Area Error ,ID=" + kvp.Value.DistrictID);
                    continue;
                }
            }
            return result;

        }

        private Dictionary<Vector2, DistrictData> GetFuntionBlockAreaDetailDefaultData(Dictionary<Vector2, int> dic)
        {

            Dictionary<Vector2, DistrictData> result = new Dictionary<Vector2, DistrictData>();
            if (dic == null)
                return result;
            foreach(KeyValuePair<Vector2, int> kvp in dic)
            {
                if(kvp.Value == -1)
                {
                    //AddEmptySlot
                    result.Add( kvp.Key,new DistrictData { DistrictID = -1 });
                    continue;
                }else if(kvp.Value == -2)
                {
                    //Add UnLockSlot
                    result.Add(kvp.Key, new DistrictData { DistrictID = -2 });
                    continue;
                }
                DistrictData dd = DistrictModule.Instance.GetDistrictDataByKey(kvp.Value);
                if (dd == null)
                    continue;
                result.Add(kvp.Key, dd);
            }
            return result;

        }


        private Dictionary<Vector2, int> GetFuntionBlockAreaDetailDefault<T>(FunctionBlock block) where T : class
        {
            int id = block.FunctionBlockID;
            switch (GetFunctionBlockType(id))
            {
                case FunctionBlockType.Manufacture:
                    return TryParseAreaDetailDefault(GetFunctionBlock_ManufactureData(GetFunctionBlockByBlockID(id).FunctionBlockTypeIndex).AreaDetailDefault, GetFunctionBlockAreaMax<T>(block));
                case FunctionBlockType.Energy:
                    return TryParseAreaDetailDefault(GetFunctionBlock_EnergyData(GetFunctionBlockByBlockID(id).FunctionBlockTypeIndex).AreaDetailDefault, GetFunctionBlockAreaMax<T>(block));
                default:
                    Debug.LogError("Fetch AreaDetailDefault Error   id="+id);
                    return null;
            }
        }

        private Dictionary<Vector2, int> TryParseAreaDetailDefault(string content ,Vector2 areaMax)
        {
            Dictionary<Vector2, int> result = new Dictionary<Vector2, int> ();
            List<string> ls = Utility.TryParseStringList(content, ';');
            if(string.IsNullOrEmpty(content) || ls.Count == 0)
            {
                Debug.LogWarning("Parse AreaDetailFail!");
                return result;
            }
            if (ls.Count != (int)areaMax.y)
            {
                Debug.LogError("District Area Not Match content=" + content + "AreaMax=" + areaMax);
            }
            for (int i = 0; i < ls.Count; i++)
            {
                List<int> li = Utility.TryParseIntList(ls[i], ',');
                if (li.Count != (int)areaMax.x)
                {
                    Debug.LogError("District Area Not Match content=" + content + "AreaMax=" + areaMax);
                }
                for(int j = 0; j < li.Count; j++)
                {
                    Vector2 pos = new Vector2(i, j);
                    result.Add(pos, li[j]);
                }
            }
            return result;
        }


        #endregion

        #region Main Function

        //Place Block
        public void PlaceFunctionBlock(int functionBlockID,Vector3 checkpos)
        {
            FunctionBlock currentFunctionBlock = GetFunctionBlockByBlockID(functionBlockID);
            if (currentFunctionBlock == null)
                return;
            if (CheckBlockCanPlace(currentFunctionBlock,checkpos) )
            {
                string tempUID=GenerateGUID();
                AddFunctionBlock(tempUID, currentFunctionBlock);
            }
            else
            {
                Debug.Log("Can not place functionBlock ");
            }
           


            //FunctionBlockHistory history = new FunctionBlockHistory(1, functionBlockID);

            //FunctionBlockHistoryDic.Add(functionBlockID, history);
        }

        public void AddFunctionBlock(string functionBlockUID,FunctionBlock functionBlock)
        {
            if (FunctionBlockGUIDList.Contains(functionBlockUID))
            {
                GenerateGUID();
            }
            CurrentFunctionBlockDataDic.Add(functionBlockUID, functionBlock);
            FunctionBlockGUIDList.Add(functionBlockUID);
        }

        public string GenerateGUID()
        {
            return Guid.NewGuid().ToString(); 
        }


        public bool CheckBlockCanPlace(FunctionBlock block,Vector3 checkPos)
        {
            //Todo
            return true;
        }


        public void InitDistrictBlockModel<T>(FunctionBlock block) where T:class
        {
            Dictionary<Vector2, DistrictAreaInfo> districtDic = GetFuntionBlockAreaDetailDefaultData<T>(block);
            if (districtDic == null)
                return;
            
            foreach(KeyValuePair<Vector2, DistrictAreaInfo> kvp in districtDic)
            {
                var data = kvp.Value.data;
                if(data==null || data.DistrictID == -1 || data.DistrictID == -2)
                {
                    //empty Model
                    continue;
                }


                Vector2 districtArea = DistrictModule.Instance.GetDistrictArea(data);
                if(districtArea.x==1 && districtArea.y == 1)
                {
                    //Init Block 1X1
                    string ModelPath = "Assets/Prefabs/District/" + data.ModelPath;
                    GameObject DistrictModel = ObjectManager.Instance.InstantiateObject(ModelPath);
                }
            }

          
        }

        /// <summary>
        /// Init Block Box Collider
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="block"></param>
        /// <returns></returns>
        public Vector3 InitFunctionBlockBoxCollider<T>(FunctionBlock block) where T:class
        {
            Vector2 area = GetFunctionBlockAreaMax<T>(block);
            return new Vector3(area.x, 3.0f, area.y);
        }

        //Formula
        public List<Dictionary<Material,ushort>> GetFunctionBlockFormulaDataList(FunctionBlock block,FormulaModule.MaterialProductType GetType)
        {
            List<Dictionary<Material, ushort>> result = new List<Dictionary<Material, ushort>>();

            List<FormulaData> data= FormulaModule.Instance.GetFormulaDataList(FetchFunctionBlockTypeIndex<FunctionBlock_Manufacture>(block.FunctionBlockID).FormulaInfoID);
            for(int i = 0; i < data.Count; i++)
            {
                Dictionary<Material, ushort> maDic = FormulaModule.Instance.GetFormulaMaterialDic(data[i].FormulaID, GetType);
                result.Add(maDic);
            }
            return result;
        }


   
   

        #endregion
    }

    public class FunctionBlockHistory
    {
        public int FunctionBlockGUID { get; set; }
        public int FacotoryID { get; set; }

        public FunctionBlockHistory(int guid,int functionBlockID)
        {
            this.FacotoryID = functionBlockID;
            this.FunctionBlockGUID = guid;
        }

    }

    //BlockLevelData
    public class BlockLevelData
    {
        public int LevelIndex;
        public int NeedEXP;
    }


}