using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OptionsManager : MonoBehaviour {

	#region SINGLETON PATTERN
	public static OptionsManager _instance;
	public static OptionsManager Instance
	{
		get
		{
			if (_instance == null){
				_instance = GameObject.FindObjectOfType<OptionsManager>();
			}

			return _instance;
		}
	}
	#endregion

	public Toggle touch;
	public Toggle tilt;
	public Toggle joystick;
	public Toggle low;
	public Toggle med;
	public Toggle high;
	public Toggle bloom;
	public Toggle lensflare;
	public Toggle motionBlur;
	public Toggle ambientOcclusion;
	public Toggle antiAliasing;
	public Toggle shadows;
	public Toggle RTReflections;
	public Toggle SSR;
	public Slider drawDistance;

	public Slider masterVolume;
	public Slider musicVolume;

	public delegate void OptionsChanged();
	public static event OptionsChanged OnOptionsChanged;

	void OnEnable(){

		if (touch && tilt && joystick) {

			if (RCC_Settings.Instance.mobileControllerEnabled) {

				if (PlayerPrefs.GetInt("ControllerType", 0) == 0) {
					touch.isOn = true;
					tilt.isOn = false;
					joystick.isOn = false;
				}
				if (PlayerPrefs.GetInt("ControllerType", 0) == 1) {
					touch.isOn = false;
					tilt.isOn = true;
					joystick.isOn = false;
				}
				if (PlayerPrefs.GetInt("ControllerType", 0) == 3) {
					touch.isOn = false;
					tilt.isOn = false;
					joystick.isOn = true;
				}

			}

		}

        if (QualitySettings.GetQualityLevel () == 0) {
			low.isOn = true;
			high.isOn = false;
			med.isOn = false;
		}
		if (QualitySettings.GetQualityLevel () == 1) {
			low.isOn = false;
			high.isOn = false;
			med.isOn = true;
		}
		if (QualitySettings.GetQualityLevel () == 2) {
			low.isOn = false;
			high.isOn = true;
			med.isOn = false;
		}

		if(lensflare)
			lensflare.isOn = RCC_PlayerPrefsX.GetBool ("LensFlare", true);
		if(bloom)
			bloom.isOn = RCC_PlayerPrefsX.GetBool ("Bloom", true);
		if(motionBlur)
			motionBlur.isOn = RCC_PlayerPrefsX.GetBool ("MotionBlur", true);
		if(ambientOcclusion)
			ambientOcclusion.isOn = RCC_PlayerPrefsX.GetBool ("AO", true);
		if(antiAliasing)
			antiAliasing.isOn = RCC_PlayerPrefsX.GetBool ("AntiAliasing", true);
		if(shadows)
			shadows.isOn = RCC_PlayerPrefsX.GetBool ("Shadows", false);
		if(RTReflections)
			RTReflections.isOn = RCC_PlayerPrefsX.GetBool ("RTReflections", false);
		if(SSR)
			SSR.isOn = RCC_PlayerPrefsX.GetBool ("SSR", false);
		if(drawDistance)
			drawDistance.value = PlayerPrefs.GetInt ("DrawDistance", 300);

		masterVolume.value = PlayerPrefs.GetFloat ("MasterVolume", 1f);
		musicVolume.value = PlayerPrefs.GetFloat ("MusicVolume", 1f);

	}

	public void OnUpdate(){

		if(OnOptionsChanged != null)
			OnOptionsChanged ();

	}

	public void SetControllerType(Toggle toggle){

		if (toggle.isOn) {

			toggle.isOn = false;
			return;

		}

		switch(toggle.name){

		case "Touchscreen":
			PlayerPrefs.SetInt ("ControllerType", 0);
			RCC.SetMobileController(RCC_Settings.MobileController.TouchScreen);
			touch.isOn = true;
			tilt.isOn = false;
			joystick.isOn = false;
			break;
		case "Accelerometer":
			PlayerPrefs.SetInt ("ControllerType", 1);
			RCC.SetMobileController(RCC_Settings.MobileController.Gyro);
			touch.isOn = false;
			tilt.isOn = true;
			joystick.isOn = false;
			break;
		case "SteeringWheel":
			PlayerPrefs.SetInt ("ControllerType", 2);
			RCC.SetMobileController(RCC_Settings.MobileController.SteeringWheel);
			break;
		case "Joystick":
			PlayerPrefs.SetInt ("ControllerType", 3);
			RCC.SetMobileController(RCC_Settings.MobileController.Joystick);
			touch.isOn = false;
			tilt.isOn = false;
			joystick.isOn = true;
			break;

		}

		if(OnOptionsChanged != null)
			OnOptionsChanged ();

	}

	public void SetMasterVolume(Slider slider){

		PlayerPrefs.SetFloat ("MasterVolume", slider.value);

		if(OnOptionsChanged != null)
			OnOptionsChanged ();

	}

	public void SetMusicVolume(Slider slider){

		PlayerPrefs.SetFloat ("MusicVolume", slider.value);

		if(OnOptionsChanged != null)
			OnOptionsChanged ();

	}

	public void SetQuality(Toggle toggle){

		if (toggle.isOn) {
			toggle.isOn = false;
			return;
		}
		
		switch(toggle.name){

		case "Low":
			QualitySettings.SetQualityLevel (0);
			high.isOn = false;
			med.isOn = false;
			break;
		case "Medium":
			QualitySettings.SetQualityLevel (1);
			low.isOn = false;
			high.isOn = false;
			break;
		case "High":
			QualitySettings.SetQualityLevel (2);
			low.isOn = false;
			med.isOn = false;
			break;

		}

		if(OnOptionsChanged != null)
			OnOptionsChanged ();

	}

	public void SetLensFlare(Toggle toggle){
		
		RCC_PlayerPrefsX.SetBool ("LensFlare", toggle.isOn);

		if(OnOptionsChanged != null)
			OnOptionsChanged ();

	}

	public void SetBloom(Toggle toggle){

		RCC_PlayerPrefsX.SetBool ("Bloom", toggle.isOn);

		if(OnOptionsChanged != null)
			OnOptionsChanged ();

	}

	public void SetMotionBlur(Toggle toggle){

		RCC_PlayerPrefsX.SetBool ("MotionBlur", toggle.isOn);

		if(OnOptionsChanged != null)
			OnOptionsChanged ();

	}

	public void SetAO(Toggle toggle){

		RCC_PlayerPrefsX.SetBool ("AO", toggle.isOn);

		if(OnOptionsChanged != null)
			OnOptionsChanged ();

	}

	public void SetAntiAliasing(Toggle toggle){

		RCC_PlayerPrefsX.SetBool ("AntiAliasing", toggle.isOn);

		if(OnOptionsChanged != null)
			OnOptionsChanged ();

	}

	public void SetShadows(Toggle toggle){

		RCC_PlayerPrefsX.SetBool ("Shadows", toggle.isOn);

		if(OnOptionsChanged != null)
			OnOptionsChanged ();

	}

	public void SetRTReflections(Toggle toggle){

		RCC_PlayerPrefsX.SetBool ("RTReflections", toggle.isOn);

		if(OnOptionsChanged != null)
			OnOptionsChanged ();

	}

	public void SetSSR(Toggle toggle){

		RCC_PlayerPrefsX.SetBool ("SSR", toggle.isOn);

		if(OnOptionsChanged != null)
			OnOptionsChanged ();

	}

	public void SetDrawDistance(Slider slider){

		PlayerPrefs.SetInt ("DrawDistance", (int)slider.value);

		if(OnOptionsChanged != null)
			OnOptionsChanged ();

	}

	public void QuitGame(){

		Application.Quit ();

	}

}
