Shader "Hidden/Sycoforge/Unlit/Mask Deferred"
{
    Properties
    {

    }

    SubShader
    {
		Tags { "Queue" = "Geometry+10"}
		ColorMask 0
		ZWrite On
		ZTest LEqual
		Offset -1,-1

		LOD 100

		Pass
		{
			Name "DEFERRED"
			Tags { "LightMode" = "Deferred" }

			CGPROGRAM
			// compile directives
			#pragma vertex vert_surf
			#pragma fragment frag_surf
			#pragma target 3.0
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2

			#include "HLSLSupport.cginc"
			#include "UnityShaderVariables.cginc"

			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"

			half _SSDMasking;

			uniform sampler2D _NormalBuffer;		  // RT2



			struct Input
			{
				float2 uv_MainTex;
			};

			// vertex-to-fragment interpolation data
			struct v2f_surf
			{
				float4 pos : SV_POSITION;
				float2 pack0 : TEXCOORD0; // _MainTex
				float4 screenPos : TEXCOORD1; // _MainTex
			};

			// vertex shader
			v2f_surf vert_surf(appdata_full v)
			{
				v2f_surf o;
				UNITY_INITIALIZE_OUTPUT(v2f_surf,o);
				float4 p = UnityObjectToClipPos(v.vertex);
				o.pos = p;				
				o.screenPos = ComputeScreenPos(p);
				return o;
			}

			fixed4 unity_Ambient;

			//out half4 outDiffuse		: SV_Target0,	// RT0: diffuse color (rgb), occlusion (a)
			//out half4 outSpecSmoothness : SV_Target1,	// RT1: spec color (rgb), smoothness (a)
			//out half4 outNormal : SV_Target2,	// RT2: normal (rgb), --unused, very low precision-- (a) 
			//out half4 outEmission : SV_Target3	// RT3: emission (rgb), --unused-- (a)

			// fragment shader
			void frag_surf(v2f_surf IN, 
				// Mappings and channel order
				out half4 outNormal			: SV_Target2	// RT2: normal (rgb), --unused, very low precision-- (a) 
			)
			{
				float2 uv = IN.screenPos.xy / IN.screenPos.w;

				// prepare and unpack data
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT(Input, surfIN);

				float4 normal = tex2D(_NormalBuffer, uv);

				normal.w = 0.6;
				outNormal = normal*8;
			}

			ENDCG

		}
    }
}
