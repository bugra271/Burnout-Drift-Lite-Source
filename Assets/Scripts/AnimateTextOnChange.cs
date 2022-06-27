using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AnimateTextOnChange : MonoBehaviour {

	private Text text;
	private Animation anim;

	public bool changeColorOnChange = false;
	public bool changeColorToRedOnLowerValue = false;
	public bool permanent = false;
	public bool shake = false;
	public bool shakeOnEnable = false;
	public bool shakeOnColorChange = false;

	public float damageTime = 0.1f; // duration of shake and red color

	public float shakeRange = 20f; // shake range change be changed from inspector,
	//keep it mind that max it can go is half in either direct

	public Color color;
	private Color defColor;

	private string oldText;

	private Color oldColor;

	private Vector3 orgPosition;
	private Quaternion orgRotation;

	void Awake () {

		text = GetComponent<Text> ();
		anim = GetComponent<Animation> ();
		defColor = text.color;
		oldText = text.text;
		oldColor = text.color;
		
	}

	void Start(){

		orgPosition = transform.position;
		orgRotation = transform.rotation;

		if (shakeOnEnable)
			Shake();

	}

	void Update () {

		if (Time.time < 1f) {
			defColor = text.color;
			oldText = text.text;
			oldColor = text.color;
			return;
		}

		if (oldText != text.text) {
			
			if (anim && !anim.isPlaying)
				anim.Play ();
			
			if (!changeColorToRedOnLowerValue && changeColorOnChange)
				text.color = color;
			
			else if (changeColorToRedOnLowerValue && StringToFloat(text.text, 0) < StringToFloat(oldText, 0))
				text.color = Color.red;
			
			if (shake)
				Shake();
			
		} else {
			
			if (anim && !anim.isPlaying && !permanent)
				text.color = Color.Lerp(text.color, defColor, Time.deltaTime * 10f);
			
		}

		if (shakeOnColorChange && oldColor != text.color) {

			Shake();

		}

		oldText = text.text;
		oldColor = text.color;
		
	}

	private float StringToFloat(string stringValue, float defaultValue){

		float result = defaultValue;
		float.TryParse(stringValue, out result);
		return result;

	}

	public void Shake(){

		StartCoroutine (ShakeNum());

	}

	private IEnumerator ShakeNum(){

		float elapsed = 0.0f;

//		orgPosition = transform.position;
//		orgRotation = transform.rotation;

		while (elapsed < damageTime){
			
			elapsed += Time.deltaTime;

			float x = Random.value * shakeRange - (shakeRange /2);
			float y = Random.value * shakeRange - (shakeRange /2);
			float z = Random.value * shakeRange - (shakeRange /2);

//			transform.eulerAngles = new Vector3(originalRotation.x, originalRotation.y, originalRotation.z + z);
			transform.position = Vector3.Lerp(transform.position, new Vector3(orgPosition.x + x, orgPosition.y + y, orgPosition.z + z), Time.deltaTime * 50f);
			yield return null;

		}

		transform.position = orgPosition;
		transform.rotation = orgRotation;

	}

	void OnDisable(){

//		transform.position = orgPosition;
//		transform.rotation = orgRotation;

	}

}
