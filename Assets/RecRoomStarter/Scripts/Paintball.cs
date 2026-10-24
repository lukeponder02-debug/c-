using System.Collections.Generic;
using UnityEngine;

namespace RecRoomStarter
{
    // A flying paintball. When it hits something it leaves a splat,
    // tells a Target it got hit, and disappears.
    public class Paintball : MonoBehaviour
    {
        public Color color = Color.red;

        // Old splats get cleaned up so the game doesn't slow down.
        const int MaxSplats = 200;
        static readonly Queue<GameObject> splats = new Queue<GameObject>();

        void Start()
        {
            Destroy(gameObject, 5f); // in case it flies off into the sky
        }

        void OnCollisionEnter(Collision collision)
        {
            Target target = collision.collider.GetComponentInParent<Target>();
            if (target != null)
                target.Hit();
            else if (collision.contactCount > 0)
                MakeSplat(collision.GetContact(0), collision.rigidbody);

            Destroy(gameObject);
        }

        void MakeSplat(ContactPoint contact, Rigidbody hitBody)
        {
            GameObject splat = GameObject.CreatePrimitive(PrimitiveType.Quad);
            splat.name = "Splat";
            Destroy(splat.GetComponent<Collider>());

            // Lay the splat flat against the surface, just in front of it, with a random spin.
            splat.transform.position = contact.point + contact.normal * 0.01f;
            splat.transform.rotation = Quaternion.FromToRotation(Vector3.back, contact.normal) * Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));
            splat.transform.localScale = Vector3.one * Random.Range(0.3f, 0.5f);

            Renderer splatRenderer = splat.GetComponent<Renderer>();
            splatRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            BlockColor.Apply(splatRenderer, color);

            // Stick to objects that move, like a cube you can throw.
            if (hitBody != null)
                splat.transform.SetParent(hitBody.transform, true);

            splats.Enqueue(splat);
            while (splats.Count > MaxSplats)
            {
                GameObject oldest = splats.Dequeue();
                if (oldest != null)
                    Destroy(oldest);
            }
        }
    }
}
