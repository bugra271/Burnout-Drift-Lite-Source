Thanks for purchase Orbit Camera.

- Get Started:

- Import the 'Orbit Camera' in your project.
- Drag the prefab 'Orbit Camera' (Camera Orbit/Content/Prefabs/Orbit Camera) to the scene that you want add.
- Drag the target to camera orbit to the var 'Target' of bl_CameraOrbit' of Orbit Camera prefab in scene.
- Customize variables to your taste and you are ready!.

---------------------------------------------------------------------------------------------------------------

Add button for rotate: 

- When you need rotate camera with UI buttons, you only need do this:
   
   - Create the Buttons (Horizontals and Verticals as you need)
   - in this buttons add the script bl_OrbitButton.cs
   - in the var "Camera Orbit" al camera target to rotate (with a bl_CameraOrbit script add).
   - Setting the directional Axis that you need rotate.
   - Where amount of Horizontal is < 0 = Right, > 0 is Left and in Vertical < 0 is down , > 0 is up.
   - Ready!.

-------------------------------------------------------------------------------------------------------------

Use For Mobile.

-If you want use for mobile, you only need setup a few things for this.
firts, in bl_CameraOrbit script mark the field "isForMobile" as true.
then you need create a UI Image (UGUI) in your canvas and add the script "bl_OrbitTouchPad", remember that the area of this image,
is the are where you can use for rotate the camera, so is recomendable that you set this in the botton of all UI for not block others UI interactable.

- else if you use the "bl_OrbitTouchPad" remember set the field "OverrideEditor" as true for avoid brong movements.

-------------------------------------------------------------------------------------------------------------
Public Vars:

Transform Target: Transform to camera orbit and follow.
bool AutoTakeInfo: take the closest circular position to the default position (editor position) at the start.
float Distance: Distance between the camera and the target.
Vector2 DistanceClamp: Min and Max Distance (x = min y = max).
Vector2 YLimitClamp: Min and Max Y angle limit (x = min y = max).
Vector2 SpeedAxis: Speep of movement in respective angles (x = horizontal y = vertical)
bool RequieredInput: Requiered input for rotate and move camera?
KeyCode InputKey: Key to move camera (when RequieredInput is true).
float InputMultiplier: how much multiple the input amount.
float InputLerp: Smooth for input value.
bool UseKey: use Horizontal and Vertical Key axis instead of mouse axis?
float PuwFogAmount: Amount for 'puw' effect of camera (zoom when up click)
float OutInputSpeed: Speed to apply to rotation when Input is up.
float FogStart: Where the fog camera will start?
float FogLerp: Smooth amount for fog transition.
float DelayStartFog: how much time take to fog go to normal fog from StartFog (just in start function).
float DistanceInfluence: How much the distance influence the movement amount.

----------------------------------------------------------------------------------------------------------------

Change Target in RealTime:

If you want change the target to camera orbit in runtime, simple call the function "SetTarget(newTarget)" of bl_CameraOrbit.
where "newTarget" is the reference transfor of new object that will orbit.

----------------------------------------------------------------------------------------------------------------

Remove the Zoom effect on start:

In the scene demo you will see a "zoom" effect on the start, this is a feature but if you don't want use it, simple need
set the variable "FogStart" of bl_CameraOrbit script to the same "fieldOfView" of CameraOrbit (60 for default).

----------------------------------------------------------------------------------------------------------------

Set the default position and rotation on start:

The orbit camera calculate the orbit movement depend of distance and rotation to be "circular" this mean that you can't see a default position
due this will be corrected since the first called of Update function (due the orbit operation), but we have made that take the most close position
to the default on start, but not will be exactly the same.

----------------------------------------------------------------------------------------------------------------

Contact / Support:
Forum: http://lovattostudio.com/Forum/index.php
Email: brinerjhonson.lc@gmail.com


