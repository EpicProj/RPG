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

    public class DistrictContentSlotPanel : MonoBehaviour
    {


        /// <summary>
        /// 生成基础区划
        /// </summary>
        public void InitDistrictArea(FunctionBlockInfoData blockInfo)
        {
            if (blockInfo.currentDistrictDataDic == null)
                return;
            //gridlayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            //gridlayoutGroup.constraintCount = (int)blockInfo.districtAreaMax.x;
            foreach (KeyValuePair<Vector2, DistrictAreaInfo> kvp in blockInfo.currentDistrictDataDic)
            {
                int index = FunctionBlockModule.GetDistrictAreaIndex(blockInfo.districtAreaMax, kvp.Key);
                //if (_elementList.Length < index)
                //{
                //    Debug.LogError("Area Error!");
                //    continue;
                //}
                //else
                //{
                //    if (kvp.Value.isLargeDistrict == false)
                //    {
                //        //Init Small District
                      
                //        DistrictSlot slot = (DistrictSlot)_elementList[index - 1];
                //        slot.InitDistrictAreaSlot(kvp.Value); 
                //    }
                //    else if (kvp.Value.isLargeDistrict == true && kvp.Value.OriginCoordinate == kvp.Key)
                //    {
                //        //Init Large District
                       
                //        List<Vector2> v2 = DistrictModule.GetRealDistrictTypeArea(kvp.Value.data, kvp.Key);
                //        for (int i = 0; i < v2.Count; i++)
                //        {
                //            int pos = FunctionBlockModule.GetDistrictAreaIndex(blockInfo.districtAreaMax, v2[i]);
                //            DistrictSlot slot = (DistrictSlot)_elementList[pos];
                //            slot.InitDistrictAreaSlot(kvp.Value);
                //        }
                //    }

                //}

            }
        }

    }
}