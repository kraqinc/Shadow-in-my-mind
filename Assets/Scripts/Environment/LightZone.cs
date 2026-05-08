using UnityEngine;
using ShadowInMyMind.Core;
using ShadowInMyMind.Sanity;

namespace ShadowInMyMind.Environment
{
    public class LightZone : MonoBehaviour
    {
        public float RecoveryRate = 5f;
        public bool  OneTimeUse   = false;
        private bool _used;
        private SanitySystem _sanity;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            _sanity = GameManager.Instance?.SanitySystem;
        }

        private void OnTriggerStay(Collider other)
        {
            if (!other.CompareTag("Player") || _sanity == null) return;
            if (OneTimeUse && _used) return;
            _sanity.RecoverSanity(RecoveryRate * Time.deltaTime);
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            if (OneTimeUse) _used = true;
        }
    }
}
