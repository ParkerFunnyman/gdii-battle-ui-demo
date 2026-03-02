using System;
using UnityEngine;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
#endif

namespace CharlesEngine
{
    public class GameInput : MonoBehaviour
    {
        public InputMapping GameInputMapping;
        private readonly GestureDetector _gestureDetector = new GestureDetector();

        public bool GetKeyDown(InputAction action)
        {
            var code = GameInputMapping != null ? GameInputMapping[action] : KeyCode.None;
            if (code == KeyCode.None) return false;

#if ENABLE_LEGACY_INPUT_MANAGER
            return Input.GetKeyDown(code);
#elif ENABLE_INPUT_SYSTEM
            return GetDown_NewInputSystem(code);
#else
            return false;
#endif
        }

        public bool GetKeyUp(InputAction action)
        {
#if UNITY_IOS || UNITY_ANDROID
            if (action == InputAction.PauseGame && _gestureDetector.EscapeMenuGesture) return true;
            if (action == InputAction.SkipVideo && _gestureDetector.SkipVidGesture) return true;
#endif

            var code = GameInputMapping != null ? GameInputMapping[action] : KeyCode.None;
            if (code == KeyCode.None) return false;

#if ENABLE_LEGACY_INPUT_MANAGER
            return Input.GetKeyUp(code);
#elif ENABLE_INPUT_SYSTEM
            return GetUp_NewInputSystem(code);
#else
            return false;
#endif
        }

        private void Update()
        {
            _gestureDetector.Update();
        }

#if ENABLE_INPUT_SYSTEM
        private static bool GetDown_NewInputSystem(KeyCode code)
        {
            // Mouse buttons
            if (TryGetMouseButton(code, out var btn))
                return btn.wasPressedThisFrame;

            // Gamepad buttons (KeyCode.JoystickButtonX)
            if (TryGetGamepadButton(code, out var gpBtn))
                return gpBtn.wasPressedThisFrame;

            // Keyboard
            var key = KeyCodeToInputSystemKey(code);
            if (key == Key.None) return false;

            var kb = Keyboard.current;
            return kb != null && kb[key].wasPressedThisFrame;
        }

        private static bool GetUp_NewInputSystem(KeyCode code)
        {
            if (TryGetMouseButton(code, out var btn))
                return btn.wasReleasedThisFrame;

            if (TryGetGamepadButton(code, out var gpBtn))
                return gpBtn.wasReleasedThisFrame;

            var key = KeyCodeToInputSystemKey(code);
            if (key == Key.None) return false;

            var kb = Keyboard.current;
            return kb != null && kb[key].wasReleasedThisFrame;
        }

        private static bool TryGetMouseButton(KeyCode code, out ButtonControl button)
        {
            button = null;
            var mouse = Mouse.current;
            if (mouse == null) return false;

            switch (code)
            {
                case KeyCode.Mouse0: button = mouse.leftButton; return true;
                case KeyCode.Mouse1: button = mouse.rightButton; return true;
                case KeyCode.Mouse2: button = mouse.middleButton; return true;
                case KeyCode.Mouse3: button = mouse.backButton; return true;
                case KeyCode.Mouse4: button = mouse.forwardButton; return true;
                default: return false;
            }
        }

        private static bool TryGetGamepadButton(KeyCode code, out ButtonControl button)
        {
            button = null;

            // KeyCode.JoystickButton0..19 are common legacy-style mappings
            // We'll map a subset to Gamepad.current in a reasonable way.
            var gp = Gamepad.current;
            if (gp == null) return false;

            // Only handle joystick buttons; axes won't work via KeyCode anyway.
            var name = code.ToString(); // e.g. "JoystickButton0"
            if (!name.StartsWith("JoystickButton", StringComparison.Ordinal)) return false;

            if (!int.TryParse(name.Substring("JoystickButton".Length), out var idx))
                return false;

            switch (idx)
            {
                case 0: button = gp.buttonSouth; return true;   // A / Cross
                case 1: button = gp.buttonEast;  return true;   // B / Circle
                case 2: button = gp.buttonWest;  return true;   // X / Square
                case 3: button = gp.buttonNorth; return true;   // Y / Triangle

                case 4: button = gp.leftShoulder;  return true; // LB
                case 5: button = gp.rightShoulder; return true; // RB

                case 6: button = gp.selectButton; return true;  // Back
                case 7: button = gp.startButton;  return true;  // Start

                case 8: button = gp.leftStickButton;  return true; // L3
                case 9: button = gp.rightStickButton; return true; // R3

                // Dpad often comes in as "buttons" in legacy, but in the new system it's a Dpad control:
                case 10: button = gp.dpad.up;    return true;
                case 11: button = gp.dpad.down;  return true;
                case 12: button = gp.dpad.left;  return true;
                case 13: button = gp.dpad.right; return true;

                default:
                    return false;
            }
        }

        private static Key KeyCodeToInputSystemKey(KeyCode kc)
        {
            // Fix common name mismatches between KeyCode and InputSystem.Key
            switch (kc)
            {
                case KeyCode.Return: return Key.Enter;
                case KeyCode.KeypadEnter: return Key.NumpadEnter;

                case KeyCode.LeftControl: return Key.LeftCtrl;
                case KeyCode.RightControl: return Key.RightCtrl;

                case KeyCode.LeftAlt: return Key.LeftAlt;
                case KeyCode.RightAlt: return Key.RightAlt;

                case KeyCode.Alpha0: return Key.Digit0;
                case KeyCode.Alpha1: return Key.Digit1;
                case KeyCode.Alpha2: return Key.Digit2;
                case KeyCode.Alpha3: return Key.Digit3;
                case KeyCode.Alpha4: return Key.Digit4;
                case KeyCode.Alpha5: return Key.Digit5;
                case KeyCode.Alpha6: return Key.Digit6;
                case KeyCode.Alpha7: return Key.Digit7;
                case KeyCode.Alpha8: return Key.Digit8;
                case KeyCode.Alpha9: return Key.Digit9;
            }

            // Most keys match by name: A, Space, Escape, F1, UpArrow, etc.
            if (Enum.TryParse(kc.ToString(), out Key parsed))
                return parsed;

            return Key.None;
        }
#endif
    }
}
