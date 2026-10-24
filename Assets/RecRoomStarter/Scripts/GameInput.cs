using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace RecRoomStarter
{
    // One place that answers "is the player pressing X right now?"
    // Unity has two input systems (old and new). This works with either one,
    // so the rest of the game never has to care which one your project uses.
    public static class GameInput
    {
#if ENABLE_INPUT_SYSTEM
        static Keyboard Keys => Keyboard.current;
        static Mouse MouseDevice => Mouse.current;

        public static Vector2 Move
        {
            get
            {
                if (Keys == null) return Vector2.zero;
                float x = (Keys.dKey.isPressed ? 1f : 0f) - (Keys.aKey.isPressed ? 1f : 0f);
                float y = (Keys.wKey.isPressed ? 1f : 0f) - (Keys.sKey.isPressed ? 1f : 0f);
                return new Vector2(x, y);
            }
        }

        // Scaled to roughly match the old input system's mouse speed.
        public static Vector2 Look => MouseDevice == null ? Vector2.zero : MouseDevice.delta.ReadValue() * 0.1f;

        public static bool JumpPressed => Keys != null && Keys.spaceKey.wasPressedThisFrame;
        public static bool SprintHeld => Keys != null && Keys.leftShiftKey.isPressed;
        public static bool GrabPressed => Keys != null && Keys.eKey.wasPressedThisFrame;
        public static bool SwapColorPressed => Keys != null && Keys.qKey.wasPressedThisFrame;
        public static bool UnlockPressed => Keys != null && Keys.escapeKey.wasPressedThisFrame;
        public static bool ShootPressed => MouseDevice != null && MouseDevice.leftButton.wasPressedThisFrame;
#else
        public static Vector2 Move => new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        public static Vector2 Look => new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        public static bool JumpPressed => Input.GetKeyDown(KeyCode.Space);
        public static bool SprintHeld => Input.GetKey(KeyCode.LeftShift);
        public static bool GrabPressed => Input.GetKeyDown(KeyCode.E);
        public static bool SwapColorPressed => Input.GetKeyDown(KeyCode.Q);
        public static bool UnlockPressed => Input.GetKeyDown(KeyCode.Escape);
        public static bool ShootPressed => Input.GetMouseButtonDown(0);
#endif
    }
}
