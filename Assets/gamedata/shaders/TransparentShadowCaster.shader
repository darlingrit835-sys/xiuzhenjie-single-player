Shader "TransparentShadowCaster"
{
    Properties
    {
        _Color ("Main Color", Color) = (1,1,1,1)
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        Pass
        {
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            #pragma vertex vert_img
            #pragma fragment frag
            #include "UnityCG.cginc"

            fixed4 _Color;

            fixed4 frag(v2f_img input) : SV_Target
            {
                return _Color;
            }
            ENDCG
        }

        Pass
        {
            Tags { "LightMode"="ShadowCaster" }
            ZWrite On
            ZTest LEqual

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_shadowcaster
            #include "UnityCG.cginc"

            fixed4 _Color;

            struct VertexOutput
            {
                V2F_SHADOW_CASTER;
            };

            VertexOutput vert(appdata_base v)
            {
                VertexOutput output;
                TRANSFER_SHADOW_CASTER_NORMALOFFSET(output)
                return output;
            }

            float4 frag(VertexOutput input) : SV_Target
            {
                clip(_Color.a - 0.001);
                SHADOW_CASTER_FRAGMENT(input)
            }
            ENDCG
        }
    }
}
