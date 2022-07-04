using ch.sycoforge.Decal;
using UnityEngine;

[RequireComponent(typeof(EasyDecal))]
public class DecalPreProcessor : MonoBehaviour
{
    private void Awake()
    {
        EasyDecal decal = GetComponent<EasyDecal>();

        decal.Distance = Random.Range(0.0001f, 0.001f);
    }
}
