using UnityEngine;
using System.Collections;

[RequireComponent(typeof(HR_ModApplier))]
public class PlayerManager : MonoBehaviour {

    internal RCC_CarControllerV3 carController;

    public float totalPoints {

        get {
            return totalDriftPoints + currentDriftPoints + speedingPoints + airPoints;
        }

    }

    public Vector3 localVelocity = new Vector3(0, 0, 0);

    public float totalDriftPoints = 0;
    public float currentDriftPoints = 0;
    public int currentCoins = 0;
    public int currentMP = 1;

    public float driftingTime = 0f;
    public bool canScore = true;

    public float speedingPoints = 0f;
    public float airPoints = 0f;

    public float airTime = 0f;
    public float ticketTime = 0f;

    public float curSpeedingPonts = 0f;
    public float curAirPonts = 0f;

    public float speed = 0f;

    public delegate void onDriftScoreAchieved(PlayerManager Player);
    public static event onDriftScoreAchieved OnDriftScoreAchieved;

    void Awake() {

        carController = GetComponent<RCC_CarControllerV3>();

    }

    void NoDriftZone_OnRedZoneEntered(PlayerManager player, float targetSpeed) {

        GameplayManager.Instance.speedLimit = targetSpeed;

    }

    void NoDriftZone_OnRedZoneExited(PlayerManager player) {

        GameplayManager.Instance.speedLimit = GameplayManager.Instance.defSpeedLimit;

    }

    void Update() {

        if (!carController.canControl)
            return;

        if (!GameplayManager.Instance)
            return;

        if (canScore && carController.speed >= GameplayManager.Instance.speedLimit && carController.driftingNow) {

            driftingTime += Time.deltaTime;

            if (driftingTime >= Settings.Instance.driftTime)
                currentDriftPoints += (Settings.Instance.driftPointsMP * currentMP) * Time.deltaTime;

        } else {

            driftingTime -= Time.deltaTime;

        }

        driftingTime = Mathf.Clamp(driftingTime, 0f, Settings.Instance.driftTime + 1.5f);

        if (currentDriftPoints > 0 && driftingTime < Settings.Instance.driftTime) {

            totalDriftPoints += currentDriftPoints;
            currentCoins += Mathf.RoundToInt(currentDriftPoints / 10f);
            currentDriftPoints = 0;

            if (OnDriftScoreAchieved != null)
                OnDriftScoreAchieved(this);

        }

        switch (Mathf.FloorToInt(currentDriftPoints / 1000f)) {

            case 0:
                currentMP = 1;
                break;

            case 1:
                currentMP = 2;
                break;

            case 3:
                currentMP = 3;
                break;

            case 5:
                currentMP = 4;
                break;

            case 10:
                currentMP = 5;
                break;

            case 20:
                currentMP = 10;
                break;

        }

        if (!carController.isGrounded && speed > 20f)
            airTime += Time.deltaTime;
        else
            airTime -= Time.deltaTime;

        airTime = Mathf.Clamp(airTime, 0f, 3f);

        if (airTime >= .5f) {

            if (!carController.isGrounded)
                curAirPonts += Time.deltaTime * 100f;

        } else {

            airPoints += curAirPonts;
            curAirPonts = 0f;

        }

        if (curSpeedingPonts != 0)
            ticketTime -= Time.deltaTime;
        else
            ticketTime = 3f;

        ticketTime = Mathf.Clamp(ticketTime, 0f, 3f);

        if (ticketTime > .5f) {



        } else {

            speedingPoints += curSpeedingPonts;
            curSpeedingPonts = 0f;

        }

    }

    private void FixedUpdate() {

        if (!carController)
            return;

        localVelocity = transform.InverseTransformDirection(carController.rigid.velocity);
        speed = localVelocity.z * 3.6f;

    }

    void OnCollisionEnter(Collision col) {

        Vector3 colRelVel = col.relativeVelocity;
        colRelVel *= 1f - Mathf.Abs(Vector3.Dot(transform.up, col.contacts[0].normal));

        float cos = Mathf.Abs(Vector3.Dot(col.contacts[0].normal, colRelVel.normalized));

        if (colRelVel.magnitude * cos < Settings.Instance.minimumCollision)
            return;

        if (col.gameObject.layer == LayerMask.NameToLayer("Road"))
            return;

        if (col.gameObject.layer == LayerMask.NameToLayer("Prop"))
            return;

        if (currentDriftPoints > 10) {

            AudioClip random = Settings.Instance.crashedAudioClips[Random.Range(0, Settings.Instance.crashedAudioClips.Length)];
            CreateAudioSource.NewAudioSource(gameObject, "Crashed SFX", 0f, 0f, 1f, random, false, true, true);

        }

        driftingTime = 0f;
        currentDriftPoints = 0;

    }

    void OnTriggerEnter(Collider col) {

        NoDriftZone noDriftZone = col.gameObject.GetComponent<NoDriftZone>();

        if (!noDriftZone)
            return;

        NoDriftZone_OnRedZoneEntered(this, noDriftZone.speedLimit);

    }

    void OnTriggerExit() {

        NoDriftZone_OnRedZoneExited(this);

    }

}
