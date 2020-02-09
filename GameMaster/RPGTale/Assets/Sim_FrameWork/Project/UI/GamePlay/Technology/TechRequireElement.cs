using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Sim_FrameWork
{
    public class TechRequireElement : BaseElementSimple
    {
        public enum RequireType
        {
            Material,
            PreTech,
            ResearchPoint,
        }


        private Transform titleWaringTrans;
        private Image rarityBG;
        private Image icon;
        private Text nameText;
        private Transform lockIconTrans;

        private Transform SelectEffectTrans;
        private Animation anim;

        public RequireType type;
        private BaseDataModel _model;

        private const string Research_Require_TechPoint_Text = "Research_Require_TechPoint_Text";

        public override void Awake()
        {
            titleWaringTrans = UIUtility.FindTransfrom(transform, "Title");
            rarityBG = UIUtility.SafeGetComponent<Image>(UIUtility.FindTransfrom(transform, "BG/Circle"));
            icon= UIUtility.SafeGetComponent<Image>(UIUtility.FindTransfrom(transform, "BG/Icon")); ;
            nameText = UIUtility.SafeGetComponent<Text>(UIUtility.FindTransfrom(transform, "Name"));
            lockIconTrans = UIUtility.FindTransfrom(transform, "Name/Icon");
            SelectEffectTrans = UIUtility.FindTransfrom(transform, "Select");
            anim = UIUtility.SafeGetComponent<Animation>(transform);
        }

        void Start()
        {
           
        }

        public void SetUpElement(RequireType type,object[] param,bool showWarning)
        {
            this.type = type;
            if(type== RequireType.Material)
            {
                int materialID = (int)param[0];
                int count = (int)param[1];
                MaterialDataModel maModel = new MaterialDataModel();
                if (maModel.Create(materialID))
                {
                    _model = maModel;
                    SetElementInfo(maModel.Icon, maModel.Name + " X" + count, maModel.Rarity.color);
                    ShowLockWaring(showWarning);
                }
            }
            else if(type== RequireType.PreTech)
            {
                int techID = (int)param[0];
                TechnologyDataModel techModel = new TechnologyDataModel();
                if (techModel.Create(techID))
                {
                    _model = techModel;
                    SetElementInfo(techModel.Icon, techModel.Name, techModel.Rarity.color);
                    ShowLockWaring(showWarning);
                }
            }
            else if(type == RequireType.ResearchPoint)
            {
                ushort count = (ushort)param[0];
                var techPointImage = Utility.LoadSprite("SpriteOutput/UI/Main/Technology/TechPage_PointCost_Icon");
                string text = MultiLanguage.Instance.GetTextValue(Research_Require_TechPoint_Text) + ":" + count.ToString();
                SetElementInfo(techPointImage, text, Color.white);
                ShowLockWaring(showWarning);
            }

            if (anim != null)
                anim.Play();
        }

        void SetElementInfo(Sprite sp,string name,Color rarityColor)
        {
            icon.sprite = sp;
            nameText.text = name;
            nameText.color = new Color(rarityColor.r, rarityColor.g, rarityColor.b, 0.8f);
            rarityBG.color = new Color(rarityColor.r, rarityColor.g, rarityColor.b, 0.25f);
        }


        public void ShowLockWaring(bool active)
        {
            titleWaringTrans.gameObject.SetActive(active);
            lockIconTrans.gameObject.SetActive(active);
        }


        public override void OnPointerEnter(PointerEventData eventData)
        {
            SelectEffectTrans.gameObject.SetActive(true);
            if(type== RequireType.Material)
            {
                UIGuide.Instance.ShowMaterialDetailInfo((MaterialDataModel)_model);
            }

        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            SelectEffectTrans.gameObject.SetActive(false);
            if (type == RequireType.Material)
            {
                UIManager.Instance.HideWnd(UIPath.WindowPath.Material_Info_UI);
            }


        }


    }
}