using UnityEngine;

namespace RecRoomStarter
{
    // Draws the crosshair, score, paint color and controls on screen.
    public class GameHUD : MonoBehaviour
    {
        public PaintballGun gun;

        GUIStyle bigText;
        GUIStyle smallText;

        void OnEnable()
        {
            Score.Reset();
        }

        void OnGUI()
        {
            if (bigText == null)
            {
                bigText = new GUIStyle(GUI.skin.label) { fontSize = 28, fontStyle = FontStyle.Bold };
                smallText = new GUIStyle(GUI.skin.label) { fontSize = 16 };
            }

            DrawCrosshair();

            GUI.Label(new Rect(20, 15, 400, 40), "Score: " + Score.Points, bigText);

            if (gun != null)
            {
                GUI.Label(new Rect(20, 55, 100, 30), "Paint:", smallText);
                Color old = GUI.color;
                GUI.color = gun.CurrentColor;
                GUI.DrawTexture(new Rect(75, 58, 24, 24), Texture2D.whiteTexture);
                GUI.color = old;
            }

            string controls = "WASD move   Mouse look   Space jump   Shift sprint\n" +
                              "Click shoot / throw   E grab / drop   Q change color   Esc free mouse";
            GUI.Label(new Rect(20, Screen.height - 60, 800, 50), controls, smallText);

            if (Cursor.lockState != CursorLockMode.Locked)
            {
                var center = new GUIStyle(bigText) { alignment = TextAnchor.MiddleCenter };
                GUI.Label(new Rect(0, Screen.height / 2f - 80, Screen.width, 40), "Click to play", center);
            }
        }

        void DrawCrosshair()
        {
            const float size = 6f;
            Color old = GUI.color;
            GUI.color = Color.white;
            GUI.DrawTexture(new Rect((Screen.width - size) / 2f, (Screen.height - size) / 2f, size, size), Texture2D.whiteTexture);
            GUI.color = old;
        }
    }
}
