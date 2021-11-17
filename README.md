# Easy Offset

Beat Saber mod for intuitive controller offset adjustments.

Tired from a pile of magic config numbers? Find the best offset for your grip just by moving your hand!

- [How to use](https://github.com/Reezonate/EasyOffset#how-to-use)
- [Config migration](https://github.com/Reezonate/EasyOffset#config-migration)
- [Support the project](https://ko-fi.com/reezonate)

# Compatibility

- Not compatible with other offset adjustment mods (e.g. SaberTailor, ControllerSettingsHelper, etc.)
- Config values are different from the base game settings, SaberTailor, etc.
- **PC ONLY**! Quest version is planned, but not guaranteed
- Best used with [CustomMenuPointers](https://github.com/dawnvt/CustomMenuPointers)

# How to install
- Install **BSIPA** and **BeatSaberMarkupLanguage** dependencies using
  [ModAssistant](https://github.com/Assistant/ModAssistant)

- Download the latest .dll for your game version from the
  [Releases page](https://github.com/Reezonate/EasyOffset/releases)
  and put it in `/plugins` directory inside your game folder

# Config migration

### From the base game settings

- Go to `Settings` > `Mod Settings` > `Easy Offset`
- Use `Z Offset` slider to change the `Pivot Point` position (0 - top of the hilt, 17 - bottom of the hilt)
- Press the `Import from settings` button

### From SaberTailor mod

```diff
- SaberTailor.json file is required in the UserData directory
- World offset is not supported!
```

- Go to `Settings` > `Mod Settings` > `Easy Offset`
- Use `Z Offset` slider to change the `Pivot Point` position (0 - top of the hilt, 17 - bottom of the hilt)
- Press the `Import from SaberTailor` button
- Make sure to disable SaberTailor mod to avoid interference

# How to use
Before trying to find your best config, spend some time in different modes and experiment with the tools at your disposal

### Config values:

- `Pivot point` - saber origin position relative to the controller (Displayed as a white dot surrounded by a grid)
- `Direction vector` - saber tip position relative to `Pivot point`
- `ZOffset` - saber position offset along `Direction vector`

### First steps:
- Select your controller model in the `Controller Type` list
- Choose a button you can press without changing your grip
- If there is no such button, check the `Use Free Hand` toggle to use the button on the other hand

### To get a decent config, you have to achieve two goals:

- `Pivot point` should be stable as the wrist rotates. In other words - it should be aligned with your actual wrist pivot point as much as possible
- Saber trail shouldn't move in a huge circle while you pointing forward and rolling your wrist comfortably. The smaller the radius, the better

To help you achieve these goals, there are four adjustment modes:

- [Basic](https://github.com/Reezonate/EasyOffset#basic-adjustment-mode)
- [Position](https://github.com/Reezonate/EasyOffset#position-adjustment-mode)
- [Rotation](https://github.com/Reezonate/EasyOffset#rotation-adjustment-mode)
- [Swing Benchmark](https://github.com/Reezonate/EasyOffset#swing-benchmark-adjustment-mode)

## `Basic` adjustment mode:
``` diff
+ Easy: Suitable for any skill level
```
Simple drag and drop adjustment mode

- Select `Basic` in the `Adjustment Mode` list
- While holding the selected button, move your hand and pick up the saber in a new position by releasing the button
- You can move the saber along its axis using the `ZOffset` slider

![Basic mode image](media/Basic.png)

## `Position` adjustment mode:
``` diff
+ Easy: Suitable for any skill level
```
Position only adjustment mode. Displays `Pivot point` coordinates in centimeters

Allows you to change the `Pivot point` without changing the `Direction vector`

- Select `Position` in the `Adjustment Mode` list
- Align the `Pivot point` with your wrist pivot point. You can see where it is relative to the controller IRL
- World-aligned 3D grid serves as a stationary reference to track `Pivot point` movement.
  Keeping it in one cell regardless of wrist rotation is good enough
- You can move the saber along its axis using the `ZOffset` slider

![Position mode preview](media/Position.png)

## `Rotation` adjustment mode:
``` diff
- Advanced: An understanding of how the saber rotation affects your swing is required
```

Rotation only adjustment mode. Displays `Direction Vector` in spherical coordinates. Best used with `Use Free Hand` enabled

Allows you to change the `Direction vector` without changing the `Pivot point`

- Select `Rotation` in the `Adjustment Mode` list
- Point forward with your palm facing down and press the button
- Rotate controller left or right for the curved swing correction (use the horizontal yellow line as a guide)
- Rotate controller up or down for the underswing correction (use the vertical yellow line as a guide)
- Additional straight swing reference line will appear after the benchmark test

You can raise/lower your free hand to zoom in/out

![Rotation mode preview](media/Rotation.png)

## `Swing Benchmark` adjustment mode:
``` diff
- Advanced: Requires developed technique and ability to full swing
```
Gives you objective measurements of your swing. Use for config and grip analysis. Best used with `Use Free Hand` enabled

Results will gradually change from vertical to horizontal swing. Optimize your config for a diagonal swing, unless you know what you're looking for

- Select `Swing Benchmark` in the `Adjustment Mode` list
- Repeat one exact swing several times while holding the button
- Stand still, look forward, and swing exactly as you do in-game
- Swing angle of at least 140° is required

Calculation error mostly depends on a `Tip wobble` (smaller is better) and `Angle` (higher is better). The more repeatable your swings are, the more precise result you'll get

After the successful test, the calculated wrist rotation axis and straight swing reference plane will appear on your hand.

![Benchmark mode preview](media/Benchmark.png)

### `Swing Curve` measurement
Indicates how straight your swing is

- Negative values mean that your saber is pointing too much inwards (to the center)
- Positive values mean that your saber is pointing too much outwards (away from the center)
- Values around zero mean that your swing is perfectly straight

How to improve:

- You can reduce it manually in the `Rotation` mode (A special reference line will appear after the test)
- You can completely remove it automatically by pressing the `Apply rotation fix` button. This action will change your config, backup if you're not sure!
- Straight swing requires fairly wide hand placement, which may feel uncomfortable at first if you had a big outward curve

### `Tip Wobble` measurement
Indicates how much tip of your saber deviates from the plane.

One of the main accuracy limiting factors. You have only 2 cm of margin to score 115 consistently, 4 cm for 114, etc. (assuming you have no underswing, no time dependency, and always aim for the center)

- If it is caused by the 8-shaped swing, you have poor weight balance with your grip
- If your swing is not repeatable at all, you have to practice more and optimize your technique
- Stretching regularly will help reduce wobble at the limiting angles

How to improve:

- You can remove it only by changing your grip to a weight-balanced one
- You can reduce it by using more arm at a stamina cost
- Work on your technique: make your swings as smooth and slow as possible

### `Arm Usage` measurement
Indicates how much the hilt of your saber moves during the swing

Preference, optimize to your play style

- Higher arm usage requires a lot of stamina and significantly limits your speed capabilities
- Lower arm usage increases wobble, limiting your accuracy

### `Angle` measurement
Indicates how much angle is covered by your swing. Also shows your backhand and forehand angles (relative to the horizontal plane)

- According to the game scoring system, you need at least a 100° pre-swing angle and a 60° post-swing angle. Note that these values are relative to the block (e.g. bottom lane up-swing requires a much greater forehand angle than the top lane up-swing)

How to improve:

- If you are constantly under-swinging a certain direction, change the forehand/backhand angle balance by rotating the saber up/down in the `Rotation` mode
- Regular stretching will help increase the range of motion in your wrist

# Extra features

## `Rotation Auto` adjustment mode:
``` diff
- Expert: An understanding of what a good config looks like is required. The result may not be optimal
- Config-breaking: Drastically changes your config. Save the preset to have a backup
```

Automatic calculation of the `Direction vector` based on your movement. Great for getting initial values for a new grip, requires manual tweaking in `Rotation` mode afterward.
Affects only rotation, doesn't change the position values

- Select `Rotation Auto` in the `Adjustment Mode` list
- Point forward, press the button, and start rotating your wrist as you do when turning a doorknob
- Hold the button until trail movement is minimized. 2-3 seconds usually enough
- Fine-tune using `Swing Benchmark` and `Rotation` modes

The result depends only on your motion while a button is pressed. Make sure to choose the most comfortable rotation axis

## `Room Offset` adjustment mode:
``` diff
+ Easy: suitable for any skill level
```
World pulling locomotion in Beat Saber! This mode uses base game room offset settings to move you around

- Select `Room offset` in the `Adjustment Mode` list
- Hold the button and pull or push the world around you
- To move only vertically, disable X and Z axles

Useful for quick floor level alignment. Just put one controller on the floor and use its model as a reference
