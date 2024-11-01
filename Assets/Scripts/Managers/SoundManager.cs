using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

namespace BlueRiver
{
    [RequireComponent(typeof(AudioSource))]
    public class SoundManager : MonoSingleton<SoundManager>
    {
        public float masterVolumeSFX = 1.0f;
        public float masterVolumeBGM = 1.0f;

        [SerializeField] AudioClip BGMClip;
        [SerializeField] AudioClip[] audioClip;

        Dictionary<string, AudioClip> audioClipsDic;
        AudioSource sfxPlayer;
        AudioSource bgmPlayer;

        protected override void Awake()
        {
            base.Awake();
            AwakeAfter();
        }

        private void AwakeAfter()
        {
            sfxPlayer = GetComponent<AudioSource>();
            SetupBGM();

            audioClipsDic = new Dictionary<string, AudioClip>();
            foreach (AudioClip a in audioClip)
            {
                audioClipsDic.Add(a.name, a);
            }
        }

        private void Start()
        {
            if (bgmPlayer != null)
                bgmPlayer.Play();
        }

        private void SetupBGM()
        {
            if (BGMClip == null) return;

            var child = new GameObject("BGM");
            child.transform.SetParent(transform);
            bgmPlayer = Utils.Utils.GetOrAddComponent<AudioSource>(child);
            bgmPlayer.clip = BGMClip;
            bgmPlayer.volume = masterVolumeBGM;
        }

        public void PlaySound(string name, float volume = 1.0f)
        {
            if (audioClipsDic.ContainsKey(name) == false) return;

            sfxPlayer.PlayOneShot(audioClipsDic[name], volume * masterVolumeSFX);
        }

        public GameObject PlayLoopSound(string name)
        {
            if (audioClipsDic.ContainsKey(name) == false) return null;

            GameObject _obj = new GameObject("LoopSound");
            AudioSource source = Utils.Utils.GetOrAddComponent<AudioSource>(_obj);
            source.clip = audioClipsDic[name];
            source.volume = masterVolumeBGM;
            source.loop = true;
            source.Play();
            return _obj;
        }

        public void StopBGM()
        {
            bgmPlayer?.Stop();
        }

        public void SetVolumeSFX(float volume)
        {
            masterVolumeSFX = volume;
        }

        public void SetVolumeBGM(float volume)
        {
            masterVolumeBGM = volume;
            bgmPlayer.volume = masterVolumeBGM;
        }
    }
}