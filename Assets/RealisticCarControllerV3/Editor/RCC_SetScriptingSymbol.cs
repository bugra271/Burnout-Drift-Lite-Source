//----------------------------------------------
//            Realistic Car Controller
//
// Copyright © 2014 - 2022 BoneCracker Games
// http://www.bonecrackergames.com
// Buğra Özdoğanlar
//
//----------------------------------------------

using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;

public class RCC_SetScriptingSymbol{
	
	private static BuildTargetGroup[] buildTargetGroups = new BuildTargetGroup[]{

		BuildTargetGroup.Standalone,
		BuildTargetGroup.Android,
		BuildTargetGroup.iOS,
		BuildTargetGroup.WebGL,
		#if !UNITY_2019_3_OR_NEWER
		BuildTargetGroup.Facebook,
		#endif
		BuildTargetGroup.XboxOne,
		BuildTargetGroup.PS4,
		BuildTargetGroup.tvOS,

	};

	public static void SetEnabled(string defineName, bool enable){

		bool updated = false;
		
		//Debug.Log("setting "+defineName+" to "+enable);
		foreach (var group in buildTargetGroups){

			var defines = GetDefinesList(group);
			
			if (enable){

				if (!defines.Contains(defineName)) {

					defines.Add(defineName);
					updated = true;

				}

			}else{

				if (defines.Contains(defineName)) {

					while (defines.Contains(defineName))
						defines.Remove(defineName);

					updated = true;

				}
			
			}

			if (updated) {

				string definesString = string.Join(";", defines.ToArray());
				PlayerSettings.SetScriptingDefineSymbolsForGroup(group, definesString);

			}

		}

	}

	public static List<string> GetDefinesList(BuildTargetGroup group){
		
		return new List<string>(PlayerSettings.GetScriptingDefineSymbolsForGroup(group).Split(';'));

	}

}
