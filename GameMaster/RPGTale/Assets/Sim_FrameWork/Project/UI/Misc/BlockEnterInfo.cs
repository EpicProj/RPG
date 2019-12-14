using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sim_FrameWork
{
    public class BlockEnterInfo : MonoBehaviour
    {

        private Animation _anim;
        private Text _name;
        private Image _icon;
        private FunctionBlockDataModel _model;

        void Awake()
        {
            _anim = UIUtility.SafeGetComponent<Animation>(transform);
            _name = UIUtility.SafeGetComponent<Text>(UIUtility.FindTransfrom(transform, "BG/Name"));
            _icon = UIUtility.SafeGetComponent<Image>(UIUtility.FindTransfrom(transform, "BG/Icon"));
        }

        public void HideInfo()
        {
            gameObject.SetActive(false);
        }

        public bool SetUpEnterInfo(FunctionBlockDataModel model,Vector3 pos)
        {
            _model = model;
            if (model.ID != 0)
            {
                transform.localPosition = pos;
                gameObject.SetActive(true);
                _anim.Play();
                _name.text = model.Name;
                _icon.sprite = model.Icon;
                
                return true;
            }
            return false;
        }
    }
}