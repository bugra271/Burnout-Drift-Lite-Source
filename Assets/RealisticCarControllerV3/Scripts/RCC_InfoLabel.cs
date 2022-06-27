//----------------------------------------------
//            Realistic Car Controller
//
// Copyright © 2014 - 2022 BoneCracker Games
// http://www.bonecrackergames.com
// Buğra Özdoğanlar
//
//----------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles RCC Canvas dashboard elements.
/// </summary>
[AddComponentMenu("BoneCracker Games/Realistic Car Controller/UI/RCC UI Info Displayer")]
[RequireComponent (typeof(Text))]
public class RCC_InfoLabel : RCC_Singleton<RCC_InfoLabel> {

	private Text text;
	private float timer = 1f;

	void Start () {

		text = GetComponent<Text> ();
		text.enabled = false;
		
	}

	void Update(){

		if (timer < 1.5f) {
			
			if (!text.enabled)
				text.enabled = true;
			
		} else {
			
			if (text.enabled)
				text.enabled = false;
			
		}

		timer += Time.deltaTime;

	}

	public void ShowInfo (string info) {

		if (!text)
			return;

		text.text = info;
		timer = 0f;

//		StartCoroutine (ShowInfoCo(info, time));
		
	}

	IEnumerator ShowInfoCo(string info, float time){

		text.enabled = true;
		text.text = info;
		yield return new WaitForSeconds (time);
		text.enabled = false;

	}

}
