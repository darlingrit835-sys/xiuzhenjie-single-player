Shader "Custom/Painting"
{
    Properties
    {
        _MainTex ("Brush Texture", 2D) = "white" {}
        _Color ("Brush Color", Color) = (1,1,1,1)
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        Pass
        {
            Cull Off
            ZWrite Off
            ZTest Always
            Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha

            CGPROGRAM
            #pragma vertex vert_img
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _Color;

            fixed4 frag(v2f_img input) : SV_Target
            {
                return tex2D(_MainTex, TRANSFORM_TEX(input.uv, _MainTex)) * _Color;
            }
            ENDCG
        }
    }
}
