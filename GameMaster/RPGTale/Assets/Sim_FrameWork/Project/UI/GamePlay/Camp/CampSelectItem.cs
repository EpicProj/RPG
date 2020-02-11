using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Sim_FrameWork
{
    public class CampSelectItem : BaseElement
    {
        private int currentSelcetID;
        public CampBaseModel _model;
        public override void Awake()
        {
            base.Awake();
            
            currentSelcetID = Config.ConfigData.GlobalSetting.CampChoosePage_DefaultSelect_CampID;
            
        }

        public override void ChangeAction(BaseDataModel model)
        {
            _model = (CampBaseModel)model;
            SetUpElement();
            SetSelect();
        }

        void SetUpElement()
        {
            var info = _model.CampInfo;
            if (info != null)
            {
                transform.FindTransfrom("BG").SafeGetComponent<Image>().sprite = Utility.LoadSprite(info.campBGSmallPath);
                transform.FindTransfrom("Bottom/Name").SafeGetComponent<Text>().text = info.campName;
                transform.FindTransfrom("Bottom/Name/Icon").SafeGetComponent<Image>().sprite = Utility.LoadSprite(info.campIconPath);
                transform.FindTransfrom("HardLevelInfo/Text").SafeGetComponent<Text>().text = info.hardLevel.ToString();
                AddButtonClickListener(transform.SafeGetComponent<Button>(), OnElementClick);
            }
            DoFadeAnim();
        }

        void OnElementClick()
        {
            transform.FindTransfrom("SelectEffect").SafeSetActive(true);
            UIManager.Instance.SendMessageToWnd(UIPath.WindowPath.Camp_SelectMainPage, new UIMessage(UIMsgType.CampSelectPage_SelectCamp, new List<object>() { _model.ID }));
        }

        public void SetSelect(int selectID=0)
        {
            if(selectID!=0)
                currentSelcetID = selectID;

            if (_model.ID == currentSelcetID)
            {
                transform.FindTransfrom("SelectEffect").SafeSetActive(true);
            }
            else
            {
                transform.FindTransfrom("SelectEffect").SafeSetActive(false);
            }
        }
        
        void DoFadeAnim()
        {
            var canvas = transform.SafeGetComponent<CanvasGroup>();
            canvas.alpha = 0;
            canvas.DoCanvasFade(1, 0.8f);
        }
    }
}