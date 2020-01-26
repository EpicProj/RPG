using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Sim_FrameWork
{
    public class ExplorePointCmpt : BaseElementSimple
    {
        private const string ExplorePoint_Finish_Text = "ExplorePoint_Finish_Text";

        private bool isShowInfoTip=false;

        private Image _progressImage;
        private Text _progressTimeText;

        private Animation _showAnim;
        public ExplorePointData pointData;

        //Slider Lerp
        private int lerpSpeed = 10;
        private float currentTimeProgress = 0f;


        public override void Awake()
        {
            _showAnim = transform.FindTransfrom("Info").SafeGetComponent<Animation>();
            _progressImage = transform.FindTransfrom("Ring/Fill").SafeGetComponent<Image>();
            _progressImage.fillAmount = currentTimeProgress;
            _progressTimeText = transform.FindTransfrom("Ring/Progress").SafeGetComponent<Text>();
        }

        public void InitPoint(ExplorePointData data)
        {
            if (data != null)
            {
                var btn = transform.SafeGetComponent<Button>();
                btn.onClick.RemoveAllListeners();
                btn.onClick.AddListener(OnPointClick);

                pointData = data;
                transform.FindTransfrom("Info/Name").SafeGetComponent<Text>().text = data.pointName;
                transform.FindTransfrom("Info/Energy/Value").SafeGetComponent<Text>().text = data.EnergyCost.ToString();
                transform.FindTransfrom("Info/Time/Value").SafeGetComponent<Text>().text = data.TimeCost.ToString();
                _progressTimeText.text = "";
                var pos = SolarSystemManager.Instance.GetPointPositionUI(data);
                transform.GetComponent<RectTransform>().anchoredPosition = pos;
                
            }
        }

        public void RefreshPointTimer(ExplorePointData data)
        {
            if (data != null)
            {
                pointData = data;
                if (pointData.RemainTime <= 0)
                {
                    _progressTimeText.text = MultiLanguage.Instance.GetTextValue(ExplorePoint_Finish_Text);
                    _progressImage.fillAmount = 1;
                }
                else
                {
                    _progressTimeText.text = data.RemainTime.ToString() + " " + MultiLanguage.Instance.GetTextValue(Config.GeneralTextData.Game_Time_Text_Day);
                    StartCoroutine(SlierLerp(1 - (float)data.RemainTime / (float)data.TimeCost, currentTimeProgress));
                }
              
            }
        }

        IEnumerator SlierLerp(float maxNum,float minNum)
        {
            float delta = (maxNum - minNum) / lerpSpeed;
            float result = minNum;

            for(int i = 0; i < lerpSpeed; i++)
            {
                result += delta;
                _progressImage.fillAmount = result;
                yield return new WaitForSeconds(0.05f);
            }
            _progressImage.fillAmount = maxNum;
            currentTimeProgress = maxNum;
            StopCoroutine(SlierLerp(maxNum, minNum));
        }


        void OnPointClick()
        {
            UIManager.Instance.SendMessageToWnd(UIPath.WindowPath.Explore_Point_Page, new UIMessage(UIMsgType.ExplorePage_Show_PointDetail, new List<object>(1) { pointData }));
        }


        public override void OnPointerEnter(PointerEventData eventData)
        {
            if (isShowInfoTip == false)
            {
                AudioManager.Instance.PlaySound(AudioClipPath.UISound.Button_General);
                if (_showAnim != null)
                {
                    _showAnim.Play();
                    isShowInfoTip = true;
                }
            }
        }
        public override void OnPointerExit(PointerEventData eventData)
        {
            isShowInfoTip = false;
            transform.FindTransfrom("Info").SafeGetComponent<CanvasGroup>().alpha = 0;
        }

    }
}