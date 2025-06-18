using System;
using UnityEngine;

namespace SoundSystem
{
    public class SoundManager : MonoBehaviour
    {
        public static SoundManager Instance;

        [Header("Звуки игры")]
        public Sound[] sounds;

        void Awake()
        {
            // Singleton
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
                return;
            }

            // Создаем AudioSource для каждого звука
            foreach (Sound s in sounds)
            {
                s.source = gameObject.AddComponent<AudioSource>();
                s.source.clip = s.clip;
                s.source.volume = s.volume;
                s.source.pitch = s.pitch;
                s.source.loop = s.loop;
            }
        }

        public void Play(string name)
        {
            Sound s = System.Array.Find(sounds, sound => sound.name == name);
            if (s == null)
            {
                Debug.LogWarning("Звук не найден: " + name);
                return;
            }

            s.source.PlayOneShot(s.clip);
            Debug.Log("Clicked");
        }

        public void Stop(string name)
        {
            Sound s = System.Array.Find(sounds, sound => sound.name == name);
            if (s != null && s.source.isPlaying)
                s.source.Stop();
        }
        
        public void MenuClick()
        {
           
            Play("Click");
        }
    }
}
