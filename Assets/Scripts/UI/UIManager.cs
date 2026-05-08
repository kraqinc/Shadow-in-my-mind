using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using ShadowInMyMind.Core;
using ShadowInMyMind.Sanity;

namespace ShadowInMyMind.UI
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance { get; private set; }

        [Header("HUD")]
        public CanvasGroup SanityBarGroup;
        public Image SanityFill;
        public Image VHSOverlay;

        [Header("Screens")]
        public CanvasGroup DeathScreen;
        public CanvasGroup PauseScreen;

        private SanitySystem _sanity;

        private void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this;
        }

        private void Start()
        {
            _sanity = GameManager.Instance?.SanitySystem;
            SetAlpha(DeathScreen, 0f);
            SetAlpha(PauseScreen, 0f);
        }

        private void Update()
        {
            if (_sanity == null) return;

            if (SanityFill != null)
                SanityFill.fillAmount = _sanity.SanityPercent;

            if (SanityBarGroup != null)
            {
                float target = _sanity.SanityPercent < 0.4f ? 1f : 0f;
                SanityBarGroup.alpha = Mathf.Lerp(SanityBarGroup.alpha, target, Time.deltaTime * 2f);
            }

            if (VHSOverlay != null)
            {
                Color c = VHSOverlay.color;
                c.a = Mathf.Lerp(0f, 0.3f, 1f - _sanity.SanityPercent);
                VHSOverlay.color = c;
            }
        }

        public void ShowDeathScreen() => StartCoroutine(FadeIn(DeathScreen, 2f));

        private void SetAlpha(CanvasGroup cg, float a)
        {
            if (cg == null) return;
            cg.alpha = a;
            cg.interactable = a > 0f;
            cg.blocksRaycasts = a > 0f;
        }

        private IEnumerator FadeIn(CanvasGroup cg, float dur)
        {
            if (cg == null) yield break;
            cg.gameObject.SetActive(true);
            for (float t = 0; t < dur; t += Time.deltaTime)
            { cg.alpha = t / dur; yield return null; }
            cg.alpha = 1f;
            cg.interactable = cg.blocksRaycasts = true;
        }
    }
}
