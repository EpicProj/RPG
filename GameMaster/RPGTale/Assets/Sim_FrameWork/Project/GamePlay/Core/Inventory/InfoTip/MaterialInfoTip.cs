using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sim_FrameWork
{
    public class MaterialInfoTip : MonoBehaviour
    {
        private Image TitleSprite;
        private Text Name;
        private Text Desc;
        private Image TypeIcon;
        private Text TypeName;
        private Image SubTypeIcon;
        private Text SubTypeName;

        private Text Rarity;
        private CanvasGroup canvasGroup;

        private float targetAlpha = 0;
        public float smoothing = 1;

        private void Start()
        {
            TitleSprite = transform.Find("TitleBG/Icon").GetComponent<Image>();
            Name = transform.Find("TitleBG/Name").GetComponent<Text>();
            Desc = transform.Find("Desc").GetComponent<Text>();
            TypeIcon = transform.Find("Type/Icon").GetComponent<Image>();
            TypeName = transform.Find("Type/Type").GetComponent<Text>();
            SubTypeIcon= transform.Find("Type/SubIcon").GetComponent<Image>();
            SubTypeName= transform.Find("Type/SubType").GetComponent<Text>();
            Rarity = transform.Find("Rarity/Text").GetComponent<Text>();
            canvasGroup = GetComponent<CanvasGroup>();
        }

        void Update()
        {
            if (canvasGroup.alpha != targetAlpha)
            {
                canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, targetAlpha, smoothing * Time.deltaTime);
                if (Mathf.Abs(canvasGroup.alpha - targetAlpha) < 0.01f)
                {
                    canvasGroup.alpha = targetAlpha;
                }
            }
        }

        public void OnShow(Material ma)
        {
            TitleSprite.sprite = MaterialModule.GetMaterialSprite(ma.MaterialID);
            Name.text = MaterialModule.GetMaterialName(ma);
            Desc.text = MaterialModule.GetmaterialDesc(ma);
            TypeIcon.sprite = MaterialModule.GetMaterialMainTypeSprite(ma);
            TypeName.text = MaterialModule.GetMaterialMainTypeName(ma);
            SubTypeIcon.sprite = MaterialModule.GetMaterialSubTypeIcon(ma);
            SubTypeName.text = MaterialModule.GetMaterialSubTypeName(ma);
            Rarity.text = MaterialModule.Instance.GetMaterialRarityName(ma);
            Rarity.color = MaterialModule.Instance.TryParseRarityColor(ma);
          
            targetAlpha = 1;
        }

        public void Hide()
        {
            targetAlpha = 0;
        }

        public void SetLocalPosition(Vector3 position)
        {
            transform.localPosition = position;
        }

    }
}