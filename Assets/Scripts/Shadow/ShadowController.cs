using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using ShadowInMyMind.Core;
using ShadowInMyMind.Sanity;

namespace ShadowInMyMind.Shadow
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class ShadowController : MonoBehaviour
    {
        [Header("References")]
        public Transform Player;
        private NavMeshAgent _agent;
        private SanitySystem _sanity;

        [Header("State Thresholds (Sanity %)")]
        public float ObserveThreshold = 0.80f;
        public float StalkThreshold   = 0.60f;
        public float ChaseThreshold   = 0.30f;

        [Header("Speeds")]
        public float StalkSpeed = 2.5f;
        public float ChaseSpeed = 6f;

        [Header("Distances")]
        public float MinObserveDistance = 20f;
        public float CatchDistance      = 5f;

        [Header("Audio")]
        public AudioClip[] WhisperClips;
        public AudioClip BreakdownClip;
        private AudioSource _audio;

        private ShadowState _state = ShadowState.Dormant;
        private float _whisperTimer;

        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
            _audio = GetComponent<AudioSource>();
        }

        private void Start()
        {
            _sanity = GameManager.Instance?.SanitySystem;
            Player  = GameManager.Instance?.Player?.transform;
        }

        private void Update()
        {
            if (Player == null || _sanity == null) return;

            float s = _sanity.SanityPercent;

            if      (s > ObserveThreshold) SetState(ShadowState.Dormant);
            else if (s > StalkThreshold)   SetState(ShadowState.Observing);
            else if (s > ChaseThreshold)   SetState(ShadowState.Stalking);
            else                           SetState(ShadowState.Chasing);

            ExecuteState();
            TickWhispers();
        }

        private void ExecuteState()
        {
            switch (_state)
            {
                case ShadowState.Dormant:
                    gameObject.SetActive(false);
                    break;

                case ShadowState.Observing:
                    gameObject.SetActive(true);
                    KeepDistance();
                    FacePlayer();
                    break;

                case ShadowState.Stalking:
                    _agent.speed = StalkSpeed;
                    MoveTowardsBehind();
                    _sanity.SetShadowNearby(Dist() < 12f);
                    break;

                case ShadowState.Chasing:
                case ShadowState.Breakdown:
                    _agent.speed = _state == ShadowState.Breakdown ? ChaseSpeed * 1.5f : ChaseSpeed;
                    _agent.SetDestination(Player.position);
                    _sanity.SetShadowNearby(true);
                    if (Dist() <= CatchDistance)
                        GameManager.Instance?.SetState(GameState.GameOver);
                    break;
            }
        }

        private void KeepDistance()
        {
            if (Dist() < MinObserveDistance)
            {
                Vector3 away = (transform.position - Player.position).normalized * MinObserveDistance;
                NavMeshHit hit;
                if (NavMesh.SamplePosition(Player.position + away, out hit, 5f, NavMesh.AllAreas))
                    _agent.SetDestination(hit.position);
            }
        }

        private void MoveTowardsBehind()
        {
            Vector3 behind = Player.position - Player.forward * 8f;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(behind, out hit, 5f, NavMesh.AllAreas))
                _agent.SetDestination(hit.position);
        }

        private void FacePlayer()
        {
            Vector3 dir = (Player.position - transform.position);
            dir.y = 0;
            if (dir != Vector3.zero)
                transform.rotation = Quaternion.LookRotation(dir);
        }

        private void TickWhispers()
        {
            _whisperTimer -= Time.deltaTime;
            if (_whisperTimer <= 0f && _state >= ShadowState.Stalking)
            {
                if (WhisperClips != null && WhisperClips.Length > 0)
                    _audio?.PlayOneShot(WhisperClips[Random.Range(0, WhisperClips.Length)]);
                _whisperTimer = Random.Range(8f, 20f);
            }
        }

        public void TriggerBreakdown()
        {
            SetState(ShadowState.Breakdown);
            if (BreakdownClip != null) _audio?.PlayOneShot(BreakdownClip);
        }

        private float Dist() => Vector3.Distance(transform.position, Player.position);

        private void SetState(ShadowState s)
        {
            if (_state == s) return;
            _state = s;
        }
    }

    public enum ShadowState { Dormant, Observing, Stalking, Chasing, Breakdown }
}
