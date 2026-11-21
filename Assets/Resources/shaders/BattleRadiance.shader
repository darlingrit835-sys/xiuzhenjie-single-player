Shader "XZJ/BattleRadiance"
{
	SubShader
	{
		Tags { "Queue"="Transparent" "RenderType"="Transparent" "IgnoreProjector"="True" }
		Cull Off Lighting Off ZWrite Off Blend SrcAlpha One
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			struct Input { float4 vertex : POSITION; fixed4 color : COLOR; float2 uv : TEXCOORD0; };
			struct Vary { float4 pos : SV_POSITION; fixed4 color : COLOR; float2 uv : TEXCOORD0; };
			Vary vert(Input v) { Vary o; o.pos=UnityObjectToClipPos(v.vertex); o.color=v.color; o.uv=v.uv; return o; }
			fixed4 frag(Vary i) : SV_Target
			{
				float2 p = i.uv * 2 - 1;
				float halo = saturate(1 - dot(p,p));
				float core = saturate(1 - 3.2 * length(p));
				float star = pow(saturate(1 - abs(p.x * p.y) * 23), 3) * halo;
				return fixed4(lerp(i.color.rgb, fixed3(1,.99,.87), core), i.color.a * (halo*halo*.42 + core*.6 + star*.55));
			}
			ENDCG
		}
	}
}
