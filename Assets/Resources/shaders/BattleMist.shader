Shader "XiuZhenJie/Battle Mist"
{
    Properties
    {
        _MainTex ("Sight history", 2D) = "black" {}
        _CloudInk ("Mountain cloud painting", 2D) = "gray" {}
        _BlendStart ("Transition time", Float) = 0
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" "IgnoreProjector"="True" }
        Cull Off ZWrite Off ZTest Always Lighting Off Blend SrcAlpha OneMinusSrcAlpha
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            sampler2D _MainTex, _CloudInk;
            float4 _MainTex_TexelSize;
            float _BlendStart;
            struct v2f { float4 pos:SV_POSITION; float2 uv:TEXCOORD0; float2 world:TEXCOORD1; };
            v2f vert(appdata_base v) { v2f o; o.pos=UnityObjectToClipPos(v.vertex); o.uv=v.texcoord.xy; o.world=mul(unity_ObjectToWorld,v.vertex).xy*.5; return o; }
            float hash(float2 p) { float3 a=frac(p.xyx*.1031); a+=dot(a,a.yzx+33.33); return frac((a.x+a.y)*a.z); }
            float wash(float2 p) {
                float2 i=floor(p), f=frac(p); f=f*f*(3.-2.*f);
                return lerp(lerp(hash(i),hash(i+float2(1,0)),f.x),lerp(hash(i+float2(0,1)),hash(i+1.),f.x),f.y);
            }
            fixed4 frag(v2f i):SV_Target
            {
                float2 p=i.world;
                float2 drift=float2(_Time.y*.055,-_Time.y*.026);
                float billow=wash(p*.67+drift)*.66+wash(p*1.91-drift*.7)*.34;
                float2 warp=float2(billow-.5,wash(p*.95+drift+12.)-.5)*.38;
                float2 mask=tex2D(_MainTex,i.uv+warp*_MainTex_TexelSize.xy).rg;
                float fog=lerp(mask.x,mask.y,saturate((_Time.y-_BlendStart)*4.5));
                clip(fog-.031);
                float alpha=smoothstep(.03,.95,fog);
                float rim=4.*alpha*(1.-alpha);
                // The same world coordinates on both planes continue the cloud painting beyond the map.
                float2 cloudUV=float2(p.x*.063+p.y*.021,p.y*.061-p.x*.014);
                cloudUV+=float2(_Time.y*.00075,-_Time.y*.00048)+warp*.026;
                cloudUV=1.-abs(frac(cloudUV*.5)*2.-1.);
                fixed3 vapor=tex2D(_CloudInk,cloudUV).rgb;
                float pigment=dot(vapor,float3(.22,.59,.19));
                fixed3 cloud=lerp(fixed3(.044,.087,.12),fixed3(.29,.43,.46),saturate(pigment*1.6));
                cloud+=vapor*.12+billow*.018;
                cloud+=fixed3(.045,.064,.06)*rim;
                return fixed4(cloud,alpha);
            }
            ENDCG
        }
    }
}
