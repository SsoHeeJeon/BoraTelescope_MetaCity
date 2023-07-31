Shader "NNCam/Composition"
{
    Properties
    {
        _BackgroundTex("Background", 2D) = ""{}
        _LightEffect("Lightvalue", Range(0, 1)) = 0.1
        _TotalEffect("Totalvalue",Float) = 0
        [HideInInspector] _CameraTex("Camera", 2D) = ""{}
        [HideInInspector] _MaskTex("Mask", 2D) = ""{}
        [HideInInspector] _Threshold("Threshold", Range(0, 1)) = 0.5
        _CameraTex_TexelSize("TexelSize", Float) =0.5
    }

    CGINCLUDE

    #include "UnityCG.cginc"

    sampler2D _BackgroundTex;
    sampler2D _CameraTex;
    sampler2D _MaskTex;
    float4 _MaskTex_TexelSize;
    float4 _CameraTex_TexelSize;
    float _Threshold;
    float result;
    float _LightEffect;
    float _TotalEffect;

    void Vertex(float4 position : POSITION,
                float2 uv : TEXCOORD0,
                out float4 outPosition : SV_Position,
                out float2 outUV : TEXCOORD0)
    {
        outPosition = UnityObjectToClipPos(position);
        outUV = uv;
    }

    float4 Fragment(float4 position : SV_Position,
                    float2 uv : TEXCOORD0) : SV_Target
    {
        float3 fg = tex2D(_CameraTex, uv).rgb + (tex2D(_CameraTex, uv).rgb - tex2D(_CameraTex, uv - _MaskTex_TexelSize * 0.02).rgb + float3(_LightEffect,_LightEffect,_LightEffect)) * _TotalEffect;
        float3 bg = tex2D(_BackgroundTex, uv).rgb;
        // Hack: Slide UVs to fit the mask to the camera texture.
        float2 mask_uv = uv - _MaskTex_TexelSize * float2(-0.5 , 0.17);
        // Sample the mask texture and un-normalize the value.

        float mask = (tex2D(_MaskTex, mask_uv).r - 0.5) * 1000;
        mask /= 0.1;

        /*float mask = (tex2D(_MaskTex, mask_uv).r - 0.5) * 32;
        mask /= 2;*/

        /*float mask = (tex2D(_MaskTex, mask_uv).r - 0.5) * 128;
        mask/=1.5;*/
        // Apply a sigmoid activator to the mask value.
        mask = 1 / (1 + exp(-mask));
        float th1 = max(0, _Threshold - 1);
        float th2 = min(1, _Threshold + 1);
        return float4(lerp(float3(0.1607843, 0.9921569, 0), fg, smoothstep(th1, th2, mask)), 0.1);
    }

    ENDCG

    SubShader
    {
        Pass
        {
            CGPROGRAM
            #pragma vertex Vertex
            #pragma fragment Fragment
            #pragma result Fragment
            ENDCG
        }
    }
}
