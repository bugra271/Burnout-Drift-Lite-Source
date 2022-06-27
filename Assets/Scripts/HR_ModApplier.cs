//----------------------------------------------
//           	   Highway Racer
//
// Copyright © 2016 BoneCracker Games
// http://www.bonecrackergames.com
//
//----------------------------------------------

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(PlayerManager))]
public class HR_ModApplier : MonoBehaviour {
	
	private RCC_CarControllerV3 carController;

	internal Color bodyColor;
	public MeshRenderer bodyRenderer;
	public int bodyRendererMaterialIndex = 0;

	internal GameObject selectedWheel;
	internal int wheelIndex;

	//---------------------------//

//	private int _speedLevel = 0;
//	public int speedLevel
//	{
//		get
//		{
//			return _speedLevel;
//		}
//		set
//		{
//			if(value <= 5)
//				_speedLevel = value;
//		}
//	}
//
//	private int _handlingLevel = 0;
//	public int handlingLevel
//	{
//		get
//		{
//			return _handlingLevel;
//		}
//		set
//		{
//			if(value <= 5)
//				_handlingLevel = value;
//		}
//	}
//
//	private int _brakeLevel = 0;
//	public int brakeLevel
//	{
//		get
//		{
//			return _brakeLevel;
//		}
//		set
//		{
//			if(value <= 5)
//				_brakeLevel = value;
//		}
//	}
//	
//	private float defMaxSpeed;
//	private float defHandling;
//	private float defMaxBrake;
//
//	public float maxUpgradeSpeed;
//	public float maxUpgradeHandling;
//	public float maxUpgradeBrake;

	void Awake () {

		carController = GetComponent<RCC_CarControllerV3>();

//		defMaxSpeed = carController.maxspeed;
//		defHandling = carController.highspeedsteerAngle;
//		defMaxBrake = carController.brakeTorque;

		if (PlayerPrefs.HasKey (transform.name + "SelectedWheel")) {
			wheelIndex = PlayerPrefs.GetInt (transform.name + "SelectedWheel", 0);
			selectedWheel = SelectableWheels.Instance.wheels [wheelIndex].wheel;
		} else {
			selectedWheel = null;
		}

//		_speedLevel = PlayerPrefs.GetInt(transform.name + "SpeedLevel");
//		_handlingLevel = PlayerPrefs.GetInt(transform.name + "HandlingLevel");
//		_brakeLevel = PlayerPrefs.GetInt(transform.name + "BrakeLevel");

		bodyColor = RCC_PlayerPrefsX.GetColor(transform.name + "BodyColor", Color.white);

	}

	void OnEnable(){

//		HR_ModificationUpgrade[] mods = GameObject.FindObjectsOfType<HR_ModificationUpgrade>();
//
//		for (int i = 0; i < mods.Length; i++) {
//
//			mods[i].applier = GetComponent<HR_ModApplier>();
//
//		}
		
		UpdateStats();
//		CheckGroundGap ();

	}

	public void UpdateStats (){

//		carController.maxspeed = Mathf.Lerp(defMaxSpeed, maxUpgradeSpeed, _speedLevel / 5f);
//		carController.highspeedsteerAngle = Mathf.Lerp(defHandling, maxUpgradeHandling, _handlingLevel / 5f);
//		carController.brakeTorque = Mathf.Lerp(defMaxBrake, maxUpgradeBrake, _brakeLevel / 5f);

		if(bodyRenderer)
			bodyRenderer.sharedMaterials[bodyRendererMaterialIndex].color = bodyColor;
		else
			Debug.LogError("Missing Body Renderer On ModApllier Component");

		if (selectedWheel) {

			PlayerPrefs.SetInt(transform.name + "SelectedWheel", wheelIndex);

			RCC_Customization.ChangeWheels (carController, selectedWheel, true);

			carController.FrontLeftWheelCollider.wheelCollider.radius = RCC_GetBounds.MaxBoundsExtent (selectedWheel.transform);
			carController.FrontRightWheelCollider.wheelCollider.radius = RCC_GetBounds.MaxBoundsExtent (selectedWheel.transform);
			carController.RearLeftWheelCollider.wheelCollider.radius = RCC_GetBounds.MaxBoundsExtent (selectedWheel.transform);
			carController.RearRightWheelCollider.wheelCollider.radius = RCC_GetBounds.MaxBoundsExtent (selectedWheel.transform);

//			RCC_Customization.SetFrontSuspensionsDistances (carController, RCC_GetBounds.MaxBoundsExtent (selectedWheel.transform) / 2f);
//			RCC_Customization.SetRearSuspensionsDistances (carController, RCC_GetBounds.MaxBoundsExtent (selectedWheel.transform) / 2f);

		}

//		PlayerPrefs.SetInt(transform.name + "SpeedLevel", _speedLevel);
//		PlayerPrefs.SetInt(transform.name + "HandlingLevel", _handlingLevel);
//		PlayerPrefs.SetInt(transform.name + "BrakeLevel", _brakeLevel);
		RCC_PlayerPrefsX.SetColor(transform.name + "BodyColor", bodyColor);
	
	}

	void Update(){

//		if(maxUpgradeSpeed < carController.maxspeed)
//			maxUpgradeSpeed = carController.maxspeed;
//
//		if(maxUpgradeHandling < carController.highspeedsteerAngle)
//			maxUpgradeHandling = carController.highspeedsteerAngle;
//
//		if(maxUpgradeBrake < carController.brakeTorque)
//			maxUpgradeBrake = carController.brakeTorque;

	}

	void CheckGroundGap(){

		WheelCollider wheel = GetComponentInChildren<WheelCollider> ();
		float distancePivotBetweenWheel = Vector3.Distance (new Vector3(0f, transform.position.y, 0f), new Vector3(0f, wheel.transform.position.y, 0f));

		RaycastHit hit;

		if (Physics.Raycast (wheel.transform.position, -Vector3.up, out hit, 10f)) {
			transform.position = new Vector3 (transform.position.x, hit.point.y + distancePivotBetweenWheel + (wheel.radius / 1f) + (wheel.suspensionDistance / 2f), transform.position.z);
		}

	}

}
