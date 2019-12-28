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

        private Transform _infoTrans;

        private Image _progressImage;
        private Text _progressTimeText;

        private Animation _showAnim;
        private Text _pointName;
        private Text _energyCost;
        private Text _timeCost;
        private Button _btn;

        public ExplorePointData pointData;

        //Slider Lerp
        private int lerpSpeed = 10;
        private float currentTimeProgress = 0f;


        public override void Awake()
        {
            _infoTrans = UIUtility.FindTransfrom(transform, "Info");
            _showAnim = UIUtility.SafeGetComponent<Animation>(_infoTrans);
            _pointName = UIUtility.SafeGetComponent<Text>(UIUtility.FindTransfrom(_infoTrans, "Name"));
            _energyCost = UIUtility.SafeGetComponent<Text>(UIUtility.FindTransfrom(_infoTrans, "Energy/Value"));
            _timeCost = UIUtility.SafeGetComponent<Text>(UIUtility.FindTransfrom(_infoTrans, "Time/Value"));
            _btn = UIUtility.SafeGetComponent<Button>(transform);

            _progressImage = UIUtility.SafeGetComponent<Image>(UIUtility.FindTransfrom(transform, "Ring/Fill"));
            _progressImage.fillAmount = currentTimeProgress;
            _progressTimeText = UIUtility.SafeGetComponent<Text>(UIUtility.FindTransfrom(transform, "Ring/Progress"));
        }

        public void InitPoint(ExplorePointData data)
        {
            if (data != null)
            {
                pointData = data;
                _pointName.text = data.pointName;
                _energyCost.text = data.EnergyCost.ToString();
                _timeCost.text = data.TimeCost.ToString();
                _progressTimeText.text = "";
                var pos = SolarSystemManager.Instance.GetPointPositionUI(data);
                transform.GetComponent<RectTransform>().anchoredPosition = pos;
                _btn.onClick.AddListener(OnPointClick);
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
            UIUtility.SafeGetComponent<CanvasGroup>(_infoTrans).alpha = 0;
        }

    }
}