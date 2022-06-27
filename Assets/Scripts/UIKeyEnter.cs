using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIKeyEnter : MonoBehaviour {

	public string inputName;
	Button buttonMe;

	void Start () {
		
		buttonMe = GetComponent<Button>();

	}

	void Update() {
		
		if(Input.GetButtonDown(inputName))
		{
			buttonMe.onClick.Invoke();
		}
		
	}

}