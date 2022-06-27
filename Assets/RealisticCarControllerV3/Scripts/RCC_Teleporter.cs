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

[AddComponentMenu("BoneCracker Games/Realistic Car Controller/Misc/RCC Teleporter")]
public class RCC_Teleporter : MonoBehaviour{

	public Transform spawnPoint;
	
	void OnTriggerEnter(Collider col){

		if (col.isTrigger)
			return;

		RCC_CarControllerV3 carController = col.gameObject.GetComponentInParent<RCC_CarControllerV3> ();

		if (!carController)
			return;

		RCC.Transport (carController, spawnPoint.position, spawnPoint.rotation);
		
	}

}
