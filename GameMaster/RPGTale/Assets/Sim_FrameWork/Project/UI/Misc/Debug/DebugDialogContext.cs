using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sim_FrameWork.UI
{
    public class DebugDialogContext : WindowBase
    {
        private Text contentText;
        public override void Awake(params object[] paralist)
        {
            base.Awake(paralist);
            contentText = Transform.FindTransfrom("Text").SafeGetComponent<Text>();
        }
        public override void OnShow(params object[] paralist)
        {
        }
        public override bool OnMessage(UIMessage msg)
        {
           if(msg.type == UIMsgType.ShowDebugMsg)
            {
                string content = (string)msg.content[0];
                UpdateMsg(content);
                return true;
            }

            return false;
        }

        void UpdateMsg(string content)
        {
            string currentText = contentText.text;
            contentText.text = currentText + "/n" + content;
        }
    }
}