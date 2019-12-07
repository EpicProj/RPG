using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sim_FrameWork.UI;

namespace Sim_FrameWork
{
    public class DistrictGroup : MonoBehaviour
    {
        public List<DistrictSlot> _districtSlotList;

        FunctionBlockInfoData infoData;

        void Awake()
        {
            _districtSlotList = new List<DistrictSlot>();
        }

        public void InitSlot(FunctionBlockInfoData info)
        {
            infoData = info;
            var count = info.districtAreaMax.x * info.districtAreaMax.y;
            foreach(KeyValuePair<Vector2,DistrictAreaBase > kvp in info.currentDistrictBaseDic)
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