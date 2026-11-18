Shader "Custom/CubeGuideMask"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        _StencilComp ("Stencil Comparison", Float) = 8
        _Stencil ("Stencil ID", Float) = 0
        _StencilOp ("Stencil Operation", Float) = 0
        _StencilWriteMask ("Stencil Write Mask", Float) = 255
        _StencilReadMask ("Stencil Read Mask", Float) = 255
        _ColorMask ("Color Mask", Float) = 15
        [Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip ("Use Alpha Clip", Float) = 0
        _Center ("Center", Vector) = (0,0,0,0)
        _SliderX ("Hole Half Width", Range(0, 1000)) = 1000
        _SliderY ("Hole Half Height", Range(0, 1000)) = 1000
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "CanUseSpriteAtlas"="True" }
        Pass
        {
            Name "GUIDE_MASK"
            Stencil
            {
                Ref [_Stencil]
                Comp [_StencilComp]
                Pass [_StencilOp]
                ReadMask [_StencilReadMask]
                WriteMask [_StencilWriteMask]
            }
            Cull Off
            ZWrite Off
            ZTest [unity_GUIZTestMode]
            Blend SrcAlpha OneMinusSrcAlpha
            ColorMask [_ColorMask]

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_local _ UNITY_UI_CLIP_RECT
            #pragma multi_compile_local _ UNITY_UI_ALPHACLIP
            #include "UnityCG.cginc"
            #include "UnityUI.cginc"

            struct VertexOutput
            {
                float4 position : SV_POSITION;
                float2 uv : TEXCOORD0;
                float2 canvasPosition : TEXCOORD1;
                fixed4 color : COLOR;
            };

            sampler2D _MainTex;
            fixed4 _Color;
            fixed4 _TextureSampleAdd;
            float4 _ClipRect;
            float4 _Center;
            float _SliderX;
            float _SliderY;

            VertexOutput vert(appdata_full input)
            {
                VertexOutput output;
                output.position = UnityObjectToClipPos(input.vertex);
                output.uv = input.texcoord.xy;
                output.canvasPosition = input.vertex.xy;
                output.color = input.color * _Color;
                return output;
            }

            fixed4 frag(VertexOutput input) : SV_Target
            {
                fixed4 color = (tex2D(_MainTex, input.uv) + _TextureSampleAdd) * input.color;
                float2 distanceFromCenter = abs(input.canvasPosition - _Center.xy);
                float2 outsideHole = step(float2(_SliderX, _SliderY), distanceFromCenter);
                color.a *= max(outsideHole.x, outsideHole.y);

                #ifdef UNITY_UI_CLIP_RECT
                color.a *= UnityGet2DClipping(input.canvasPosition, _ClipRect);
                #endif
                #ifdef UNITY_UI_ALPHACLIP
                clip(color.a - 0.001);
                #endif
                return color;
            }
            ENDCG
        }
    }
}
