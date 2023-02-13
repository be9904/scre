using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class FullScreenBlurRenderPass : ScriptableRenderPass
{
    // used to label this pass in Unity's Frame Debug utility
    string profilerTag;

    Material blitMaterial;
    RenderTargetIdentifier cameraColorTargetIdent;
    RenderTargetHandle tempTexture;

    private static readonly int _blurStrength = Shader.PropertyToID("_BlurStrength");

    public FullScreenBlurRenderPass(string profilerTag, 
        FullScreenBlurSettings passSettings)
    {
        this.profilerTag = profilerTag;
        renderPassEvent = passSettings.renderPassEvent;
        blitMaterial = new Material(passSettings.blitShader);
        blitMaterial.SetInt(_blurStrength, passSettings.blurStrength.Value);
    }
    
    public void Setup(RenderTargetIdentifier cameraColorTargetIdent)
    {
        this.cameraColorTargetIdent = cameraColorTargetIdent;
    }
    
    // called each frame before Execute, use it to set up things the pass will need
    public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
    {
        // create a temporary render texture that matches the camera
        cmd.GetTemporaryRT(tempTexture.id, cameraTextureDescriptor);
    }

    // Here you can implement the rendering logic.
    // Use <c>ScriptableRenderContext</c> to issue drawing commands or execute command buffers
    // https://docs.unity3d.com/ScriptReference/Rendering.ScriptableRenderContext.html
    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        // fetch a command buffer to use
        CommandBuffer cmd = CommandBufferPool.Get(profilerTag);
        cmd.Clear();
        
        cmd.Blit(cameraColorTargetIdent, tempTexture.Identifier(), blitMaterial, 0);
        cmd.Blit(tempTexture.Identifier(), cameraColorTargetIdent, blitMaterial, 1);
        
        context.ExecuteCommandBuffer(cmd);
        
        cmd.Clear();
        CommandBufferPool.Release(cmd);
    }

    // called after Execute, use it to clean up anything allocated in Configure
    public override void OnCameraCleanup(CommandBuffer cmd)
    {
        cmd.ReleaseTemporaryRT(tempTexture.id);
    }
}
