using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Sim_FrameWork.UI {
    public class ConsolePageContext : WindowBase
    {
        public ConsolePage m_page;


        public override void Awake(params object[] paralist)
        {
            m_page = UIUtility.SafeGetComponent<ConsolePage>(Transform);
            AddBtnListener();
        }
        public override void OnShow(params object[] paralist)
        {
            
        }

        public override void OnUpdate()
        {
            
        }

        void ParseContent(string c)
        {
            UnityAction<string,string> PrintConsole = (content,info) => {
                var s =  content + "\n" + info+"\n";
                m_page.content.text += s;
            };
            var text = m_page.input.text;
            string[] split = text.Split(',');
            switch (split[0])
            {
                case "AddCurrency":
                    /// Add Currency
                    if (split.Length != 2)
                    {
                        PrintConsole(c, "Input Error");
                        break;
                    }
                    var value = Utility.TryParseInt(split[1]);
                    PlayerManager.Instance.AddCurrency(value, ResourceAddType.current);
                    break;
                case "AddMaterial":
                    if (split.Length != 3)
                    {
                        PrintConsole(c, "Input Error");
                        break;
                    }
                    var materialID = Utility.TryParseInt(split[1]);
                    if (MaterialModule.GetMaterialByMaterialID(materialID) == null)
                    {
                        PrintConsole(c, "MaterialID Not Found!");
                        break;
                    }
                    var valueMa = Utility.TryParseInt(split[2]);
                    PlayerManager.Instance.AddMaterialData(materialID, (ushort)valueMa);
                    break;
                case "ShowEvent":
                    if (split.Length != 2)
                    {
                        PrintConsole(c, "Input Error");
                        break;
                    }
                    var EventID = Utility.TryParseInt(split[1]);
                    if (ExploreModule.GetExploreEventDataByKey(EventID) == null)
                    {
                        PrintConsole(c, "EventID Not Found!");
                        break;
                    }
                    UIGuide.Instance.ShowRandomEventDialog(EventID,0,0,0);
                    break;
            }
        }

        void AddBtnListener()
        {
            AddButtonClickListener(m_page.ClearBtn, () =>
            {
                m_page.content.text = "";
            });
            AddButtonClickListener(m_page.ConfirmBtn, () =>
            {
                ParseContent(m_page.input.text);
                m_page.input.text = "";
                m_page.bar.value = 0;
            });
        }

    }


}