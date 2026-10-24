using UnityEngine;

namespace RecRoomStarter
{
    // Gives an object a solid color without having to make a material by hand.
    // Change the color in the Inspector and you'll see it update right away.
    [ExecuteAlways]
    public class BlockColor : MonoBehaviour
    {
        public Color color = Color.white;

        void OnEnable() => Apply(GetComponent<Renderer>(), color);
        void OnValidate() => Apply(GetComponent<Renderer>(), color);

        public static void Apply(Renderer renderer, Color color)
        {
            if (renderer == null)
                return;

            var block = new MaterialPropertyBlock();
            renderer.GetPropertyBlock(block);
            block.SetColor("_BaseColor", color); // URP / HDRP
            block.SetColor("_Color", color);     // Built-in render pipeline
            renderer.SetPropertyBlock(block);
        }
    }
}
