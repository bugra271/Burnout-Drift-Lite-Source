using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BurnoutAPI {

	public delegate void onPlayerCoinsChanged(int changeAmount);
	public static event onPlayerCoinsChanged OnPlayerCoinsChanged;

	public static int GetCurrency () {

		return PlayerPrefs.GetInt("Currency", 10000);

	}
	
	public static void ConsumeCurrency (int consume) {

		int current = GetCurrency();

		PlayerPrefs.SetInt ("Currency", current - consume);

		if(OnPlayerCoinsChanged != null)
			OnPlayerCoinsChanged (-consume);

	}

	public static void AddCurrency (int add) {

		int current = GetCurrency();

		PlayerPrefs.SetInt ("Currency", current + add);

		if(OnPlayerCoinsChanged != null)
			OnPlayerCoinsChanged (add);

	}

	public static string[] GetUnlockedVehicles () {

		List<string> unlockedVehices = new List<string> ();

		for (int i = 0; i < PlayerCars.Instance.playerCars.Length; i++) {

			if (PlayerPrefs.HasKey (PlayerCars.Instance.playerCars [i].car.transform.name + "Owned"))
				unlockedVehices.Add(PlayerCars.Instance.playerCars [i].car.transform.name + "Owned");

		}

		return unlockedVehices.ToArray();

	}

	public static string[] GetUnlockedPaints () {

		List<string> unlockedPaints = new List<string> ();

		for (int i = 0; i < 10; i++) {

			if (PlayerPrefs.HasKey ("OwnedColor" + i.ToString()))
				unlockedPaints.Add("OwnedColor" + i.ToString());

		}

		return unlockedPaints.ToArray();

	}

	public static string[] GetUnlockedWheels () {

		List<string> unlockedWheels = new List<string> ();

			for (int i = 0; i < 50; i++) {

				if (PlayerPrefs.HasKey ("OwnedWheel" + i.ToString()))
					unlockedWheels.Add("OwnedWheel" + i.ToString());

			}

		return unlockedWheels.ToArray();

	}

}
