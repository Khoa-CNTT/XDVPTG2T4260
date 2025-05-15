using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

using tuleeeeee.Events;

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
        /// 
        /// </summary>
        /// <param name="context"></param>
        public void OnScrollInput(InputAction.CallbackContext context)
        {
            Vector2 newInput = context.ReadValue<Vector2>();
            ScrollEvent.Invoke(newInput);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public void OnSelectWeapon(InputAction.CallbackContext context)
        {
            string pressedKey = context.control.name;
            int number = int.Parse(pressedKey);

            SelectWeaponEvent.Invoke(number);
        }

        public void OnFastSwitchWeapon(InputAction.CallbackContext context)
        {
            bool isClick = false;
            if (context.started)
            {
                isClick = true;
            }
            else if (context.canceled)
            {
                isClick= false;
            }

            FastSwitchWeaponEvent.Invoke(isClick);
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
        /// <summary>
        ///  Reload
        /// </summary>
        /// <param name="context"></param>
        public void OnReloadInput(InputAction.CallbackContext context)
        {
            bool isReloading = false;
            if (context.started)
            {
                isReloading = true;
            }
            else if (context.canceled)
            {
                isReloading = false;
            }
            ReloadEvent.Invoke(isReloading);
        }
        /// <summary>
        /// Fire
        /// </summary>
        /// <param name="context"></param>
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
        private readonly ScrollEvent onScrollEvent = new ScrollEvent();
        private readonly SelectWeaponEvent onSelectWeaponEvent = new SelectWeaponEvent();
        private readonly FastSwitchWeaponEvent onFastSwitchWeaponEvent = new FastSwitchWeaponEvent();
        private readonly AttackEvent onAttackEvent = new AttackEvent();
        private readonly ReloadEvent onReloadEvent = new ReloadEvent();
        public UnityEvent<Vector2> LookEvent => onLookEvent;
        public UnityEvent<Vector2> ScrollEvent => onScrollEvent;
        public UnityEvent<int> SelectWeaponEvent => onSelectWeaponEvent;
        public UnityEvent<bool> FastSwitchWeaponEvent => onFastSwitchWeaponEvent;
        public UnityEvent<bool> AttackEvent => onAttackEvent;
        public UnityEvent<bool> ReloadEvent => onReloadEvent;

        #endregion

    }
}