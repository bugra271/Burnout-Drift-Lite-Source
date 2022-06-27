//----------------------------------------------
//            Realistic Car Controller
//
// Copyright © 2014 - 2022 BoneCracker Games
// http://www.bonecrackergames.com
// Buğra Özdoğanlar
//
//----------------------------------------------

using UnityEngine;
using UnityEngine.Audio;
using System.Collections;

/// <summary>
/// Stored all general shared RCC settings here.
/// </summary>
[System.Serializable]
public class RCC_Settings : ScriptableObject {

	#region singleton
	private static RCC_Settings instance;
	public static RCC_Settings Instance { get { if (instance == null) instance = Resources.Load("RCC Assets/RCC_Settings") as RCC_Settings; return instance; } }
	#endregion

	public int behaviorSelectedIndex;

	public BehaviorType selectedBehaviorType {
		get {
			if (overrideBehavior)
				return behaviorTypes[behaviorSelectedIndex];
			else
				return null;
		}
	}

	public bool overrideBehavior = true;
	public bool overrideFPS = true;
	public bool overrideFixedTimeStep = true;
	[Range(.005f, .06f)] public float fixedTimeStep = .02f;
	[Range(.5f, 20f)] public float maxAngularVelocity = 6;
	public int maxFPS = 60;

	// Behavior Types
	[System.Serializable]
	public class BehaviorType {

		public string behaviorName = "New Behavior";

		[Header("Steering Helpers")]
		public bool steeringHelper = true;
		public bool tractionHelper = true;
		public bool angularDragHelper = false;
		public bool counterSteering = true;
		public bool limitSteering = true;
		public bool steeringSensitivity = true;
		public RCC_CarControllerV3.SteeringType steeringType = RCC_CarControllerV3.SteeringType.Curve;
		public bool ABS = false;
		public bool ESP = false;
		public bool TCS = false;
		public bool applyExternalWheelFrictions = false;
		public bool applyRelativeTorque = false;

		[Space()]
		public float highSpeedSteerAngleMinimum = 20f;
		public float highSpeedSteerAngleMaximum = 40f;

		public float highSpeedSteerAngleAtspeedMinimum = 100f;
		public float highSpeedSteerAngleAtspeedMaximum = 200f;

		[Space()]
		public float counterSteeringMinimum = .1f;
		public float counterSteeringMaximum = 1f;

		[Space()]
		public float steeringSensitivityMinimum = .5f;
		public float steeringSensitivityMaximum = 1f;

		[Space()]
		[Range(0f, 1f)] public float steerHelperAngularVelStrengthMinimum = .1f;
		[Range(0f, 1f)] public float steerHelperAngularVelStrengthMaximum = 1;

		[Range(0f, 1f)] public float steerHelperLinearVelStrengthMinimum = .1f;
		[Range(0f, 1f)] public float steerHelperLinearVelStrengthMaximum = 1f;

		[Range(0f, 1f)] public float tractionHelperStrengthMinimum = .1f;
		[Range(0f, 1f)] public float tractionHelperStrengthMaximum = 1f;

		[Space()]
		public float antiRollFrontHorizontalMinimum = 1000f;
		public float antiRollRearHorizontalMinimum = 1000f;

		[Space()]
		[Range(0f, 1f)] public float gearShiftingDelayMaximum = .15f;

		[Range(0f, 10f)] public float angularDrag = .1f;
		[Range(0f, 1f)] public float angularDragHelperMinimum = .1f;
		[Range(0f, 1f)] public float angularDragHelperMaximum = 1f;

		[Header("Wheel Frictions Forward")]
		public float forwardExtremumSlip = .4f;
		public float forwardExtremumValue = 1f;
		public float forwardAsymptoteSlip = .8f;
		public float forwardAsymptoteValue = .5f;

		[Header("Wheel Frictions Sideways")]
		public float sidewaysExtremumSlip = .2f;
		public float sidewaysExtremumValue = 1f;
		public float sidewaysAsymptoteSlip = .5f;
		public float sidewaysAsymptoteValue = .75f;

	}

	public bool useFixedWheelColliders = true;
	public bool lockAndUnlockCursor = true;

	// Behavior Types
	public BehaviorType[] behaviorTypes;

	// Main Controller Settings
	public bool useAutomaticGear = true;
	public bool useAutomaticClutch = true;
	public bool runEngineAtAwake = true;
	public bool autoReverse = true;
	public bool autoReset = true;

	public GameObject contactParticles;
	public GameObject scratchParticles;
	public GameObject wheelDeflateParticles;

	public Units units;
	public enum Units { KMH, MPH }

	// UI Dashboard Type
	public UIType uiType;
	public enum UIType { UI, NGUI, None }

	// Information telemetry about current vehicle
	public bool useTelemetry = false;

	// For mobile inputs
	public enum MobileController { TouchScreen, Gyro, SteeringWheel, Joystick }
	public MobileController mobileController;
	public bool mobileControllerEnabled = false;

	// Mobile controller buttons and accelerometer sensitivity
	public float UIButtonSensitivity = 10f;
	public float UIButtonGravity = 10f;
	public float gyroSensitivity = 2f;

	// Used for using the lights more efficent and realistic
	public bool useLightsAsVertexLights = true;
	public bool useLightProjectorForLightingEffect = false;

	// Other stuff
	public bool setTagsAndLayers = false;
	public string RCCTag = "Player";
	public string RCCLayer = "RCC";
	public string WheelColliderLayer = "RCC_WheelCollider";
	public string DetachablePartLayer = "RCC_DetachablePart";
	public bool tagAllChildrenGameobjects = false;

	public GameObject chassisJoint;
	public GameObject exhaustGas;
	public RCC_SkidmarksManager skidmarksManager;
	public GameObject projector;
	public LayerMask projectorIgnoreLayer;

	public GameObject headLights;
	public GameObject brakeLights;
	public GameObject reverseLights;
	public GameObject indicatorLights;
	public GameObject lightTrailers;
	public GameObject mirrors;

	public RCC_Camera RCCMainCamera;
	public GameObject hoodCamera;
	public GameObject cinematicCamera;
	public GameObject RCCCanvas;
	public GameObject RCCTelemetry;

	public bool dontUseAnyParticleEffects = false;
	public bool dontUseSkidmarks = false;

	// Sound FX
	public AudioMixerGroup audioMixer;
	public AudioClip[] gearShiftingClips;
	public AudioClip[] crashClips;
	public AudioClip reversingClip;
	public AudioClip windClip;
	public AudioClip brakeClip;
	public AudioClip wheelDeflateClip;
	public AudioClip wheelInflateClip;
	public AudioClip wheelFlatClip;
	public AudioClip indicatorClip;
	public AudioClip bumpClip;
	public AudioClip NOSClip;
	public AudioClip turboClip;
	public AudioClip[] blowoutClip;
	public AudioClip[] exhaustFlameClips;

	[Range(0f, 1f)] public float maxGearShiftingSoundVolume = .25f;
	[Range(0f, 1f)] public float maxCrashSoundVolume = 1f;
	[Range(0f, 1f)] public float maxWindSoundVolume = .1f;
	[Range(0f, 1f)] public float maxBrakeSoundVolume = .1f;

	// Used for folding sections of RCC Settings
	public bool foldGeneralSettings = false;
	public bool foldBehaviorSettings = false;
	public bool foldControllerSettings = false;
	public bool foldUISettings = false;
	public bool foldWheelPhysics = false;
	public bool foldSFX = false;
	public bool foldOptimization = false;
	public bool foldTagsAndLayers = false;

}
