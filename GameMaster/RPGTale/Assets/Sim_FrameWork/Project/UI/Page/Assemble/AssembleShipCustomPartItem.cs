using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Sim_FrameWork
{
    public class AssembleShipCustomPartItem : BaseElementSimple
    {
        public Config.AssembleShipPartConfig.ConfigData _configData;

        private Transform Line;
        private Text _name;
        private Image _icon;

        private Transform IconAdd;
        private Transform _enterEffect;
        private Button _btn;

        public AssemblePartInfo partInfo;

        private bool isEquiped = false;

        public override void Awake()
        {
            Line = transform.FindTransfrom("Line");
            IconAdd = transform.FindTransfrom("Line/Content/IconAdd");
            _btn = transform.FindTransfrom("Line/Content").SafeGetComponent<Button>();
            _icon = transform.FindTransfrom("Line/Content/Icon").SafeGetComponent<Image>();
            _name= transform.FindTransfrom("Line/Content/Name").SafeGetComponent<Text>();
            _enterEffect = transform.FindTransfrom("Line/Content/EnterEffect");
            _enterEffect.gameObject.SetActive(false);
        }

        public void SetUpItem(Config.AssembleShipPartConfig.ConfigData configData)
        {
            _btn.onClick.RemoveAllListeners();
            if (configData == null)
                return;
            _configData = configData;
            _name.text = "";
            _icon.gameObject.SetActive(false);
            IconAdd.gameObject.SetActive(true);

            transform.localPosition = new Vector3((float)configData.PosX, (float)configData.PosY, 0);
            Line.SafeGetComponent<RectTransform>().sizeDelta = new Vector2(2, (float)configData.LineWidth);

            _btn.onClick.AddListener(OnBtnClick);
            isEquiped = false;
        }

        public void AddShipPart(AssemblePartInfo info)
        {
            partInfo = info;
            _icon.gameObject.SetActive(true);
            IconAdd.gameObject.SetActive(false);

            _icon.sprite = info.typePresetData.partIconSmall;
            _name.text = info.customName;

            isEquiped = true;
        }

        void OnBtnClick()
        {
            AudioManager.Instance.PlaySound(AudioClipPath.UISound.Button_Click);
            UIGuide.Instance.ShowAssemblePartChooseDialog(_configData.EquipPartType, _configData.EquipPartType[0], 2, _configData.configID);
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            _enterEffect.gameObject.SetActive(true);

            if (isEquiped && partInfo!=null)
            {
                UIGuide.Instance.ShowAssemblePartInfoUI(partInfo);
            }
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            _enterEffect.gameObject.SetActive(false);

            if (isEquiped && partInfo != null)
            {
                UIManager.Instance.HideWnd(UIPath.WindowPath.Assemble_PartInfo_UI);
            }
        }


    }
}