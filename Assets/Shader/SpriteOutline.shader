Shader "Unlit/SpriteOutline"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_EnableOutline ("Outline", Float) = 0
	}
	SubShader
	{
		Tags
		{
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
			"PreviewType" = "Plane"
			"CanUseSpriteAtlas" = "True"
		}
		LOD 100
		Cull Off
		Lighting Off
		ZWrite Off
		Blend One OneMinusSrcAlpha
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float4 _MainTex_TexelSize;
			float _EnableOutline;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// holy **** what a waste of shader ops :D

				fixed4 col = tex2D(_MainTex, i.uv);
				if (col.a == 0) discard;

				if (_EnableOutline > 0)
				{
					float a = 1;
					for (int x = -3; x <= 3; ++x)
					{
						for (int y = -3; y <= 3; ++y)
						{
							a *= tex2D(_MainTex, i.uv + fixed2(_MainTex_TexelSize.x * x, _MainTex_TexelSize.y * y)).a;
							if (a == 0)
							{
								col.rgba =
									fixed4(1, 1, 1, 1) * fixed4(1, 0, 0.5, 1);
								break;
							}
						}
					}
				}
				col.rgb *= col.a;

				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}
			ENDCG
		}
	}
}
