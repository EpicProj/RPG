using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork.UI
{
    public class ProductLineChangeDialogContext : WindowBase
    {

        private ProductLineChangeDialog m_dialog;


        private List<FormulaData> _formulaList = new List<FormulaData>();
        private bool _isFirstChoose=true;
        private int _currentFormulaID = 0;

        private const string FormulaChange_Dialog_Title_Change = "FormulaChange_Dialog_Title_Change";
        private const string FormulaChange_Dialog_Title_Choose = "FormulaChange_Dialog_Title_Choose";

        public override void Awake(params object[] paralist)
        {
            m_dialog = UIUtility.SafeGetComponent<ProductLineChangeDialog>(Transform);
            _formulaList = (List<FormulaData>)paralist[0];
            _isFirstChoose = (bool)paralist[1];
            m_dialog.Title.text = MultiLanguage.Instance.GetTextValue(_isFirstChoose ? FormulaChange_Dialog_Title_Choose : FormulaChange_Dialog_Title_Change);

            AddBtnListener();
        }

        public override void OnShow(params object[] paralist)
        {
            _formulaList = (List<FormulaData>)paralist[0];
            _isFirstChoose = (bool)paralist[1];
            
        }

        public override bool OnMessage(UIMessage msg)
        {
            return base.OnMessage(msg);
        }


        private void AddBtnListener()
        {
            AddButtonClickListener(m_dialog.CloseBtn, () =>
            {
                UIManager.Instance.HideWnd(UIPath.WindowPath.ProductLine_Change_Dialog);
            });
        }
    }

}