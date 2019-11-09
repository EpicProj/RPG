using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sim_FrameWork.UI
{
    public enum DistrictSlotType
    {
        Empty,
        UnLock,
        NormalDistrict,
        LargeDistrict
    }

    public class DistrictContentSlotPanel : InventoryBase
    {
        private GridLayoutGroup gridlayoutGroup;

        public override void Awake()
        {
            base.Awake();
            InitData();
            gridlayoutGroup = GetComponent<GridLayoutGroup>();
        }

        /// <summary>
        /// 生成区划格基底
        /// </summary>
        public void InitDistrictDataSlot(FunctionBlockInfoData blockInfo)
        {
            if (blockInfo.currentDistrictBaseDic == null)
                return;
            foreach (KeyValuePair<Vector2, DistrictAreaBase> kvp in blockInfo.currentDistrictBaseDic)
            {
                var data = kvp.Value.data;
                if (data.DistrictID == -1)
                {
                    //Init EmptySlot
                    InitEmptyDisBlock(kvp.Value);
                }
                else if (data.DistrictID == -2)
                {
                    //Init UnlockSlot
                    InitUnlockDisBlock(kvp.Value);
                }
                else
                {
                    Debug.LogError("Init District Slot Error! key=" + kvp.Key);
                    continue;
                }
            }
        }

        /// <summary>
        /// 生成基础区划
        /// </summary>
        public void InitDistrictArea(FunctionBlockInfoData blockInfo)
        {
            if (blockInfo.currentDistrictDataDic == null)
                return;
            gridlayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            gridlayoutGroup.constraintCount = (int)blockInfo.districtAreaMax.x;
            foreach (KeyValuePair<Vector2, DistrictAreaInfo> kvp in blockInfo.currentDistrictDataDic)
            {
                int index = FunctionBlockModule.GetDistrictAreaIndex(blockInfo.districtAreaMax, kvp.Key);
                if (slotList.Length < index)
                {
                    Debug.LogError("Area Error!");
                    continue;
                }
                else
                {
                    if (kvp.Value.isLargeDistrict == false)
                    {
                        //Init Small District
                      
                        DistrictSlot slot = (DistrictSlot)slotList[index - 1];
                        slot.InitDistrictAreaSlot(kvp.Value); 
                    }
                    else if (kvp.Value.isLargeDistrict == true && kvp.Value.OriginCoordinate == kvp.Key)
                    {
                        //Init Large District
                       
                        List<Vector2> v2 = DistrictModule.GetRealDistrictTypeArea(kvp.Value.data, kvp.Key);
                        for (int i = 0; i < v2.Count; i++)
                        {
                            int pos = FunctionBlockModule.GetDistrictAreaIndex(blockInfo.districtAreaMax, v2[i]);
                            DistrictSlot slot = (DistrictSlot)slotList[pos];
                            slot.InitDistrictAreaSlot(kvp.Value);
                        }
                    }

                }

            }
        }




        public void InitEmptyDisBlock(DistrictAreaBase info)
        {
            //ContainEmptyInfo
            GameObject EmptySlot = ObjectManager.Instance.InstantiateObject(UIPath.PrefabPath.DISTRICTSLOT_PREFAB_PATH);
            EmptySlot.GetComponent<Image>().sprite = info.sprite;
            EmptySlot.transform.SetParent(transform, false);
            EmptySlot.transform.localScale = Vector3.one;
            EmptySlot.transform.localPosition = Vector3.zero;
            EmptySlot.transform.Find("EmptyInfo").gameObject.SetActive(true);
            EmptySlot.GetComponent<DistrictSlot>().InitBaseInfo(info);
        }


        public void InitUnlockDisBlock(DistrictAreaBase info)
        {
            GameObject UnlockSlot = ObjectManager.Instance.InstantiateObject(UIPath.PrefabPath.DISTRICTSLOT_PREFAB_PATH);
            UnlockSlot.transform.SetParent(transform, false);
            UnlockSlot.GetComponent<DistrictSlot>().InitBaseInfo(info);
        }



    }
}