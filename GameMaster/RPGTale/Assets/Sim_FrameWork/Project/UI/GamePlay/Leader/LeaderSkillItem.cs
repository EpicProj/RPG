using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Sim_FrameWork {
    public class LeaderSkillItem : BaseElementSimple
    {
        private LeaderSkillInfo _info;

        private Image _icon;
        private Text _skillName;
        private Slider _slider;

        private Text _levelText;

        private const string LeaderSkill_LV_Text = "LeaderSkill_LV_Text";
        public override void Awake()
        {
            _icon = transform.FindTransfrom("Icon").SafeGetComponent<Image>();
            _skillName = transform.FindTransfrom("Name").SafeGetComponent<Text>();
            _slider = transform.FindTransfrom("Slider").SafeGetComponent<Slider>();
            _levelText = transform.FindTransfrom("Level").SafeGetComponent<Text>();
        }

        public void SetUpItem(LeaderSkillInfo info)
        {
            if (info == null)
                return;
            _info = info;
            _icon.sprite = Utility.LoadSprite(info.skillIconPath);
            _skillName.text = info.skillName;

            _levelText.text = Utility.ParseStringParams(MultiLanguage.Instance.GetTextValue(LeaderSkill_LV_Text),
                new string[] { _info.currentLevel.ToString() });

            ///Fade
            var canvas = transform.SafeGetComponent<CanvasGroup>();
            canvas.alpha = 0;
            canvas.DoCanvasFade(1, 0.8f);
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);
        }

    }
}