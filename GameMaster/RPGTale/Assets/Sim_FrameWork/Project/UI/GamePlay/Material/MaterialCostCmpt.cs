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
            _icon = UIUtility.SafeGetComponent<Image>(UIUtility.FindTransfrom(transform, "BG/Icon"));
            _name = UIUtility.SafeGetComponent<Text>(UIUtility.FindTransfrom(transform, "Name"));
            _count= UIUtility.SafeGetComponent<Text>(UIUtility.FindTransfrom(transform, "Value"));

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