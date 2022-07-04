using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMove : MonoBehaviour
{
    public float AngularSpeed = 1;

    // Update is called once per frame
    void Update()
    {
        this.transform.RotateAround(Vector3.zero, Vector3.up, Time.deltaTime * AngularSpeed);

        Ray ray = new Ray(this.transform.position + Vector3.up, Vector3.down);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit))
        {
            this.transform.position = hit.point;
        }
    }
}
