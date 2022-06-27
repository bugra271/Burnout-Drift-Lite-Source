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
public class RCC_CrashShredder : MonoBehaviour{

    public Transform hingePoint;        // Hinge joint.
    private Rigidbody rigid;                //  Rigid.

    public Vector3 direction;               //  Direction of the force.
    public float force = 1f;                    //  Strength of the force.

    void Start(){

        rigid = GetComponent<Rigidbody>();  //  Getting rigid of the gameobject.
        CreateHinge();      //  Creating hinge point.

    }

    void FixedUpdate(){

        if (!rigid)
            return;

        rigid.AddRelativeTorque(direction * force, ForceMode.Acceleration);
        
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
