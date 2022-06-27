using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISkipIntro : MonoBehaviour {

	private Toggle toggle;

	void Awake () {

		toggle = GetComponent<Toggle> ();
		
	}

	void OnEnable () {

		if (!toggle)
			return;

		toggle.isOn = RCC_PlayerPrefsX.GetBool ("SkipIntro", false);
		
	}

}
