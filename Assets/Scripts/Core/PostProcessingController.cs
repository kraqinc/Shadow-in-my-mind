using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System.Collections;

namespace ShadowInMyMind.Core
{
    [RequireComponent(typeof(Volume))]
    public class PostProcessingController : MonoBehaviour
    {
        public static PostProcessingController Instance { get; private set; }

        private Volume _vol;
        private Vignette _vignette;
        private ChromaticAberration _chromatic;
        private LensDistortion _lens;
        private ColorAdjustments _color;
        private FilmGrain _grain;

        public float MaxVignette   = 0.65f;
        public float MaxChromatic  = 1f;
        public float MaxLens       = -0.5f;
        public float MaxGrain      = 0.6f;
        public float MinExposure   = -2f;

        private bool _glitching;

        private void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this;
            _vol = GetComponent<Volume>();
            _vol.profile.TryGet(out _vignette);
            _vol.profile.TryGet(out _chromatic);
            _vol.profile.TryGet(out _lens);
            _vol.profile.TryGet(out _color);
            _vol.profile.TryGet(out _grain);
        }

        public void SetIntensity(float t)
        {
            t = Mathf.Clamp01(t);
            if (_vignette  != null) _vignette.intensity.value  = t * MaxVignette;
            if (_chromatic != null) _chromatic.intensity.value = t * MaxChromatic;
            if (_lens      != null) _lens.intensity.value      = t * MaxLens;
            if (_grain     != null) _grain.intensity.value     = t * MaxGrain;
            if (_color     != null) _color.postExposure.value  = t * MinExposure;
        }

        public void TriggerGlitch(float duration = 0.5f)
        {
            if (!_glitching) StartCoroutine(GlitchRoutine(duration));
        }

        private IEnumerator GlitchRoutine(float dur)
        {
            _glitching = true;
            if (_chromatic != null) _chromatic.intensity.value = 1f;
            if (_lens      != null) _lens.intensity.value      = Random.Range(-0.8f, 0.8f);
            yield return new WaitForSeconds(dur);
            _glitching = false;
        }
    }
}
