# c-
Coding rec room

**No Unity? Play the browser version:** open `web/index.html` in any web browser
(Chrome, Safari, Edge…). It works on computers, on phones and tablets with touch controls,
and in VR headsets like the Meta Quest.

### Playing in VR (Meta Quest)

1. Turn on GitHub Pages for this repo: **Settings → Pages → Build and deployment → Source:
   Deploy from a branch**, pick the branch with the game and the **/ (root)** folder, and save.
2. After a minute the game is online at `https://lukeponder02-debug.github.io/c-/web/`.
3. Open that link in the headset's **Browser** app and press **Play in VR**.

VR controls: left stick walks, right stick turns, trigger shoots, grip grabs, **A** jumps,
**B** changes paint color. Score and time show on the board above the targets.

### Bonebreak (second game)

A wave-survival shooter with broken bones and gore, in `web/bonebreak.html`.
In VR it's at `https://lukeponder02-debug.github.io/c-/web/bonebreak/` — press **Deploy in VR**.
VR controls: left stick moves (click to sprint), right stick turns, trigger shoots, grip reloads,
**A** jumps, **B** swaps rifle/shotgun. Health, ammo and score are on a watch on your left wrist.

**Sandbox mode:** pick *Sandbox* on the main menu to get a spawn gun (key **3**). Press **E** for
the spawn menu (soldier, heavy, dummy, crate, explosive barrel), hold fire to see a glowing preview
where it will appear, and let go to spawn it. **R** removes whatever you're aiming at. The menu also
has God mode, a switch for whether soldiers shoot back, and *Clear everything*. In VR, grip picks
the next thing to spawn.

After changing a game, run `python3 web/build_standalone.py` to update the standalone pages.

A Rec Room-style starter game made in **Unity** with **C#**. You get a room with a paintball gun,
targets to shoot, and toys you can pick up and throw.

## What you can do in the game

| Key | Action |
| --- | --- |
| W A S D | Walk |
| Mouse | Look around |
| Space | Jump |
| Shift | Sprint |
| Left click | Shoot a paintball (or throw what you're holding) |
| E | Pick up / drop a cube or ball |
| Q | Change paint color |
| Esc | Get your mouse back (click the game to keep playing) |

Hit the targets on the far wall to score points. Two of them move!

## Getting it running (first time)

You only have to do this once.

1. **Install Unity.** Download [Unity Hub](https://unity.com/download), open it, and install a
   Unity Editor version (any **2022.3 LTS** or **Unity 6** version works).
2. **Make a new project.** In Unity Hub click **New project**, pick the **3D** template
   (either "3D (Built-In)" or "Universal 3D" is fine), give it a name, and click **Create project**.
3. **Copy the game in.** Download this repository (green **Code** button → **Download ZIP** on GitHub)
   and unzip it. Copy the `Assets/RecRoomStarter` folder into your new project's `Assets` folder.
   You can drag it straight into the **Project** window at the bottom of Unity.
4. **Build the room.** Wait for Unity to finish loading (the spinner in the bottom right stops).
   A new menu called **Rec Room Starter** appears at the top of the editor.
   Click **Rec Room Starter → Build Demo Room**.
5. **Play!** Press the **▶ Play** button at the top middle of the editor, then click inside the
   game view. Press Play again to stop.

## How the code is organized

Everything lives in `Assets/RecRoomStarter`. Each script does one job, and each one starts with a
comment explaining what it does.

| File | What it does |
| --- | --- |
| `Scripts/PlayerController.cs` | Walking, looking, jumping, and respawning if you fall off |
| `Scripts/GameInput.cs` | Reads the keyboard and mouse |
| `Scripts/Grabber.cs` | Picking up, dropping and throwing objects |
| `Scripts/Grabbable.cs` | Put this on an object to make it something you can pick up |
| `Scripts/PaintballGun.cs` | Shoots paintballs and switches colors |
| `Scripts/Paintball.cs` | A flying paintball: makes splats and hits targets |
| `Scripts/Target.cs` | A target that pops when hit and comes back |
| `Scripts/Score.cs` | Keeps track of your points |
| `Scripts/GameHUD.cs` | Draws the crosshair, score and controls on screen |
| `Scripts/BlockColor.cs` | Gives an object a color |
| `Scripts/PhysicsCompat.cs` | Small helper so the code works in old and new Unity |
| `Editor/RecRoomStarterBuilder.cs` | The **Build Demo Room** menu that builds the whole scene |

## Easy things to try changing

You don't need to write any code for these. Click an object in the **Hierarchy** (left side),
then change its values in the **Inspector** (right side).

- **Jump higher or run faster:** click **Player** and change *Jump Height* or *Sprint Speed*
  on the Player Controller.
- **Make the paintballs faster:** click **Player → Camera** and change *Shot Speed* on the
  Paintball Gun.
- **Make a target worth more:** click a **Target** and change *Points*.
- **Make a target move:** click a **Target** and set *Move Amount* (try X = 3).
- **Recolor anything:** click it and change the color on its Block Color component.
- **Add a new toy:** right-click in the Hierarchy → **3D Object → Cube**, then in the Inspector
  click **Add Component** and add **Rigidbody** and **Grabbable**.

Changes made while the game is playing get undone when you stop it, so stop the game before
editing anything you want to keep. Save with **Ctrl+S** (Cmd+S on Mac).
