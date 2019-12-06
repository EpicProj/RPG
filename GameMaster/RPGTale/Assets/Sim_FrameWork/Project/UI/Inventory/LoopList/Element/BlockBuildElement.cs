using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sim_FrameWork.UI
{
    public class BlockBuildElement : BaseElementSimple
    {
        public Text Name;
        public Image BlockIcon;
        private Button btn;

        public FunctionBlockDataModel model;
        public int buildID;

        public override void Awake()
        {
            btn = UIUtility.SafeGetComponent<Button>(transform);
        }


        public void InitBuildElement(FunctionBlockDataModel _model,int buildID)
        {
            this.buildID = buildID;
            model = _model;
            Name.text = _model.Name;
            BlockIcon.sprite = _model.Icon;
        }


    }
}