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
public class Settings : ScriptableObject {
	
	#region singleton
	public static Settings instance;
	public static Settings Instance{	get{if(instance == null) instance = Resources.Load("Settings") as Settings; return instance;}}
	#endregion

	public int driftPointsMP = 1;
	public float driftTime = 3f;
	public bool resetDriftPointsAfterCollision = true;
	public float minimumCollision = 5f;

	public AudioClip countingPointsAudioClip;
	public AudioClip labelSlideAudioClip;
	public AudioClip[] crashedAudioClips;

}
