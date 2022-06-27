using UnityEngine;
using System.Collections;

public class LensFlareBrightness : MonoBehaviour {

	private Light _light;
	private LensFlare lensflare;

	public float multiplier = 2f;

	void Start () {

		_light = GetComponent<Light> ();
		lensflare = GetComponent<LensFlare> ();
		
	}

	void Update () {

		lensflare.brightness = _light.intensity * multiplier;
		lensflare.color = _light.color * (_light.intensity * multiplier);

	}

}
