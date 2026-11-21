Shader "XiuZhenJie/Battle Atlas"
{
    Properties
    {
        [PerRendererData] _MainTex ("Survey", 2D) = "black" {}
        _CloudInk ("Ink wash", 2D) = "gray" {}
        _Ground ("Ground type", 2D) = "gray" {}
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
        Cull Off ZWrite Off ZTest [unity_GUIZTestMode] Blend SrcAlpha OneMinusSrcAlpha
        Pass
        {
            CGPROGRAM
            #pragma target 3.0
            #pragma vertex vert_img
            #pragma fragment frag
            #include "UnityCG.cginc"
            sampler2D _MainTex, _CloudInk, _Ground;
            float4 _MainTex_TexelSize;
            half penLine(float2 p,float2 a,float2 b,half pen)
            {
                float2 v = b-a;
                float t = saturate(dot(p-a,v)/dot(v,v));
                return 1.-smoothstep(.018,.018+pen,length(p-a-t*v));
            }
            // Small cartographic marks remain legible as the chart changes size.
            half symbol(float2 p,int kind,half pen)
            {
                if (kind == 0 || kind == 3)
                {
                    half peak = penLine(p,float2(-.31,-.16),float2(-.03,.24),pen);
                    peak = max(peak,penLine(p,float2(-.03,.24),float2(.25,-.16),pen));
                    return max(peak,penLine(p,float2(.08,.07),float2(.17,.15),pen));
                }
                if (kind == 2)
                {
                    half pine = penLine(p,float2(-.22,-.10),float2(0,.23),pen);
                    pine = max(pine,penLine(p,float2(0,.23),float2(.22,-.10),pen));
                    return max(pine,penLine(p,float2(0,-.22),float2(0,.07),pen));
                }
                if (kind == 4)
                {
                    float wave=abs(p.y-.052*sin(p.x*10.5));
                    return (1.-smoothstep(.018,.018+pen,wave))*(1.-smoothstep(.23,.32,abs(p.x)));
                }
                if (kind == 5 || kind == 6)
                {
                    half roof = penLine(p,float2(-.27,.06),float2(0,.22),pen);
                    roof=max(roof,penLine(p,float2(0,.22),float2(.27,.06),pen));
                    roof=max(roof,penLine(p,float2(-.15,.04),float2(-.15,-.17),pen));
                    return max(roof,penLine(p,float2(.15,.04),float2(.15,-.17),pen));
                }
                return 0;
            }
            fixed4 frag(v2f_img i) : SV_Target
            {
                float2 uv = i.uv;
                half3 wash = tex2D(_CloudInk,uv).rgb;
                half ridge = dot(wash,half3(.26,.57,.17));
                half grain = tex2D(_CloudInk,uv * .63 + .19).g;
                float2 wobble = float2(ridge - .35,grain - .35) * _MainTex_TexelSize.xy * .65;
                fixed4 field = tex2D(_MainTex,uv+wobble);
                half3 clouds = lerp(half3(.13,.21,.23), half3(.40,.50,.48),smoothstep(.08,.86,ridge));
                half3 land = lerp(half3(.27,.33,.32),field.rgb,.46 + .54*field.a);
                land *= .92 + grain*.20;
                half edge = smoothstep(.21,.40,field.a)*(1.-smoothstep(.40,.62,field.a));
                half3 chart = lerp(clouds,land,smoothstep(.16,.61,field.a));
                chart = lerp(chart,half3(.77,.72,.53),edge*.63);
                float2 tiles = uv * _MainTex_TexelSize.zw;
                float2 index = floor(tiles);
                int type = (int)round(tex2D(_Ground,uv).a * 255.);
                half pen = clamp(fwidth(tiles.x)*.62,.026,.10);
                if (field.a > .60 && fmod(index.x+index.y,2.) < .5)
                {
                    half mark = symbol(frac(tiles)-.5,type,pen);
                    chart = lerp(chart,half3(.19,.30,.29),mark*.55);
                }
                return fixed4(chart,1);
            }
            ENDCG
        }
    }
}
