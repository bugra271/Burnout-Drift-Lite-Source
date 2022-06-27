using UnityEngine;
using System.Collections;

[System.Serializable]
public class SelectableWheels : ScriptableObject {

	public static SelectableWheels instance;
	public static SelectableWheels Instance
	{
		get
		{
			if(instance == null)
				instance = Resources.Load("SelectableWheels") as SelectableWheels;
			return instance;
		}
		
	}

	[System.Serializable]
	public class Wheels{

		public GameObject wheel;
		public int price;

	}

	public Wheels[] wheels;

}
