using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sim_FrameWork
{
    public class SlotItem : MonoBehaviour
    {
        public Image itemImage;
        private FunctionBlock functionBlock;

        public int Amount { get; private set; }
        private float targetScale = 1.0f;

        public void OnUpdate()
        {

        }

        public void SetFunctionBlock(FunctionBlock block, int amount = 1)
        {
            this.functionBlock = block;
            this.Amount = amount;
            itemImage.sprite = FunctionBlockModule.Instance.GetFunctionBlockIcon(block.FunctionBlockID);

        }


        public void AddAmount(int amount = 1)
        {
            this.Amount += amount;
        }

        public void ReduceAmount(int amount = 1)
        {
            this.Amount -= amount;

        }
        public void SetAmount(int amount)
        {
            this.Amount = amount;
        }



        //Action
        public void Show()
        {
            gameObject.SetActive(true);
        }
        public void Hide()
        {
            gameObject.SetActive(false);
        }
        public void SetLocalPosition(Vector3 pos)
        {
            transform.localPosition = pos;
        }

    }
}