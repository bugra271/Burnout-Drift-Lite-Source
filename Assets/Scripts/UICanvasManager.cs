using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICanvasManager : MonoBehaviour {

	#region Singleton
	public static UICanvasManager instance;
	public static UICanvasManager Instance{	get{if(instance == null) instance = GameObject.FindObjectOfType<UICanvasManager>(); return instance;}}
	#endregion

	private Canvas canvas;
	private PlayerManager player;

	public GameObject gamePlayPanel;
	public GameObject gameOverPanel;
	public GameObject replayPanel;
	public GameObject optionsPanel;
	public GameObject assistanceButtons;

	public Text totalPoints;
	public Text currentPoints;
	public Text currentCoins;
	public Text currentMP;
	public Text totalDiamonds;

	public Text countDown;

	public GameObject star1;
	public GameObject star2;
	public GameObject star3;

	public float totalAward;
	public Text totalAwardText;
	public Text totalBonusText;

	public Text targetScore1;
	public Text targetScore2;
	public Text targetScore3;

	public Image targetScore1Cross;
	public Image targetScore2Cross;
	public Image targetScore3Cross;

	public Image comboPointsRadial;
	public Text comboPointsCombo;
	public GameObject comboPointsTexts;

	public Text bestScore1;
	public Text bestScore2;
	public Text bestScore3;

	public Text speeding;
	public Text jump;

	void Awake(){
		
		canvas = GetComponent<Canvas> ();
		canvas.enabled = false;

		gamePlayPanel.SetActive (false);
		gameOverPanel.SetActive (false);
		replayPanel.SetActive (false);

		star1.SetActive (false);
		star2.SetActive (false);
		star3.SetActive (false);

	}

	void OnEnable () {

		GameplayManager.OnPlayerSpawned += GameplayManager_OnPlayerSpawned;
		GameplayManager.OnRaceStarted += GameplayManager_OnRaceStarted;
		GameplayManager.OnRaceFinished += GameplayManager_OnRaceFinished;
		GameplayManager.OnRacePaused += GameplayManager_OnRacePaused;
		PlayerManager.OnDriftScoreAchieved += PlayerManager_OnScoreAchieved;

		StopAllCoroutines();
		StartCoroutine("LateDisplay");

	}

	void GameplayManager_OnRacePaused (bool state){

		optionsPanel.SetActive (state);
		assistanceButtons.SetActive(!state);
		
	}

	void PlayerManager_OnScoreAchieved (PlayerManager Player){

		List<float> allScores = new List<float> ();

		allScores.Add (GameplayManager.Instance.targetScore1);
		allScores.Add (GameplayManager.Instance.targetScore2);
		allScores.Add (GameplayManager.Instance.targetScore3);

		for (int i = 0; i < allScores.Count; i++) {

			if (Player.totalPoints >= allScores [i]) {

				switch (i) {

				case 0:
					
					if (!targetScore1Cross.gameObject.activeInHierarchy) {
						
						foreach(Image im in targetScore1.GetComponentsInChildren<Image> ()){
							im.color = Color.green;
						}

						targetScore1Cross.gameObject.SetActive (true);
						targetScore1.text += " ";

					}

					break;

				case 1:
					
					if (!targetScore2Cross.gameObject.activeInHierarchy) {
						foreach(Image im in targetScore2.GetComponentsInChildren<Image> ()){
							im.color = Color.green;
						}

						targetScore2Cross.gameObject.SetActive (true);
						targetScore2.text += " ";

					}

					break;

				case 2:
					
					if (!targetScore3Cross.gameObject.activeInHierarchy) {
						
						foreach(Image im in targetScore3.GetComponentsInChildren<Image> ()){
							im.color = Color.green;
						}

						targetScore3Cross.gameObject.SetActive (true);
						targetScore3.text += " ";

					}

					break;

				}

			}

		}
		
	}

	void GameplayManager_OnPlayerSpawned (PlayerManager Player){

		player = Player;

	}

	void GameplayManager_OnRaceFinished (float playerScore, float playerCoins, float targetScore1, float targetScore2, float targetScore3, float _bestScore1, float _bestScore2, float _bestScore3){

		gameOverPanel.SetActive (true);
		gamePlayPanel.SetActive (false);

		totalAwardText.text = playerCoins.ToString ("F0");

		List<float> allScores = new List<float> ();

		allScores.Add (targetScore3);
		allScores.Add (targetScore2);
		allScores.Add (targetScore1);

		bestScore1.text = "1st High Score: " +  _bestScore1.ToString ();
		bestScore2.text = "2nd High Score: " +  _bestScore2.ToString ();
		bestScore3.text = "3rd High Score: " +  _bestScore3.ToString ();

		bool starAchieved = false;

		for (int i = 0; i < allScores.Count; i++) {

			if (!starAchieved && playerScore >= allScores [i]) {

				switch (i) {

				case 0:
					star1.SetActive (true);
					star2.SetActive (true);
					star3.SetActive (true);
					totalBonusText.text = "Time Bonus: " + targetScore3.ToString ();
					break;

				case 1:
					star1.SetActive (true);
					star2.SetActive (true);
					star3.SetActive (false);
					totalBonusText.text = "Time Bonus: " + targetScore2.ToString ();
					break;

				case 2:
					star1.SetActive (true);
					star2.SetActive (false);
					star3.SetActive (false);
					totalBonusText.text = "Time Bonus: " + targetScore1.ToString ();
					break;

				}

				starAchieved = true;
					
			} else if(!starAchieved){

				star1.SetActive (false);
				star2.SetActive (false);
				star3.SetActive (false);
				totalBonusText.text = "No Time Bonus!";

			}
			
		}

		gameOverPanel.BroadcastMessage("Animate");
		gameOverPanel.BroadcastMessage("GetNumber");

	}

	void GameplayManager_OnRaceStarted (){

		canvas.enabled = true;
		gamePlayPanel.SetActive (true);
		gameOverPanel.SetActive (false);
		assistanceButtons.SetActive(true);

		targetScore1.gameObject.SetActive (true);
		targetScore2.gameObject.SetActive (true);
		targetScore3.gameObject.SetActive (true);

		targetScore1.text = GameplayManager.Instance.targetScore1.ToString();
		targetScore2.text = GameplayManager.Instance.targetScore2.ToString();
		targetScore3.text = GameplayManager.Instance.targetScore3.ToString();

	}

	void Update(){

		if (!player)
			return;

		if (player.curAirPonts != 0)
			jump.text = "Flying!\n" + player.curAirPonts.ToString("F0");
		else
			jump.text = "";

		if (player.curSpeedingPonts != 0)
			speeding.text = "Speed Ticket!\n" + player.curSpeedingPonts.ToString("F0");
		else
			speeding.text = "";

	}

	IEnumerator LateDisplay () {

		while(true){

			yield return new WaitForSeconds(.02f);

			if (player) {

				totalPoints.text = player.totalPoints.ToString ("F0");
				currentPoints.text = player.currentDriftPoints.ToString ("F0");
				currentCoins.text = player.currentCoins.ToString ("F0");
				currentMP.text = player.currentMP.ToString () + " X";
				totalDiamonds.text = BurnoutAPI.GetDiamond().ToString();

				if (player.driftingTime > 0) {

					if (!comboPointsRadial.gameObject.activeInHierarchy)
						comboPointsRadial.gameObject.SetActive (true);

					comboPointsRadial.fillAmount = Mathf.Lerp (0f, 1f, player.driftingTime / (Settings.Instance.driftTime));
					comboPointsCombo.text = "X" + player.currentMP.ToString ();

				} else {

					if(comboPointsRadial.gameObject.activeInHierarchy)
						comboPointsRadial.gameObject.SetActive(false);

					comboPointsRadial.fillAmount = 0f;
					comboPointsCombo.text = "";

				}

				if (player.carController.driftingNow && player.driftingTime > (Settings.Instance.driftTime - 1.5f)) {

					if (!comboPointsTexts.gameObject.activeInHierarchy)
						comboPointsTexts.gameObject.SetActive (true);

				} else {

					if (comboPointsTexts.gameObject.activeInHierarchy)
						comboPointsTexts.gameObject.SetActive (false);

				}

				float timeCount = (GameplayManager.Instance.timer);

				int min;
				int sec;
				int fraction;

				min = (int)(timeCount / 60f);
				sec = (int)(timeCount % 60f);
				fraction = (int)((timeCount * 100) % 100);

				if (timeCount != -1)
					countDown.text = string.Format("{00:00}:{01:00}:{02:00}", min, sec, fraction);
				else
					countDown.text = "";

			}

		}

	}

	void OnDisable () {

		GameplayManager.OnPlayerSpawned -= GameplayManager_OnPlayerSpawned;
		GameplayManager.OnRaceStarted -= GameplayManager_OnRaceStarted;
		GameplayManager.OnRaceFinished -= GameplayManager_OnRaceFinished;
		PlayerManager.OnDriftScoreAchieved -= PlayerManager_OnScoreAchieved;
		GameplayManager.OnRacePaused -= GameplayManager_OnRacePaused;

	}

}
