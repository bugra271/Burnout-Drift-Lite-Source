using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RCC_LOD : MonoBehaviour {

    private float distanceToCamera = 0f;
    public float lodBias = 2f;

    [System.Serializable]
    public class LODGroup {

        public List<GameObject> group = new List<GameObject>();
        internal RCC_WheelCollider[] wheelColliderGroup;
        public bool active = false;

        public void Add(GameObject add) {

            if (!group.Contains(add))
                group.Add(add);

        }

        public void EnableGroup() {

            if (active)
                return;

            for (int i = 0; i < group.Count; i++)
                group[i].SetActive(true);

            if (wheelColliderGroup != null && wheelColliderGroup.Length > 0) {

                for (int i = 0; i < wheelColliderGroup.Length; i++) {

                    wheelColliderGroup[i].alignWheel = true;
                    wheelColliderGroup[i].drawSkid = true;

                }

            }

            active = true;

        }

        public void DisableGroup() {

            if (!active)
                return;

            for (int i = 0; i < group.Count; i++)
                group[i].SetActive(false);

            if (wheelColliderGroup != null && wheelColliderGroup.Length > 0) {

                for (int i = 0; i < wheelColliderGroup.Length; i++) {

                    wheelColliderGroup[i].alignWheel = false;
                    wheelColliderGroup[i].drawSkid = false;

                }

            }

            active = false;

        }

    }

    private LODGroup[] lodGroup;

    private int level = 0;
    private int oldLevel = -1;

    private void Awake() {

        lodGroup = new LODGroup[3];
        lodGroup[0] = new LODGroup();
        lodGroup[1] = new LODGroup();
        lodGroup[2] = new LODGroup();

    }

    private IEnumerator Start() {

        yield return new WaitForFixedUpdate();

        if (transform.Find("All Audio Sources"))
            lodGroup[1].Add(transform.Find("All Audio Sources").gameObject);

        RCC_Light[] allLights = GetComponentsInChildren<RCC_Light>();

        foreach (RCC_Light item in allLights)
            lodGroup[0].Add(item.gameObject);

        RCC_HoodCamera hoodCamera = GetComponentInChildren<RCC_HoodCamera>();

        if (hoodCamera)
            lodGroup[1].Add(hoodCamera.gameObject);

        RCC_WheelCamera wheelCamera = GetComponentInChildren<RCC_WheelCamera>();

        if (wheelCamera)
            lodGroup[1].Add(wheelCamera.gameObject);

        ParticleSystem[] allParticles = GetComponentsInChildren<ParticleSystem>();

        foreach (ParticleSystem item in allParticles)
            lodGroup[1].Add(item.gameObject);

        lodGroup[0].wheelColliderGroup = GetComponentsInChildren<RCC_WheelCollider>();

    }

    void Update() {

        if (RCC_SceneManager.Instance.activeMainCamera)
            distanceToCamera = Vector3.Distance(transform.position, RCC_SceneManager.Instance.activeMainCamera.transform.position);

        if (distanceToCamera < 25f * lodBias)
            level = 2;
        else if (distanceToCamera < 50f * lodBias)
            level = 1;
        else if (distanceToCamera < 100f * lodBias)
            level = 0;

        if (level != oldLevel)
            SetLOD();

        oldLevel = level;

    }

    private void SetLOD() {

        for (int i = level; i >= 0; i--)
            lodGroup[i].EnableGroup();

        int lev = (lodGroup.Length - 1) - level;

        for (int i = 0; i < lev; i++) {

            lodGroup[i].DisableGroup();

        }

    }

}
