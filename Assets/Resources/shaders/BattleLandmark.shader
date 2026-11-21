Shader "XiuZhenJie/Battle Landmark"
{
    Properties
    {
        [PerRendererData] _MainTex ("Landscape scroll", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        [HideInInspector] _RendererColor ("Renderer Color", Color) = (1,1,1,1)
        [HideInInspector] _Flip ("Flip", Vector) = (1,1,1,1)
        [PerRendererData] _AlphaTex ("External Alpha", 2D) = "white" {}
        [PerRendererData] _EnableExternalAlpha ("Enable External Alpha", Float) = 0
    }
    SubShader
    {
        Tags {"Queue"="Transparent" "RenderType"="Transparent" "CanUseSpriteAtlas"="False"}
        Cull Off Lighting Off ZWrite Off Blend One OneMinusSrcAlpha
        Pass
        {
            CGPROGRAM
            #pragma vertex SpriteVert
            #pragma fragment LandmarkFrag
            #pragma multi_compile _ ETC1_EXTERNAL_ALPHA
            #include "UnitySprites.cginc"
            fixed4 LandmarkFrag(v2f i) : SV_Target
            {
                fixed4 art = SampleSpriteTexture(i.texcoord) * i.color;
                float2 local = frac(i.texcoord * float2(3, 2));
                float2 edge = min(local, 1.0 - local);
                half border = smoothstep(.005, .085, min(edge.x, edge.y));
                art.a *= border;
                art.rgb *= art.a;
                return art;
            }
            ENDCG
        }
    }
}
