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

[AddComponentMenu("BoneCracker Games/Realistic Car Controller/Misc/RCC Trailer Attacher")]
public class RCC_TrailerAttachPoint : MonoBehaviour {

	void OnTriggerEnter(Collider col){

		RCC_TrailerAttachPoint otherAttacher = col.gameObject.GetComponent<RCC_TrailerAttachPoint> ();

		if (!otherAttacher)
			return;

		RCC_CarControllerV3 otherVehicle = otherAttacher.gameObject.GetComponentInParent<RCC_CarControllerV3> ();

		if (!otherVehicle)
			return;
		
		GetComponentInParent<ConfigurableJoint>().transform.SendMessage ("AttachTrailer", otherVehicle, SendMessageOptions.DontRequireReceiver);

	}

    private void Reset() {

		if (GetComponent<BoxCollider>() == null)
			gameObject.AddComponent<BoxCollider>().isTrigger = true;

    }

}
