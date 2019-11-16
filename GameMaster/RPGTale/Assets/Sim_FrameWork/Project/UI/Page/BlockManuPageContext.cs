using System.Collections;
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

        public override void Awake(params object[] paralist)
        {
            blockInfo = (FunctionBlockInfoData)paralist[0];
            manufactoryInfo = (ManufactoryInfo)paralist[1];

            m_page = UIUtility.SafeGetComponent<BlockManuPage>(Transform);
            ProgressRingImage = UIUtility.SafeGetComponent<Image>(UIUtility.FindTransfrom(m_page.ProgressTrans, "Ring"));
            ProgressValueText = UIUtility.SafeGetComponent<Text>(UIUtility.FindTransfrom(m_page.ProgressTrans, "Value"));
            EXPValueText = UIUtility.SafeGetComponent<Text>(UIUtility.FindTransfrom(m_page.EXPContent, "Value"));
            EXPValuePercentText = UIUtility.SafeGetComponent<Text>(UIUtility.FindTransfrom(m_page.EXPContent, "Percent"));
            LVText = UIUtility.SafeGetComponent<Text>(UIUtility.FindTransfrom(m_page.BlockLevelInfo, "Value"));

            AddButtonListener();
            InitInfoPanel();
        }



        public override void OnShow(params object[] paralist)
        {         
            blockInfo = (FunctionBlockInfoData)paralist[0];
            manufactoryInfo = (ManufactoryInfo)paralist[1];
            AudioManager.Instance.PlaySound(AudioClipPath.UISound.Page_Open);
        }

        public override bool OnMessage(UIMessage msg)
        {
            switch (msg.type)
            {
                case UIMsgType.UpdateManuSlot:
                    ManufactFormulaInfo formulaInfo = (ManufactFormulaInfo)msg.content[0];
                    //Update Info
                    manufactoryInfo.formulaInfo = formulaInfo;

                    UpdateManuMaterialSlot(formulaInfo);
                    return true;
                case UIMsgType.UpdateLevelInfo:
                    FunctionBlockLevelInfo levelInfo = (FunctionBlockLevelInfo)msg.content[0];
                    //Update Info
                    blockInfo.levelInfo = levelInfo;
                    //UpdateLevel(levelInfo);
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
            UpdateProgress(manufactoryInfo.formulaInfo);
        }


        void AddButtonListener()
        {
            AddButtonClickListener(m_page.BackBtn, () =>
            {
                UIManager.Instance.HideWnd(UIPath.WindowPath.BlockManu_Page);
            });
        }


        private void InitInfoPanel()
        {
            if (blockInfo == null)
                return;

            SpeedText = UIUtility.SafeGetComponent<Text>(m_page.BlockInfoContent.transform.Find("BaseInfo/Speed/Value"));
            EnergyText = UIUtility.SafeGetComponent<Text>(m_page.BlockInfoContent.Find("BaseInfo/Energy/Value"));
            MaintianText = UIUtility.SafeGetComponent<Text>(m_page.BlockInfoContent.Find("BaseInfo/Maintain/Value"));
            WorkerText = UIUtility.SafeGetComponent<Text>(m_page.BlockInfoContent.Find("BaseInfo/Worker/Value"));
           
            RefreshInfoText(manufactoryInfo.CurrentSpeed, InfoType.Speed);
            RefreshInfoText(manufactoryInfo.EnergyCostNormal, InfoType.Energy);
            RefreshInfoText(manufactoryInfo.WorkerNum, InfoType.Worker);
            RefreshInfoText(manufactoryInfo.Maintain, InfoType.Maintain);

            m_page.Title.transform.Find("Text").GetComponent<Text>().text = blockInfo.dataModel.Name;
            m_page.Title.transform.Find("Text/Icon").GetComponent<Image>().sprite = blockInfo.dataModel.Icon;
            m_page.BlockBG.sprite = blockInfo.dataModel.BG;
            m_page.BlockDesc.text = blockInfo.dataModel.Desc;
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

        public void UpdateManuMaterialSlot(ManufactFormulaInfo info)
        {
            Dictionary<Material, ushort> InputDic = info.realInputDataDic;
            Dictionary<Material, ushort> OutputDic = info.realOutputDataDic;
            //Update Input
            //foreach (KeyValuePair<Material, ushort> kvp in InputDic)
            //{
            //    manuSlotPanel.InitManuInputMaterialSlot(kvp.Key, kvp.Value);
            //}
            //Update Output
            //foreach (KeyValuePair<Material, ushort> kvp in OutputDic)
            //{
            //    manuSlotPanel.InitManuOutputMaterialSlot(kvp.Key, kvp.Value);
            //}


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
        /// LV Info
        /// </summary>
        /// <param name="info"></param>
        private void SetLvValue(FunctionBlockLevelInfo info)
        {
            EXPValueText.text = string.Format("{0}/{1}", info.CurrentBlockExp.ToString(), info.CurrentBlockMaxEXP.ToString());
        }

        public void UpdateLevel(FunctionBlockLevelInfo info)
        {
            SetLevel(info.currentBlockLevel);
            //SetCurrentLvSlider(info);
            SetLvValue(info);
        }
        private void SetLevel(int level)
        {
            LVText.text = level.ToString();
        }

    }
}