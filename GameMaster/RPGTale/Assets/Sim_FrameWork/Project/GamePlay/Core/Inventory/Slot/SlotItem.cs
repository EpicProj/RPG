using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sim_FrameWork
{
    public class SlotItem : MonoBehaviour
    {
        private Image itemImage;
        public FunctionBlock functionBlock { get; private set; }
        public DistrictAreaInfo districtInfo { get; private set; }
        public Material material { get; private set; }



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

        public void SetMaterialData(Material ma,int amount=1)
        {
            this.material = ma;
            this.Amount = amount;
            if (amount == 0)
            {
                //destory
                Destroy(this.gameObject);
            }
            ItemImage.sprite = MaterialModule.Instance.GetMaterialSprite(ma.MaterialID);
            transform.Find("AmountBG/Amount").GetComponent<Text>().text = amount.ToString();

        }


        #region District

        public void SetDistrictArea(DistrictAreaInfo info, int amount=1)
        {
            this.districtInfo =info;
            this.Amount = amount;

            ItemImage.sprite = info.sprite;
            transform.Find("Name").GetComponent<Text>().text = DistrictModule.Instance.GetDistrictName(info.data);

            switch (info.slotType)
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
        public void Show(bool bTop=false)
        {
            gameObject.SetActive(true);
            if (bTop)
            {
                gameObject.transform.SetAsLastSibling();
            }
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