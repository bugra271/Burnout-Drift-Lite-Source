using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeCash : MonoBehaviour {

    public GameObject content;
    public float cooldown = 30f;

    void OnTriggerEnter(Collider col) {

        PlayerManager player = col.gameObject.GetComponentInParent<PlayerManager>();

        if (!player)
            return;

        content.SetActive(false);
        RewardedAdsPanel.Instance.Show();
        StartCoroutine(Cooldown());

    }

    private IEnumerator Cooldown() {

        yield return new WaitForSeconds(cooldown);
        content.SetActive(true);

    }

}
