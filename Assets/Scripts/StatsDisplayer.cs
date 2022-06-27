using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StatsDisplayer : MonoBehaviour {

	private PlayerManager player;

	public Text totalPoints;
	public Text currentPoints;
	public Text currentCoins;
	public Text currentMP;

	void OnEnable () {

		GameplayManager.OnPlayerSpawned += GameplayManager_OnPlayerSpawned;
	
	}

	void GameplayManager_OnPlayerSpawned (PlayerManager Player){

		player = Player;
		
	}
	
	void OnDisable () {

		GameplayManager.OnPlayerSpawned -= GameplayManager_OnPlayerSpawned;

	}

	void Update(){

		if (!player)
			return;

		totalPoints.text = player.totalDriftPoints.ToString ("F0");
		currentPoints.text = player.currentDriftPoints.ToString ("F0");
		currentCoins.text = player.currentCoins.ToString ("F0");
		currentMP.text = player.currentMP.ToString () + " X";

	}

}
