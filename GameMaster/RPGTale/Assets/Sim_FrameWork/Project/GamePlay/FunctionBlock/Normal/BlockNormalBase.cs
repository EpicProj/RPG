using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Sim_FrameWork
{
    public class BlockNormalBase : MonoBehaviour
    {
        public FunctionBlockBase _blockBase;
        public BlockNormalInfoData _infoData;

        public void SetData()
        {
            _blockBase = UIUtility.SafeGetComponent<FunctionBlockBase>(transform);
            _infoData = new BlockNormalInfoData(_blockBase.info.BlockID);
            _blockBase.OnBlockSelectAction += Onselect;
            _blockBase.OnBlockAreaEnterAction += OnBlockAreaEnter;

            _infoData.InitModifier();
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
        public List<BlockModifier> blockModifierList;

        public BlockNormalInfoData(int blockID)
        {
            blockModifierList = FunctionBlockModule.GetBlockEffectList(blockID);
        }

        public class BlockModifier
        {
            public ModifierBase modiferBase;
            public string iconPath;
            public bool isDestory;
        }

        public void InitModifier()
        {
            if (blockModifierList != null)
            {
                for(int i = 0; i < blockModifierList.Count; i++)
                {
                    MainShipManager.Instance.AddPowerAreaModifier(blockModifierList[i].modiferBase);
                }
            }
        }
    }
}