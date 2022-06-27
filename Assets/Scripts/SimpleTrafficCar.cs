using UnityEngine;
using System.Collections;

public class SimpleTrafficCar : MonoBehaviour {

	private Vector3 spawnPoint;
	public float speed = 1f;

	public Transform[] wheels;

	void Start () {

		spawnPoint = transform.position;
	
	}
	
	// Update is called once per frame
	void Update () {

		transform.Translate (Vector3.forward * speed * Time.deltaTime, Space.Self);

		for (int i = 0; i < wheels.Length; i++) {

			wheels [i].transform.Rotate (Vector3.right * speed * Time.deltaTime * 50f, Space.Self);

		}

		if (transform.position.x > 1000)
			transform.position = new Vector3 (1000, transform.position.y, transform.position.z);
	
	}

	void OnTriggerEnter(){

		StartCoroutine (ReEnable(2f));

	}

	IEnumerator ReEnable(float delay){

		transform.position += transform.rotation * -Vector3.forward * Random.Range (150f, 250f);

		yield return new WaitForSeconds (delay);

//		transform.position = spawnPoint;
//		gameObject.SetActive (true);

	}

}
