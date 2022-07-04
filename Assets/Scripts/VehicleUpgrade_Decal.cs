using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ch.sycoforge.Decal;

public class VehicleUpgrade_Decal : MonoBehaviour{

    private VehicleUpgrade_DecalManager decalManager;
    private EasyDecal decal;
    public Material material;
    public int lastSelected = -1;

    private void Start() {

        decal = GetComponent <EasyDecal > ();
        decalManager = GetComponentInParent<VehicleUpgrade_DecalManager>();

        lastSelected = PlayerPrefs.GetInt(transform.root.name + transform.name, -1);

        if (lastSelected == -1) {

            decal.DecalMaterial = decalManager.nullMaterial;

        } else {

            SetDecalMaterial(GetComponentInParent<VehicleUpgrade_DecalManager>().materials[lastSelected]);

        }

    }

    public void SetDecalMaterial(Material mat) {

        decal.DecalMaterial = mat;
        PlayerPrefs.SetInt(transform.root.name + transform.name, lastSelected);

    }

    public void Clear() {

        PlayerPrefs.SetInt(transform.root.name + transform.name, -1);
        decal.DecalMaterial = decalManager.nullMaterial;

    }

}
