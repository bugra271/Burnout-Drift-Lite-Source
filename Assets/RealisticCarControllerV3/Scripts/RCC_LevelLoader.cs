//----------------------------------------------
//            Realistic Car Controller
//
// Copyright © 2014 - 2022 BoneCracker Games
// http://www.bonecrackergames.com
// Buğra Özdoğanlar
//
//----------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[AddComponentMenu("BoneCracker Games/Realistic Car Controller/Misc/RCC Level Loader")]
public class RCC_LevelLoader : MonoBehaviour {

	public void LoadLevel (string levelName) {

		SceneManager.LoadScene (levelName);
		
	}

}
