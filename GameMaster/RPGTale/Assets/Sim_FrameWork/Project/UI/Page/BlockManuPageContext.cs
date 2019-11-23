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

        private FunctionBlockBase blockBase;
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

        private FormulaContentCmpt _formulaContentCpmt;
        //Formula
        bool isfirstChoose = true;

        #region Override Method
        public override void Awake(params object[] paralist)
        {
            blockBase = (FunctionBlockBase)paralist[0];
            manufactoryInfo = (ManufactoryInfo)paralist[1];
            formulaInfo = (ManufactFormulaInfo)paralist[2];
            m_page = UIUtility.SafeGetComponent<BlockManuPage>(Transform);

            InitTransformRef();
            AddButtonListener();
            InitInfoPanel();
        }


        public override void OnShow(params object[] paralist)
        {
            blockBase = (FunctionBlockBase)paralist[0];
            manufactoryInfo = (ManufactoryInfo)paralist[1];
            formulaInfo = (ManufactFormulaInfo)paralist[2];

            AudioManager.Instance.PlaySound(AudioClipPath.UISound.Page_Open);
            RefreshInfoAll(manufactoryInfo);
            if (!formulaInfo.NotChoose)
            {
                _formulaContentCpmt.RefreshFormulaSlotNum(formulaInfo);
            }
            UpdateLevel();
            RefreshFormulaChoose();
        }

        public override bool OnMessage(UIMessage msg)
        {
            switch (msg.type)
            {
                case UIMsgType.UpdateManuSlot:
                    ManufactFormulaInfo info = (ManufactFormulaInfo)msg.content[0];
                    //Update Info
                    formulaInfo = info;
                    _formulaContentCpmt.RefreshFormulaSlotNum(formulaInfo);
                    return true;
                case UIMsgType.UpdateLevelInfo:
                    blockBase.info.levelInfo = (FunctionBlockLevelInfo)msg.content[0];
                    UpdateLevel();
                    return true;
                case UIMsgType.UpdateSpeedText:
                    //UpdateSpeed
                    float Addspeed = (float)msg.content[0];
                    manufactoryInfo.AddCurrentSpeed(Addspeed);
                    RefreshInfoText(manufactoryInfo.CurrentSpeed, InfoType.Speed);
                    return true;

                case UIMsgType.ProductLine_Formula_Change:
                    int formulaID = (int)msg.content[0];
                    return RefreshFormula(formulaID);
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

            _formulaContentCpmt = UIUtility.SafeGetComponent<FormulaContentCmpt>(m_page.ManuContent);
        }

        void AddButtonListener()
        {
            AddButtonClickListener(m_page.BackBtn, () =>
            {
                UIManager.Instance.HideWnd(UIPath.WindowPath.BlockManu_Page);
            });
            AddButtonClickListener(m_page.FormulaChange, () =>
            {
                PopUpFormulaChooseDialog();
            });

            ///初始化选择
            AddButtonClickListener(m_page.FormulaChooseBtn, () =>
            {
                PopUpFormulaChooseDialog();
            });
        }



        void InitInfoPanel()
        {
            if (blockBase == null)
                return;

            SpeedText = UIUtility.SafeGetComponent<Text>(m_page.BlockInfoContent.transform.Find("BaseInfo/Speed/Value"));
            EnergyText = UIUtility.SafeGetComponent<Text>(m_page.BlockInfoContent.Find("BaseInfo/Energy/Value"));
            MaintianText = UIUtility.SafeGetComponent<Text>(m_page.BlockInfoContent.Find("BaseInfo/Maintain/Value"));
            WorkerText = UIUtility.SafeGetComponent<Text>(m_page.BlockInfoContent.Find("BaseInfo/Worker/Value"));

            RefreshInfoAll(manufactoryInfo);
            m_page.Title.transform.Find("Text").GetComponent<Text>().text = blockBase.info.dataModel.Name;
            m_page.Title.transform.Find("Text/Icon").GetComponent<Image>().sprite = blockBase.info.dataModel.Icon;
            m_page.BlockBG.sprite = blockBase.info.dataModel.BG;
            m_page.BlockDesc.text = blockBase.info.dataModel.Desc;

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


        void RefreshFormulaChoose()
        {
            var manu = UIUtility.SafeGetComponent<CanvasGroup>(m_page.ManuContent);
            var choose = UIUtility.SafeGetComponent<CanvasGroup>(m_page.FormulaChooseBtn.transform);
            if (isfirstChoose)
            {
                UIUtility.ActiveCanvasGroup(manu, false);
                m_page.FormulaChange.gameObject.SetActive(false);

                UIUtility.ActiveCanvasGroup(choose, true);
            }
            else
            {
                UIUtility.ActiveCanvasGroup(manu, true);
                m_page.FormulaChange.gameObject.SetActive(true);

                UIUtility.ActiveCanvasGroup(choose, false);
            }
        }

        void PopUpFormulaChooseDialog()
        {
            UIManager.Instance.PopUpWnd(UIPath.WindowPath.ProductLine_Change_Dialog, WindowType.Dialog, true, formulaInfo.FormulaChooseList, isfirstChoose);
            AudioManager.Instance.PlaySound(AudioClipPath.UISound.Page_Open);
        }

        /// <summary>
        /// 更换生产线
        /// </summary>
        /// <param name="formulaID"></param>
        /// <returns></returns>
        bool RefreshFormula(int formulaID)
        {
            if (FormulaModule.GetFormulaDataByID(formulaID) == null)
                return false;
            ManufactFormulaInfo info = new ManufactFormulaInfo(formulaID, blockBase.info.block);
            formulaInfo = info;
            isfirstChoose = false;
            RefreshFormulaChoose();
            _formulaContentCpmt.Init(info, FormulaContentCmpt.InitType.Normal);
        
            var block= FunctionBlockManager.Instance.GetFunctionBlockObject(blockBase.instanceID);
            if (block != null)
            {
                var manuBase = UIUtility.SafeGetComponent<ManufactoryBase>(block.transform);
                manuBase.formulaInfo = info;
            }

            return true;
        }


        /// <summary>
        /// 更新生产进度条
        /// </summary>
        /// <param name="info"></param>
        public void UpdateProgress(ManufactFormulaInfo info)
        {
            if (info.NotChoose)
                return;
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
            LVText.text = blockBase.info.levelInfo.currentBlockLevel.ToString();
            SetLVSlider();
            EXPValueText.text = string.Format("{0}/{1}", blockBase.info.levelInfo.CurrentBlockExp.ToString(), blockBase.info.levelInfo.CurrentBlockMaxEXP.ToString());
        }


        private void SetLVSlider()
        {
            var sliderGroup = UIUtility.SafeGetComponent<SliderGroup>(UIUtility.FindTransfrom(m_page.EXPContent, "Content"));
            float progress = (float)blockBase.info.levelInfo.CurrentBlockExp / (float)blockBase.info.levelInfo.CurrentBlockMaxEXP;
            sliderGroup.RefreshSlider(progress);
            UIUtility.SafeGetComponent<Text>(UIUtility.FindTransfrom(m_page.EXPContent, "Percent")).text = ((int)(progress * 100)).ToString();
        }

    }
}