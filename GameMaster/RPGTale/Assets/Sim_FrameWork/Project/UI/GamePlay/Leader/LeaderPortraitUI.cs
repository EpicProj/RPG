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

        bool sp_bg_finish = false;
        bool sp_face_finish = false;
        bool sp_hair_finish = false;
        bool sp_cloth_finish = false;
        bool sp_mouth_finish = false;
        bool sp_eyes_finish = false;
        bool sp_ear_finish = false;
        bool sp_nose_finish = false;

        public void Awake()
        {
            _bg = transform.FindTransfrom("Content/BG").SafeGetComponent<Image>();
            _face = transform.FindTransfrom("Content/Face").SafeGetComponent<Image>();
            _hair = transform.FindTransfrom("Content/Hair").SafeGetComponent<Image>();
            _cloth = transform.FindTransfrom("Content/Cloth").SafeGetComponent<Image>();
            _mouth = transform.FindTransfrom("Content/Mouth").SafeGetComponent<Image>();
            _eyes = transform.FindTransfrom("Content/Eyes").SafeGetComponent<Image>();
            _ear = transform.FindTransfrom("Content/Ear").SafeGetComponent<Image>();
            _nose = transform.FindTransfrom("Content/Nose").SafeGetComponent<Image>();
           
        }

        void LoadStartEffect()
        {
            transform.FindTransfrom("Content").SafeGetComponent<CanvasGroup>().ActiveCanvasGroup(false);
            transform.FindTransfrom("Loading").SafeSetActive(true);
        }

        public void SetUpItem(LeaderPortraitInfo info)
        {
            ///同时更换用异步
            LoadStartEffect();
            AysncLoadSprite(info.portrait_bg.spritePath,
                info.portrait_face.spritePath,
                info.portrait_hair.spritePath,
                info.portrait_cloth.spritePath,
                info.portrait_mouth.spritePath,
                info.portrait_eyes.spritePath,
                info.portrait_ear.spritePath,
                info.portrait_nose.spritePath);
        }
        public void RefreshSprite(LeaderPortraitType type, Sprite sp)
        {
            switch (type)
            {
                case LeaderPortraitType.BG:
                    _bg.sprite = sp;
                    break;
                case LeaderPortraitType.Cloth:
                    _cloth.sprite = sp;
                    break;
                case LeaderPortraitType.Ear:
                    _ear.sprite = sp;
                    break;
                case LeaderPortraitType.Eyes:
                    _eyes.sprite = sp;
                    break;
                case LeaderPortraitType.Face:
                    _face.sprite = sp;
                    break;
                case LeaderPortraitType.Hair:
                    _hair.sprite = sp;
                    break;
                case LeaderPortraitType.Mouth:
                    _mouth.sprite = sp;
                    break;
                case LeaderPortraitType.Nose:
                    _nose.sprite = sp;
                    break;
            }
        }

        #region AsyncLoad
        void AysncLoadSprite(string spbg,string spface, string sphair, string spcloth, string spmouth, string speyes, string spear, string spnose)
        {
            sp_bg_finish = false;
            sp_face_finish = false;
            sp_hair_finish = false;
            sp_cloth_finish = false;
            sp_mouth_finish = false;
            sp_eyes_finish = false;
            sp_ear_finish = false;
            sp_nose_finish = false;

            ResourceManager.Instance.AsyncLoadResource("Assets/" + spbg + ".png", OnSpriteBg_LoadFinish, LoadResPriority.RES_HIGHT,true);
            ResourceManager.Instance.AsyncLoadResource("Assets/" + spface + ".png", OnSpriteFace_LoadFinish, LoadResPriority.RES_HIGHT, true);
            ResourceManager.Instance.AsyncLoadResource("Assets/" + sphair + ".png", OnSpriteHair_LoadFinish, LoadResPriority.RES_HIGHT, true);
            ResourceManager.Instance.AsyncLoadResource("Assets/" + spcloth + ".png", OnSpriteCloth_LoadFinish, LoadResPriority.RES_HIGHT, true);
            ResourceManager.Instance.AsyncLoadResource("Assets/" + spmouth + ".png", OnSpriteMouth_LoadFinish, LoadResPriority.RES_HIGHT, true);
            ResourceManager.Instance.AsyncLoadResource("Assets/" + speyes + ".png", OnSpriteEyes_LoadFinish, LoadResPriority.RES_HIGHT, true);
            ResourceManager.Instance.AsyncLoadResource("Assets/" + spear + ".png", OnSpriteEar_LoadFinish, LoadResPriority.RES_HIGHT, true);
            ResourceManager.Instance.AsyncLoadResource("Assets/" + spnose + ".png", OnSpriteNose_LoadFinish, LoadResPriority.RES_HIGHT, true);      
        }

        private void OnSpriteBg_LoadFinish(string path, Object obj, object param1, object param2, object param3)
        {
            if (obj != null)
            {
                Sprite sp_bg = obj as Sprite;
                _bg.sprite = sp_bg;
                sp_bg_finish = true;
                if (sp_bg_finish && sp_face_finish && sp_hair_finish && sp_cloth_finish && sp_mouth_finish && sp_eyes_finish && sp_ear_finish && sp_nose_finish)
                {
                    transform.FindTransfrom("Loading").SafeSetActive(false);
                    transform.FindTransfrom("Content").SafeGetComponent<CanvasGroup>().ActiveCanvasGroup(true);
                }
            }
        }
        private void OnSpriteFace_LoadFinish(string path, Object obj, object param1, object param2, object param3)
        {
            if (obj != null)
            {
                Sprite sp_face = obj as Sprite;
                _face.sprite = sp_face;
                sp_face_finish = true;
                if (sp_bg_finish && sp_face_finish && sp_hair_finish && sp_cloth_finish && sp_mouth_finish && sp_eyes_finish && sp_ear_finish && sp_nose_finish)
                {
                    transform.FindTransfrom("Loading").SafeSetActive(false);
                    transform.FindTransfrom("Content").SafeGetComponent<CanvasGroup>().ActiveCanvasGroup(true);
                }
            }
        }
        private void OnSpriteHair_LoadFinish(string path, Object obj, object param1, object param2, object param3)
        {
            if (obj != null)
            {
                Sprite sp_hair = obj as Sprite;
                _hair.sprite = sp_hair;
                sp_hair_finish = true;
                if (sp_bg_finish && sp_face_finish && sp_hair_finish && sp_cloth_finish && sp_mouth_finish && sp_eyes_finish && sp_ear_finish && sp_nose_finish)
                {
                    transform.FindTransfrom("Loading").SafeSetActive(false);
                    transform.FindTransfrom("Content").SafeGetComponent<CanvasGroup>().ActiveCanvasGroup(true);
                }
            }
        }
        private void OnSpriteCloth_LoadFinish(string path, Object obj, object param1, object param2, object param3)
        {
            if (obj != null)
            {
                Sprite sp_cloth = obj as Sprite;
                _cloth.sprite = sp_cloth;
                sp_cloth_finish = true;
                if (sp_bg_finish && sp_face_finish && sp_hair_finish && sp_cloth_finish && sp_mouth_finish && sp_eyes_finish && sp_ear_finish && sp_nose_finish)
                {
                    transform.FindTransfrom("Loading").SafeSetActive(false);
                    transform.FindTransfrom("Content").SafeGetComponent<CanvasGroup>().ActiveCanvasGroup(true);
                }
            }
        }
        private void OnSpriteMouth_LoadFinish(string path, Object obj, object param1, object param2, object param3)
        {
            if (obj != null)
            {
                Sprite sp_mouth = obj as Sprite;
                _mouth.sprite = sp_mouth;
                sp_mouth_finish = true;
                if (sp_bg_finish && sp_face_finish && sp_hair_finish && sp_cloth_finish && sp_mouth_finish && sp_eyes_finish && sp_ear_finish && sp_nose_finish)
                {
                    transform.FindTransfrom("Loading").SafeSetActive(false);
                    transform.FindTransfrom("Content").SafeGetComponent<CanvasGroup>().ActiveCanvasGroup(true);
                }
            }
        }
        private void OnSpriteEyes_LoadFinish(string path, Object obj, object param1, object param2, object param3)
        {
            if (obj != null)
            {
                Sprite sp_eyes = obj as Sprite;
                _eyes.sprite = sp_eyes;
                sp_eyes_finish = true;
                if (sp_bg_finish && sp_face_finish && sp_hair_finish && sp_cloth_finish && sp_mouth_finish && sp_eyes_finish && sp_ear_finish && sp_nose_finish)
                {
                    transform.FindTransfrom("Loading").SafeSetActive(false);
                    transform.FindTransfrom("Content").SafeGetComponent<CanvasGroup>().ActiveCanvasGroup(true);
                }
            }
        }
        private void OnSpriteEar_LoadFinish(string path, Object obj, object param1, object param2, object param3)
        {
            if (obj != null)
            {
                Sprite sp_ear = obj as Sprite;
                _ear.sprite = sp_ear;
                sp_ear_finish = true;
                if (sp_bg_finish && sp_face_finish && sp_hair_finish && sp_cloth_finish && sp_mouth_finish && sp_eyes_finish && sp_ear_finish && sp_nose_finish)
                {
                    transform.FindTransfrom("Loading").SafeSetActive(false);
                    transform.FindTransfrom("Content").SafeGetComponent<CanvasGroup>().ActiveCanvasGroup(true);
                }
            }
        }
        private void OnSpriteNose_LoadFinish(string path, Object obj, object param1, object param2, object param3)
        {
            if (obj != null)
            {
                Sprite sp_nose = obj as Sprite;
                _nose.sprite = sp_nose;
                sp_nose_finish = true;
                if (sp_bg_finish && sp_face_finish && sp_hair_finish && sp_cloth_finish && sp_mouth_finish && sp_eyes_finish && sp_ear_finish && sp_nose_finish)
                {
                    transform.FindTransfrom("Loading").SafeSetActive(false);
                    transform.FindTransfrom("Content").SafeGetComponent<CanvasGroup>().ActiveCanvasGroup(true);
                }
            }
        }
        #endregion
    
    }
}