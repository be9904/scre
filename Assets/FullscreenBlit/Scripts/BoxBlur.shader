// Two-pass box blur shader created for URP 12 and Unity 2021.2
// Made by Alexander Ameye 
// https://alexanderameye.github.io/

Shader "Hidden/Box Blur"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white"
        _BlurStrength("Blur Strength", int) = 1
    }

    SubShader
    {
        Tags
        {
            "RenderType"="Opaque" "RenderPipeline" = "UniversalPipeline"
        }

        HLSLINCLUDE
        #pragma vertex vert
        #pragma fragment frag

        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

        struct Attributes
        {
            float4 vertex : POSITION;
            float2 uv : TEXCOORD0;
        };

        struct Varyings
        {
            float4 cpos : SV_POSITION;
            float2 uv : TEXCOORD0;
        };

        Texture2D _MainTex;
        SamplerState sampler_MainTex;
        float4 _MainTex_TexelSize;
        float4 _MainTex_ST;

        int _BlurStrength;

        Varyings vert(Attributes IN)
        {
            Varyings OUT;
            OUT.cpos = TransformObjectToHClip(IN.vertex.xyz);
            OUT.uv = IN.uv * _MainTex_ST.xy + _MainTex_ST.zw;
            return OUT;
        }
        ENDHLSL

        Pass
        {
            Name "VERTICAL"

            HLSLPROGRAM
            half4 frag(Varyings IN) : SV_TARGET
            {
                float2 res = _MainTex_TexelSize.xy;
                half4 sum = 0;

                for (float y = -_BlurStrength; y <= _BlurStrength; y++)
                {
                    float2 offset = float2(0, y);
                    half4 sample = _MainTex.Sample(sampler_MainTex, IN.uv + offset * res); 
                    if(sample.a > 0) sum += half4(sample.rgb, 1.0); 
                }

                if(sum.a > 0) sum /= sum.a;
                
                return sum;
            }
            ENDHLSL
        }

        Pass
        {
            Name "HORIZONTAL"

            HLSLPROGRAM
            half4 frag(Varyings IN) : SV_TARGET
            {
                float2 res = _MainTex_TexelSize.xy;
                half4 sum = 0;

                for (float x = -_BlurStrength; x <= _BlurStrength; x++)
                {
                    float2 offset = float2(x, 0);
                    half4 sample = _MainTex.Sample(sampler_MainTex, IN.uv + offset * res); 
                    if(sample.a > 0) sum += half4(sample.rgb, 1.0); 
                }

                if(sum.a > 0) sum /= sum.a;
                
                return sum;
            }
            ENDHLSL
        }
    }
}