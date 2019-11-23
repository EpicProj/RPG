using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork
{
    public class ApplicationManager : MonoSingleton<ApplicationManager>
    {

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
}