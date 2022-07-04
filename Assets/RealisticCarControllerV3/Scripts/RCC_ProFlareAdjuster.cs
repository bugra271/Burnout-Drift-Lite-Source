//----------------------------------------------
//            Realistic Car Controller
//
// Copyright © 2014 - 2021 BoneCracker Games
// http://www.bonecrackergames.com
// Buğra Özdoğanlar
//
//----------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("BoneCracker Games/Realistic Car Controller/Pro Flare/RCC Pro Flare Adjuster")]
public class RCC_ProFlareAdjuster : MonoBehaviour{

	private Light _light;
    private ProFlare proFlare;
    private float defaultScale = 1f;

    public float flareMultiplier = 1f;

    public bool changeScale = true;
    public bool changeColor = true;

    void Start(){

        _light = GetComponent<Light>();
        proFlare = GetComponentInChildren<ProFlare>();
        defaultScale = proFlare.GlobalScale;
        
    }

    void Update(){

        if (!proFlare || !_light)
            return;

        if(changeScale)
            proFlare.GlobalScale = defaultScale * _light.intensity * flareMultiplier;

        if(changeColor)
            proFlare.GlobalTintColor = new Color(_light.color.r, _light.color.g, _light.color.b, 1f) * _light.intensity * flareMultiplier;
        
    }

}
