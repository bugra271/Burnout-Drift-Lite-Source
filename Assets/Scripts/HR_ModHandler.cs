//Modification Script For Checking/Applying User Modifications. Also Controls UI Buttons, Sliders and Texts About Modding

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HR_ModHandler : MonoBehaviour {

	//Classes
	private RCC_CarControllerV3 currentCar;
	private HR_ModApplier currentApplier;

	//UI Panels
	[Header("Modify Panels")]
	public GameObject colorClass;
	public GameObject wheelClass;
	public GameObject upgradesClass;

	//UI Buttons
	[Header("Modify Buttons")]
	public Button bodyPaintButton;
	public Button rimButton;
	public Button upgradeButton;
	private Color orgButtonColor;

	void Awake () {

		orgButtonColor = bodyPaintButton.image.color;
	
	}
	
	void Update(){

		currentCar = MainMenuManager.Instance.currentCar.GetComponent<RCC_CarControllerV3>();
		currentApplier = currentCar.GetComponent<HR_ModApplier>();

	}

	public void ChooseClass(GameObject activeClass){

		colorClass.SetActive(false);
		wheelClass.SetActive(false);
		upgradesClass.SetActive(false);

		if(activeClass)
			activeClass.SetActive(true);

	}
	
	public void CheckButtonColors(Button activeButton){
		
		bodyPaintButton.image.color = orgButtonColor;
		rimButton.image.color = orgButtonColor;
		upgradeButton.image.color = orgButtonColor;
		
		activeButton.image.color = new Color(.65f, 1f, 0f);
		
	}

	public void ChangeChassisColor (Color color) {

		HR_ModApplier applier = GameObject.FindObjectOfType<HR_ModApplier>();
		applier.bodyColor = color;
		applier.UpdateStats();
		
	}

	public void ChangeWheels (int wheelIndex) {
		
		HR_ModApplier applier = GameObject.FindObjectOfType<HR_ModApplier>();
		applier.selectedWheel = SelectableWheels.Instance.wheels[wheelIndex].wheel;
		applier.wheelIndex = wheelIndex;
		applier.UpdateStats();
		
	}

	public void BuyProperty(int price, string prefsKey){

		PlayerPrefs.SetInt (prefsKey, 1);
		BurnoutAPI.ConsumeCurrency (price);

	}

}
