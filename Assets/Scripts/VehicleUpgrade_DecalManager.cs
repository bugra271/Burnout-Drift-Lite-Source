using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ch.sycoforge.Decal;

public class VehicleUpgrade_DecalManager : MonoBehaviour{

    public VehicleUpgrade_Decal[] decal;

    public Material[] materials;
    public Material nullMaterial;
    public int selectedIndex = 0;

    private void OnEnable() {

        StartCoroutine(Fix());

    }

    private IEnumerator Fix() {

        yield return new WaitForSeconds(1);

        for (int i = 0; i < decal.Length; i++) {

            EasyDecal dec = decal[i].gameObject.GetComponent<EasyDecal>();
            dec.LateBake();

        }

    }

    public void SetDecalMaterial(int index) {

        decal[selectedIndex].lastSelected = index;
        
        if (index == -1)
            decal[selectedIndex].Clear();
        else
            decal[selectedIndex].SetDecalMaterial(materials[index]);

        if(index != -1)
            PlayerPrefs.SetInt(transform.root.name + materials[index].name, 1);

        PlayerPrefs.SetInt(transform.root.name + decal[selectedIndex].transform.name + "SelectedDecal", index);

    }

}
