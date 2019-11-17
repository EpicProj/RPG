using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork
{

    public class ManufactoryBase : MonoBehaviour
    {

        public ManufactoryInfo manufactoryInfo;
        public ManufactFormulaInfo formulaInfo;
        public FunctionBlockBase _blockBase;

        /// <summary>
        /// 当前配方
        /// </summary>
        private int _currentFormulaID = 0;
        public int CurrentFormulaID { get { return _currentFormulaID; }  }


        public void SetData()
        {
            _blockBase = UIUtility.SafeGetComponent<FunctionBlockBase>(transform);
            manufactoryInfo = new ManufactoryInfo(_blockBase.functionBlock);
            List<FormulaData> formulaData = FunctionBlockModule.GetFormulaList(_blockBase.info.block);
            if (formulaData.Count == 1)
            {
                formulaInfo = new ManufactFormulaInfo(formulaData[0].FormulaID, _blockBase.info.block);
            }

            var info = formulaInfo;
            for (int i = 0; i < info.currentInputItem.Count; i++)
            {
                formulaInfo.realInputItem.Add(new FormulaItem(info.currentInputItem[i].model,0));
            }

            for (int i = 0; i < info.currentOutputItem.Count; i++)
            {
                formulaInfo.realOutputItem.Add(new FormulaItem(info.currentOutputItem[i].model, 0));
            }

            formulaInfo.realEnhanceItem = info.currentEnhanceItem;


            formulaInfo.currentNeedTime = GetCurrentFormulaNeedTime();
            _blockBase.OnBlockSelectAction += Onselect;

        }

        private void Onselect()
        {
            UIManager.Instance.PopUpWnd(UIPath.WindowPath.BlockManu_Page, WindowType.Page, true, _blockBase.info, manufactoryInfo,formulaInfo);
        }

        /// <summary>
        /// 更换配方
        /// </summary>
        /// <param name="formulaID"></param>
        public void ChangeFormula(int formulaID)
        {
            _currentFormulaID = formulaID;
            formulaInfo.RefreshFormula(formulaID);
        }

        #region Manufact
        /// <summary>
        /// 增加材料到输入栏
        /// </summary>
        /// <param name="item"></param>
        /// <param name="addCount"></param>
        public void AddMaterialToInputSlot(FormulaItem item,ushort addCount)
        {
            for (int i = 0; i < formulaInfo.currentInputItem.Count; i++)
            {
                if (item == formulaInfo.currentInputItem[i])
                {
                    //计算格子容量 TODO
                    formulaInfo.realInputItem[i].count += addCount;
                    StartManufact();
                }
            }
        }

        private void AddOutputSlotNum()
        {
            for(int i = 0; i < formulaInfo.currentOutputItem.Count; i++)
            {
                ushort count = formulaInfo.currentInputItem[i].count;
                formulaInfo.realOutputItem[i].count += count;
                //Add to PlayerData
                PlayerManager.Instance.AddMaterialData(formulaInfo.currentOutputItem[i].model.ID, count);
            }
        }

        private void StartManufact()
        {
            if (CheckCanproduce() && formulaInfo.inProgress == false)
            {
                formulaInfo.inProgress = true;
                StartCoroutine(WaitManufactFinish());
            }
        }

        private bool CheckCanproduce()
        {
            for(int i = 0; i < formulaInfo.realInputItem.Count; i++)
            {
                if (formulaInfo.realInputItem[i].count < formulaInfo.currentInputItem[i].count)
                {
                    formulaInfo.inProgress = false;
                    return false;
                }
            }
            return true;
        }


        IEnumerator WaitManufactFinish()
        {
            yield return new WaitForSeconds(GetCurrentFormulaNeedTime());
            formulaInfo.inProgress = false;
            ///Reduce Input num
            ReduceInputSlotNum();
            ///Add OutPut
            AddOutputSlotNum();

            ///UpdateUI
            UIManager.Instance.SendMessageToWnd(UIPath.WindowPath.BlockManu_Page, new UIMessage(UIMsgType.UpdateManuSlot, new List<object>(1) { formulaInfo }));

            ///UpdateEXP
            _blockBase.info.levelInfo.AddCurrentBlockEXP(formulaInfo.currentFormulaData.EXP);
            UIManager.Instance.SendMessageToWnd(UIPath.WindowPath.BlockManu_Page, new UIMessage(UIMsgType.UpdateLevelInfo, new List<object>(1) { _blockBase.info.levelInfo }));

            StartManufact();
        }

        private void ReduceInputSlotNum()
        {
            for (int i = 0; i < formulaInfo.currentInputItem.Count; i++)
            {
                formulaInfo.realInputItem[i].count -= formulaInfo.currentInputItem[i].count;
            }
        }

        private float GetCurrentFormulaNeedTime()
        {
            float totalTime = formulaInfo.currentFormulaData.ProductSpeed;
            float time = totalTime / manufactoryInfo.CurrentSpeed;
            return time;
        }

        #endregion

    }
    public class ManufactoryInfo
    {
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



        public ManufactoryInfo(FunctionBlock block)
        {
            IndustryData = FunctionBlockModule.FetchFunctionBlockTypeIndex<FunctionBlock_Industry>(block.FunctionBlockID);
            inherentLevelData = FunctionBlockModule.GetManuInherentLevelData(IndustryData);
            AddWorkerNum(int.Parse(IndustryData.MaintenanceBase));
            AddEnergyCostNormal(Utility.TryParseIntList(IndustryData.EnergyConsumptionBase, ',')[0]);
            AddMaintain(float.Parse(IndustryData.MaintenanceBase));
            AddCurrentSpeed(FunctionBlockModule.GetIndustrySpeed(block.FunctionBlockID));
        }


    }



    public class ManufactFormulaInfo
    {
        /// <summary>
        /// 可选配方列表
        /// </summary>
        public List<FormulaData> FormulaChooseList = new List<FormulaData>();
        public int CurrentFormulaID;
        public FormulaData currentFormulaData;
        public bool inProgress = false;
        /// <summary>
        /// 当前生产所需最大时长
        /// </summary>
        public float currentNeedTime;


        /// <summary>
        /// 配方所需材料
        /// </summary> 
        public List<FormulaItem> currentInputItem;
        public List<FormulaItem> currentOutputItem;
        public FormulaItem  currentEnhanceItem;
        /// <summary>
        /// 实际输入材料
        /// </summary>
        public List<FormulaItem> realInputItem = new List<FormulaItem>();
        public List<FormulaItem> realOutputItem = new List<FormulaItem>();
        public FormulaItem realEnhanceItem;


        public ManufactFormulaInfo(int currentFormulaID,FunctionBlock block)
        {
            CurrentFormulaID = currentFormulaID;
            FormulaChooseList = FunctionBlockModule.GetFormulaList(block);
            currentFormulaData = FormulaModule.GetFormulaDataByID(currentFormulaID);
            currentInputItem = FormulaModule.GetFormulaItemList(currentFormulaID, FormulaModule.MaterialProductType.Input);
            currentOutputItem = FormulaModule.GetFormulaItemList(currentFormulaID, FormulaModule.MaterialProductType.Output);
            currentEnhanceItem = FormulaModule.GetFormulaEnhanceMaterial(currentFormulaID);

        }



        public void RefreshFormula(int formulaID)
        {

            CurrentFormulaID = formulaID;
            currentFormulaData = FormulaModule.GetFormulaDataByID(formulaID);
            currentInputItem = FormulaModule.GetFormulaItemList(formulaID, FormulaModule.MaterialProductType.Input);
            currentOutputItem = FormulaModule.GetFormulaItemList(formulaID, FormulaModule.MaterialProductType.Output);
            currentEnhanceItem = FormulaModule.GetFormulaEnhanceMaterial(CurrentFormulaID);

        }

    }

    public class FormulaItem
    {
        public MaterialDataModel model;
        public ushort count;

        public FormulaItem(MaterialDataModel Model, ushort Count)
        {
            model = Model;
            count = Count;
        }

        public FormulaItem(Material ma, ushort Count)
        {
            MaterialDataModel _model = new MaterialDataModel();
            _model.Create(ma.MaterialID);
            model = _model;
            count = Count;
        }

    }

}