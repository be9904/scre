using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class GlitchPass : ScriptableRenderPass
{
    private string profilerTag;

    private RenderTargetIdentifier cameraColorTargetIdent;
    private RenderTargetHandle tempTexture;
    
    // pass params
    private float verticalJumpTime;
    
    // feature settings
    private float scanLineJitter;
    private float verticalJump;
    private float horizontalShake;
    private float colorDrift;

    private Material blitMaterial;
    private bool useTexture;
    private int texID;

    // shader properties
    private static readonly int scanLineJitterID = Shader.PropertyToID("_ScanLineJitter");
    private static readonly int verticalJumpID = Shader.PropertyToID("_VerticalJump");
    private static readonly int horizontalShakeID = Shader.PropertyToID("_HorizontalShake");
    private static readonly int colorDriftID = Shader.PropertyToID("_ColorDrift");
    
    public GlitchPass(string profilerTag, GlitchSettings passSettings)
    {
        this.profilerTag = profilerTag;
        renderPassEvent = passSettings.renderPassEvent;

        scanLineJitter = passSettings.scanLineJitter;
        verticalJump = passSettings.verticalJump;
        horizontalShake = passSettings.horizontalShake;
        colorDrift = passSettings.colorDrift;
        
        blitMaterial = new Material(passSettings.shader);
        blitMaterial.hideFlags = HideFlags.DontSave;
        // blitMaterial.mainTexture = null;
        
        useTexture = passSettings.useTexture;
        if (useTexture)
            texID = Shader.PropertyToID("_GlitchTexture");
    }

    public void Setup(RenderTargetIdentifier cameraColorTargetIdent)
    {
        this.cameraColorTargetIdent = cameraColorTargetIdent;
    }
    
    // called each frame before Execute, use it to set up things the pass will need
    public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
    {
        // create a temporary render texture that matches the camera
        if (!useTexture)
            cmd.GetTemporaryRT(tempTexture.id, cameraTextureDescriptor);
        
    }

    // Here you can implement the rendering logic.
    // Use <c>ScriptableRenderContext</c> to issue drawing commands or execute command buffers
    // https://docs.unity3d.com/ScriptReference/Rendering.ScriptableRenderContext.html
    // You don't have to call ScriptableRenderContext.submit, the render pipeline will call it at specific points in the pipeline.
    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        CommandBuffer cmd = CommandBufferPool.Get();
        cmd.Clear();
        
        UpdateMaterialProperties();
        
        if (!useTexture) // blit from camera color
        {
            cmd.Blit(cameraColorTargetIdent, tempTexture.Identifier());
            cmd.Blit(tempTexture.Identifier(), cameraColorTargetIdent, blitMaterial, 0);
        }
        else // blit from global(uniform) texture
        {
            cmd.Blit(Shader.GetGlobalTexture(texID), cameraColorTargetIdent, blitMaterial, 0);
        }

        context.ExecuteCommandBuffer(cmd);
        
        cmd.Clear();
        CommandBufferPool.Release(cmd);
    }

    // Cleanup any allocated resources that were created during the execution of this render pass.
    public override void OnCameraCleanup(CommandBuffer cmd)
    {
        if(!useTexture)
            cmd.ReleaseTemporaryRT(tempTexture.id);
    }

    void UpdateMaterialProperties()
    {
        verticalJumpTime += Time.deltaTime * verticalJump * 11.3f;

        var sl_thresh = Mathf.Clamp01(1.0f - scanLineJitter * 1.2f);
        var sl_disp = 0.002f + Mathf.Pow(scanLineJitter, 3) * 0.05f;
        blitMaterial.SetVector(scanLineJitterID, new Vector2(sl_disp, sl_thresh));

        var vj = new Vector2(verticalJump, verticalJumpTime);
        blitMaterial.SetVector(verticalJumpID, vj);
        blitMaterial.SetFloat(horizontalShakeID, horizontalShake * 0.2f);

        var cd = new Vector2(colorDrift * 0.04f, Time.time * 606.11f);
        blitMaterial.SetVector(colorDriftID, cd);
    }
}
