//----------------------------------------------
//            Realistic Car Controller
//
// Copyright © 2016 BoneCracker Games
// http://www.bonecrackergames.com
//
//----------------------------------------------

using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(PlayerCars))]
public class PlayerCarsEditor : Editor {

    PlayerCars asset;
    List<PlayerCars.SelectablePlayerCars> playerCars = new List<PlayerCars.SelectablePlayerCars>();
    Color orgColor;
    Vector2 scrollPos;

    public override void OnInspectorGUI() {

        serializedObject.Update();
        asset = (PlayerCars)target;
        orgColor = GUI.color;
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, false, false);
        EditorGUIUtility.labelWidth = 80f;

        GUILayout.Label("Player Cars", EditorStyles.boldLabel);

        EditorGUI.indentLevel++;

        if (asset.playerCars != null && asset.playerCars.Length >= 1) {

            for (int i = 0; i < asset.playerCars.Length; i++) {

                EditorGUILayout.Space();

                EditorGUILayout.BeginVertical(GUI.skin.box);

                if (asset.playerCars[i].car)
                    EditorGUILayout.LabelField(asset.playerCars[i].car.name, EditorStyles.boldLabel);

                EditorGUILayout.Space();
                asset.playerCars[i].car = (GameObject)EditorGUILayout.ObjectField("Player Car Prefab", asset.playerCars[i].car, typeof(GameObject), false, GUILayout.MaxWidth(475f));
                EditorGUILayout.Space();

                if (asset.playerCars[i].car && asset.playerCars[i].car.GetComponent<RCC_CarControllerV3>()) {



                } else {

                    EditorGUILayout.HelpBox("Select A RCC Based Car", MessageType.Error);

                }

                EditorGUILayout.Space();
                EditorGUILayout.BeginHorizontal();

                if (asset.playerCars != null && asset.playerCars[i] != null && asset.playerCars[i].car) {

                    asset.playerCars[i].price = EditorGUILayout.IntField("Price", asset.playerCars[i].price, GUILayout.MaxWidth(150f));
                    asset.playerCars[i].diamond = EditorGUILayout.IntField("Diamond", asset.playerCars[i].diamond, GUILayout.MaxWidth(150f));

                }

                if (GUILayout.Button("\u2191", GUILayout.MaxWidth(25f))) {
                    Up(i);
                }

                if (GUILayout.Button("\u2193", GUILayout.MaxWidth(25f))) {
                    Down(i);
                }

                GUI.color = Color.red;

                if (GUILayout.Button("X", GUILayout.MaxWidth(25f))) {
                    RemoveCar(i);
                }

                GUI.color = orgColor;

                EditorGUILayout.Space();
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.EndVertical();

            }

        }

        GUI.color = Color.cyan;

        if (GUILayout.Button("Create Player Car")) {

            AddNewCar();

        }

        GUI.color = orgColor;

        EditorGUILayout.EndScrollView();

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Created by Buğra Özdoğanlar\nBoneCrackerGames", EditorStyles.centeredGreyMiniLabel, GUILayout.MaxHeight(50f));

        serializedObject.ApplyModifiedProperties();

        if (GUI.changed)
            EditorUtility.SetDirty(asset);

    }

    void AddNewCar() {

        playerCars.Clear();
        playerCars.AddRange(asset.playerCars);
        PlayerCars.SelectablePlayerCars newCar = new PlayerCars.SelectablePlayerCars();
        playerCars.Add(newCar);
        asset.playerCars = playerCars.ToArray();
        PlayerPrefs.SetInt("SelectedPlayerCarIndex", 0);

    }

    void RemoveCar(int index) {

        playerCars.Clear();
        playerCars.AddRange(asset.playerCars);
        playerCars.RemoveAt(index);
        asset.playerCars = playerCars.ToArray();
        PlayerPrefs.SetInt("SelectedPlayerCarIndex", 0);

    }

    void Up(int index) {

        if (index <= 0)
            return;

        playerCars.Clear();
        playerCars.AddRange(asset.playerCars);

        PlayerCars.SelectablePlayerCars currentCar = playerCars[index];
        PlayerCars.SelectablePlayerCars previousCar = playerCars[index - 1];

        playerCars.RemoveAt(index);
        playerCars.RemoveAt(index - 1);

        playerCars.Insert(index - 1, currentCar);
        playerCars.Insert(index, previousCar);

        asset.playerCars = playerCars.ToArray();
        PlayerPrefs.SetInt("SelectedPlayerCarIndex", 0);

    }

    void Down(int index) {

        if (index >= asset.playerCars.Length - 1)
            return;

        playerCars.Clear();
        playerCars.AddRange(asset.playerCars);

        //		foreach(HR_PlayerCars.Cars qwe in playerCars)
        //			Debug.Log(qwe.playerCar.name);

        PlayerCars.SelectablePlayerCars currentCar = playerCars[index];
        PlayerCars.SelectablePlayerCars nextCar = playerCars[index + 1];

        playerCars.RemoveAt(index);
        playerCars.Insert(index, nextCar);

        playerCars.RemoveAt(index + 1);
        playerCars.Insert(index + 1, currentCar);

        asset.playerCars = playerCars.ToArray();
        PlayerPrefs.SetInt("SelectedPlayerCarIndex", 0);

    }

}
