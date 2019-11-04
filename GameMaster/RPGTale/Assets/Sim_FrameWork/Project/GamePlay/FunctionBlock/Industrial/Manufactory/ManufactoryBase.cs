using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork
{

    public class ManufactoryBase : FunctionBlockBase
    {

        public ManufactoryInfo manufactoryInfo;

        /// <summary>
        /// 当前配方
        /// </summary>
        private int _currentFormulaID = 0;
        public int CurrentFormulaID { get { return _currentFormulaID; }  }

        public override void InitData()
        {
            base.InitData();
            manufactoryInfo = new ManufactoryInfo(info.block);
            InitFormula();
        }

        public void InitFormula()
        {
            List<FormulaData> formulaData = FunctionBlockModule.GetFormulaList(info.block);
            if (formulaData.Count == 1)
            {
                manufactoryInfo.formulaInfo = new ManufactFormulaInfo(formulaData[0].FormulaID, info.block);
            }

            foreach(var material in manufactoryInfo.formulaInfo.currentInputMaterialFormulaDic.Keys)
            {
                manufactoryInfo.formulaInfo.realInputDataDic.Add(material, 0);
            }
            foreach(var material in manufactoryInfo.formulaInfo.currentOutputMaterialFormulaDic.Keys)
            {
                manufactoryInfo.formulaInfo.realOutputDataDic.Add(material, 0);
            }
            //获取材料列表
            manufactoryInfo.formulaInfo.currentNeedTime = GetCurrentFormulaNeedTime();
        }

        /// <summary>
        /// 更换配方
        /// </summary>
        /// <param name="formulaID"></param>
        public void ChangeFormula(int formulaID)
        {
            _currentFormulaID = formulaID;
            manufactoryInfo.formulaInfo.RefreshFormulaID(formulaID);
        }

        #region Manufact
        /// <summary>
        /// 增加材料到输入栏
        /// </summary>
        /// <param name="id"></param>
        /// <param name="count"></param>
        /// <param name="info"></param>
        public void AddMaterialToInputSlot(int id,ushort count)
        {
            //是否有效输入
            //需要优化
            Material ma = MaterialModule.GetMaterialByMaterialID(id);
            if (manufactoryInfo.formulaInfo.currentInputMaterialFormulaDic.ContainsKey(ma))
            {
                if (manufactoryInfo.formulaInfo.realInputDataDic[ma] +count > ma.BlockCapacity)
                {
                    return;
                }
                manufactoryInfo.formulaInfo.realInputDataDic[ma] += count;
                StartManufact();
            }
            else
            {
                //非配方
                return;
            }
        }

        public void ReduceInputSlotNum()
        {
           for(int i = 0; i <manufactoryInfo.formulaInfo.inputMaList.Count; i++)
           {
                manufactoryInfo.formulaInfo.realInputDataDic[manufactoryInfo.formulaInfo.inputMaList[i]] -= manufactoryInfo.formulaInfo.currentInputMaterialFormulaDic[manufactoryInfo.formulaInfo.inputMaList[i]];
           }

        }

        public void AddOutputSlotNum()
        {
            for(int i = 0; i < manufactoryInfo.formulaInfo.outputMaList.Count; i++)
            {
                ushort count = manufactoryInfo.formulaInfo.currentOutputMaterialFormulaDic[manufactoryInfo.formulaInfo.outputMaList[i]];
                manufactoryInfo.formulaInfo.realOutputDataDic[manufactoryInfo.formulaInfo.outputMaList[i]] += count;
                //Add to PlayerData
                PlayerManager.Instance.AddMaterialData(manufactoryInfo.formulaInfo.outputMaList[i].MaterialID, count);
            }
        }
        public void StartManufact()
        {
            if (CheckCanproduce() && manufactoryInfo.formulaInfo.inProgress == false)
            {
                manufactoryInfo.formulaInfo.inProgress = true;
                StartCoroutine(WaitManufactFinish());
            }
        }

        public bool CheckCanproduce()
        {
            foreach (KeyValuePair<Material, ushort> kvp in manufactoryInfo.formulaInfo.realInputDataDic)
            {
                if (kvp.Value < manufactoryInfo.formulaInfo.currentInputMaterialFormulaDic[kvp.Key])
                {
                    manufactoryInfo.formulaInfo.inProgress = false;
                    return false;
                }
            }
            return true;
        }


        IEnumerator WaitManufactFinish()
        {
            yield return new WaitForSeconds(GetCurrentFormulaNeedTime());
            manufactoryInfo.formulaInfo.inProgress = false;
            //Reduce Input num
            ReduceInputSlotNum();
            //Add OutPut
            AddOutputSlotNum();
            //UpdateUI
            UIManager.Instance.SendMessageToWnd(UIPath.FUNCTIONBLOCK_INFO_DIALOG, new UIMessage(UIMsgType.UpdateManuSlot, new List<object>(1) { manufactoryInfo.formulaInfo }));

            //UpdateEXP
            info.levelInfo.AddCurrentBlockEXP(manufactoryInfo.formulaInfo.currentFormulaData.EXP);
            UIManager.Instance.SendMessageToWnd(UIPath.FUNCTIONBLOCK_INFO_DIALOG, new UIMessage(UIMsgType.UpdateLevelInfo, new List<object>(1) { info.levelInfo }));

            StartManufact();
        }

        public float GetCurrentFormulaNeedTime()
        {
            float totalTime = manufactoryInfo.formulaInfo.currentFormulaData.ProductSpeed;
            float time = totalTime / manufactoryInfo.CurrentSpeed;
            return time;
        }

        #endregion

    }
    public class ManufactoryInfo
    {

        public ManufactFormulaInfo formulaInfo;
        public FunctionBlock_Industry IndustryData;
        public ManufactoryBaseInfoData.ManufactureInherentLevelData inherentLevelData;

        /// <summary>
        /// current Manu Speed
        /// </summary>
        [SerializeField]
        private float _currentSpeed = 0;
        public float CurrentSpeed { get { return _currentSpeed; } protected  set {  } }

        /// <summary>
        /// WorkerNum
        /// </summary>
        [SerializeField]
        private int _workerNum = 0;
        public int WorkerNum { get { return _workerNum; } protected set { } }

        private float _maintain = 0;
        public float Maintain { get { return _maintain; } protected set { } }

        /// <summary>
        /// 基础电力消耗
        /// </summary>
        [SerializeField]
        private float _energyCostNormal = 0;
        public float EnergyCostNormal { get { return _energyCostNormal; } protected set { } }
        [SerializeField]
        private float _energyCostMagic = 0;
        public float EnergyCostMagic { get { return _energyCostNormal; } protected set { } }


        public void AddCurrentSpeed(float speed)
        {
            _currentSpeed += speed;
            if (_currentSpeed <= 0)
                _currentSpeed = 0;
        }
        public void AddWorkerNum(int num)
        {
            _workerNum += num;
            if (_workerNum <= 0)
                _workerNum = 0;
        }
        public void AddMaintain(float num)
        {
            _maintain += num;
            if (_maintain <= 0)
                _maintain = 0;

        }
        public void AddEnergyCostNormal(float num)
        {
            _energyCostNormal += num;
            if (_energyCostNormal < 0)
            {
                _energyCostNormal = 0;
            }
        }
        public void AddEnergyCostMagic(float num)
        {
            _energyCostMagic += num;
            if (_energyCostMagic < 0)
            {
                _energyCostMagic = 0;
            }
        }



        public ManufactoryInfo(FunctionBlock block)
        {
            IndustryData = FunctionBlockModule.FetchFunctionBlockTypeIndex<FunctionBlock_Industry>(block.FunctionBlockID);
            inherentLevelData = FunctionBlockModule.GetManuInherentLevelData(IndustryData);
            AddWorkerNum(int.Parse(IndustryData.MaintenanceBase));
            AddEnergyCostNormal(Utility.TryParseIntList(IndustryData.EnergyConsumptionBase, ',')[0]);
            AddEnergyCostMagic(Utility.TryParseIntList(IndustryData.EnergyConsumptionBase, ',')[1]);
            AddMaintain(float.Parse(IndustryData.MaintenanceBase));
            AddCurrentSpeed(FunctionBlockModule.GetIndustrySpeed(block.FunctionBlockID));
        }


    }



    public class ManufactFormulaInfo
    {


        public List<FormulaData> FormulaIDList = new List<FormulaData>();
        public int CurrentFormulaID;
        public FormulaData currentFormulaData;
        public bool inProgress = false;
        public float currentNeedTime;


        public List<Material> inputMaList = new List<Material>();
        public List<Material> outputMaList = new List<Material>();

        /// <summary>
        /// 实际槽位中的物品
        /// </summary> 
        public Dictionary<Material, ushort> currentInputMaterialFormulaDic;
        public Dictionary<Material, ushort> currentOutputMaterialFormulaDic;
        public Dictionary<Material, ushort> currentBypruductMaterialFormulaDic;
        /// <summary>
        /// 配方槽位物品
        /// </summary>
        public Dictionary<Material, ushort> realInputDataDic = new Dictionary<Material, ushort>();
        public Dictionary<Material, ushort> realOutputDataDic = new Dictionary<Material, ushort>();
        public Dictionary<Material, ushort> realBypruductDataDic = new Dictionary<Material, ushort>();

        /// <summary>
        /// 所有配方列表
        /// </summary>
        public List<Dictionary<Material, ushort>> InputMaterialFormulaList = new List<Dictionary<Material, ushort>>();
        public List<Dictionary<Material, ushort>> OutputMaterialFormulaList = new List<Dictionary<Material, ushort>>();
        public List<Dictionary<Material, ushort>> BypruductMaterialFormulaList = new List<Dictionary<Material, ushort>>();

        public ManufactFormulaInfo(int currentFormulaID,FunctionBlock block)
        {
            CurrentFormulaID = currentFormulaID;
            FormulaIDList = FunctionBlockModule.GetFormulaList(block);
            currentFormulaData = FormulaModule.GetFormulaDataByID(currentFormulaID);
            currentInputMaterialFormulaDic = FormulaModule.GetFormulaMaterialDic(currentFormulaID, FormulaModule.MaterialProductType.Input);
            currentOutputMaterialFormulaDic = FormulaModule.GetFormulaMaterialDic(currentFormulaID, FormulaModule.MaterialProductType.Output);
            currentBypruductMaterialFormulaDic = FormulaModule.GetFormulaMaterialDic(currentFormulaID, FormulaModule.MaterialProductType.Byproduct);

            InputMaterialFormulaList = FunctionBlockModule.GetFunctionBlockFormulaDataList(block, FormulaModule.MaterialProductType.Input);
            OutputMaterialFormulaList = FunctionBlockModule.GetFunctionBlockFormulaDataList(block, FormulaModule.MaterialProductType.Output);
            BypruductMaterialFormulaList = FunctionBlockModule.GetFunctionBlockFormulaDataList(block, FormulaModule.MaterialProductType.Byproduct);
            //INIT SLOT
            inputMaList = FormulaModule.GetFormulaTotalMaterialList(currentFormulaID, FormulaModule.MaterialProductType.Input);
            outputMaList = FormulaModule.GetFormulaTotalMaterialList(currentFormulaID, FormulaModule.MaterialProductType.Output);
        }

        public void RefreshFormulaID(int formulaID)
        {
            CurrentFormulaID = formulaID;
            currentFormulaData = FormulaModule.GetFormulaDataByID(formulaID);
            currentInputMaterialFormulaDic = FormulaModule.GetFormulaMaterialDic(formulaID, FormulaModule.MaterialProductType.Input);
            currentOutputMaterialFormulaDic = FormulaModule.GetFormulaMaterialDic(formulaID, FormulaModule.MaterialProductType.Output);
            currentBypruductMaterialFormulaDic = FormulaModule.GetFormulaMaterialDic(formulaID, FormulaModule.MaterialProductType.Byproduct);
            //INIT SLOT
            inputMaList = FormulaModule.GetFormulaTotalMaterialList(formulaID, FormulaModule.MaterialProductType.Input);
            outputMaList = FormulaModule.GetFormulaTotalMaterialList(formulaID, FormulaModule.MaterialProductType.Output);
        }

    }

}