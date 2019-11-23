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


        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.M))
            {
                AddMaterialToInputSlot(100, 10);
            }
        }

        public void SetData()
        {
            _blockBase = UIUtility.SafeGetComponent<FunctionBlockBase>(transform);
            manufactoryInfo = new ManufactoryInfo(_blockBase.functionBlock);
            List<FormulaData> formulaData = FunctionBlockModule.GetFormulaList(_blockBase.info.block);
            if (formulaData.Count >= 1)
            {
                formulaInfo = new ManufactFormulaInfo(formulaData[0].FormulaID, _blockBase.info.block);
            }

            var info = formulaInfo;
            for (int i = 0; i < info.currentInputItem.Count; i++)
            {
                formulaInfo.realInputItem.Add(new FormulaItem(info.currentInputItem[i].model,0));
            }

            formulaInfo.realOutputItem = new FormulaItem(info.currentOutputItem.model, 0);
            formulaInfo.realEnhanceItem = new FormulaItem(info.currentEnhanceItem.model,0) ;


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
        public void AddMaterialToInputSlot(int MaterialID,ushort addCount)
        {
            for (int i = 0; i < formulaInfo.currentInputItem.Count; i++)
            {
                if (MaterialID == formulaInfo.currentInputItem[i].model.ID)
                {
                    //计算格子容量 TODO
                    formulaInfo.realInputItem[i].count += addCount;
                    SendMessage(UIMsgType.UpdateManuSlot);
                    StartManufact();
                }
            }
        }

        private void AddOutputSlotNum()
        {
            ushort count = formulaInfo.currentOutputItem.count;
            formulaInfo.realOutputItem.count += count;
            //Add to PlayerData
            PlayerManager.Instance.AddMaterialData(formulaInfo.currentOutputItem.model.ID, count);
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
            SendMessage(UIMsgType.UpdateManuSlot);

            ///UpdateEXP
            _blockBase.info.levelInfo.AddCurrentBlockEXP(formulaInfo.currentFormulaData.EXP);
            UIManager.Instance.SendMessageToWnd(UIPath.WindowPath.BlockManu_Page, new UIMessage(UIMsgType.UpdateLevelInfo, new List<object>(1) { _blockBase.info.levelInfo }));

            StartManufact();
        }

        private void SendMessage(UIMsgType type)
        {
            switch (type)
            {
                case UIMsgType.UpdateManuSlot:
                    UIManager.Instance.SendMessageToWnd(UIPath.WindowPath.BlockManu_Page, new UIMessage(UIMsgType.UpdateManuSlot, new List<object>(1) { formulaInfo }));
                    break;
            }
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
        public float MaxNeedTime;

        /// <summary>
        /// 配方所需材料
        /// </summary> 
        public List<FormulaItem> currentInputItem;
        public FormulaItem currentOutputItem;
        public FormulaItem  currentEnhanceItem;
        /// <summary>
        /// 实际输入材料
        /// </summary>
        public List<FormulaItem> realInputItem = new List<FormulaItem>();
        public FormulaItem realOutputItem;
        public FormulaItem realEnhanceItem;


        public ManufactFormulaInfo(int currentFormulaID,FunctionBlock block)
        {
            CurrentFormulaID = currentFormulaID;
            FormulaChooseList = FunctionBlockModule.GetFormulaList(block);
            currentFormulaData = FormulaModule.GetFormulaDataByID(currentFormulaID);
            currentInputItem = FormulaModule.GetFormulaItemList(currentFormulaID, FormulaModule.MaterialProductType.Input);
            currentOutputItem = FormulaModule.GetFormulaOutputMaterial(currentFormulaID);
            currentEnhanceItem = FormulaModule.GetFormulaEnhanceMaterial(currentFormulaID);
            MaxNeedTime = currentFormulaData.ProductSpeed;
        }

        public ManufactFormulaInfo(int currentFormulaID)
        {
            CurrentFormulaID = currentFormulaID;
            currentFormulaData = FormulaModule.GetFormulaDataByID(currentFormulaID);
            currentInputItem = FormulaModule.GetFormulaItemList(currentFormulaID, FormulaModule.MaterialProductType.Input);
            currentOutputItem = FormulaModule.GetFormulaOutputMaterial(currentFormulaID);
            currentEnhanceItem = FormulaModule.GetFormulaEnhanceMaterial(currentFormulaID);
            MaxNeedTime = currentFormulaData.ProductSpeed;
        }



        public void RefreshFormula(int formulaID)
        {

            CurrentFormulaID = formulaID;
            currentFormulaData = FormulaModule.GetFormulaDataByID(formulaID);
            currentInputItem = FormulaModule.GetFormulaItemList(formulaID, FormulaModule.MaterialProductType.Input);
            currentOutputItem = FormulaModule.GetFormulaOutputMaterial(formulaID);
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