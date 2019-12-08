using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sim_FrameWork
{
    public class BuildDetailDistrictElement : MonoBehaviour
    {
        private Transform lockTrans;
        private Transform empty;
        private Image icon;


        private void Awake()
        {
            lockTrans = UIUtility.FindTransfrom(transform, "Lock");
            empty = UIUtility.FindTransfrom(transform, "Empty");
            icon = UIUtility.SafeGetComponent<Image>(UIUtility.FindTransfrom(transform, "Empty/Icon"));
        }

        public void InitLockState()
        {
            empty.gameObject.SetActive(false);
            lockTrans.gameObject.SetActive(true);
        }

        public void InitEmpetyState(Sprite sprite=null)
        {
            empty.gameObject.SetActive(true);
            icon.sprite = sprite;
            lockTrans.gameObject.SetActive(false);
        }
    }
}