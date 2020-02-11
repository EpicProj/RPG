using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sim_FrameWork
{
    public class LeaderPortraitUI : MonoBehaviour
    {
        private Image _bg;
        private Image _face;
        private Image _hair;
        private Image _cloth;
        private Image _mouth;
        private Image _eyes;
        private Image _ear;
        private Image _nose;

        public void Awake()
        {
            _bg = transform.FindTransfrom("BG").SafeGetComponent<Image>();
            _face = transform.FindTransfrom("Face").SafeGetComponent<Image>();
            _hair = transform.FindTransfrom("Hair").SafeGetComponent<Image>();
            _cloth = transform.FindTransfrom("Cloth").SafeGetComponent<Image>();
            _mouth = transform.FindTransfrom("Mouth").SafeGetComponent<Image>();
            _eyes = transform.FindTransfrom("Eyes").SafeGetComponent<Image>();
            _ear = transform.FindTransfrom("Ear").SafeGetComponent<Image>();
            _nose = transform.FindTransfrom("Nose").SafeGetComponent<Image>();
        }

        public void SetUpItem(LeaderPortraitInfo info)
        {
            _bg.sprite = Utility.LoadSprite(info.portrait_bg.spritePath);
            _face.sprite = Utility.LoadSprite(info.portrait_face.spritePath);
            _hair.sprite = Utility.LoadSprite(info.portrait_hair.spritePath);
            _cloth.sprite = Utility.LoadSprite(info.portrait_cloth.spritePath);
            _mouth.sprite = Utility.LoadSprite(info.portrait_mouth.spritePath);
            _eyes.sprite = Utility.LoadSprite(info.portrait_eyes.spritePath);
            _ear.sprite = Utility.LoadSprite(info.portrait_ear.spritePath);
            _nose.sprite = Utility.LoadSprite(info.portrait_nose.spritePath);
        }
    }
}