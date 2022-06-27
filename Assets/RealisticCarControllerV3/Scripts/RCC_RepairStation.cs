//----------------------------------------------
//            Realistic Car Controller
//
// Copyright © 2014 - 2022 BoneCracker Games
// http://www.bonecrackergames.com
// Buğra Özdoğanlar
//
//----------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("BoneCracker Games/Realistic Car Controller/Misc/RCC Repair Station")]
public class RCC_RepairStation : MonoBehaviour {

	private RCC_CarControllerV3 targetVehicle;

	void OnTriggerEnter (Collider col) {

		if (targetVehicle == null) {

			if (col.gameObject.GetComponentInParent<RCC_CarControllerV3> ())
				targetVehicle = col.gameObject.GetComponentInParent<RCC_CarControllerV3> ();

		}

		if (targetVehicle)
			targetVehicle.Repair();

	}

	void OnTriggerExit (Collider col) {

		if (col.gameObject.GetComponentInParent<RCC_CarControllerV3>()) {

			col.gameObject.GetComponentInParent<RCC_CarControllerV3>().Repair();
			targetVehicle = null;

		}

	}

}
