using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork {
    public class WareHouseDialogContent : WindowBase {

        public WareHouseDialog m_dialog;
        public List<MaterialStorageData> storageDataList;

        public override void Awake(params object[] paralist)
        {
            storageDataList = (List<MaterialStorageData>)paralist[0];
            m_dialog = GameObject.GetComponent<WareHouseDialog>();
            AddBtnListener();
            InitSotrageItem();
        }


        public override void OnShow(params object[] paralist)
        {
            storageDataList = (List<MaterialStorageData>)paralist[0];
            //TODO
            InitSotrageItem();
        }

        public override bool OnMessage(string msgID, params object[] paralist)
        {
            switch (msgID)
            {
                case "UpdateWarehouseData":
                    storageDataList = (List<MaterialStorageData>)paralist[0];
                    InitSotrageItem();
                    return true;
            }
         
            return false;
        }




        private void AddBtnListener()
        {
            AddButtonClickListener(m_dialog.CloseBtn, delegate ()
            {
                UIManager.Instance.HideWnd(UIPath.WAREHOURSE_DIALOG);
            });
        }


        private void InitSotrageItem()
        {
            for(int i = 0; i < storageDataList.Count; i++)
            {
                GameObject obj = ObjectManager.Instance.InstantiateObject(UIPath.MATERIAL_WAREHOUSE_PREFAB_PATH);
                MaterialSlot slot = obj.GetComponent<MaterialSlot>();
                slot.SetUpMaterialItem(storageDataList[i]);
                obj.transform.SetParent(m_dialog.MaterialContent.transform,false);
            }
        }

        private void UpdateStorageItem()
        {
        }

    }
}