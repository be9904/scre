//
// KinoGlitch - Video glitch effect
//
// Copyright (C) 2015 Keijiro Takahashi
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of
// this software and associated documentation files (the "Software"), to deal in
// the Software without restriction, including without limitation the rights to
// use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
// the Software, and to permit persons to whom the Software is furnished to do so,
// subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
// FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
// COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
// IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
// CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
Shader "Hidden/Kino/Glitch/Analog/Logo"
{
    Properties
    {
        _MainTex ("-", 2D) = "" {}
    }
    
    SubShader
    {
        Pass
        {
            Name "Kino Glitch"
            ZTest Always 
            Cull Off
            ZWrite Off
            
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            Texture2D _MainTex;
            SamplerState sampler_MainTex;
            float2 _MainTex_TexelSize;

            float2 _ScanLineJitter; // (displacement, threshold)
            float2 _VerticalJump;   // (amount, time)
            float _HorizontalShake;
            float2 _ColorDrift;     // (amount, time)

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

            float nrand(float x, float y)
            {
                return frac(sin(dot(float2(x, y), float2(12.9898, 78.233))) * 43758.5453);
            }

            Varyings vert(Attributes IN)
            {
                Varyings OUT;

                OUT.cpos = TransformObjectToHClip(IN.vertex);
                OUT.uv = IN.uv;

                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                float u = IN.uv.x;
                float v = IN.uv.y;

                // Scan line jitter
                float jitter = nrand(v, _Time.x) * 2 - 1;
                jitter *= step(_ScanLineJitter.y, abs(jitter)) * _ScanLineJitter.x;

                // Vertical jump
                float jump = lerp(v, frac(v + _VerticalJump.y), _VerticalJump.x);

                // Horizontal shake
                float shake = (nrand(_Time.x, 2) - 0.5) * _HorizontalShake;

                // Color drift
                float drift = sin(jump + _ColorDrift.y) * _ColorDrift.x;

                half4 src1 = _MainTex.Sample(sampler_MainTex, frac(float2(u + jitter + shake, jump)));
                half4 src2 = _MainTex.Sample(sampler_MainTex, frac(float2(u + jitter + shake + drift, jump)));

                return half4(1 - src1.r, 1 - src2.g, 1 - src1.b, 1);
            }
            ENDHLSL
        }
    }
}
