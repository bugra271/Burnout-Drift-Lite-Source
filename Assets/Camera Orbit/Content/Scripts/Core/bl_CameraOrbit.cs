using UnityEngine;
using System.Collections;

public class bl_CameraOrbit : bl_CameraBase {
    [HideInInspector]
    public bool m_Interact = true;

    [Header("Target")]
    public Transform Target;
    [Header("Settings")]
    public bool isForMobile = false;
    public bool AutoTakeInfo = true;
    public float Distance = 5f;
    [Range(0.01f, 5)] public float SwichtSpeed = 2;
    public Vector2 DistanceClamp = new Vector2(1.5f, 5);
    public Vector2 YLimitClamp = new Vector2(-20, 80);
    public Vector2 SpeedAxis = new Vector2(100, 100);
    public bool LockCursorOnRotate = true;
    [Header("Input")]
    public bool RequieredInput = true;
    public CameraMouseInputType RotateInputKey = CameraMouseInputType.LeftAndRight;
    [Range(0.001f, 0.07f)]
    public float InputMultiplier = 0.02f;
    [Range(0.1f, 15)]
    public float InputLerp = 7;
    public bool useKeys = false;
    [Header("Movement")]
    public CameraMovementType MovementType = CameraMovementType.Normal;
    [Range(-90, 90)]
    public float PuwFogAmount = -5;
    [Range(0.1f, 20)]
    public float LerpSpeed = 7;
    [Range(1f, 100)]
    public float OutInputSpeed = 20;
    [Header("Fog")]
    [Range(5, 179)]
    public float FogStart = 100;
    [Range(0.1f, 15)]
    public float FogLerp = 7;
    [Range(0.0f, 7)]
    public float DelayStartFog = 2;
    [Range(1, 10)]
    public float ScrollSensitivity = 5;
    [Range(1, 25)]
    public float ZoomSpeed = 7;
    [Range(1, 7)]
    public float DistanceInfluence = 2;
    [Header("Auto Rotation")]
    public bool AutoRotate = true;
    [Range(0, 20)]
    public float AutoRotSpeed = 5;
    [Header("Collision")]
    public bool DetectCollision = true;
    public bool TeleporOnHit = true;
    [Range(0.01f, 4)]
    public float CollisionRadius = 2;
    [Header("Fade")]
    public bool FadeOnStart = true;
    [Range(0.01f, 5)] public float FadeSpeed = 2;
    [SerializeField]
    private Texture2D FadeTexture;

    private float y;
    private float x;
    private Ray Ray;
    private bool LastHaveInput = false;
    private float distance = 0;
    private float currentFog = 60;
    private float defaultFog;
    public float horizontal;
    public float vertical;
    private float defaultAutoSpeed;
    private float lastHorizontal;
    private bool canFogControl = false;
    private bool haveHit = false;
    private float LastDistance;
    private bool m_CanRotate = true;
    private Vector3 ZoomVector;
    private Quaternion CurrentRotation;
    private Vector3 CurrentPosition;
    private float FadeAlpha = 1;
    private bool isSwitchingTarget = false;

    /// <summary>
    /// 
    /// </summary>
    void Start() {
        SetUp();
    }

    /// <summary>
    /// 
    /// </summary>
    void SetUp() {
        //SetUp default position for camera
        //For avoid the effect of 'telepor' in the firts movement
        if (AutoTakeInfo) {
            distance = Vector3.Distance(transform.position, Target.position);
            Distance = distance;
            Vector3 eulerAngles = Transform.eulerAngles;
            x = eulerAngles.y;
            y = eulerAngles.x;
            horizontal = x;
            vertical = y;
        } else {
            distance = Distance;
        }
        currentFog = GetCamera.fieldOfView;
        defaultFog = currentFog;
        GetCamera.fieldOfView = FogStart;
        defaultAutoSpeed = AutoRotSpeed;
        StartCoroutine(IEDelayFog());
        if (RotateInputKey == CameraMouseInputType.MobileTouch && FindObjectOfType<bl_OrbitTouchPad>() == null) {
            Debug.LogWarning("For use  mobile touched be sure to put the 'OrbitTouchArea in the canvas of scene");
        }
    }

    /// <summary>
    /// 
    /// </summary>
    void LateUpdate() {
        if (Target == null) {
            Debug.LogWarning("Target is not assigned to orbit camera!", this);
            return;
        }
        if (isSwitchingTarget)
            return;

        if (CanRotate) {
            //Calculate the distance of camera
            ZoomControll(false);

            //Control rotation of camera
            OrbitControll();

            //Auto rotate the camera when key is not pressed.
            if (AutoRotate && !isInputKeyRotate) { AutoRotation(); }
        } else {
            //Calculate the distance of camera
            ZoomControll(true);
        }

        //When can't interact with inputs not need continue here.
        if (!m_Interact)
            return;

        //Control fog effect in camera.
        FogControl();

        //Control all insput for apply to the rotation.
        InputControl();
    }

    /// <summary>
    /// 
    /// </summary>
    void InputControl() {
        if (LockCursorOnRotate && !useKeys) {
            if (!isForMobile) {
                if (!isInputKeyRotate && LastHaveInput) {
                    if (LockCursorOnRotate && Interact) { bl_CameraUtils.LockCursor(false); }
                    LastHaveInput = false;
                    if (lastHorizontal >= 0) { AutoRotSpeed = OutInputSpeed; } else { AutoRotSpeed = -OutInputSpeed; }
                }
                if (isInputKeyRotate && !LastHaveInput) {
                    if (LockCursorOnRotate && Interact) { bl_CameraUtils.LockCursor(true); }
                    LastHaveInput = true;
                }
            }
        }

        if (isInputUpKeyRotate) {
            currentFog -= PuwFogAmount;
        }
    }

    /// <summary>
    /// Rotate auto when any key is pressed.
    /// </summary>
    void AutoRotation() {
        AutoRotSpeed = (lastHorizontal > 0) ? Mathf.Lerp(AutoRotSpeed, defaultAutoSpeed, Time.deltaTime / 2) :
        Mathf.Lerp(AutoRotSpeed, -defaultAutoSpeed, Time.deltaTime / 2);
        horizontal += Time.deltaTime * AutoRotSpeed;
    }

    /// <summary>
    /// 
    /// </summary>
    void FogControl() {
        if (!canFogControl)
            return;

        //Controll the 'puw' effect of fog camera.
        currentFog = Mathf.SmoothStep(currentFog, defaultFog, Time.deltaTime * FogLerp);
        //smooth transition with lerp
        GetCamera.fieldOfView = Mathf.Lerp(GetCamera.fieldOfView, currentFog, Time.deltaTime * FogLerp);
    }

    private IEnumerator Freeze() {

        float timer = 3f;

        while (timer > 0f) {

            timer -= Time.deltaTime;
            AutoRotationSpeed = 0f;
            yield return null;

        }

        AutoRotationSpeed = 10;

    }

    /// <summary>
    /// 
    /// </summary>
    void OrbitControll() {

        if (isInputKeyRotate) {
            if (!isForMobile) {
                if (RequieredInput && !useKeys && isInputKeyRotate || !RequieredInput) {
                    horizontal += ((SpeedAxis.x * Distance) * InputMultiplier) * AxisX;
                    vertical -= (SpeedAxis.y * InputMultiplier) * AxisY;
                    lastHorizontal = AxisX;
                    AutoRotationSpeed = 0f;
                    StartCoroutine(Freeze());

                } else if (useKeys) {
                    horizontal -= ((KeyAxisX * SpeedAxis.x) * Distance) * InputMultiplier;
                    vertical += (KeyAxisY * SpeedAxis.y) * InputMultiplier;
                    lastHorizontal = KeyAxisX;
                }
            }
        }

        //clamp 'vertical' angle
        vertical = bl_CameraUtils.ClampAngle(vertical, YLimitClamp.x, YLimitClamp.y);
        //smooth movement of responsiveness input.
        x = Mathf.Lerp(x, horizontal, Time.deltaTime * InputLerp);
        y = Mathf.Lerp(y, vertical, Time.deltaTime * InputLerp);

        if (distance > 100) {
            x = x / (distance / (DistanceInfluence * 10));
            if (horizontal > x) {
                horizontal = x;
            }
        }

        //clamp 'y' angle
        y = bl_CameraUtils.ClampAngle(y, YLimitClamp.x, YLimitClamp.y);

        //convert vector to quaternion for apply to rotation
        CurrentRotation = Quaternion.Euler(y, x, 0f);

        //calculate the position and clamp on a circle
        CurrentPosition = ((CurrentRotation * ZoomVector)) + Target.position;

        //swicht in the movement select
        switch (MovementType) {
            case CameraMovementType.Dynamic:
                Transform.position = Vector3.Lerp(Transform.position, CurrentPosition, (LerpSpeed) * Time.deltaTime);
                Transform.rotation = Quaternion.Lerp(Transform.rotation, CurrentRotation, (LerpSpeed * 2) * Time.deltaTime);
                break;
            case CameraMovementType.Normal:
                Transform.rotation = CurrentRotation;
                Transform.position = CurrentPosition;
                break;
            case CameraMovementType.Towars:
                Transform.rotation = Quaternion.RotateTowards(Transform.rotation, CurrentRotation, (LerpSpeed));
                Transform.position = Vector3.MoveTowards(Transform.position, CurrentPosition, (LerpSpeed));
                break;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    void ZoomControll(bool autoApply) {
        //clamp distance and check this.
        distance = Mathf.Clamp(distance - (MouseScrollWheel * ScrollSensitivity), DistanceClamp.x, DistanceClamp.y);
        //Collision detector with a simple raycast
        if (DetectCollision) {
            //Calculate direction from target
            Vector3 forward = Transform.position - Target.position;
            //create a ray from transform to target
            Ray = new Ray(Target.position, forward.normalized);
            RaycastHit hit;
            //if ray detect a an obstacle in between the point of origin and the target
            if (Physics.SphereCast(Ray.origin, CollisionRadius, Ray.direction, out hit, distance)) {
                if (!haveHit) { LastDistance = distance; haveHit = true; }
                distance = Mathf.Clamp(hit.distance, DistanceClamp.x, DistanceClamp.y);
                if (TeleporOnHit) { Distance = distance; }
            } else {

                StartCoroutine(DetectHit());
            }
            distance = (distance < 1) ? 1 : distance;// distance is recomendable never is least than 1
            if (!haveHit || !TeleporOnHit) {
                Distance = Mathf.SmoothStep(Distance, distance, Time.deltaTime * ZoomSpeed);
            }
        } else {
            distance = (distance < 1) ? 1 : distance;// distance is recomendable never is least than 1
            Distance = Mathf.SmoothStep(Distance, distance, Time.deltaTime * ZoomSpeed);
        }

        //apply distance to vector depth z
        ZoomVector = new Vector3(0f, 0f, -this.Distance);

        if (autoApply) {
            //calculate the position and clamp on a circle
            CurrentPosition = ((CurrentRotation * ZoomVector)) + Target.position;

            //swicht in the movement select
            switch (MovementType) {
                case CameraMovementType.Dynamic:
                    Transform.position = Vector3.Lerp(Transform.position, CurrentPosition, (LerpSpeed) * Time.deltaTime);
                    Transform.rotation = Quaternion.Lerp(Transform.rotation, CurrentRotation, (LerpSpeed * 2) * Time.deltaTime);
                    break;
                case CameraMovementType.Normal:
                    Transform.rotation = CurrentRotation;
                    Transform.position = CurrentPosition;
                    break;
                case CameraMovementType.Towars:
                    Transform.rotation = Quaternion.RotateTowards(Transform.rotation, CurrentRotation, (LerpSpeed));
                    Transform.position = Vector3.MoveTowards(Transform.position, CurrentPosition, (LerpSpeed));
                    break;
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    private bool isInputKeyRotate {
        get {
            switch (RotateInputKey) {
                case CameraMouseInputType.All:
                    return (Input.GetKey(KeyCode.Mouse0) || Input.GetKey(KeyCode.Mouse1) || Input.GetKey(KeyCode.Mouse2) || Input.GetMouseButton(0));
                case CameraMouseInputType.LeftAndRight:
                    return (Input.GetKey(KeyCode.Mouse0) || Input.GetKey(KeyCode.Mouse1));
                case CameraMouseInputType.LeftMouse:
                    return (Input.GetKey(KeyCode.Mouse0));
                case CameraMouseInputType.RightMouse:
                    return (Input.GetKey(KeyCode.Mouse1));
                case CameraMouseInputType.MouseScroll:
                    return (Input.GetKey(KeyCode.Mouse2));
                case CameraMouseInputType.MobileTouch:
                    return (Input.GetMouseButton(0) || Input.GetMouseButton(1));
                default:
                    return (Input.GetKey(KeyCode.Mouse0));
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    void OnGUI() {
        if (isSwitchingTarget) {
            GUI.color = new Color(1, 1, 1, FadeAlpha);
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), FadeTexture, ScaleMode.StretchToFill);
            return;
        }

        if (FadeOnStart && FadeAlpha > 0) {
            FadeAlpha -= Time.deltaTime * FadeSpeed;
            GUI.color = new Color(1, 1, 1, FadeAlpha);
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), FadeTexture, ScaleMode.StretchToFill);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    private bool isInputUpKeyRotate {
        get {
            switch (RotateInputKey) {
                case CameraMouseInputType.All:
                    return (Input.GetKeyUp(KeyCode.Mouse0) || Input.GetKeyUp(KeyCode.Mouse1) || Input.GetKeyUp(KeyCode.Mouse2) || Input.GetMouseButtonUp(0));
                case CameraMouseInputType.LeftAndRight:
                    return (Input.GetKeyUp(KeyCode.Mouse0) || Input.GetKeyUp(KeyCode.Mouse1));
                case CameraMouseInputType.LeftMouse:
                    return (Input.GetKeyUp(KeyCode.Mouse0) || Input.GetMouseButtonUp(0));
                case CameraMouseInputType.RightMouse:
                    return (Input.GetKeyUp(KeyCode.Mouse1));
                case CameraMouseInputType.MouseScroll:
                    return (Input.GetKeyUp(KeyCode.Mouse2));
                case CameraMouseInputType.MobileTouch:
                    return (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1));
                default:
                    return (Input.GetKey(KeyCode.Mouse0) || Input.GetMouseButton(0));
            }
        }
    }

    /// <summary>
    /// Call this function for change the target to orbit
    /// the change will be by a smooth fade effect
    /// </summary>
    public void SetTarget(Transform newTarget) {
        StopCoroutine("TranslateTarget");
        StartCoroutine("TranslateTarget", newTarget);
    }

    /// <summary>
    /// 
    /// </summary>
    IEnumerator TranslateTarget(Transform newTarget) {
        isSwitchingTarget = true;
        while (FadeAlpha < 1) {
            transform.position = Vector3.Lerp(transform.position, transform.position + new Vector3(0, 2, -2), Time.deltaTime);
            FadeAlpha += Time.smoothDeltaTime * SwichtSpeed;
            yield return null;
        }
        Target = newTarget;
        isSwitchingTarget = false;
        while (FadeAlpha > 0) {
            FadeAlpha -= Time.smoothDeltaTime * SwichtSpeed;
            yield return null;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    IEnumerator DetectHit() {
        yield return new WaitForSeconds(0.4f);
        if (haveHit) { distance = LastDistance; haveHit = false; }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    IEnumerator IEDelayFog() {
        yield return new WaitForSeconds(DelayStartFog);
        canFogControl = true;
    }

    /// <summary>
    /// 
    /// </summary>
    public float Horizontal {
        get {
            return horizontal;
        }
        set {
            horizontal += value;
            lastHorizontal = horizontal;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public float Vertical {
        get {
            return vertical;
        }
        set {
            vertical += value;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public bool Interact {
        get {
            return m_Interact;
        }
        set {
            m_Interact = value;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public bool CanRotate {
        get {
            return m_CanRotate;
        }
        set {
            m_CanRotate = value;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public float AutoRotationSpeed {
        get {
            return defaultAutoSpeed;
        }
        set {
            defaultAutoSpeed = value;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    public void SetZoom(float value) {
        distance += (-(value * 0.5f) * ScrollSensitivity);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    public void SetStaticZoom(float value) {
        distance += value;
    }

    /// <summary>
    /// 
    /// </summary>
    void OnDrawGizmosSelected() {
        Gizmos.color = new Color32(0, 221, 221, 255);
        if (Target != null) {
            Gizmos.DrawLine(transform.position, Target.position);
            Gizmos.matrix = Matrix4x4.TRS(Target.position, transform.rotation, new Vector3(1f, 0, 1f));
            Gizmos.DrawWireSphere(Target.position, Distance);
            Gizmos.matrix = Matrix4x4.identity;
        }
        Gizmos.DrawCube(transform.position, new Vector3(1, 0.2f, 0.2f));
        Gizmos.DrawCube(transform.position, Vector3.one / 2);
    }

    public void SetHorizontal(float hor) {

        horizontal = hor;

    }

    public void SetVertical(float ver) {

        vertical = ver;

    }

    public void ToggleAutoRotation(bool state) {

        AutoRotate = state;

    }

}