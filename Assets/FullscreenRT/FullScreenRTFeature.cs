using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[System.Serializable]
public class FullScreenRTSettings
{
    public Material blitMaterial;
    public ComputeShader computeShader;
}

public class FullScreenRTFeature : ScriptableRendererFeature
{
    FullScreenRTPass rtPass;
    FullScreenRTSettings passSettings = new FullScreenRTSettings();

    /// <inheritdoc/>
    public override void Create()
    {
        rtPass = new FullScreenRTPass(passSettings);

        // Configures where the render pass should be injected.
        rtPass.renderPassEvent = RenderPassEvent.AfterRenderingOpaques;
    }

    // Here you can inject one or multiple render passes in the renderer.
    // This method is called when setting up the renderer once per-camera.
    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        var cameraColorTargetIdent = renderer.cameraColorTarget;
        rtPass.Setup(cameraColorTargetIdent);
        
        renderer.EnqueuePass(rtPass);
    }
}


