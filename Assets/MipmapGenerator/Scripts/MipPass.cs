using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class MipPass : ScriptableRenderPass
{
    private string profilerTag;

    private RenderTargetIdentifier cameraColorTargetIdent;

    private ComputeShader computeShader;
    private int mipLevel;
    private int Resolution;
    private RenderTexture outputRT;

    public MipPass(string profilerTag, MipSettings passSettings)
    {
        renderPassEvent = passSettings.renderPassEvent;

        this.profilerTag = profilerTag;
        computeShader = passSettings.computeShader;
        mipLevel = passSettings.mipLevel.Value;
        Resolution = passSettings.inputTexture.Variable.texture.width;
        outputRT = GenerateMipmap(mipLevel);
    }

    public void Setup(RenderTargetIdentifier cameraColorTargetIdent)
    {
        this.cameraColorTargetIdent = cameraColorTargetIdent;
    }

    // Here you can implement the rendering logic.
    // Use <c>ScriptableRenderContext</c> to issue drawing commands or execute command buffers
    // https://docs.unity3d.com/ScriptReference/Rendering.ScriptableRenderContext.html
    // You don't have to call ScriptableRenderContext.submit, the render pipeline will call it at specific points in the pipeline.
    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        CommandBuffer cmd = CommandBufferPool.Get();
        cmd.Clear();
        
        if (mipLevel == 0) // original image
        {
            cmd.Blit(Shader.GetGlobalTexture("_MIP"), cameraColorTargetIdent);
        }
        else // mipmap
        {
            cmd.Blit(ProgramUtility.RTtoTex2D(outputRT, TextureWrapMode.Clamp), cameraColorTargetIdent);
        }
        
        context.ExecuteCommandBuffer(cmd);
        
        cmd.Clear();
        CommandBufferPool.Release(cmd);
    }

    RenderTexture GenerateMipmap(int level)
    {
        outputRT = new RenderTexture(1024, 1024, 24)
        {
            enableRandomWrite = true
        };
        outputRT.Create();
        
        Texture2D readBuffer = (Texture2D)Shader.GetGlobalTexture("_MIP");

        // don't allocate to write buffer yet
        RenderTexture writeBuffer = new RenderTexture(Resolution, Resolution, 24);
        Resolution = outputRT.width;

        for (int k = 0; k < level; k++)
        {
            int mipResolution = Resolution >> (k + 1);
            if (mipResolution <= 0) break;
            
            writeBuffer = new RenderTexture(mipResolution, mipResolution, 24)
            {
                enableRandomWrite = true
            };
            writeBuffer.Create();

            computeShader.SetFloat("Resolution", readBuffer.width * 1.0f);
            computeShader.SetTexture(0, "_ReadBuffer", readBuffer);
            computeShader.SetTexture(0, "Result", writeBuffer);
            computeShader.Dispatch(
                0,
                writeBuffer.width / 8,
                writeBuffer.height / 8,
                1
            );

            readBuffer = ProgramUtility.RTtoTex2D(writeBuffer);
        }

        return writeBuffer;
    }
}
