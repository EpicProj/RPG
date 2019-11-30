using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Sim_FrameWork {
    public class TechObjectElement : BaseElementSimple {

        private Button Btn;
        private Transform selectTrans;
        private Transform lockTrans;
        private Animation selectAnim;

        private Text _timeText;
        private Text _nameText;
        private Image _icon;
        private Text _techCost;

        private Image _rarityBG;
        private Image _rarityEffect01;
        private Image _rarityEffect02;
        private Image _rarityEffect03;


        public TechnologyDataModel _dataModel;

        public override void Awake()
        {
            Btn = UIUtility.SafeGetComponent<Button>(transform);
            selectTrans = UIUtility.FindTransfrom(transform, "Content/Icon/Select");
            selectTrans.gameObject.SetActive(false);
            lockTrans = UIUtility.FindTransfrom(transform, "Lock");
            selectAnim = UIUtility.SafeGetComponent<Animation>(selectTrans);
            _nameText = UIUtility.SafeGetComponent<Text>(UIUtility.FindTransfrom(transform, "Content/Name"));
            _timeText = UIUtility.SafeGetComponent<Text>(UIUtility.FindTransfrom(transform, "Content/Time"));
            _icon = UIUtility.SafeGetComponent<Image>(UIUtility.FindTransfrom(transform, "Content/Icon/BG/Image"));
            _techCost = UIUtility.SafeGetComponent<Text>(UIUtility.FindTransfrom(transform, "Content/TechCost/Num"));

            _rarityBG = UIUtility.SafeGetComponent<Image>(UIUtility.FindTransfrom(transform, "Content/Icon/BG"));
            _rarityEffect01 = UIUtility.SafeGetComponent<Image>(UIUtility.FindTransfrom(transform, "Content/Icon/Select/SelectEffect/Image"));
            _rarityEffect02 = UIUtility.SafeGetComponent<Image>(UIUtility.FindTransfrom(transform, "Content/Icon/Select/SelectEffect2/Image"));
            _rarityEffect03 = UIUtility.SafeGetComponent<Image>(UIUtility.FindTransfrom(transform, "Content/Icon/Select/SelectEffect3/Image"));
        }


        public override void OnPointerEnter(PointerEventData eventData)
        {
            if (selectAnim != null && selectTrans!=null)
            {
                selectTrans.gameObject.SetActive(true);
                selectAnim.Play();
            }
        }
        public override void OnPointerExit(PointerEventData eventData)
        {
            if (selectAnim != null && selectTrans != null)
            {
                selectTrans.gameObject.SetActive(false);
                selectAnim.Stop();
            }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="model"></param>
        public void SetUpTech(TechnologyDataModel model)
        {
            if (model.ID == 0)
                return;
            _dataModel = model;
            _nameText.text = _dataModel.Name;
            _nameText.color = _dataModel.Rarity.color;
            _techCost.text = _dataModel.TechCost.ToString();
            _icon.sprite = _dataModel.Icon;

            InitRarity();
            AddBtnClickListener();
            
        }

        private void InitRarity()
        {
            _rarityBG.color = _dataModel.Rarity.color;
            _rarityEffect01.color = _dataModel.Rarity.color; 
            _rarityEffect02.color = _dataModel.Rarity.color; 
            _rarityEffect03.color = _dataModel.Rarity.color; 
        }


        private void AddBtnClickListener()
        {
            if (Btn != null)
            {
                Btn.onClick.AddListener(() =>
                {
                    UIManager.Instance.PopUpWnd(UIPath.WindowPath.Technology_Detail_Dialog, WindowType.Dialog, true);
                });
            }
        }

    }
}