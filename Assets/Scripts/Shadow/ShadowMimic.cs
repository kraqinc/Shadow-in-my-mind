using UnityEngine;
using System.Collections;
using ShadowInMyMind.Core;
using ShadowInMyMind.Sanity;

namespace ShadowInMyMind.Shadow
{
    public class ShadowMimic : MonoBehaviour
    {
        public float ActivationThreshold = 0.5f;
        public GameObject NormalObject;
        public GameObject ShadowObject;
        public float MinReplaceTime = 5f;
        public float MaxReplaceTime = 20f;

        private SanitySystem _sanity;

        private void Start()
        {
            _sanity = GameManager.Instance?.SanitySystem;
            ShowNormal();
            StartCoroutine(MimicRoutine());
        }

        private IEnumerator MimicRoutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(Random.Range(MinReplaceTime, MaxReplaceTime));
                if (_sanity == null || _sanity.SanityPercent > ActivationThreshold) continue;
                ShowShadow();
                yield return new WaitForSeconds(Random.Range(2f, 5f));
                ShowNormal();
            }
        }

        private void ShowNormal()
        {
            if (NormalObject) NormalObject.SetActive(true);
            if (ShadowObject) ShadowObject.SetActive(false);
        }

        private void ShowShadow()
        {
            if (NormalObject) NormalObject.SetActive(false);
            if (ShadowObject) ShadowObject.SetActive(true);
        }
    }
}
