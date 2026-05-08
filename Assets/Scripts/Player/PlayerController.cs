using UnityEngine;
using ShadowInMyMind.Core;

namespace ShadowInMyMind.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour
    {
        [Header("Movement")]
        public float WalkSpeed   = 3.5f;
        public float SprintSpeed = 6f;
        public float CrouchSpeed = 1.5f;
        public float Gravity     = -15f;

        [Header("Look")]
        public float Sensitivity  = 2f;
        public float MaxLookAngle = 85f;
        public Transform CameraHolder;

        [Header("Mobile")]
        public bool  MobileControls = true;
        public FloatingJoystick MoveJoystick;
        public FloatingJoystick LookJoystick;

        [Header("Footsteps")]
        public AudioClip[] FootstepClips;
        public float FootstepInterval = 0.5f;

        private CharacterController _cc;
        private AudioSource _audio;
        private Vector3 _velocity;
        private float _xRot;
        private float _footTimer;

        private void Awake()
        {
            _cc    = GetComponent<CharacterController>();
            _audio = GetComponent<AudioSource>();
            if (!MobileControls)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible   = false;
            }
        }

        private void Update()
        {
            if (GameManager.Instance?.CurrentState == GameState.GameOver) return;
            if (GameManager.Instance?.CurrentState == GameState.Paused)   return;

            HandleLook();
            HandleMove();
            HandleFootsteps();
        }

        private void HandleLook()
        {
            float mx, my;
            if (MobileControls && LookJoystick != null)
            {
                mx = LookJoystick.Horizontal * Sensitivity;
                my = LookJoystick.Vertical   * Sensitivity;
            }
            else
            {
                mx = Input.GetAxis("Mouse X") * Sensitivity;
                my = Input.GetAxis("Mouse Y") * Sensitivity;
            }

            _xRot = Mathf.Clamp(_xRot - my, -MaxLookAngle, MaxLookAngle);
            CameraHolder.localRotation = Quaternion.Euler(_xRot, 0f, 0f);
            transform.Rotate(Vector3.up * mx);
        }

        private void HandleMove()
        {
            float h, v;
            if (MobileControls && MoveJoystick != null)
            {
                h = MoveJoystick.Horizontal;
                v = MoveJoystick.Vertical;
            }
            else
            {
                h = Input.GetAxis("Horizontal");
                v = Input.GetAxis("Vertical");
            }

            bool sprinting = Input.GetKey(KeyCode.LeftShift);
            bool crouching = Input.GetKey(KeyCode.LeftControl);
            float speed    = crouching ? CrouchSpeed : sprinting ? SprintSpeed : WalkSpeed;

            Vector3 move = transform.right * h + transform.forward * v;
            if (_cc.isGrounded && _velocity.y < 0f) _velocity.y = -2f;
            _velocity.y += Gravity * Time.deltaTime;
            _cc.Move((move * speed + _velocity) * Time.deltaTime);
        }

        private void HandleFootsteps()
        {
            if (!_cc.isGrounded) return;
            Vector3 flatVel = new Vector3(_cc.velocity.x, 0, _cc.velocity.z);
            if (flatVel.magnitude < 0.5f) return;

            _footTimer -= Time.deltaTime;
            if (_footTimer <= 0f)
            {
                if (FootstepClips != null && FootstepClips.Length > 0)
                    _audio?.PlayOneShot(FootstepClips[Random.Range(0, FootstepClips.Length)], 0.5f);
                _footTimer = FootstepInterval;
            }
        }
    }
}
