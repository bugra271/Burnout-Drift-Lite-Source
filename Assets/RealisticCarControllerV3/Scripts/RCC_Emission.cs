//----------------------------------------------
//            Realistic Car Controller
//
// Copyright © 2014 - 2022 BoneCracker Games
// http://www.bonecrackergames.com
// Buğra Özdoğanlar
//
//----------------------------------------------

using UnityEngine;
using System.Collections;

[System.Serializable]
public class RCC_Emission {

    public Renderer lightRenderer;

    public int materialIndex = 0;
    public bool noTexture = false;
    public bool applyAlpha = false;
    [Range(.1f, 10f)]public float multiplier = 1f;
    private int emissionColorID;
    private int colorID;

    private Material material;
    private Color targetColor;

    private bool initialized = false;

    public void Init() {

        if (!lightRenderer) {

            Debug.LogError("No renderer selected for emission! Selected a renderer for this light, or disable emission.");
            return;

        }

        material = lightRenderer.materials[materialIndex];
        material.EnableKeyword("_EMISSION");
        emissionColorID = Shader.PropertyToID("_EmissionColor");
        colorID = Shader.PropertyToID("_Color");

        if (!material.HasProperty(emissionColorID))
            Debug.LogError("Material has no emission color id!");

        initialized = true;

    }

    public void Emission(Light sharedLight) {

        if (!initialized) {

            Init();
            return;

        }

        if (!sharedLight.enabled)
            targetColor = Color.white * 0f;

        if (!noTexture)
            targetColor = Color.white * sharedLight.intensity * multiplier;
        else
            targetColor = sharedLight.color * sharedLight.intensity * multiplier;

        if (applyAlpha)
            material.SetColor(colorID, new Color(1f, 1f, 1f, sharedLight.intensity * multiplier));

        if (material.GetColor(emissionColorID) != (targetColor))
            material.SetColor(emissionColorID, targetColor);

    }

}
