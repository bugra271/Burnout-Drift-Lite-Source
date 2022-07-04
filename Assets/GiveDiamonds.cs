using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiveDiamonds : RCC_Singleton<GiveDiamonds>{

    public void Give(){

        StartCoroutine(qwe());
        
    }

    private IEnumerator qwe() {

        HR_InfoDisplayer.Instance.ShowInfo("Free Diamond Earned!", "You've earned a diamond by watching an ad!", HR_InfoDisplayer.InfoType.Rewarded);
        yield return new WaitForSeconds(.5f);
        BurnoutAPI.AddDiamond(1);

    }

}
