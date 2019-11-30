using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sim_FrameWork.UI
{
    public class ProductLineChangeDialogContext : WindowBase
    {

        private ProductLineChangeDialog m_dialog;
        private FormulaContentCmpt _formulaContentCmpt;

        private List<FormulaData> _formulaList = new List<FormulaData>();
        private bool _isFirstChoose=true;
        private int _currentFormulaID = 0;

        private Text _formulaName;
        private Text _timeText;
        private Text _desc;
        private LoopList _loopList;
        private TypeWriterEffect typeWriter;

        private const string FormulaChange_Dialog_Title_Change = "FormulaChange_Dialog_Title_Change";
        private const string FormulaChange_Dialog_Title_Choose = "FormulaChange_Dialog_Title_Choose";

        private const string FormulaChange_Success_Hint_Text = "FormulaChange_Success_Hint_Text";

        public override void Awake(params object[] paralist)
        {
            m_dialog = UIUtility.SafeGetComponent<ProductLineChangeDialog>(Transform);
            _formulaList = (List<FormulaData>)paralist[0];
            _isFirstChoose = (bool)paralist[1];
            m_dialog.Title.text = MultiLanguage.Instance.GetTextValue(_isFirstChoose ? FormulaChange_Dialog_Title_Choose : FormulaChange_Dialog_Title_Change);

            InitRef();
            AddBtnListener();
            SetDefaultSelect();
        }

        public override void OnShow(params object[] paralist)
        {
            _formulaList = (List<FormulaData>)paralist[0];
            _isFirstChoose = (bool)paralist[1];
            
        }

        public override void OnClose()
        {
            AudioManager.Instance.PlaySound(AudioClipPath.UISound.Btn_Close);
        }

        public override bool OnMessage(UIMessage msg)
        {
            switch (msg.type)
            {
                ///UpDate Formula
                case UIMsgType.ProductLine_Formula_Change:
                    _currentFormulaID = (int)msg.content[0];
                    HideAllSelect();
                    return RefreshFormula();
                default:
                    return false;
            }
        }


        private void AddBtnListener()
        {
            AddButtonClickListener(m_dialog.CloseBtn, () =>
            {
                UIManager.Instance.HideWnd(this);
            });

            AddButtonClickListener(m_dialog.ConfirmBtn, () =>
            {
                UIManager.Instance.SendMessageToWnd(UIPath.WindowPath.BlockManu_Page, new UIMessage(UIMsgType.ProductLine_Formula_Change, new List<object>(1) { _currentFormulaID }));
                AudioManager.Instance.PlaySound(AudioClipPath.UISound.Button_Click);

                UIManager.Instance.ShowGeneralHint(FormulaChange_Success_Hint_Text, 1.0f);
                UIManager.Instance.HideWnd(this);
            });
        }
        private void InitRef()
        {
            _formulaName = UIUtility.SafeGetComponent<Text>(UIUtility.FindTransfrom(m_dialog.ManuContent,"Name"));
            _timeText = UIUtility.SafeGetComponent<Text>(UIUtility.FindTransfrom(m_dialog.ManuContent, "Time"));
            _formulaContentCmpt = UIUtility.SafeGetComponent<FormulaContentCmpt>(UIUtility.FindTransfrom(m_dialog.ManuContent, "FormulaContent"));
            _desc = UIUtility.SafeGetComponent<Text>(UIUtility.FindTransfrom(m_dialog.ManuContent, "Desc"));
            _loopList = UIUtility.SafeGetComponent<LoopList>(m_dialog.ScrollViewTrans);
            typeWriter = UIUtility.SafeGetComponent<TypeWriterEffect>(_desc.transform);
        }



        bool RefreshFormula()
        {
            if (FormulaModule.GetFormulaDataByID(_currentFormulaID) == null)
                return false;

            ManufactFormulaInfo info = new ManufactFormulaInfo(_currentFormulaID);
            if (_formulaContentCmpt != null)
            {
                _formulaContentCmpt.Init(info, FormulaContentCmpt.InitType.FormulaChange);
                _formulaContentCmpt.RefreshManuElementTrans(info, FormulaContentCmpt.InitType.FormulaChange);
            }

            var currentFormulaData = FormulaModule.GetFormulaDataByID(_currentFormulaID);
            _timeText.text =currentFormulaData.ProductSpeed.ToString("0.0");
            _formulaName.text = FormulaModule.GetFormulaName(currentFormulaData);
            _desc.text = FormulaModule.GetFormulaDesc(currentFormulaData);
            typeWriter.StartEffect();
            return true;
        }

        void HideAllSelect()
        {
            var list = _loopList.ElementList;
            for(int i = 0; i < list.Count;i++)
            {
                FormulaChooseElement element = (FormulaChooseElement)list[i];
                element.Select(false);
            }
        }

        /// <summary>
        /// default select
        /// </summary>
        void SetDefaultSelect()
        {
            if (_formulaList.Count != 0)
            {
                _currentFormulaID = _formulaList[0].FormulaID;
                RefreshFormula();
                InitFormulaList();
            }
        }

        void InitFormulaList()
        {
            _loopList.InitData(GetFormulaModel());
            FormulaChooseElement element = (FormulaChooseElement)_loopList.ElementList[_loopList.ElementList.Count-1];
            element.Select(true);
        }
        

        List<List<BaseDataModel>> GetFormulaModel()
        {
            List<List<BaseDataModel>> result = new List<List<BaseDataModel>>();
            for(int i = _formulaList.Count -1; i >= 0 ; i--)
            {
                FormulaDataModel formula = new FormulaDataModel();
                formula.Create(_formulaList[i].FormulaID);
                result.Add(new List<BaseDataModel>(1) {formula });
            }
            return result;
        }

    }

}