using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class LoadingScene : MonoBehaviour {

	[Header("UI Loading Section")]
	private AsyncOperation async;
	public Slider loadingBar;

	void Start () {

		async = SceneManager.LoadSceneAsync (1, LoadSceneMode.Single);
	
	}
	
	// Update is called once per frame
	void Update () {

		if(async != null &&  !async.isDone){
			loadingBar.value = async.progress;
		}
	
	}
}
