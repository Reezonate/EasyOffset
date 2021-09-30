# Easy Offset

Beat Saber mod for intuitive controller offset adjustments. 

Tired from a pile of magic config numbers? Find the best offset for your grip just by moving your hand!

Read [user guide](https://github.com/Reezonate/EasyOffset#how-to-use) for more info 

![About image](media/About.png)

In this mod offset is defined by three config values for each hand:

- `Pivot point` - saber origin position relative to the controller
- `Direction vector` - saber tip position relative to `Pivot point`
- `ZOffset` - saber position offset along `Direction vector`

# Compatibility

Not compatible with other offset adjustment mods (e.g. SaberTailor, ControllerTweaks)

**PC ONLY**! Quest version is planned, but not guaranteed

# Coming soon™
- Config import and export in .json format

# How to install
- Install **BSIPA** and **BeatSaberMarkupLanguage** dependencies using
 [ModAssistant](https://github.com/Assistant/ModAssistant)
  
- Download the latest .dll for your game version from the 
[Releases page](https://github.com/Reezonate/EasyOffset/releases)
and put it in `/plugins` directory inside your game folder

# How to use
Before trying to find your best config, spend some time in different modes and experiment with the tools at your disposal

First steps:
- Select your controller model in the `DisplayController` list
- Choose a button you can press without changing your grip
- If there is no such button, check the `Use Free Hand` toggle to use the button on the other hand

To get a decent config, you have to achieve two goals:
- `Pivot point` should be stable as the wrist rotates. In other words - it should be aligned with your actual wrist pivot point as much as possible
- Saber trail shouldn't move in a huge circle while you pointing forward and rolling your wrist comfortably. The smaller the radius, the better

To help you achieve these goals, there are three adjustment modes:
- `Basic`
- `Pivot Only`
- `Direction Only`

## `Basic` adjustment mode: 
``` diff
+ Easy, suitable for any skill level
```
Simple adjustment mode made for beginners and casual players
- Select `Basic` in the `Adjustment Mode` list
- While holding the selected button, move your hand and pick up the saber in a new position by releasing the button
- You can move the saber along its axis using the `ZOffset` slider

## `Pivot Only` adjustment mode: 
``` diff
+ Easy - suitable for any skill level
```
Precise position adjustment mode, displays `Pivot point` coordinates in centimeters

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
Precise swing adjustment mode for advanced players, displays `Direction Vector` in spherical coordinates. Best used with `Use Free Hand` enabled

Allows you to change the `Direction vector` without changing the `Pivot point`

 - Select `Direction Only` in the `Adjustment Mode` list
 - Point saber left or right for the curved swing correction
 - Point saber up or down for the underswing correction
 - Raise your free hand to zoom in for greater precision

![Direction Only mode preview](media/DirectionOnly.png)

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
- Push the button and start rolling your wrist left and right. 
  Not like you're cutting the block, but as if you are trying to drill a hole in it
- Hold the button until trail movement is minimized. 2-3 seconds usually enough

## `Room Offset` adjustment mode:
``` diff
+ Easy: suitable for any skill level
```
World pulling locomotion in Beat Saber! This mode uses base game room offset settings to move you around

- Select `Room offset` in the `Adjustment Mode` list
- Hold the button and pull or push the world around you
- you can reset offset in base game settings

Useful for quick floor level alignment. Just put one controller on the floor and use its model as a reference
