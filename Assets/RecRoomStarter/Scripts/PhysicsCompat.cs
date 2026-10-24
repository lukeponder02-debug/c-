using UnityEngine;

namespace RecRoomStarter
{
    // Unity 6 renamed Rigidbody.velocity to Rigidbody.linearVelocity.
    // This helper picks the right one so the game works in older and newer Unity.
    public static class PhysicsCompat
    {
        public static void SetVelocity(Rigidbody body, Vector3 velocity)
        {
#if UNITY_6000_0_OR_NEWER
            body.linearVelocity = velocity;
#else
            body.velocity = velocity;
#endif
        }
    }
}
