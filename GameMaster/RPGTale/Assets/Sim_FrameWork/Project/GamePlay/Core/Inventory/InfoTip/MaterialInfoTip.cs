using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sim_FrameWork
{
    public class MaterialInfoTip : MonoBehaviour
    {
        private Text Name;
        private Text Desc;
        private Image TypeIcon;
        private Text TypeName;
        private CanvasGroup canvasGroup;

        private float targetAlpha = 0;
        public float smoothing = 1;

        private void Start()
        {
            Name = transform.Find("TitleBG/Name").GetComponent<Text>();
            Desc = transform.Find("Desc").GetComponent<Text>();
            TypeIcon = transform.Find("Type/Icon").GetComponent<Image>();
            TypeName = transform.Find("Type/Type").GetComponent<Text>();
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
            Name.text = MaterialModule.Instance.GetMaterialName(ma);
            Desc.text = MaterialModule.Instance.GetmaterialDesc(ma);
            TypeIcon.sprite = MaterialModule.Instance.GetMaterialMainTypeSprite(ma);
            TypeName.text = MaterialModule.Instance.GetMaterialMainTypeName(ma);

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