using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardedAdsPanel : RCC_Singleton<RewardedAdsPanel> {

    public GameObject content;

    void Start()
    {
        
    }

    // Update is called once per frame
    public void Show(){

        content.SetActive(true);
        StartCoroutine(Close());
        
    }

    private IEnumerator Close() {

        yield return new WaitForSeconds(10);
        content.SetActive(false);

    }

}
