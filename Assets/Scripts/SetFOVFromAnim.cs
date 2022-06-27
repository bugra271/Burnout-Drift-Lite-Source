using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetFOVFromAnim : MonoBehaviour {

	private Camera animCamera;
	public Camera targetCamera;

	void OnEnable () {

		animCamera = GetComponent<Camera> ();
		
	}

	void Update(){

		if (!animCamera)
			return;

		if (!targetCamera)
			return;

		if (!targetCamera.transform.IsChildOf (transform))
			return;

		targetCamera.fieldOfView = animCamera.fieldOfView;

	}

}
