Shader "XiuZhenJie/Battle Terrain"
{
    Properties
    {
        _MainTex ("Cliff, water, woodland and path coverage", 2D) = "black" {}
        _Sites ("Quarry, camp and ruins", 2D) = "black" {}
        _Pigments ("Painted earth, woodland, slate and streambed", 2D) = "white" {}
        _Sunlight ("Province daylight", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags {"Queue"="Transparent" "RenderType"="Transparent" "IgnoreProjector"="True"}
        Cull Off ZWrite Off Lighting Off
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0
            #include "UnityCG.cginc"
            sampler2D _MainTex, _Sites, _Pigments;
            float4 _MainTex_TexelSize;
            fixed4 _Sunlight;
            struct v2f { float4 pos:SV_POSITION; float2 uv:TEXCOORD0; };
            v2f vert(appdata_base v)
            {
                v2f o;
                o.pos=UnityObjectToClipPos(v.vertex);
                o.uv=v.texcoord.xy;
                return o;
            }

            float hash(float2 p)
            {
                float3 n=frac(p.xyx*float3(.1031,.11369,.13787));
                n+=dot(n,n.yzx+19.19);
                return frac((n.x+n.y)*n.z);
            }

            float wash(float2 p)
            {
                float2 g=floor(p), f=frac(p);
                f=f*f*(3.-2.*f);
                return lerp(lerp(hash(g),hash(g+float2(1,0)),f.x),
                            lerp(hash(g+float2(0,1)),hash(g+1.),f.x),f.y);
            }

            fixed3 brush(float2 quadrant, float2 coords)
            {
                // Alternating the direction of a swatch keeps the painted edges continuous.
                float2 mirror=1.-abs(frac(coords*.5)*2.-1.);
                return tex2D(_Pigments,(quadrant+lerp(.012,.988,mirror))*.5).rgb;
            }

            float lightness(float3 c) { return dot(c,float3(.27,.57,.16)); }
            fixed4 frag(v2f i):SV_Target
            {
                float2 texel=_MainTex_TexelSize.xy;
                float2 p=i.uv*_MainTex_TexelSize.zw;
                float plume=wash(p*.29+2.7);
                float stroke=wash(p*1.19+1.1);
                float2 draft=float2(wash(p*.53+4.),wash(p*.49+13.))-.5;
                float2 flow=float2(wash(p*1.8),stroke)-.5;
                // Keep every cell centre faithful to movement while easing square corners into coves and spurs.
                float2 warp=.48*draft+.085*flow;
                float2 uv=i.uv+warp*texel;
                fixed4 field=tex2D(_MainTex,uv);
                fixed3 site=tex2D(_Sites,uv).rgb;
                float rim=field.r, wet=field.g;
                float tree=smoothstep(.12,.83,field.b+.11*(stroke-.5));
                float trail=smoothstep(.12,.85,field.a-.09*(stroke-.5));

                float2 leafUV=float2(p.x*.155+p.y*.055, p.y*.145-p.x*.041)+draft*.16;
                fixed3 moss=brush(float2(1,1),leafUV+.24);
                fixed3 ochre=brush(float2(0,1),float2(p.x*.147-p.y*.069,p.y*.126+p.x*.045)+draft*.18);
                fixed3 slate=brush(float2(0,0),float2(p.x*.138+p.y*.047,p.y*.159-p.x*.038)+draft*.12);
                fixed3 lake=brush(float2(1,0),float2(p.x*.145+p.y*.038,p.y*.135-p.x*.023)+flow*.025);
                // The low contrast clearing gives units and harvest nodes room to read.
                fixed3 loam=lerp(ochre,fixed3(.64,.59,.435),.23);
                fixed3 fern=lerp(moss,fixed3(.24,.40,.275),.16)*float3(.96,1.04,1.03);
                fixed3 floorInk=lerp(loam,fern,saturate(.33+tree*.73-trail*.29+.13*(plume-.5)));
                float gravel=smoothstep(.15,.77,site.r);
                float ruins=smoothstep(.25,.78,site.b);
                float camp=smoothstep(.20,.80,site.g);
                floorInk=lerp(floorInk,lerp(loam,slate,.57)*float3(1.07,.99,.88),gravel);
                floorInk=lerp(floorInk,lerp(slate,fixed3(.53,.56,.50),.35),ruins*.75);
                floorInk=lerp(floorInk,loam*float3(1.07,.98,.85),camp*.63);
                // Pools grade from a stony shallow shelf down into jade water, with a moving wave crest.
                float shore=smoothstep(.28,.48,wet);
                float depth=smoothstep(.48,.91,wet);
                fixed3 shingle=lerp(loam,slate,.64)*float3(.91,.97,.90);
                floorInk=lerp(floorInk,shingle,shore*(1.-depth));
                float wave=sin(wet*62.+stroke*7.-_Time.y*1.5);
                float foam=(1.-smoothstep(.016,.064,abs(wet-.52)))*
                           (.36+.64*smoothstep(-.37,.55,wave));
                fixed3 shallows=lerp(lake,fixed3(.26,.62,.56),.55);
                fixed3 pool=lerp(shallows,lerp(lake,fixed3(.04,.31,.34),.37),depth);
                pool+=smoothstep(.46,.80,lightness(lake))*sin(p.y*7.+stroke*5.-_Time.y*.72)*.035;
                floorInk=lerp(floorInk,pool,smoothstep(.40,.56,wet));
                floorInk=lerp(floorInk,fixed3(.77,.88,.76),foam*.68);
                // Offset the plateau top uphill. The exposed face has its own painted strata and a cast shadow.
                float ahead=tex2D(_MainTex,uv+float2(-.08,.23)*texel).r;
                float behind=tex2D(_MainTex,uv+float2(.10,-.36)*texel).r;
                float cliff=smoothstep(.42,.52,rim);
                float plateau=smoothstep(.48,.61,behind);
                float ledge=cliff*plateau;
                float face=cliff*(1.-plateau);
                float penumbra=smoothstep(.34,.65,ahead)*(1.-cliff);
                floorInk*=1.-penumbra*.34;
                float fissure=wash(float2(p.x*4.3+p.y*.65,p.y*1.72)+draft);
                fixed3 crag=lerp(slate,slate*float3(.63,.69,.76),.62);
                crag=lerp(crag,fern*.75,smoothstep(.57,.86,plume)*.34);
                fixed3 escarp=lerp(slate*float3(.48,.58,.70),fixed3(.14,.20,.235),.36);
                escarp*=.77+fissure*.40;
                floorInk=lerp(floorInk,escarp,face);
                floorInk=lerp(floorInk,crag,ledge);
                float crest=cliff*(1.-smoothstep(.022,.085,abs(behind-.56)))*(.48+.52*stroke);
                floorInk=lerp(floorInk,lerp(slate,fixed3(.65,.72,.61),.55),crest*.70);
                floorInk*=_Sunlight.rgb*(.94+.10*plume+.025*stroke);
                return fixed4(saturate(floorInk),1);
            }
            ENDCG
        }
    }
}
