using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sim_FrameWork.UI;

namespace Sim_FrameWork
{
    public class DistrictGroup : MonoBehaviour
    {
        public List<DistrictSlot> _districtSlotList;

        FunctionBlockDistrictInfo infoData;

        void Awake()
        {
            _districtSlotList = new List<DistrictSlot>();
        }

        public void InitSlot(FunctionBlockDistrictInfo info)
        {
            infoData = info;
            var count = info.size.x * info.size.y;
            foreach(KeyValuePair<Vector2,DistrictAreaInfo > kvp in info.currentDistrictDataDic)
            {
                var element = ObjectManager.Instance.InstantiateObject(UIPath.PrefabPath.DISTRICT_PREFAB_PATH);
                var districtSlot = UIUtility.SafeGetComponent<DistrictSlot>(element.transform);
                if (districtSlot != null)
                {
                    _districtSlotList.Add(districtSlot);
                    districtSlot.InitBaseInfo(kvp.Value);
                    element.transform.SetParent(transform, false);
                    element.name = kvp.Key.ToString();
                }
                else
                {
                    break;
                }

            }
        }

    }
}