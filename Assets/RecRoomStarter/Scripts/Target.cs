using UnityEngine;

namespace RecRoomStarter
{
    // Hit it with a paintball to score points. It pops, then comes back a moment later.
    // Set "Move Amount" to make it slide back and forth.
    public class Target : MonoBehaviour
    {
        public int points = 1;
        public float secondsUntilRespawn = 2f;

        [Header("Optional movement")]
        public Vector3 moveAmount = Vector3.zero;
        public float moveSpeed = 1f;

        Vector3 startPosition;
        bool isUp = true;

        void Start()
        {
            startPosition = transform.position;
        }

        void Update()
        {
            if (moveAmount != Vector3.zero)
                transform.position = startPosition + moveAmount * Mathf.Sin(Time.time * moveSpeed);
        }

        public void Hit()
        {
            if (!isUp)
                return;

            Score.Add(points);
            SetUp(false);
            Invoke(nameof(PopBackUp), secondsUntilRespawn);
        }

        void PopBackUp() => SetUp(true);

        void SetUp(bool up)
        {
            isUp = up;
            foreach (Renderer part in GetComponentsInChildren<Renderer>())
                part.enabled = up;
            foreach (Collider part in GetComponentsInChildren<Collider>())
                part.enabled = up;
        }
    }
}
