using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork
{
    public class FormulaContentCmpt : MonoBehaviour
    {
        public enum InitType
        {
            Normal,
            FormulaChange
        }

        //ManuCore
        private Transform InputContent;
        private Transform OutputContent;
        private Transform EnhanceContent;
        private Transform EnhanceLine;

        private Transform Enhance_Trans;
        private Transform Output_Trans;

        private ManufactFormulaInfo _info;

        public void Awake()
        {
            InputContent = UIUtility.FindTransfrom(transform, "Input");
            OutputContent = UIUtility.FindTransfrom(transform, "Output");
            EnhanceContent = UIUtility.FindTransfrom(transform, "Enhance");
            

            Enhance_Trans = UIUtility.FindTransfrom(EnhanceContent, "ManuSlotElement");
            Output_Trans = UIUtility.FindTransfrom(OutputContent, "ManuSlotElement");
        }

        public void Init(ManufactFormulaInfo info,InitType type)
        {
            _info = info;
            switch (type)
            {
                case InitType.FormulaChange:
                    EnhanceLine = UIUtility.FindTransfrom(transform, "EnhanceLine");
                    InitFormulaSlot(InitType.FormulaChange);  
                    break;
                case InitType.Normal:
                    InitFormulaSlot(InitType.Normal);
                    RefreshManuElementTrans(_info,type);
                    break;
            }  
        }

        /// <summary>
        /// Init
        /// </summary>
        /// <param name="info"></param>
        private void InitFormulaSlot(InitType type)
        {
            ///Input
            for (int i = 0; i < _info.currentInputItem.Count; i++)
            {
                var element = UIUtility.SafeGetComponent<ManuSlotElement>(InputContent.GetChild(i));
                if(type== InitType.Normal)
                {
                    element.SetUpElement(_info.currentInputItem[i].model, _info.currentInputItem[i].count, _info.realInputItem[i].count);
                }
                else
                {
                    element.SetUpFormulaChangeElement(_info.currentInputItem[i].model, _info.currentInputItem[i].count);
                } 
            }

            ///Enhance
            if (_info.currentEnhanceItem.model.ID != 0 && _info.currentEnhanceItem != null)
            {
                Enhance_Trans.gameObject.SetActive(true);
                var element = UIUtility.SafeGetComponent<ManuSlotElement>(Enhance_Trans);
                if (type == InitType.Normal)
                {
                    element.SetUpElement(_info.currentEnhanceItem.model, _info.currentEnhanceItem.count, _info.realEnhanceItem.count);
                }
                else
                {
                    element.SetUpFormulaChangeElement(_info.currentEnhanceItem.model, _info.currentEnhanceItem.count);
                }
            }

            ///OutPut
            var outputElement = UIUtility.SafeGetComponent<ManuSlotElement>(Output_Trans);
            if(type== InitType.Normal)
            {
                outputElement.SetUpElement(_info.currentOutputItem.model, _info.currentOutputItem.count, _info.realOutputItem.count);
            }
            else
            {
                outputElement.SetUpFormulaChangeElement(_info.currentOutputItem.model, _info.currentOutputItem.count);
            }
          
        }

        /// <summary>
        /// Refresh
        /// </summary>
        public void RefreshManuElementTrans(ManufactFormulaInfo info,InitType type)
        {
            _info = info;
            if (_info == null)
                return;
            var inputCount = _info.currentInputItem.Count;
            foreach (Transform trans in InputContent)
            {
                trans.gameObject.SetActive(false);
            }
            for (int i = 0; i < inputCount; i++)
            {
                InputContent.GetChild(i).gameObject.SetActive(true);
            }

            ///Enhance
            if (_info.currentEnhanceItem.model.ID == 0)
            {
                Enhance_Trans.gameObject.SetActive(false);
                if(type== InitType.FormulaChange)
                {
                    EnhanceLine.gameObject.SetActive(false);
                }
            }
            else
            {
                Enhance_Trans.gameObject.SetActive(true);
                if(type== InitType.FormulaChange)
                {
                    EnhanceLine.gameObject.SetActive(true);
                }
            }
        }

        /// <summary>
        /// Refresh Count
        /// </summary>
        /// <param name="info"></param>
        public void RefreshFormulaSlotNum(ManufactFormulaInfo info)
        {
            _info = info;
            for (int i = 0; i < _info.currentInputItem.Count; i++)
            {
                var element = UIUtility.SafeGetComponent<ManuSlotElement>(InputContent.GetChild(i));
                element.RefreshCount(_info.currentInputItem[i].model, _info.realInputItem[i].count);
            }

            var outputElement = UIUtility.SafeGetComponent<ManuSlotElement>(Output_Trans);
            outputElement.RefreshCount(_info.currentOutputItem.model, _info.realOutputItem.count);

        }


    }
}