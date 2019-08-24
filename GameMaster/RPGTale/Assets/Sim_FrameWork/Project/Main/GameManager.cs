using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Sim_FrameWork
{
    public class GameManager : MonoSingleton<GameManager>
    {
        public const string ITEM_UI_PATH = "ItemUIPrefab.prefab";

        //Inventory
        private bool isPickedItem = false;
        public bool IsPickedItem { get { return isPickedItem; } }
        private SlotItem pickedItem;
        public SlotItem PickedItem { get { return pickedItem; } }


        private Canvas MainCanvas;

        protected override void Awake()
        {
            base.Awake();
            AssetBundleManager.Instance.LoadAssetBundleConfig();
            ResourceManager.Instance.Init(this);
            ObjectManager.Instance.Init(transform.Find("RecyclePoolTrs"), transform.Find("SceneTrs"));
            UIManager.Instance.Init(GameObject.Find("MainCanvas").transform as RectTransform, GameObject.Find("MainCanvas/Window").transform as RectTransform, GameObject.Find("MainCanvas/UICamera").GetComponent<Camera>(), GameObject.Find("MainCanvas/EventSystem").GetComponent<EventSystem>());
            MainCanvas = GameObject.Find("MainCanvas").GetComponent<Canvas>();
        }


        public void Update()
        {
            UIManager.Instance.OnUpdate();
            OnUpdateInventory();
        }


        public void RegisterUI()
        {


        }

        #region Inventory

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
        public void PickUpFunctionBlock(FunctionBlock block,int amount = 1)
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
            RectTransformUtility.ScreenPointToLocalPointInRectangle(MainCanvas.transform as RectTransform, Input.mousePosition, null, out position);
            return position;
        }

        #endregion

    }
}