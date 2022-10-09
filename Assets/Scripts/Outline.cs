using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

//Thank you minsin56 for providing this script.
[Serializable,VolumeComponentMenu("Post-processing/Custom/Outline")]
public class Outline : CustomPass
{
    public LayerMask outlineLayer = 0;
    [ColorUsage(false, true)] public Color outlineColor = Color.black;
    public float threshold = 1;
    public float thickness = 1;
    public float outlineIntensity = 1;

    public static List<Renderer> OutlineRenderers = new();

    // To make sure the shader will ends up in the build, we keep it's reference in the custom pass
    [SerializeField, HideInInspector] private Shader outlineShader;

    private Material _fullscreenOutline;
    private RTHandle _outlineBuffer;
    
    private static readonly int OutlineColor = Shader.PropertyToID("OutlineColor");
    private static readonly int OutlineBuffer = Shader.PropertyToID("OutlineBuffer");
    private static readonly int Threshold = Shader.PropertyToID("Threshold");
    private static readonly int Thickness = Shader.PropertyToID("Thickness");
    private static readonly int OutlineIntensity = Shader.PropertyToID("OutlineIntensity");
    private static readonly int TexelSize = Shader.PropertyToID("TexelSize");

    protected override void Setup(ScriptableRenderContext renderContext, CommandBuffer cmd)
    {
        outlineShader = Shader.Find("Hidden/Shader/Outline");
        _fullscreenOutline = CoreUtils.CreateEngineMaterial(outlineShader);

        _outlineBuffer = RTHandles.Alloc
        (
            Vector2.one, TextureXR.slices, dimension: TextureXR.dimension,
            colorFormat: GraphicsFormat.B10G11R11_UFloatPack32,
            useDynamicScale: true, name: "Outline Buffer"
        );
    }

    protected override void Execute(CustomPassContext ctx)
    {
        CoreUtils.SetRenderTarget(ctx.cmd, _outlineBuffer, ClearFlag.Color);

        OutlineRenderers.ForEach((renderer =>
        {
            for (int i = 0; i < renderer.sharedMaterials.Length; i++)
            {
                ctx.cmd.DrawRenderer(renderer, renderer.sharedMaterials[i], i);
            }
        }));
        
        CustomPassUtils.DrawRenderers(ctx, outlineLayer);


        ctx.propertyBlock.SetColor(OutlineColor, outlineColor);
        ctx.propertyBlock.SetTexture(OutlineBuffer, _outlineBuffer);
        ctx.propertyBlock.SetFloat(Threshold, Mathf.Max(0.000001f, threshold * 0.01f));
        ctx.propertyBlock.SetFloat(Thickness, thickness);
        ctx.propertyBlock.SetFloat(OutlineIntensity, outlineIntensity);
        ctx.propertyBlock.SetVector(TexelSize, _outlineBuffer.rt.texelSize);
        
        
        CoreUtils.DrawFullScreen(ctx.cmd, _fullscreenOutline, ctx.cameraColorBuffer, shaderPassId: 0,
            properties: ctx.propertyBlock);
    }

    protected override void Cleanup()
    {
        CoreUtils.Destroy(_fullscreenOutline);
        _outlineBuffer.Release();
    }
}
