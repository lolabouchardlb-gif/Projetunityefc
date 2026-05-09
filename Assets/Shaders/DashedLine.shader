Shader "Custom/URP/DashedLine"
{
    Properties
    {
        _Color("Color", Color) = (1,1,1,1)
        _DashSize("Dash Size", Float) = 0.5
        _GapSize("Gap Size", Float) = 0.3
        _Speed("Scroll Speed", Float) = 0
    }

    SubShader
    {
        Tags 
        { 
            "RenderType"="Transparent"
            "Queue"="Transparent"
        }

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            Cull Off

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            CBUFFER_START(UnityPerMaterial)
                float4 _Color;
                float _DashSize;
                float _GapSize;
                float _Speed;
            CBUFFER_END

            Varyings vert (Attributes v)
            {
                Varyings o;
                o.positionHCS = TransformObjectToHClip(v.positionOS.xyz);
                o.uv = v.uv;
                return o;
            }

            half4 frag (Varyings i) : SV_Target
            {
                // Animation du défilement
                float uv = i.uv.x + _Time.y * _Speed;

                float pattern = _DashSize + _GapSize;
                float pos = fmod(uv, pattern);

                // masque dash / gap
                float alpha = step(pos, _DashSize);

                return float4(_Color.rgb, _Color.a * alpha);
            }
            ENDHLSL
        }
    }
}