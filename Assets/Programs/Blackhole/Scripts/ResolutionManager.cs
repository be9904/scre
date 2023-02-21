using System;
using UnityEngine;

public class ResolutionManager : MonoBehaviour
{
    public static readonly Vector2 Resolution = new(1920, 1080);
    public static ResolutionManager Instance { get; private set; }

    [SerializeField] private Camera[] cameras;

    private ResolutionManager()
    {
        if (Instance != this && Instance != null)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void Update()
    {
        var aspect = Resolution.x / Resolution.y;
        foreach (var camera in cameras)
        {
            camera.aspect = aspect;
        }
    }
}