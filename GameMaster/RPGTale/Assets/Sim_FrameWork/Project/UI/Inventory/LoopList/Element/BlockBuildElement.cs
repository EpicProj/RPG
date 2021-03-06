﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Sim_FrameWork.UI
{
    public class BlockBuildElement : BaseElement
    {
        public Text Name;
        public Image BlockIcon;
        private Button btn;
        private Image TypeIcon;

        private Image BG;
        private Image BG_Line;

        public BuildPanelModel _model;

        public override void ChangeAction(BaseDataModel model)
        {
            _model = (BuildPanelModel)model;
            InitBuildElement();
        }

        public override void Awake()
        {
            btn = UIUtility.SafeGetComponent<Button>(transform);
            TypeIcon = UIUtility.SafeGetComponent<Image>(UIUtility.FindTransfrom(transform,"TypeIcon"));
            BG = UIUtility.SafeGetComponent<Image>(UIUtility.FindTransfrom(transform, "BG"));
            BG_Line = UIUtility.SafeGetComponent<Image>(UIUtility.FindTransfrom(transform, "BG/Line"));
        }


        public void InitBuildElement()
        {
            FunctionBlockDataModel blockModel = new FunctionBlockDataModel();
            if (_model.BuildData != null)
            {
                if (blockModel.Create(_model.BuildData.FunctionBlockID))
                {
                    Name.text = blockModel.Name;
                    BlockIcon.sprite = blockModel.Icon;
                    TypeIcon.sprite = blockModel.TypeIcon;
                    btn.onClick.AddListener(OnBlildElementClick);
                }
                SwitchSelectState(false);
            }
        }   
        
        void OnBlildElementClick()
        {
            AudioManager.Instance.PlaySound(AudioClipPath.UISound.Button_Click);
            if (MapManager.Instance.isSelectBlock_Panel == false)
            {
                MapManager.Instance.InitBlockBuildPanelSelect(_model.BuildData.BuildID, true);
                SwitchSelectState(true);
            }
        }

        public void SwitchSelectState(bool select)
        {
            Color selectColor = new Color(1, 1, 1);
            Color unSelectColor = new Color(0, 0, 0);
            if (select)
            {
                BG.color = selectColor;
                BG_Line.color = selectColor;
            }
            else
            {
                BG.color = unSelectColor;
                BG_Line.color = unSelectColor;
            }
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            UIManager.Instance.PopUpWnd(UIPath.WindowPath.BuildPanel_Detail_UI, WindowType.SPContent, true, _model);
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            UIManager.Instance.HideWnd(UIPath.WindowPath.BuildPanel_Detail_UI);
        }
    }
}