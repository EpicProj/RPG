using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace Sim_FrameWork
{
    public class GeneralChooseTab : MonoBehaviour
    {
        private Image _icon;
        private Button _btn;
        private Text _text;

        public string Type;
        private void Awake()
        {
            _icon = UIUtility.SafeGetComponent<Image>(UIUtility.FindTransfrom(transform, "BGLine/Icon"));
            _text = UIUtility.SafeGetComponent<Text>(UIUtility.FindTransfrom(transform, "BGLine/Name"));
            _btn = UIUtility.SafeGetComponent<Button>(transform);
        }

        /// <summary>
        /// Ship Part Tab
        /// </summary>
        /// <param name="partTypeData"></param>
        public void SetUpTab(Config.AssemblePartMainType partTypeData , bool isDesignPage)
        {
            _btn.onClick.RemoveAllListeners();
            if (partTypeData != null)
            {
                Type = partTypeData.Type;
                _icon.sprite = Utility.LoadSprite(partTypeData.IconPath);
                _text.text = MultiLanguage.Instance.GetTextValue(partTypeData.TypeName);
                _btn.onClick.AddListener(()=> OnPartBtnClick(isDesignPage));
            }
        }

        void OnPartBtnClick(bool isDesignPage)
        {
            AudioManager.Instance.PlaySound(AudioClipPath.UISound.Button_Click);
            if (isDesignPage)
            {
                UIManager.Instance.SendMessageToWnd(UIPath.WindowPath.Assemble_Part_Design_Page, new UIMessage(UIMsgType.Assemble_PartTab_Select, new List<object>() { Type }));
            }
            else
            {
                UIManager.Instance.SendMessageToWnd(UIPath.WindowPath.Assemble_Part_Choose_Dialog, new UIMessage(UIMsgType.Assemble_PartTab_Select_ChooseDialog, new List<object>() { Type }));
            }
          
        }

        /// <summary>
        /// Ship MainType Tab
        /// </summary>
        /// <param name="shipTypeData"></param>
        public void SetUpTab(Config.AssembleShipMainType shipTypeData)
        {
            _btn.onClick.RemoveAllListeners();
            if (shipTypeData != null)
            {
                Type = shipTypeData.Type;
                _icon.sprite = Utility.LoadSprite(shipTypeData.IconPath);
                _text.text = MultiLanguage.Instance.GetTextValue(shipTypeData.TypeName);
                _btn.onClick.AddListener(OnShipBtnClick);
            }
        }

        void OnShipBtnClick()
        {
            AudioManager.Instance.PlaySound(AudioClipPath.UISound.Button_Click);
            UIManager.Instance.SendMessageToWnd(UIPath.WindowPath.Assemble_Ship_Design_Page, new UIMessage(UIMsgType.Assemble_ShipTab_Select, new List<object>() { Type }));
        }

    }
}