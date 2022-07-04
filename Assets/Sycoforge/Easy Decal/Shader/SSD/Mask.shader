// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Hidden/Sycoforge/Unlit/Mask"
{
	Properties
	{

	}
	SubShader
	{
		Tags { "Queue" = "Geometry"}
		ColorMask 0
		ZWrite On
		ZTest LEqual
		Offset -1,-1

		Stencil
		{
			Ref 1
			Comp always
			Pass replace
		}
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"


			struct appdata
			{
				float4 vertex : POSITION;
			};
			struct v2f
			{
				float4 pos : SV_POSITION;
			};
			v2f vert(appdata v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				return o;
			}
			half4 frag(v2f i) : SV_Target
			{
				return half4(1,1,0,1);
			}
			ENDCG
		}
	}
}
