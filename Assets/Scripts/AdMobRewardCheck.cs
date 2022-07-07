using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdMobRewardCheck : MonoBehaviour{

    private static AdMobRewardCheck _Instance;
    public static AdMobRewardCheck Instance {

        get {

            if (_Instance == null) {

                GameObject go = new GameObject("AdMobRewardedChecker");
                go.transform.position = Vector3.zero;
                _Instance = go.AddComponent<AdMobRewardCheck>();

            }

            return _Instance;

        }

    }

    //Auto-called AFTER onRewardedVideoClosed is called. This method is documented on Unity website.
    void OnApplicationPause(bool pause) {
        
        if (AdMob.EligibleReward && !pause) {

            AdMob.EligibleReward = false;
            HR_InfoDisplayer.Instance.ShowInfo("Free Diamond Earned!", "You've earned a diamond by watching an ad!", HR_InfoDisplayer.InfoType.Rewarded);
            BurnoutAPI.AddDiamond(1);

        }

    }

}
