using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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


        void Start()
        {
            mainCanvas = GameObject.Find("MainCanvas").GetComponent<Canvas>();
        }

        void Update()
        {
            OnUpdateInventory();
        }

        //Update
        void OnUpdateInventory()
        {
            if (isPickedItem)
            {
                //Fllow  MousePos
                pickedItem.SetLocalPosition(GetCurrentMousePos());
            }
        }

        //Pick Item
        public void PickUpFunctionBlock(FunctionBlock block, int amount = 1)
        {
            pickedItem.SetFunctionBlock(block, amount);
            isPickedItem = true;
            pickedItem.Show();
            pickedItem.SetLocalPosition(GetCurrentMousePos());
        }

        //获取当前鼠标位置
        Vector2 GetCurrentMousePos()
        {
            Vector2 position;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(mainCanvas.transform as RectTransform, Input.mousePosition, null, out position);
            return position;
        }

    }
}