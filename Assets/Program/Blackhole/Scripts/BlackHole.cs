using System;
using UnityEngine;
using UnityEngine.VFX;


public class BlackHole : MonoBehaviour
{
    [Header("Settings")] [SerializeField] private Material blackHoleMaterial;
    [SerializeField] private VisualEffect discVfx;

    [Header("Parameters")] public float radius;
    public float discRadius;

    private int materialRadiusId;
    private int vfxRadiusId;
    private int dicsRadiusId;

#if UNITY_EDITOR
    private void Reset()
    {
        discVfx = GetComponentInChildren<VisualEffect>();
    }
#endif

    private void Start()
    {
        materialRadiusId = Shader.PropertyToID("_Radius");
        vfxRadiusId = Shader.PropertyToID("Radius");
        dicsRadiusId = Shader.PropertyToID("DiscRadius");
    }

    private void Update()
    {
        blackHoleMaterial.SetFloat(materialRadiusId, radius);
        discVfx.SetFloat(vfxRadiusId, radius);
        discVfx.SetFloat(dicsRadiusId, discRadius);
    }
}