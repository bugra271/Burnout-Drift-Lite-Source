using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeedLimitDisplayer : MonoBehaviour {

	private PlayerManager player;
	public GameObject content;
	public Slider speedLimitSlider;
	public Text currentSpeedText;
	public Text targetSpeedText;

	private ColorBlock colorb;

	public float targetSpeed = 0f;

	void OnEnable(){

		colorb = speedLimitSlider.colors;

		player = GameplayManager.Instance.currentPlayerCar.GetComponent<PlayerManager> ();

	}

	void Update(){

		if (player && speedLimitSlider) {

			targetSpeed = GameplayManager.Instance.speedLimit;

			speedLimitSlider.value = Mathf.Lerp (0f, 1f, player.speed / targetSpeed);

			colorb.disabledColor = Color.Lerp (Color.red, Color.green, speedLimitSlider.value);
			speedLimitSlider.colors = colorb;
			currentSpeedText.text = player.speed.ToString ("F0");
			targetSpeedText.text = targetSpeed.ToString ();

			if (GameplayManager.Instance.speedLimit != GameplayManager.Instance.defSpeedLimit) {

				if (!content.activeInHierarchy)
					content.SetActive (true);

			} else {

				if (content.activeInHierarchy)
					content.SetActive (false);

			}

		}

	}

	public void CloseInfo(){

		content.SetActive (false);

	}

	IEnumerator CloseInfoDelayed(){

		yield return new WaitForSeconds (3);

		content.SetActive (false);

	}

}
