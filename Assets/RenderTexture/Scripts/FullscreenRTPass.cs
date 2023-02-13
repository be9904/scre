using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class FullScreenRTPass : ScriptableRenderPass
{
    private string profilerTag;
    
    private RenderTargetIdentifier cameraColorTargetIdent;

    private ComputeShader computeShader;
    private int kernelID;
    private RenderTexture outputRT;
    
    public FullScreenRTPass(string profilerTag, FullScreenRTSettings passSettings)
    {
        kernelID = passSettings.kernelID;
        computeShader = passSettings.computeShader;
    }

    public void Setup(RenderTargetIdentifier cameraColorTargetIdent)
    {
        this.cameraColorTargetIdent = cameraColorTargetIdent;
    }
    
    // This method is called before executing the render pass.
    // It can be used to configure render targets and their clear state. Also to create temporary render target textures.
    // When empty this render pass will render to the active camera render target.
    // You should never call CommandBuffer.SetRenderTarget. Instead call <c>ConfigureTarget</c> and <c>ConfigureClear</c>.
    // The render pipeline will ensure target setup and clearing happens in a performant manner.
    public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
    {
        // draw on render texture
        outputRT = new RenderTexture(512, 512, 24)
        {
            enableRandomWrite = true
        };
        outputRT.Create();
        
        computeShader.SetFloat("Resolution", outputRT.width);
        
        computeShader.SetTexture(kernelID, "Result", outputRT);
        computeShader.Dispatch(
            kernelID, 
            outputRT.width / 8, 
            outputRT.height / 8, 
            1
        );
    }

    // Here you can implement the rendering logic.
    // Use <c>ScriptableRenderContext</c> to issue drawing commands or execute command buffers
    // https://docs.unity3d.com/ScriptReference/Rendering.ScriptableRenderContext.html
    // You don't have to call ScriptableRenderContext.submit, the render pipeline will call it at specific points in the pipeline.
    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        CommandBuffer cmd = CommandBufferPool.Get();
        cmd.Clear();
        
        cmd.Blit(outputRT, cameraColorTargetIdent);
        
        context.ExecuteCommandBuffer(cmd);
        
        cmd.Clear();
        CommandBufferPool.Release(cmd);
    }

    // Cleanup any allocated resources that were created during the execution of this render pass.
    public override void OnCameraCleanup(CommandBuffer cmd)
    {
        outputRT.Release();
    }
}
