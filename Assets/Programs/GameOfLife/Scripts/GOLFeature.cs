using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[System.Serializable]
public class GOLSettings
{
    public RenderPassEvent renderPassEvent;
    public GOLGame golGame;
}

public class GOLFeature : ScriptableRendererFeature
{
    GOLPass golPass;
    public GOLSettings passSettings = new GOLSettings();

    /// <inheritdoc/>
    public override void Create()
    {
        golPass = new GOLPass(
            "GOL Fullscreen",
            passSettings
        );
    }

    // Here you can inject one or multiple render passes in the renderer.
    // This method is called when setting up the renderer once per-camera.
    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        ref CameraData cameraData = ref renderingData.cameraData; 
        RenderTargetIdentifier cameraColorTarget = cameraData.renderer.cameraColorTarget;
        golPass.Setup(cameraColorTarget);
        
        renderer.EnqueuePass(golPass);
    }
}


