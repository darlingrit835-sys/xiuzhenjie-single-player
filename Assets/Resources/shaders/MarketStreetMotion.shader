Shader "XiuZhenJie/MarketStreetMotion"
{
    Properties
    {
        [PerRendererData] _MainTex ("Street painting", 2D) = "white" {}
        _SpriteUV ("Sprite UV bounds", Vector) = (0,0,1,1)
        _DesignToLocal ("Painting pixel to UI unit", Vector) = (1,1,0,0)
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
            #pragma target 3.0
            #pragma multi_compile_local _ UNITY_UI_CLIP_RECT
            #include "UnityCG.cginc"
            #include "UnityUI.cginc"

            sampler2D _MainTex;
            float4 _SpriteUV, _DesignToLocal, _ClipRect;
            float _MarketStreetClock;

            struct Varyings
            {
                float4 pos : SV_POSITION;
                float2 art : TEXCOORD0;
                half2 patch : TEXCOORD1;
                half4 state : TEXCOORD2; // effect, seed, opacity, age
                float2 uiPosition : TEXCOORD3;
            };

            Varyings vert(appdata_full v)
            {
                Varyings o;
                float kind = floor(v.texcoord.x * .125);
                float cornerUV = v.texcoord.x - kind * 8;
                float corner = floor(cornerUV * .5);
                float variant = floor(v.texcoord.y * .5);
                float seed = variant / 31;
                float2 localUV = float2(fmod(corner, 2), floor(corner * .5));
                float t = _MarketStreetClock;
                float age = 0;
                float opacity = v.color.a;
                o.art = float2(cornerUV - corner * 2, v.texcoord.y - variant * 2);
                o.patch = localUV;

                if (kind >= 5)
                {
                    float2 travel;
                    float angle = seed * 6.283;
                    if (kind < 5.5) // Sparks rise from the mouth of the forge.
                    {
                        age = frac(t / (1.15 + .95 * seed) + seed);
                        travel = float2(sin(age * 6 + angle) * 14 + (seed - .5) * 28,
                            7 + age * (48 + seed * 65));
                    }
                    else if (kind < 6.5) // Pinpricks orbit the cluster of spirit crystals.
                    {
                        age = frac(t / (2.5 + 1.8 * seed) + seed);
                        travel = float2(cos(age * 5 + angle) * (16 + 14 * seed), age * 56);
                    }
                    else if (kind < 7.5) // Incense and forge smoke drift with the street breeze.
                    {
                        age = frac(t / (2.8 + seed * 2) + seed);
                        travel = float2(age * 22 + sin(age * 6.4 + angle) * age * 13, age * 78);
                        travel.x += (localUV.x - .5) * age * 14;
                    }
                    else // Fallen petals slip sideways and turn as they descend.
                    {
                        age = frac(t / (3.8 + 1.8 * seed) + seed);
                        travel = float2(age * (65 + 26 * seed) + sin(age * 7.4 + angle) * 16,
                            -age * (67 + seed * 41) + sin(age * 9 + angle) * 5);
                        travel.y += (localUV.x - .5) * sin(age * 12 + angle) * 8;
                    }
                    v.vertex.xy += travel * _DesignToLocal.xy;
                    opacity *= smoothstep(0, .13, age) * (1 - smoothstep(.63, 1, age));
                }
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uiPosition = v.vertex.xy;
                o.state = half4(kind, seed, opacity, age);
                return o;
            }

            half4 paint(float2 art)
            {
                return tex2D(_MainTex, lerp(_SpriteUV.xy, _SpriteUV.zw, art));
            }

            half4 frag(Varyings i) : SV_Target
            {
                float kind = i.state.x;
                float phase = i.state.y * 6.283;
                float t = _MarketStreetClock;
                float2 p = i.patch * 2 - 1;
                float radius = length(p);
                half4 result;
                if (kind < .5) // The upper seam is pinned. Only the loose lower edge waves.
                {
                    float loose = smoothstep(.12, .90, i.patch.y);
                    float wave = sin(t * 2.7 + i.patch.x * 3.5 + phase);
                    float slow = sin(t * 1.43 + phase);
                    float2 shift = loose * float2(5.2 * slow + 2.3 * wave, 3.5 * wave);
                    result = paint(i.art + shift / float2(1024, 1536));
                    result.a *= 1 - smoothstep(.70, 1, max(abs(p.x), abs(p.y)));
                }
                else if (kind < 1.5)
                {
                    float core = 1 - smoothstep(.2, 1, radius);
                    float flicker = .5 + .3 * sin(t * 7.2 + p.x * 5) + .2 * sin(t * 13.1 - p.x * 7);
                    result = paint(i.art + core * float2(sin(4 * t + 4 * p.y) * .0026, flicker * .0035));
                    half heat = saturate((result.r - result.b) * 1.9);
                    result.rgb += heat * core * flicker * half3(.35, .18, .035);
                    result.a *= core;
                }
                else if (kind < 3.5) // A warm lantern or a violet crystal lights its surroundings.
                {
                    float breath = .55 + .32 * sin(t * 2.55 + phase) + .13 * sin(t * 4.6 + phase);
                    float falloff = saturate(1 - radius);
                    half3 light = kind < 2.5 ? half3(1, .60, .16) : half3(.90, .60, 1);
                    float core = pow(falloff, 3.5);
                    result.rgb = lerp(light, half3(1, .94, .77), core);
                    result.a = falloff * falloff * (.18 + .68 * breath) + .26 * core;
                }
                else if (kind < 4.5) // A travelling highlight follows the engraved bronze dial.
                {
                    float angle = atan2(p.y, p.x);
                    float beam = saturate(cos(angle - t * 1.65));
                    float rim = 1 - smoothstep(.022, .085, abs(radius - .85));
                    float inner = 1 - smoothstep(.018, .061, abs(radius - .56));
                    result = half4(.72, 1, .89, (rim + inner * .6) * pow(beam, 7) * .89);
                }
                else if (kind < 5.5)
                {
                    float core = saturate(1 - radius);
                    result = half4(1, .50 + .46 * core, .16 + .52 * core,
                        (1 - smoothstep(.25, .98, radius)) * .94);
                }
                else if (kind < 6.5)
                {
                    float star = saturate(1 - min(abs(p.x) * 3.4 + abs(p.y), abs(p.y) * 3.4 + abs(p.x)));
                    result = half4(.95, .81, 1, star * (.72 + .28 * sin(t * 6 + phase)));
                }
                else if (kind < 7.5)
                {
                    float bend = sin(i.patch.y * 6.8 + i.state.w * 6 + phase) * .28;
                    float thread = 1 - smoothstep(.05, .55, abs(p.x - bend));
                    result = half4(.91, .87, .79, thread * (1 - p.y * p.y) * .36);
                }
                else
                {
                    float fold = .60 + .40 * abs(cos(t * 2.1 + phase));
                    float petal = length(float2(p.x, p.y / fold + p.x * .24));
                    result = half4(1, .58 + .25 * fold, .66 + .15 * fold, 1 - smoothstep(.55, 1, petal));
                }

                result.a *= i.state.z;
                #ifdef UNITY_UI_CLIP_RECT
                result.a *= UnityGet2DClipping(i.uiPosition, _ClipRect);
                #endif
                return result;
            }
            ENDCG
        }
    }
}
