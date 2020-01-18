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

        public string _propertyName;

        private void Awake()
        {
            _icon = UIUtility.SafeGetComponent<Image>(UIUtility.FindTransfrom(transform, "Icon"));
            _name = UIUtility.SafeGetComponent<Text>(UIUtility.FindTransfrom(transform, "Name"));
            _value = UIUtility.SafeGetComponent<Text>(UIUtility.FindTransfrom(transform, "Value"));
        }

        public void SetUpItem(Sprite icon,string name,float value,string propertyName)
        {
            _icon.sprite = icon;
            _name.text = name;
            _propertyName = propertyName;
            if(string.Compare(_propertyName, "Time") == 0)
            {
                _value.text = string.Format("{0:N1}", value);
                return;
            }

            var type = AssembleModule.GetAssemblePartPropertyTypeData(_propertyName);
            _value.text = ValueFormat(type, value);
        }

        public void RefreshValue(float value)
        {
            if (string.Compare(_propertyName, "Time") ==0)
            {
                _value.text = string.Format("{0:N1}", value);
                return;
            }
            var type = AssembleModule.GetAssemblePartPropertyTypeData(_propertyName);
            _value.text = ValueFormat(type,value);
        }



        string ValueFormat(AssemblePartPropertyTypeData type, float value)
        {
            if (type.Type == 1)
            {
                ///Two decimal places
                return string.Format("{0:N2}", value);
            }
            else if (type.Type == 2)
            {
                ///One decimal places
                return string.Format("{0:N1}", value);
            }
            else if (type.Type == 3)
            {
                return ((int)value).ToString();
            }
            return string.Empty;
        }
    }
}