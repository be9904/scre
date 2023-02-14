using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class GOLPass : ScriptableRenderPass
{
    private string profilerTag;
    
    private RenderTargetIdentifier cameraColorTargetIdent;

    private GOLGame golGame;

    public GOLPass(string profilerTag, GOLSettings passSettings)
    {
        renderPassEvent = passSettings.renderPassEvent;

        this.profilerTag = profilerTag;
        golGame = passSettings.golGame;
    }
    
    public void Setup(RenderTargetIdentifier cameraColorTargetIdent)
    {
        this.cameraColorTargetIdent = cameraColorTargetIdent;
    }

    public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
    {
        
    }

    public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
    {
    }

    // Here you can implement the rendering logic.
    // Use <c>ScriptableRenderContext</c> to issue drawing commands or execute command buffers
    // https://docs.unity3d.com/ScriptReference/Rendering.ScriptableRenderContext.html
    // You don't have to call ScriptableRenderContext.submit, the render pipeline will call it at specific points in the pipeline.
    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        // dispatch compute shader
        
        CommandBuffer cmd = CommandBufferPool.Get();
        cmd.Clear();
        
        cmd.Blit(golGame.Result, cameraColorTargetIdent);

        context.ExecuteCommandBuffer(cmd);
        
        cmd.Clear();
        CommandBufferPool.Release(cmd);
    }
}
