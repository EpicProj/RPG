using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Sim_FrameWork.UI;

namespace Sim_FrameWork {

    public enum WindowType
    {
        Page,
        Dialog,
        SPContent,
    }

    public class UIManager : Singleton<UIManager> {

        //UI节点
        public RectTransform m_UiRoot;
        //窗口节点
        private RectTransform m_WndRoot;
        private RectTransform m_DialogRoot;
        private RectTransform m_SpcontentRoot;

        //UI摄像机
        private Camera m_UICamera;
        //EventSystem节点
        private EventSystem m_EventSystem;
        //屏幕的宽高比
        private float m_CanvasRate = 0;

        private string m_UIPrefabPath = "Assets/Prefabs/UI/";
        //注册的字典
        private Dictionary<string, System.Type> m_RegisterDic = new Dictionary<string, System.Type>();
        //所有打开的窗口
        private Dictionary<string, WindowBase> m_WindowDic = new Dictionary<string, WindowBase>();
        //打开的窗口列表
        private List<WindowBase> m_WindowList = new List<WindowBase>();

        public List<string> _currentWindowNameList=new List<string> ();

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="uiRoot">UI父节点</param>
        /// <param name="wndRoot">窗口父节点</param>
        /// <param name="uiCamera">UI摄像机</param>
        public void Init(RectTransform uiRoot, RectTransform wndRoot, RectTransform dialogRoot,RectTransform spContentRoot,  Camera uiCamera, EventSystem eventSystem)
        {
            m_UiRoot = uiRoot;
            m_WndRoot = wndRoot;
            m_DialogRoot = dialogRoot;
            m_SpcontentRoot = spContentRoot;
            m_UICamera = uiCamera;
            m_EventSystem = eventSystem;
            m_CanvasRate = Screen.height / (m_UICamera.orthographicSize * 2);
            if (m_UiRoot == null)
                Debug.LogWarning("UiRoot=null,Root=" + uiRoot);
            if (wndRoot == null)
                Debug.LogWarning("WndRoot=null,Root=" + wndRoot);
        }

        /// <summary>
        /// 设置所有节目UI路径
        /// </summary>
        /// <param name="path"></param>
        public void SetUIPrefabPath(string path)
        {
            m_UIPrefabPath = path;
        }

        /// <summary>
        /// 显示或者隐藏所有UI
        /// </summary>
        public void ShowOrHideUI(bool show)
        {
            if (m_UiRoot != null)
            {
                m_UiRoot.gameObject.SetActive(show);
            }
        }

        /// <summary>
        /// 设置默认选择对象
        /// </summary>
        /// <param name="obj"></param>
        public void SetNormalSelectObj(GameObject obj)
        {
            if (m_EventSystem == null)
            {
                m_EventSystem = EventSystem.current;
            }
            m_EventSystem.firstSelectedGameObject = obj;
        }

        /// <summary>
        /// 窗口的更新
        /// </summary>
        public void OnUpdate()
        {
            for (int i = 0; i < m_WindowList.Count; i++)
            {
                if (m_WindowList[i] != null)
                {
                    m_WindowList[i].OnUpdate();
                }
            }
        }

        /// <summary>
        /// 窗口注册方法
        /// </summary>
        /// <typeparam name="T">窗口泛型类</typeparam>
        /// <param name="name">窗口名</param>
        public void Register<T>(string name) where T : UI.WindowBase
        {
            if (!m_RegisterDic.ContainsKey(name))
            {
                m_RegisterDic[name] = typeof(T);
            }
        }

        /// <summary>
        /// 发送消息给窗口
        /// </summary>
        /// <param name="name">窗口名</param>
        /// <param name="msgID">消息ID</param>
        /// <param name="paralist">参数数组</param>
        /// <returns></returns>
        public bool SendMessageToWnd(string name, UIMessage message)
        {
            WindowBase wnd = FindWndByName<WindowBase>(name);
            if (wnd != null && wnd.currentStates== WindowBase.WindowStates.show)
            {
                return wnd.OnMessage(message);
            }
            return false;
        }

        /// <summary>
        /// 根据窗口名查找窗口
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public T FindWndByName<T>(string name) where T : WindowBase
        {
            WindowBase wnd = null;
            if (m_WindowDic.TryGetValue(name, out wnd))
            {
                return (T)wnd;
            }

            return null;
        }
        public void ResetWinDic()
        {
            m_WindowDic.Clear();
        }




        /// <summary>
        /// 打开窗口
        /// </summary>
        /// <param name="wndName"></param>
        /// <param name="bTop"></param>
        /// <param name="para1"></param>
        /// <param name="para2"></param>
        /// <param name="para3"></param>
        /// <returns></returns>
        public WindowBase PopUpWnd(string wndName, WindowType type = WindowType.Page, bool bTop = true, params object[] paralist)
        {
            WindowBase wnd = FindWndByName<WindowBase>(wndName);
            if (wnd == null)
            {
                System.Type tp = null;
                if (m_RegisterDic.TryGetValue(wndName, out tp))
                {
                    wnd = System.Activator.CreateInstance(tp) as WindowBase;
                }
                else
                {
                    Debug.LogError("找不到窗口对应的脚本，窗口名是：" + wndName);
                    return null;
                }

                GameObject wndObj = ObjectManager.Instance.InstantiateObject(m_UIPrefabPath + wndName +".prefab", false, false);
                if (wndObj == null)
                {
                    Debug.Log("创建窗口Prefab失败：" + wndName);
                    return null;
                }

                if (!m_WindowDic.ContainsKey(wndName))
                {
                    m_WindowList.Add(wnd);
                    m_WindowDic.Add(wndName, wnd);
                }

                wnd.GameObject = wndObj;
                wnd.Transform = wndObj.transform;
                wnd.Name = wndName;
                wnd.Awake(paralist);
                if(type== WindowType.Page)
                {
                    wndObj.transform.SetParent(m_WndRoot, false);
                }
                else if(type == WindowType.Dialog)
                {
                    wndObj.transform.SetParent(m_DialogRoot, false);
                }else if(type== WindowType.SPContent)
                {
                    wndObj.transform.SetParent(m_SpcontentRoot, false);
                }
               

                if (bTop)
                {
                    wndObj.transform.SetAsLastSibling();
                }

                wnd.OnShow(paralist);
                wnd.currentStates = WindowBase.WindowStates.show;
                _currentWindowNameList.Add(wndName);
            }
            else
            {
                ShowWnd(wndName, bTop ,paralist);
                _currentWindowNameList.Add(wndName);
            }

            return wnd;
        }

        /// <summary>
        /// 根据窗口名关闭窗口
        /// </summary>
        /// <param name="name"></param>
        /// <param name="destory"></param>
        public void CloseWnd(string name, bool destory = false)
        {
            WindowBase wnd = FindWndByName<WindowBase>(name);
            CloseWnd(wnd, destory);
        }

        /// <summary>
        /// 根据窗口对象关闭窗口
        /// </summary>
        /// <param name="window"></param>
        /// <param name="destory"></param>
        public void CloseWnd(WindowBase window, bool destory = false)
        {
            if (window != null)
            {
                window.OnDisable();
                window.OnClose();
                if (_currentWindowNameList.Contains(window.Name))
                {
                    _currentWindowNameList.Remove(window.Name);
                }
                if (m_WindowDic.ContainsKey(window.Name))
                {
                    m_WindowDic.Remove(window.Name);
                    m_WindowList.Remove(window);
                }

                if (destory)
                {
                    ObjectManager.Instance.ReleaseObject(window.GameObject, 0, true);
                }
                else
                {
                    ObjectManager.Instance.ReleaseObject(window.GameObject, recycleParent: false);
                }
                window.GameObject = null;
                window = null;
            }
        }

        /// <summary>
        /// 关闭所有窗口
        /// </summary>
        public void CloseAllWnd()
        {
            for (int i = m_WindowList.Count - 1; i >= 0; i--)
            {
                CloseWnd(m_WindowList[i]);
            }
        }

        /// <summary>
        /// 切换到唯一窗口
        /// </summary>
        public void SwitchStateByName(string name,WindowType type,bool bTop = true)
        {
            CloseAllWnd();
            PopUpWnd(name,type,bTop);
        }

        /// <summary>
        /// 根据名字隐藏窗口
        /// </summary>
        /// <param name="name"></param>
        public void HideWnd(string name)
        {
            WindowBase wnd = FindWndByName<WindowBase>(name);
            HideWnd(wnd);
        }

        /// <summary>
        /// 根据窗口对象隐藏窗口
        /// </summary>
        /// <param name="wnd"></param>

        public void HideWnd(WindowBase wnd)
        {
            if (wnd != null)
            {
                wnd.GameObject.SetActive(false);
                wnd.OnDisable();
                wnd.currentStates = WindowBase.WindowStates.hide;
                if (_currentWindowNameList.Contains(wnd.Name))
                {
                    _currentWindowNameList.Remove(wnd.Name);
                }
            }
        }

        /// <summary>
        /// 根据窗口名字显示窗口
        /// </summary>
        /// <param name="name"></param>
        /// <param name="paralist"></param>
        public void ShowWnd(string name, bool bTop = true, params object[] paralist)
        {
            WindowBase wnd = FindWndByName<UI.WindowBase>(name);
            ShowWnd(wnd, bTop, paralist);
        }

        /// <summary>
        /// 根据窗口对象显示窗口
        /// </summary>
        /// <param name="wnd"></param>
        /// <param name="paralist"></param>
        public void ShowWnd(WindowBase wnd, bool bTop = true, params object[] paralist)
        {
            if (wnd != null)
            {
                if (wnd.GameObject != null && !wnd.GameObject.activeSelf)
                    wnd.GameObject.SetActive(true);
                if (bTop) wnd.Transform.SetAsLastSibling();
                wnd.OnShow(paralist);
                wnd.currentStates = WindowBase.WindowStates.show;
            }
        }

        /// <summary>
        /// 显示通用提示条
        /// </summary>
        /// <param name="textID"></param>
        /// <param name="duration"></param>
        public void ShowGeneralHint(string textID,float duration)
        {
            GeneralHintDialogItem item = new GeneralHintDialogItem(textID, duration);
            PopUpWnd(UIPath.WindowPath.General_Hint_Dialog, WindowType.Dialog, true, item);
        }


    }
}