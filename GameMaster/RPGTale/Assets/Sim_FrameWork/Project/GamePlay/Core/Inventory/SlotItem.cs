using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sim_FrameWork
{
    public class SlotItem : MonoBehaviour
    {
        private Image itemImage;
        private FunctionBlock functionBlock;
        private DistrictData districtData;


        private Image ItemImage {
            get
            {
                if (itemImage == null)
                {
                    itemImage = GetComponent<Image>();
                }
                return itemImage;
            }
        }

        public int Amount { get; private set; }
        private float targetScale = 1.0f;

        public void OnUpdate()
        {

        }

        public void SetFunctionBlock(FunctionBlock block, int amount = 1)
        {
            this.functionBlock = block;
            this.Amount = amount;
            ItemImage.sprite = FunctionBlockModule.Instance.GetFunctionBlockIcon(block.FunctionBlockID);

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

        #region District

        public void SetDistrictArea(DistrictData data,DistrictSlotType slotType,Sprite sp)
        {
            this.districtData = data;

            ItemImage.sprite = sp;
            transform.Find("Name").GetComponent<Text>().text = DistrictModule.Instance.GetDistrictName(data);

            switch (slotType)
            {
                case DistrictSlotType.NormalDistrict:
                    
                    break;
                case DistrictSlotType.LargeDistrict:
                    break;

                default:
                    break;
            }
        }

        #endregion

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