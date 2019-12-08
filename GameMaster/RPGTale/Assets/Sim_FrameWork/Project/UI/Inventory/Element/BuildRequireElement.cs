using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sim_FrameWork
{
    public class BuildRequireElement : MonoBehaviour
    {
        private MaterialDataModel _model;

        private Text _name;
        private Text _count;
        private Image _icon;



        private void Awake()
        {
            _name = UIUtility.SafeGetComponent<Text>(UIUtility.FindTransfrom(transform, "Name"));
            _count = UIUtility.SafeGetComponent<Text>(UIUtility.FindTransfrom(transform, "Value"));
            _icon = UIUtility.SafeGetComponent<Image>(UIUtility.FindTransfrom(transform, "BG/Icon"));
        }

        public void InitBuildCost(MaterialDataModel model,int count)
        {
            _model = model;
            _name.text = model.Name;
            var currentCount = PlayerManager.Instance.GetMaterialStoreCount(model.ID);
            _count.text = string.Format("{0} / {1}", currentCount.ToString(), count.ToString());
            _icon.sprite = model.Icon;
        }
      
    }
}