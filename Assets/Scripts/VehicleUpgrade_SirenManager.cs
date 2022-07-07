using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleUpgrade_SirenManager : MonoBehaviour {

    public VehicleUpgrade_Siren[] sirens;
    private int selectedIndex = -1;

    private void OnEnable() {

        CheckUpgrades();

    }

    public void CheckUpgrades() {

        for (int i = 0; i < sirens.Length; i++)
            sirens[i].gameObject.SetActive(false);

        selectedIndex = PlayerPrefs.GetInt(transform.root.name + "SelectedSiren", -1);

        if (selectedIndex != -1)
            sirens[selectedIndex].gameObject.SetActive(true);

    }

    public void Upgrade(int index) {

        selectedIndex = index;

        for (int i = 0; i < sirens.Length; i++)
            sirens[i].gameObject.SetActive(false);

        sirens[index].gameObject.SetActive(true);

        PlayerPrefs.SetInt(transform.root.name + sirens[index].transform.name, 1);
        PlayerPrefs.SetInt(transform.root.name + "SelectedSiren", index);

    }

}
