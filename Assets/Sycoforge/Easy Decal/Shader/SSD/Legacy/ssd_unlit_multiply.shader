//-----------------------------------------------------
// Forward screen space decal multiply shader. Version 0.9
// Copyright (c) 2021 by Sycoforge
//-----------------------------------------------------
Shader "Easy Decal/SSD/Unlit Multiply SSD" 
{
	Properties 
	{
		_MainTex("Base (RGB)", 2D) = "white" {}
		_Tint("Tint (RGBA)", Color) = (1,1,1,1)
		_Threshold ("Clipping Threshold", Range(0,1)) = 0
	}
	SubShader 
	{
		Tags 
		{ 
			"RenderType"= "Transparent" 
			"Queue" = "Transparent+1" 
			// Temporarily disable batching -> TODO encoding transform to geometry channels
			"DisableBatching" = "True" 
		}

		Stencil
		{
			Ref 1
			Comp notequal
			Pass keep
		}

		ZWrite On 
		ZTest Always
		Lighting Off
		Cull Front
		Blend Zero SrcColor
		Offset -1,-1

		Pass
		{		
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment fragMultiply
			#pragma target 3.0
			#pragma multi_compile_fog

			#include "SycoFSSD.cginc"

			ENDCG
		}
	} 
	FallBack "Unlit/Transparent"
}

