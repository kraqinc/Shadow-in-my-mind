using UnityEngine;
using ShadowInMyMind.Core;
using ShadowInMyMind.Sanity;
using ShadowInMyMind.Audio;

namespace ShadowInMyMind.Puzzles
{
    public class MemoryFragment : MonoBehaviour
    {
        public string FragmentID;
        [TextArea(3,6)] public string NarrativeText;
        public AudioClip NarrativeAudio;
        public float SanityDrain     = 10f;
        public float FloatAmplitude  = 0.3f;
        public float FloatSpeed      = 1f;
        public GameObject CollectVFX;

        private bool _collected;
        private Vector3 _startPos;

        private void Start() => _startPos = transform.position;

        private void Update()
        {
            if (!_collected)
                transform.position = _startPos + Vector3.up * Mathf.Sin(Time.time * FloatSpeed) * FloatAmplitude;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_collected || !other.CompareTag("Player")) return;
            _collected = true;

            GameManager.Instance?.SanitySystem?.DrainInstant(SanityDrain);
            PostProcessingController.Instance?.TriggerGlitch(1.2f);
            if (NarrativeAudio != null) AudioManager.Instance?.PlayOneShot(NarrativeAudio);
            if (CollectVFX != null) Instantiate(CollectVFX, transform.position, Quaternion.identity);

            PlayerPrefs.SetInt("Fragment_" + FragmentID, 1);
            Destroy(gameObject, 0.1f);
        }
    }
}
