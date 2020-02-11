using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sim_FrameWork
{
    public class GeneralDropDownChooseElement
    {
        public int index;
        public string desc;
        public int linkParam;
    }

    public class DropDownChooseItem : MonoBehaviour
    {
        private string configID;
        private List<GeneralDropDownChooseElement> elementList;

        private Dropdown _dropDown;
        private void Awake()
        {
            _dropDown = transform.FindTransfrom("Dropdown").SafeGetComponent<Dropdown>();
        }

        public void SetUpItem(string configID, string iconPath, string nameTextID, int currentLevel, List<GeneralDropDownChooseElement> elementList)
        {
            this.elementList = elementList;

            transform.FindTransfrom("Icon").SafeGetComponent<Image>().sprite = Utility.LoadSprite(iconPath);
            transform.FindTransfrom("Text").SafeGetComponent<Text>().text = MultiLanguage.Instance.GetTextValue(nameTextID);

            List<Dropdown.OptionData> list = new List<Dropdown.OptionData>();
            for(int i = 0; i < elementList.Count; i++)
            {
                Dropdown.OptionData data = new Dropdown.OptionData();
                data.text =MultiLanguage.Instance.GetTextValue (elementList[i].desc);
                list.Add(data);
            }
            _dropDown.options.Clear();
            _dropDown.AddOptions(list);
            
        }
    }
}
