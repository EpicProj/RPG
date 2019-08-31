using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sim_FrameWork
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
        private const string DISTRICTSLOT_PREFAB_PATH = "Assets/Prefabs/Object/BlockGrid.prefab";

        public override void Awake()
        {
            base.Awake();
           
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
                    InitEmptyDisBlock(kvp.Value.data);
                }
                else if (data.DistrictID == -2)
                {
                    //Init UnlockSlot
                    InitUnlockDisBlock(kvp.Value.data);
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
            foreach (KeyValuePair<Vector2, DistrictAreaInfo> kvp in blockInfo.currentDistrictDataDic)
            {
                int index = FunctionBlockModule.Instance.GetDistrictAreaIndex(blockInfo.districtAreaMax, kvp.Key);
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
                        Sprite sp = DistrictModule.Instance.GetDistrictIconSpriteList(kvp.Value.data.DistrictID)[0];
                        Slot slot = slotList[index - 1];
                        slot.InitDistrictAreaSlot(kvp.Value.data, DistrictSlotType.NormalDistrict, sp);
                            
                           
                    }
                    else if (kvp.Value.isLargeDistrict == true && kvp.Value.OriginCoordinate == kvp.Key)
                    {
                        //Init Large District
                        List<Sprite> sps = DistrictModule.Instance.GetDistrictIconSpriteList(kvp.Value.data.DistrictID);
                        List<Vector2> v2 = DistrictModule.Instance.GetRealDistrictTypeArea(kvp.Value.data, kvp.Key);
                        for (int i = 0; i < v2.Count; i++)
                        {
                            int pos = FunctionBlockModule.Instance.GetDistrictAreaIndex(blockInfo.districtAreaMax, v2[i]);
                            Slot slot = slotList[pos];
                            slot.InitDistrictAreaSlot(kvp.Value.data, DistrictSlotType.LargeDistrict, sps[i]);
                        }
                    }

                }

            }
        }

        public void InitEmptyDisBlock(DistrictData data)
        {
            //ContainEmptyInfo
            GameObject EmptySlot = ObjectManager.Instance.InstantiateObject(DISTRICTSLOT_PREFAB_PATH);
            EmptySlot.GetComponent<Image>().sprite = DistrictModule.Instance.GetDistrictIconSpriteList(data.DistrictID)[0];
            EmptySlot.transform.SetParent(transform, false);
            EmptySlot.transform.localScale = Vector3.one;
            EmptySlot.transform.localPosition = Vector3.zero;
            EmptySlot.transform.Find("EmptyInfo").gameObject.SetActive(true);
        }


        public void InitUnlockDisBlock(DistrictData data)
        {
            GameObject UnlockSlot = ObjectManager.Instance.InstantiateObject(DISTRICTSLOT_PREFAB_PATH);
            UnlockSlot.transform.SetParent(transform, false);

        }

    }
}