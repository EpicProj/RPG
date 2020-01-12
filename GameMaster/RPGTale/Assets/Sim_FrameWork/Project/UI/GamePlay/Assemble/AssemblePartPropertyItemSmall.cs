using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sim_FrameWork
{
    public class AssemblePartPropertyItemSmall : MonoBehaviour
    {

        private Text _name;
        private Image _icon;
        private Text _value;

        private void Awake()
        {
            _icon = UIUtility.SafeGetComponent<Image>(UIUtility.FindTransfrom(transform, "Icon"));
            _name = UIUtility.SafeGetComponent<Text>(UIUtility.FindTransfrom(transform, "Name"));
            _value = UIUtility.SafeGetComponent<Text>(UIUtility.FindTransfrom(transform, "Value"));
        }

        public void SetUpItem(Sprite icon,string name,string value)
        {
            _icon.sprite = icon;
            _name.text = name;
            _value.text = value;
        }
    }
}