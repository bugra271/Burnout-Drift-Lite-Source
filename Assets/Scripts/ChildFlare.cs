using UnityEngine;
using System.Collections;

public class ChildFlare : MonoBehaviour {

	private LensFlare flare;
	private LensFlare parentFlare;

	void Start () {

		flare = transform.GetComponent<LensFlare> ();
		parentFlare = GetComponentInParent<LensFlare> ();
	
	}
	
	// Update is called once per frame
	void Update () {

		flare.brightness = parentFlare.brightness;
		flare.color = parentFlare.color;
	
	}

}
