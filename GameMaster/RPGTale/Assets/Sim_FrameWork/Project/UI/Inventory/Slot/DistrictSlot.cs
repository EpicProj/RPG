using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Sim_FrameWork.UI
{
    public class DistrictSlot : BaseElementDrag {

        public DistrictAreaInfo _info;
        private Transform lockTrans;
        private Transform buildTrans;

        void Awake()
        {
            lockTrans = UIUtility.FindTransfrom(transform, "LockInfo");
            buildTrans = UIUtility.FindTransfrom(transform, "Build");
        }


        public void InitBaseInfo(DistrictAreaInfo info)
        {
            _info = info;
            if (info.Locked)
            {
                lockTrans.gameObject.SetActive(true);
                buildTrans.gameObject.SetActive(false);
            }
            else
            {
                lockTrans.gameObject.SetActive(false);
                buildTrans.gameObject.SetActive(true);
            }


        }
        public void InitDistrictAreaSlot(DistrictAreaInfo info)
        {
            if (transform.childCount == 1)
            {
                //Contain Empty Info
                GameObject itemObj = ObjectManager.Instance.InstantiateObject(UIPath.PrefabPath.DISTRICT_PREFAB_PATH);
                itemObj.transform.SetParent(transform, false);
                itemObj.transform.localScale = Vector3.one;
                itemObj.transform.localPosition = Vector3.zero;
                itemObj.GetComponent<SlotItem>().SetDistrictArea(info);
            }
            else if(transform.childCount==2)
            {
                //Exchange
                transform.Find("District(Clone)").GetComponent<SlotItem>().SetDistrictArea(info);
            }
        }

        public override void OnBeginDrag(PointerEventData eventData)
        {
            if (InventoryManager.Instance.IsPickedItem == false)
            {
                //未选中区划
                if (transform.childCount > 1)
                {
                    // 1 Has EmptyInfo
                    SlotItem currentItem = transform.GetChild(1).GetComponent<SlotItem>();
                    InventoryManager.Instance.PickUpDistrictArea(currentItem.districtInfo);
                    Destroy(currentItem.gameObject);
                }
            }
        }

        public override void OnDrag(PointerEventData eventData)
        {
            if (InventoryManager.Instance.IsPickedItem == true)
            {
                //跟随鼠标
                InventoryManager.Instance.UpdatePickedItemPos();
            }
        }
        public override void OnEndDrag(PointerEventData eventData)
        {
            GameObject target = InventoryManager.Instance.GetDistrictSlotItemByRay();
           
            SlotItem pickedItem = InventoryManager.Instance.PickedItem;
            if (target == null)
            {
                //如果拖到了无效位置，回复
                InitDistrictAreaSlot(pickedItem.districtInfo);
                InventoryManager.Instance.RemoveItem();
                return;
            }
               
            if (InventoryManager.Instance.IsPickedItem == true)
            {
                DistrictSlot targetSlot = target.GetComponent<DistrictSlot>();
                if (target.transform.childCount > 1)
                {
                    SlotItem targetItem = target.transform.GetChild(1).GetComponent<SlotItem>(); //目标区划
                    //选中区划
                    if (CheckNotInLockPosition(targetSlot._info))
                    {
                        //目标格有区划
                        InventoryManager.Instance.RemoveItem();
                        //Exchange
                        this.InitDistrictAreaSlot(targetItem.districtInfo);
                        target.transform.GetComponent<DistrictSlot>().InitDistrictAreaSlot(pickedItem.districtInfo);
                      
                    }
                }
                else
                {
                    //目标格无区划
                    if (CheckNotInLockPosition(targetSlot._info))
                    {
                        target.transform.GetComponent<DistrictSlot>().InitDistrictAreaSlot(InventoryManager.Instance.PickedItem.districtInfo);
                        InventoryManager.Instance.RemoveItem();
                    }
                    else
                    {
                        //无效区域，回复
                        InitDistrictAreaSlot(pickedItem.districtInfo);
                        InventoryManager.Instance.RemoveItem();
                    }
                }
            }
        }

        public bool CheckNotInLockPosition(DistrictAreaInfo info)
        {
            if(info.slotType== DistrictSlotType.UnLock)
            {
                return false;
            }
            return true;
        }

 

    }
}