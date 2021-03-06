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
	public GameObject modificationClass;
	public GameObject upgradesClass;
	public GameObject decalsClass;
	public GameObject neonsClass;
	public GameObject spoilerClass;
	public GameObject sirenClass;

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

		if (!currentApplier)
			return;

	}

	public void ChooseClass(GameObject activeClass){

		colorClass.SetActive(false);
		wheelClass.SetActive(false);
		modificationClass.SetActive(false);
		upgradesClass.SetActive(false);
		decalsClass.SetActive(false);
		neonsClass.SetActive(false);
		spoilerClass.SetActive(false);
		sirenClass.SetActive(false);

		if (activeClass)
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

	public void UpgradeSpeed() {

		HR_ModApplier applier = GameObject.FindObjectOfType<HR_ModApplier>();
		applier.speedLevel++;
		applier.UpdateStats();

	}

	public void UpgradeHandling() {

		HR_ModApplier applier = GameObject.FindObjectOfType<HR_ModApplier>();
		applier.handlingLevel++;
		applier.UpdateStats();

	}

	public void UpgradeBrake() {

		HR_ModApplier applier = GameObject.FindObjectOfType<HR_ModApplier>();
		applier.brakeLevel++;
		applier.UpdateStats();

	}

	public void BuyProperty(int price, string prefsKey){

		PlayerPrefs.SetInt (prefsKey, 1);
		BurnoutAPI.ConsumeCurrency (price);

	}

	public void ToggleAutoRotation(bool state) {

		Camera.main.gameObject.GetComponent<bl_CameraOrbit>().ToggleAutoRotation(state);

	}

	public void SetHorizontal(float hor) {

		Camera.main.gameObject.GetComponent<bl_CameraOrbit>().horizontal = hor;

	}

	public void SetVertical(float ver) {

		Camera.main.gameObject.GetComponent<bl_CameraOrbit>().vertical = ver;

	}

	public void SetDecalIndex(int index) {

		VehicleUpgrade_DecalManager dm = currentApplier.GetComponentInChildren<VehicleUpgrade_DecalManager>();

		dm.selectedIndex = index;

	}

}
