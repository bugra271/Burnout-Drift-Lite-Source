// Easy Decal shader using Unity's standard pbs workflow. v1.0
Shader "Easy Decal/ED Standard (Metallic, Emission, Vertex Alpha, Cutoff)" 
{
	Properties 
	{
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_MetallicMap ("Metallic (R) Smoothness (A)", 2D) = "white" {}
		_BumpMap ("Normalmap", 2D) = "bump" {}
		_Occlusion ("Ambient Occlusion (R)", 2D) = "white" {}
		_Metallic ("Metallic Multiplier", Range(0,1)) = 1.0
		_Smoothness("Smoothness Multiplier", Range(0,1)) = 1.0
		_Cutoff ("Cutoff", Range(0.01,1)) = 0.1
		[HDR]_EmissionColor("Emission Color", Color) = (1,1,1,1)
		_EmissionMap("Emission (RGB)", 2D) = "black" {}
	}

	SubShader 
	{
		Tags 
		{ 
			"Queue"="AlphaTest" 
			"RenderType"="TransparentCutout" 
			"ForceNoShadowCasting" = "True"
		}
		LOD 200
		Offset -1,-1

		
		CGPROGRAM

		#pragma surface surf Standard alphatest:_Cutoff vertex:vert fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		#include "UnityPBSLighting.cginc"
		sampler2D _MainTex;
		sampler2D _MetallicMap;
		sampler2D _BumpMap;
		sampler2D _Occlusion;
		sampler2D _EmissionMap;

		half _Metallic;
		half _Smoothness;
		fixed4 _Color;
		fixed4 _EmissionColor;

		struct Input 
		{
			float2 uv_MainTex;
			float2 uv_BumpMap;
			float2 uv_EmissionMap;
			float4 color;
		};
 
		void vert (inout appdata_full v, out Input o)
		{
			UNITY_INITIALIZE_OUTPUT(Input, o);
			o.color = v.color; 
		}


		void surf (Input IN, inout SurfaceOutputStandard o) 
		{
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			fixed4 m = tex2D (_MetallicMap, IN.uv_MainTex);
			fixed4 n = tex2D (_BumpMap, IN.uv_BumpMap);
			fixed ao = tex2D(_Occlusion, IN.uv_BumpMap).r;
			fixed4 e = tex2D (_EmissionMap, IN.uv_EmissionMap);

			o.Albedo = c.rgb * IN.color.rgb * _Color;
			o.Metallic = m.r * _Metallic;
			o.Smoothness = m.a * _Smoothness;
			o.Normal = UnpackNormal (n);
			o.Occlusion = ao;
			o.Emission = e * _EmissionColor;
			o.Alpha = c.a * IN.color.a;
		}
		ENDCG
	}
	FallBack "Transparent"
}
