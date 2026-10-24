using UnityEngine;

namespace RecRoomStarter
{
    // Walk with WASD, look with the mouse, jump with Space, sprint with Shift.
    // Press Esc to get your mouse back, click the game to start playing again.
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour
    {
        [Header("Movement")]
        public float walkSpeed = 5f;
        public float sprintSpeed = 8f;
        public float jumpHeight = 1.4f;
        public float gravity = -20f;
        public float pushPower = 2f;

        [Header("Looking")]
        public Transform cameraHolder;
        public float lookSensitivity = 2f;

        [Header("Falling off the map")]
        public float respawnBelowY = -20f;

        // True when the player is in control (mouse captured by the game).
        // Other scripts check this so a click that captures the mouse doesn't also fire a paintball.
        public static bool CanAct => Cursor.lockState == CursorLockMode.Locked && Time.frameCount != lockedOnFrame;
        static int lockedOnFrame = -1;

        CharacterController controller;
        Vector3 spawnPoint;
        float verticalVelocity;
        float pitch;

        void Start()
        {
            controller = GetComponent<CharacterController>();
            spawnPoint = transform.position;
            SetCursorLocked(true);
        }

        void Update()
        {
            if (GameInput.UnlockPressed)
                SetCursorLocked(false);
            else if (Cursor.lockState != CursorLockMode.Locked && GameInput.ShootPressed)
                SetCursorLocked(true);

            bool playing = Cursor.lockState == CursorLockMode.Locked;

            if (playing)
                Look();

            Move(playing);

            if (transform.position.y < respawnBelowY)
                Respawn();
        }

        void Look()
        {
            Vector2 look = GameInput.Look * lookSensitivity;

            // Turn the whole body left/right...
            transform.Rotate(0f, look.x, 0f);

            // ...but only tilt the camera up/down, and stop before it flips over.
            pitch = Mathf.Clamp(pitch - look.y, -85f, 85f);
            if (cameraHolder != null)
                cameraHolder.localEulerAngles = new Vector3(pitch, 0f, 0f);
        }

        void Move(bool playing)
        {
            Vector2 input = playing ? GameInput.Move : Vector2.zero;
            float speed = playing && GameInput.SprintHeld ? sprintSpeed : walkSpeed;

            Vector3 move = transform.right * input.x + transform.forward * input.y;
            move = Vector3.ClampMagnitude(move, 1f) * speed;

            if (controller.isGrounded && verticalVelocity < 0f)
                verticalVelocity = -2f; // keeps us pressed onto the floor

            if (playing && controller.isGrounded && GameInput.JumpPressed)
                verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);

            verticalVelocity += gravity * Time.deltaTime;
            move.y = verticalVelocity;

            controller.Move(move * Time.deltaTime);
        }

        void Respawn()
        {
            // A CharacterController ignores teleports while it's on, so switch it off for a moment.
            controller.enabled = false;
            transform.position = spawnPoint;
            controller.enabled = true;
            verticalVelocity = 0f;
        }

        // Lets you shove loose objects around by walking into them.
        void OnControllerColliderHit(ControllerColliderHit hit)
        {
            Rigidbody body = hit.collider.attachedRigidbody;
            if (body == null || body.isKinematic || hit.moveDirection.y < -0.3f)
                return;

            Vector3 pushDirection = new Vector3(hit.moveDirection.x, 0f, hit.moveDirection.z);
            PhysicsCompat.SetVelocity(body, pushDirection * pushPower);
        }

        static void SetCursorLocked(bool locked)
        {
            Cursor.lockState = locked ? CursorLockMode.Locked : CursorLockMode.None;
            Cursor.visible = !locked;
            if (locked)
                lockedOnFrame = Time.frameCount;
        }
    }
}
