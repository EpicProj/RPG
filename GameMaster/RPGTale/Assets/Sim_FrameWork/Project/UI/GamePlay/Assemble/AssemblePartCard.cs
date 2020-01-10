﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Sim_FrameWork
{
    public class AssemblePartCard : BaseElement
    {

        private Text _titleName;
        private Image _partImage;
        private Text _partDesc;
        private Transform _partPropertyTrans;
        private Transform _partEquipTargetTrans;

        private Button _btn;

        private AssembleTypePresetModel _model;
        public override void Awake()
        {
            _titleName = UIUtility.SafeGetComponent<Text>(UIUtility.FindTransfrom(transform, "TitleName"));
            _partDesc = UIUtility.SafeGetComponent<Text>(UIUtility.FindTransfrom(transform, "Content/Desc"));
            _partImage= UIUtility.SafeGetComponent<Image>(UIUtility.FindTransfrom(transform, "Content/IconBG/Icon"));
            _partPropertyTrans = UIUtility.FindTransfrom(transform, "Content/PartProperty/Content");
            _partEquipTargetTrans = UIUtility.FindTransfrom(transform, "Content/PartEquipTarget/Content");
            _btn = UIUtility.SafeGetComponent<Button>(UIUtility.FindTransfrom(transform,"Btn"));
            _btn.onClick.AddListener(OnBtnClick);
        }

        public override void ChangeAction(List<BaseDataModel> model)
        {
            _model = (AssembleTypePresetModel)model[0];
            SetUpElement();
        }


        void SetUpElement()
        {
            _titleName.text = _model.PresetInfo.partName;
            _partImage.sprite = _model.PresetInfo.partSprite;
            _partDesc.text = _model.PresetInfo.partDesc;

            RefreshPartProperty();
            RefreshPartEquipTarget();

        }

        void RefreshPartProperty()
        {
            foreach (Transform trans in _partPropertyTrans)
            {
                trans.gameObject.SetActive(false);
            }

            for (int i = 0; i < _model.PresetInfo.partsPropertyConfig.configData.Count; i++)
            {
                if (i > Config.GlobalConfigData.AssemblePart_Max_PropertyNum)
                    break;
                var data = _model.PresetInfo.partsPropertyConfig.configData[i];
                var trans = _partPropertyTrans.GetChild(i);
                UIUtility.SafeGetComponent<Image>(UIUtility.FindTransfrom(trans, "Icon")).sprite = Utility.LoadSprite(data.PropertyIcon, Utility.SpriteType.png);
                UIUtility.SafeGetComponent<Text>(UIUtility.FindTransfrom(trans, "Name")).text = MultiLanguage.Instance.GetTextValue(data.PropertyName);
                UIUtility.SafeGetComponent<Text>(UIUtility.FindTransfrom(trans, "Value")).text = string.Format("{0} ~ {1}", data.PropertyRangeMin.ToString(), data.PropertyRangeMax.ToString());
                trans.gameObject.SetActive(true);
            }
        }

        void RefreshPartEquipTarget()
        {
            foreach (Transform trans in _partEquipTargetTrans)
            {
                trans.gameObject.SetActive(false);
            }

            var equipType = AssembleModule.GetAssemblePartEquipType(_model.ID);
            for (int i = 0; i < equipType.Count; i++)
            {
                if (i > Config.GlobalConfigData.AssemblePart_Target_MaxNum)
                    break;
                var equipData = AssembleModule.GetAssembleMainTypeData(equipType[i]);
                if (equipData != null)
                {
                    var trans = _partEquipTargetTrans.GetChild(i);
                    UIUtility.SafeGetComponent<Image>(UIUtility.FindTransfrom(trans, "Icon")).sprite = Utility.LoadSprite(equipData.IconPath, Utility.SpriteType.png);
                    UIUtility.SafeGetComponent<Text>(UIUtility.FindTransfrom(trans, "Name")).text = MultiLanguage.Instance.GetTextValue(equipData.TypeNameText);
                    trans.gameObject.SetActive(true);
                }
            }
        }

        void OnBtnClick()
        {
            AudioManager.Instance.PlaySound(AudioClipPath.UISound.Button_Click);
            UIManager.Instance.SendMessageToWnd(UIPath.WindowPath.Assemble_Part_Design_Page, new UIMessage(UIMsgType.Assemble_PartPreset_Select, new List<object>() { _model.ID }));
        }


    }
}