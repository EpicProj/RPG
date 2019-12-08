using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sim_FrameWork.UI
{
    public class BuildPanelDetailContext : WindowBase
    {
        private BuildPanelDetail m_page;

        private Image _blockIcon;
        private Text _blockName;
        private Transform _gridContentTrans;
        private Transform _buildCostContentTrans;
        private Text _districtMaxText;
        private Text _desc;
        private Text _timeCost;
        private TypeWriterEffect _typeEffect;

        private Animation _anim;

        private BuildPanelModel model;


        #region Override Method
        public override void Awake(params object[] paralist)
        {
            m_page = UIUtility.SafeGetComponent<BuildPanelDetail>(Transform);
            InitRef();
        }

        public override void OnShow(params object[] paralist)
        {
            var _model = (BuildPanelModel)paralist[0];
            if (_model.ID != model.ID)
            {
                model = _model;
                InitBaseData();
                RefreshDistrictElement();
                RefreshBuildCostPanel();
            }
            if (_anim != null)
                _anim.Play();
            if (_typeEffect != null)
                _typeEffect.StartEffect();
        }

        public override void OnDisable()
        {
            base.OnDisable();
        }

        public override bool OnMessage(UIMessage msg)
        {
            return base.OnMessage(msg);
        }

        public override void OnUpdate()
        {
            Transform.localPosition = InventoryManager.Instance.GetCurrentMousePos();
        }

        void InitRef()
        {
            _anim = UIUtility.SafeGetComponent<Animation>(Transform);
            _blockIcon = UIUtility.SafeGetComponent<Image>(UIUtility.FindTransfrom(m_page.Title, "Icon"));
            _blockName = UIUtility.SafeGetComponent<Text>(UIUtility.FindTransfrom(m_page.Title, "Name"));
            _districtMaxText = UIUtility.SafeGetComponent<Text>(UIUtility.FindTransfrom(m_page.LeftPanel, "Value"));
            _desc = UIUtility.SafeGetComponent<Text>(UIUtility.FindTransfrom(m_page.RightPanel, "Desc"));
            _typeEffect = UIUtility.SafeGetComponent<TypeWriterEffect>(UIUtility.FindTransfrom(m_page.RightPanel, "Desc"));
            _gridContentTrans = UIUtility.FindTransfrom(m_page.LeftPanel, "GridContent");
            _buildCostContentTrans = UIUtility.FindTransfrom(m_page.RightPanel, "Content");
            _timeCost = UIUtility.SafeGetComponent<Text>(UIUtility.FindTransfrom(m_page.RightPanel, "TimeCost/Value"));
        }

        #endregion

        #region Private Method

        bool InitBaseData()
        {
            if (model.ID != 0)
            {
                FunctionBlockDataModel blockModel = new FunctionBlockDataModel();
                if (blockModel.Create(model.BuildData.FunctionBlockID))
                {
                    _blockIcon.sprite = blockModel.Icon;
                    _blockName.text = blockModel.Name;
                    _desc.text = model.Desc;
                    _timeCost.text = model.BuildData.TimeCost.ToString();
                    var blockData = FunctionBlockModule.GetFunctionBlockByBlockID(model.BuildData.FunctionBlockID);
                    if (blockData != null)
                    {
                        var areaMax = FunctionBlockModule.GetFunctionBlockAreaMax(blockData);
                        _districtMaxText.text = string.Format("{0} X {1}", areaMax.x.ToString(), areaMax.y.ToString());
                    }
                    
                    return true;
                }
            }
            return false;
        }


        void InitDistrictBuildObj()
        {
            if (_gridContentTrans.childCount == GlobalConfigData.BuildDetail_District_Area_Max)
                return;
            for (int i = 0; i < GlobalConfigData.BuildDetail_District_Area_Max; i++)
            {
                var obj = ObjectManager.Instance.InstantiateObject(UIPath.PrefabPath.BuildDetail_District_Element);
                if (obj == null)
                    break;
                obj.name = "DistrictElement " + i;
                obj.transform.SetParent(_gridContentTrans, false);
            }

        }
        /// <summary>
        /// 刷新区划格
        /// </summary>
        void RefreshDistrictElement()
        {
            InitDistrictBuildObj();
            var gridLayout = UIUtility.SafeGetComponent<GridLayoutGroup>(_gridContentTrans);
            var blockData = FunctionBlockModule.GetFunctionBlockByBlockID(model.BuildData.FunctionBlockID);
            if(blockData != null)
            {
                foreach(Transform trans in _gridContentTrans)
                {
                    trans.gameObject.SetActive(false);
                }
                var districtDic = FunctionBlockModule.GetFuntionBlockAreaDetailDefaultDataInfo(blockData);

                ///Init Size
                var districtMax = FunctionBlockModule.GetFunctionBlockAreaMax(blockData);
                gridLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
                gridLayout.constraintCount =(int)districtMax.x;
                ///Init Grid
                int index = 0;
                foreach(KeyValuePair<Vector2,DistrictAreaBase> kvp in districtDic)
                {
                    var element = UIUtility.SafeGetComponent<BuildDetailDistrictElement>(_gridContentTrans.GetChild(index));
                    if (element != null)
                    {
                        index++;
                        if (kvp.Value.Locked == false)
                        {
                            element.gameObject.SetActive(true);
                            element.InitEmpetyState(); //TODO
                        }
                        else if (kvp.Value.Locked == true)
                        {
                            element.gameObject.SetActive(true);
                            element.InitLockState();
                        }
                    }
                    else
                    {
                        continue;
                    }
                }
            }
        }


        /// <summary>
        /// 生成建造花费
        /// </summary>
        void InitBuildCostElement()
        {
            if (_buildCostContentTrans.childCount == GlobalConfigData.BuildDetail_Cost_MaxInit_Count)
                return;
            for(int i = 0; i < GlobalConfigData.BuildDetail_Cost_MaxInit_Count; i++)
            {
                var obj = ObjectManager.Instance.InstantiateObject(UIPath.PrefabPath.BuildDetail_Cost_Element);
                if (obj != null)
                {
                    obj.name = "BuildRequire " + i;
                    obj.transform.SetParent(_buildCostContentTrans, false);
                }
            }
        }
        void RefreshBuildCostPanel()
        {
            InitBuildCostElement();
            foreach (Transform trans in _buildCostContentTrans)
            {
                trans.gameObject.SetActive(false);
            }

            var materialDic = PlayerModule.Instance.GetBuildMaterialCost(model.BuildData);
            int index = 0;
            foreach(KeyValuePair<Material,ushort> kvp in materialDic)
            {
                MaterialDataModel model = new MaterialDataModel();
                if (model.Create(kvp.Key.MaterialID))
                {
                    var element = UIUtility.SafeGetComponent<BuildRequireElement>(_buildCostContentTrans.GetChild(index));
                    if (element != null)
                    {
                        element.InitBuildCost(model, kvp.Value);
                        element.gameObject.SetActive(true);
                        index++;
                    }
                }
                else
                {
                    continue;
                }
            }
            
        }


        #endregion
    }
}