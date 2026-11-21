Shader "XiuZhenJie/HudGlint"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite", 2D) = "white" {}
        _SpriteUV ("Sprite UV bounds", Vector) = (0,0,1,1)
        _Sweep ("Sweep position", Float) = -1
        [HideInInspector] _StencilComp ("Stencil comparison", Float) = 8
        [HideInInspector] _Stencil ("Stencil ID", Float) = 0
        [HideInInspector] _StencilOp ("Stencil operation", Float) = 0
        [HideInInspector] _StencilWriteMask ("Stencil write mask", Float) = 255
        [HideInInspector] _StencilReadMask ("Stencil read mask", Float) = 255
        [HideInInspector] _ColorMask ("Color mask", Float) = 15
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" "CanUseSpriteAtlas"="True" }
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

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_local _ UNITY_UI_CLIP_RECT
            #include "UnityCG.cginc"
            #include "UnityUI.cginc"

            struct Varyings
            {
                float4 position : SV_POSITION;
                float2 uv : TEXCOORD0;
                float2 uiPosition : TEXCOORD1;
                half4 color : COLOR;
            };

            sampler2D _MainTex;
            float4 _SpriteUV, _ClipRect;
            float _Sweep;

            Varyings vert(appdata_full v)
            {
                Varyings o;
                o.position = UnityObjectToClipPos(v.vertex);
                o.uiPosition = v.vertex.xy;
                o.uv = v.texcoord.xy;
                o.color = v.color;
                return o;
            }

            half4 frag(Varyings i) : SV_Target
            {
                float2 uv = (i.uv - _SpriteUV.xy) / max(_SpriteUV.zw - _SpriteUV.xy, .00001);
                float diagonal = .77 * uv.x + .23 * (1.0 - uv.y);
                half band = 1.0 - smoothstep(.012, .17, abs(diagonal - _Sweep));
                half4 color = i.color;
                color.a *= band * tex2D(_MainTex, i.uv).a;
                #ifdef UNITY_UI_CLIP_RECT
                color.a *= UnityGet2DClipping(i.uiPosition, _ClipRect);
                #endif
                return color;
            }
            ENDCG
        }
    }
}
