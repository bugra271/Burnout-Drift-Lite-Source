using UnityEngine;
using System.Collections;

public class Waypoint : MonoBehaviour {

	void OnTriggerEnter(Collider col){

		if (col.transform.CompareTag ("Player"))
			GetComponentInParent<WaypointManager> ().WaypointPassed (this);

	}

	void OnEnable(){

		GameplayManager.OnRaceStarted += GameplayManager_OnPlayerSpawned;

	}

	void GameplayManager_OnPlayerSpawned ()
	{
		//print ("hebele hübele");
	}

	void OnDisable(){

		GameplayManager.OnRaceStarted -= GameplayManager_OnPlayerSpawned;

	}

}
