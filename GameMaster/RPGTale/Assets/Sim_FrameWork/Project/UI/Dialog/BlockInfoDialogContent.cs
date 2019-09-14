﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sim_FrameWork
{
    public class BlockInfoDialogContent : WindowBase
    {

        public enum InfoType
        {
            Speed,
            Energy,
            Maintain,
            Worker,
        }

        private const string INFOPANEL_MANUSPEED_TITLE = "FuntionBlockInfoDialog_Info_Manufact_Speed_Title";
        private const string INFOPANEL_ENERGY_TITLE = "FuntionBlockInfoDialog_Info_Energy_Title";
        private const string INFOPANEL_MAINTAIN_TITLE = "FuntionBlockInfoDialog_Info_Maintain_Title";
        private const string INFOPANEL_WOEKER_TITLE = "FuntionBlockInfoDialog_Info_Worker_Title";
  

        private Text SpeedText;
        private Text EnergyText;
        private Text MaintianText;
        private Text WorkerText;

        private Text ProcessIndicator;
        private Image ProgressImage;

        private BlockInfoDialog m_dialog;
        private Button clostBtn;

        //Info
        FunctionBlockInfoData blockInfo;
        ManufactoryInfo manufactoryInfo;
        private float currentManuSpeed;

        private float currentProcess;
        private float targetProcess = 100;

        private DistrictContentSlotPanel slotPanel;
        private BlockManufactSlotPanel manuSlotPanel;
        private Transform InputSlotContent;
        private Transform OutputSlotContent;
        private Transform ByproductSlotContent;

        //LV
        private Text LvValue;


        /// <summary>
        /// [0] currentBlockid   [1] currentDistrictData
        /// </summary>
        /// <param name="paralist"></param>
        public override void Awake(params object[] paralist)
        {
            blockInfo = (FunctionBlockInfoData)paralist[0];
            manufactoryInfo = (ManufactoryInfo)paralist[1];

            m_dialog = GameObject.GetComponent<BlockInfoDialog>();
            slotPanel = m_dialog.DistrictSlotContent.GetComponent<DistrictContentSlotPanel>();
            manuSlotPanel = m_dialog.ManufactPanel.GetComponent<BlockManufactSlotPanel>();
            InputSlotContent = manuSlotPanel.transform.Find("InputSlotContent");
            OutputSlotContent = manuSlotPanel.transform.Find("Output/OutputSlotContent");
            ByproductSlotContent = manuSlotPanel.transform.Find("Output/ByproductSlotContent");
            //Level
           
            LvValue = m_dialog.LevelValue.transform.Find("Value").GetComponent<Text>();
            m_dialog.InherentLevelText.text = FunctionBlockModule.GetCurrentInherentLevelName(manufactoryInfo.inherentLevelData);

            clostBtn = GameObject.Find("Btn").GetComponent<Button>();
            ProgressImage = m_dialog.Processbar.transform.Find("Progress").GetComponent<Image>();
            ProcessIndicator = m_dialog.Processbar.transform.Find("Indicator").GetComponent<Text>();
            AddBtnListener();
            InitInfoPanel();
            InitBaseData();
            currentManuSpeed = manufactoryInfo.CurrentSpeed;
        }



        public override void OnShow(params object[] paralist)
        {
            //Init Text
            m_dialog.Title.transform.Find("BG2/Desc/FacotryName").GetComponent<Text>().text = FunctionBlockModule.GetFunctionBlockName(blockInfo.block);
            m_dialog.BlockInfoDesc.text = FunctionBlockModule.GetFunctionBlockDesc(blockInfo.block);
            blockInfo = (FunctionBlockInfoData)paralist[0];
            manufactoryInfo = (ManufactoryInfo)paralist[1];
            //Init Sprite
            //m_dialog.FactoryBG.GetComponent<Image>().sprite = FunctionBlockModule.Instance.GetFunctionBlockIcon(currentBlock.FunctionBlockID);
        }

        private void InitBaseData()
        {
            slotPanel.InitDistrictDataSlot(blockInfo);
            slotPanel.InitData();
            slotPanel.InitDistrictArea(blockInfo);
            RefreshManuSlot(manufactoryInfo.formulaInfo);
            UpdateLevel(blockInfo.levelInfo);
            InitDistrictBuildContent();
        }

        public void RefreshManuSlot(ManufactFormulaInfo info)
        {
            int inputSlotCount = info.currentInputMaterialFormulaDic.Count;
            int outputSlotCount = info.currentOutputMaterialFormulaDic.Count;
            int byproductCount = info.currentBypruductMaterialFormulaDic.Count;

            //Init inputSlot
            foreach (Transform trans in InputSlotContent.transform)
            {
                trans.gameObject.SetActive(false);
            }
            for (int i = 0; i < inputSlotCount; i++)
            {
                InputSlotContent.GetChild(i).gameObject.SetActive(true);
            }
            //Init outPutSlot
            foreach(Transform trans in OutputSlotContent.transform)
            {
                trans.gameObject.SetActive(false);
            }
            for(int i = 0; i < outputSlotCount; i++)
            {
                OutputSlotContent.GetChild(i).gameObject.SetActive(true);
            }
            //Init Byproduct
            foreach(Transform trans in ByproductSlotContent.transform)
            {
                trans.gameObject.SetActive(false);
            }
            for(int i = 0; i < byproductCount; i++)
            {
                ByproductSlotContent.GetChild(i).gameObject.SetActive(true);
            }
        }

        public override void OnUpdate()
        {
            UpdateProgress(manufactoryInfo.formulaInfo);
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                UIManager.Instance.HideWnd(UIPath.FUNCTIONBLOCK_INFO_DIALOG);
            }
        }

        public override bool OnMessage(UIMessage msg)
        {
            switch (msg.type)
            {
                case  UIMsgType.UpdateManuSlot:
                    ManufactFormulaInfo formulaInfo = (ManufactFormulaInfo)msg.content;
                    //Update Info
                    manufactoryInfo.formulaInfo = formulaInfo;

                    UpdateManuMaterialSlot(formulaInfo);
                    return true;
                case  UIMsgType.UpdateLevelInfo:
                    FunctionBlockLevelInfo levelInfo = (FunctionBlockLevelInfo)msg.content;
                    //Update Info
                    blockInfo.levelInfo = levelInfo;
                    UpdateLevel(levelInfo);
                    return true;
                case  UIMsgType.UpdateSpeedText:
                    //UpdateSpeed
                    float Addspeed = (float)msg.content;
                    manufactoryInfo.AddCurrentSpeed(Addspeed);
                    RefreshInfoText(manufactoryInfo.CurrentSpeed, InfoType.Speed);
                    return true;

                default:
                    Debug.LogError("UI msg Error , msgID=" + msg.type);
                    return false;
            }
            
        }

    

        private void InitInfoPanel()
        {
            m_dialog.InfoData.transform.Find("Speed/Info/Item/Text").GetComponent<Text>().text = MultiLanguage.Instance.GetTextValue(INFOPANEL_MANUSPEED_TITLE);
            SpeedText = m_dialog.InfoData.transform.Find("Speed/Value/Value").GetComponent<Text>();
            m_dialog.InfoData.transform.Find("Energy/Info/Item/Text").GetComponent<Text>().text = MultiLanguage.Instance.GetTextValue(INFOPANEL_ENERGY_TITLE);
            EnergyText = m_dialog.InfoData.transform.Find("Energy/Value/Value").GetComponent<Text>();
            m_dialog.InfoData.transform.Find("Maintain/Info/Item/Text").GetComponent<Text>().text = MultiLanguage.Instance.GetTextValue(INFOPANEL_MAINTAIN_TITLE);
            MaintianText= m_dialog.InfoData.transform.Find("Maintain/Value/Value").GetComponent<Text>();
            m_dialog.InfoData.transform.Find("Worker/Info/Item/Text").GetComponent<Text>().text = MultiLanguage.Instance.GetTextValue(INFOPANEL_WOEKER_TITLE);
            WorkerText = m_dialog.InfoData.transform.Find("Worker/Value/Value").GetComponent<Text>();
            RefreshInfoText(manufactoryInfo.CurrentSpeed, InfoType.Speed);
            RefreshInfoText(manufactoryInfo.EnergyCostNormal, InfoType.Energy);
            RefreshInfoText(manufactoryInfo.WorkerNum, InfoType.Worker);
            RefreshInfoText(manufactoryInfo.Maintain, InfoType.Maintain);
        }

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



        //Button
        private void AddBtnListener()
        {
            AddButtonClickListener(clostBtn, delegate () { HideInfoDialog();});
        }

        private void HideInfoDialog()
        {
            UIManager.Instance.HideWnd(UIPath.FUNCTIONBLOCK_INFO_DIALOG);
        }

     
        //LV and EXP
        public void UpdateLevel(FunctionBlockLevelInfo info)
        {
            SetLevel(info.currentBlockLevel);
            SetCurrentLvSlider(info);
            SetLvValue(info);
        }

        private void SetLevel(int level)
        {
            m_dialog.Level.text = level.ToString();
        }
        private void SetCurrentLvSlider(FunctionBlockLevelInfo info)
        {
            int totalEXP = info.CurrentBlockMaxEXP;
            float value = info.CurrentBlockExp*100/ totalEXP;
            if (m_dialog.isActiveAndEnabled)
            {
                m_dialog.StartCoroutine(m_dialog.SliderLerp(value, 5f));
            }
           
           
        }
        private void SetLvValue(FunctionBlockLevelInfo info)
        {
            LvValue.text = string.Format("{0} / {1}", info.CurrentBlockExp.ToString(), info.CurrentBlockMaxEXP.ToString());
        }

 

        //Progress
        public void UpdateProgress(ManufactFormulaInfo info)
        {
            if (info.inProgress)
            {
                if (currentProcess < targetProcess)
                {
                    currentProcess += (100.0f * Time.deltaTime) / info.currentNeedTime;

                    ProcessIndicator.text = ((int)currentProcess).ToString() + "%";
                    ProgressImage.fillAmount = currentProcess / 100.0f;
                }
                else if (currentProcess > targetProcess)
                {
                    // Complete
                    currentProcess=0;
                    ProgressImage.fillAmount = 0;
                    ProcessIndicator.text = ((int)currentProcess).ToString() + "%";
                }
            }
            else
            {
                currentProcess = 0;
                ProgressImage.fillAmount = 0;
                ProcessIndicator.text = ((int)currentProcess).ToString() + "%";
            }
          
        }

        public void UpdateManuMaterialSlot(ManufactFormulaInfo info)
        {
            Dictionary<Material, ushort> InputDic = info.realInputDataDic;
            Dictionary<Material, ushort> OutputDic = info.realOutputDataDic;
            //Update Input
            foreach(KeyValuePair<Material,ushort> kvp in InputDic)
            {
                manuSlotPanel.InitManuInputMaterialSlot(kvp.Key, kvp.Value);
            }
            //Update Output
            foreach(KeyValuePair<Material,ushort> kvp in OutputDic)
            {
                manuSlotPanel.InitManuOutputMaterialSlot(kvp.Key, kvp.Value);
            }


        }

        public void InitDistrictBuildContent()
        {
            for(int i = 0; i < blockInfo.ActiveDistrictBuildList.Count; i++)
            {
                GameObject districtBuild = ObjectManager.Instance.InstantiateObject(UIPath.DISTRICT_BUILD_PREFAB_PATH);
                DistrictBuildElement element = districtBuild.GetComponent<DistrictBuildElement>();
                element.InitCostElementData(blockInfo.ActiveDistrictBuildList[i]);
                districtBuild.transform.SetParent(m_dialog.BuildContent.transform, false);
            }
        }




    }
}