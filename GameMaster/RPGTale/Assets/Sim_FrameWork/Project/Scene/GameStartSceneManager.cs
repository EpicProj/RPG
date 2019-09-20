using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sim_FrameWork.UI;
using UnityEngine.EventSystems;

namespace Sim_FrameWork
{
    public class GameStartSceneManager : MonoSingleton<GameStartSceneManager>
    {
        protected override void Awake()
        {
            base.Awake();
            AssetBundleManager.Instance.LoadAssetBundleConfig();
            ObjectManager.Instance.Init(transform.Find("RecyclePoolTrs"), transform.Find("SceneTrs"));
            ResourceManager.Instance.Init(this);
            UIManager.Instance.Init(GameObject.Find("MainCanvas").transform as RectTransform, GameObject.Find("MainCanvas/Window").transform as RectTransform, GameObject.Find("MainCanvas/UICamera").GetComponent<Camera>(), GameObject.Find("MainCanvas/EventSystem").GetComponent<EventSystem>());

            RegisterUI();
            UIManager.Instance.PopUpWnd(UIPath.Game_Entry_Page);
        }


        private void RegisterUI()
        {
           
        }

    }
}