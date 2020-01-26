using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Sim_FrameWork
{
    public class MaterialCostCmpt : BaseElementSimple
    {
        public MaterialCostItem _item;

        private Image _icon;
        private Text _name;
        private Text _count;


        public override void Awake()
        {
            _icon = transform.FindTransfrom("BG/Icon").SafeGetComponent<Image>();
            _name = transform.FindTransfrom("Name").SafeGetComponent<Text>();
            _count= transform.FindTransfrom("Value").SafeGetComponent<Text>();

        }

        public void SetUpItem(MaterialCostItem item)
        {
            _item = item;
            _icon.sprite = item.model.Icon;
            _name.text = item.model.Name;
            _count.text = item.count.ToString();
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            UIGuide.Instance.ShowMaterialDetailInfo(_item.model);
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            UIManager.Instance.HideWnd(UIPath.WindowPath.Material_Info_UI);
        }
    }
}