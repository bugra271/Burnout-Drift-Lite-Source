using UnityEngine;
using System.Collections;
using ch.sycoforge.Decal;
using System.Collections.Generic;

namespace ch.sycoforge.Decal.Demo
{
    /// <summary>
    /// Demo class for runtime decal placement.
    /// </summary>
    public class Footprints : MonoBehaviour
    {
        //------------------------------------
        // ExposedFields
        //------------------------------------
        public EasyDecal DecalPrefab;
        public float DistanceThreshold = 0.8f;
        public float FootDistance = 0.5f;

        private float distance;
        private int index;
        private Vector3 lastPosition;

        //------------------------------------
        // Methods
        //------------------------------------

        public void Start()
        {
            if (DecalPrefab == null)
            {
                Debug.LogError("The DynamicDemo script has no decal prefab attached.");
            }
        }

        public void Update()
        {
            if(DecalPrefab == null) { return; }

            if (distance >= DistanceThreshold)
            {
                Vector3 offset = (index == 0 ? Vector3.right : Vector3.left) * FootDistance * 0.5f;
                Vector3 localPosition = offset;
                Vector3 worldPosition = this.transform.TransformPoint(offset);

                // Shoot a ray thru towards the ground
                Ray ray = new Ray(worldPosition + Vector3.up * 0.1f, Vector3.down);
                RaycastHit hit;

                // Check if mouseRay hit something
                if (Physics.Raycast(ray, out hit, 200))
                {
                    // Instantiate the decal prefab according the hit normal
                    EasyDecal decal = EasyDecal.ProjectAt(DecalPrefab.gameObject, hit.collider.gameObject, hit.point, hit.normal);
                    decal.AtlasRegionIndex = index;
                    decal.transform.rotation = Quaternion.Euler(Vector3.up * (this.transform.rotation.eulerAngles.y));
                }

                index = ++index % 2;
                distance = 0;
            }

            distance += Vector3.Distance(lastPosition, this.transform.position);
            lastPosition = this.transform.position;
        }
    }
}
