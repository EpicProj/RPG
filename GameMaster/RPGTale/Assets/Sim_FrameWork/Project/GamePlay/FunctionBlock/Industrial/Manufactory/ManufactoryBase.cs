using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork
{
    public class ManufactoryBase : FunctionBlockBase
    {

        public enum ManufactoryType
        {
            Iron,
            Electronic
        }

        public enum ManufactoryInherentLevel
        {
            Elementary,
            Secondary,
            Advanced
        }

        public FunctionBlock_Manufacture manufactoryData;

        public List<Dictionary<Material, ushort>> InputMaterialFormulaList = new List<Dictionary<Material, ushort>>();
        public List<Dictionary<Material, ushort>> OutputMaterialFormulaList = new List<Dictionary<Material, ushort>>();
        public List<Dictionary<Material, ushort>> BypruductMaterialFormulaList = new List<Dictionary<Material, ushort>>();

        List<Material> inputMaList = new List<Material>();
        List<Material> outputMaList = new List<Material>();

        public FormulaData currentFormulaData;


        public override void InitData()
        {
            base.InitData();
            GetFormulaData();
            InitFunctionBlock_ManuInfo();
           
           
        }

        public void InitFormulaInfo()
        {
            currentFormulaData = FormulaModule.Instance.GetFormulaDataByID(currentFormulaID);
            info.formulaInfo = new ManufactFormulaInfo();
            info.formulaInfo.FormulaIDList = FunctionBlockModule.Instance.GetFormulaDataList(info.block);
            info.formulaInfo.CurrentFormulaID = currentFormulaID;
            info.formulaInfo.currentNeedTime = GetCurrentFormulaNeedTime();
            info.formulaInfo.currentInputMaterialFormulaDic = FormulaModule.Instance.GetFormulaMaterialDic(currentFormulaID, FormulaModule.MaterialProductType.Input);
            info.formulaInfo.currentOutputMaterialFormulaDic = FormulaModule.Instance.GetFormulaMaterialDic(currentFormulaID, FormulaModule.MaterialProductType.Output);
            info.formulaInfo.currentBypruductMaterialFormulaDic = FormulaModule.Instance.GetFormulaMaterialDic(currentFormulaID, FormulaModule.MaterialProductType.Byproduct);
            
            InitFormula();
        }

        

        public FunctionBlockInfoData InitFunctionBlock_ManuInfo()
        {
            manufactoryData = FunctionBlockModule.Instance.FetchFunctionBlockTypeIndex<FunctionBlock_Manufacture>(functionBlock.FunctionBlockID);
            info.AddWorkerNum (int.Parse(manufactoryData.MaintenanceBase));
            info.AddEnergyCostNormal (Utility.TryParseIntList(manufactoryData.EnergyConsumptionBase,',')[0]);
            info.AddEnergyCostMagic  (Utility.TryParseIntList(manufactoryData.EnergyConsumptionBase, ',')[1]);

            info.CurrentSpeed = FunctionBlockModule.Instance.GetManufactureSpeed(functionBlock.FunctionBlockID);
            info.districtAreaMax = FunctionBlockModule.Instance.GetFunctionBlockAreaMax<FunctionBlock_Manufacture>(functionBlock);
            info.currentDistrictDataDic = FunctionBlockModule.Instance.GetFuntionBlockOriginAreaInfo<FunctionBlock_Manufacture>(functionBlock); ;
            info.currentDistrictBaseDic = FunctionBlockModule.Instance.GetFuntionBlockAreaDetailDefaultDataInfo<FunctionBlock_Manufacture>(functionBlock);
          
            InitFormulaInfo();
            return info;
        }


        public void GetFormulaData()
        {
            InputMaterialFormulaList = FunctionBlockModule.Instance.GetFunctionBlockFormulaDataList(functionBlock, FormulaModule.MaterialProductType.Input);
            OutputMaterialFormulaList = FunctionBlockModule.Instance.GetFunctionBlockFormulaDataList(functionBlock, FormulaModule.MaterialProductType.Output);
            BypruductMaterialFormulaList = FunctionBlockModule.Instance.GetFunctionBlockFormulaDataList(functionBlock, FormulaModule.MaterialProductType.Byproduct);
        }

        public void InitFormula()
        {
            foreach(var material in info.formulaInfo.currentInputMaterialFormulaDic.Keys)
            {
                info.formulaInfo.realInputDataDic.Add(material, 0);
            }
            foreach(var material in info.formulaInfo.currentOutputMaterialFormulaDic.Keys)
            {
                info.formulaInfo.realOutputDataDic.Add(material, 0);
            }
            //获取材料列表
            inputMaList = FormulaModule.Instance.GetFormulaTotalMaterialList(currentFormulaID, FormulaModule.MaterialProductType.Input);
            outputMaList = FormulaModule.Instance.GetFormulaTotalMaterialList(currentFormulaID, FormulaModule.MaterialProductType.Output);
          
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
            Material ma = MaterialModule.Instance.GetMaterialByMaterialID(id);
            if (info.formulaInfo.currentInputMaterialFormulaDic.ContainsKey(ma))
            {
                if (info.formulaInfo.realInputDataDic[ma] +count > ma.BlockCapacity)
                {
                    return;
                }
                info.formulaInfo.realInputDataDic[ma] += count;
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
           for(int i = 0; i < inputMaList.Count; i++)
           {
               info.formulaInfo.realInputDataDic[inputMaList[i]] -= info.formulaInfo.currentInputMaterialFormulaDic[inputMaList[i]];
           }

        }

        public void AddOutputSlotNum()
        {
            for(int i = 0; i < outputMaList.Count; i++)
            {
                ushort count = info.formulaInfo.currentOutputMaterialFormulaDic[outputMaList[i]];
                info.formulaInfo.realOutputDataDic[outputMaList[i]] += count;
                //Add to PlayerData
                PlayerModule.Instance.AddMaterialData(outputMaList[i].MaterialID, count);
            }

          
        }


        public void StartManufact()
        {
            if (CheckCanproduce() && info.formulaInfo.inProgress == false)
            {
                info.formulaInfo.inProgress = true;
                StartCoroutine(WaitManufactFinish());
            }
        }

        public bool CheckCanproduce()
        {
            foreach (KeyValuePair<Material, ushort> kvp in info.formulaInfo.realInputDataDic)
            {
                if (kvp.Value < info.formulaInfo.currentInputMaterialFormulaDic[kvp.Key])
                {
                    info.formulaInfo.inProgress = false;
                    return false;
                }
            }
            return true;
        }


        IEnumerator WaitManufactFinish()
        {
            yield return new WaitForSeconds(GetCurrentFormulaNeedTime());
            info.formulaInfo.inProgress = false;
            //Reduce Input num
            ReduceInputSlotNum();
            //Add OutPut
            AddOutputSlotNum();
            //UpdateUI
            UIManager.Instance.SendMessageToWnd(UIPath.FUCNTIONBLOCK_INFO_DIALOG, "UpdateManuSlot", info.formulaInfo);

            //UpdateEXP
            info.levelInfo.AddCurrentBlockEXP(currentFormulaData.EXP);
            UIManager.Instance.SendMessageToWnd(UIPath.FUCNTIONBLOCK_INFO_DIALOG, "UpdateLevelInfo", info.levelInfo);

            StartManufact();
        }

        public float GetCurrentFormulaNeedTime()
        {
            float totalTime = currentFormulaData.ProductSpeed;
            float time = totalTime / info.CurrentSpeed;
            return time;
        }

        #endregion

    }

}