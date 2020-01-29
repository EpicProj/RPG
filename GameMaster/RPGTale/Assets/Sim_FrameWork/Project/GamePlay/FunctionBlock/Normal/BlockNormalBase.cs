using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Sim_FrameWork
{
    public class BlockNormalBase : MonoBehaviour
    {
        public FunctionBlockBase _blockBase;
        public void SetData()
        {
            _blockBase = UIUtility.SafeGetComponent<FunctionBlockBase>(transform);

            _blockBase.OnBlockSelectAction += Onselect;
            _blockBase.OnBlockAreaEnterAction += OnBlockAreaEnter;
        }

        private void Onselect()
        {
            UIGuide.Instance.ShowBlockNormalInfoDialog(this);
            InventoryManager.Instance.HideBlockEnterInfo();
        }

        private void OnBlockAreaEnter(bool enter)
        {
            if (enter)
            {
                InventoryManager.Instance.ShowBlockEnterInfo(_blockBase.info.dataModel, _blockBase.CenterPositionScreen);
            }
            else
            {
                InventoryManager.Instance.HideBlockEnterInfo();
            }

        }
    }

    public class BlockNormalInfoData
    {

    }
}