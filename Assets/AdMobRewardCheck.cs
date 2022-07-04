using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdMobRewardCheck : MonoBehaviour{


    //Auto-called AFTER onRewardedVideoClosed is called. This method is documented on Unity website.
    void OnApplicationPause(bool pause) {
        print("123");
        if (AdMob.EligibleReward && !pause) {
            print("456");
            GiveDiamonds.Instance.Give();
            AdMob.EligibleReward = false;

        }

    }

}
