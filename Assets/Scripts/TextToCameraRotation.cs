using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextToCameraRotation : MonoBehaviour {

    void Update() {

        transform.rotation = Camera.main.transform.rotation;

    }

}
