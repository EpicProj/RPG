using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Sim_FrameWork
{
    public class ExplorePointCmpt : BaseElementSimple
    {
        private bool isShowInfoTip=false;

        private Transform _infoTrans;
        private Animation _showAnim;
        private Text _pointName;
        private Text _energyCost;
        private Button _btn;

        private ExplorePointData pointData;

        public override void Awake()
        {
            _infoTrans = UIUtility.FindTransfrom(transform, "Info");
            _showAnim = UIUtility.SafeGetComponent<Animation>(_infoTrans);
            _pointName = UIUtility.SafeGetComponent<Text>(UIUtility.FindTransfrom(_infoTrans, "Name"));
            _energyCost = UIUtility.SafeGetComponent<Text>(UIUtility.FindTransfrom(_infoTrans, "Energy/Value"));
            _btn = UIUtility.SafeGetComponent<Button>(transform);
        }

        public void InitPoint(ExplorePointData data)
        {
            if (data != null)
            {
                pointData = data;
                _pointName.text = data.pointName;
                _energyCost.text = data.EnergyCost.ToString();
                var pos = SolarSystemManager.Instance.GetPointPositionUI(data);
                transform.GetComponent<RectTransform>().anchoredPosition = pos;
                _btn.onClick.AddListener(OnPointClick);
            }
        }

        void OnPointClick()
        {
            UIGuide.Instance.ShowRandomEventDialog(pointData.eventID);
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