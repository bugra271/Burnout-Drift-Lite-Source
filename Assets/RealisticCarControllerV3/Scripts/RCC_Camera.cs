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
using UnityEngine.EventSystems;

/// <summary>
/// Main RCC Camera controller. Includes 6 different camera modes with many customizable settings. It doesn't use different cameras on your scene like *other* assets. Simply it parents the camera to their positions that's all. No need to be Einstein.
/// Also supports collision detection.
/// </summary>
[AddComponentMenu("BoneCracker Games/Realistic Car Controller/Camera/RCC Camera")]
public class RCC_Camera : MonoBehaviour {

    [System.Serializable]
    public class CameraTarget {

        public RCC_CarControllerV3 playerVehicle;

        public float speed { get { if (!playerVehicle) return 0f; return playerVehicle.speed; } }

        public Vector3 localVelocity { get { if (!playerVehicle) return Vector3.zero; return playerVehicle.transform.InverseTransformDirection(playerVehicle.rigid.velocity); } }

        private RCC_HoodCamera _hoodCamera;
        public RCC_HoodCamera hoodCamera {

            get {

                if (!playerVehicle)
                    return null;

                if (!_hoodCamera)
                    _hoodCamera = playerVehicle.GetComponentInChildren<RCC_HoodCamera>();

                return _hoodCamera;

            }

        }

        private RCC_WheelCamera _wheelCamera;
        public RCC_WheelCamera wheelCamera {

            get {

                if (!playerVehicle)
                    return null;

                if (!_wheelCamera)
                    _wheelCamera = playerVehicle.GetComponentInChildren<RCC_WheelCamera>();

                return _wheelCamera;

            }

        }

    }

    public CameraTarget cameraTarget = new CameraTarget();

    // Currently rendering?
    public bool isRendering = true;

    public RCC_Inputs inputs;

    public Camera actualCamera;          // Camera is not attached to this main gameobject. Camera is parented to pivot gameobject. Therefore, we can apply additional position and rotation changes.
    public GameObject pivot;        // Pivot center of the camera. Used for making offsets and collision movements.

    // Camera Modes.
    public enum CameraMode { TPS, FPS, WHEEL, FIXED, CINEMATIC, TOP }
    public CameraMode cameraMode;
    private CameraMode lastCameraMode;

    private RCC_FixedCamera fixedCamera {get {return RCC_FixedCamera.Instance;}}
    private RCC_CinematicCamera cinematicCamera {get {return RCC_CinematicCamera.Instance;}}

    public bool TPSLockX = true;
    public bool TPSLockY = true;
    public bool TPSLockZ = true;
    public bool TPSFreeFall = true;

    public bool useTopCameraMode = false;               // Shall we use top camera mode?
    public bool useHoodCameraMode = true;               // Shall we use hood camera mode?
    public bool useOrbitInTPSCameraMode = true;     // Shall we use orbit control in TPS camera mode?
    public bool useOrbitInHoodCameraMode = true;    // Shall we use orbit control in hood camera mode?
    public bool useWheelCameraMode = true;          // Shall we use wheel camera mode?
    public bool useFixedCameraMode = true;              // Shall we use fixed camera mode?
    public bool useCinematicCameraMode = true;      // Shall we use cinematic camera mode?
    public bool useOrthoForTopCamera = false;           // Shall we use ortho in top camera mode?
    public bool useOcclusion = true;                            // Shall we use camera occlusion?
    private bool ocluded = false;
    public LayerMask occlusionLayerMask = -1;

    public bool useAutoChangeCamera = false;            // Shall we change camera mode by auto?
    public float autoChangeCameraTimer = 0f;

    public Vector3 topCameraAngle = new Vector3(45f, 45f, 0f);      // If so, we will use this Vector3 angle for top camera mode.

    private float distanceOffset = 0f;
    public float maximumZDistanceOffset = 10f;      // Distance offset for top camera mode. Related with vehicle speed. If vehicle speed is higher, camera will move to front of the vehicle.
    public float topCameraDistance = 100f;              // Top camera height / distance.

    // Target position.
    private Vector3 targetPosition = Vector3.zero;

    // Used for resetting orbit values when direction of the vehicle has been changed.
    private int direction = 1;
    private int lastDirection = 1;

    public float TPSDistance = 6f;                                  // The distance for TPS camera mode.
    public float TPSHeight = 2f;                                        // The height we want the camera to be above the target for TPS camera mode.

    [Range(0f, 1f)] public float TPSRotationDamping = .7f;                      // Rotation movement damper.
    [Range(0f, 25f)] public float TPSTiltMaximum = 15f;                         // Maximum tilt angle related with rigidbody local velocity.
    [Range(0f, 10f)] public float TPSTiltMultiplier = 1.5f;                                // Tilt angle multiplier.
    [Range(-45f, 45f)] public float TPSYaw = 0f;                                        // Yaw angle.
    [Range(-45f, 45f)] public float TPSPitch = 10f;                                     // Pitch angle.

    //	Target tilt angle.
    private float TPSTiltAngle = 0f;                                // Current tilt angle.

    public bool TPSAutoFocus = true;                                // Auto focus to player vehicle. Adjusts distance and height depends on vehicle bounds.
    public bool TPSAutoReverse = true;                          // Auto reverse when player vehicle is at reverse gear.
    public bool TPSCollision = true;                                    // Collision effect when player vehicle crashes.
    public Vector3 TPSOffset = new Vector3(0f, 0f, .2f);   // TPS position offset.
    public Vector3 TPSStartRotation = new Vector3(0f, 0f, 0f);   // Rotation of the camera will be this when game starts.
    private Quaternion TPSLastRotation;

    internal float targetFieldOfView = 60f;         // Camera will adapt its field of view to this target field of view. All field of views below this line will feed this value.

    [Range(10f, 90f)] public float TPSMinimumFOV = 40f;         // Minimum field of view related with vehicle speed.
    [Range(10f, 160f)] public float TPSMaximumFOV = 60f;            // Maximum field of view related with vehicle speed.

    [Range(10f, 160f)] public float hoodCameraFOV = 60f;            // Hood field of view.
    [Range(10f, 160f)] public float wheelCameraFOV = 60f;           // Wheel field of view.

    public float minimumOrtSize = 10f;          // Minimum ortho size related with vehicle speed.
    public float maximumOrtSize = 20f;          // Maximum ortho size related with vehicle speed.

    internal int cameraSwitchCount = 0;                 // Used in switch case for running corresponding camera mode method.

    private float xVelocity, yVelocity, zVelocity;

    private Vector3 collisionVector = Vector3.zero;             // Collision vector.
    private Vector3 collisionPos = Vector3.zero;                    // Collision position.
    private Quaternion collisionRot = Quaternion.identity;  // Collision rotation.

    private float zoomScroll = 0;
    [Range(.5f, 10f)] public float zoomScrollMultiplier = 5f;
    public float minimumScroll = 0f;
    public float maximumScroll = 5f;

    // Raw Orbit X and Y inputs.
    internal float orbitX = 0f;
    internal float orbitY = 0f;

    // Smooth Orbit X and Y inputs.
    internal float orbitX_Smoothed = 0f;
    internal float orbitY_Smoothed = 0f;

    // Minimum and maximum Orbit X, Y degrees.
    public float minOrbitY = -15f;
    public float maxOrbitY = 70f;

    //	Orbit X and Y speeds.
    public float orbitXSpeed = 100f;
    public float orbitYSpeed = 100f;
    public float orbitSmooth = 40f;

    //	Resetting orbits.
    public bool orbitReset = false;
    private float orbitResetTimer = 0f;
    private float oldOrbitX = 0f;
    public float oldOrbitY = 0f;

    public bool lookBack = false;

    public delegate void onBCGCameraSpawned(GameObject BCGCamera);
    public static event onBCGCameraSpawned OnBCGCameraSpawned;

    void Awake() {

        // Getting Camera.
        actualCamera = GetComponentInChildren<Camera>();

    }

    void OnEnable() {

        // Calling this event when BCG Camera spawned.
        if (OnBCGCameraSpawned != null)
            OnBCGCameraSpawned(gameObject);

        // Listening player vehicle collisions for crashing effects.
        RCC_CarControllerV3.OnRCCPlayerCollision += RCC_CarControllerV3_OnRCCPlayerCollision;

        // Listening input events.
        RCC_InputManager.OnChangeCamera += RCC_InputManager_OnChangeCamera;
        RCC_InputManager.OnLookBack += RCC_InputManager_OnLookBack;

    }

    void RCC_CarControllerV3_OnRCCPlayerCollision(RCC_CarControllerV3 RCC, Collision collision) {

        Collision(collision);

    }

    private void RCC_InputManager_OnLookBack(bool state) {

        lookBack = state;

    }

    private void RCC_InputManager_OnChangeCamera() {

        ChangeCamera();

    }

    public void SetTarget(GameObject player) {

        cameraTarget = new CameraTarget {
            playerVehicle = player.GetComponent<RCC_CarControllerV3>()
        };

        if (TPSAutoFocus)
            StartCoroutine(AutoFocus());

        ResetCamera();

    }

    public void RemoveTarget() {

        transform.SetParent(null);
        cameraTarget.playerVehicle = null;

    }

    void Update() {

        // If it's active, enable the camera. If it's not, disable the camera.
        if (!isRendering) {

            if (actualCamera.gameObject.activeInHierarchy)
                actualCamera.gameObject.SetActive(false);

            return;

        } else {

            if (!actualCamera.gameObject.activeInHierarchy)
                actualCamera.gameObject.SetActive(true);

        }

        // Early out if we don't have the player vehicle.
        if (!cameraTarget.playerVehicle) {

            return;

        }

        Inputs();

        // Lerping current field of view to target field of view.
        actualCamera.fieldOfView = Mathf.Lerp(actualCamera.fieldOfView, targetFieldOfView, Time.deltaTime * 5f);

    }

    void LateUpdate() {

        // Early out if we don't have the player vehicle.
        if (!cameraTarget.playerVehicle)
            return;

        // Even if we have the player vehicle and it's disabled, return.
        if (!cameraTarget.playerVehicle.gameObject.activeSelf)
            return;

        if (Time.time == 0)
            return;

        // Run the corresponding method with choosen camera mode.
        switch (cameraMode) {

            case CameraMode.TPS:

                if (useOrbitInTPSCameraMode)
                    ORBIT();

                TPS();

                break;

            case CameraMode.FPS:

                if (useOrbitInHoodCameraMode)
                    ORBIT();

                FPS();

                break;

            case CameraMode.WHEEL:
                WHEEL();
                break;

            case CameraMode.FIXED:
                FIXED();
                break;

            case CameraMode.CINEMATIC:
                CINEMATIC();
                break;

            case CameraMode.TOP:
                TOP();
                break;

        }

        if (lastCameraMode != cameraMode)
            ResetCamera();

        lastCameraMode = cameraMode;
        autoChangeCameraTimer += Time.deltaTime;

        if (useAutoChangeCamera && autoChangeCameraTimer > 10) {

            autoChangeCameraTimer = 0f;
            ChangeCamera();

        }

    }

    private void FixedUpdate() {

        // Early out if we don't have the player vehicle.
        if (!cameraTarget.playerVehicle)
            return;

        // Even if we have the player vehicle and it's disabled, return.
        if (!cameraTarget.playerVehicle.gameObject.activeSelf)
            return;

        DrawRay();

    }

    private void Inputs() {

        inputs = RCC_InputManager.GetInputs();

        orbitX += inputs.orbitX;
        orbitY -= inputs.orbitY;

        // Clamping Y.
        orbitY = Mathf.Clamp(orbitY, minOrbitY, maxOrbitY);

        orbitX_Smoothed = Mathf.Lerp(orbitX_Smoothed, orbitX, Time.deltaTime * orbitSmooth);
        orbitY_Smoothed = Mathf.Lerp(orbitY_Smoothed, orbitY, Time.deltaTime * orbitSmooth);

    }

    // Change camera by increasing camera switch counter.
    public void ChangeCamera() {

        // Increasing camera switch counter at each camera changing.
        cameraSwitchCount++;

        // We have 6 camera modes at total. If camera switch counter is greater than maximum, set it to 0.
        if (cameraSwitchCount >= 6)
            cameraSwitchCount = 0;

        switch (cameraSwitchCount) {

            case 0:
                cameraMode = CameraMode.TPS;
                break;

            case 1:
                if (useHoodCameraMode && cameraTarget.hoodCamera) {
                    cameraMode = CameraMode.FPS;
                } else {
                    ChangeCamera();
                }
                break;

            case 2:
                if (useWheelCameraMode && cameraTarget.wheelCamera) {
                    cameraMode = CameraMode.WHEEL;
                } else {
                    ChangeCamera();
                }
                break;

            case 3:
                if (useFixedCameraMode && fixedCamera) {
                    cameraMode = CameraMode.FIXED;
                } else {
                    ChangeCamera();
                }
                break;

            case 4:
                if (useCinematicCameraMode && cinematicCamera) {
                    cameraMode = CameraMode.CINEMATIC;
                } else {
                    ChangeCamera();
                }
                break;

            case 5:
                if (useTopCameraMode) {
                    cameraMode = CameraMode.TOP;
                } else {
                    ChangeCamera();
                }
                break;

        }

    }

    // Change camera by directly setting it to specific mode.
    public void ChangeCamera(CameraMode mode) {

        cameraMode = mode;

    }

    private void FPS() {

        if (Time.timeScale == 0)
            return;

        // Assigning orbit rotation, and transform rotation.
        if (useOrbitInHoodCameraMode)
            transform.rotation = cameraTarget.playerVehicle.transform.rotation * Quaternion.Euler(orbitY_Smoothed, orbitX_Smoothed, 0f);
        else
            transform.rotation = cameraTarget.playerVehicle.transform.rotation;

    }

    private void WHEEL() {

        if (useOcclusion && ocluded)
            ChangeCamera(CameraMode.TPS);

    }

    private void TPS2() {

        // Position at the target
        Vector3 positionTarget = cameraTarget.playerVehicle.transform.position;

        // Then offset by distance behind the new angle
        positionTarget += cameraTarget.playerVehicle.transform.rotation * ((-Vector3.forward * TPSDistance) + (Vector3.forward * (cameraTarget.speed / 15f)));
        positionTarget += Vector3.up * TPSHeight;

        Vector3 lookAtTarget = cameraTarget.playerVehicle.transform.position;
        lookAtTarget += (cameraTarget.playerVehicle.transform.rotation * TPSOffset);

        if (Vector3.Distance(transform.position, positionTarget) > 15) {

            transform.position = cameraTarget.playerVehicle.transform.position;
            transform.position += cameraTarget.playerVehicle.transform.rotation * (-Vector3.forward * TPSDistance);
            transform.position += Vector3.up * TPSHeight;

        }

        transform.position = Vector3.Lerp(transform.position, positionTarget, Time.deltaTime * 3f);
        transform.LookAt(lookAtTarget);

    }

    private void TPS() {

        transform.rotation = TPSLastRotation;
        TPSLastRotation = transform.rotation;

        // If TPS Auto Reverse is enabled and vehicle is moving backwards, reset X and Y orbits when vehicle direction is changed. Camera will look directly rear side of the vehicle.
        if (lastDirection != cameraTarget.playerVehicle.direction)
            direction = cameraTarget.playerVehicle.direction;

        lastDirection = cameraTarget.playerVehicle.direction;

        //	Vehicle direction angle used for back side camera angle. 0 means forwards, 180 means backwards.
        int dir = 0;

        float rotDamp = TPSRotationDamping;

        // Calculate the current rotation angles for TPS mode.
        if (TPSAutoReverse)
            dir = (direction == 1 ? 0 : 180);

        if (lookBack) {

            dir = 180;
            rotDamp = 1f;

        }

        if (TPSFreeFall) {

            if (!cameraTarget.playerVehicle.isGrounded)
                rotDamp = -500f;

        }

        float xAngle = 0f;

        if (TPSLockX)
            xAngle = Mathf.SmoothDampAngle(transform.eulerAngles.x, cameraTarget.playerVehicle.transform.eulerAngles.x * (dir == 180 ? -1f : 1f), ref xVelocity, 1f - rotDamp);

        if (useOrbitInTPSCameraMode && orbitY != 0)
            xAngle = orbitY_Smoothed;

        float yAngle = 0f;

        if (TPSLockY) {

            if (!useOrbitInTPSCameraMode) {

                yAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, cameraTarget.playerVehicle.transform.eulerAngles.y + dir, ref yVelocity, 1f - rotDamp);

            } else {

                if (orbitX != 0)
                    yAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, cameraTarget.playerVehicle.transform.eulerAngles.y + orbitX_Smoothed, ref yVelocity, .025f);
                else
                    yAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, cameraTarget.playerVehicle.transform.eulerAngles.y + dir, ref yVelocity, 1f - rotDamp);

            }

        } else {

            if (useOrbitInTPSCameraMode && orbitX != 0)
                yAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, orbitX_Smoothed, ref yVelocity, .025f);

        }

        float zAngle = 0f;

        if (TPSLockZ)
            zAngle = Mathf.SmoothDampAngle(transform.eulerAngles.z, cameraTarget.playerVehicle.transform.eulerAngles.z, ref zVelocity, 1f - rotDamp);

        zoomScroll += inputs.scroll.y * zoomScrollMultiplier;
        zoomScroll = Mathf.Clamp(zoomScroll, minimumScroll, maximumScroll);

        // Position at the target
        Vector3 position = cameraTarget.playerVehicle.transform.position;

        // Rotation at the target
        Quaternion rotation = Quaternion.Euler(xAngle, yAngle, zAngle);

        // Then offset by distance behind the new angle
        position += rotation * (-Vector3.forward * (TPSDistance + zoomScroll));

        //position += playerCar.transform.rotation * TPSOffset;
        position += Vector3.up * TPSHeight;

        //// Look at the target
        if (Time.timeScale != 0) {

            transform.rotation = rotation;
            transform.position = position;

        }

        // Collision positions and rotations that affects pivot of the camera.
        if (Time.deltaTime != 0) {

            collisionPos = Vector3.Lerp(collisionPos, Vector3.zero, Time.deltaTime * 5f);
            collisionRot = Quaternion.Lerp(collisionRot, Quaternion.identity, Time.deltaTime * 5f);

        }

        // Lerping position and rotation of the pivot to collision.
        pivot.transform.localPosition = Vector3.Lerp(pivot.transform.localPosition, collisionPos, Time.deltaTime * 10f);
        pivot.transform.localRotation = Quaternion.Lerp(pivot.transform.localRotation, collisionRot, Time.deltaTime * 10f);

        // Lerping targetFieldOfView from TPSMinimumFOV to TPSMaximumFOV related with vehicle speed.
        targetFieldOfView = Mathf.Lerp(TPSMinimumFOV, TPSMaximumFOV, Mathf.Abs(cameraTarget.speed) / 150f);

        // Rotates camera by Z axis for tilt effect.
        TPSTiltAngle = TPSTiltMaximum * (Mathf.Clamp(cameraTarget.localVelocity.x, -1f, 1f) * Mathf.Abs(cameraTarget.localVelocity.x) / 250f);
        TPSTiltAngle *= -TPSTiltMultiplier;

        if (useOcclusion)
            OccludeRay(cameraTarget.playerVehicle.transform.position);

        TPSLastRotation = transform.rotation;
        transform.rotation *= Quaternion.Euler(TPSPitch, 0f, TPSYaw + TPSTiltAngle);

    }

    void FIXED() {

        if (fixedCamera.transform.parent != null)
            fixedCamera.transform.SetParent(null);

        if (useOcclusion && ocluded) {

            fixedCamera.ChangePosition();
            ocluded = false;

        }

    }

    private void TOP() {

        // Early out if we don't have the player vehicle.
        if (!cameraTarget.playerVehicle)
            return;

        // Setting ortho or perspective?
        actualCamera.orthographic = useOrthoForTopCamera;

        distanceOffset = Mathf.Lerp(0f, maximumZDistanceOffset, Mathf.Abs(cameraTarget.speed) / 100f);
        targetFieldOfView = Mathf.Lerp(minimumOrtSize, maximumOrtSize, Mathf.Abs(cameraTarget.speed) / 100f);
        actualCamera.orthographicSize = targetFieldOfView;

        // Setting proper targetPosition for top camera mode.
        targetPosition = cameraTarget.playerVehicle.transform.position;
        targetPosition += cameraTarget.playerVehicle.transform.rotation * Vector3.forward * distanceOffset;

        // Assigning position and rotation.
        transform.position = targetPosition;
        transform.rotation = Quaternion.Euler(topCameraAngle);

        // Pivot position.
        pivot.transform.localPosition = new Vector3(0f, 0f, -topCameraDistance);

    }

    private void ORBIT() {

        if (oldOrbitX != orbitX) {

            oldOrbitX = orbitX;
            orbitResetTimer = 2f;

        }

        if (oldOrbitY != orbitY) {

            oldOrbitY = orbitY;
            orbitResetTimer = 2f;

        }

        if (orbitResetTimer > 0)
            orbitResetTimer -= Time.deltaTime;

        Mathf.Clamp(orbitResetTimer, 0f, 2f);

        if (orbitReset && cameraTarget.speed >= 25f && orbitResetTimer <= 0f) {

            orbitX = 0f;
            orbitY = 0f;

        }

    }

    public void OnDrag(PointerEventData pointerData) {

        // Receiving drag input from UI.
        orbitX += pointerData.delta.x * orbitXSpeed / 1000f;
        orbitY -= pointerData.delta.y * orbitYSpeed / 1000f;

        orbitResetTimer = 0f;

    }

    public void OnDrag(float x, float y) {

        // Receiving drag input from UI.
        orbitX += x * orbitXSpeed / 10f;
        orbitY -= y * orbitYSpeed / 10f;

        orbitResetTimer = 0f;

    }

    private void CINEMATIC() {

        if (cinematicCamera.transform.parent != null)
            cinematicCamera.transform.SetParent(null);

        targetFieldOfView = cinematicCamera.targetFOV;

        if (useOcclusion && ocluded)
            ChangeCamera(CameraMode.TPS);

    }

    public void Collision(Collision collision) {

        // If it's not enable or camera mode is TPS, return.
        if (!enabled || !isRendering || cameraMode != CameraMode.TPS || !TPSCollision)
            return;

        // Local relative velocity.
        Vector3 colRelVel = collision.relativeVelocity;
        colRelVel *= 1f - Mathf.Abs(Vector3.Dot(transform.up, collision.GetContact(0).normal));

        float cos = Mathf.Abs(Vector3.Dot(collision.GetContact(0).normal, colRelVel.normalized));

        if (colRelVel.magnitude * cos >= 5f) {

            collisionVector = transform.InverseTransformDirection(colRelVel) / (30f);

            collisionPos -= collisionVector * 5f;
            collisionRot = Quaternion.Euler(new Vector3(-collisionVector.z * 10f, -collisionVector.y * 10f, -collisionVector.x * 10f));
            targetFieldOfView = actualCamera.fieldOfView - Mathf.Clamp(collision.relativeVelocity.magnitude, 0f, 15f);

        }

    }

    private void ResetCamera() {

        if (fixedCamera)
            fixedCamera.canTrackNow = false;

        TPSTiltAngle = 0f;

        collisionPos = Vector3.zero;
        collisionRot = Quaternion.identity;

        actualCamera.transform.localPosition = Vector3.zero;
        actualCamera.transform.localRotation = Quaternion.identity;

        pivot.transform.localPosition = collisionPos;
        pivot.transform.localRotation = collisionRot;

        orbitX = TPSStartRotation.y;
        orbitY = TPSStartRotation.x;

        zoomScroll = 0f;

        if (TPSStartRotation != Vector3.zero)
            TPSStartRotation = Vector3.zero;

        actualCamera.orthographic = false;
        ocluded = false;

        switch (cameraMode) {

            case CameraMode.TPS:
                transform.SetParent(null);
                targetFieldOfView = TPSMinimumFOV;
                break;

            case CameraMode.FPS:
                transform.SetParent(cameraTarget.hoodCamera.transform, false);
                transform.localPosition = Vector3.zero;
                transform.localRotation = Quaternion.identity;
                targetFieldOfView = hoodCameraFOV;
                cameraTarget.hoodCamera.FixShake();
                break;

            case CameraMode.WHEEL:
                transform.SetParent(cameraTarget.wheelCamera.transform, false);
                transform.localPosition = Vector3.zero;
                transform.localRotation = Quaternion.identity;
                targetFieldOfView = wheelCameraFOV;
                break;

            case CameraMode.FIXED:
                transform.SetParent(fixedCamera.transform, false);
                transform.localPosition = Vector3.zero;
                transform.localRotation = Quaternion.identity;
                targetFieldOfView = 60;
                fixedCamera.canTrackNow = true;
                break;

            case CameraMode.CINEMATIC:
                transform.SetParent(cinematicCamera.pivot.transform, false);
                transform.localPosition = Vector3.zero;
                transform.localRotation = Quaternion.identity;
                targetFieldOfView = 30f;
                break;

            case CameraMode.TOP:
                transform.SetParent(null);
                targetFieldOfView = minimumOrtSize;
                pivot.transform.localPosition = Vector3.zero;
                pivot.transform.localRotation = Quaternion.identity;
                targetPosition = cameraTarget.playerVehicle.transform.position;
                targetPosition += cameraTarget.playerVehicle.transform.rotation * Vector3.forward * distanceOffset;
                transform.position = cameraTarget.playerVehicle.transform.position;
                break;

        }

    }

    public void ToggleCamera(bool state) {

        // Enabling / disabling activity.
        isRendering = state;

    }

    private void OccludeRay(Vector3 targetFollow) {

        //declare a new raycast hit.
        RaycastHit wallHit = new RaycastHit();

        if (Physics.Linecast(targetFollow, transform.position, out wallHit, occlusionLayerMask)) {

            if (!wallHit.collider.isTrigger && !wallHit.transform.IsChildOf(cameraTarget.playerVehicle.transform)) {

                //the x and z coordinates are pushed away from the wall by hit.normal.
                //the y coordinate stays the same.
                Vector3 occludedPosition = new Vector3(wallHit.point.x + wallHit.normal.x * .2f, wallHit.point.y + wallHit.normal.y * .2f, wallHit.point.z + wallHit.normal.z * .2f);

                transform.position = occludedPosition;

            }

        }

    }

    private void DrawRay() {

        //Declare a new raycast hit.
        RaycastHit wallHit = new RaycastHit();

        if (Physics.Linecast(cameraTarget.playerVehicle.transform.position, transform.position, out wallHit, occlusionLayerMask)) {

            if (!wallHit.collider.isTrigger && !wallHit.transform.IsChildOf(cameraTarget.playerVehicle.transform))
                ocluded = true;

        }

    }

    public IEnumerator AutoFocus() {

        float timer = 3f;
        float bounds = RCC_GetBounds.MaxBoundsExtent(cameraTarget.playerVehicle.transform);

        while (timer > 0f) {

            timer -= Time.deltaTime;
            TPSDistance = Mathf.Lerp(TPSDistance, bounds * 2.8f, Time.deltaTime * 2f);
            TPSHeight = Mathf.Lerp(TPSHeight, bounds * .65f, Time.deltaTime * 2f);
            yield return null;

        }

        TPSDistance = bounds * 2.8f;
        TPSHeight = bounds * .65f;

    }

    public IEnumerator AutoFocus(Transform transformBounds) {

        float timer = 3f;
        float bounds = RCC_GetBounds.MaxBoundsExtent(transformBounds);

        while (timer > 0f) {

            timer -= Time.deltaTime;
            TPSDistance = Mathf.Lerp(TPSDistance, bounds * 2.8f, Time.deltaTime * 2f);
            TPSHeight = Mathf.Lerp(TPSHeight, bounds * .65f, Time.deltaTime * 2f);
            yield return null;

        }

        TPSDistance = bounds * 2.8f;
        TPSHeight = bounds * .65f;

    }

    public IEnumerator AutoFocus(Transform transformBounds1, Transform transformBounds2) {

        float timer = 3f;
        float bounds = (RCC_GetBounds.MaxBoundsExtent(transformBounds1) + RCC_GetBounds.MaxBoundsExtent(transformBounds2));

        while (timer > 0f) {

            timer -= Time.deltaTime;
            TPSDistance = Mathf.Lerp(TPSDistance, bounds * 2.8f, Time.deltaTime * 2f);
            TPSHeight = Mathf.Lerp(TPSHeight, bounds * .65f, Time.deltaTime * 2f);
            yield return null;

        }

        TPSDistance = bounds * 2.8f;
        TPSHeight = bounds * .65f;

    }

    public IEnumerator AutoFocus(Transform transformBounds1, Transform transformBounds2, Transform transformBounds3) {

        float timer = 3f;
        float bounds = (RCC_GetBounds.MaxBoundsExtent(transformBounds1) + RCC_GetBounds.MaxBoundsExtent(transformBounds2) + RCC_GetBounds.MaxBoundsExtent(transformBounds3));

        while (timer > 0f) {

            timer -= Time.deltaTime;
            TPSDistance = Mathf.Lerp(TPSDistance, bounds * 2.8f, Time.deltaTime * 2f);
            TPSHeight = Mathf.Lerp(TPSHeight, bounds * .65f, Time.deltaTime * 2f);
            yield return null;

        }

        TPSDistance = bounds * 2.8f;
        TPSHeight = bounds * .65f;

    }

    void OnDisable() {

        RCC_CarControllerV3.OnRCCPlayerCollision -= RCC_CarControllerV3_OnRCCPlayerCollision;

        // Listening input events.
        RCC_InputManager.OnChangeCamera -= RCC_InputManager_OnChangeCamera;
        RCC_InputManager.OnLookBack -= RCC_InputManager_OnLookBack;

    }

}