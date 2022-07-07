using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup_Diamond : MonoBehaviour {

    public GameObject content;
    public float cooldown = 300f;

    void OnTriggerEnter(Collider col) {

        PlayerManager player = col.gameObject.GetComponentInParent<PlayerManager>();

        if (!player)
            return;

        content.SetActive(false);
        HR_InfoDisplayer.Instance.ShowInfo("", "1 Diamond Found!", HR_InfoDisplayer.InfoType.Rewarded);
        BurnoutAPI.AddDiamond(1);
        StartCoroutine(Cooldown());

    }

    private IEnumerator Cooldown() {

        yield return new WaitForSeconds(cooldown);
        content.SetActive(true);

    }

}
