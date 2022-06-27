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

/// <summary>
/// Suspension impact force based on axis controller.
/// </summary>
public class RCC_SuspensionBumper : MonoBehaviour{

    private RCC_CarControllerV3 carController;

    public string inputHorizontalID = "Suspension_Horizontal";
    public string inputVerticalID = "Suspension_Vertical";

    public float inputHorizontal = 0f;
    public float inputVertical = 0f;

    public float targetSuspension = .5f;

    [System.Serializable]
    private class SuspensionWheel {

        public WheelCollider wheelCollider;
        public float defaultSuspensionDistance = .2f;

        public void Init(WheelCollider wc) {

            wheelCollider = wc;
            defaultSuspensionDistance = wc.suspensionDistance;

        }

        public void Move(float distance) {

            wheelCollider.suspensionDistance = Mathf.MoveTowards(wheelCollider.suspensionDistance, distance, Time.deltaTime * 50f);

        }

    }

    private SuspensionWheel frontLeftWheel;
    private SuspensionWheel frontRightWheel;
    private SuspensionWheel rearLeftWheel;
    private SuspensionWheel rearRightWheel;

    void Start(){

        carController = GetComponent<RCC_CarControllerV3>();

        if (!carController)
            return;

        WheelCollider frontLeftWheelCollider = carController.FrontLeftWheelCollider.wheelCollider;
        SuspensionWheel frontLeft = new SuspensionWheel();
        frontLeft.Init(frontLeftWheelCollider);
        frontLeftWheel = frontLeft;

        WheelCollider frontRightWheelCollider = carController.FrontRightWheelCollider.wheelCollider;
        SuspensionWheel frontRight = new SuspensionWheel();
        frontRight.Init(frontRightWheelCollider);
        frontRightWheel = frontRight;

        WheelCollider rearLeftWheelCollider = carController.RearLeftWheelCollider.wheelCollider;
        SuspensionWheel rearLeft = new SuspensionWheel();
        rearLeft.Init(rearLeftWheelCollider);
        rearLeftWheel = rearLeft;

        WheelCollider rearRightWheelCollider = carController.RearRightWheelCollider.wheelCollider;
        SuspensionWheel rearRight = new SuspensionWheel();
        rearRight.Init(rearRightWheelCollider);
        rearRightWheel = rearRight;

    }

    // Update is called once per frame
    void Update(){

        if (!carController)
            return;

        //inputHorizontal = Input.GetAxisRaw("Suspension_Horizontal");
        //inputVertical = Input.GetAxisRaw("Suspension_Vertical");

        //float targetSuspension_FL = Mathf.Clamp(targetSuspension * Mathf.Clamp01(inputHorizontal + (-inputVertical)), .05f, 1f);
        //frontLeftWheel.Move(targetSuspension_FL + frontLeftWheel.defaultSuspensionDistance);

        //float targetSuspension_FR = Mathf.Clamp(targetSuspension * Mathf.Clamp01(-inputHorizontal + (-inputVertical)), .05f, 1f);
        //frontRightWheel.Move(targetSuspension_FR + frontRightWheel.defaultSuspensionDistance);

        //float targetSuspension_RL = Mathf.Clamp(targetSuspension * Mathf.Clamp01(inputHorizontal + (inputVertical)), .05f, 1f);
        //rearLeftWheel.Move(targetSuspension_RL + rearLeftWheel.defaultSuspensionDistance);

        //float targetSuspension_RR = Mathf.Clamp(targetSuspension * Mathf.Clamp01(-inputHorizontal + (inputVertical)), .05f, 1f);
        //rearRightWheel.Move(targetSuspension_RR + rearRightWheel.defaultSuspensionDistance);

    }

}
