using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

using tuleeeeee.Events;
using UnityEngine.Windows;

namespace tuleeeeee.MyInput
{
    public class PlayerInputHandler : MonoBehaviour
    {
        private Camera _camera;
        public Vector2 RawMovementInput { get; private set; }
        public int NormInputX { get; private set; }
        public int NormInputY { get; private set; }
        public bool RollInput { get; private set; }
        public bool RollInputStop { get; private set; }
        public bool ShotInput { get; private set; }

        private float rollInputStartTime;
        [SerializeField]
        private float inputHoldTime = 0.2f;

        private void Awake()
        {
            _camera = Camera.main;
        }

        /// <summary>
        ///   Method called when the user input movement
        /// </summary>
        /// <param name="context"></param>
        /// 
        public void OnMoveInput(InputAction.CallbackContext context)
        {
            RawMovementInput = context.ReadValue<Vector2>();
            NormInputX = (int)(RawMovementInput * Vector2.right).normalized.x;
            NormInputY = (int)(RawMovementInput * Vector2.up).normalized.y;
        }

        /// <summary>
        /// Method called when the user input mouse
        /// </summary>
        /// <param name="context"></param>
        public void OnLookInput(InputAction.CallbackContext context)
        {
            Vector2 newInput = context.ReadValue<Vector2>();

            // For mouse input (large values)
            if (Mathf.Abs(newInput.x) > 1f || Mathf.Abs(newInput.y) > 1f)
            {
                Vector3 mouseWorldPosition = _camera.ScreenToWorldPoint(
                    new Vector3(newInput.x, newInput.y, _camera.nearClipPlane));
                mouseWorldPosition.z = 0;
                LookEvent.Invoke((mouseWorldPosition - transform.position).normalized);
            }
            // For controller input (small values)
            else
            {
                if (newInput.magnitude > 0.1f) // Deadzone check
                {
                    LookEvent.Invoke(newInput.normalized);
                }
            }

            /*  if (!(newInput.normalized == newInput)) // GamePad
              {
                  Vector2 worldPos = _camera.ScreenToWorldPoint(newInput);
                  newInput = (worldPos - (Vector2)transform.position).normalized;
              }

              if (newInput.magnitude >= .9f)
              {
                  LookEvent.Invoke(newInput);
              }*/
        }

        /// <summary>
        /// Method called when the user input roll
        /// </summary>
        /// <param name="context"></param>
        public void OnRollInput(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                RollInput = true;
                RollInputStop = false;
                rollInputStartTime = Time.time;
            }
            else if (context.canceled)
            {
                RollInput = false;
                RollInputStop = true;
            }
        }
        public void OnFireInput(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                ShotInput = true;
            }
            else if (context.canceled)
            {
                ShotInput = false;
            }
            AttackEvent.Invoke(ShotInput);
        }

        public void UseRollInput() => RollInput = false;
        private void CheckRollInputHoldTime()
        {
            if (Time.time >= rollInputStartTime + inputHoldTime)
            {
                RollInput = false;
            }
        }

        #region EVENTS

        private readonly LookEvent onLookEvent = new LookEvent();
        private readonly AttackEvent onAttackEvent = new AttackEvent();
        public UnityEvent<Vector2> LookEvent => onLookEvent;
        public UnityEvent<bool> AttackEvent => onAttackEvent;

        #endregion

    }
}