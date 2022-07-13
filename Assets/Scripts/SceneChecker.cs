using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneChecker : MonoBehaviour {

    private static SceneChecker _sceneChecker;
    public static SceneChecker Instance {

        get {

            if(_sceneChecker == null) {

                GameObject go = new GameObject("Scene Checker");
                _sceneChecker = go.AddComponent<SceneChecker>();

            }

            return _sceneChecker;

        }
    
    }

    public MainMenuManager mManager;
    public HR_ModHandler mHandler;
    public ImageEffectsOptions cam;

    public void Check() { }

}
