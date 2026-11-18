Shader "XiuZhenJie/GuideFog"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _FocusRect ("Visible Rectangle Min/Max", Vector) = (0,0,0,0)
        _FocusMotion ("Reveal, Pulse, Sweep X/Y", Vector) = (1,0.5,1,0)
        _Drift ("Unscaled Mist Time", Float) = 0
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" "IgnoreProjector"="True" }
        Cull Off
        ZWrite Off
        ZTest [unity_GUIZTestMode]
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct VertexOutput
            {
                float4 position : SV_POSITION;
                float4 screenPosition : TEXCOORD0;
            };

            float4 _FocusRect;
            float4 _FocusMotion;
            float _Drift;

            VertexOutput vert(appdata_base input)
            {
                VertexOutput output;
                output.position = UnityObjectToClipPos(input.vertex);
                output.screenPosition = ComputeScreenPos(output.position);
                return output;
            }

            float Hash(float2 cell)
            {
                float3 seed = frac(cell.xyx * 0.1031);
                seed += dot(seed, seed.yzx + 33.33);
                return frac((seed.x + seed.y) * seed.z);
            }

            float Noise(float2 position)
            {
                float2 cell = floor(position);
                float2 weight = frac(position);
                weight = weight * weight * (3.0 - 2.0 * weight);
                float bottom = lerp(Hash(cell), Hash(cell + float2(1.0, 0.0)), weight.x);
                float top = lerp(Hash(cell + float2(0.0, 1.0)), Hash(cell + 1.0), weight.x);
                return lerp(bottom, top, weight.y);
            }

            fixed4 frag(VertexOutput input) : SV_Target
            {
                float2 screenUV = input.screenPosition.xy / input.screenPosition.w;
                float2 aspect = float2(_ScreenParams.x / _ScreenParams.y, 1.0);
                float2 view = (screenUV - 0.5) * aspect;
                float2 inkUV = view * float2(8.5, 5.0);
                float ink = 0.72 * Noise(inkUV + float2(_Drift * 0.07, 1.8))
                          + 0.28 * Noise(inkUV * 2.15 - float2(2.1, _Drift * 0.08));
                float vignette = smoothstep(0.18, 0.56, length(view));
                half3 inkColor = lerp(half3(0.031, 0.042, 0.052), half3(0.109, 0.133, 0.132), ink);
                half opacity = 0.58 + 0.17 * ink + 0.13 * vignette;

                float2 center = (_FocusRect.xy + _FocusRect.zw) * 0.5;
                float2 referencePixels = aspect * 900.0;
                float2 focusSize = (_FocusRect.zw - _FocusRect.xy) * referencePixels * 0.5;
                float2 local = (screenUV - center) * referencePixels;
                float2 halfSize = focusSize - (1.0 - _FocusMotion.x) * min(focusSize * 0.38, 18.0);
                float radius = min(min(halfSize.x, halfSize.y) * 0.30, 9.0);
                float2 edge = abs(local) - halfSize + radius;
                float distance = length(max(edge, 0.0)) + min(max(edge.x, edge.y), 0.0) - radius;
                float hasFocus = step(0.5, min(focusSize.x, focusSize.y));
                half fog = opacity * lerp(1.0, smoothstep(-0.2, 7.0, distance), hasFocus);

                float2 direction = local / max(length(local), 1.0);
                float sweep = pow(saturate(dot(direction, _FocusMotion.zw)), 12.0);
                float pulse = _FocusMotion.y;
                float rimCoverage = 1.0 - smoothstep(0.35, 1.25, abs(distance));
                float halo = smoothstep(-1.0, 0.6, distance) * (1.0 - smoothstep(1.0, 6.0 + 4.0 * pulse, distance));
                half glint = hasFocus * saturate(rimCoverage * (0.57 + 0.19 * pulse + 0.24 * sweep)
                    + halo * (0.05 + 0.15 * pulse + 0.26 * sweep));
                half alpha = glint + fog * (1.0 - glint);
                half3 highlight = lerp(half3(0.80, 0.66, 0.40), half3(1.0, 0.95, 0.79), 0.38 * pulse + 0.62 * sweep);
                return fixed4(lerp(inkColor, highlight, glint / max(alpha, 0.001)), alpha);
            }
            ENDCG
        }
    }
}
