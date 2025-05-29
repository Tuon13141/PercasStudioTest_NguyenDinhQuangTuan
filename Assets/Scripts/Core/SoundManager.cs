using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

namespace Project_Data.Scripts
{
    public class SoundManager : Singleton<SoundManager>
    {
        public AudioClip clickSound;
        public AudioClip openHud;
        public AudioClip closeHud;
 
        public AudioSource bgMusic;
        public bool IsSoundOn { get; private set; } = true;
        public bool IsMusicOn { get; private set; } = true;

        public void Init(bool sound, bool music)
        {
            IsMusicOn = music;
            IsSoundOn = sound;
            SetMusic(music);
        }
        public void PlayClickSound()
        {
            if (!IsSoundOn)
                return;
            PlaySound(clickSound);
        }

        public void PlayOpenHudSound()
        {
            if (!IsSoundOn)
                return;
            PlaySound(openHud);
        }

        public void PlayCloseHudSound()
        {
            if (!IsSoundOn)
                return;
            PlaySound(closeHud);
        }

        public void SetSounds(bool isOn)
        {
            IsSoundOn = isOn;
            GlobalConfig.Instance.UserData.sound = isOn;
        }

        public void SetMusic(bool isOn)
        {
            IsMusicOn = isOn;
            GlobalConfig.Instance.UserData.music = isOn;

            if (IsMusicOn)
            {
                bgMusic.Play();
            }
            else
            {
                bgMusic.Stop();
            }
        }

        public void PlaySound(AudioClip audioClip, float volume = 1)
        {
            GameObject gameObject = new GameObject("One shot audio");
            gameObject.transform.position = gameObject.transform.position;
            AudioSource audioSource = (AudioSource)gameObject.AddComponent(typeof(AudioSource));
            audioSource.clip = audioClip;
            audioSource.spatialBlend = 0f;
            audioSource.volume = volume;
            audioSource.Play();
            Object.Destroy(gameObject, audioClip.length * ((Time.timeScale < 0.01f) ? 0.01f : Time.timeScale));
        }
    }
}