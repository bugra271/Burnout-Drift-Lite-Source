using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class MainMenuManager : MonoBehaviour {

    #region Singleton
    public static MainMenuManager instance;
    public static MainMenuManager Instance { get { if (instance == null) instance = GameObject.FindObjectOfType<MainMenuManager>(); return instance; } }
    #endregion

    private List<GameObject> instantiatedPlayerCars = new List<GameObject>();

    internal GameObject currentCar;
    internal int selectedPlayerCarIndex = 0;

    private int playerCoins = 0;
    private int playerDiamonds = 0;

    [Header("Spawn Point")]
    public Transform spawnPoint;

    [Header("UI Menus")]
    public GameObject topAndButtomButtons;
    public GameObject entranceMenu;
    public GameObject carSelectMenu;
    public GameObject sceneSelectMenu;
    public GameObject creditsMenu;
    public GameObject optionsMenu;
    public GameObject shopMenu;
    public GameObject loadingMenu;

    [Header("UI Buttons")]
    public Button multiplayerButton;

    [Header("UI Currency")]
    public Text currencyText;
    public Text diamondsText;

    [Header("UI Buttons")]
    public GameObject buyCarButton;
    public GameObject buyCarButtonDiamond;
    public GameObject selectCarButton;
    public GameObject modCarPanel;

    [Header("UI Slides")]
    public Slider speed;
    public Slider handling;
    public Slider brake;

    [Header("UI Loading Section")]
    private AsyncOperation async;
    public Slider loadingBar;

    private static int playCount = 0;

    void Start() {

        StartGame();

    }

    void OnEnable() {

        BurnoutAPI.OnPlayerCoinsChanged += OnPlayerCoinsChanged;

    }

    void StartGame() {

        Time.timeScale = 1;
        AudioListener.pause = false;
        Application.targetFrameRate = 60;
        playerCoins = BurnoutAPI.GetCurrency();
        playerDiamonds = BurnoutAPI.GetDiamond();
        AudioListener.volume = PlayerPrefs.GetFloat("MasterVolume", 1f);
        selectedPlayerCarIndex = PlayerPrefs.GetInt("SelectedPlayerCarIndex", 0);

        CreateCars();
        SpawnPlayer();

        if (playCount > 0)
            OpenMenu(carSelectMenu);

        playCount++;

        int bestScore = 0;

        List<int> bestScores = new List<int>();
        bestScores.AddRange(RCC_PlayerPrefsX.GetIntArray("BestScores"));
        bestScores.Sort();
        bestScores.Reverse();

        if (bestScores != null && bestScores.Count >= 1)
            bestScore = bestScores[0];

        StartCoroutine(GoogleMobileAds());

    }

    private IEnumerator GoogleMobileAds() {

        yield return new WaitForSeconds(.15f);
        AdMob.Init();
        yield return new WaitForSeconds(.5f);
        AdMob.RequestBanner();
        yield return new WaitForSeconds(.5f);
        AdMob.RequestInterstitial();
        yield return new WaitForSeconds(.5f);
        AdMob.RequestRewarded();
        AdMobRewardCheck.Instance.tag = "Untagged";
        yield return new WaitForSeconds(.15f);
        AdMob.ShowBanner();

    }

    public void OnPlayerCoinsChanged() {

        playerCoins = BurnoutAPI.GetCurrency();
        playerDiamonds = BurnoutAPI.GetDiamond();

    }

    void Update() {

        currencyText.text = playerCoins.ToString();
        diamondsText.text = playerDiamonds.ToString();

        if (async != null && !async.isDone)
            loadingBar.value = async.progress;

    }

    private void CreateCars() {

        for (int i = 0; i < PlayerCars.Instance.playerCars.Length; i++) {

            RCC_CarControllerV3 car = RCC.SpawnRCC(PlayerCars.Instance.playerCars[i].car.GetComponent<RCC_CarControllerV3>(), spawnPoint.position, spawnPoint.rotation, true, false, false);
            car.gameObject.SetActive(false);
            instantiatedPlayerCars.Add(car.gameObject);
            loadingBar.value = Mathf.Lerp(0f, 1f, (float)(i) / PlayerCars.Instance.playerCars.Length);

        }

    }

    private void SpawnPlayer() {

        if (PlayerCars.Instance.playerCars[selectedPlayerCarIndex].price <= 0 && PlayerCars.Instance.playerCars[selectedPlayerCarIndex].diamond <= 0)
            PlayerPrefs.SetInt(PlayerCars.Instance.playerCars[selectedPlayerCarIndex].car.name + "Owned", 1);

        if (PlayerPrefs.GetInt(PlayerCars.Instance.playerCars[selectedPlayerCarIndex].car.name + "Owned") == 1) {

            if (buyCarButton.GetComponentInChildren<Text>())
                buyCarButton.GetComponentInChildren<Text>().text = "";

            buyCarButton.SetActive(false);
            buyCarButtonDiamond.SetActive(false);
            selectCarButton.SetActive(true);
            modCarPanel.SetActive(true);

        } else {

            selectCarButton.SetActive(false);
            modCarPanel.SetActive(false);

            if (PlayerCars.Instance.playerCars[selectedPlayerCarIndex].diamond != 0) {

                buyCarButton.SetActive(false);
                buyCarButtonDiamond.SetActive(true);

            } else {

                buyCarButton.SetActive(true);
                buyCarButtonDiamond.SetActive(false);

            }

            if (buyCarButton.GetComponentInChildren<Text>())
                buyCarButton.GetComponentInChildren<Text>().text = "BUY FOR\n" + PlayerCars.Instance.playerCars[selectedPlayerCarIndex].price.ToString("F0");

            if (buyCarButtonDiamond.GetComponentInChildren<Text>())
                buyCarButtonDiamond.GetComponentInChildren<Text>().text = "BUY FOR\n" + PlayerCars.Instance.playerCars[selectedPlayerCarIndex].diamond.ToString("F0");

        }

        for (int i = 0; i < instantiatedPlayerCars.Count; i++)
            instantiatedPlayerCars[i].gameObject.SetActive(false);

        instantiatedPlayerCars[selectedPlayerCarIndex].gameObject.SetActive(true);

        currentCar = instantiatedPlayerCars[selectedPlayerCarIndex].gameObject;
        RCC_SceneManager.Instance.activePlayerVehicle = currentCar.GetComponent<RCC_CarControllerV3>();
        GameObject.FindObjectOfType<HR_ModHandler>().ChooseClass(null);
        StartCoroutine(LoadStats());

        //UpdateStats();

    }

    private IEnumerator LoadStats() {

        yield return new WaitForSeconds(.1f);
        GameObject.FindObjectOfType<RCC_CustomizerExample>().LoadStats();

    }

    public void BuyCar() {

        if (BurnoutAPI.GetCurrency() >= PlayerCars.Instance.playerCars[selectedPlayerCarIndex].price) {

            PlayerPrefs.SetInt(PlayerCars.Instance.playerCars[selectedPlayerCarIndex].car.name + "Owned", 1);
            BurnoutAPI.ConsumeCurrency(PlayerCars.Instance.playerCars[selectedPlayerCarIndex].price);

        } else {

            HR_InfoDisplayer.Instance.ShowInfo("Not Enough Coins", "You have to earn " + (PlayerCars.Instance.playerCars[selectedPlayerCarIndex].price - BurnoutAPI.GetCurrency()).ToString() + " more coins to buy this car", HR_InfoDisplayer.InfoType.NotEnoughMoney);
            return;

        }

        SpawnPlayer();

    }

    public void BuyCarWithDiamond() {

        if (BurnoutAPI.GetDiamond() >= PlayerCars.Instance.playerCars[selectedPlayerCarIndex].diamond) {

            PlayerPrefs.SetInt(PlayerCars.Instance.playerCars[selectedPlayerCarIndex].car.name + "Owned", 1);
            BurnoutAPI.ConsumeDiamond(PlayerCars.Instance.playerCars[selectedPlayerCarIndex].diamond);

        } else {

            HR_InfoDisplayer.Instance.ShowInfo("Not Enough Daimonds", "You have to earn " + (PlayerCars.Instance.playerCars[selectedPlayerCarIndex].diamond - BurnoutAPI.GetDiamond()).ToString() + " more diamonds to buy this car", HR_InfoDisplayer.InfoType.NotEnoughMoney);
            return;

        }

        SpawnPlayer();

    }

    public void SelectPrevious() {

        if (selectedPlayerCarIndex > 0)
            selectedPlayerCarIndex--;
        else
            selectedPlayerCarIndex = PlayerCars.Instance.playerCars.Length - 1;

        SpawnPlayer();

    }

    public void SelectNext() {

        if (selectedPlayerCarIndex < PlayerCars.Instance.playerCars.Length - 1)
            selectedPlayerCarIndex++;
        else
            selectedPlayerCarIndex = 0;

        SpawnPlayer();

    }

    public void SelectCar() {

        PlayerPrefs.SetInt("SelectedPlayerCarIndex", selectedPlayerCarIndex);

    }

    public void OpenMenu(GameObject activeMenu) {

        topAndButtomButtons.SetActive(false);
        entranceMenu.SetActive(false);
        carSelectMenu.SetActive(false);
        sceneSelectMenu.SetActive(false);
        creditsMenu.SetActive(false);
        optionsMenu.SetActive(false);
        shopMenu.SetActive(false);
        loadingMenu.SetActive(false);

        if (activeMenu != null)
            activeMenu.SetActive(true);

    }

    public void SelectScene(int sceneIndex) {

        PlayerPrefs.SetInt("SelectedLevelIndex", sceneIndex);

    }

    public void OpenScene(int sceneIndex) {

        PlayerPrefs.SetInt("SelectedLevelIndex", sceneIndex);

        async = SceneManager.LoadSceneAsync(sceneIndex);

        AdMob.HideBanner();

    }

    public void OpenScene() {

        int sceneIndex = PlayerPrefs.GetInt("SelectedLevelIndex", 2);

        async = SceneManager.LoadSceneAsync(sceneIndex);

        AdMob.HideBanner();

    }

    public void Quit() {

        System.Diagnostics.Process.GetCurrentProcess().Kill();

    }

    public void SkipIntro(Toggle state) {

        RCC_PlayerPrefsX.SetBool("SkipIntro", state.isOn);

    }

    public void UpdateStats(RCC_CarControllerV3 car) {

        speed.value = Mathf.Lerp(.1f, 1f, (car.maxspeed) / 400f);
        handling.value = Mathf.Lerp(.1f, 1f, car.tractionHelperStrength / .6f);
        brake.value = Mathf.Lerp(.1f, 1f, car.brakeTorque / 5000f);

    }

    void OnDisable() {

        BurnoutAPI.OnPlayerCoinsChanged -= OnPlayerCoinsChanged;

    }

}
