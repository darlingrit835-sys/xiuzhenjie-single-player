Shader "XiuZhenJie/Meridian Veil"
{
    Properties
    {
        [PerRendererData] _MainTex ("Spirit", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        [HideInInspector] _RendererColor ("Renderer Color", Color) = (1,1,1,1)
        [HideInInspector] _Flip ("Flip", Vector) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" "IgnoreProjector"="True" }
        Cull Off Lighting Off ZWrite Off Blend One OneMinusSrcAlpha
        Pass
        {
            CGPROGRAM
            #pragma vertex SpriteVert
            #pragma fragment Fragment
            #include "UnitySprites.cginc"
            fixed4 Fragment(v2f i) : SV_Target
            {
                float2 p = i.texcoord - .5;
                float edge = 1.0 - smoothstep(.39, .506, max(abs(p.x), abs(p.y)));
                float aura = 1.0 - smoothstep(.39, .65, length(p));
                fixed4 c = SampleSpriteTexture(i.texcoord) * i.color;
                c.a *= edge * aura;
                c.rgb *= c.a;
                return c;
            }
            ENDCG
        }
    }
}
