using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[System.Serializable]
public class FullScreenRTSettings
{
    public RenderPassEvent renderPassEvent;
    public ComputeShader computeShader;
    public Shader blitShader;
    [HideInInspector] public bool useShader = false;
    
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
        if(passSettings.blitShader != null)
            passSettings.useShader = true;
        
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


