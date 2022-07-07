using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleUpgrade_SpoilerManager : MonoBehaviour {

    public VehicleUpgrade_Spoiler[] spoiler;
    private int selectedIndex = -1;

    private void OnEnable() {

        CheckUpgrades();

    }

    public void CheckUpgrades() {

        for (int i = 0; i < spoiler.Length; i++)
            spoiler[i].gameObject.SetActive(false);

        selectedIndex = PlayerPrefs.GetInt(transform.root.name + "SelectedSpoiler", -1);

        if (selectedIndex != -1)
            spoiler[selectedIndex].gameObject.SetActive(true);

    }

    public void Upgrade(int index) {

        selectedIndex = index;

        for (int i = 0; i < spoiler.Length; i++)
            spoiler[i].gameObject.SetActive(false);

        spoiler[index].gameObject.SetActive(true);
        
        PlayerPrefs.SetInt(transform.root.name + spoiler[index].transform.name, 1);
        PlayerPrefs.SetInt(transform.root.name + "SelectedSpoiler", index);

    }

}
