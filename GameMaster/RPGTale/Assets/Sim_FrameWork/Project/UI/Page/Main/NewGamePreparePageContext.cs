using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sim_FrameWork.UI
{
    public partial class NewGamePreparePageContext : WindowBase
    {

        #region Override Method

        public override void Awake(params object[] paralist)
        {
            base.Awake(paralist);
            AddBtnClick();
            SetUpGamePreparePanel();
        }

        public override bool OnMessage(UIMessage msg)
        {
            return base.OnMessage(msg);
        }

        public override void OnShow(params object[] paralist)
        {
            base.OnShow(paralist);
        }


        #endregion

        void AddBtnClick()
        {
            AddButtonClickListener(Transform.FindTransfrom("Back").SafeGetComponent<Button>(), () =>
            {
                UIManager.Instance.HideWnd(this);
                UIGuide.Instance.ShowGameEntryPage();
            });
        }

        void OnGameStartBtnClick()
        {
            ScenesManager.Instance.LoadSceneStartCallBack = () =>
            {
                DataManager.Instance.InitGameData();
            };
            ScenesManager.Instance.LoadingScene(UIPath.ScenePath.Scene_Test);
        }



        void SetUpGamePreparePanel()
        {
            InitDropDownPrepareItem();
            InitSliderPrepareItem();
        }

        void InitDropDownPrepareItem()
        {

        }

        void InitSliderPrepareItem()
        {
            var list = DataManager.Instance.gamePrepareData.preparePropertyDataList;
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].configType == 2)
                {
                    var trans = Transform.FindTransfrom("Content/HardSetPanel/Content");
                    var item = ObjectManager.Instance.InstantiateObject(UIPath.PrefabPath.General_SliderSelectItem);
                    if (item != null)
                    {
                        item.transform.SetParent(trans,false);
                        var itemTrans= item.transform.SafeGetComponent<SliderSelectItem>();
                        ///Init List
                        var config = PlayerModule.GetGamePrepareConfigItem(list[i].configID);
                        if (config != null)
                        {
                            List<GeneralSliderSelectElement> elementList = new List<GeneralSliderSelectElement>();
                            for(int j = 0; j < config.levelMap.Count; j++)
                            {
                                GeneralSliderSelectElement element = new GeneralSliderSelectElement
                                {
                                    showScaleSymbol = config.showScaleSymbol,
                                    index = config.levelMap[j].Level,
                                    linkParam = config.levelMap[j].hardLevelChange,
                                    value = (float)config.levelMap[j].numParam
                                };
                                elementList.Add(element);
                            }
                            itemTrans.SetUpItem(config.configIconPath, config.configNameText,config.defaultSelectLevel,elementList);   
                        }
                    }
                }
            }
        }
    }




    public partial class NewGamePreparePageContext : WindowBase
    {

    }
}