//----------------------------------------------
//           	   Highway Racer
//
// Copyright © 2016 BoneCracker Games
// http://www.bonecrackergames.com
//
//----------------------------------------------

using UnityEngine;
using System.Collections;

public static class HR_BoundsExtension{

	public static bool ContainBounds(Transform t, Bounds bounds, Bounds target){

		if(bounds.Contains(target.ClosestPoint(t.position))){
			return true;
		}

		return false;

	}

}
