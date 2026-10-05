// Pulsing rim glow drawn on top of a loot object (add it as an extra material on the renderer).
Shader "Custom/LootGlow"
{
    Properties
    {
        [HDR] _GlowColor ("Glow Color", Color) = (1, 0.75, 0.25, 1)
        _RimPower ("Rim Power", Range(0.5, 8)) = 2.5
        _Intensity ("Intensity", Range(0, 4)) = 1.4
        _PulseSpeed ("Pulse Speed", Float) = 3
        _Inflate ("Inflate", Range(0, 0.05)) = 0.01
    }
    SubShader
    {
        Tags { "RenderType" = "Transparent" "Queue" = "Transparent" "RenderPipeline" = "UniversalPipeline" }

        Pass
        {
            Name "Glow"
            Tags { "LightMode" = "SRPDefaultUnlit" }
            Blend One One
            ZWrite Off
            Cull Back

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            CBUFFER_START(UnityPerMaterial)
                half4 _GlowColor;
                half _RimPower;
                half _Intensity;
                half _PulseSpeed;
                half _Inflate;
            CBUFFER_END

            struct Attributes { float4 positionOS : POSITION; float3 normalOS : NORMAL; };
            struct Varyings { float4 positionCS : SV_POSITION; float3 normalWS : TEXCOORD0; float3 viewWS : TEXCOORD1; };

            Varyings vert(Attributes i)
            {
                Varyings o;
                float3 posWS = TransformObjectToWorld(i.positionOS.xyz + i.normalOS * _Inflate);
                o.positionCS = TransformWorldToHClip(posWS);
                o.normalWS = TransformObjectToWorldNormal(i.normalOS);
                o.viewWS = GetWorldSpaceViewDir(posWS);
                return o;
            }

            half4 frag(Varyings i) : SV_Target
            {
                float3 n = normalize(i.normalWS);
                float3 v = normalize(i.viewWS);
                half rim = pow(1 - saturate(dot(n, v)), _RimPower);
                half pulse = 0.65 + 0.35 * sin(_Time.y * _PulseSpeed);
                return half4(_GlowColor.rgb * (rim * 1.5 + 0.12) * pulse * _Intensity, 1);
            }
            ENDHLSL
        }
    }
}
