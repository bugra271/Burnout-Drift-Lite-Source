using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StatsDisplayer : MonoBehaviour {

	private PlayerManager player;

	public Text driftPoints;
	public Text currentDriftPoints;
	public Text currentCoins;
	public Text currentDriftMP;

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

		driftPoints.text = player.totalDriftPoints.ToString ("F0");
		currentDriftPoints.text = player.currentDriftPoints.ToString ("F0");
		currentCoins.text = player.currentCoins.ToString ("F0");
		currentDriftMP.text = player.currentMP.ToString () + " X";

	}

}
