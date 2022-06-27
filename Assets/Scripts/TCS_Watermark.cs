using UnityEngine;
using System.Collections;

public class TCS_Watermark : MonoBehaviour {

	public static TCS_Watermark instance;
	internal AudioSource audioSource;

	private float masterVolume = 1f;
	private float musicVolume = 1f;

	public float maxVolume = 1f;

	public AudioClip[] clips;
	public int index = 0;

	void Awake(){

		if (instance){
			Destroy (gameObject);
		}else{
			instance = this;
			DontDestroyOnLoad (gameObject);
		}

		audioSource = GetComponent<AudioSource> ();
		audioSource.ignoreListenerPause = true;

	}

	void OnEnable(){

		OptionsManager.OnOptionsChanged += OptionsManager_OnOptionsChanged;

	}

	void Start () {
		
		Check ();
		StartCoroutine (StartMusic());

	}

	void HandleOnAdsOpened (){

		print ("HandleOnAdsOpened");

		if(!audioSource.mute)
			audioSource.mute = true;
		
	}

	void HandleOnAdsClosed (){

		print ("HandleOnAdsClosed");

		if(audioSource.mute)
			audioSource.mute = false;
		
	}

	void Check(){

		masterVolume = PlayerPrefs.GetFloat ("MasterVolume", 1f);
		musicVolume = PlayerPrefs.GetFloat ("MusicVolume", 1f);

		AudioListener.volume = masterVolume;
		audioSource.volume = musicVolume * maxVolume;

	}

	IEnumerator StartMusic(){

		yield return new WaitForSeconds (1);

		for (int i = 0; i < clips.Length; i++) {

			audioSource.clip = clips [i];
			audioSource.Play ();
			yield return new WaitForSeconds (clips[i].length);

		}

		StartCoroutine ("StartMusic");

	}

	void OptionsManager_OnOptionsChanged (){

		Check ();

	}

	void OnDisable(){

		StopCoroutine (StartMusic());
		OptionsManager.OnOptionsChanged -= OptionsManager_OnOptionsChanged;

	}

}
