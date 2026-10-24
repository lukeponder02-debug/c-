using UnityEngine;

namespace RecRoomStarter
{
    // Click to shoot paintballs. Press Q to switch paint color.
    // Goes on the player's camera.
    public class PaintballGun : MonoBehaviour
    {
        public Color[] paintColors =
        {
            new Color(1f, 0.25f, 0.3f),   // red
            new Color(0.2f, 0.6f, 1f),    // blue
            new Color(0.3f, 0.9f, 0.35f), // green
            new Color(1f, 0.85f, 0.2f),   // yellow
            new Color(0.75f, 0.4f, 1f),   // purple
        };

        public float shotSpeed = 35f;
        public float secondsBetweenShots = 0.15f;

        [Tooltip("Optional: the gun model in your hand. It changes to match your paint color.")]
        public Renderer gunModel;

        [Tooltip("So you can't shoot while you're holding something.")]
        public Grabber grabber;

        int colorIndex;
        float nextShotTime;
        Collider playerBody;

        public Color CurrentColor => paintColors[colorIndex];

        void Start()
        {
            playerBody = GetComponentInParent<CharacterController>();
            BlockColor.Apply(gunModel, CurrentColor);
        }

        void Update()
        {
            if (!PlayerController.CanAct)
                return;

            if (GameInput.SwapColorPressed)
            {
                colorIndex = (colorIndex + 1) % paintColors.Length;
                BlockColor.Apply(gunModel, CurrentColor);
            }

            if (grabber != null && grabber.UsedClickThisFrame)
                return;

            if (GameInput.ShootPressed && Time.time >= nextShotTime)
            {
                Shoot();
                nextShotTime = Time.time + secondsBetweenShots;
            }
        }

        void Shoot()
        {
            GameObject ball = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            ball.name = "Paintball";
            ball.transform.position = transform.position + transform.forward * 0.6f - transform.up * 0.1f;
            ball.transform.localScale = Vector3.one * 0.12f;
            BlockColor.Apply(ball.GetComponent<Renderer>(), CurrentColor);

            // Don't let the paintball bump into the player who shot it.
            if (playerBody != null)
                Physics.IgnoreCollision(ball.GetComponent<Collider>(), playerBody);

            Rigidbody body = ball.AddComponent<Rigidbody>();
            body.mass = 0.05f;
            body.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            PhysicsCompat.SetVelocity(body, transform.forward * shotSpeed);

            Paintball paintball = ball.AddComponent<Paintball>();
            paintball.color = CurrentColor;
        }
    }
}
