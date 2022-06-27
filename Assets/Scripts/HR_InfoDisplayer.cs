using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HR_InfoDisplayer : MonoBehaviour {

	#region SINGLETON PATTERN
	public static HR_InfoDisplayer _instance;
	public static HR_InfoDisplayer Instance
	{
		get
		{
			if (_instance == null){
				_instance = GameObject.FindObjectOfType<HR_InfoDisplayer>();
			}

			return _instance;
		}
	}
	#endregion

	public InfoType infoType;
	public enum InfoType{NotEnoughMoney, Rewarded, Info}

	public GameObject notEnoughMoney;
	public GameObject reward;
	public GameObject info;

	public Text notEnoughMoneyDescText;
	public Text rewardDescText;
	public Text infoDescText;

	public Button close;

	public void ShowInfo (string title, string description, InfoType type) {

		switch (type) {

		case InfoType.NotEnoughMoney:
			notEnoughMoney.SetActive (true);
			notEnoughMoneyDescText.text = description;
			StartCoroutine ("CloseInfoDelayed");
			break;

		case InfoType.Rewarded:
			reward.SetActive (true);
			rewardDescText.text = description;
			StartCoroutine ("CloseInfoDelayed");
			break;

		case InfoType.Info:
			info.SetActive (false);
			infoDescText.text = description;
			StartCoroutine ("CloseInfoDelayed");
			break;

		}
	
	}

	public void CloseInfo(){

		notEnoughMoney.SetActive (false);

	}

	IEnumerator CloseInfoDelayed(){

		yield return new WaitForSeconds (3);

		notEnoughMoney.SetActive (false);
		reward.SetActive (false);
		info.SetActive (false);

	}

}
