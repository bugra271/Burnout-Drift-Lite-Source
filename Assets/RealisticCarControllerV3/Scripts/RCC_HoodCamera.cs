//----------------------------------------------
//            Realistic Car Controller
//
// Copyright © 2014 - 2022 BoneCracker Games
// http://www.bonecrackergames.com
// Buğra Özdoğanlar
//
//----------------------------------------------

using System.Collections;
using UnityEngine;

/// <summary>
/// RCC Camera will be parented to this gameobject when current camera mode is Hood Camera.
/// </summary>
[AddComponentMenu("BoneCracker Games/Realistic Car Controller/Camera/RCC Hood Camera")]
public class RCC_HoodCamera : MonoBehaviour {

    private void Awake() {

		CheckConnecter();

	}

    public void FixShake(){

		StartCoroutine (FixShakeDelayed());
		
	}

	IEnumerator FixShakeDelayed(){

		if (!GetComponent<Rigidbody> ())
			yield break;

		yield return new WaitForFixedUpdate ();
		GetComponent<Rigidbody> ().interpolation = RigidbodyInterpolation.None;
		yield return new WaitForFixedUpdate ();
		GetComponent<Rigidbody> ().interpolation = RigidbodyInterpolation.Interpolate;

	}

	private void CheckConnecter() {

		ConfigurableJoint joint = GetComponent<ConfigurableJoint>();

		if (!joint)
			return;

		if (joint.connectedBody == null) {

			Debug.LogError("Hood camera of the " + transform.root.name + " has configurable joint with no connected body! Disabling rigid and joint of the camera.");
			Destroy(joint);

			Rigidbody rigid = GetComponent<Rigidbody>();

			if(rigid)
				Destroy(rigid);

		}

	}

}

