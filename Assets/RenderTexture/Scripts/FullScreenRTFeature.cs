using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[System.Serializable]
public class FullScreenRTSettings
{
    public RenderPassEvent renderPassEvent = RenderPassEvent.AfterRenderingTransparents;
    public Shader blitShader;
    public ComputeShader computeShader;
    
    // runtime options
    public SInt kernelID;
}

public class FullScreenRTFeature : ScriptableRendererFeature
{
    FullScreenRTPass rtPass;
    public FullScreenRTSettings passSettings = new FullScreenRTSettings();

    /// <inheritdoc/>
    public override void Create()
    {
        rtPass = new FullScreenRTPass(
            "Fullscreen Render Texture",
            passSettings
        );
    }

    // Here you can inject one or multiple render passes in the renderer.
    // This method is called when setting up the renderer once per-camera.
    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        ref CameraData cameraData = ref renderingData.cameraData; 
        RenderTargetIdentifier cameraColorTarget = cameraData.renderer.cameraColorTarget;
        rtPass.Setup(cameraColorTarget);
        
        renderer.EnqueuePass(rtPass);
    }
}


