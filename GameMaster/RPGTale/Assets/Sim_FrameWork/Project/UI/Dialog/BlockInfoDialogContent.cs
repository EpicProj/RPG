using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sim_FrameWork.UI
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

            m_dialog = UIUtility.SafeGetComponent<BlockInfoDialog>(Transform);
            slotPanel = UIUtility.SafeGetComponent<DistrictContentSlotPanel>(m_dialog.DistrictSlotContent.transform);
            manuSlotPanel = UIUtility.SafeGetComponent<BlockManufactSlotPanel>(m_dialog.ManufactPanel.transform);
            InputSlotContent = manuSlotPanel.transform.Find("InputSlotContent");
            OutputSlotContent = manuSlotPanel.transform.Find("Output/OutputSlotContent");
            ByproductSlotContent = manuSlotPanel.transform.Find("Output/ByproductSlotContent");
            //Level

            LvValue = UIUtility.SafeGetComponent<Text>(m_dialog.LevelValue.transform.Find("Value"));
            m_dialog.InherentLevelText.text = FunctionBlockModule.GetCurrentInherentLevelName(manufactoryInfo.inherentLevelData);

            clostBtn = UIUtility.SafeGetComponent<Button>(GameObject.Find("Btn").transform);
            ProgressImage = UIUtility.SafeGetComponent<Image>(m_dialog.Processbar.transform.Find("Progress"));
            ProcessIndicator = UIUtility.SafeGetComponent<Text>(m_dialog.Processbar.transform.Find("Indicator"));
            AddBtnListener();
            InitBaseData();
            currentManuSpeed = manufactoryInfo.CurrentSpeed;
        }



        public override void OnShow(params object[] paralist)
        {
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

        public override bool OnMessage(UIMessage msg)
        {
            switch (msg.type)
            {
                case  UIMsgType.UpdateManuSlot:
                    ManufactFormulaInfo formulaInfo = (ManufactFormulaInfo)msg.content[0];
                    //Update Info
                    manufactoryInfo.formulaInfo = formulaInfo;

                    UpdateManuMaterialSlot(formulaInfo);
                    return true;
                case  UIMsgType.UpdateLevelInfo:
                    FunctionBlockLevelInfo levelInfo = (FunctionBlockLevelInfo)msg.content[0];
                    //Update Info
                    blockInfo.levelInfo = levelInfo;
                    UpdateLevel(levelInfo);
                    return true;
                case  UIMsgType.UpdateSpeedText:
                    //UpdateSpeed
                    float Addspeed = (float)msg.content[0];
                    manufactoryInfo.AddCurrentSpeed(Addspeed);
                    return true;

                default:
                    Debug.LogError("UI msg Error , msgID=" + msg.type);
                    return false;
            }
            
        }

    


        //Button
        private void AddBtnListener()
        {
            AddButtonClickListener(clostBtn, delegate () { HideInfoDialog();});
        }

        private void HideInfoDialog()
        {
            UIManager.Instance.HideWnd(UIPath.WindowPath.FUNCTIONBLOCK_INFO_DIALOG);
        }

     
        //LV and EXP
        public void UpdateLevel(FunctionBlockLevelInfo info)
        {
            SetLevel(info.currentBlockLevel);
            SetCurrentLvSlider(info);
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
                var districtBuild = ObjectManager.Instance.InstantiateObject(UIPath.PrefabPath.DISTRICT_BUILD_PREFAB_PATH);
                var element = UIUtility.SafeGetComponent<DistrictBuildElement>(districtBuild.transform);
                element.InitCostElementData(blockInfo.ActiveDistrictBuildList[i]);
                districtBuild.transform.SetParent(m_dialog.BuildContent.transform, false);
            }
        }




    }
}