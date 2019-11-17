using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Sim_FrameWork
{
    public class DistrictBuildElement : BaseElement
    {
        public GameObject BuildCostContent;
        public Image Icon;
        public Text Name;

        public void InitCostElementData(DistrictData districtData)
        {
            if (districtData == null)
            {
                Debug.LogError("Init Cost Element Error ,DistrictData is null");
                return;
            }
            Icon.sprite = DistrictModule.GetDistrictIconSpriteList(districtData.DistrictID)[0];
            Name.text = DistrictModule.GetDistrictName(districtData);

            //Set MaterialCost
            Dictionary<Material, int> materialCost = DistrictModule.Instance.GetMaterialCost(districtData);
            foreach(KeyValuePair<Material,int> kvp in materialCost)
            {
                GameObject element = ObjectManager.Instance.InstantiateObject(UIPath.PrefabPath.DISTRICT_BUILD_COST_PREFAB_PATH);
                element.transform.Find("Image").GetComponent<Image>().sprite = MaterialModule.GetMaterialSprite(kvp.Key.MaterialID);
                element.transform.Find("Text").GetComponent<Text>().text = string.Format("{0} X{1}", MaterialModule.GetMaterialName(kvp.Key.MaterialID), kvp.Value.ToString());
                element.transform.SetParent(BuildCostContent.transform,false);
            }
            //Set Currency
            GameObject currency = ObjectManager.Instance.InstantiateObject(UIPath.PrefabPath.DISTRICT_BUILD_COST_PREFAB_PATH);
            currency.transform.Find("Image").GetComponent<Image>().sprite = MaterialModule.GetMaterialSprite(1);
            currency.transform.Find("Text").GetComponent<Text>().text = string.Format("{0} X{1}", MaterialModule.GetMaterialName(1),districtData.CurrencyCost.ToString());
            currency.transform.SetParent(BuildCostContent.transform, false);

            //Set Time
            GameObject time = ObjectManager.Instance.InstantiateObject(UIPath.PrefabPath.DISTRICT_BUILD_COST_PREFAB_PATH);
            time.transform.Find("Image").GetComponent<Image>().sprite = MaterialModule.GetMaterialSprite(2);
            time.transform.Find("Text").GetComponent<Text>().text = string.Format("{0} X{1}", MaterialModule.GetMaterialName(2), districtData.CurrencyCost.ToString());
            time.transform.SetParent(BuildCostContent.transform, false);
        }


    }
}