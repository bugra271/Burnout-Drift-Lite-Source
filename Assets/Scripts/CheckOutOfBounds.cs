using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckOutOfBounds : MonoBehaviour {

	public bool reposition = true;

	void OnTriggerEnter (Collider col) {

		if (!col.transform.root.CompareTag ("Player"))
			return;
        
		if (reposition) {

			GameObject player = col.transform.root.gameObject;
			Rigidbody playerRigid = col.transform.root.GetComponent<Rigidbody>();

			playerRigid.velocity = Vector3.zero;
			playerRigid.angularVelocity = Vector3.zero;

			player.transform.position = GameplayManager.Instance.spawnPoint.position;
			player.transform.rotation = GameplayManager.Instance.spawnPoint.rotation;

		}
		
	}

}
