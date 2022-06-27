using UnityEngine;
using System.Collections;

public class Obstacle : MonoBehaviour {

	private Rigidbody rigid;

	public string name = "Prop";
	public int points = 0;
	internal bool crashed = false;
	public bool destroy = true;
	public bool kinematize = true;

	public ParticleSystem blow;
	public ParticleSystem.EmissionModule engineBlowEm;

	void Start () {

		rigid = GetComponent<Rigidbody> ();

		if(kinematize)
			rigid.Sleep ();

		if (blow) {

			engineBlowEm = blow.emission;
			engineBlowEm.enabled = false;

		}

	}

	void OnCollisionEnter(Collision col){

		if (col.relativeVelocity.magnitude < 5)
			return;

		if(blow)
			blow.gameObject.SetActive (true);

		if(destroy)
			Destroy (gameObject, 5f);

	}

}
