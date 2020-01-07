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


        public void SetUpTab(Config.AssemblePartMainType typeData)
        {
            _btn.onClick.RemoveAllListeners();
            if (typeData != null)
            {
                Type = typeData.Type;
                _icon.sprite = Utility.LoadSprite(typeData.IconPath, Utility.SpriteType.png);
                _text.text = MultiLanguage.Instance.GetTextValue(typeData.TypeName);
                _btn.onClick.AddListener(OnBtnClick);
            }
         
        }

        void OnBtnClick()
        {
            AudioManager.Instance.PlaySound(AudioClipPath.UISound.Button_Click);
            UIManager.Instance.SendMessageToWnd(UIPath.WindowPath.Assemble_Part_Design_Page, new UIMessage(UIMsgType.Assemble_PartTab_Select, new List<object>() { Type }));
        }
    }
}