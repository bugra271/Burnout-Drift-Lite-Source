using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainCameraTrigger : MonoBehaviour {

//	public RainCameraController rainController;
	public GameObject[] objects;

	void OnTriggerEnter (Collider col) {
		
		if (!col.transform.root.CompareTag ("Player"))
			return;

//		if (rainController && rainController.gameObject.activeInHierarchy)
//			rainController.Stop ();

		foreach (GameObject go in objects)
			go.SetActive (false);
		
	}

	void OnTriggerExit (Collider col) {

		if (!col.transform.root.CompareTag ("Player"))
			return;

//		if (rainController && rainController.gameObject.activeInHierarchy)
//			rainController.Play ();

		foreach (GameObject go in objects)
			go.SetActive (true);

	}

}
