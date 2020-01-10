using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace Sim_FrameWork
{
    public class AssemblePartChooseTab : MonoBehaviour
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


        public void SetUpTab(Config.AssemblePartMainType partTypeData)
        {
            _btn.onClick.RemoveAllListeners();
            if (partTypeData != null)
            {
                Type = partTypeData.Type;
                _icon.sprite = Utility.LoadSprite(partTypeData.IconPath, Utility.SpriteType.png);
                _text.text = MultiLanguage.Instance.GetTextValue(partTypeData.TypeName);
                _btn.onClick.AddListener(OnPartBtnClick);
            }
        }

        public void SetUpTab(Config.AssembleShipMainType shipTypeData)
        {
            _btn.onClick.RemoveAllListeners();
            if (shipTypeData != null)
            {
                Type = shipTypeData.Type;
                _icon.sprite = Utility.LoadSprite(shipTypeData.IconPath, Utility.SpriteType.png);
                _text.text = MultiLanguage.Instance.GetTextValue(shipTypeData.TypeName);
                _btn.onClick.AddListener(OnShipBtnClick);
            }
        }

        void OnPartBtnClick()
        {
            AudioManager.Instance.PlaySound(AudioClipPath.UISound.Button_Click);
            UIManager.Instance.SendMessageToWnd(UIPath.WindowPath.Assemble_Part_Design_Page, new UIMessage(UIMsgType.Assemble_PartTab_Select, new List<object>() { Type }));
        }

        void OnShipBtnClick()
        {
            AudioManager.Instance.PlaySound(AudioClipPath.UISound.Button_Click);
            UIManager.Instance.SendMessageToWnd(UIPath.WindowPath.Assemble_Ship_Design_Page, new UIMessage(UIMsgType.Assemble_ShipTab_Select, new List<object>() { Type }));
        }
    }
}