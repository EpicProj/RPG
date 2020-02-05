using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Sim_FrameWork
{
    public class ApplicationManager : MonoSingleton<ApplicationManager>
    {

        ///*Game Version*///
        public static readonly float GAME_VERSION = 1.0f;

        protected override void Awake()
        {
            base.Awake();
            ObjectManager.Instance.Init(GameObject.Find("RecyclePoolTrs").transform, GameObject.Find("SceneTrs").transform);
        }

        private List<Timer> m_destoryTimerList = new List<Timer>();
        private void LateUpdate()
        {
            foreach(var timer in m_timerList)
            {
                if(timer.timeState == TimerState.Complete || timer.timeState == TimerState.Destory)
                {
                    m_destoryTimerList.Add(timer);
                }else if(timer.timeState != TimerState.Start)
                {
                    continue;
                }

                timer.Update((int)(Time.deltaTime * 1000));
            }

            if (m_destoryTimerList.Count == 0)
                return;
            foreach(var timer in m_destoryTimerList)
            {
                DeleteTimer(timer);
            }
            m_destoryTimerList.Clear();
        }


        #region Coroutine

        public static CoroutineState CreateCoroutine(IEnumerator enumerator)
        {
            return new CoroutineState(enumerator);
        }


        public class CoroutineState
        {


            bool _running;
            public bool Running
            {
                get { return _running; }
            }
            bool _paused;
            public bool Paused
            {
                get { return _paused; }
            }

            public delegate void FinishedHandler(bool manual);
            public event FinishedHandler finished;

            IEnumerator coroutine;
            bool stopped;


             
            public CoroutineState(IEnumerator e)
            {
                coroutine = e;
            }

            public void Pause()
            {
                _paused = true;
            }
            public void UnPause()
            {
                _paused = false;
            }

            public void Start()
            {
                _running = true;
                ApplicationManager.Instance.StartCoroutine(enumerator());
            }

            public void Stop()
            {
                stopped = true;
                _running = false;
            }

            IEnumerator enumerator()
            {
                yield return null;
                IEnumerator e = coroutine;
                while (_running)
                {
                    if (_paused)
                        yield return null;
                    else
                    {
                        if(e!=null && e.MoveNext())
                        {
                            yield return e.Current;
                        }
                        else
                        {
                            _running = false;
                        }
                    }
                }

                FinishedHandler handler = finished;
                if (handler != null)
                    handler(stopped);
            }

        }
        #endregion

        #region Timer

        private List<Timer> m_timerList = new List<Timer>();

        /// <summary>
        /// Start Timer 毫秒
        /// </summary>
        /// <param name="dutation"></param> 持续时长
        /// <param name="triggerTime"></param> 间隔
        /// <returns></returns>
        public static Timer StartTimer(int dutation,int triggerTime=-1,Action onStart=null,Action<int> onUpdate=null,Action onComplete=null,Action onPause=null,Action onContinue=null)
        {
            return Instance._timer(dutation, triggerTime, onStart, onUpdate, onComplete, onPause, onContinue);
        }


        private Timer _timer(int duration,int triggerTime,Action onstart,Action<int> onUpdate,Action onComplete,Action onPause,Action OnContinue)
        {
            Timer timer = new Timer();
            timer.Init(duration);
            timer.TriggerTime = triggerTime;
            timer.OnStart = onstart;
            timer.OnUpdate = onUpdate;
            timer.OnComplete = onComplete;
            timer.OnPause = onPause;
            timer.OnContinue = OnContinue;

            m_timerList.Add(timer);
            return timer;
        }


        public void AddTimer(Timer timer)
        {
            m_timerList.Add(timer);
        }

        public bool DeleteTimer(Timer timer)
        {
            timer.DestoryTimer();
            return m_timerList.Remove(timer);
        }

        public void DeleteAllTimer()
        {
            foreach(var timer in m_timerList)
            {
                timer.DestoryTimer();
                m_timerList.Clear();
            }
        }


        #endregion

    }


    public class Coroutine_Extend
    {
        ApplicationManager.CoroutineState coroutine;

        public bool Running
        {
            get { return coroutine.Running; }
        }
        public bool Paused
        {
            get { return coroutine.Paused; }
        }

        public delegate void Callback(bool manual);
        public event Callback callback;

        public Coroutine_Extend(IEnumerator e,bool autoStart = true)
        {
            coroutine = ApplicationManager.CreateCoroutine(e);
            coroutine.finished += CoroutineFinished;
            if (autoStart)
                Start();
                
        }

        public void Start()
        {
            coroutine.Start();
        }
        public void Stop()
        {
            coroutine.Stop();
        }

        public void Pause()
        {
            coroutine.Pause();
        }

        public void UnPause()
        {
            coroutine.UnPause();
        }

        void CoroutineFinished(bool manual)
        {
            Callback c = callback;
            if (c != null)
                c(manual);
        }



    }

    public enum TimerState
    {
        Idle,
        Start,
        Pause,
        Complete,
        Destory
    }

    public class Timer
    {
        public int StartTime { get; protected set; }
        public int CurrentTime { get; protected set; }
        public int Duration { get; protected set; }
        public int EndTime { get; protected set; }
        public int TriggerTime { get; set; }
        public int TriggerNum { get; set; }


        public TimerState timeState { get; protected set; }

        public Action OnStart;
        public Action OnPause;
        public Action OnContinue;
        public Action OnComplete;
        public Action<int> OnUpdate;

        public void Init(int duration)
        {
            Duration = duration;
            StartTime = (int)(Time.time * 1000);
            EndTime = StartTime + Duration;
            CurrentTime = StartTime;
            TriggerNum = 1;
            TriggerTime = -1;
            timeState = TimerState.Idle;
        }

        public void StartTimer()
        {
            timeState = TimerState.Start;
            OnStart?.Invoke();
        }

        public void Update(int deltaTime)
        {
            if (timeState == TimerState.Pause)
                EndTime += deltaTime;

            if (timeState != TimerState.Start)
                return;

            CurrentTime += deltaTime;
            if (Duration != -1 && CurrentTime > EndTime)
            {
                Complete();
            }
            else if (TriggerTime < 0)
            {
                return;
            }else if (CurrentTime > StartTime + TriggerNum * TriggerTime)
            {
                if (OnUpdate != null)
                {
                    OnUpdate(TriggerNum);
                    TriggerNum++;
                }
            }
        }

        public void Pause(bool callback=true)
        {
            timeState = TimerState.Pause;
            if (callback && OnPause != null)
                OnPause();
        }

        public void Continue(bool callback = true)
        {
            timeState = TimerState.Start;
            if (callback && OnContinue != null)
                OnContinue();
        }

        public void Complete(bool callback = true)
        {
            timeState = TimerState.Complete;
            if (callback && OnComplete != null)
                OnComplete();
        }

        public void DestoryTimer(bool callback = true)
        {
            Complete(callback);
            timeState = TimerState.Destory;
            StartTime = 0;
            Duration = 0;
            EndTime = 0;
            TriggerNum = 1;
            TriggerTime = -1;

            OnStart = null;
            OnPause = null;
            OnUpdate = null;
            OnContinue = null;
            OnComplete = null;
        }


    }

}