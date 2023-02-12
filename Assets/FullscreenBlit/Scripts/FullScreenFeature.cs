using UnityEngine;
using UnityEngine.Rendering.Universal;

[System.Serializable]
public class FullScreenPassSettings
{
    public bool isEnabled = true;
    public RenderPassEvent renderPassEvent = RenderPassEvent.AfterRenderingTransparents;
    public Shader blitShader;

    [Range(0, 20)] public int blurStrength = 5;
}

public class FullScreenFeature : ScriptableRendererFeature
{
    FullScreenRenderPass fullscreenPass;
    public FullScreenPassSettings passSettings = new FullScreenPassSettings();

    /// <inheritdoc/>
    public override void Create()
    {
        fullscreenPass = new FullScreenRenderPass(
            "Fullscreen Pass",
            passSettings
        );
    }

    // Here you can inject one or multiple render passes in the renderer.
    // This method is called when setting up the renderer once per-camera.
    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        if (!passSettings.isEnabled) return;

        var cameraColorTargetIdent = renderer.cameraColorTarget;
        fullscreenPass.Setup(cameraColorTargetIdent);
        
        renderer.EnqueuePass(fullscreenPass);
    }
}


