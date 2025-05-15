using System;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.InputSystem;
using UnityEngine;

public enum PlayerMode { Solo, TwoPlayer }

namespace TrDuc.Managers
{
    public class PlayerModeManager
    {
        public PlayerMode CurrentPlayerMode { get; private set; }

        private PlayerInput _playerInput1;
        private PlayerInput _playerInput2;
        private bool _initialized = false;

        public event Action<PlayerMode> OnMode;

        public PlayerModeManager(PlayerInput p1, PlayerInput p2)
        {
            if (p1 == null || p2 == null)
            {
                Debug.LogError("PlayerModeManager: PlayerInput references cannot be null!");
                return;
            }

            _playerInput1 = p1;
            _playerInput2 = p2;
            _initialized = true;
        }
        public void SetPlayerMode(PlayerMode mode)
        {
            if (!_initialized) return;

            CurrentPlayerMode = mode;
            OnMode?.Invoke(mode);
        }

        public void ApplyPlayerMode()
        {
            if (!_initialized) return;

            AssignControllers();
        }

        public PlayerMode GetPlayerMode()
        {
            return CurrentPlayerMode;
        }

        private void AssignControllers()
        {
            if (!_initialized) return;

            try
            {
                var gamepads = Gamepad.all;
                bool hasKeyboard = Keyboard.current != null;

                switch (CurrentPlayerMode)
                {
                    case PlayerMode.Solo:
                        ConfigureSoloMode(hasKeyboard, gamepads);
                        break;

                    case PlayerMode.TwoPlayer:
                        ConfigureTwoPlayerMode(hasKeyboard, gamepads);
                        break;
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Error assigning controllers: {e.Message}");
            }
        }

        private void ConfigureSoloMode(bool hasKeyboard, ReadOnlyArray<Gamepad> gamepads)
        {
            if (_playerInput2 != null)
                _playerInput2.gameObject.SetActive(false);

            if (hasKeyboard)
            {
                _playerInput1.SwitchCurrentControlScheme("Keyboard");
                Debug.Log("Solo Mode: Player 1 using Keyboard");
            }
            else if (gamepads.Count > 0)
            {
                _playerInput1.SwitchCurrentControlScheme("Gamepad", gamepads[0]);
                Debug.Log("Solo Mode: Player 1 using Gamepad");
            }
            else
            {
                Debug.LogWarning("Solo Mode: No input devices available!");
            }
        }

        private void ConfigureTwoPlayerMode(bool hasKeyboard, ReadOnlyArray<Gamepad> gamepads)
        {
            if (_playerInput2 == null) return;

            _playerInput2.gameObject.SetActive(true);

            if (gamepads.Count == 0 && hasKeyboard)
            {
                _playerInput1.SwitchCurrentControlScheme("Keyboard");
                _playerInput2.SwitchCurrentControlScheme("Keyboard2");
                Debug.Log("Two Player Mode: Both using Keyboards");
            }
            else if (gamepads.Count == 1 && hasKeyboard)
            {
                _playerInput1.SwitchCurrentControlScheme("Keyboard");
                _playerInput2.SwitchCurrentControlScheme("Gamepad", gamepads[0]);
                Debug.Log("Two Player Mode: P1 Keyboard, P2 Gamepad");
            }
            else if (gamepads.Count >= 2)
            {
                _playerInput1.SwitchCurrentControlScheme("Gamepad", gamepads[0]);
                _playerInput2.SwitchCurrentControlScheme("Gamepad", gamepads[1]);
                Debug.Log("Two Player Mode: Both using Gamepads");
            }
            else
            {
                Debug.LogWarning("Not enough input devices - falling back to Solo Mode");
                CurrentPlayerMode = PlayerMode.Solo;
                ConfigureSoloMode(hasKeyboard, gamepads);
            }
        }

        public void UpdateControllers()
        {
            if (!_initialized) return;

            try
            {
                int currentGamepadCount = Gamepad.all.Count;
                int previousGamepadCount = _playerInput1.devices.Count +
                                         (_playerInput2 != null ? _playerInput2.devices.Count : 0);

                if (Math.Abs(currentGamepadCount - previousGamepadCount) >= 1)
                {
                    AssignControllers();
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Error updating controllers: {e.Message}");
            }
        }
    }
}