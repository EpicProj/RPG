using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sim_FrameWork
{
    [RequireComponent(typeof(Text))]
    public class GetTextByID : MonoBehaviour
    {
        public string Textid;
        void Awake()
        {
            InitText();
        }

        void InitText()
        {
            if (!string.IsNullOrEmpty(Textid))
            {
                MultiLanguage.Instance.GetTextValue(Textid);
            }
        }
    }
}