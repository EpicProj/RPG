using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sim_FrameWork
{
    public class AssemblePartPropertyItem : MonoBehaviour
    {
        private Image Icon;
        private Text Name;
        private Text ValueMin;
        private Text ValueMax;

        private string _propertyName;
        public string PropertyName
        {
            get { return _propertyName; }
        }

        private float _min;
        public float Min
        {
            get { return _min; }
        }

        private float _max;
        public float Max
        {
            get { return _max; }
        }


        private void Awake()
        {
            Icon = UIUtility.SafeGetComponent<Image>(UIUtility.FindTransfrom(transform, "Icon"));
            Name= UIUtility.SafeGetComponent<Text>(UIUtility.FindTransfrom(transform, "Name"));
            ValueMin= UIUtility.SafeGetComponent<Text>(UIUtility.FindTransfrom(transform, "ValueMin"));
            ValueMax = UIUtility.SafeGetComponent<Text>(UIUtility.FindTransfrom(transform, "ValueMax"));
        }

        public void SetUpItem(string propertyName, Sprite icon,string name,float valueMin,float valueMax)
        {
            _propertyName = propertyName;
            Icon.sprite = icon;
            Name.text = name;
            ValueMin.text = valueMin.ToString();
            ValueMax.text = valueMax.ToString();
            _min = valueMin;
            _max = valueMax;
        }

        public void ChangeValueMin(float value)
        {
            _min = value;
            ValueMin.text = value.ToString();
        }

        public void ChangeValueMax(float value)
        {
            _max = value;
            ValueMax.text = value.ToString();
        }

    }
}