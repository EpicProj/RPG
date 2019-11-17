﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sim_FrameWork.UI
{
    public class BlockManuPageContext : WindowBase
    {
        public enum InfoType
        {
            Speed,
            Energy,
            Maintain,
            Worker,
        }

        private BlockManuPage m_page;

        private FunctionBlockInfoData blockInfo;
        private ManufactoryInfo manufactoryInfo;
        private ManufactFormulaInfo formulaInfo;

        private Text SpeedText;
        private Text EnergyText;
        private Text MaintianText;
        private Text WorkerText;

        //Progress
        private float targetProgress = 100.0f;
        private float currentProgress = 0f;
        private Image ProgressRingImage;
        private Text ProgressValueText;

        //Level & EXP
        private Text EXPValueText;
        private Text EXPValuePercentText;
        private Text LVText;

        //ManuCore
        private Transform InputContent;
        private Transform OutputContent;
        private Transform EnhanceContent;

        private Transform Enhance_Trans;
        private Transform Output_Trans;

        #region Override Method
        public override void Awake(params object[] paralist)
        {
            blockInfo = (FunctionBlockInfoData)paralist[0];
            manufactoryInfo = (ManufactoryInfo)paralist[1];
            formulaInfo = (ManufactFormulaInfo)paralist[2];

            m_page = UIUtility.SafeGetComponent<BlockManuPage>(Transform);

            InitTransformRef();
            AddButtonListener();
            InitInfoPanel();
            RefreshManuElementTrans(formulaInfo);
            InitFormulaSlot(formulaInfo);
        }


        public override void OnShow(params object[] paralist)
        {         
            blockInfo = (FunctionBlockInfoData)paralist[0];
            manufactoryInfo = (ManufactoryInfo)paralist[1];
            formulaInfo = (ManufactFormulaInfo)paralist[2];

            AudioManager.Instance.PlaySound(AudioClipPath.UISound.Page_Open);
            RefreshInfoAll(manufactoryInfo);
            RefreshFormulaSlotNum(formulaInfo);
            UpdateLevel();
        }

        public override bool OnMessage(UIMessage msg)
        {
            switch (msg.type)
            {
                case UIMsgType.UpdateManuSlot:
                    ManufactFormulaInfo info = (ManufactFormulaInfo)msg.content[0];
                    //Update Info
                    formulaInfo = info;
                    RefreshFormulaSlotNum(formulaInfo);
                    return true;
                case UIMsgType.UpdateLevelInfo:
                    blockInfo.levelInfo = (FunctionBlockLevelInfo)msg.content[0];
                    UpdateLevel();
                    return true;
                case UIMsgType.UpdateSpeedText:
                    //UpdateSpeed
                    float Addspeed = (float)msg.content[0];
                    manufactoryInfo.AddCurrentSpeed(Addspeed);
                    RefreshInfoText(manufactoryInfo.CurrentSpeed, InfoType.Speed);
                    return true;

                default:
                    Debug.LogError("UI msg Error , msgID=" + msg.type);
                    return false;
            }
        }



        public override void OnUpdate()
        {
            UpdateProgress(formulaInfo);
        }

        #endregion

        #region Init Method

        void InitTransformRef()
        {
            ProgressRingImage = UIUtility.SafeGetComponent<Image>(UIUtility.FindTransfrom(m_page.ProgressTrans, "Ring"));
            ProgressValueText = UIUtility.SafeGetComponent<Text>(UIUtility.FindTransfrom(m_page.ProgressTrans, "Value"));
            EXPValueText = UIUtility.SafeGetComponent<Text>(UIUtility.FindTransfrom(m_page.EXPContent, "Value"));
            EXPValuePercentText = UIUtility.SafeGetComponent<Text>(UIUtility.FindTransfrom(m_page.EXPContent, "Percent"));
            LVText = UIUtility.SafeGetComponent<Text>(UIUtility.FindTransfrom(m_page.BlockLevelInfo, "Value"));

            InputContent = UIUtility.FindTransfrom(m_page.ManuContent, "Input");
            OutputContent = UIUtility.FindTransfrom(m_page.ManuContent, "Output");
            EnhanceContent = UIUtility.FindTransfrom(m_page.ManuContent, "Enhance");

            Enhance_Trans = UIUtility.FindTransfrom(EnhanceContent, "ManuSlotElement");
            Output_Trans = UIUtility.FindTransfrom(OutputContent, "ManuSlotElement");
        }

        void AddButtonListener()
        {
            AddButtonClickListener(m_page.BackBtn, () =>
            {
                UIManager.Instance.HideWnd(UIPath.WindowPath.BlockManu_Page);
            });
        }


        void InitInfoPanel()
        {
            if (blockInfo == null)
                return;

            SpeedText = UIUtility.SafeGetComponent<Text>(m_page.BlockInfoContent.transform.Find("BaseInfo/Speed/Value"));
            EnergyText = UIUtility.SafeGetComponent<Text>(m_page.BlockInfoContent.Find("BaseInfo/Energy/Value"));
            MaintianText = UIUtility.SafeGetComponent<Text>(m_page.BlockInfoContent.Find("BaseInfo/Maintain/Value"));
            WorkerText = UIUtility.SafeGetComponent<Text>(m_page.BlockInfoContent.Find("BaseInfo/Worker/Value"));

            RefreshInfoAll(manufactoryInfo);
            m_page.Title.transform.Find("Text").GetComponent<Text>().text = blockInfo.dataModel.Name;
            m_page.Title.transform.Find("Text/Icon").GetComponent<Image>().sprite = blockInfo.dataModel.Icon;
            m_page.BlockBG.sprite = blockInfo.dataModel.BG;
            m_page.BlockDesc.text = blockInfo.dataModel.Desc;
        }


        #endregion

        public void RefreshInfoText(float value, InfoType type)
        {
            switch (type)
            {
                case InfoType.Energy:
                    EnergyText.text = value.ToString();
                    break;
                case InfoType.Maintain:
                    MaintianText.text = value.ToString();
                    break;
                case InfoType.Speed:
                    SpeedText.text = value.ToString();
                    break;
                case InfoType.Worker:
                    WorkerText.text = value.ToString();
                    break;
                default:
                    break;
            }
        }
        private void RefreshInfoAll(ManufactoryInfo info)
        {
            RefreshInfoText(info.CurrentSpeed, InfoType.Speed);
            RefreshInfoText(info.EnergyCostNormal, InfoType.Energy);
            RefreshInfoText(info.WorkerNum, InfoType.Worker);
            RefreshInfoText(info.Maintain, InfoType.Maintain);
        }

        private void RefreshManuElementTrans(ManufactFormulaInfo info)
        {
            var inputCount = info.currentInputItem.Count;
            foreach(Transform trans in InputContent)
            {
                trans.gameObject.SetActive(false);
            }
            for(int i = 0; i < inputCount; i++)
            {
                InputContent.GetChild(i).gameObject.SetActive(true);
            }
            
            ///Enhance
            if (info.currentEnhanceItem == null)
            {
                Enhance_Trans.gameObject.SetActive(false);
            }
            else
            {
                Enhance_Trans.gameObject.SetActive(true);
            }

        }

        /// <summary>
        /// 初始化格子
        /// </summary>
        /// <param name="info"></param>
        public void InitFormulaSlot(ManufactFormulaInfo info)
        {
            ///Input
            for(int i = 0; i < info.currentInputItem.Count; i++)
            {
                var element = UIUtility.SafeGetComponent<ManuSlotElement>(InputContent.GetChild(i));
                element.SetUpElement(info.currentInputItem[i].model, info.currentInputItem[i].count, info.realInputItem[i].count);
            }

            ///Enhance
            if (info.currentEnhanceItem != null)
            {
                var element = UIUtility.SafeGetComponent<ManuSlotElement>(Enhance_Trans);
                element.SetUpElement(info.currentEnhanceItem.model, info.currentEnhanceItem.count, info.realEnhanceItem.count);
            }

            ///OutPut
            var outputElement = UIUtility.SafeGetComponent<ManuSlotElement>(Output_Trans);
            outputElement.SetUpElement(info.currentOutputItem.model, info.currentOutputItem.count, info.realOutputItem.count);
          
        }

        public void RefreshFormulaSlotNum(ManufactFormulaInfo info)
        {
            for (int i = 0; i < info.currentInputItem.Count; i++)
            {
                var element = UIUtility.SafeGetComponent<ManuSlotElement>(InputContent.GetChild(i));
                element.RefreshCount(info.currentInputItem[i].model,info.realInputItem[i].count);
            }

            var outputElement = UIUtility.SafeGetComponent<ManuSlotElement>(Output_Trans);
            outputElement.RefreshCount(info.currentOutputItem.model, info.realOutputItem.count);

        }



        /// <summary>
        /// 更新生产进度条
        /// </summary>
        /// <param name="info"></param>
        public void UpdateProgress(ManufactFormulaInfo info)
        {
            if (info.inProgress)
            {
                if (currentProgress < targetProgress)
                {
                    currentProgress += (100.0f * Time.deltaTime) / info.currentNeedTime;

                    ProgressValueText.text = ((int)currentProgress).ToString() + "%";
                    ProgressRingImage.fillAmount = currentProgress / 100.0f;
                }
                else if (currentProgress > targetProgress)
                {
                    // Complete
                    currentProgress = 0;
                    ProgressRingImage.fillAmount = 0;
                    ProgressValueText.text = ((int)currentProgress).ToString() + "%";
                }
            }
            else
            {
                currentProgress = 0;
                ProgressRingImage.fillAmount = 0;
                ProgressValueText.text = ((int)currentProgress).ToString() + "%";
            }
        }

        /// <summary>
        /// 更新等级
        /// </summary>
        public void UpdateLevel()
        {
            LVText.text =blockInfo.levelInfo.currentBlockLevel.ToString();
            SetLVSlider();
            EXPValueText.text = string.Format("{0}/{1}", blockInfo.levelInfo.CurrentBlockExp.ToString(), blockInfo.levelInfo.CurrentBlockMaxEXP.ToString());
        }


        private void SetLVSlider()
        {
            var sliderGroup = UIUtility.SafeGetComponent<SliderGroup>(UIUtility.FindTransfrom(m_page.EXPContent, "Content"));
            float progress = (float)blockInfo.levelInfo.CurrentBlockExp / (float)blockInfo.levelInfo.CurrentBlockMaxEXP;
            sliderGroup.RefreshSlider(progress);
            UIUtility.SafeGetComponent<Text>(UIUtility.FindTransfrom(m_page.EXPContent, "Percent")).text = ((int)(progress * 100)).ToString();
        }

    }
}