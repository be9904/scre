using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[System.Serializable]
public class GlitchSettings
{
    [Range(0, 1)] public float scanLineJitter;
    [Range(0, 1)] public float verticalJump;
    [Range(0, 1)] public float horizontalShake;
    [Range(0, 1)] public float colorDrift;

    public Shader shader;
    
    public bool useTexture;
    public STexture2D inputTexture;
}

public class GlitchFeature : ScriptableRendererFeature
{
    GlitchPass glitchPass;
    public GlitchSettings passSettings = new GlitchSettings();

    /// <inheritdoc/>
    public override void Create()
    {
        if(passSettings.useTexture)
            Shader.SetGlobalTexture("_GlitchTexture", passSettings.inputTexture);
        
        glitchPass = new GlitchPass(
            "KinoGlitch",
            passSettings
        );

        // Configures where the render pass should be injected.
        glitchPass.renderPassEvent = RenderPassEvent.AfterRenderingOpaques;
    }

    // Here you can inject one or multiple render passes in the renderer.
    // This method is called when setting up the renderer once per-camera.
    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        ref CameraData cameraData = ref renderingData.cameraData; 
        RenderTargetIdentifier cameraColorTarget = cameraData.renderer.cameraColorTarget;
        glitchPass.Setup(cameraColorTarget);
        
        renderer.EnqueuePass(glitchPass);
    }
}


