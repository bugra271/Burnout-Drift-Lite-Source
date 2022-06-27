//----------------------------------------------
//            Realistic Car Controller
//
// Copyright © 2014 - 2022 BoneCracker Games
// http://www.bonecrackergames.com
// Buğra Özdoğanlar
//
//----------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class RCC_CrashHammer : MonoBehaviour{

    public Transform hingePoint;
    private Rigidbody rigid;
    public Vector3 torque;

    public float length = 1f;
    public float speed = 1f;

    void Start(){

        rigid = GetComponent<Rigidbody>();
        CreateHinge();
        
    }

    void FixedUpdate(){

        if (!rigid)
            return;
        
        rigid.AddRelativeForce(torque * ((float)Mathf.Sin(Time.time * speed) * length), ForceMode.Acceleration);

    }

    private void CreateHinge() {

        GameObject hinge = new GameObject("Hinge_" + transform.name);
        hinge.transform.position = hingePoint.position;
        hinge.transform.rotation = hingePoint.rotation;

        Rigidbody hingeRigid = hinge.AddComponent<Rigidbody>();
        hingeRigid.isKinematic = true;
        hingeRigid.useGravity = false;

        AttachHinge(hingeRigid);

    }

    private void AttachHinge(Rigidbody hingeRigid) {

        ConfigurableJoint joint = GetComponent<ConfigurableJoint>();

        if (!joint) {

            print("Configurable Joint of the " + transform.name + " not found! Be sure this gameobject has Configurable Joint with right config.");
            return;

        }

        joint.autoConfigureConnectedAnchor = false;
        joint.connectedBody = hingeRigid;
        joint.connectedAnchor = Vector3.zero;

    }

    void Reset() {

        if (hingePoint == null) {

            hingePoint = new GameObject("Hinge Point").transform;
            hingePoint.SetParent(transform, false);
            
        }

    }

}
