using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class GameplayManager : MonoBehaviour {

    #region Singleton
    public static GameplayManager instance;
    public static GameplayManager Instance { get { if (instance == null) instance = GameObject.FindObjectOfType<GameplayManager>(); return instance; } }
    #endregion

    internal RCC_CarControllerV3 currentPlayerCar;
    private int selectedPlayerCarIndex = 0;
    public Transform spawnPoint;

    public GameType gameType;
    public enum GameType { CountDown, FreeRide }

    public bool raceStarted = false;
    public float countDown = 0f;
    public GameObject introCamera;

    public float targetTime = 100f;
    public float timer = 0f;

    public float targetScore3 = 10000f;
    public float targetScore2 = 5000f;
    public float targetScore1 = 2500f;

    public List<int> bestScore = new List<int>();

    public float speedLimit = 35f;
    internal float defSpeedLimit = 35f;

    public bool isNight = false;
    public bool record = true;

    #region Events

    public delegate void onPlayerSpawned(PlayerManager Player);
    public static event onPlayerSpawned OnPlayerSpawned;

    public delegate void onRaceStarted();
    public static event onRaceStarted OnRaceStarted;

    public delegate void onRaceFinished(float playerScore, float playerCoins, float targetScore1, float targetScore2, float targetScore3, float bestScore1, float bestScore2, float bestScore3);
    public static event onRaceFinished OnRaceFinished;

    public delegate void onRacePaused(bool state);
    public static event onRacePaused OnRacePaused;

    #endregion

    void Awake() {

        Time.timeScale = 1;
        AudioListener.pause = false;
        defSpeedLimit = speedLimit;

    }

    void Start() {

        StartGame();

    }

    public void StartGame() {

        timer = targetTime;
        selectedPlayerCarIndex = PlayerPrefs.GetInt("SelectedPlayerCarIndex", 0);
        AudioListener.volume = PlayerPrefs.GetFloat("MasterVolume", 1f);

        SpawnPlayer();
        StartCoroutine(StartRace());

    }

    void Update() {

        if (!raceStarted)
            return;

        if (gameType == GameType.CountDown) {

            timer -= Time.deltaTime;

            if (timer <= 0f) {

                raceStarted = false;
                FinishRace();

            }

        } else {

            timer = -1f;

        }

        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
            Pause();

    }

    private void SpawnPlayer() {

        print("Spawning player");

        currentPlayerCar = RCC.SpawnRCC(PlayerCars.Instance.playerCars[selectedPlayerCarIndex].car.GetComponent<RCC_CarControllerV3>(), spawnPoint.position, spawnPoint.rotation, false, false, false);

        if (OnPlayerSpawned != null)
            OnPlayerSpawned(currentPlayerCar.GetComponent<PlayerManager>());

        RCC_Customization.LoadStats(currentPlayerCar.GetComponent<RCC_CarControllerV3>());

        currentPlayerCar.lowBeamHeadLightsOn = isNight;

    }

    private IEnumerator IntroCamera() {

        if (introCamera) {

            RCC_SceneManager.Instance.activePlayerCamera.gameObject.transform.SetParent(introCamera.transform, false);
            RCC_SceneManager.Instance.activePlayerCamera.gameObject.transform.localPosition = Vector3.zero;
            RCC_SceneManager.Instance.activePlayerCamera.gameObject.transform.localRotation = Quaternion.identity;

            yield return new WaitForSeconds(countDown);

            RCC_SceneManager.Instance.activePlayerCamera.gameObject.transform.SetParent(null, true);

        }

        yield return null;

    }

    private IEnumerator StartRace() {

        print("Race Started");

        StartCoroutine(IntroCamera());

        if (RCC_PlayerPrefsX.GetBool("SkipIntro", false))
            countDown = 0;

        yield return new WaitForSeconds(countDown);

        if (OnRaceStarted != null)
            OnRaceStarted();

        RCC.RegisterPlayerVehicle(currentPlayerCar, true, true);

        raceStarted = true;

        if (record)
            RCC.StartStopRecord();

        yield return new WaitForSeconds(3f);
        introCamera.GetComponentInParent<Animator>().enabled = false;

    }

    public void FinishRace() {

        print("Race Completed");

        currentPlayerCar.canControl = false;
        GameObject.FindObjectOfType<RCC_Camera>().ChangeCamera(RCC_Camera.CameraMode.FIXED);

        currentPlayerCar.GetComponent<PlayerManager>().canScore = false;

        float totalDriftPoints = currentPlayerCar.GetComponent<PlayerManager>().totalPoints;

        float currentCoins = currentPlayerCar.GetComponent<PlayerManager>().currentCoins;
        currentCoins += currentPlayerCar.GetComponent<PlayerManager>().currentDriftPoints / 10;
        currentPlayerCar.GetComponent<PlayerManager>().currentDriftPoints = 0;

        float totalPoints = PlayerPrefs.GetInt("TotalPoints");
        totalPoints += totalDriftPoints;

        PlayerPrefs.SetInt("TotalPoints", Mathf.RoundToInt(totalPoints));
        BurnoutAPI.AddCurrency((int)(currentCoins));

        bestScore.AddRange(RCC_PlayerPrefsX.GetIntArray("BestScores"));
        bestScore.Add(Mathf.CeilToInt(totalDriftPoints));
        RCC_PlayerPrefsX.SetIntArray("BestScores", bestScore.ToArray());
        bestScore.Sort();
        bestScore.Reverse();

        int[] bestScores = new int[3];

        if (bestScore != null) {

            for (int i = 0; i < Mathf.Clamp(bestScore.Count, 0, 3); i++)
                bestScores[i] = bestScore[i];

        }

        if (OnRaceFinished != null)
            OnRaceFinished(totalDriftPoints, currentCoins, targetScore1, targetScore2, targetScore3, bestScores[0], bestScores[1], bestScores[2]);

        PlayerPrefs.SetInt("BestScore", bestScores[0]);

        if (record)
            RCC.StartStopRecord();

        AdMob.ShowInterstitial();

    }

    public void Pause() {

        print("Paused");

        if (AudioListener.pause) {

            Time.timeScale = 1;
            AudioListener.pause = false;

            AdMob.HideBanner();

        } else {

            Time.timeScale = 0;
            AudioListener.pause = true;

            AdMob.ShowBanner();

        }

        if (OnRacePaused != null)
            OnRacePaused(AudioListener.pause);

    }

    public void Quit() {

        print("Quitting");

        AdMob.HideBanner();

        SceneManager.LoadScene(0);

    }

    public void Restart() {

        print("Restarting");

        AdMob.HideBanner();

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }

    public void Replay() {

        print("Replaying");

        if (!record)
            return;

        UICanvasManager.Instance.gameOverPanel.SetActive(false);
        UICanvasManager.Instance.replayPanel.SetActive(true);
        RCC_SceneManager.Instance.activePlayerCamera.useAutoChangeCamera = true;
        RCC_SceneManager.Instance.activePlayerCamera.autoChangeCameraTimer = Random.Range(0f, 10f);
        RCC.StartStopReplay();

        AdMob.ShowBanner();

    }

}
