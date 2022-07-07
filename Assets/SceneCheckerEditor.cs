using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SceneChecker))]
public class SceneCheckerEditor : Editor{

    SceneChecker sc;

    public override void OnInspectorGUI() {

        sc = (SceneChecker)target;
        serializedObject.Update();

        sc.mManager = FindObjectOfType<MainMenuManager>();
        sc.mHandler = FindObjectOfType<HR_ModHandler>();
        sc.cam = FindObjectOfType<ImageEffectsOptions>();

        if (!sc.mManager)
            EditorGUILayout.HelpBox("MainMenuManager Not Found", MessageType.Error);
        else {
            if(sc.mManager.spawnPoint == null)
                EditorGUILayout.HelpBox("Spawn Point Not Found", MessageType.Error);
        }

        if (!sc.mHandler)
            EditorGUILayout.HelpBox("HR_ModManagerNot Found", MessageType.Error);

        if (!sc.cam)
            EditorGUILayout.HelpBox("Showroom Camera Not Found", MessageType.Error);

        serializedObject.ApplyModifiedProperties();

    }
}
