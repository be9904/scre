using UnityEngine;
using UnityEngine.Rendering.Universal;

[System.Serializable]
public class FullScreenBlurSettings
{
    public RenderPassEvent renderPassEvent;
    public Shader blitShader;

    // runtime options
    public SBool isEnabled;
    public SInt blurStrength;
}

public class FullScreenBlurFeature : ScriptableRendererFeature
{
    FullScreenBlurPass fullscreenPass;
    public FullScreenBlurSettings passSettings = new FullScreenBlurSettings();

    /// <inheritdoc/>
    public override void Create()
    {
        fullscreenPass = new FullScreenBlurPass(
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


