using UnityEngine;

namespace RecRoomStarter
{
    // Look at a Grabbable object and press E to pick it up.
    // Press E again to drop it, or click to throw it.
    // Goes on the player's camera.
    public class Grabber : MonoBehaviour
    {
        public float reach = 3f;
        public float holdDistance = 1.8f;
        public float throwSpeed = 14f;

        Rigidbody held;
        int releasedOnFrame = -1;

        public bool IsHolding => held != null;

        // The paintball gun checks this so throwing an object doesn't also fire a shot.
        public bool UsedClickThisFrame => held != null || releasedOnFrame == Time.frameCount;

        void Update()
        {
            if (!PlayerController.CanAct)
                return;

            if (GameInput.GrabPressed)
            {
                if (held != null) Drop();
                else TryGrab();
            }
            else if (held != null && GameInput.ShootPressed)
            {
                Throw();
            }
        }

        void FixedUpdate()
        {
            if (held == null)
                return;

            // Pull the object toward a point in front of the camera.
            Vector3 holdPoint = transform.position + transform.forward * holdDistance;
            Vector3 toHoldPoint = holdPoint - held.position;

            // If it got stuck behind a wall, let go.
            if (toHoldPoint.magnitude > reach * 2f)
            {
                Drop();
                return;
            }

            Vector3 velocity = Vector3.ClampMagnitude(toHoldPoint / Time.fixedDeltaTime * 0.5f, 20f);
            PhysicsCompat.SetVelocity(held, velocity);
            held.angularVelocity *= 0.8f;
        }

        void TryGrab()
        {
            if (!Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, reach, ~0, QueryTriggerInteraction.Ignore))
                return;

            Rigidbody body = hit.rigidbody;
            if (body == null || body.GetComponent<Grabbable>() == null)
                return;

            held = body;
            held.useGravity = false;
        }

        void Drop()
        {
            held.useGravity = true;
            held = null;
            releasedOnFrame = Time.frameCount;
        }

        void Throw()
        {
            Rigidbody thrown = held;
            Drop();
            PhysicsCompat.SetVelocity(thrown, transform.forward * throwSpeed);
        }
    }
}
