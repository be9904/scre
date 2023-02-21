Shader "Unlit/FishMovement"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Cutoff("Alpha cutoff", Range(0,1)) = 0.5
        _Frequency("Frequency", Range(1, 10)) = 5
        _Amplitude("Amplitude", Range(0.01, 1)) = 0.02
        _Speed("Speed", Range(0.5, 5)) = 1
        _Random("Random", Range(0, 1)) = 0.5
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue" = "AlphaTest"}
        LOD 100
        Cull Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            half _Cutoff;
            float4 _MainTex_ST;
            half _Random;
            half _Amplitude;
            half _Frequency;
            half _Speed;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);

                float2 value = o.uv.xy;
                float dist = distance(value.x, 0.9);
                o.vertex.xyz += v.normal * sin(_Frequency * dist - (_Time.y + _Random) * _Speed ) * sqrt(_Frequency * dist) * _Amplitude;

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                clip(col.a - _Cutoff);
                return col;
            }
            ENDCG
        }
    }
}
