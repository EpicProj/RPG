using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sim_FrameWork
{
    public class GeneralOrganizationElement : BaseElement
    {
        [Header("Base Info")]
        public Image Icon;
        public Text OrganizationName;
        public Image AreaIcon;
        public Text AreaText;



        private OrganizationDataModel _model;


    }
}