Shader "XiuZhenJie/Cultivation Starfield"
{
    Properties
    {
        [PerRendererData] _MainTex ("Starfield", 2D) = "white" {}
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
                fixed4 c = SampleSpriteTexture(i.texcoord) * i.color;
                // Keep the cavern floor and side cliffs visible as the sky opens above the seated figure.
                float horizon = smoothstep(.18, .63, i.texcoord.y);
                float cliffs = smoothstep(.27, .5, abs(i.texcoord.x - .5));
                c.a *= lerp(.32, .99, horizon) - .18 * cliffs * (1.0 - horizon);
                c.rgb *= c.a;
                return c;
            }
            ENDCG
        }
    }
}
