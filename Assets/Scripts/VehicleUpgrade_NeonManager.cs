using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleUpgrade_NeonManager : MonoBehaviour {

    public VehicleUpgrade_Neon[] neon;
    private int selectedIndex = -1;

    public string red = "Neon_Red";
    public string green = "Neon_Green";
    public string blue = "Neon_Blue";
    public string white = "Neon_White";
    public string orange = "Neon_Orange";
    public string yellow = "Neon_Yellow";

    private void OnEnable() {

        CheckUpgrades();

    }

    public void CheckUpgrades() {

        for (int i = 0; i < neon.Length; i++)
            neon[i].gameObject.SetActive(false);

        selectedIndex = PlayerPrefs.GetInt(transform.root.name + "SelectedNeon", -1);

        if (selectedIndex != -1)
            neon[selectedIndex].gameObject.SetActive(true);

    }

    public void Upgrade(int index) {

        selectedIndex = index;

        for (int i = 0; i < neon.Length; i++)
            neon[i].gameObject.SetActive(false);

        neon[index].gameObject.SetActive(true);

        switch (selectedIndex) {

            case 0:
                PlayerPrefs.SetInt(transform.root.name + red, 1);
                break;

            case 1:
                PlayerPrefs.SetInt(transform.root.name + green, 1);
                break;

            case 2:
                PlayerPrefs.SetInt(transform.root.name + blue, 1);
                break;

            case 3:
                PlayerPrefs.SetInt(transform.root.name + yellow, 1);
                break;

            case 4:
                PlayerPrefs.SetInt(transform.root.name + orange, 1);
                break;

            case 5:
                PlayerPrefs.SetInt(transform.root.name + white, 1);
                break;

        }

        PlayerPrefs.SetInt(transform.root.name + "SelectedNeon", selectedIndex);

    }

}
