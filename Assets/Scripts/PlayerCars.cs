//----------------------------------------------
//            Realistic Car Controller
//
// Copyright © 2016 BoneCracker Games
// http://www.bonecrackergames.com
//
//----------------------------------------------

using UnityEngine;
using System.Collections;

/// <summary>
/// Stored all general shared RCC settings here.
/// </summary>
[System.Serializable]
public class PlayerCars : ScriptableObject {
	
	#region singleton
	public static PlayerCars instance;
	public static PlayerCars Instance{	get{if(instance == null) instance = Resources.Load("PlayerCars") as PlayerCars; return instance;}}
	#endregion

	[System.Serializable]
	public class SelectablePlayerCars{

		public GameObject car;
		public int price;

		public CarType carType;
		public enum CarType {Racing, Offroad}

	}

	public SelectablePlayerCars[] playerCars;

}
