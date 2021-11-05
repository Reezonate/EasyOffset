# Easy Offset

Beat Saber mod for intuitive controller offset adjustments. 

Tired from a pile of magic config numbers? Find the best offset for your grip just by moving your hand!

Read [user guide](https://github.com/Reezonate/EasyOffset#how-to-use) for more info 

![About image](media/About.png)

In this mod offset is defined by three config values for each hand:

- `Pivot point` - saber origin position relative to the controller (white dot surrounded by a grid)
- `Direction vector` - saber tip position relative to `Pivot point`
- `ZOffset` - saber position offset along `Direction vector`

# Compatibility

Not compatible with other offset adjustment mods (e.g. SaberTailor, ControllerTweaks)

Config values are different from base game settings, SaberTailor, etc.

**PC ONLY**! Quest version is planned, but not guaranteed

# Coming soon™
- cool new features

# How to install
- Install **BSIPA** and **BeatSaberMarkupLanguage** dependencies using
 [ModAssistant](https://github.com/Assistant/ModAssistant)
  
- Download the latest .dll for your game version from the 
[Releases page](https://github.com/Reezonate/EasyOffset/releases)
and put it in `/plugins` directory inside your game folder

# How to use
Before trying to find your best config, spend some time in different modes and experiment with the tools at your disposal

First steps:
- Select your controller model in the `Controller Type` list
- Choose a button you can press without changing your grip
- If there is no such button, check the `Use Free Hand` toggle to use the button on the other hand

To get a decent config, you have to achieve two goals:
- `Pivot point` should be stable as the wrist rotates. In other words - it should be aligned with your actual wrist pivot point as much as possible
- Saber trail shouldn't move in a huge circle while you pointing forward and rolling your wrist comfortably. The smaller the radius, the better

To help you achieve these goals, there are four adjustment modes:
- `Basic`
- `Pivot Only`
- `Direction Only`
- `Swing Benchmark`

## `Basic` adjustment mode: 
``` diff
+ Easy: suitable for any skill level
```
Simple adjustment mode made for beginners and casual players
- Select `Basic` in the `Adjustment Mode` list
- While holding the selected button, move your hand and pick up the saber in a new position by releasing the button
- You can move the saber along its axis using the `ZOffset` slider

## `Pivot Only` adjustment mode: 
``` diff
+ Easy: suitable for any skill level
```
Position adjustment mode. Displays `Pivot point` coordinates in centimeters

Allows you to change the `Pivot point` without changing the `Direction vector`

 - Select `Pivot Only` in the `Adjustment Mode` list
 - Align the `Pivot point` with your wrist pivot point. You can see where it is relative to the controller IRL
 - World-aligned 3D grid serves as a stationary reference to track `Pivot point` movement. 
   Keeping it in one cell regardless of wrist rotation is good enough
 - You can move the saber along its axis using the `ZOffset` slider

![Pivot Only mode preview](media/PivotOnly.png)

## `Direction Only` adjustment mode:
``` diff
- Advanced: requires understanding of your own swinging technique
```
Rotation adjustment mode. Displays `Direction Vector` in spherical coordinates. Best used with `Use Free Hand` enabled

Allows you to change the `Direction vector` without changing the `Pivot point`

 - Select `Direction Only` in the `Adjustment Mode` list
 - Point forward with your palm facing down and press the button
 - Rotate controller left or right for the curved swing correction (use the horizontal yellow line as a guide)
 - Rotate controller up or down for the underswing correction (use the vertical yellow line as a guide)
 - Raise your free hand to zoom in for greater precision

![Direction Only mode preview](media/DirectionOnly.png)

## `Swing Benchmark` adjustment mode:
``` diff
- Advanced: requires developed technique and ability to full swing
```
Gives you objective measurements of your swing. Use for config and grip analysis. Best used with `Use Free Hand` enabled

- Select `Swing Benchmark` in the `Adjustment Mode` list
- Repeat one exact swing several times while holding the button
- Don't move around, look forward, and swing exactly like you do in game. At least 140° swing angle required
- Results will gradually change from vertical to horizontal swing. Optimize your config for a diagonal swing, unless you know what you're looking for
- Humans are not robots, each time results will be slightly different

After the successful test, calculated wrist rotation axis and straight swing reference plane will appear on your hand

### `Swing Curve` measurement
Indicates how straight your swing is

- Negative values mean that your saber is pointing too much inwards (to the center)
- Positive values mean that your saber is pointing too much outwards (away from the center)
- Values around zero mean that your swing is perfectly straight

How to improve:

- You can reduce it manually in the `Direction Only` mode (Special reference line will appear after the test)
- You can completely remove it automatically by pressing the `Apply rotation fix` button. This action will change your config, backup if you're not sure!
- Straight swing requires fairly wide hand placement, which may feel uncomfortable at first if you had a big outward curve

### `Tip Wobble` measurement
Indicates how much tip of your saber deviates from the plane. 

One of the main accuracy limiting factors. You have only 3 cm of margin to score 115 consistently, 6 cm for 114 and etc. (assuming you're not underswinging, have no time dependency, and always aim for the center)

- If it is caused by the 8-shaped swing, you have poor weight balance with your grip
- If your swing is not repeatable at all, you have to practice more and optimize your technique

How to improve:

- You can remove it only by changing your grip to a weight-balanced one
- You can reduce it by using more arm at a stamina cost
- Work on your technique: make your swings as smooth and slow as possible

### `Arm Usage` measurement
Indicates how much hilt of your saber moves during the swing

Preference, depends on your play style

- Higher arm usage requires a lot of stamina and significantly limits your speed capabilities
- Lower arm usage increases wobble, limiting your accuracy

# Extra features

## `Direction Auto` adjustment mode:
``` diff
- Expert: requires deep understanding of what good config looks like
```
Automatic calculation of the `Direction vector` based on your movement. Great for getting initial values for a new grip, requires manual tweaking in `Direction Only` mode afterward. 
Affects **ONLY** `Direction vector`

This is simple math, not magic, and the result depends only on your motion while a button is pressed. 
If you'd tape your controller to a car wheel, this mode will simply align the saber with the wheel axis. 
Sadly, it won't make your car hit 115s with a perfect swing

- Select `Direction Auto` in the `Adjustment Mode` list
- Point forward, press the button and start rotating your wrist as you do when turning a doorknob
- Hold the button until trail movement is minimized. 2-3 seconds usually enough
- Fine-tune using the `Direction Only` mode

## `Room Offset` adjustment mode:
``` diff
+ Easy: suitable for any skill level
```
World pulling locomotion in Beat Saber! This mode uses base game room offset settings to move you around

- Select `Room offset` in the `Adjustment Mode` list
- Hold the button and pull or push the world around you
- To move only vertically, disable X and Z axles

Useful for quick floor level alignment. Just put one controller on the floor and use its model as a reference

# Base game config migration

- Create AnyName.json file in `<YourGameDirectory>/UserData/EasyOffset/Presets/` directory and open it with any text editor (e.g. Notepad)
- Paste the following template:
``` json
{
  "version": "BaseGame",
  "IsValveController": false,
  "IsVRModeOculus": false,
  "ZOffset": 110,
  "PositionX": 0,
  "PositionY": 0,
  "PositionZ": 0,
  "RotationX": 0,
  "RotationY": 0,
  "RotationZ": 0
}
```
- **if you were using Valve Index controllers**: change `false` to `true` in the `IsValveController` field
- **if you were using oculus VR mode**: change `false` to `true` in the `IsVRModeOculus` field
- Type the desired `ZOffset (mm)` value. Leave it as it is if you're not sure
- Type `Position (mm)` and `Rotation (deg)` values from your base game config into the corresponding fields
- Save file
- Now you should be able to load your config in-game

# Saber Tailor config migration

``` diff
- "World Offset" feature is not supported
```

- Create AnyName.json file in `<YourGameDirectory>/UserData/EasyOffset/Presets/` directory and open it with any text editor (e.g. Notepad)
- Paste the following template:
``` json 
{
  "version": "SaberTailor",
  "UseBaseGameAdjustmentMode": false,
  "IsValveController": false,
  "IsVRModeOculus": false,
  "rightHandZOffset": 110,
  "leftHandZOffset": 110,
  "GripLeftPosition": {
    "x": 0,
    "y": 0,
    "z": 0
  },
  "GripRightPosition": {
    "x": 0,
    "y": 0,
    "z": 0
  },
  "GripLeftRotation": {
    "x": 0,
    "y": 0,
    "z": 0
  },
  "GripRightRotation": {
    "x": 0,
    "y": 0,
    "z": 0
  }
}
```

- **If you were using "base game adjustment mode"**: change `false` to `true` in the `UseBaseGameAdjustmentMode` field
- **If you were using Valve Index controllers**: change `false` to `true` in the `IsValveController` field
- **If you were using oculus VR mode**: change `false` to `true` in the `IsVRModeOculus` field
- Type the desired `ZOffset (mm)` value for each hand. Leave it as it is if you're not sure
- Type `Position (mm)` and `Rotation (deg)` values from your saber tailor config into the corresponding fields. You can directly copy those values from your SaberTailor.json file
- Save file
- Now you should be able to load your config in-game