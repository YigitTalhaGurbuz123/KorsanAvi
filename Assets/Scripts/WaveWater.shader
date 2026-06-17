Shader "Custom/WaveWater"
{
    Properties
    {
        _Color ("Water Color", Color) = (0,0.5,0.7,1)
        _Amplitude ("Wave Height", Float) = 0.3
        _Frequency ("Wave Frequency", Float) = 1.0
        _Speed ("Wave Speed", Float) = 1.0
    }
    SubShader
    {
        Tags 
        { 
            "RenderPipeline" = "UniversalPipeline"
            "Queue" = "Transparent"
            "RenderType" = "Transparent"
        }

        Pass
        {
            Tags { "LightMode" = "UniversalForward" }

            ZWrite Off
            ZTest LEqual
            Cull Back
            Blend SrcAlpha OneMinusSrcAlpha

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
            #pragma target 2.0

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            CBUFFER_START(UnityPerMaterial)
                float4 _Color;
                float _Amplitude;
                float _Frequency;
                float _Speed;
            CBUFFER_END

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                UNITY_SETUP_INSTANCE_ID(IN);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);

                float3 pos = IN.positionOS.xyz;

                float wave = sin((pos.x + pos.z) * _Frequency + _Time.y * _Speed) * _Amplitude;
                pos.y += wave;

                OUT.positionHCS = TransformObjectToHClip(float4(pos, 1.0));
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                return _Color;
            }

            ENDHLSL
        }
    }

    FallBack "Universal Render Pipeline/Lit"
}