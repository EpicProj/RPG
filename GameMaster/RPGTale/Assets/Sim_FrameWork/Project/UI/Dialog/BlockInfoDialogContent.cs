using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sim_FrameWork
{
    public class BlockInfoDialogContent : WindowBase
    {

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
        FunctionBlockInfoData blockInfo;
        private float currentManuSpeed;

        private float currentProcess;
        private float targetProcess = 100;

        private DistrictContentSlotPanel slotPanel;
        private BlockManufactSlotPanel manuSlotPanel;
        private Transform InputSlotContent;
        private Transform OutputSlotContent;
        private Transform ByproductSlotContent;



        /// <summary>
        /// [0] currentBlockid   [1] currentDistrictData
        /// </summary>
        /// <param name="paralist"></param>
        public override void Awake(params object[] paralist)
        {
            blockInfo = (FunctionBlockInfoData)paralist[0];

            m_dialog = GameObject.GetComponent<BlockInfoDialog>();
            slotPanel = m_dialog.DistrictSlotContent.GetComponent<DistrictContentSlotPanel>();
            manuSlotPanel = m_dialog.ManufactPanel.GetComponent<BlockManufactSlotPanel>();
            InputSlotContent = manuSlotPanel.transform.Find("InputSlotContent");
            OutputSlotContent = manuSlotPanel.transform.Find("Output/OutputSlotContent");
            ByproductSlotContent = manuSlotPanel.transform.Find("Output/ByproductSlotContent");

            clostBtn = GameObject.Find("MainBG").GetComponent<Button>();
            ProgressImage = m_dialog.Processbar.transform.Find("Progress").GetComponent<Image>();
            ProcessIndicator = m_dialog.Processbar.transform.Find("Indicator").GetComponent<Text>();
            AddBtnListener();
            InitInfoPanel();
            InitBaseData();
            currentManuSpeed = blockInfo.CurrentSpeed;
        }



        public override void OnShow(params object[] paralist)
        {
            //Init Text
            m_dialog.Title.transform.Find("BG2/Desc/FacotryName").GetComponent<Text>().text = FunctionBlockModule.Instance.GetFunctionBlockName(blockInfo.block);
            m_dialog.BlockInfoDesc.text = FunctionBlockModule.Instance.GetFunctionBlockDesc(blockInfo.block);
            blockInfo = (FunctionBlockInfoData)paralist[0];
            //Init Sprite
            //m_dialog.FactoryBG.GetComponent<Image>().sprite = FunctionBlockModule.Instance.GetFunctionBlockIcon(currentBlock.FunctionBlockID);
        }

        private void InitBaseData()
        {
            slotPanel.InitDistrictDataSlot(blockInfo);
            slotPanel.InitData();
            slotPanel.InitDistrictArea(blockInfo);
            RefreshManuSlot(blockInfo.formulaInfo);
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
            UpdateProgress(blockInfo.formulaInfo);
        }

        public override bool OnMessage(UIMsgID msgID, params object[] paralist)
        {
            if(msgID== UIMsgID.Update)
            {
                ManufactFormulaInfo formulaInfo = (ManufactFormulaInfo)paralist[0];
                //Update Info
                blockInfo.formulaInfo = formulaInfo;

                UpdateManuMaterialSlot(formulaInfo);

            }
            return true;
        }


        private void UpdateDistrictSlot()
        {

            
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

        }


        //Button
        private void AddBtnListener()
        {
            AddButtonClickListener(clostBtn, delegate () { HideInfoDialog();});
        }

        private void HideInfoDialog()
        {
            UIManager.Instance.HideWnd(UIPath.FUCNTIONBLOCK_INFO_DIALOG);
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
                }
            }
            else
            {
                currentProcess = 0;
                ProgressImage.fillAmount = 0;
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






    }
}