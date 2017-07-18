// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

Shader "Unlit/ThermalShader"
{
	Properties
	{
		_ThermalColorLUT ("Thermal Color Look-up texture", 2D) = "white" {}
	}
	SubShader
	{
		Tags { "ThermalType" = "Human" }
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			#include "ThermalShaderCore.cginc"

			float _ThermalPowExponentHuman;
			float _ThermalMaxHuman;
			
			fixed4 frag (v2f i) : SV_Target
			{
				return thermal_frag(i,_ThermalPowExponentHuman,_ThermalMaxHuman);
			}
			ENDCG
		}

	}

	SubShader
	{
		Tags { "ThermalType" = "Environment" }
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			#include "ThermalShaderCore.cginc"

			float _ThermalPowExponentEnvironment;
			float _ThermalMaxEnvironment;
			
			fixed4 frag (v2f i) : SV_Target
			{
				return thermal_frag(i,_ThermalPowExponentEnvironment,_ThermalMaxEnvironment);
			}
			ENDCG
		}

	}

	SubShader
	{
		Tags { "ThermalType" = "Alien" }
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			#include "ThermalShaderCore.cginc"

			float _ThermalPowExponentAlien;
			float _ThermalMaxAlien;
			
			fixed4 frag (v2f i) : SV_Target
			{
				return thermal_frag(i,_ThermalPowExponentAlien,_ThermalMaxAlien);
			}
			ENDCG
		}

	}

}
