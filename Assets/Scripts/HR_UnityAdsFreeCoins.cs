//using UnityEngine;
//using UnityEngine.Advertisements;
//
//public class HR_UnityAdsFreeCoins : MonoBehaviour
//{
//
//	public void ShowRewardedAd()
//	{
//		if (Advertisement.IsReady("rewardedVideo"))
//		{
//			var options = new ShowOptions { resultCallback = HandleShowResult };
//			Advertisement.Show("rewardedVideo", options);
//		}
//	}
//
//	private void HandleShowResult(ShowResult result)
//	{
//		switch (result)
//		{
//
//		case ShowResult.Finished:
//			Debug.Log ("The ad was successfully shown.");
//			PlayerPrefs.SetInt ("Currency", PlayerPrefs.GetInt ("Currency") + 2500);
//			HR_InfoDisplayer.Instance.ShowInfo ("Congratulations", "You have earned 2500 coins by watching video ad!", HR_InfoDisplayer.InfoType.Rewarded);
//			gameObject.SetActive (false);
//			break;
//		case ShowResult.Skipped:
//			Debug.Log("The ad was skipped before reaching the end.");
//			HR_InfoDisplayer.Instance.ShowInfo ("Skipped Video", "The ad was skipped before reaching the end...", HR_InfoDisplayer.InfoType.UnsuccesfullRewarded);
//			break;
//		case ShowResult.Failed:
//			Debug.LogError("The ad failed to be shown.");
//			HR_InfoDisplayer.Instance.ShowInfo ("Failed Video", "The ad failed to be shown...", HR_InfoDisplayer.InfoType.UnsuccesfullRewarded);
//			break;
//
//		}
//	}
//}