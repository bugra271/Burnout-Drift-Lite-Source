using UnityEngine;
using System.Collections;

public class WaypointManager : MonoBehaviour {

	public Waypoint startFinishWaypoint;

	private Waypoint[] waypoints;
	private int[] waypointIDs;

	void Start () {

		waypoints = GetComponentsInChildren<Waypoint> ();
		waypointIDs = new int[waypoints.Length];
		startFinishWaypoint.gameObject.SetActive (false);
	
	}

	public void WaypointPassed (Waypoint wp) {

		for (int i = 0; i < waypoints.Length; i++) {

			if (wp == waypoints [i]) {
				waypointIDs [i] = 1;
				break;
			}
			
		}

		bool completed = true;

		for (int i = 0; i < waypoints.Length; i++) {

			if (wp != startFinishWaypoint && waypointIDs [i] == 0) {
				completed = false;
			}

		}
			
		startFinishWaypoint.gameObject.SetActive (true);

		if(wp == startFinishWaypoint)
			LapCompleted ();
	
	}

	public void LapCompleted(){

		print ("LapCompleted");

		gameObject.SetActive (false);
		GameplayManager.Instance.FinishRace ();

	}

}
