using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork.UI
{
    public class BlockManuPageContext : WindowBase
    {
        private BlockManuPage _page;



        public override void Awake(params object[] paralist)
        {
            _page = UIUtility.SafeGetComponent<BlockManuPage>(Transform);

        }



        public override void OnShow(params object[] paralist)
        {
            base.OnShow(paralist);
        }

        public override bool OnMessage(UIMessage msg)
        {
            return base.OnMessage(msg);
        }



        public override void OnUpdate()
        {
           
        }
    }
}