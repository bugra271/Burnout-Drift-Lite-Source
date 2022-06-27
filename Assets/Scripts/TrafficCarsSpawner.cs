using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficCarsSpawner : MonoBehaviour {

	public List<GameObject> spawnableTrafficCars;
	public GameObject spawnPointsContainer;

	private List<Transform> spawnPositions = new List<Transform>();

	public int maximumVehicles = 15;

	void Start () {

		Spawn ();
		
	}

	public void Spawn () {

		Transform[] childs = spawnPointsContainer.GetComponentsInChildren<Transform> ();
		GameObject vehicles = new GameObject ("Traffic Vehicles");
		vehicles.transform.SetParent (transform, false);

		foreach (var item in childs) {
			spawnPositions.Add (item);
		}

		spawnPositions.Remove (spawnPointsContainer.transform);

		for (int i = 0; i < spawnableTrafficCars.Count; i++) {
			
			GameObject temp = spawnableTrafficCars[i];
			int randomIndex = Random.Range(i, spawnableTrafficCars.Count);
			spawnableTrafficCars[i] = spawnableTrafficCars[randomIndex];
			spawnableTrafficCars[randomIndex] = temp;

		}

		for (int i = 0; i < spawnPositions.Count; i++) {

			Transform temp = spawnPositions[i];
			int randomIndex = Random.Range(i, spawnPositions.Count);
			spawnPositions[i] = spawnPositions[randomIndex];
			spawnPositions[randomIndex] = temp;

		}

		int k = 0;
		int j = 0;

		for (int i = 0; i < maximumVehicles; i++) {

			k++;
			j++;

			if (k >= spawnableTrafficCars.Count)
				k = 0;

			if (j >= spawnPositions.Count) {
				
				j = 0;
				break;

			}

			int r = Random.Range (0, 2);
			GameObject.Instantiate (spawnableTrafficCars [k], spawnPositions [j].position, r == 0 ? spawnPositions [j].rotation : spawnPositions[j].rotation * Quaternion.AngleAxis(180f, Vector3.up), vehicles.transform);

		}

	}

}
