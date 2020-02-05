using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork {
    public class AudioManager : MonoSingleton<AudioManager>
    {
        private AudioListener audioListener;

        private AudioSource BGMSource;

        protected override void Awake()
        {
            base.Awake();
            audioListener = transform.SafeGetComponent<AudioListener>();
            BGMSource = transform.FindTransfrom("BGM").SafeGetComponent<AudioSource>();
            DontDestroyOnLoad(gameObject);
        }

        public void PlaySound(AudioClip clip, bool loop = false)
        {
            if (clip == null)
                return;

            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.clip = clip;
            source.Play();
            source.loop = loop;

            if (!loop)
            {
                StartCoroutine(DestoryAfterPlay(source));
            }
        }

        public void PlaySound(string path,bool loop = false)
        {
            var clip = ResourceManager.Instance.LoadResource<AudioClip>(path);
            PlaySound(clip, loop);
        }


        public void PlayBGM(AudioClip clip,bool loop = true)
        {
            if (clip == null)
                return;
            if (BGMSource != null)
            {
                BGMSource.clip = clip;
                BGMSource.Play();
                BGMSource.loop = loop;
            }
        }
        public void PlayBGM(string path,bool loop = true)
        {
            var clip = ResourceManager.Instance.LoadResource<AudioClip>(path);
            PlayBGM(clip, loop);
        }

        public void StopBGM()
        {
            BGMSource.Stop();
            ResourceManager.Instance.ReleaseResource(BGMSource.clip);
        }


        private IEnumerator DestoryAfterPlay(AudioSource source)
        {
            yield return new WaitForSeconds(source.clip.length);
            ResourceManager.Instance.ReleaseResource(source.clip);
            Destroy(source);
        }

        public void StopAllSounds()
        {
            foreach(AudioSource source in transform)
            {
                ResourceManager.Instance.ReleaseResource(source.clip);
                Destroy(source);
            }
        }

    }
}