using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RecRoomStarter
{
    // Adds a "Rec Room Starter" menu to the top of the Unity editor.
    // Clicking "Build Demo Room" makes a complete, playable scene for you.
    public static class RecRoomStarterBuilder
    {
        const string SceneFolder = "Assets/Scenes";
        const string ScenePath = SceneFolder + "/RecRoomStarter.unity";

        static readonly Color FloorColor = new Color(0.55f, 0.75f, 0.95f);
        static readonly Color WallColor = new Color(0.95f, 0.93f, 0.88f);
        static readonly Color RampColor = new Color(1f, 0.6f, 0.3f);
        static readonly Color PlatformColor = new Color(0.45f, 0.85f, 0.6f);

        static readonly Color[] ToyColors =
        {
            new Color(1f, 0.35f, 0.35f),
            new Color(0.35f, 0.65f, 1f),
            new Color(1f, 0.85f, 0.3f),
            new Color(0.65f, 0.45f, 1f),
            new Color(0.4f, 0.9f, 0.5f),
        };

        [MenuItem("Rec Room Starter/Build Demo Room")]
        public static void BuildDemoRoom()
        {
            if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                return;

            Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

            BuildLight();
            BuildRoom();
            BuildToys();
            BuildTargets();
            BuildPlayer();

            if (!AssetDatabase.IsValidFolder(SceneFolder))
                AssetDatabase.CreateFolder("Assets", "Scenes");
            EditorSceneManager.SaveScene(scene, ScenePath);
            AddSceneToBuildSettings();

            EditorUtility.DisplayDialog(
                "Demo room built!",
                "Your room is saved at " + ScenePath + ".\n\nPress the Play button (top middle of the editor) to try it.",
                "Let's go");
        }

        static void BuildLight()
        {
            var sun = new GameObject("Sun");
            var light = sun.AddComponent<Light>();
            light.type = LightType.Directional;
            light.intensity = 1.2f;
            light.shadows = LightShadows.Soft;
            sun.transform.rotation = Quaternion.Euler(50f, -30f, 0f);
        }

        static void BuildRoom()
        {
            var room = new GameObject("Room").transform;

            Block("Floor", PrimitiveType.Cube, new Vector3(0f, -0.5f, 0f), new Vector3(30f, 1f, 30f), FloorColor, room);

            Block("Wall North", PrimitiveType.Cube, new Vector3(0f, 2.5f, 15.5f), new Vector3(32f, 6f, 1f), WallColor, room);
            Block("Wall South", PrimitiveType.Cube, new Vector3(0f, 2.5f, -15.5f), new Vector3(32f, 6f, 1f), WallColor, room);
            Block("Wall East", PrimitiveType.Cube, new Vector3(15.5f, 2.5f, 0f), new Vector3(1f, 6f, 30f), WallColor, room);
            Block("Wall West", PrimitiveType.Cube, new Vector3(-15.5f, 2.5f, 0f), new Vector3(1f, 6f, 30f), WallColor, room);

            // A ramp up to a high platform, plus a couple of boxes to hop between.
            var ramp = Block("Ramp", PrimitiveType.Cube, new Vector3(-9f, 0.85f, 2f), new Vector3(3f, 0.4f, 7f), RampColor, room);
            ramp.transform.rotation = Quaternion.Euler(-18f, 0f, 0f);
            Block("High Platform", PrimitiveType.Cube, new Vector3(-9f, 1.75f, 8f), new Vector3(5f, 0.5f, 5f), PlatformColor, room);
            Block("Hop Box 1", PrimitiveType.Cube, new Vector3(-4f, 0.5f, 9f), new Vector3(2f, 1f, 2f), PlatformColor, room);
            Block("Hop Box 2", PrimitiveType.Cube, new Vector3(0f, 1f, 9f), new Vector3(2f, 2f, 2f), PlatformColor, room);

            // A table to stack things on.
            Block("Table", PrimitiveType.Cube, new Vector3(6f, 0.5f, -6f), new Vector3(4f, 1f, 2f), RampColor, room);
        }

        static void BuildToys()
        {
            var toys = new GameObject("Toys").transform;

            // Stackable cubes on the table.
            for (int i = 0; i < 6; i++)
            {
                var position = new Vector3(4.6f + (i % 3) * 1.2f, 1.3f + (i / 3) * 0.6f, -6f);
                Toy("Cube", PrimitiveType.Cube, position, 0.5f, ToyColors[i % ToyColors.Length], toys);
            }

            // Balls scattered around the floor.
            for (int i = 0; i < 5; i++)
            {
                var position = new Vector3(-3f + i * 1.5f, 0.5f, -4f);
                Toy("Ball", PrimitiveType.Sphere, position, 0.6f, ToyColors[i], toys);
            }
        }

        static void BuildTargets()
        {
            var targets = new GameObject("Targets").transform;

            // A row of targets on the far wall, two of which slide back and forth.
            MakeTarget(new Vector3(-6f, 2f, 14.9f), Vector3.zero, targets);
            MakeTarget(new Vector3(-2f, 3.5f, 14.9f), new Vector3(2f, 0f, 0f), targets);
            MakeTarget(new Vector3(2f, 2f, 14.9f), Vector3.zero, targets);
            MakeTarget(new Vector3(6f, 3.5f, 14.9f), new Vector3(0f, 1f, 0f), targets);
            MakeTarget(new Vector3(10f, 2f, 14.9f), Vector3.zero, targets);
        }

        static void BuildPlayer()
        {
            var player = new GameObject("Player");
            player.transform.position = new Vector3(0f, 0.1f, -10f);

            var body = player.AddComponent<CharacterController>();
            body.height = 1.8f;
            body.radius = 0.35f;
            body.center = new Vector3(0f, 0.9f, 0f);

            var cameraObject = new GameObject("Camera");
            cameraObject.tag = "MainCamera";
            cameraObject.transform.SetParent(player.transform, false);
            cameraObject.transform.localPosition = new Vector3(0f, 1.6f, 0f);
            cameraObject.AddComponent<Camera>().nearClipPlane = 0.05f;
            cameraObject.AddComponent<AudioListener>();

            // A simple "paintball gun" shape that sits in your view, bottom right.
            var gunModel = GameObject.CreatePrimitive(PrimitiveType.Cube);
            gunModel.name = "Gun Model";
            Object.DestroyImmediate(gunModel.GetComponent<Collider>());
            gunModel.transform.SetParent(cameraObject.transform, false);
            gunModel.transform.localPosition = new Vector3(0.3f, -0.25f, 0.5f);
            gunModel.transform.localScale = new Vector3(0.08f, 0.1f, 0.35f);

            var controller = player.AddComponent<PlayerController>();
            controller.cameraHolder = cameraObject.transform;

            var grabber = cameraObject.AddComponent<Grabber>();

            var gun = cameraObject.AddComponent<PaintballGun>();
            gun.grabber = grabber;
            gun.gunModel = gunModel.GetComponent<Renderer>();

            var hud = player.AddComponent<GameHUD>();
            hud.gun = gun;
        }

        static GameObject Block(string name, PrimitiveType shape, Vector3 position, Vector3 scale, Color color, Transform parent)
        {
            var block = GameObject.CreatePrimitive(shape);
            block.name = name;
            block.transform.SetParent(parent, false);
            block.transform.position = position;
            block.transform.localScale = scale;
            block.AddComponent<BlockColor>().color = color;
            return block;
        }

        static GameObject Toy(string name, PrimitiveType shape, Vector3 position, float size, Color color, Transform parent)
        {
            var toy = Block(name, shape, position, Vector3.one * size, color, parent);
            toy.AddComponent<Rigidbody>().mass = 0.5f;
            toy.AddComponent<Grabbable>();
            return toy;
        }

        static void MakeTarget(Vector3 position, Vector3 moveAmount, Transform parent)
        {
            var root = new GameObject("Target");
            root.transform.SetParent(parent, false);
            root.transform.position = position;

            var target = root.AddComponent<Target>();
            target.moveAmount = moveAmount;

            // Cylinders stand up by default, so tip them over to face the player.
            var tilt = Quaternion.Euler(90f, 0f, 0f);

            var outer = Block("Outer Ring", PrimitiveType.Cylinder, position, new Vector3(1.4f, 0.05f, 1.4f), new Color(1f, 0.3f, 0.3f), root.transform);
            outer.transform.rotation = tilt;

            var inner = Block("Bullseye", PrimitiveType.Cylinder, position - new Vector3(0f, 0f, 0.03f), new Vector3(0.6f, 0.05f, 0.6f), Color.white, root.transform);
            inner.transform.rotation = tilt;
        }

        static void AddSceneToBuildSettings()
        {
            var scenes = EditorBuildSettings.scenes.ToList();
            if (scenes.Any(s => s.path == ScenePath))
                return;

            scenes.Insert(0, new EditorBuildSettingsScene(ScenePath, true));
            EditorBuildSettings.scenes = scenes.ToArray();
        }
    }
}
