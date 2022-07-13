using UnityEngine;
using System.Collections;

public class ImageEffectsOptions : MonoBehaviour {

    #region SINGLETON PATTERN
    public static ImageEffectsOptions _instance;
    public static ImageEffectsOptions Instance {
        get {
            if (_instance == null) {
                _instance = GameObject.FindObjectOfType<ImageEffectsOptions>();
            }

            return _instance;
        }
    }
    #endregion

    void Start() {

        Check();

    }

    public void Check() {

        int drawD = PlayerPrefs.GetInt("DrawDistance", 300);
        Camera.main.farClipPlane = drawD;

    }

    void OnEnable() {

        OptionsManager.OnOptionsChanged += OptionsManager_OnOptionsChanged;

    }

    public void OptionsManager_OnOptionsChanged() {

        Check();

    }

    void OnDisable() {

        OptionsManager.OnOptionsChanged -= OptionsManager_OnOptionsChanged;

    }

}
