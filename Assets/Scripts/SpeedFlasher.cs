using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedFlasher : MonoBehaviour {

    public GameObject content;
    public Animator animator;

    public float points = 5000f;
    public float minSpeed = 100f;
    public float activeTime = 30f;

    public AudioClip audio;

    // Update is called once per frame
    void OnTriggerEnter(Collider col) {

        PlayerManager player = col.transform.GetComponentInParent<PlayerManager>();

        if (!player)
            return;

        if (player.speed < minSpeed)
            return;

        animator.SetTrigger("Flash");
        content.SetActive(false);
        player.curSpeedingPonts += points;
        RCC_Core.NewAudioSource(gameObject, audio.name, 0f, 0f, 1f, audio, false, true, true);
        StartCoroutine(Activate(activeTime));

    }

    private IEnumerator Activate(float timer) {

        yield return new WaitForSeconds(timer);
        content.SetActive(true);

    }

}
