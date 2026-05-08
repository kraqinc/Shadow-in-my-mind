using UnityEngine;
using System.Collections;

namespace ShadowInMyMind.Audio
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }

        public AudioSource AmbienceSource;
        public AudioSource WhisperSource;
        public AudioSource MusicSource;

        public AudioClip[] AmbienceClips;

        [Range(0f,1f)] public float WhisperTargetVolume;
        public float WhisperFadeSpeed = 2f;
        public float MaxWhisperVolume = 0.6f;

        private void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Update()
        {
            if (WhisperSource != null)
                WhisperSource.volume = Mathf.Lerp(
                    WhisperSource.volume,
                    WhisperTargetVolume * MaxWhisperVolume,
                    Time.deltaTime * WhisperFadeSpeed);
        }

        public void PlayAmbience()
        {
            if (AmbienceClips == null || AmbienceClips.Length == 0) return;
            AmbienceSource.clip = AmbienceClips[Random.Range(0, AmbienceClips.Length)];
            AmbienceSource.loop = true;
            AmbienceSource.Play();
        }

        public void SetWhisperVolume(float t) => WhisperTargetVolume = Mathf.Clamp01(t);

        public void PlayOneShot(AudioClip clip, float vol = 1f)
        {
            if (clip == null || AmbienceSource == null) return;
            AmbienceSource.PlayOneShot(clip, vol);
        }

        public IEnumerator FadeOut(AudioSource src, float dur)
        {
            float start = src.volume;
            for (float t = 0; t < dur; t += Time.deltaTime)
            {
                src.volume = Mathf.Lerp(start, 0f, t / dur);
                yield return null;
            }
            src.Stop();
            src.volume = start;
        }
    }
}
