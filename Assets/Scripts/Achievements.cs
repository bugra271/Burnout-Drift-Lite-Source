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
public class Achievements : ScriptableObject {
	
	#region singleton
	public static Achievements instance;
	public static Achievements Instance{	get{if(instance == null) instance = Resources.Load("Achievements") as Achievements; return instance;}}
	#endregion

	[System.Serializable]
	public class Achievement{

		public string achievementName = "Achievement Name";

		public string achievementIDGP = "Achievement ID GP";
		public string achievementIDNet = "Achievement ID Net";
		public string achievementGA = "Achievement GameArter";
		public string achievementSteam = "Achievement Steam";

	}

	public Achievement[] achievement;

}
