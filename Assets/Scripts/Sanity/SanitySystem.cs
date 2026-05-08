using UnityEngine;
using ShadowInMyMind.Core;
using ShadowInMyMind.Audio;

namespace ShadowInMyMind.Sanity
{
    public class SanitySystem : MonoBehaviour
    {
        [Header("Sanity")]
        [Range(0f,100f)] public float MaxSanity = 100f;
        [Range(0f,100f)] public float CurrentSanity = 100f;

        [Header("Drain")]
        public float PassiveDrainRate    = 0.5f;
        public float ShadowProximityDrain = 3f;
        public float BreakdownThreshold  = 20f;

        [Header("Events")]
        public UnityEngine.Events.UnityEvent OnBreakdownEnter;
        public UnityEngine.Events.UnityEvent OnBreakdownExit;
        public UnityEngine.Events.UnityEvent OnSanityDepleted;

        public float SanityPercent => CurrentSanity / MaxSanity;

        private bool _inBreakdown;
        private bool _shadowNearby;

        private void Update()
        {
            if (GameManager.Instance?.CurrentState != GameState.Playing) return;

            float drain = PassiveDrainRate + (_shadowNearby ? ShadowProximityDrain : 0f);
            CurrentSanity = Mathf.Clamp(CurrentSanity - drain * Time.deltaTime, 0f, MaxSanity);

            PostProcessingController.Instance?.SetIntensity(1f - SanityPercent);
            AudioManager.Instance?.SetWhisperVolume(1f - SanityPercent);

            if (CurrentSanity <= 0f) OnSanityDepleted?.Invoke();

            if (!_inBreakdown && CurrentSanity <= BreakdownThreshold)
            {
                _inBreakdown = true;
                GameManager.Instance?.SetState(GameState.MentalBreakdown);
                OnBreakdownEnter?.Invoke();
            }
            else if (_inBreakdown && CurrentSanity > BreakdownThreshold + 10f)
            {
                _inBreakdown = false;
                GameManager.Instance?.SetState(GameState.Playing);
                OnBreakdownExit?.Invoke();
            }
        }

        public void SetShadowNearby(bool nearby) => _shadowNearby = nearby;
        public void RecoverSanity(float amount)  => CurrentSanity = Mathf.Clamp(CurrentSanity + amount, 0f, MaxSanity);
        public void DrainInstant(float amount)   => CurrentSanity = Mathf.Clamp(CurrentSanity - amount, 0f, MaxSanity);
    }
}
