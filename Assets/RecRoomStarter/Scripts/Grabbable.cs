using UnityEngine;

namespace RecRoomStarter
{
    // Put this on any object (that also has a Rigidbody) to let the player pick it up.
    [RequireComponent(typeof(Rigidbody))]
    public class Grabbable : MonoBehaviour
    {
    }
}
