using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sim_FrameWork
{
    public class AssembleShipCustomPartItem : MonoBehaviour
    {
        public AssembleShipPartConfig.ConfigData _configData;

        private Transform Line;
        private Text _name;
        private Image _icon;

        private Transform IconAdd;
        private Button _btn;

        private void Awake()
        {
            Line = UIUtility.FindTransfrom(transform, "Line");
            IconAdd = UIUtility.FindTransfrom(transform, "Line/Content/IconAdd");
            _btn = UIUtility.SafeGetComponent<Button>(UIUtility.FindTransfrom(transform, "Line/Content"));
            _icon = UIUtility.SafeGetComponent<Image>(UIUtility.FindTransfrom(transform, "Line/Content/Icon"));
            _name= UIUtility.SafeGetComponent<Text>(UIUtility.FindTransfrom(transform, "Line/Content/Name"));
        }

        public void SetUpItem(AssembleShipPartConfig.ConfigData configData)
        {
            _btn.onClick.RemoveAllListeners();
            if (configData == null)
                return;
            _configData = configData;
            _name.text = "";
            _icon.gameObject.SetActive(false);
            IconAdd.gameObject.SetActive(true);


            transform.localPosition = new Vector3((float)configData.PosX, (float)configData.PosY, 0);
            Line.GetComponent<RectTransform>().sizeDelta = new Vector2(2, (float)configData.LineWidth);

            _btn.onClick.AddListener(OnBtnClick);
        }

        public void AddShipPart()
        {
            _icon.gameObject.SetActive(true);
            IconAdd.gameObject.SetActive(false);
        }

        void OnBtnClick()
        {
            AudioManager.Instance.PlaySound(AudioClipPath.UISound.Button_Click);
            UIGuide.Instance.ShowAssemblePartChooseDialog(_configData.EquipPartType,_configData.EquipPartType[0]);
        }


    }
}