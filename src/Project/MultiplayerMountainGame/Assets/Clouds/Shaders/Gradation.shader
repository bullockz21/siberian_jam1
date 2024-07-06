Shader "Hidden/TLab/Skybox/Raymarching"
{
    Properties
    {
        _ColorUpper("Color Upper", Color) = (0.0, 0.0, 0.0)
        _ColorLower("Color Lower", Color) = (1.0, 1.0, 1.0)
    }
        SubShader
    {
        Tags { "Renderpipeline" = "UniversalPipeline" }

        Cull Off ZWrite Off

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                half4 vertex : POSITION;
                half3 uv : TEXCOORD0;
            };

            struct Varyings
            {
                half3 uv : TEXCOORD0;
                half4 vertex : SV_POSITION;
            };

            half3 _ColorUpper;
            half3 _ColorLower;

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.vertex = TransformObjectToHClip(IN.vertex.xyz);
                OUT.uv = IN.uv;
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                half4 bgColor = half4(lerp(_ColorLower, _ColorUpper, IN.uv.y * 0.5 + 0.5), 1.0);

                return bgColor;
            }
            ENDHLSL
        }
    }
}
