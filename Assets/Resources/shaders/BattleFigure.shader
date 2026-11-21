Shader "XiuZhenJie/Battle Figure"
{
    Properties
    {
        [PerRendererData] _MainTex ("Figure", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        [HideInInspector] _RendererColor ("Renderer Color", Color) = (1,1,1,1)
        [HideInInspector] _Flip ("Flip", Vector) = (1,1,1,1)
        [PerRendererData] _AlphaTex ("External Alpha", 2D) = "white" {}
        [PerRendererData] _EnableExternalAlpha ("Enable External Alpha", Float) = 0
    }
    SubShader
    {
        Tags {"Queue"="Transparent" "RenderType"="Transparent" "IgnoreProjector"="True" "CanUseSpriteAtlas"="True"}
        Cull Off Lighting Off ZWrite Off Blend One OneMinusSrcAlpha
        Pass
        {
            CGPROGRAM
            #pragma vertex SpriteVert
            #pragma fragment FigureFrag
            #pragma target 3.0
            #pragma multi_compile _ ETC1_EXTERNAL_ALPHA
            #include "UnitySprites.cginc"
            float4 _MainTex_TexelSize;
            fixed4 FigureFrag(v2f i):SV_Target
            {
                fixed4 brush=SampleSpriteTexture(i.texcoord);
                float2 d=max(_MainTex_TexelSize.xy, fwidth(i.texcoord))*1.35;
                half lightSide=SampleSpriteTexture(i.texcoord+float2(d.x,-d.y)).a;
                half shadeSide=SampleSpriteTexture(i.texcoord+float2(-d.x,d.y)).a;
                half fringe=max(max(lightSide,shadeSide),max(SampleSpriteTexture(i.texcoord+d).a, SampleSpriteTexture(i.texcoord-d).a));
                half cover=smoothstep(.035,.97,brush.a);
                half border=(1.-cover)*smoothstep(.09,.68,fringe)*.86;
                fixed3 ink=lerp(fixed3(.10,.16,.16),fixed3(.61,.57,.39),max(0.,lightSide-shadeSide)*.45);
                fixed3 pigment=saturate((brush.rgb-.44)*1.10+.44);
                return fixed4((pigment*cover+ink*border)*i.color.rgb*i.color.a,(cover+border)*i.color.a);
            }
            ENDCG
        }
    }
}
