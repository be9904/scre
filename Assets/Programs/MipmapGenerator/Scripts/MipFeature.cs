using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[System.Serializable]
public class MipSettings
{
    public RenderPassEvent renderPassEvent;
    public ComputeShader computeShader;

    // runtime options
    public STexture2D inputTexture;
    public SInt mipLevel;
}

public class MipFeature : ScriptableRendererFeature
{
    MipPass mipPass;
    public MipSettings passSettings;

    /// <inheritdoc/>
    public override void Create()
    {
        Shader.SetGlobalTexture("_MIP", passSettings.inputTexture);
        
        mipPass = new MipPass(
            "Mipmap Render pass",
            passSettings
        );

        // Configures where the render pass should be injected.
        mipPass.renderPassEvent = RenderPassEvent.AfterRenderingOpaques;
    }

    // Here you can inject one or multiple render passes in the renderer.
    // This method is called when setting up the renderer once per-camera.
    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        ref CameraData cameraData = ref renderingData.cameraData; 
        RenderTargetIdentifier cameraColorTarget = cameraData.renderer.cameraColorTarget;
        mipPass.Setup(cameraColorTarget);
        
        renderer.EnqueuePass(mipPass);
    }
}


