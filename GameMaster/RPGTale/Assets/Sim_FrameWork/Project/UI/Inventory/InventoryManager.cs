using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Sim_FrameWork.UI;

namespace Sim_FrameWork
{
    public class InventoryManager : MonoSingleton<InventoryManager>
    {

        //Inventory
        private bool isPickedItem = false;
        public bool IsPickedItem { get { return isPickedItem; } }

        private SlotItem pickedItem;
        public SlotItem PickedItem { get { return pickedItem; } }

        private Canvas mainCanvas;
        private Camera uiCamera;

        private BlockEnterInfo _enterInfo;
        private bool isShowBlockEnterInfo;
        public bool IsShowBlockEnterInfo { get { return isShowBlockEnterInfo; } }
        

        void Start()
        {
            InitRefrence();
            pickedItem.Hide();

        }


        void InitRefrence()
        {
            mainCanvas = UIUtility.SafeGetComponent<Canvas>(Utility.SafeFindGameobject("MainCanvas").transform);
            uiCamera = UIUtility.SafeGetComponent<Camera>(Utility.SafeFindGameobject("MainCanvas/UICamera").transform);
            pickedItem = mainCanvas.transform.Find("SPContent/PickedDistrict").GetComponent<SlotItem>();
            _enterInfo = UIUtility.SafeGetComponent<BlockEnterInfo>(UIUtility.FindTransfrom(mainCanvas.transform, "SPContent/BlockEnterInfo"));
        }


        protected override void Awake()
        {
            base.Awake();
        }



        //Pick Item

        public void PickUpDistrictArea(DistrictAreaInfo info)
        {
            pickedItem.SetDistrictArea(info);
            isPickedItem = true;
            pickedItem.Show(true);

            //Hide Tip TODO
            //Floow Mouse
            pickedItem.SetLocalPosition(GetCurrentMousePos());

        }


        /// <summary>
        /// 获取当前鼠标位置
        /// </summary>
        /// <returns></returns>
        public Vector2 GetCurrentMousePos()
        {
            Vector2 position;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(mainCanvas.transform as RectTransform, Input.mousePosition, uiCamera, out position);
            return position;
        }

        public Vector2 ScreenPointToLocalPoint(Vector2 point)
        {
            Vector2 localPoint = new Vector2();
            RectTransformUtility.ScreenPointToLocalPointInRectangle(mainCanvas.GetComponent<RectTransform>(), point, uiCamera, out localPoint);
            return localPoint;
        }


        //格子跟随
        public void UpdatePickedItemPos()
        {
            pickedItem.SetLocalPosition(GetCurrentMousePos());
        }
        //获取当前位置SLot
        public GameObject GetDistrictSlotItemByRay()
        {
            PointerEventData data = new PointerEventData(EventSystem.current);
            data.position = Input.mousePosition;
            GraphicRaycaster ray = mainCanvas.GetComponent<GraphicRaycaster>();
            List<RaycastResult> results = new List<RaycastResult>();
            ray.Raycast(data, results);
            for (int i = 0; i < results.Count; i++)
            {
                if (results[i].gameObject.name == "BlockGrid(Clone)")
                {
                    return results[i].gameObject;
                }
            }
            return null;
        }
 
        //Romove Item
        public void RemoveItem(int amount=1)
        {
            pickedItem.ReduceAmount(amount);
            if (pickedItem.Amount <= 0)
            {
                isPickedItem = false;
                pickedItem.Hide();
            }
        }

       

        ///Block Enter Info
       
        public void ShowBlockEnterInfo(FunctionBlockDataModel model,Vector3 pos)
        {
            if (_enterInfo != null)
            {
                if (_enterInfo.SetUpEnterInfo(model,pos))
                    isShowBlockEnterInfo = true;
            }
        }
        public void HideBlockEnterInfo()
        {
            if (_enterInfo != null)
            {
                _enterInfo.HideInfo();
                isShowBlockEnterInfo = false;
            }
        }
    }
}