using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class FullScreenRTPass : ScriptableRenderPass
{
    private string profilerTag;
    
    private RenderTargetIdentifier cameraColorTargetIdent;

    private Material blitMaterial;
    private bool useShader;
    
    private ComputeShader computeShader;
    private int kernelID;
    private RenderTexture outputRT;

    public FullScreenRTPass(string profilerTag, FullScreenRTSettings passSettings)
    {
        renderPassEvent = passSettings.renderPassEvent;
        
        this.profilerTag = profilerTag;
        kernelID = passSettings.kernelID.Value;
        if (passSettings.useShader)
        {
            useShader = passSettings.useShader;
            blitMaterial = new Material(passSettings.blitShader);
        }
        computeShader = passSettings.computeShader;
        
        DispatchComputeShader();
    }

    public void Setup(RenderTargetIdentifier cameraColorTargetIdent)
    {
        this.cameraColorTargetIdent = cameraColorTargetIdent;
    }

    // Here you can implement the rendering logic.
    // Use <c>ScriptableRenderContext</c> to issue drawing commands or execute command buffers
    // https://docs.unity3d.com/ScriptReference/Rendering.ScriptableRenderContext.html
    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        CommandBuffer cmd = CommandBufferPool.Get();
        cmd.Clear();
        
        if(useShader)
            cmd.Blit(outputRT, cameraColorTargetIdent, blitMaterial, 0);
        else
            cmd.Blit(outputRT, cameraColorTargetIdent);

        context.ExecuteCommandBuffer(cmd);
        
        cmd.Clear();
        CommandBufferPool.Release(cmd);
    }

    void DispatchComputeShader()
    {
        // draw on render texture
        outputRT = new RenderTexture(2048, 2048, 24)
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
}
