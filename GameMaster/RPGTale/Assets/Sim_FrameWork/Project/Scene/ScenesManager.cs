using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

namespace Sim_FrameWork
{
    public class ScenesManager : MonoSingleton<ScenesManager>
    {

        public string CurrentSceneName;
        public bool AlreadyLoadScene;

        [HideInInspector]
        public static int LoadingProgress = 0;


        /// <summary>
        /// Over  CallBack
        /// </summary>
        public Action LoadSceneOverCallBack;
        /// <summary>
        /// Start CallBack
        /// </summary>
        public Action LoadSceneStartCallBack;

        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(gameObject);
        }

        public void LoadingScene(string SceneName)
        {
            LoadingProgress = 0;
            StartCoroutine(LoadingSceneAsync(SceneName));
        }


        /// <summary>
        /// Set Scene Environment
        /// </summary>
        /// <param name="sceneName"></param>
        void SetSceneSetting(string sceneName)
        {

        }


        IEnumerator LoadingSceneAsync(string sceneName)
        {
            LoadSceneStartCallBack?.Invoke();

            ClearCache();
            UIManager.Instance.ResetWinDic();
            AlreadyLoadScene = false;

            /// LoadScene
            AsyncOperation unLoadScene = SceneManager.LoadSceneAsync(UIPath.ScenePath.Scene_Loading, LoadSceneMode.Single);
            while(unLoadScene !=null && !unLoadScene.isDone)
            {
                yield return new WaitForEndOfFrame();
            }
            LoadingProgress = 0;
            int targetProgress = 0;

            AsyncOperation asyncScene = SceneManager.LoadSceneAsync(sceneName);
            if(asyncScene!=null && !asyncScene.isDone)
            {
                asyncScene.allowSceneActivation = false;
                while (asyncScene.progress < 0.9f)
                {
                    targetProgress = (int)asyncScene.progress * 100;
                    yield return new WaitForEndOfFrame();

                    ///Smooth
                    while (LoadingProgress < targetProgress)
                    {
                        ++LoadingProgress;
                        yield return new WaitForEndOfFrame();
                    }
                }
                CurrentSceneName = sceneName;
                SetSceneSetting(sceneName);
                targetProgress = 100;
                while (LoadingProgress < targetProgress - 2)
                {
                    ++LoadingProgress;
                    yield return new WaitForEndOfFrame();
                }
                LoadingProgress = 100;
                asyncScene.allowSceneActivation = true;
                AlreadyLoadScene = true;

                LoadSceneOverCallBack?.Invoke();

            }
        }


        protected void ClearCache()
        {
            ObjectManager.Instance.ClearCache();
            ResourceManager.Instance.ClearCache();
        }

    }
}